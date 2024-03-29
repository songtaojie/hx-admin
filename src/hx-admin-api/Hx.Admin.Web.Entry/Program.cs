using Hx.Admin.Serilog.Filters;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Templates;
using System;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger(); 
try
{
   
    Log.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureHxWebApp();
    builder.Services.AddAdminCoreService(builder.Configuration);
    builder.Logging.ClearProviders();
    builder.Host.UseSerilogSetup();
    var app = builder.Build();
    app.UseAdminCoreApp(builder.Environment);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "程序因异常而停止");
}
finally
{
    // 确保在应用程序退出之前刷新和停止内部计时器/线程(避免Linux上的分段错误)
    Log.CloseAndFlush();
}