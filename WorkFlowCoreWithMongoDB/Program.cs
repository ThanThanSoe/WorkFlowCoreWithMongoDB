using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using WorkflowCore.Interface;
using WorkflowCore.Services;
using WorkFlowCoreWithMongoDB.Models;
using WorkFlowCoreWithMongoDB.Workflows;
using WorkFlowCoreWithMongoDB.Workflows.Steps;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient("mongodb://localhost:27017")); // Update with your MongoDB connection string

// Register Workflow Core services with MongoDB persistence
builder.Services.AddWorkflow(x =>
{
    var mongoSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    x.UseMongoDB(mongoSettings.ConnectionString, mongoSettings.DatabaseName);
});

// Add custom workflow
builder.Services.AddTransient<StepA>();
builder.Services.AddTransient<StepB>();
builder.Services.AddTransient<StepC>();
builder.Services.AddTransient<StepD>();

// Configure CORS to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors("AllowAll");

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Start the workflow host
var workflowHost = app.Services.GetRequiredService<IWorkflowHost>();
workflowHost.Start();
workflowHost.RegisterWorkflow<DynamicWorkflow, WorkflowRequest>();

app.Run();
