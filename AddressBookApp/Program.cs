using Business_Layer.Service;
using Business_Layer.Interface;
using Repository_Layer.Service;
using Repository_Layer.Interface;
using Repository_Layer.Context;
using Repository_Layer.Entity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FluentValidation.AspNetCore;
using Business_Layer.Validation;
using FluentValidation;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped <AddressBookBL> ();
builder.Services.AddScoped<IAddressBookBL,AddressBookBL>();
builder.Services.AddScoped<IAddressBookRL,AddressBookRL>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();


builder.Services.AddValidatorsFromAssemblyContaining<AddressBookValidator>();
builder.Services.AddSingleton(new JwtService("ThisIsA32CharLongSecretKey!123456455465869708yyuvhhuvcgguugyft7uyfu78"));
builder.Services.AddSingleton<RedisCacheService>();




var connectionString = builder.Configuration.GetConnectionString("SqlConnection");

builder.Services.AddDbContext<AddressBookDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
