using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("WebChatConnection")));
builder.Services.AddScoped<IRepository<User>,Repository<User>>();
builder.Services.AddScoped<IRepository<Chat>,Repository<Chat>>();
builder.Services.AddScoped<IRepository<Message>,Repository<Message>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChatService,ChatService>();
builder.Services.AddScoped<IMessageService,MessageService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWTOptions"));
builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontOrigin",
            builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials(); 
            });
    });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseRouting();

app.UseCors("AllowFrontOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chathub");
app.MapControllers();

app.Run();
