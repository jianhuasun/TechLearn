# Hangfire详解

## 一、概述

### 1、概述

在 .NET 和 .NET Core 应用程序中执行后台处理的简单方法。无需 Windows 服务或单独的进程。由持久存储支持。开源且免费用于商业用途。

官网地址：[https://www.hangfire.io/](https://www.hangfire.io/)

文档地址：[https://docs.hangfire.io/en/latest/](https://docs.hangfire.io/en/latest/)

### 2、架构组成

Hangfire由三个主要部分组成：*客户端*、*存储*和*服务器*。

![image-20220622110305734](http://cdn.bluecusliyou.com/202206221103818.png)

#### （1）要求

> Hangfire 适用于大多数 .NET 平台：.NET Framework 4.5 或更高版本、.NET Core 1.0 或更高版本，或任何与 .NET Standard 1.3 兼容的平台。
>
> 您可以将它与几乎任何应用程序框架集成，包括 ASP.NET、ASP.NET Core、控制台应用程序、Windows 服务、WCF等。

#### （2）存储

> 存储是 Hangfire 保存与后台作业处理相关的所有信息的地方。类型、方法名称、参数等所有细节都被序列化并放入存储中，没有数据保存在进程的内存中。存储子系统在 Hangfire 中被很好地抽象出来，可以为 RDBMS 和 NoSQL 解决方案实现。
>
> 这是开始使用框架之前所需的唯一配置。以下示例显示如何使用 SQL Server 数据库配置 Hangfire。请注意，连接字符串可能会有所不同，具体取决于您的环境。

```
GlobalConfiguration.Configuration 
    .UseSqlServerStorage(@"Server=.\SQLEXPRESS; Database=Hangfire.Sample; Integrated Security=True");
```

#### （3）客户端

> 客户端可以创建任何类型的后台作业：即时任务（立即执行），延迟任务（在一段时间后执行调用），定时任务（每小时、每天执行方法等）。
>
> Hangfire 不需要你创建特殊的类。后台作业基于常规的静态或实例方法调用。

```C#
//实例方法调用
var client = new BackgroundJobClient();
client.Enqueue(() => Console.WriteLine("Hello world!"));

//静态方法调用
BackgroundJob.Enqueue(() => Console.WriteLine("Hello world!"));
```

#### （4）服务器

> 后台作业由[Hangfire Server](https://docs.hangfire.io/en/latest/background-processing/processing-background-jobs.html)处理。它被实现为一组专用的（不是线程池的）后台线程，它们从存储中获取作业并处理它们。服务器还负责保持存储清洁并自动删除旧数据。
>
> Hangfire 为每个存储后端使用可靠的获取算法，因此您可以在 Web 应用程序内部开始处理，而不会在应用程序重新启动、进程终止等时丢失后台作业的风险。
>
> 您只需要创建一个`BackgroundJobServer`类的实例并开始处理：

```C#
using (new BackgroundJobServer())
{
    Console.WriteLine("Hangfire Server started. Press ENTER to exit...");
    Console.ReadLine();
}
```

### 3、特性

![image-20220622204031012](http://cdn.bluecusliyou.com/202206222040168.png)

### 4、分布式

> 客户端创建任务到存储和服务器从存储拉取任务执行之间是可以分开执行的。
>
> 通过同一个存储进行交换数据，而不直接交换数据，可以实现客户端和服务器的解耦。

## 二、快速开始

### 1、nuget引入程序集

> Hangfire 作为几个 NuGet 包分发，从主要包 Hangfire.Core 开始，它包含所有主要类和抽象。Hangfire.SqlServer 等其他包提供功能或抽象实现。要开始使用 Hangfire，请安装主软件包并选择一个可用的存储。
>
> 这里我是用MySql作为Hangfire的Storage。Hangfire 官方在免费版中只提供了 SqlServer 接入的支持，在收费版多一个 Redis。需要 MongoDB、SqlServer 、PostgreSql、SQLite 等其他 Storages 的可以自己寻找第三方的开源项目，这里有一个官方推荐的扩展清单[https://www.hangfire.io/extensions.html](https://www.hangfire.io/extensions.html)，清单中列出了一些其他种类的 Storages。

```bash
Hangfire.Core  版本1.7.30，Net5
Hangfire.MySqlStorage  mysql数据库存储
```

### 2、安装数据库

> 安装数据库

安装数据库可以使用docker容器化安装，简单易用，一行命令解决，docker相关知识可以参考[docker详解](https://blog.csdn.net/liyou123456789/article/details/122292877)。

```bash
docker run --name mysqlserver -v /data/mysql/conf:/etc/mysql/conf.d -v /data/mysql/logs:/logs -v /data/mysql/data:/var/lib/mysql -e MYSQL_ROOT_PASSWORD=123456 -d -i -p 3306:3306 mysql:latest --lower_case_table_names=1
```

| 参数                                  | 说明                                                |
| ------------------------------------- | --------------------------------------------------- |
| --name mysqlserver                    | 容器运行的名字                                      |
| -v /data/mysql/conf:/etc/mysql/conf.d | 将宿主机/data/mysql/conf映射到容器/etc/mysql/conf.d |
| -v /data/mysql/logs:/logs             | 将宿主机/data/mysql/logs映射到容器/logs             |
| -v /data/mysql/data:/var/lib/mysql    | 将宿主机/data/mysql/data映射到容器 /var/lib/mysql   |
| -e MYSQL_ROOT_PASSWORD=123456         | 数据库初始密码123456                                |
| -p 3306:3306                          | 将宿主机3306端口映射到容器的3306端口                |
| --lower_case_table_names=1            | 设置表名忽略大小写，只能首次修改，后续无法修改      |

> 创建数据库hangfiredb

### 3、代码实现

> 使用`GlobalConfiguration`类执行配置。它的`Configuration`属性提供了很多扩展方法，既来自 Hangfire.Core，也来自其他包。
>
> 方法调用可以链式调用，因此无需一次又一次地使用类名。全局配置是为了简单起见，几乎每个 Hangfire 类都允许您为存储、过滤器等指定覆盖。在 ASP.NET Core 环境中，全局配置类隐藏在`AddHangfire`方法中。

```C#
internal class Program
{
    static void Main(string[] args)
    {
        //配置存储
        GlobalConfiguration.Configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)//全局配置兼容版本，向下兼容
            .UseColouredConsoleLogProvider()//输出日志
            .UseSimpleAssemblyNameTypeSerializer()//使用简单程序集名称类型序列化程序
            .UseRecommendedSerializerSettings()//使用推荐的序列化配置
            .UseStorage(new MySqlStorage(
             "server=服务器IP地址;Database=hangfiredb;user id=root;password=123456;SslMode=none",
             new MySqlStorageOptions
             {
                 TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                 QueuePollInterval = TimeSpan.FromSeconds(15),
                 JobExpirationCheckInterval = TimeSpan.FromHours(1),
                 CountersAggregateInterval = TimeSpan.FromMinutes(5),
                 PrepareSchemaIfNecessary = true,
                 DashboardJobListLimit = 50000,
                 TransactionTimeout = TimeSpan.FromMinutes(1),
                 TablesPrefix = "Hangfire"
             }));

        //客户端创建任务
        BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));

        //服务器运行任务
        using (var server = new BackgroundJobServer())
        {
            Console.ReadLine();
        }
    }
}
```

> 可选参数说明：
>
> - `TransactionIsolationLevel`- 事务隔离级别。默认为已提交读。
> - `QueuePollInterval`- 作业队列轮询间隔。默认值为 15 秒。
> - `JobExpirationCheckInterval`- 作业过期检查间隔（管理过期记录）。默认值为 1 小时。
> - `CountersAggregateInterval`- 聚合计数器的间隔。默认为 5 分钟。
> - `PrepareSchemaIfNecessary`- 如果设置为`true`，它会创建数据库表。默认为`true`。
> - `DashboardJobListLimit`- 仪表板作业列表限制。默认值为 50000。
> - `TransactionTimeout`- 交易超时。默认值为 1 分钟。
> - `TablesPrefix`- 数据库中表的前缀。默认为无

### 4、运行结果

#### （1）第一次运行数据库表会自动创建

![image-20220622152015151](http://cdn.bluecusliyou.com/202206221520196.png)

#### （2）任务执行成功

![image-20220622151856211](http://cdn.bluecusliyou.com/202206221518269.png)

## 三、支持多种任务类型

### 1、即时任务

> 即时任务作业只执行**一次**，几乎在创建后 **立即执行。**

```C#
var jobId = BackgroundJob .Enqueue(() => Console .WriteLine( "即时任务！" ));
```

### 2、延迟任务

> 延迟的作业也**只执行一次**，但不是在一定**时间间隔**后立即执行。

```C#
var jobId2 = BackgroundJob.Schedule(() => Console.WriteLine("延迟任务！"), TimeSpan.FromMilliseconds(10));
```

### 3、重复任务

> 重复作业在指定的**CRON 计划上****多次**触发。

```C#
RecurringJob.AddOrUpdate("myrecurringjob", () => Console.WriteLine("重复任务！"), Cron.Minutely);
```

### 4、继续任务

> 当其父作业**完成**时继续执行。

```C#
BackgroundJob.ContinueJobWith(jobId2,() => Console.WriteLine("jobId2执行完了再继续执行！"));
```

### 5、批次任务-收费

> 批处理是一组以**原子方式创建**并被视为单个实体的后台作业。

```C#
var batchId = BatchJob.StartNew(x =>
{
    x.Enqueue(() => Console.WriteLine("Job 1"));
    x.Enqueue(() => Console.WriteLine("Job 2"));
});
```

### 6、批处理继续任务--收费

> 当父批处理中的**所有后台作业****完成**时， 将触发批处理继续。

```C#
BatchJob.ContinueBatchWith(batchId, x =>
{
    x.Enqueue(() => Console.WriteLine("Last Job"));
});
```

### 7、封装任务

> 业务逻辑复杂的时候可以封装到方法中

```C#
//封装的方法
public class SyncUserDataSchedule
{
    public void SyncUserData()
    {
        Console.WriteLine($"同步用户数据--{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
    }
}
```

```C#
RecurringJob.AddOrUpdate<SyncUserDataSchedule>(c => c.SyncUserData(), Cron.Minutely);
```

## 四、集成到Asp.NetCore5框架

### 1、需求

> 仪表盘
>
> 权限控制
>
> http请求任务
>
> 请求参数
>
> 自定义任务
>
> corn配置

### 2、nuget引入程序集

```bash
Microsoft.AspNetCore.App
Hangfire.Core
Hangfire.MySqlStorage                   --mysql数据库存储
Hangfire.AspNetCore                     --AspNetCore支持
Hangfire.Dashboard.BasicAuthorization   --可视化+权限控制
Hangfire.HttpJob                        --httpJob
```

### 3、创建数据库

> 同上快速开始,创建hangfiredb

### 4、配置文件配置

> 添加连接字符串和打印日志等级
>
> Allow User Variables=true;这个参数重要，否则页面无法正常显示。

```json
{
  "ConnectionStrings": {
    "HangfireConnection":"server=服务器IP地址;Database=hangfiredb;userid=root;password=123456;SslMode=none;Allow User Variables=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information",
      "Hangfire": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### 5、Startup支持hangfire

> 账号验证也可以配置到数据库和配置文件等

```C#
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Config = configuration;
    }

    public IConfiguration Config { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        // Add Hangfire services.
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()                
            .UseStorage(new MySqlStorage(
             Config["ConnectionStrings:HangfireConnection"],
             new MySqlStorageOptions
             {
                 TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                 QueuePollInterval = TimeSpan.FromSeconds(15),
                 JobExpirationCheckInterval = TimeSpan.FromHours(1),
                 CountersAggregateInterval = TimeSpan.FromMinutes(5),
                 PrepareSchemaIfNecessary = true,
                 DashboardJobListLimit = 50000,
                 TransactionTimeout = TimeSpan.FromMinutes(1),
                 TablesPrefix = "Hangfire"
             })).UseHangfireHttpJob());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHangfireServer();
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
            {
                RequireSsl = false,
                SslRedirect = false,
                LoginCaseSensitive = true,
                Users = new []
                {
                    new BasicAuthAuthorizationUser
                    {
                        Login = "admin",
                        PasswordClear =  "test"
                    }
                }
            })}
        });

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        });
    }
}
```

### 6、运行成果

> 访问页面[http://localhost:8848/hangfire](http://localhost:8848/hangfire)，首次需要验证账号

![image-20220624144843124](http://cdn.bluecusliyou.com/202206241448406.png)

> 登陆之后可以看到整个仪表盘和配置界面
>
> Job管理：仪表盘主页
>
> 作业：作业的一些执行情况
>
> 重试：一些失败重试的作业信息
>
> 周期性作业：可以配置一些周期性作业
>
> 服务器：展示当前运行任务的服务器信息

![image-20220624145004636](http://cdn.bluecusliyou.com/202206241450721.png)

![image-20220624145145470](http://cdn.bluecusliyou.com/202206241451551.png)

![image-20220624150110226](http://cdn.bluecusliyou.com/202206241501299.png)

## 五、集成到Asp.NetCore6框架

> 其他部分同Asp.NetCore5，Program如下

```C#
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add Hangfire services.
        builder.Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseStorage(new MySqlStorage(
             builder.Configuration["ConnectionStrings:HangfireConnection"],
             new MySqlStorageOptions
             {
                 TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                 QueuePollInterval = TimeSpan.FromSeconds(15),
                 JobExpirationCheckInterval = TimeSpan.FromHours(1),
                 CountersAggregateInterval = TimeSpan.FromMinutes(5),
                 PrepareSchemaIfNecessary = true,
                 DashboardJobListLimit = 50000,
                 TransactionTimeout = TimeSpan.FromMinutes(1),
                 TablesPrefix = "Hangfire"
             })).UseHangfireHttpJob());


        var app = builder.Build();

        app.UseHangfireServer();
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
            {
                RequireSsl = false,
                SslRedirect = false,
                LoginCaseSensitive = true,
                Users = new []
                {
                    new BasicAuthAuthorizationUser
                    {
                        Login = "admin",
                        PasswordClear =  "test"
                    }
                }
            })}
        });

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}
```

## 六、最佳实践建议

### 1、业务和定时任务解耦

> 定时任务只配置请求业务api，将业务封装到api对外暴露api，实现业务和定时任务的解耦。
