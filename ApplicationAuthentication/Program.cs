using ApplicationAuthentication.Data;
using ApplicationAuthentication.Data;
using ApplicationAuthentication.Repositories;
using ApplicationAuthentication.Repositories.Interfaces;
using ApplicationAuthentication.Services;
using ApplicationAuthentication.Services.Interfaces;
using ApplicationAuthentication.Authentication.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); // register the controller related services
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//repository registrations
builder.Services.AddScoped<DatabaseConnection>(); //register database connection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPasswordHashingService, PasswordHashingService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IApplicationValidationService, ApplicationValidationService>();
builder.Services.AddScoped<IUserAuthorizationProvider, UserAuthorizationProvider>();
builder.Services.AddScoped<IUserApplicationRepository, UserApplicationRepository>();
builder.Services.AddScoped<IUserApplicationAccessService, UserApplicationAccessService>();


//var passwordHasher = new PasswordHashingService();

//var passwordHash = passwordHasher.HashPassword("Test@12345");

//Console.WriteLine($"Generated Password Hash: {passwordHash}");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); //maps controller routes so  requests can reach our API endpoints

app.Run();
