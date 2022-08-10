namespace Net6WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //������־
            builder.Services.AddLogging(logBuilder =>
            {
                logBuilder.ClearProviders();//ɾ�����������Ĺ�����־��¼������
                logBuilder.SetMinimumLevel(LogLevel.Trace);//������͵�log����
                logBuilder.AddLog4Net("CfgFile/log4Net.config");//֧��log4net
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}