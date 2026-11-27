namespace Api.Controllers;

public record CreateUserRequest(string UserName, string Email, string Password);