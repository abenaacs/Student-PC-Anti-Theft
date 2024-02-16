using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Safe_Campus.Models;
using Safe_Campus.Services;
using SafeCampus.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{   //Adding authorization on the swagger
    option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    option.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication().AddJwtBearer(option =>
{
         // adding JWT authentication
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Token").Value!))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Student", policy =>
    {
        policy.RequireRole("Student");
    });
    options.AddPolicy("Guard", policy =>
    {
        policy.RequireRole("Guard");
    });
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireRole("Admin");
    });
});
      //configuring our database setting
builder.Services.Configure<CampusDatabaseSettings>(builder.Configuration.GetSection("CampusDatabaseSettings"));
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddSingleton<LaptopService>();
builder.Services.AddSingleton<ReportService>();
builder.Services.AddSingleton<TrackService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
