using CityInfo.API;
using CityInfo.API.DbContexts;
using CityInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

//Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders(); // builin logger
//builder.Logging.AddConsole(); // builin logger
builder.Host.UseSerilog();

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
builder.Services.AddSwaggerGen();
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

var app = builder.Build();

  

// Configure the HTTP request pipeline.

// exception handller middleware - only add when we are not in a development enviroment
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

// Enable Swagger for all environments
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware order
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// then we use endpoint. We configure these by calling into MapControllers which
// adds endpoints for controller actions without specifying routes

app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

//app.MapControllers();
 
app.Run();
