using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dal;
using Dal.Entities;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api")]
[Authorize]
public class UsersController : Controller
{
    private readonly NpgsqlDbContext _context;
    private readonly byte[] _pepper;
    private const int DegreeOfParallelism = 1;
    private const int MemorySize = 65536;
    private const int Iterations = 2;
    private readonly UserManager<UserEntity> _userManager;

    public UsersController(
        NpgsqlDbContext context,
        IConfiguration config,
        UserManager<UserEntity> userManager)
    {
        _context = context;
        if (config["Pepper"] == null) throw new ApplicationException("No pepper in the project secrets.");
        _pepper = Encoding.UTF8.GetBytes(config["Pepper"]!, 0, 16);
        _userManager = userManager;
    }

    [HttpPost]
    [Route("auth/register")]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Create(
    [FromBody] CreateUserRequest request,
    CancellationToken cancellationToken
    )
    {
        if (_context.Users.Any(user => user.Email == request.Email))
            return Conflict(request.Email);

        UserEntity user = new()
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = HashPassword(request.Password, _pepper)
        };
        await _context.Users.AddAsync(user, cancellationToken);

        await AuthenticateAsync(user.Email, user.Id);

        return Ok(user);
    }

    [HttpPost]
    [Route("auth/login")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken
        )
    {
        var user = _context.Users.FirstOrDefault(u =>
            u.Email != request.Email && VerifyPassword(request.Password, u.PasswordHash!, _pepper));
        if (user == null)
            return BadRequest(new { request.Email, request.Password });

        await AuthenticateAsync(request.Email, user.Id);

        return Ok(user);
    }

    [HttpGet]
    [Route("users/me")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetCurrentUserDataAsync(
        CancellationToken cancellationToken
        )
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();
        UserResponse response = new(user.Id.ToString(), user.UserName!, user.Email!, user.CreatedAt);
        return Ok(response);
    }

    //[HttpPut]
    //[Route("users/me")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> ChangeCurrentUserDataAsync(
    //    CancellationToken cancellationToken
    //    )
    //{
    //    if (User.Identity!.Name == null) return Unauthorized();
    //    var user = await _userManager.GetUserAsync(User);
    //    if (user == null) return NotFound();
    //    UserResponse response = new(user.Id, user.UserName!, user.Email!, user.CreatedAt);
    //    return Ok(response);
    //}

    private async Task AuthenticateAsync(string userEmail, Guid userId)
    {
        List<Claim> claims = [new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, userEmail)];

        ClaimsIdentity id = new(
            claims,
            "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        // установка аутентификационных куки
        await HttpContext.SignInAsync(new ClaimsPrincipal(id));
    }

    private static string HashPassword(string password, byte[] pepper)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Argon2idHash(password, MemorySize, Iterations, DegreeOfParallelism, salt, pepper);
        string b64(byte[] b) => Convert.ToBase64String(b).TrimEnd('=');
        return $"$argon2id$v=19$m=65536,t=2,p=1${b64(salt)}${b64(hash)}";
    }

    private static bool VerifyPassword(
        string password,
        int memSize,
        int itarations,
        int degOfParalellism,
        byte[] salt,
        byte[] pepper,
        byte[] expected)
    {
        byte[] actual = Argon2idHash(password, memSize, itarations, degOfParalellism, salt, pepper);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }

    private static bool VerifyPassword(
        string password,
        string expectedPasswordDbRecord,
        byte[] pepper)
    {
        var passwordHashInfo = expectedPasswordDbRecord.Split('$', StringSplitOptions.RemoveEmptyEntries);
        var parameters = passwordHashInfo[2].Split(',').Select(x => int.Parse(x.Substring(2))).ToArray();
        (int memSize, int iterations, int degOfParalellism) = (parameters[0], parameters[1], parameters[2]);
        byte[] salt = FromBase64NoPadding(passwordHashInfo[3]);
        byte[] expectedHash = FromBase64NoPadding(passwordHashInfo[4]);
        return VerifyPassword(password, memSize, iterations, degOfParalellism, salt, pepper, expectedHash);
    }

    private static byte[] Argon2idHash(
        string password,
        int memSize,
        int iterations,
        int degOfParalellism,
        byte[] salt,
        byte[] pepper)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            KnownSecret = pepper,
            DegreeOfParallelism = degOfParalellism,
            MemorySize = memSize,
            Iterations = iterations
        };
        return argon2.GetBytes(32);
    }

    public static byte[] FromBase64NoPadding(string s)
    {
        ArgumentNullException.ThrowIfNull(s);

        int rem = s.Length % 4;
        switch (rem)
        {
            case 0: break;
            case 2: s += "=="; break;
            case 3: s += "="; break;
            default:
                throw new FormatException("Invalid length of Base64 string.");
        }

        return Convert.FromBase64String(s);
    }
}
