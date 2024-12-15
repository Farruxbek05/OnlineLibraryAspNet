using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineLibraryAspNet.Class;
using System.Text;

namespace OnlineLibraryAspNet.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwaggerGenWithBearer(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Online Library v1", Version = "v1" });

            // Dokumenlar orasida filtr qo'yish
            options.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (docName == "v1" && !apiDesc.RelativePath.StartsWith("api/SuperAdmin"))
                    return true;

                if (docName == "superadmin" && apiDesc.RelativePath.StartsWith("api/SuperAdmin"))
                    return true;

                return false;
            });

            // JWT uchun xavfsizlik ta'rifi
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = @"JWT Authorization header using the Bearer scheme. <br />
                                Enter 'Bearer' [space] and then your token in the text input below. <br /><br />
                                Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            // Xavfsizlik talabi
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
            });
            options.CustomSchemaIds(type => type.FullName);
            options.EnableAnnotations(); // Annotations-ni yoqish
        });

    }
    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOption>(configuration.GetSection("Jwt"));

        var signingKeyString = configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(signingKeyString))
        {
            throw new ArgumentNullException(nameof(signingKeyString), "Signing key cannot be null or empty.");
        }

        var signingKey = Encoding.UTF8.GetBytes(signingKeyString);

        services.AddSingleton(_ => new SymmetricSecurityKey(signingKey));

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                ValidateIssuer = true,
                ValidateAudience = true,
                IssuerSigningKey = services.BuildServiceProvider().GetRequiredService<SymmetricSecurityKey>(),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }
    public static void AddIdentityService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwt(configuration);
        //services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<JwtOption>();
    }
}
