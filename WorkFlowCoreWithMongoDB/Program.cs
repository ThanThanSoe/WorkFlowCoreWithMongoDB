using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WorkflowCore.Interface;
using WorkflowCore.Services;
using WorkflowCore.Persistence.MongoDB;
using WorkFlowCoreWithMongoDB.Models; // Ensure you include your models
using WorkFlowCoreWithMongoDB.Workflows; // Ensure you include your workflows

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(); // Register the MVC services for controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MongoDB settings to the service collection
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Register the MongoClient with options
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    if (string.IsNullOrEmpty(settings.ConnectionString))
    {
        throw new ArgumentNullException(nameof(settings.ConnectionString), "MongoDB connection string is missing.");
    }
    return new MongoClient(settings.ConnectionString);
});

// Register Workflow Core services
builder.Services.AddWorkflow(x =>
{
    var mongoSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    x.UseMongoDB(mongoSettings.ConnectionString, mongoSettings.DatabaseName);
   // x.AddWorkflow<DynamicWorkflow,WorkflowRequest>(); // Ensure you use AddWorkflow here
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use the WorkflowHost
var host = app.Services.GetRequiredService<IWorkflowHost>(); // Change to GetRequiredService
host.Start(); // Start the workflow host
host.RegisterWorkflow<DynamicWorkflow, WorkflowRequest>();
host.StartWorkflow("DynamicWorkflow");

app.MapControllers(); // Map the controllers to the routes

app.Run(); // Run the application
