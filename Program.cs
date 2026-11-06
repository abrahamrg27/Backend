using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Backend.DTOs;
using Backend.Validators;
using backend.services;
using repository;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddSingleton<IPeopleService, PeopleService>();
builder.Services.AddKeyedSingleton<IPeopleService, PeopleService>("peopleService");
builder.Services.AddKeyedSingleton<IPeopleService, People2Service>("people2Service");

builder.Services.AddKeyedSingleton<IRandomService, RandomService>("randomSingleton");
builder.Services.AddKeyedScoped<IRandomService, RandomService>("randomScoped");
builder.Services.AddKeyedTransient<IRandomService, RandomService>("randomTransient");
builder.Services.AddScoped<IValidator<BeerInsertDto>, BeerInsertValidator>();
builder.Services.AddScoped<IValidator<BeerUpdateDto>, BeerUpdateValidator>();
builder.Services.AddScoped<IValidator<BrandInsertDto>, BrandInsertValidator>();
builder.Services.AddScoped<IValidator<BrandUpdateDto>, BrandUpdateValidator>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddHttpClient<IPostService, PostService>(c => 
c.BaseAddress = new Uri(builder.Configuration["BaseUrlPost"]));
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreConnection"));
});
builder.Services.AddKeyedScoped<ICommonService<BeerDto, BeerInsertDto, BeerUpdateDto>, BeerService>("beerService");
builder.Services.AddScoped<IRepository<Beer>, BeerRepository>();
builder.Services.AddKeyedScoped<ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto>, BrandService>("brandService");
builder.Services.AddKeyedScoped<IBrandService, BrandService>("brandService");
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IRepository<Brand>, BrandRepository>();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
