using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectForAkelon.ExcelTask;
using ProjectForAkelon.ExcelTask.Services;

var host = Host.CreateDefaultBuilder().ConfigureServices(
    services =>
    {
        services.AddSingleton<IApplication, Application>();
        services.AddSingleton<IDataController, DataController>();
    }
).Build();

var app = host.Services.GetRequiredService<IApplication>();
app.Run();