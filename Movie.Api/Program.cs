using Asp.Versioning;
using Microsoft.OpenApi.Models;
using Movie.Api.Auth;
using Movie.Api.Common.Extensions;
using Movie.Api.Common.MiddleWares;
using Movie.Api.Endpoints;
using Movie.Application;
using Movie.Application.Common;
using Movie.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

var settings = Settings.GetSettingsFromEnvironment();

builder.Services
    .AddApplication(settings)
    .AddInfrastructure(settings);

builder.Services.AddScoped<ICurrentUser>(opts =>
{
    var accessor = opts.GetRequiredService<IHttpContextAccessor>();
    if (accessor.HttpContext?.User.Identity != null && accessor.HttpContext != null &&
        accessor.HttpContext.User.Identity.IsAuthenticated)
    {
        return new CurrentUser
        {
            UserId = accessor.HttpContext.User.GetUserAuthenticationInfo().UserId,
        };
    }

    return null;
});

builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();

builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1.0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
    x.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
}).AddApiExplorer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((opts) =>
{
    opts.CustomSchemaIds(c => c.FullName.Replace('+', '.'));
    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie app", Version = "v1" });
    opts.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});


builder.Services.AddAuthorization();
builder.Services.AddBearerAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieApp"); });
}

app.UseCors();

app.UseAppHealthCheck();
app.UseAuthentication();
app.UseAuthorization();
app.UseGenericErrorHandling();
app.UsePreventClickJacking();
app.MapApiEndpoints();
app.Run();