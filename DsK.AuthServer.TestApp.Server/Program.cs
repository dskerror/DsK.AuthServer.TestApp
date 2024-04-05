using DsK.AuthServer.TestApp.Server.HttpClients;
using DsK.AuthServer.TestApp.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<AuthorizarionServerAPIHttpClient>();
builder.Services.AddHttpClient<AuthorizarionServerAPIHttpClient>("AuthorizarionServerAPI", c =>
{
    c.BaseAddress = new System.Uri("https://localhost:7045");
});

builder.Services.Configure<TokenSettingsModel>(builder.Configuration.GetSection("TokenSettings"));

var IssuerSigningKey = builder.Configuration.GetSection("TokenSettings").GetValue<string>("Key") ?? "";
if (IssuerSigningKey == "")
{
    return; //Exit app if IssuerSigningKey is not found
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration.GetSection("TokenSettings").GetValue<string>("Issuer"),
        ValidateIssuer = true,
        ValidAudience = builder.Configuration.GetSection("TokenSettings").GetValue<string>("Audience"),
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IssuerSigningKey)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
    };
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "myOrigins",
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
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
app.UseCors("myOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();
