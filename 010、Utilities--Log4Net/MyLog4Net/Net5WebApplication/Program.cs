using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Net5WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logBuilder =>
                {
                    logBuilder.ClearProviders();//ɾ�����������Ĺ�����־��¼������
                    logBuilder.SetMinimumLevel(LogLevel.Trace);//������͵�log����
                    logBuilder.AddLog4Net("CfgFile/log4Net.config");//֧��log4net
                });
    }
}
