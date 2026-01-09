using Microsoft.EntityFrameworkCore;
using StudyFlow.src.Application;
using StudyFlow.src.Application.Abstractions;
using StudyFlow.src.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHealthService, HealthService>();
builder.Services.AddScoped<IDummyCourseService, DummyCourseService>();
builder.Services.AddScoped<IDummySessionService, DummySessionService>();

/*builder.Services.AddDbContext<StudyFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
*/
builder.Services.AddDbContext<StudyFlowDbContext>(options =>
     options.UseInMemoryDatabase("MyInMemoryDb"));
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
