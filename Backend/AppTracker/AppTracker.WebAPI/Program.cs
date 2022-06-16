using AppTracker.DataAccessLayer.Contracts;
using AppTracker.DataAccessLayer.Implementations;
using AppTracker.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Configuration File to DI
builder.Services.Configure<BuildSettingsOptions>(
    builder.Configuration.GetSection(nameof(BuildSettingsOptions)));
builder.Services.AddScoped<IUserAccountDAO, UserAccountDAO>();

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