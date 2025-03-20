using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class AuthExtension{

    public static IServiceCollection AddJwtAuth(this IServiceCollection collection, IConfiguration configuration){
        var authSettings = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
        collection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {

                options.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context => {
                        context.Token = context.Request.Cookies["JwtCookie"];

                        return Task.CompletedTask;
                    }    
                };
            });

        return collection;
    }
}