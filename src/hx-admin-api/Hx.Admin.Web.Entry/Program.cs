using Serilog;
using Serilog.Events;
using System;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
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
    builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());
    var app = builder.Build();
    app.UseAdminCoreApp(builder.Environment);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "�������쳣��ֹͣ");
}
finally
{
    // ȷ����Ӧ�ó����˳�֮ǰˢ�º�ֹͣ�ڲ���ʱ��/�߳�(����Linux�ϵķֶδ���)
    Log.CloseAndFlush();
}