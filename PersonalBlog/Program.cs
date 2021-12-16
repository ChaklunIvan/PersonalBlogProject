using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalBlog.Data;
using PersonalBlog.Data.Repositories;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Domain.Models;
using PersonalBlog.Security;
using PersonalBlog.Security.Jwt;
using PersonalBlog.Services;
using PersonalBlog.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Create middleware for all services
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();

var authenticationConfiguration = new AuthenticationConfiguration();
builder.Configuration.Bind("Authentication", authenticationConfiguration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>

o.TokenValidationParameters = new TokenValidationParameters()
{
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.RefreshTokenSecret)),
    ValidIssuer = authenticationConfiguration.Issuer,
    ValidAudience = authenticationConfiguration.Audience,
    ValidateIssuerSigningKey = true,
    ValidateIssuer = true,
    ValidateAudience = true,
    ClockSkew = TimeSpan.Zero
}
);

var identity = builder.Services.AddIdentityCore<User>();
var identityBuilder = new IdentityBuilder(identity.UserType, identity.Services);
identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>();
identityBuilder.AddUserManager<UserManager<User>>();
builder.Services.AddAuthentication();

builder.Services.AddSingleton(authenticationConfiguration);
builder.Services.AddSingleton<AccessTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenGenerator>();
builder.Services.AddSingleton<TokenGenerator>();
builder.Services.AddSingleton<RefreshTokenValidator>();
builder.Services.AddScoped<Authenticator>();
builder.Services.AddTransient<IGenericRepository<RefreshToken>, GenericRepository<RefreshToken>>();
builder.Services.AddTransient<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
