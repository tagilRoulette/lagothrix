using Dal;
using Dal.Entities;
using Logic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Api.DtoHandlers;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddOptions();
        builder.Services
            .Configure<UploadConfig>(
                builder.Configuration.GetSection("FileSavePath")
                );
        builder.Services.Configure<IdentityOptions>(options => options.User.RequireUniqueEmail = true);

        builder.Services.AddDtoHandlers();
        builder.Services.AddLogic();
        builder.Services.AddDal(builder.Configuration.GetConnectionString("DefaultConnection"));
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .SetIsOriginAllowed(_ => true);
            });
        });

        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();
        builder.Services.AddAuthorization();
        builder.Services.AddDataProtection();

        var app = builder.Build();

        app.MapControllers();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Run();
    }
}
