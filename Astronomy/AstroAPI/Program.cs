using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.AstroAPI;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Usual stuff.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddSwaggerGen();

        // Add services to the container.
        builder.Services.AddSingleton<AstroDbContext>();
        builder.Services.AddSingleton<AstroObjectGroupRepository>();
        builder.Services.AddSingleton<AstroObjectRepository>();
        builder.Services.AddSingleton<PlanetService>();
        builder.Services.AddSingleton<EarthService>();
        builder.Services.AddSingleton<SunService>();
        builder.Services.AddSingleton<SeasonalMarkerService>();
        builder.Services.AddSingleton<LeapSecondRepository>();
        builder.Services.AddSingleton<LeapSecondService>();

        // Build.
        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production
            // scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    /// <summary>
    /// Return exception details in JSON format.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="ex"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static ObjectResult ReturnException(ControllerBase source, Exception ex, ILogger logger)
    {
        string errorMessage = ex.Message;
        if (ex.InnerException != null)
        {
            errorMessage += $" ({ex.InnerException.Message})";
        }

        logger.LogError("Error: {ErrorMessage}", errorMessage);

        return source.StatusCode(500, new
        {
            Error =
                "There was an error. It has been logged. Please email shaun@astromultimedia.com if you have questions. About this API, I mean. Not just whatever random stuff is on your mind.",
            Exception = ex.Message,
            InnerException = ex.InnerException?.Message
        });
    }
}
