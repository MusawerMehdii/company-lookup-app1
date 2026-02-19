using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
<<<<<<< HEAD
using CleanCompanyAPi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

=======

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Auth0 JWT Authentication
>>>>>>> 1bbcfe47eee900334074b6314ae64a563f03ca13
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
        options.Audience = builder.Configuration["Auth0:Audience"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
<<<<<<< HEAD
            ValidateAudience = false,
=======
            ValidateAudience = false, // M2M token – documented
>>>>>>> 1bbcfe47eee900334074b6314ae64a563f03ca13
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpClient();
<<<<<<< HEAD
builder.Services.AddScoped<ICompanyService, CompanyService>();

=======
>>>>>>> 1bbcfe47eee900334074b6314ae64a563f03ca13
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
