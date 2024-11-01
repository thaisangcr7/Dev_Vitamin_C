var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

  

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

    // Adding the routing middleware to the request pipeline, 
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
