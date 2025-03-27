using BronesWebAPI.WebApi.Repositories;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

builder.Services.AddTransient<IEnvironmentRepository, EnvironmentRepository>(provider => new EnvironmentRepository(sqlConnectionString));
builder.Services.AddTransient<IPatientInfoRepository, PatientInfoRepository>(provider => new PatientInfoRepository(sqlConnectionString));

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 10;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddRoles<IdentityRole>()
.AddDapperStores(options =>
{
    options.ConnectionString = sqlConnectionString;
});

var app = builder.Build();

var buildTimestamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
app.MapGet("/", () =>
    $"The API is up 🚀\n" +
    $"Connection string found: {(sqlConnectionStringFound ? "✅" : "❌")}\n" +
    $"Build timestamp: {buildTimestamp}");

app.UseAuthorization();
app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.MapControllers().RequireAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
