using Dal;
using Logic;
using Microsoft.Extensions.DependencyInjection;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddOptions();
        builder.Services
            .Configure<UploadConfig>(
                builder.Build().Configuration.GetSection("FileSavePath")
                );

        //builder.Services.AddLogic()
        builder.Services.AddDal(builder.Configuration.GetConnectionString("DefaultConnection"));
        builder.Services.AddControllers();

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

        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseDeveloperExceptionPage();
        //    app.UseSwagger();
        //    app.UseSwaggerUI();
        //}

        app.Run();
    }
}
