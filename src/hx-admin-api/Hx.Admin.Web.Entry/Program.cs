using AspNetCoreRateLimit;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System.Net.Mail;
using System.Net;
using Hx.Admin.Core;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger(); 

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllersWithViews()
        .ConfigureApiBehaviorOptions(options =>
        {

        });
    builder.Services.AddHxAdminOptions();
    builder.Services.AddCache(builder.Configuration);
    builder.Services.AddSqlSugar();
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());
    // ����Nginxת����ȡ�ͻ�����ʵIP
    // ע1��������ؾ��ⲻ���ڱ���ͨ�� Loopback ��ַת������ģ�һ��Ҫ����options.KnownNetworks.Clear()��options.KnownProxies.Clear()
    // ע2��������û������� ASPNETCORE_FORWARDEDHEADERS_ENABLED Ϊ True������Ҫ��������ô���
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.All;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });
    // ��������
    builder.Services.AddInMemoryRateLimiting();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

    // ��ʱͨѶ
    builder.Services.AddSignalR();

    // logo��ʾ
    builder.Services.AddLogoDisplay();
    // ��֤��
    builder.Services.AddLazyCaptcha(builder.Configuration);
    var app = builder.Build();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}