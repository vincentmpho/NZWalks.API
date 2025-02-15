using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Profiles;
using NZWalks.API.Repositories;
using NZWalks.API.Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//inject Fluent Validation
builder.Services. AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Program>());

//inject/register connection string

var NZWalksDbConnectionString = builder.Configuration.GetConnectionString("NZWalksDbConnectionString");
builder.Services.AddDbContext<NZWalksDbContext>(options =>
{
    options.UseSqlServer(NZWalksDbConnectionString);
});

//inject/register Repository

builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IWalkRepository, WalkRepository>();
builder.Services.AddScoped<IWalkDifficultyRepository, WalkDifficultyRepository>();

//inject/registern Automapper

//Inject Automapper
builder.Services.AddAutoMapper(typeof(AutomapperProfile));

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
