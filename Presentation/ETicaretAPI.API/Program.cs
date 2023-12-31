using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Persistence;
using ETicaretAPI.Infrastructure;
using FluentValidation.AspNetCore;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();


//builder.Services.AddStorage(StorageType.Azure);
builder.Services.AddStorage<LocalStorage>();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy=>
    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200","https://localhost:4200").AllowAnyHeader()     
));

builder.Services.AddControllers(options=> options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration =>configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options=>options.SuppressModelStateInvalidFilter=true);




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, //oluşturulacak token değerini kimlerin/hangi originlerin/sitelerin kullanacağını belirlediğimiz değer.
            ValidateIssuer = true, //oluşturulacak token değerini kimin dağıttığını ifade edeceğimiz alan.
            ValidateLifetime = true, //oluşturulan token değerinin süresini kontrol edecek olan doğrulamadır.
            ValidateIssuerSigningKey = true, //Üretilecek token değerinin uygulamamıza ait bir değer olduğunu ifade eden security key verisinin doğrulanmasıdır.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires> DateTime.UtcNow:false


        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
