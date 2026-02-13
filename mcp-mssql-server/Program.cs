using mcp_mssql_server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables() 
    .Build();
builder.Services.Configure<AppSettings>(configuration);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("myapp.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Services.AddSerilog();
var app = builder.Build();


await app.RunAsync();