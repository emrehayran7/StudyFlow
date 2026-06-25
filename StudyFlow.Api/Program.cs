
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using StudyFlow.Api.src.Commands.AiRequest;
using StudyFlow.Api.src.Commands.Auth;
using StudyFlow.Api.src.Commands.Course;
using StudyFlow.Api.src.Commands.FlashCard;
using StudyFlow.Api.src.Commands.Note;
using StudyFlow.Api.src.Commands.StudySession;
using StudyFlow.Api.src.Commands.Topic;
using StudyFlow.Api.src.Middleware;
using StudyFlow.Api.src.Queries.AiRequest;
using StudyFlow.Api.src.Queries.Course;
using StudyFlow.Api.src.Queries.FlashCard;
using StudyFlow.Api.src.Queries.Note;
using StudyFlow.Api.src.Queries.StudySession;
using StudyFlow.Api.src.Queries.Topic;
using StudyFlow.Api.src.Services;
using StudyFlow.Core.Auth;
using StudyFlow.Core.Helper;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repository;
using StudyFlow.Infrastructure.Services;
using StudyFlow.Infrastructure.Services.Auth;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT access token."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document, null),
            new List<string>()
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>()!;
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,

        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddDbContext<StudyFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IStudyFlowRepository, StudyFlowRepository>();
builder.Services.AddSingleton<IAiService, OpenAiService>();
builder.Services.AddMemoryCache();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(StudyFlow.Core.Commands.Course.CreateCourse.CreateCourseCommandHandler).Assembly));

/*builder.Services.AddDbContext<StudyFlowDbContext>(options =>
     options.UseInMemoryDatabase("MyInMemoryDb"));*/
var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngular");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseStudyFlowRequestMiddleware();
app.UseAuthorization();

app.CreateAiRequest();
app.UpdateAiRequest();
app.DeleteAiRequest();
app.GetAiRequests();
app.GetAiRequestById();
app.CreateCourse();
app.Register();
app.Login();
app.RefreshToken();
app.Logout();
app.MapControllers();
app.GetCourse();
app.GetCourseById();
app.GetFlashCards();
app.GetFlashCardById();
app.CreateFlashCard();
app.GenerateFlashCardsWithAi();
app.SaveGeneratedFlashCards();
app.UpdateFlashCard();
app.DeleteFlashCard();
app.GetNotes();
app.GetNoteById();
app.CreateNote();
app.UpdateNote();
app.DeleteNote();
app.GetStudySessions();
app.GetStudySessionById();
app.CreateStudySession();   
app.UpdateStudySession();
app.DeleteStudySession();

app.UpdateCourse();
app.DeleteCourse();
app.CreateTopic();
app.GetTopics();
app.UpdateTopic();
app.DeleteTopic();
app.GetTopicById();
app.Run();
