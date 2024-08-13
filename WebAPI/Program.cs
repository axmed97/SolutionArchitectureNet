using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolver.Autofac;
using Business.DependencyResolver.IoC;
using Core.DependencyResolver;
using Core.Utilities.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add business services and dependency resolvers
builder.Services.AddBusinessService(builder.Configuration);
builder.Services.AddDependencyResolvers(new[] { new CoreModule() });

// CORS configuration
#region CORS
builder.Services.AddCors(o => o.AddPolicy("Policy", policyBuilder =>
{
    policyBuilder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetIsOriginAllowed(origin => true); // Or specify allowed origins explicitly
}));
#endregion

// Configure Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(options =>
    {
        options.RegisterModule(new AutofacBusinessModule());
    });

// API Versioning
#region API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("Api-Version"),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        x.SwaggerDoc(description.GroupName, new OpenApiInfo
        {
            Title = $"Solution Architecture API {description.ApiVersion}",
            Version = description.ApiVersion.ToString()
        });
    }
    x.SwaggerDoc("v", new OpenApiInfo { Title = "SolutionArchitectureApp", Version = "v", Description = "Solution Architecture API swagger client." });
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Example: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\\"
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
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

// JWT Authentication
#region JWT Auth
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
            expires != null ? expires > DateTime.UtcNow : false,
        NameClaimType = ClaimTypes.Email
    };
});
#endregion

builder.Services.AddLogging();
// Serilog
#region Serilog
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});
#endregion
builder.Services.AddTransient<LocalizationMiddleware>();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
        }
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("Policy");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LocalizationMiddleware>();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();