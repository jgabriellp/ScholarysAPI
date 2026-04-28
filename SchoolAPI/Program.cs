using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolAPI.Data;
using SchoolAPI.Repositories;
using SchoolAPI.Repositories.Interfaces;
using SchoolAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<DiarioService>();

builder.Services.AddScoped<IDesenvolvimentoMaternalRepository, DesenvolvimentoMaternalRepository>();
builder.Services.AddScoped<DesenvolvimentoMaternalService>();

builder.Services.AddScoped<INotaRepository, NotaRepository>();
builder.Services.AddScoped<NotaService>();

builder.Services.AddScoped<IFrequenciaRepository, FrequenciaRepository>();
builder.Services.AddScoped<FrequenciaService>();

builder.Services.AddScoped<ITurmaDisciplinaProfessorRepository, TurmaDisciplinaProfessorRepository>();
builder.Services.AddScoped<TurmaDisciplinaProfessorService>();

builder.Services.AddScoped<IDisciplinaRepository, DisciplinaRepository>();
builder.Services.AddScoped<DisciplinaService>();

builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<AlunoService>();

builder.Services.AddScoped<ITurmaRepository, TurmaRepository>();
builder.Services.AddScoped<TurmaService>();

builder.Services.AddScoped<IAnoLetivoRepository, AnoLetivoRepository>();
builder.Services.AddScoped<AnoLetivoService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT assim: Bearer {seu token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();