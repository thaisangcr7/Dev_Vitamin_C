using CityInfo.API;
using CityInfo.API.DbContexts;
using CityInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.ApplicationInsights.Extensibility;

//Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders(); // builin logger
//builder.Logging.AddConsole(); // builin logger

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Host.UseSerilog(
    (context, loggerConfiguration) => loggerConfiguration
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.ApplicationInsights(new TelemetryConfiguration
        {
            InstrumentationKey = builder.Configuration["ApplicationInsightsInstrumentationKey"]
        }, TelemetryConverter.Traces));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers( options => 
{
    options.ReturnHttpNotAcceptable = true;
})
    .AddNewtonsoftJson() 
    .AddXmlDataContractSerializerFormatters();

// register problem details services
builder.Services.AddProblemDetails();

//// Adding Problem details - Manipulate the error response
//builder.Services.AddProblemDetails(options =>
//{
//    options.CustomizeProblemDetails = ctx =>
//    {
//        ctx.ProblemDetails.Extensions.Add("additionInfo", "Additional info example ")
//        ctx.ProblemDetails.Extensions.Add("sever", Environment.MachineName);
//    };
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSingleton<FileExtensionContentTypeProvider>();



#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif
builder.Services.AddSingleton<CitiesDataStore>();

// the application will use this context - register for dependency injection
builder.Services.AddDbContext<CityInfoContext>(
    dbContextOptions=> dbContextOptions.UseSqlite(
        builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));

builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Authentication:Issuer"],
                ValidAudience = builder.Configuration["Authentication:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"]))
            };
        }
    );


//Add An Authorization Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeFromAntwerp", Policy =>
    {
        Policy.RequireAuthenticatedUser();
        Policy.RequireClaim("city", "Antwerp");
    });
});

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
}).AddMvc()
.AddApiExplorer(setupAction =>
{
    setupAction.SubstituteApiVersionInUrl = true;
});


var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider()
    .GetRequiredService<IApiVersionDescriptionProvider>();

builder.Services.AddSwaggerGen(setupAction =>
{
foreach (var description in
    apiVersionDescriptionProvider.ApiVersionDescriptions)
{
            setupAction.SwaggerDoc(
                $"{description.GroupName}",
            new()
        {
            Title = "City Info API",
            Version = description.ApiVersion.ToString(),
            Description = "Through this API you can access cities and their points of interest."
        });
    }

    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    setupAction.IncludeXmlComments(xmlCommentsFullPath);

    setupAction.AddSecurityDefinition("CityInfoAPIBearerAuth", new()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });


    setupAction.AddSecurityRequirement(new()
    {
        {
            new ()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "CityInfoAPIBearerAuth" 
                }
            },
            new List<string>()
        }
        
    });
}
    );

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor 
    | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

  

// Configure the HTTP request pipeline.

// exception handller middleware - only add when we are not in a development enviroment
if(!app.Environment.IsDevelopment())
{
 
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseForwardedHeaders();

// Enable Swagger for all environments
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            setupAction.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }

    }
    );
//}

// Middleware order
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// then we use endpoint. We configure these by calling into MapControllers which
// adds endpoints for controller actions without specifying routes

app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

//app.MapControllers();
 
app.Run();
