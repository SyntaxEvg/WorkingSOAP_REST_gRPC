using ClinicService.Data;
using ClinicService.Services;
using ClinicService.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServicesProtos.Services.Server;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var Services = builder.Services;

#region Configure EF DB Context Service (ClinicService Database)

Services.AddDbContext<ClinicServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
});

#endregion

Services.AddGrpc().AddJsonTranscoding();
Services.AddGrpcReflection();
Services.AddGrpcSwagger();
Services.AddGrpc();

Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                logging.RequestHeaders.Add("Authorization");
                logging.RequestHeaders.Add("X-Real-IP");
            });
Services.AddScoped<IPetRepository, PetRepository>();
Services.AddScoped<IConsultationRepository, ConsultationRepository>();
Services.AddScoped<IClientRepository, ClientRepository>();
Services.AddSingleton<IAuthenticateService, ClinicService.Services.Impl.AuthenticateService>();
Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Clinic servise",
        Version = "v2"
    });
    option.AddSecurityDefinition("Security",
        new OpenApiSecurityScheme()
        {
            Description = " Заголовок должен содержать Bearer XXXXXXX",
            Name = "Авторизация",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }, Array.Empty<string>()
                    }
                });
});

#region Conf Jwt
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new
        TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ClinicService.Services.Impl.AuthenticateService.SecretKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidIssuer = builder.Configuration["Settings:Security:Issuer"],
            ValidAudience = builder.Configuration["Settings:Security:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });


#endregion

Services.AddControllers();
Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGrpcService<ClinicServiceGrpc>();
app.MapGrpcService<ClinicServiceGrpc>();
app.MapGrpcReflectionService();
app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
});


app.UseAuthorization();

app.MapControllers();

app.Run();
