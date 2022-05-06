using ApiRestMaps;

// Initialization program

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);
app.Run();
