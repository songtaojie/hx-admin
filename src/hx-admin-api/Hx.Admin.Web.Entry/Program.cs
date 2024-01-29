using NLog;
using NLog.Web;
using System;


// �ڹ�������֮ǰ�����ڳ�ʼ��NLog�������������쳣��־
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    logger.Info("Starting web application");
    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureHxWebApp();
    builder.Services.AddAdminCoreService(builder.Configuration);
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    var app = builder.Build();
    app.UseAdminCoreApp(builder.Environment);
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "�������쳣��ֹͣ");
}
finally
{
    // ȷ����Ӧ�ó����˳�֮ǰˢ�º�ֹͣ�ڲ���ʱ��/�߳�(����Linux�ϵķֶδ���)
    NLog.LogManager.Shutdown();
}