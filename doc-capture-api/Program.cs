using DocCapture.API;
using DocCapture.API.Services;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
var config = configuration.GetSection("Storage");
builder.Services.AddAzureClients(clientBuilder => { clientBuilder.AddBlobServiceClient(config["ConnectionString"]); });
builder.Services.AddConfigurations();
builder.Services.AddInfrastuctureServices(configuration);
builder.Services.AddServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });
  

app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

