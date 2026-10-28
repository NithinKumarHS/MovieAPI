using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Repositories;
using MovieAPI.Repositories.Interfaces;
using MovieAPI.Services;
using MovieAPI.Services.Interfaces;
using System.Text;
using System.Text.Json.Serialization;

namespace MovieAPI;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                new JsonStringEnumConverter());
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _configuration["JwtToken:ValidAudience"],
                    ValidIssuer = _configuration["JwtToken:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:Key"]))
                };
            });

        
        services.AddSingleton<Supabase.Client>(serviceProvider =>
        {
            var client = new Supabase.Client(
                _configuration["Supabase:Url"],
                _configuration["Supabase:Key"]
                );

            return client;
        });

        services.AddAutoMapper(typeof(Startup));

        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IActorService, ActorService>();

        services.AddScoped<IProducerRepository, ProducerRepository>();
        services.AddScoped<IProducerService, ProducerService>();

        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IReviewService, ReviewService>();

        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IGenreService, GenreService>();

        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IMovieService, MovieService>();

        services.AddScoped<IAuthRepository,  AuthRepository>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddSingleton<ISupabaseStorageService, SupabaseStorageService>();

        services.AddAuthorization();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<GlobalExceptionHandler>();

        app.UseSwagger();
        app.UseSwaggerUI();


        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
