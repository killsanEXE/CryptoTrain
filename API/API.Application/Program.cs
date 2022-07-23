using System.Text;
using API.Application.Data;
using API.Application.Entities;
using API.Application.Helpers;
using API.Application.Interfaces;
using API.Application.Middleware;
using API.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var tokenKey = env == "TESTING" ? "super_strong_token_key" : 
    builder.Configuration.GetValue<string>("TokenKey");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options => {
    options.AddPolicy(name: "_myAllowSpecificOrigins", builder => {
        builder
        .AllowAnyHeader()
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddDbContext<ApplicationContext>(options => 
{
    if(env != "TESTING")
    {
        string connStr = builder.Configuration.GetConnectionString("Default Connection");
        options.UseSqlite(connStr);
    }else{
        options.UseInMemoryDatabase("TestDB");
    }

});

builder.Services.AddIdentityCore<AppUser>(opt => 
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.SignIn.RequireConfirmedEmail = true;
    opt.Password.RequireDigit = false;
    opt.Password.RequiredLength = 4;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
})
    .AddRoles<AppRole>()
    .AddRoleManager<RoleManager<AppRole>>()
    .AddSignInManager<SignInManager<AppUser>>()
    .AddRoleValidator<RoleValidator<AppRole>>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,    
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context => 
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if(!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddScoped<IEmailService, EmailService>(s => 
{
    var emailSettings = builder.Configuration.GetSection("EmailSettings");
    return new EmailService(new EmailSettings() 
    {
        Email = emailSettings.GetSection("Email").Value,
        Password = emailSettings.GetSection("Password").Value,
    });
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddScoped<ITokenService, TokenService>(f => 
    new TokenService(tokenKey, f.GetRequiredService<UserManager<AppUser>>()));
builder.Services.AddScoped<IWrapper, Wrapper>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

if(env != "TESTING")
{
    builder.Services.AddHostedService<CryptoService>(f => 
        new CryptoService(builder.Configuration.GetSection("CryptoCompareToken").Value, 
        f.GetRequiredService<IServiceProvider>()));
}

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationContext>();
    if(env == "TESTING") await context.Database.EnsureDeletedAsync();
    else await context.Database.MigrateAsync();

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await Seed.SeedUsers(userManager, roleManager);
}

// app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("_myAllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();