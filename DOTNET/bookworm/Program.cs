using System.Text;
using Bookworm.Exception;
using Bookworm.Repository;
using Bookworm.Service;
using Bookworm.Service.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddDbContextPool<AppDbContext>((options) =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DbConnection"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.19-mysql")
    )
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder => builder
            .WithOrigins("http://localhost:5174", "http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddScoped<IGenreMasterService, GenreMasterServiceImpl>();
builder.Services.AddScoped<ILanguageMasterService, LanguageMasterServiceImpl>();
builder.Services.AddScoped<ICustomerMasterService, CustomerMasterServiceImpl>();
builder.Services.AddScoped<IJwtService, JwtServiceImpl>();
builder.Services.AddScoped<ICartService, CarServiceImpl>();
builder.Services.AddScoped<IProductMasterService, ProductMasterServiceImpl>();
builder.Services.AddScoped<IMyShelfService , MyShelfService>();
builder.Services.AddScoped<IInvoiceMasterService, InvoiceMasterService>();
builder.Services.AddScoped<IRoyalityCalculationService, RoyalityCalculationService>();

builder.Services.AddExceptionHandler<ApiExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler( _ => {});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigins");

app.MapControllers();

app.Run();
