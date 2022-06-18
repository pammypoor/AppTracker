using AppTracker.DataAccessLayer.Contracts;
using AppTracker.DataAccessLayer.Implementations;
using AppTracker.Managers.Contracts;
using AppTracker.Managers.Implementations;
using AppTracker.Models;
using AppTracker.Services.Contracts;
using AppTracker.Services.Implementations;
using AppTracker.WebAPI.Controllers.Contracts;
using AppTracker.WebAPI.Controllers.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add Configuration File to DI
builder.Services.Configure<BuildSettingsOptions>(
    builder.Configuration.GetSection(nameof(BuildSettingsOptions)));
//DAL
builder.Services.AddScoped<IUserAccountDAO, UserAccountDAO>();
//Services
builder.Services.AddScoped<ITrackerService, TrackerService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

//Managers
builder.Services.AddScoped<IRegistrationManager, RegistrationManager>();

//Controllers
builder.Services.AddScoped<IRegistrationController, RegistrationController>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins(
                                                "http://localhost:3000",
                                                "https://localhost:3000")
                                              .AllowAnyHeader()
                                              .AllowAnyMethod()
                                              .AllowCredentials();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

//app.UseTokenAuthentication();

app.MapControllers();

app.Run();