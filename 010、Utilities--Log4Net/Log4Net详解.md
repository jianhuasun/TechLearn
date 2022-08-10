# Log4Net详解

## 一、Log4Net详解

### 1、功能概述

#### （1）概述

Log4net库是.Net下一个非常优秀的开源日志记录组件，是一个帮助程序员将日志信息输出到各种目标（控制台、文件、数据库等）的工具。如果应用程序出现问题，启用日志记录有助于定位问题。

日志输出可能非常庞大，以至于很快就会变得不堪重负。log4net 的显着特征之一是分层记录器的概念。使用这些记录器可以有选择地控制以任意粒度输出哪些日志语句。

使用 log4net，可以在运行时修改配置文件，你不需要重新编译源代码就能改变日志的输出形式。

官网地址：[https://logging.apache.org/log4net/](https://logging.apache.org/log4net/)

源代码：[https://github.com/apache/logging-log4net/](https://github.com/apache/logging-log4net/)

#### （2）特征

- 支持多种框架

log4net 在所有兼容 ECMA CLI 1.0 的运行时上运行。

- 输出到多个日志记录目标
- 分层日志架构

分层日志记录非常适合基于组件的开发。每个组件都有自己的记录器。在单独测试时，这些记录器的属性可以根据开发人员的需要进行设置。当与其他组件组合时，记录器会继承由组件的集成商确定的属性。可以选择性地提升一个组件上的日志记录优先级，而不会影响其他组件。当您需要来自单个组件的详细跟踪而不用来自其他组件的消息使跟踪文件拥挤时，这很有用。这一切都可以通过配置文件来完成；无需更改代码。

- XML 配置

log4net 使用 XML 配置文件进行配置。配置信息可以嵌入到其他 XML 配置文件（例如应用程序的 .config 文件）或单独的文件中。该配置易于阅读和更新，同时保留了表达所有配置的灵活性。

- 动态配置

log4net 可以监视其配置文件的更改并动态应用配置器所做的更改。日志级别、附加程序、布局以及几乎所有其他内容都可以在运行时进行调整。在许多情况下，可以在不终止相关进程的情况下诊断应用程序问题。在调查已部署应用程序的问题时，这可能是一个非常有价值的工具。

- 记录上下文

log4net 可用于在日志记录点以对开发人员透明的方式收集日志记录上下文数据。GlobalContext 和 ThreadContext 允许应用程序存储附加到日志消息的上下文数据。例如，在 Web 服务中，一旦调用者通过身份验证，调用者的用户名就可以存储在 ThreadContext 属性中。然后，此属性将作为从同一线程生成的每个后续日志消息的一部分自动记录。

- 久经考验的架构

log4net 基于高度成功的 Apache log4j™ 日志库，自 1996 年开始开发。迄今为止，这种流行且经过验证的架构已被移植到 12 种语言。

- 模块化和可扩展设计
- 高性能和灵活性

### 2、快速开始

> 读取配置文件的形式是支持所有类型的项目的，是最普遍的，netframwork或者netcore，桌面控制台或者web

#### （1）nuget引入程序集

```bash
log4net  版本基于2.0.14版，Net6
```

#### （2）创建控制台

```C#
//读取指定的配置文件
XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4Net.config")));
//创建log对象
ILog ilog = LogManager.GetLogger(typeof(Program));
//打印日志
ilog.Debug("我打出了Log4Net日志！");
```

#### （3）log4Net.config配置文件，右键始终复制

```xml
<?xml version="1.0" encoding="utf-8" ?> 
 <log4net>
    <!-- Define some output appenders -->
    <appender name="rollingAppender" type="log4net.Appender.RollingFileAppender">
      <file value="D:\logs\log4net\log4net.log" /> 
      
      <!--追加日志内容-->
      <appendToFile value="true" />

      <!--防止多线程时不能写Log,官方说线程非安全-->
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />

      <!--可以为:Once|Size|Date|Composite-->
      <!--Composite为Size和Date的组合-->
      <rollingStyle value="Composite" />

      <!--当备份文件时,为文件名加的后缀,这里可以作为每一天的日志分别存储不同的文件-->
      <datePattern value="yyyyMMdd&quot;.txt&quot;" />
      <StaticLogFileName value="false"/>

      <!--日志最大个数,都是最新的-->
      <!--rollingStyle节点为Size时,只能有value个日志-->
      <!--rollingStyle节点为Composite时,每天有value个日志-->
      <maxSizeRollBackups value="20" />

      <!--可用的单位:KB|MB|GB-->
      <maximumFileSize value="3MB" />

      <!--置为true,当前最新日志文件名永远为file节中的名字-->
      <staticLogFileName value="true" />

      <!--输出级别在INFO和ERROR之间的日志-->
      <filter type="log4net.Filter.LevelRangeFilter">
        <param name="LevelMin" value="DEBUG" />
        <param name="LevelMax" value="FATAL" />
      </filter>

      <layout type="log4net.Layout.PatternLayout">
        <!--日志输出格式：时间  日志类型  日志内容-->
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline"/>
      </layout>
    </appender>

    <!-- levels: OFF > FATAL > ERROR > WARN > INFO > DEBUG  > ALL -->
    <root>
      <priority value="ALL"/>
      <level value="ALL"/>
      <appender-ref ref="rollingAppender" />
    </root>
  </log4net>
```

#### （4）运行结果

![image-20220705105758766](http://cdn.bluecusliyou.com/202207051057061.png)

### 3、Framwork控制台或者桌面程序

#### （1）读取默认的App.config文件

```C#
//默认读取App.config中的配置
XmlConfigurator.Configure();
//创建log对象
ILog ilog = LogManager.GetLogger(typeof(Program));
//打印日志
ilog.Debug("我打出了Log4Net日志！");
```

> App.config配置文件

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<!-- 1. 添加log4net的节点声明配置代码-->
	<configSections>
		<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/>
	</configSections>

	<log4net>
		<!-- Define some output appenders -->
		<appender name="rollingAppender" type="log4net.Appender.RollingFileAppender">
			<file value="D:\logs\log4net\log4net.log"/>

			<!--追加日志内容-->
			<appendToFile value="true"/>

			<!--防止多线程时不能写Log,官方说线程非安全-->
			<lockingModel type="log4net.Appender.FileAppender+MinimalLock"/>

			<!--可以为:Once|Size|Date|Composite-->
			<!--Composite为Size和Date的组合-->
			<rollingStyle value="Composite"/>

			<!--当备份文件时,为文件名加的后缀,这里可以作为每一天的日志分别存储不同的文件-->
			<datePattern value="yyyyMMdd&quot;.txt&quot;"/>
			<StaticLogFileName value="false"/>

			<!--日志最大个数,都是最新的-->
			<!--rollingStyle节点为Size时,只能有value个日志-->
			<!--rollingStyle节点为Composite时,每天有value个日志-->
			<maxSizeRollBackups value="20"/>

			<!--可用的单位:KB|MB|GB-->
			<maximumFileSize value="3MB"/>

			<!--置为true,当前最新日志文件名永远为file节中的名字-->
			<staticLogFileName value="true"/>

			<!--输出级别在INFO和ERROR之间的日志-->
			<filter type="log4net.Filter.LevelRangeFilter">
				<param name="LevelMin" value="DEBUG"/>
				<param name="LevelMax" value="FATAL"/>
			</filter>

			<layout type="log4net.Layout.PatternLayout">
				<!--日志输出格式：时间  日志类型  日志内容-->
				<conversionPattern value="%date [%thread] %-5level %logger - %message%newline"/>
			</layout>
		</appender>

		<!-- levels: OFF > FATAL > ERROR > WARN > INFO > DEBUG  > ALL -->
		<root>
			<priority value="ALL"/>
			<level value="ALL"/>
			<appender-ref ref="rollingAppender"/>
		</root>
	</log4net>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
    </startup>
</configuration>
```

#### （2）配置动作放在AssemblyInfo.cs中

```C#
//AssemblyInfo.cs中配置好配置文件
//[assembly: log4net.Config.XmlConfigurator()] // 指定读取默认的配置文件
//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4Net.config")] // 指定读取log4net 的配置文件
//创建log对象
ILog ilog = LogManager.GetLogger(typeof(Program));
//打印日志
ilog.Debug("我打出了Log4Net日志！");
```

### 4、Framworkwebform或者mvc

#### （1）读取默认的Web.config文件

```C#
//默认读取Web.config中的配置
XmlConfigurator.Configure();
//创建log对象
ILog ilog = LogManager.GetLogger(typeof(HomeController));
//打印日志
ilog.Debug("我打出了Log4Net日志！");
```

> Web.config配置文件

```xml
<?xml version="1.0" encoding="utf-8"?>
<!--
  有关如何配置 ASP.NET 应用程序的详细信息，请访问
   https://go.microsoft.com/fwlink/?LinkId=301880
-->
<configuration>
	<!-- 1. 添加log4net的节点声明配置代码-->
	<configSections>
		<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/>
	</configSections>

	<log4net>
		<!-- Define some output appenders -->
		<appender name="rollingAppender" type="log4net.Appender.RollingFileAppender">
			<file value="D:\logs\log4net\log4net.log"/>

			<!--追加日志内容-->
			<appendToFile value="true"/>

			<!--防止多线程时不能写Log,官方说线程非安全-->
			<lockingModel type="log4net.Appender.FileAppender+MinimalLock"/>

			<!--可以为:Once|Size|Date|Composite-->
			<!--Composite为Size和Date的组合-->
			<rollingStyle value="Composite"/>

			<!--当备份文件时,为文件名加的后缀,这里可以作为每一天的日志分别存储不同的文件-->
			<datePattern value="yyyyMMdd&quot;.txt&quot;"/>
			<StaticLogFileName value="false"/>

			<!--日志最大个数,都是最新的-->
			<!--rollingStyle节点为Size时,只能有value个日志-->
			<!--rollingStyle节点为Composite时,每天有value个日志-->
			<maxSizeRollBackups value="20"/>

			<!--可用的单位:KB|MB|GB-->
			<maximumFileSize value="3MB"/>

			<!--置为true,当前最新日志文件名永远为file节中的名字-->
			<staticLogFileName value="true"/>

			<!--输出级别在INFO和ERROR之间的日志-->
			<filter type="log4net.Filter.LevelRangeFilter">
				<param name="LevelMin" value="DEBUG"/>
				<param name="LevelMax" value="FATAL"/>
			</filter>

			<layout type="log4net.Layout.PatternLayout">
				<!--日志输出格式：时间  日志类型  日志内容-->
				<conversionPattern value="%date [%thread] %-5level %logger - %message%newline"/>
			</layout>
		</appender>

		<!-- levels: OFF > FATAL > ERROR > WARN > INFO > DEBUG  > ALL -->
		<root>
			<priority value="ALL"/>
			<level value="ALL"/>
			<appender-ref ref="rollingAppender"/>
		</root>
	</log4net>
  <appSettings>
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
  </appSettings>
  <system.web>
    <compilation debug="true" targetFramework="4.8" />
    <httpRuntime targetFramework="4.8" />
  </system.web>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Antlr3.Runtime" publicKeyToken="eb42632606e9261f" />
        <bindingRedirect oldVersion="0.0.0.0-3.5.0.2" newVersion="3.5.0.2" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" />
        <bindingRedirect oldVersion="0.0.0.0-12.0.0.0" newVersion="12.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Web.Optimization" publicKeyToken="31bf3856ad364e35" />
        <bindingRedirect oldVersion="1.0.0.0-1.1.0.0" newVersion="1.1.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="WebGrease" publicKeyToken="31bf3856ad364e35" />
        <bindingRedirect oldVersion="1.0.0.0-1.6.5135.21930" newVersion="1.6.5135.21930" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Web.Helpers" publicKeyToken="31bf3856ad364e35" />
        <bindingRedirect oldVersion="1.0.0.0-3.0.0.0" newVersion="3.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Web.WebPages" publicKeyToken="31bf3856ad364e35" />
        <bindingRedirect oldVersion="1.0.0.0-3.0.0.0" newVersion="3.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Web.Mvc" publicKeyToken="31bf3856ad364e35" />
        <bindingRedirect oldVersion="1.0.0.0-5.2.7.0" newVersion="5.2.7.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
  <system.codedom>
    <compilers>
      <compiler language="c#;cs;csharp" extension=".cs" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=2.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:default /nowarn:1659;1699;1701" />
      <compiler language="vb;vbs;visualbasic;vbscript" extension=".vb" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.VBCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=2.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:default /nowarn:41008 /define:_MYTYPE=\&quot;Web\&quot; /optionInfer+" />
    </compilers>
  </system.codedom>
</configuration>
```

#### （2）配置动作放在AssemblyInfo.cs中

```C#
//AssemblyInfo.cs中配置好配置文件
//[assembly: log4net.Config.XmlConfigurator()] // 指定读取默认的配置文件
//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4Net.config")] // 指定读取log4net 的配置文件
//创建log对象
ILog ilog = LogManager.GetLogger(typeof(HomeController));
//打印日志
ilog.Debug("我打出了Log4Net日志！");
```

#### （3）在Global.asax.cs的Application_Start方法中全局配置

```C#
public class MvcApplication : System.Web.HttpApplication
{
    protected void Application_Start()
    {
        //XmlConfigurator.Configure();//读取默认配置文件
        XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4Net.config")));//读取log4Net.config配置文件
        AreaRegistration.RegisterAllAreas();
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
}
```

> 在需要的地方使用

```C#
//在Global.asax.cs的Application_Start方法中全局配置
//创建log对象
ILog ilog = LogManager.GetLogger(typeof(HomeController));
//打印日志
ilog.Debug("我打出了Log4Net日志！");
```

### 5、集成到Asp.NetCore5框架

#### （1）Nuget引入程序集

```bash
log4net    日志记录驱动程序
Microsoft.Extensions.Logging.Log4Net.AspNetCore    依赖注入框架集成
```

#### （2）配置文件

> 放到文件夹CfgFile统一管理，配置文件右键属性`始终复制`

```xml
<?xml version="1.0" encoding="utf-8" ?> 
 <log4net>
    <!-- Define some output appenders -->
    <appender name="rollingAppender" type="log4net.Appender.RollingFileAppender">
      <file value="D:\logs\log4net\log4net.log" /> 
      
      <!--追加日志内容-->
      <appendToFile value="true" />

      <!--防止多线程时不能写Log,官方说线程非安全-->
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />

      <!--可以为:Once|Size|Date|Composite-->
      <!--Composite为Size和Date的组合-->
      <rollingStyle value="Composite" />

      <!--当备份文件时,为文件名加的后缀,这里可以作为每一天的日志分别存储不同的文件-->
      <datePattern value="yyyyMMdd&quot;.txt&quot;" />
      <StaticLogFileName value="false"/>

      <!--日志最大个数,都是最新的-->
      <!--rollingStyle节点为Size时,只能有value个日志-->
      <!--rollingStyle节点为Composite时,每天有value个日志-->
      <maxSizeRollBackups value="20" />

      <!--可用的单位:KB|MB|GB-->
      <maximumFileSize value="3MB" />

      <!--置为true,当前最新日志文件名永远为file节中的名字-->
      <staticLogFileName value="true" />

      <!--输出级别在INFO和ERROR之间的日志-->
      <filter type="log4net.Filter.LevelRangeFilter">
        <param name="LevelMin" value="DEBUG" />
        <param name="LevelMax" value="FATAL" />
      </filter>

      <layout type="log4net.Layout.PatternLayout">
        <!--日志输出格式：时间  日志类型  日志内容-->
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline"/>
      </layout>
    </appender>

    <!-- levels: OFF > FATAL > ERROR > WARN > INFO > DEBUG  > ALL -->
    <root>
      <priority value="ALL"/>
      <level value="ALL"/>
      <appender-ref ref="rollingAppender" />
    </root>
  </log4net>
```

#### （3）Program添加对log4net支持

```C#
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
                logBuilder.ClearProviders();//删除所有其他的关于日志记录的配置
                logBuilder.SetMinimumLevel(LogLevel.Trace);//设置最低的log级别
                logBuilder.AddLog4Net("CfgFile/log4Net.config");//支持log4net
            });
}
```

#### （4）添加控制器和页面，注入日志对象

```C#
public class FirstController : Controller
{
    private readonly ILogger<FirstController> _ILogger;
    private readonly ILoggerFactory _ILoggerFactory;
    public FirstController(ILogger<FirstController> logger, ILoggerFactory iLoggerFactory)
    {
        this._ILogger = logger;
        _ILogger.LogInformation($"{this.GetType().FullName} 被构造。。。。LogInformation");
        _ILogger.LogError($"{this.GetType().FullName} 被构造。。。。LogError");
        _ILogger.LogDebug($"{this.GetType().FullName} 被构造。。。。LogDebug");
        _ILogger.LogTrace($"{this.GetType().FullName} 被构造。。。。LogTrace");
        _ILogger.LogCritical($"{this.GetType().FullName} 被构造。。。。LogCritical");

        this._ILoggerFactory = iLoggerFactory;
        ILogger<FirstController> _ILogger2 = _ILoggerFactory.CreateLogger<FirstController>();
        _ILogger2.LogInformation("这里是通过Factory得到的Logger写的日志");
    }

    public IActionResult Index()
    {
        _ILogger.LogInformation($"{this.GetType().FullName} Index被请求");
        return View();
    }
}
```

#### （5）请求页面，运行结果

![image-20220705132809270](http://cdn.bluecusliyou.com/202207051328351.png)

### 6、集成到Asp.NetCore6框架

#### （1）Nuget引入程序集

```bash
log4net    日志记录驱动程序
Microsoft.Extensions.Logging.Log4Net.AspNetCore    依赖注入框架集成
```

#### （2）配置文件

> 放到文件夹CfgFile统一管理，配置文件右键属性`始终复制`

```xml
<?xml version="1.0" encoding="utf-8" ?> 
 <log4net>
    <!-- Define some output appenders -->
    <appender name="rollingAppender" type="log4net.Appender.RollingFileAppender">
      <file value="D:\logs\log4net\log4net.log" /> 
      
      <!--追加日志内容-->
      <appendToFile value="true" />

      <!--防止多线程时不能写Log,官方说线程非安全-->
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />

      <!--可以为:Once|Size|Date|Composite-->
      <!--Composite为Size和Date的组合-->
      <rollingStyle value="Composite" />

      <!--当备份文件时,为文件名加的后缀,这里可以作为每一天的日志分别存储不同的文件-->
      <datePattern value="yyyyMMdd&quot;.txt&quot;" />
      <StaticLogFileName value="false"/>

      <!--日志最大个数,都是最新的-->
      <!--rollingStyle节点为Size时,只能有value个日志-->
      <!--rollingStyle节点为Composite时,每天有value个日志-->
      <maxSizeRollBackups value="20" />

      <!--可用的单位:KB|MB|GB-->
      <maximumFileSize value="3MB" />

      <!--置为true,当前最新日志文件名永远为file节中的名字-->
      <staticLogFileName value="true" />

      <!--输出级别在INFO和ERROR之间的日志-->
      <filter type="log4net.Filter.LevelRangeFilter">
        <param name="LevelMin" value="DEBUG" />
        <param name="LevelMax" value="FATAL" />
      </filter>

      <layout type="log4net.Layout.PatternLayout">
        <!--日志输出格式：时间  日志类型  日志内容-->
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline"/>
      </layout>
    </appender>

    <!-- levels: OFF > FATAL > ERROR > WARN > INFO > DEBUG  > ALL -->
    <root>
      <priority value="ALL"/>
      <level value="ALL"/>
      <appender-ref ref="rollingAppender" />
    </root>
  </log4net>
```

#### （3）Program添加对log4net支持

```C#
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        //配置日志
        builder.Services.AddLogging(logBuilder =>
        {
            logBuilder.ClearProviders();//删除所有其他的关于日志记录的配置
            logBuilder.SetMinimumLevel(LogLevel.Trace);//设置最低的log级别
            logBuilder.AddLog4Net("CfgFile/log4Net.config");//支持log4net
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
```

#### （4）添加控制器和页面，注入日志对象

```C#
public class FirstController : Controller
{
    private readonly ILogger<FirstController> _ILogger;
    private readonly ILoggerFactory _ILoggerFactory;
    public FirstController(ILogger<FirstController> logger, ILoggerFactory iLoggerFactory)
    {
        this._ILogger = logger;
        _ILogger.LogInformation($"{this.GetType().FullName} 被构造。。。。LogInformation");
        _ILogger.LogError($"{this.GetType().FullName} 被构造。。。。LogError");
        _ILogger.LogDebug($"{this.GetType().FullName} 被构造。。。。LogDebug");
        _ILogger.LogTrace($"{this.GetType().FullName} 被构造。。。。LogTrace");
        _ILogger.LogCritical($"{this.GetType().FullName} 被构造。。。。LogCritical");

        this._ILoggerFactory = iLoggerFactory;
        ILogger<FirstController> _ILogger2 = _ILoggerFactory.CreateLogger<FirstController>();
        _ILogger2.LogInformation("这里是通过Factory得到的Logger写的日志");
    }

    public IActionResult Index()
    {
        _ILogger.LogInformation($"{this.GetType().FullName} Index被请求");
        return View();
    }
}
```

#### （5）请求页面，运行结果

![image-20220705135829148](http://cdn.bluecusliyou.com/202207051358449.png)

### 7、组件详解

Log4net 具有三个主要组件：*loggers*、*appenders*和*layouts*。这三种类型的组件协同工作，使开发人员能够根据消息类型和级别记录消息，并在运行时控制这些消息的格式和报告位置。在过滤器的帮助下，对输出内容进行更精细的控制。

#### （1）loggers

Logger是应用程序需要交互的主要组件，它用来产生日志消息。产生的日志消息并不直接显示，还要预先经过Layout的格式化处理后才会输出。

logger实例由LogManager创建，LogManager类用来管理所有的Logger。通常来说，我们会以类的类型typeof(Classname)为参数来调用GetLogger()，以便跟踪我们正在进行日志记录的类。或者反射方法获得`System.Reflection.MethodBase.GetCurrentMethod().DeclaringType`

```C#
namespace log4net
{
    public class LogManager
    {
        public static ILog GetLogger(string name);
        public static ILog GetLogger(Type type);
    }
}
```

logger都继承自ILog接口，ILog定义了5个方法（Debug,Inof,Warn,Error,Fatal）分别对不同的日志等级记录日志。这5个方法还有5个重载。

以下级别按优先级递增的顺序定义：

- ALL
- DEBUG （调试信息）：记录系统用于调试的一切信息，内容或者是一些关键数据内容的输出。
- INFO（一般信息）：记录系统运行中应该让用户知道的基本信息。例如，服务开始运行，功能已经开户等。
- WARN（警告）：记录系统中不影响系统继续运行，但不符合系统运行正常条件，有可能引起系统错误的信息。例如，记录内容为空，数据内容不正确等。
- ERROR（一般错误）：记录系统中出现的导致系统不稳定，部分功能出现混乱或部分功能失效一类的错误。例如，数据字段为空，数据操作不可完成，操作出现异常等。
- FATAL（致命错误）：记录系统中出现的能使用系统完全失去功能，服务停止，系统崩溃等使系统无法继续运行下去的错误。例如，数据库无法连接，系统出现死循环。
- OFF

```C#
namespace log4net
{
    public interface ILog
    {
        /* Test if a level is enabled for logging */
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
 
        /* Log a message object */
        void Debug(object message);
        void Info(object message);
        void Warn(object message);
        void Error(object message);
        void Fatal(object message);
 
        /* Log a message object and exception */
        void Debug(object message, Exception t);
        void Info(object message, Exception t);
        void Warn(object message, Exception t);
        void Error(object message, Exception t);
        void Fatal(object message, Exception t);
 
        /* Log a message string using the System.String.Format syntax */
        void DebugFormat(string format, params object[] args);
        void InfoFormat(string format, params object[] args);
        void WarnFormat(string format, params object[] args);
        void ErrorFormat(string format, params object[] args);
        void FatalFormat(string format, params object[] args);
 
        /* Log a message string using the System.String.Format syntax */
        void DebugFormat(IFormatProvider provider, string format, params object[] args);
        void InfoFormat(IFormatProvider provider, string format, params object[] args);
        void WarnFormat(IFormatProvider provider, string format, params object[] args);
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);
        void FatalFormat(IFormatProvider provider, string format, params object[] args);
    }
}
```

一个常规的日志打印案例

```C#
//默认读取App.config中的配置
XmlConfigurator.Configure();
//创建log对象
ILog ilog = LogManager.GetLogger(typeof(Program));
//打印日志
ilog.Debug("我打出了Log4Net日志！");
```

Logger提供了多种方式来记录一个日志消息，也可以有多个Logger同时存在。

在log4net框架里，log4net使用继承体系， "System" 是 "System.Text" 的父级和"System.Text.StringBuilder"的 祖先。每个Logger都继承了它祖先的属性。所有的Logger都从Root继承，Root本身也是一个Logger。每个日志对象都被分配了一个日志优先级别。如果没有给一个日志对象显式地分配一个级别，那么该对象会试图从他的祖先root继承一个级别值。

在<root>标签里，可以定义level级别值，如果没有定义LEVEL的值，则缺省为DEBUG。在一个logger对象中的设置会覆盖根日志的设置。

```xml
<root>
  <level value="WARN" />
  <appender-ref ref="ConsoleAppender" />
</root>

<logger name="testApp.Logging">
</logger>
```

#### （2）Appender

用来定义日志的输出方式，即日志要写到那种介质上去。较常用的Log4net已经实现好了，直接在配置文件中调用即可。当然也可以自己写一个，Appenders 必须实现log4net.Appenders.IAppender 接口类。

log4net 包中定义了以下附加程序：

| 类型                                                         | 描述                                                         |
| :----------------------------------------------------------- | :----------------------------------------------------------- |
| [log4net.Appender.AdoNetAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_AdoNetAppender.htm) | 使用准备好的语句或存储过程将日志记录事件写入数据库。         |
| [log4net.Appender.AnsiColorTerminalAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_AnsiColorTerminalAppender.htm) | 将颜色突出显示的日志事件写入 ANSI 终端窗口。                 |
| [log4net.Appender.AspNetTraceAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_AspNetTraceAppender.htm) | 将日志记录事件写入 ASP 跟踪上下文。然后可以在 ASP 页的末尾或在 ASP 跟踪页上呈现这些。 |
| [log4net.Appender.BufferingForwardingAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_BufferingForwardingAppender.htm) | 在将事件转发给子附加程序之前缓冲记录事件。                   |
| [log4net.Appender.ColoredConsoleAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_ColoredConsoleAppender.htm) | 将日志记录事件写入应用程序的控制台。事件可能会进入标准我们的流或标准错误流。事件可能具有为每个级别定义的可配置文本和背景颜色。 |
| [log4net.Appender.ConsoleAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_ConsoleAppender.htm) | 将日志记录事件写入应用程序的控制台。事件可能会进入标准我们的流或标准错误流。 |
| [log4net.Appender.DebugAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_DebugAppender.htm) | 将日志事件写入 .NET 系统。                                   |
| [log4net.Appender.EventLogAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_EventLogAppender.htm) | 将日志记录事件写入 Windows 事件日志。                        |
| [log4net.Appender.FileAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_FileAppender.htm) | 将日志记录事件写入文件系统中的文件。                         |
| [log4net.Appender.ForwardingAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_ForwardingAppender.htm) | 将日志记录事件转发给子附加程序。                             |
| [log4net.Appender.LocalSyslogAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_LocalSyslogAppender.htm) | 将日志记录事件写入本地 syslog 服务（仅限 UNIX）。            |
| [log4net.Appender.MemoryAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_MemoryAppender.htm) | 将日志记录事件存储在内存缓冲区中。                           |
| [log4net.Appender.NetSendAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_NetSendAppender.htm) | 将日志事件写入 Windows Messenger 服务。这些消息显示在用户终端的对话框中。 |
| [log4net.Appender.OutputDebugStringAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_OutputDebugStringAppender.htm) | 将日志记录事件写入调试器。如果应用程序没有调试器，系统调试器会显示该字符串。如果应用程序没有调试器并且系统调试器未激活，则忽略该消息。 |
| [log4net.Appender.RemoteSyslogAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_RemoteSyslogAppender.htm) | 使用 UDP 网络将日志记录事件写入远程系统日志服务。            |
| [log4net.Appender.RemotingAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_RemotingAppender.htm) | 使用 .NET 远程处理将日志记录事件写入远程接收器。             |
| [log4net.Appender.RollingFileAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_RollingFileAppender.htm) | 将日志记录事件写入文件系统中的文件。RollingFileAppender 可以配置为根据日期或文件大小限制记录到多个文件。 |
| [log4net.Appender.SmtpAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_SmtpAppender.htm) | 将日志记录事件发送到电子邮件地址。                           |
| [log4net.Appender.SmtpPickupDirAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_SmtpPickupDirAppender.htm) | 将 SMTP 消息作为文件写入拾取目录。然后可以通过 SMTP 代理（例如 IIS SMTP 代理）读取和发送这些文件。 |
| [log4net.Appender.TelnetAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_TelnetAppender.htm) | 客户端通过 Telnet 连接以接收日志记录事件。                   |
| [log4net.Appender.TraceAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_TraceAppender.htm) | 将日志记录事件写入 .NET 跟踪系统。                           |
| [log4net.Appender.UdpAppender](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Appender_UdpAppender.htm) | 使用 UdpClient 将日志事件作为无连接 UDP 数据报发送到远程主机或多播组。 |

一个记录器可以附加多个附加程序，给定记录器的每个启用的记录请求都将转发到该记录器中的所有附加程序以及层次结构中更高的附加程序。换句话说，appender 是从 logger 层次结构中附加继承的。

例如，如果将控制台附加程序添加到*根*记录器，则所有启用的日志记录请求至少将打印在控制台上。如果另外一个文件追加器被添加到一个记录器中，比如*X ，那么为**X*和*X*的孩子启用的日志记录请求将打印在一个文件上*，并且*在控制台上。可以通过将 logger 上的 additivity 标志设置为false来覆盖此默认行为，以便 appender 累积不再是相加的 。

管理附加程序可加性的规则总结如下：

| 记录器名称      | 附加器     | 可加性标志     | 输出目标               | 输出目标Comment                                              |
| :-------------- | :--------- | :------------- | :--------------------- | :----------------------------------------------------------- |
| *root*          | A1         | not applicable | A1                     | There is no default appender attached to *root*.             |
| x               | A-x1, A-x2 | true           | A1, A-x1, A-x2         | Appenders of "x" and *root*.                                 |
| x.y             | none       | true           | A1, A-x1, A-x2         | Appenders of "x" and *root*.                                 |
| x.y.z           | A-xyz1     | true           | A1, A-x1, A-x2, A-xyz1 | Appenders in "x.y.z", "x" and *root*.                        |
| security        | A-sec      | false          | A-sec                  | No appender accumulation since the additivity flag is set to false. |
| security.access | none       | true           | A-sec                  | Only appenders of "security" because the additivity flag in "security" is set to false. |

<appender>标签定义了appender的名字和类型。 另外比较重要的是<appender>标签内部的其他标签。不同的appender有不同的<param>标签。另外还需要一个在<appender>标签内部定义一个Layout对象和filter对象。

```xml
<appender name="LogFileAppender" type="log4net.Appender.FileAppender" >
  <param name="File" value="log-file.txt" />
  <param name="AppendToFile" value="true" />
  <layout type="log4net.Layout.PatternLayout">
    <param name="Header" value="[Header]\r\n" />
    <param name="Footer" value="[Footer]\r\n"/>
    <param name="ConversionPattern"
      value="%d [%t] %-5p %c - %m%n"
    />
  </layout>
  <filter type="log4net.Filter.LevelRangeFilter">
    <param name="LevelMin" value="DEBUG" />
    <param name="LevelMax" value="WARN" />
  </filter>
</appender>
```

#### （3）Appender Filters

可以在配置中指定过滤器，以允许对通过不同附加程序记录的事件进行精细控制。

可以使用每个附加程序上定义的过滤器链来完成更复杂和自定义的事件过滤。过滤器必须实现 log4net.Filter.IFilter接口。

log4net 包中定义了以下过滤器：

| 类型                                                         | 描述                           |
| :----------------------------------------------------------- | :----------------------------- |
| [log4net.Filter.DenyAllFilter](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Filter_DenyAllFilter.htm) | 删除所有日志记录事件。         |
| [log4net.Filter.LevelMatchFilter](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Filter_LevelMatchFilter.htm) | 与事件级别完全匹配。           |
| [log4net.Filter.LevelRangeFilter](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Filter_LevelRangeFilter.htm) | 匹配一系列级别。               |
| [log4net.Filter.LoggerMatchFilter](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Filter_LoggerMatchFilter.htm) | 匹配记录器名称的开头。         |
| [log4net.Filter.PropertyFilter](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Filter_PropertyFilter.htm) | 匹配来自特定属性值的子字符串。 |
| [log4net.Filter.StringMatchFilter](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Filter_StringMatchFilter.htm) | 匹配事件消息中的子字符串。     |

#### （4）layout

Layout用于控制Appender的输出格式。一个Appender只能有一个Layout。

最常用的Layout应该是经典格式的PatternLayout，允许用户根据类似于C语言printf的转换模式指定输出格式功能。其次是SimpleLayout，**RawTimeStampLayout**和ExceptionLayout。然后还有IRawLayout，XMLLayout等几个使用较少。

- SimpleLayout简单输出格式，只输出日志级别与消息内容。

- **RawTimeStampLayout** 用来格式化时间，在向数据库输出时会用到。

- 样式如“yyyy-MM-dd HH:mm:ss“

- ExceptionLayout需要给Logger的方法传入Exception对象作为参数才起作用，否则就什么也不输出。输出的时候会包含Message和Trace。

- PatterLayout使用最多的一个Layout，能输出的信息很多。


log4net 包中包含以下布局：

| 类型                                                         | 描述                                                    |
| :----------------------------------------------------------- | :------------------------------------------------------ |
| [log4net.Layout.ExceptionLayout](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Layout_ExceptionLayout.htm) | 呈现来自日志记录事件的异常文本。                        |
| [log4net.Layout.PatternLayout](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Layout_PatternLayout.htm) | 根据一组灵活的格式化标志格式化日志事件。                |
| [log4net.Layout.RawTimeStampLayout](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Layout_RawTimeStampLayout.htm) | 从日志记录事件中提取时间戳。                            |
| [log4net.Layout.RawUtcTimeStampLayout](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Layout_RawUtcTimeStampLayout.htm) | 从通用时间的日志记录事件中提取时间戳。                  |
| [log4net.Layout.SimpleLayout](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Layout_SimpleLayout.htm) | 非常简单地格式化日志事件： [level] - [message]          |
| [log4net.Layout.XmlLayout](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Layout_XmlLayout.htm) | 将日志记录事件格式化为 XML 元素。                       |
| [log4net.Layout.XmlLayoutSchemaLog4j](https://logging.apache.org/log4net/release/sdk/html/T_log4net_Layout_XmlLayoutSchemaLog4j.htm) | 将日志记录事件格式化为符合 log4j 事件 dtd 的 XML 元素。 |

### 8、配置文件详解

#### （1）两种配置方式

log4net 环境完全可以通过编程方式进行配置。但是，使用配置文件配置 log4net 要灵活得多。目前，配置文件是用 XML 编写的。

> 使用代码实现配置

BasicConfigurator.Configure()方法 的调用 创建了一个相当简单的 log4net 设置。此方法将ConsoleAppender添加到*根*记录器 。输出将使用 PatternLayout 设置为模式 "%-4timestamp [%thread] %-5level %logger %ndc - %message%newline"进行格式化。默认情况下，*根*记录器被分配给 Level.DEBUG。

```C#
// Import log4net classes.
using log4net;
using log4net.Config;

public class MyApp 
{
    // Define a static logger variable so that it references the
    // Logger instance named "MyApp".
    private static readonly ILog log = LogManager.GetLogger(typeof(MyApp));

    static void Main(string[] args) 
    {
        // Set up a simple configuration that logs on the console.
        BasicConfigurator.Configure();

        log.Info("我打印了log4net日志");
    }
}
```

> 一些常用的代码配置方式

尽管这里用代码配置log4net也很方便，但是你却不能分别配置每个日志对象。所有的这些配置都是被应用到根日志上的。

log4net.Config.BasicConfigurator 类使用静态方法Configure 设置一个Appender 对象。而Appender的构造函数又会相应的要求Layout对象。你也可以不带参数直接调用BasicConfigurator.Configure()，它会使用一个缺省的PatternLayout对象，在一个ConsoleAppender中输出信息。

```C#
// 和PatternLayout一起使用FileAppender
log4net.Config.BasicConfigurator.Configure(
  new log4net.Appender.FileAppender(
     new log4net.Layout.PatternLayout("%d
       [%t]%-5p %c [%x] - %m%n"),"testfile.log"));
                           
// using a FileAppender with an XMLLayout
log4net.Config.BasicConfigurator.Configure(
  new log4net.Appender.FileAppender(
    new log4net.Layout.XMLLayout(),"testfile.xml")); 

// using a ConsoleAppender with a PatternLayout
log4net.Config.BasicConfigurator.Configure(
  new log4net.Appender.ConsoleAppender(
    new log4net.Layout.PatternLayout("%d
      [%t] %-5p %c - %m%n"))); 

// using a ConsoleAppender with a SimpleLayout
log4net.Config.BasicConfigurator.Configure(
  new log4net.Appender.ConsoleAppender(new
    log4net.Layout.SimpleLayout()));
```

> 使用配置文件实现配置

此版本的 MyApp 指示 XmlConfigurator 解析配置文件并相应地设置日志记录。配置文件的路径在命令行中指定。

```C#
// Import log4net classes.
using log4net;
using log4net.Config;

public class MyApp 
{
    private static readonly ILog log = LogManager.GetLogger(typeof(MyApp));

    static void Main(string[] args) 
    {
        // BasicConfigurator replaced with XmlConfigurator.
        XmlConfigurator.Configure(new System.IO.FileInfo(args[0]));

        log.Info("我打印了log4net日志");
    }
}
```

此版本的 MyApp 指示 XmlConfigurator 解析配置文件并相应地设置日志记录。配置文件的路径在命令行中指定。

这是一个示例配置文件，它产生与前面基于BasicConfigurator的示例 完全相同的输出 。

```xml
<log4net>
    <!-- A1 is set to be a ConsoleAppender -->
    <appender name="A1" type="log4net.Appender.ConsoleAppender">
 
        <!-- A1 uses PatternLayout -->
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%-4timestamp [%thread] %-5level %logger %ndc - %message%newline" />
        </layout>
    </appender>
    
    <!-- Set root logger level to DEBUG and its only appender to A1 -->
    <root>
        <level value="DEBUG" />
        <appender-ref ref="A1" />
    </root>
</log4net>
```

#### （2）log4net根元素

log4net 包含一个解析 XML DOM 的配置阅读器 log4net.Config.XmlConfigurator。本节定义了配置器接受的语法。

这是一个有效的 XML 配置示例。根元素必须是<log4net>。

```xml
<log4net>
    <appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender" >
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%date [%thread] %-5level %logger [%ndc] - %message%newline" />
        </layout>
    </appender>
    <root>
        <level value="INFO" />
        <appender-ref ref="ConsoleAppender" />
    </root>
</log4net>
```

<log4net>元素支持以下属性 ：

| 属性      | 描述                                                         |
| :-------- | :----------------------------------------------------------- |
| debug     | 可选属性。值必须是true或false。默认值为false。将此属性设置为true 以启用此配置的内部 log4net 调试。 |
| update    | 可选属性。值必须是Merge或Overwrite。默认值为合并。将此属性设置为Overwrite 以在应用此配置之前重置正在配置的存储库的配置。 |
| threshold | 可选属性。值必须是在存储库上注册的级别的名称。默认值为ALL。设置此属性以限制在整个存储库中记录的消息，而不管消息记录到的记录器。 |

<log4net>元素支持 以下子元素：

| 元素     | 描述                                         |
| :------- | :------------------------------------------- |
| appender | 允许零个或多个元素。定义一个附加程序。       |
| logger   | 允许零个或多个元素。定义记录器的配置。       |
| renderer | 允许零个或多个元素。定义对象渲染器。         |
| root     | 可选元素，最多允许一个。定义根记录器的配置。 |
| param    | 允许零个或多个元素。存储库特定参数           |

#### （3）Appender

Appender 只能定义为<log4net>元素的子元素。每个 appender 必须唯一命名。必须指定附加程序的实现类型。

此示例显示正在定义的log4net.Appender.ConsoleAppender类型的附加程序。appender 将被称为*ConsoleAppender*。

```xml
<appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender" >
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%ndc] - %message%newline" />
    </layout>
</appender>
```

<appender>元素支持以下属性 ：

| 属性 | 描述                                                         |
| :--- | :----------------------------------------------------------- |
| name | 必需的属性。值必须是此附加程序的字符串名称。该名称在此配置文件中定义的所有附加程序中必须是唯一的。Logger 的<appender-ref>元素 使用此名称 来引用 appender。 |
| type | 必需的属性。值必须是此附加程序的类型名称。如果在 log4net 程序集中未定义附加程序，则此类型名称必须是完全程序集限定的。 |

<appender>元素支持 以下子元素：

| 元素         | 描述                                                         |
| :----------- | :----------------------------------------------------------- |
| appender-ref | 允许零个或多个元素。允许 appender 引用其他 appender。并非所有附加程序都支持。 |
| filter       | 允许零个或多个元素。定义此 appender 使用的过滤器。           |
| layout       | 可选元素，最多允许一个。定义此 appender 使用的布局。         |
| param        | 允许零个或多个元素。附加特定参数。                           |

#### （4）Appender Filters

过滤器元素只能定义为<appender>元素的子元素。

<filter>元素支持以下属性 ：

| 属性 | 描述                                                         |
| :--- | :----------------------------------------------------------- |
| type | 必需的属性。值必须是此过滤器的类型名称。如果过滤器未在 log4net 程序集中定义，则此类型名称必须是完全程序集限定的。 |

<filter>元素支持 以下子元素：

| 元素  | 描述                               |
| :---- | :--------------------------------- |
| param | 允许零个或多个元素。过滤特定参数。 |

过滤器形成一个事件必须通过的链。一路上的任何过滤器都可以接受事件并停止处理，拒绝事件并停止处理，或允许事件进入下一个过滤器。如果事件在没有被拒绝的情况下到达过滤器链的末尾，它将被隐式接受并被记录。

```xml
<filter type="log4net.Filter.LevelRangeFilter">
    <levelMin value="INFO" />
    <levelMax value="FATAL" />
</filter>
```

此过滤器将拒绝级别低于INFO 或高于FATAL的事件。将记录 INFO和FATAL之间的所有事件。

如果我们只想允许具有特定子字符串（例如“database”）的消息通过，那么我们需要指定以下过滤器：

```xml
<filter type="log4net.Filter.StringMatchFilter">
    <stringToMatch value="database" />
</filter>
<filter type="log4net.Filter.DenyAllFilter" />
```

第一个过滤器将在事件的消息文本中查找子字符串“database”。如果找到文本，过滤器将接受该消息并且过滤器处理将停止，该消息将被记录。如果未找到子字符串，则事件将传递给下一个过滤器进行处理。如果没有下一个过滤器，则该事件将被隐式接受并被记录。但是因为我们不希望记录不匹配的事件，所以我们需要使用log4net.Filter.DenyAllFilter 那只会否认所有到达它的事件。此过滤器仅在过滤器链的末尾有用。

如果我们想允许在消息文本中包含“database”或“ldap”的事件，我们可以使用以下过滤器：

```xml
<filter type="log4net.Filter.StringMatchFilter">
    <stringToMatch value="database"/>
</filter>
<filter type="log4net.Filter.StringMatchFilter">
    <stringToMatch value="ldap"/>
</filter>
<filter type="log4net.Filter.DenyAllFilter" />
```

#### （5）layout

布局元素只能定义为<appender>元素的子元素。

<layout>元素支持以下属性 ：

| 属性 | 描述                                                         |
| :--- | :----------------------------------------------------------- |
| type | 必需的属性。值必须是此布局的类型名称。如果未在 log4net 程序集中定义布局，则此类型名称必须是完全程序集限定的。 |

<layout>元素支持 以下子元素：

| 元素  | 描述                               |
| :---- | :--------------------------------- |
| param | 允许零个或多个元素。布局特定参数。 |

此示例显示如何配置使用log4net.Layout.PatternLayout的布局。

```xml
<layout type="log4net.Layout.PatternLayout">
    <conversionPattern value="%date [%thread] %-5level %logger [%ndc] - %message%newline" />
</layout>
```

#### （6）root logger

只能定义一个根 logger 元素，并且它必须是<log4net>元素的子元素。根记录器是记录器层次结构的根。所有记录器最终都继承自该记录器。

```xml
<root>
    <level value="INFO" />
    <appender-ref ref="ConsoleAppender" />
</root>
```

< root>元素不支持任何属性。

<root>元素支持 以下子元素：

| 元素         | 描述                                                         |
| :----------- | :----------------------------------------------------------- |
| appender-ref | 允许零个或多个元素。允许记录器按名称引用附加程序。           |
| level        | 可选元素，最多允许一个。定义此记录器的记录级别。此记录器将仅接受处于此级别或更高级别的事件。 |
| param        | 允许零个或多个元素。记录器特定参数。                         |

#### （7）logger

记录器元素只能定义为<log4net>元素的子元素。

一个示例记录器：

```xml
<logger name="LoggerName">
    <level value="DEBUG" />
    <appender-ref ref="ConsoleAppender" />
</logger>
```

<logger>元素支持 以下属性。

| 属性       | 描述                                                         |
| :--------- | :----------------------------------------------------------- |
| name       | 必需的属性。值必须是记录器的名称。                           |
| additivity | 可选属性。值可以是true或false。默认值为true。将此属性设置为false 以防止此记录器继承父记录器上定义的附加程序。 |

<logger>元素支持 以下子元素：

| 元素         | 描述                                                         |
| :----------- | :----------------------------------------------------------- |
| appender-ref | 允许零个或多个元素。允许记录器按名称引用附加程序。           |
| level        | 可选元素，最多允许一个。定义此记录器的记录级别。此记录器将仅接受处于此级别或更高级别的事件。 |
| param        | 允许零个或多个元素。记录器特定参数。                         |

#### （8）renderer

渲染器元素只能定义为<log4net>元素的子元素。

一个示例渲染器：

```xml
<renderer renderingClass="MyClass.MyRenderer" renderedClass="MyClass.MyFunkyObject" />   
```

<renderer>元素支持 以下属性。

| 属性           | 描述                                                         |
| :------------- | :----------------------------------------------------------- |
| renderingClass | 必需的属性。值必须是此渲染器的类型名称。如果类型未在 log4net 程序集中定义，则此类型名称必须是完全程序集限定的。这是负责渲染 renderClass 的对象 *类型*。 |
| renderedClass  | 必需的属性。值必须是此渲染器的目标类型的类型名称。如果类型未在 log4net 程序集中定义，则此类型名称必须是完全程序集限定的。这是此渲染器将渲染的类型的名称。 |

< renderer>元素不支持子元素。

#### （9）Parameter

参数元素可以是许多元素的子元素。有关详细信息，请参阅上面的特定元素。

一个示例参数：

```xml
<param name="ConversionPattern" value="%date [%thread] %-5level %logger [%ndc] - %message%newline" /> 
```

<param>元素支持 以下属性。

| 属性  | 描述                                                         |
| :---- | :----------------------------------------------------------- |
| name  | 必需的属性。值必须是要在父对象上设置的参数的名称。           |
| value | 可选属性。必须指定*值*或*类型*属性之一。该属性的值是一个可以转换为参数值的字符串。 |
| type  | 可选属性。必须指定*值*或*类型*属性之一。此属性的值是要创建并设置为参数值的类型名称。如果类型未在 log4net 程序集中定义，则此类型名称必须是完全程序集限定的。 |

<param>元素支持 以下子元素：

| 元素  | 描述                               |
| :---- | :--------------------------------- |
| param | 允许零个或多个元素。参数具体参数。 |

使用嵌套参数元素的示例参数：

```xml
<param name="evaluator" type="log4net.spi.LevelEvaluator">
    <param name="Threshold" value="WARN"/>
<param>
```

**扩展参数**

配置参数直接映射到对象的可写属性。可用的属性取决于正在配置的对象的实际类型。log4net SDK 文档包含 log4net 程序集中包含的所有组件的 API 参考。

**紧凑参数语法**

所有参数可以交替使用参数名称作为元素名称而不是使用*param*元素和*名称*属性来指定。

例如一个参数：

```xml
<param name="evaluator" type="log4net.spi.LevelEvaluator">
    <param name="Threshold" value="WARN"/>
<param>
```

可以写成：

```xml
<evaluator type="log4net.spi.LevelEvaluator">
    <threshold value="WARN"/>
<evaluator>
```

### 9、配置案例

#### （1）AdoNetAppender--SQL Server

> 数据库表定义

```sql
CREATE TABLE [dbo].[Log] (
    [Id] [int] IDENTITY (1, 1) NOT NULL,
    [Date] [datetime] NOT NULL,
    [Thread] [varchar] (255) NOT NULL,
    [Level] [varchar] (50) NOT NULL,
    [Logger] [varchar] (255) NOT NULL,
    [Message] [varchar] (4000) NOT NULL,
    [Exception] [varchar] (2000) NULL
)
```

> appender配置
>
> ConnectionType：指定用于连接数据库的System.Data.IDbConnection的完全限定类型名称。
>
> *ConnectionString*：数据库连接字符串。
>
> CommandText：命令执行语句。

```xml
<appender name="AdoNetAppender" type="log4net.Appender.AdoNetAppender">
    <bufferSize value="100" />
    <connectionType value="System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
    <connectionString value="data source=[database server];initial catalog=[database name];integrated security=false;persist security info=True;User ID=[user];Password=[password]" />
    <commandText value="INSERT INTO Log ([Date],[Thread],[Level],[Logger],[Message],[Exception]) VALUES (@log_date, @thread, @log_level, @logger, @message, @exception)" />
    <parameter>
        <parameterName value="@log_date" />
        <dbType value="DateTime" />
        <layout type="log4net.Layout.RawTimeStampLayout" />
    </parameter>
    <parameter>
        <parameterName value="@thread" />
        <dbType value="String" />
        <size value="255" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%thread" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@log_level" />
        <dbType value="String" />
        <size value="50" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%level" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@logger" />
        <dbType value="String" />
        <size value="255" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%logger" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@message" />
        <dbType value="String" />
        <size value="4000" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%message" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@exception" />
        <dbType value="String" />
        <size value="2000" />
        <layout type="log4net.Layout.ExceptionLayout" />
    </parameter>
</appender>
```

#### （2）AdoNetAppender--Access

> appender配置

```xml
<appender name="AdoNetAppender_Access" type="log4net.Appender.AdoNetAppender">
    <connectionString value="Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\log\access.mdb;User Id=;Password=;" />
    <commandText value="INSERT INTO Log ([Date],[Thread],[Level],[Logger],[Message]) VALUES (@log_date, @thread, @log_level, @logger, @message)" />
    <parameter>
        <parameterName value="@log_date" />
        <dbType value="String" />
        <size value="255" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%date" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@thread" />
        <dbType value="String" />
        <size value="255" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%thread" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@log_level" />
        <dbType value="String" />
        <size value="50" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%level" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@logger" />
        <dbType value="String" />
        <size value="255" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%logger" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@message" />
        <dbType value="String" />
        <size value="1024" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%message" />
        </layout>
    </parameter>
</appender>
```

#### （3）AdoNetAppender--Oracle9i

> 数据库表定义

```sql
create table log (
   Datetime timestamp(3),
   Thread varchar2(255),
   Log_Level varchar2(255),
   Logger varchar2(255),
   Message varchar2(4000)
   );
```

> appender配置

```xml
<appender name="AdoNetAppender_Oracle" type="log4net.Appender.AdoNetAppender">
    <connectionType value="System.Data.OracleClient.OracleConnection, System.Data.OracleClient, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
    <connectionString value="data source=[mydatabase];User ID=[user];Password=[password]" />
    <commandText value="INSERT INTO Log (Datetime,Thread,Log_Level,Logger,Message) VALUES (:log_date, :thread, :log_level, :logger, :message)" />
    <bufferSize value="128" />
    <parameter>
        <parameterName value=":log_date" />
        <dbType value="DateTime" />
        <layout type="log4net.Layout.RawTimeStampLayout" />
    </parameter>
    <parameter>
        <parameterName value=":thread" />
        <dbType value="String" />
        <size value="255" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%thread" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value=":log_level" />
        <dbType value="String" />
        <size value="50" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%level" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value=":logger" />
        <dbType value="String" />
        <size value="255" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%logger" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value=":message" />
        <dbType value="String" />
        <size value="4000" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%message" />
        </layout>
    </parameter>
</appender>
```

#### （4）AdoNetAppender--Oracle8i

> 数据库表定义

```sql
CREATE TABLE CSAX30.LOG
(
      THREAD      VARCHAR2(255),
      LOG_LEVEL   VARCHAR2(255),
      LOGGER      VARCHAR2(255),
      MESSAGE     VARCHAR2(4000)
)
TABLESPACE CSAX30D LOGGING
```

> appender配置

```xml
<appender name="AdoNetAppender_Oracle" type="log4net.Appender.AdoNetAppender">
    <connectionType value ="System.Data.OracleClient.OracleConnection, System.Data.OracleClient, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
    <connectionString value="data source=<dsname>;User ID=<userid>;Password=<password>" />
    <commandText value="INSERT INTO Log (Log_Level,Logger,Message) VALUES (:log_level, :logger, :message)" />
    <bufferSize value="250" />
    <parameter>
        <parameterName value=":log_level" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%level" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value=":logger" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%logger" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value=":message" />
        <dbType value="String" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%message" />
        </layout>
    </parameter>
</appender>
```

#### （5）AdoNetAppender--IBM DB2

> 数据库表定义

```sql
CREATE TABLE "myschema.LOG" (
    "ID" INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY (
        START WITH +1
        INCREMENT BY +1
        MINVALUE +1
        MAXVALUE +2147483647
        NO CYCLE
        NO CACHE
        NO ORDER
    ),
    "DATE" TIMESTAMP NOT NULL,
    "THREAD" VARCHAR(255) NOT NULL,
    "LEVEL" VARCHAR(500) NOT NULL,
    "LOGGER" VARCHAR(255) NOT NULL,
    "MESSAGE" VARCHAR(4000) NOT NULL,
    "EXCEPTION" VARCHAR(2000)
)
IN "LRGTABLES";
```

> appender配置

```xml
<appender name="AdoNetAppender" type="log4net.Appender.AdoNetAppender">
    <bufferSize value="100" />
    <connectionType value="IBM.Data.DB2.DB2Connection,IBM.Data.DB2, Version=8.1.2.1" />
    <connectionString value="server=192.168.0.0;database=dbuser;user Id=username;password=password;persist security info=true" />
    <commandText value="INSERT INTO myschema.Log (Date,Thread,Level,Logger,Message,Exception) VALUES (@log_date,@thread,@log_level,@logger,@message,@exception)" />
    <parameter>
        <parameterName value="@log_date" />
        <dbType value="DateTime" />
        <layout type="log4net.Layout.RawTimeStampLayout" />
    </parameter>
    <parameter>
        <parameterName value="@thread" />
        <dbType value="String" />
        <size value="255" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%thread" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@log_level" />
        <dbType value="String" />
        <size value="500" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%level" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@logger" />
        <dbType value="String" />
        <size value="255" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%logger" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@message" />
        <dbType value="String" />
        <size value="4000" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%message" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@exception" />
        <dbType value="String" />
        <size value="2000" />
        <layout type="log4net.Layout.ExceptionLayout" />
    </parameter>
</appender>
```

#### （6）AdoNetAppender--SQLite

> 数据库表定义

```sql
CREATE TABLE Log (
    LogId        INTEGER PRIMARY KEY,
    Date        DATETIME NOT NULL,
    Level        VARCHAR(50) NOT NULL,
    Logger        VARCHAR(255) NOT NULL,
    Message        TEXT DEFAULT NULL
);
```

> appender配置

```xml
<appender name="AdoNetAppender" type="log4net.Appender.AdoNetAppender">
    <bufferSize value="100" />
    <connectionType value="Finisar.SQLite.SQLiteConnection, SQLite.NET, Version=0.21.1869.3794, Culture=neutral, PublicKeyToken=c273bd375e695f9c" />
    <connectionString value="Data Source=c:\\inetpub\\wwwroot\\logs\\log4net.db;Version=3;" />
    <commandText value="INSERT INTO Log (Date, Level, Logger, Message) VALUES (@Date, @Level, @Logger, @Message)" />
    <parameter>
        <parameterName value="@Date" />
        <dbType value="DateTime" />
        <layout type="log4net.Layout.RawTimeStampLayout" />
    </parameter>
    <parameter>
        <parameterName value="@Level" />
        <dbType value="String" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%level" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@Logger" />
        <dbType value="String" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%logger" />
        </layout>
    </parameter>
    <parameter>
        <parameterName value="@Message" />
        <dbType value="String" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%message" />
        </layout>
    </parameter>
</appender>
```

#### （7）AspNetTraceAppender

> 以下示例显示如何配置AspNetTraceAppender 以将消息记录到 ASP.NET TraceContext。如果消息低于WARN级别，则将消息写入 System.Web.TraceContext.Write方法。如果它们是WARN或更高，它们将被写入 System.Web.TraceContext.Warn方法。

```xml
<appender name="AspNetTraceAppender" type="log4net.Appender.AspNetTraceAppender" >
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

#### （8）BufferingForwardingAppender

> 以下示例显示了如何配置BufferingForwardingAppender 以在将 100 条消息传递到*ConsoleAppender*之前对其进行缓冲。

```xml
<appender name="BufferingForwardingAppender" type="log4net.Appender.BufferingForwardingAppender" >
    <bufferSize value="100"/>
    <appender-ref ref="ConsoleAppender" />
</appender>
```

> 此示例显示如何仅交付重要事件。LevelEvaluator 的阈值为WARN。这意味着只有在记录级别为WARN或更高级别的消息时才会传递。缓存 512条消息。

```xml
<appender name="BufferingForwardingAppender" type="log4net.Appender.BufferingForwardingAppender" >
    <bufferSize value="512" />
    <lossy value="true" />
    <evaluator type="log4net.Core.LevelEvaluator">
        <threshold value="WARN"/>
    </evaluator>
    <appender-ref ref="ConsoleAppender" />
</appender>
```

#### （9）ColoredConsoleAppender

> 以下示例显示如何配置ColoredConsoleAppender 以将消息记录到控制台。默认情况下，消息被发送到控制台标准输出流。此示例显示如何突出显示消息。

```xml
<appender name="ColoredConsoleAppender" type="log4net.Appender.ColoredConsoleAppender">
    <mapping>
        <level value="ERROR" />
        <foreColor value="White" />
        <backColor value="Red, HighIntensity" />
    </mapping>
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

> 此示例显示如何为多个级别着色。

```xml
<appender name="ColoredConsoleAppender" type="log4net.Appender.ColoredConsoleAppender">
    <mapping>
        <level value="ERROR" />
        <foreColor value="White" />
        <backColor value="Red, HighIntensity" />
    </mapping>
    <mapping>
        <level value="DEBUG" />
        <backColor value="Green" />
    </mapping>
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

#### （10）ConsoleAppender

> 以下示例显示如何配置ConsoleAppender 以将消息记录到控制台。默认情况下，消息被发送到控制台标准输出流。

```xml
<appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender">
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

> 此示例显示如何将日志消息定向到控制台错误流。

```xml
<appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender">
    <target value="Console.Error" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

#### （11）EventLogAppender

> 以下示例显示如何配置EventLogAppender以使用AppDomain.FriendlyName的事件*源记录到本地计算机上的**应用程序*事件日志。

```xml
<appender name="EventLogAppender" type="log4net.Appender.EventLogAppender" >
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

> 此示例显示如何配置EventLogAppender以使用特定事件*Source*。

```xml
<appender name="EventLogAppender" type="log4net.Appender.EventLogAppender" >
    <applicationName value="MyApp" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

#### （12）FileAppender

> 以下示例显示如何配置FileAppender 以将消息写入文件。指定的文件是*log-file.txt*。每次记录过程开始时，该文件将被附加而不是覆盖。

```xml
<appender name="FileAppender" type="log4net.Appender.FileAppender">
    <file value="log-file.txt" />
    <appendToFile value="true" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

> 此示例说明如何使用环境变量*TMP*配置要写入的文件名。还指定了用于写入文件的编码。

```xml
<appender name="FileAppender" type="log4net.Appender.FileAppender">
    <file value="${TMP}\log-file.txt" />
    <appendToFile value="true" />
    <encoding value="unicodeFFFE" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

> 这个例子展示了如何配置 appender 以使用允许多个进程写入同一个文件的最小锁定模型。

```xml
<appender name="FileAppender" type="log4net.Appender.FileAppender">
    <file value="${TMP}\log-file.txt" />
    <appendToFile value="true" />
    <lockingModel type="log4net.Appender.FileAppender+InterProcessLock" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

> 此示例显示如何配置附加程序以使用“进程间”锁定模型。

```xml
<appender name="FileAppender" type="log4net.Appender.FileAppender">
    <file value="${TMP}\log-file.txt" />
    <appendToFile value="true" />
    <lockingModel type="log4net.Appender.FileAppender+InterProcessLock" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

#### （13）ForwardingAppender

> 以下示例显示了如何配置ForwardingAppender。forwarding appender 允许使用一组约束来装饰 appender。在此示例中，*ConsoleAppender*使用级别为WARN的*阈值*进行装饰。这意味着直接指向*ConsoleAppender*的事件 将被记录，无论其级别如何，但指向*ForwardingAppender*的事件只有在其级别为WARN 时才会传递给*ConsoleAppender* 或更高。此附加程序仅在特殊情况下使用。

```xml
<appender name="ForwardingAppender" type="log4net.Appender.ForwardingAppender" >
    <threshold value="WARN"/>
    <appender-ref ref="ConsoleAppender" />
</appender>
```

#### （14）ManagedColoredConsoleAppender

> 以下示例显示如何配置ManagedColoredConsoleAppender 以将消息记录到控制台。默认情况下，消息被发送到控制台标准输出流。此示例显示如何突出显示错误消息。

```xml
<appender name="ManagedColoredConsoleAppender" type="log4net.Appender.ManagedColoredConsoleAppender">
    <mapping>
        <level value="ERROR" />
        <foreColor value="White" />
        <backColor value="Red" />
    </mapping>
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

> 此示例显示如何为多个级别着色。

```xml
<appender name="ManagedColoredConsoleAppender" type="log4net.Appender.ManagedColoredConsoleAppender">
    <mapping>
        <level value="ERROR" />
        <foreColor value="DarkRed" />
    </mapping>
    <mapping>
        <level value="WARN" />
        <foreColor value="Yellow" />
    </mapping>
    <mapping>
        <level value="INFO" />
        <foreColor value="White" />
    </mapping>
    <mapping>
        <level value="DEBUG" />
        <foreColor value="Blue" />
    </mapping> 
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date %-5level %-20.20logger: %message%newline"/>
    </layout>
</appender>
```

#### （15）MemoryAppender

> MemoryAppender 不太可能使用配置文件进行配置，但如果你想这样做，这里是如何做到的。

```xml
<appender name="MemoryAppender" type="log4net.Appender.MemoryAppender">
    <onlyFixPartialEventData value="true" />
</appender>
```

#### （16）NetSendAppender

> 以下示例显示如何配置NetSendAppender 以将消息传递到特定用户的屏幕。由于此附加程序通常仅用于重要通知，因此指定了级别错误的*阈值* 。此示例将消息传递给机器*SQUARE上的用户**nicko*。然而，使用 Windows Messenger 服务并不总是直截了当，使用此配置的一个可能结果是*服务器* 将广播寻找 WINS 服务器，然后它会要求将消息传递给*收件人*，WINS 服务器会将其传递给用户登录的第一个终端。

```xml
<appender name="NetSendAppender" type="log4net.Appender.NetSendAppender">
    <threshold value="ERROR" />
    <server value="SQUARE" />
    <recipient value="nicko" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

#### （17）OutputDebugStringAppender

> 以下示例显示如何配置OutputDebugStringAppender 以将日志消息写入OutputDebugString API。

```xml
<appender name="OutputDebugStringAppender" type="log4net.Appender.OutputDebugStringAppender" >
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

#### （18）RemotingAppender

> 以下示例显示了如何配置RemotingAppender 以将日志事件传递到指定的*Sink*（在此示例中，Sink 为tcp://localhost:8085/LoggingSink）。*在此示例中，由于BufferSize*，事件以 95 个事件块的形式传递。不会丢弃任何事件。OnlyFixPartialEventData选项允许*附加* 程序忽略某些生成速度可能非常慢的日志事件属性（例如调用位置信息）。

```xml
<appender name="RemotingAppender" type="log4net.Appender.RemotingAppender" >
    <sink value="tcp://localhost:8085/LoggingSink" />
    <lossy value="false" />
    <bufferSize value="95" />
    <onlyFixPartialEventData value="true" />
</appender>
```

> 此示例将RemotingAppender配置为仅在记录级别为ERROR 或更高级别的事件时才传递事件。传递事件时，将传递多达 200 ( *BufferSize* ) 个先前的事件（无论级别如何）以提供上下文。未传递的事件将被丢弃。

```xml
<appender name="RemotingAppender" type="log4net.Appender.RemotingAppender" >
    <sink value="tcp://localhost:8085/LoggingSink" />
    <lossy value="true" />
    <bufferSize value="200" />
    <onlyFixPartialEventData value="true" />
    <evaluator type="log4net.Core.LevelEvaluator">
        <threshold value="ERROR"/>
    </evaluator>
</appender>
```

#### （19）RollingFileAppender

> RollingFileAppender建立在 FileAppender之上 ，并具有与 appender 相同的选项。
>
> 以下示例显示如何配置RollingFileAppender 以写入文件*log.txt*。写入的文件将始终称为*log.txt* ，因为指定了*StaticLogFileName*参数。该文件将根据大小约束 ( *RollingStyle* ) 滚动。最多将保留 10 个 ( *MaxSizeRollBackups* ) 每个 100 KB ( *MaximumFileSize* ) 的旧文件。这些滚动文件将被命名为：*log.txt.1*、*log.txt.2*、*log.txt.3*等...

```xml
<appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
    <file value="log.txt" />
    <appendToFile value="true" />
    <rollingStyle value="Size" />
    <maxSizeRollBackups value="10" />
    <maximumFileSize value="100KB" />
    <staticLogFileName value="true" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

> 此示例显示如何配置RollingFileAppender 以在日期期间滚动日志文件。此示例将每分钟滚动一次日志文件！要更改滚动周期，请调整DatePattern值。例如，“yyyyMMdd”的日期模式将每天滚动。有关可用模式的列表，请参阅System.Globalization.DateTimeFormatInfo。

```xml
<appender name="RollingLogFileAppender" type="log4net.Appender.RollingFileAppender">
    <file value="logfile" />
    <appendToFile value="true" />
    <rollingStyle value="Date" />
    <datePattern value="yyyyMMdd-HHmm" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

> 这个例子展示了如何配置RollingFileAppender 在一个日期周期和一个日期周期内滚动日志文件的文件大小。每天只保留最后 10 个 1MB 的文件。

```xml
<appender name="RollingLogFileAppender" type="log4net.Appender.RollingFileAppender">
    <file value="logfile" />
    <appendToFile value="true" />
    <rollingStyle value="Composite" />
    <datePattern value="yyyyMMdd" />
    <maxSizeRollBackups value="10" />
    <maximumFileSize value="1MB" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

> 此示例显示如何配置RollingFileAppender 以在每次程序执行时滚动一次日志文件。appendToFile 属性设置为false以防止 appender 覆盖现有文件。maxSizeRollBackups设置为负 1 以允许无限数量的备份文件。文件大小确实必须受到限制，但这里设置为 50 GB，如果日志文件在单次运行期间超过此大小限制，那么它也会被滚动。

```xml
<appender name="RollingLogFileAppender" type="log4net.Appender.RollingFileAppender">
    <file value="logfile.txt" />
    <appendToFile value="false" />
    <rollingStyle value="Size" />
    <maxSizeRollBackups value="-1" />
    <maximumFileSize value="50GB" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

#### （20）SmtpAppender

> 以下示例显示如何配置SmtpAppender 通过 SMTP 电子邮件传递日志事件。*To*、*From*、*Subject*和SmtpHost *是*必需参数。此示例显示如何仅交付重要事件。LevelEvaluator 的阈值为WARN。这意味着将为每个记录的WARN或更高级别的消息发送一封电子邮件。每封电子邮件还将包含多达 512 个（*BufferSize*) 任何级别的先前消息以提供上下文。未发送的消息将被丢弃。

```xml
<appender name="SmtpAppender" type="log4net.Appender.SmtpAppender">
    <to value="to@domain.com" />
    <from value="from@domain.com" />
    <subject value="test logging message" />
    <smtpHost value="SMTPServer.domain.com" />
    <bufferSize value="512" />
    <lossy value="true" />
    <evaluator type="log4net.Core.LevelEvaluator">
        <threshold value="WARN"/>
    </evaluator>
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%newline%date [%thread] %-5level %logger [%property{NDC}] - %message%newline%newline%newline" />
    </layout>
</appender>
```

> 这个例子展示了如何配置SmtpAppender 来传递电子邮件中的所有消息，每封电子邮件有 512 ( *BufferSize* ) 消息。

```xml
<appender name="SmtpAppender" type="log4net.Appender.SmtpAppender">
    <to value="to@domain.com" />
    <from value="from@domain.com" />
    <subject value="test logging message" />
    <smtpHost value="SMTPServer.domain.com" />
    <bufferSize value="512" />
    <lossy value="false" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%newline%date [%thread] %-5level %logger [%property{NDC}] - %message%newline%newline%newline" />
    </layout>
</appender>
```

> 此示例显示了邮件消息的更详细的格式布局。

```xml
<appender name="SmtpAppender" type="log4net.Appender.SmtpAppender,log4net">
    <to value="to@domain.com" />
    <from value="from@domain.com" />
    <subject value="test logging message" />
    <smtpHost value="SMTPServer.domain.com" />
    <bufferSize value="512" />
    <lossy value="false" />
    <evaluator type="log4net.Core.LevelEvaluator,log4net">
        <threshold value="WARN" />
    </evaluator>
    <layout type="log4net.Layout.PatternLayout,log4net">
        <conversionPattern value="%property{log4net:HostName} :: %level :: %message %newlineLogger: %logger%newlineThread: %thread%newlineDate: %date%newlineNDC: %property{NDC}%newline%newline" />
    </layout>
</appender>
```

#### （21）SmtpPickupDirAppender

> SmtpPickupDirAppender的配置与 SmtpAppender类似。唯一的区别是必须指定 *PickupDir*而不是指定*SmtpHost*参数。
>
> *PickupDir*参数是一个必须存在 的路径，并且执行 appender 的代码必须有权在此目录中创建新文件并写入它们。该路径相对于应用程序的基本目录 ( AppDomain.BaseDirectory )。
>
> 以下示例显示如何配置SmtpPickupDirAppender 以通过 SMTP 电子邮件传递日志事件。*To*、*From*、*Subject*和PickupDir *是*必需参数。此示例显示如何仅交付重要事件。LevelEvaluator 的阈值为WARN。这意味着将为每个记录的WARN或更高级别的消息发送一封电子邮件。每封电子邮件还将包含多达 512 个（*BufferSize*) 任何级别的先前消息以提供上下文。未发送的消息将被丢弃。

```xml
<appender name="SmtpPickupDirAppender" type="log4net.Appender.SmtpPickupDirAppender">
    <to value="to@domain.com" />
    <from value="from@domain.com" />
    <subject value="test logging message" />
    <pickupDir value="C:\SmtpPickup" />
    <bufferSize value="512" />
    <lossy value="true" />
    <evaluator type="log4net.Core.LevelEvaluator">
        <threshold value="WARN"/>
    </evaluator>
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%newline%date [%thread] %-5level %logger [%property{NDC}] - %message%newline%newline%newline" />
    </layout>
</appender>
```

#### （22）TraceAppender

> 以下示例显示如何配置TraceAppender 以将消息记录到System.Diagnostics.Trace系统。这是随 .net 基类库提供的跟踪系统。 有关如何配置跟踪系统的更多详细信息， 请参阅System.Diagnostics.Trace类的 MSDN 文档。

```xml
<appender name="TraceAppender" type="log4net.Appender.TraceAppender">
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

#### （23）UdpAppender

> 以下示例显示如何配置UdpAppender 以将事件发送到指定*RemotePort上的**RemoteAddress*。

```xml
<appender name="UdpAppender" type="log4net.Appender.UdpAppender">
    <localPort value="8080" />
    <remoteAddress value="224.0.0.1" />
    <remotePort value="8080" />
    <layout type="log4net.Layout.PatternLayout, log4net">
        <conversionPattern value="%-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
</appender>
```

#### （24）DynamicPatternLayout

> 每当页眉或页脚应包含可能随时间变化的信息时，都应使用 DynamicPatternLayout 。与不会在每次调用时重新评估的静态PatternLayout相比， DynamicPatternLayout确实会在每次调用时 重新评估模式。例如，它确实允许在页眉和/或页脚中包含当前的 DateTime ，而静态PatternLayout是不可能的。
>
> 以下示例显示了如何配置DynamicPatternLayout。

```xml
<layout type="log4net.Layout.DynamicPatternLayout"> 
  <param name="Header" value="%newline**** Trace Opened Local: %date{yyyy-MM-dd HH:mm:ss.fff} UTC: %utcdate{yyyy-MM-dd HH:mm:ss.fff} ****%newline"/> 
  <param name="Footer" value="**** Trace Closed %date{yyyy-MM-dd HH:mm:ss.fff} ****%newline"/> 
</layout>
```

### 10、PatterLayout格式化

| 转换字符   | 效果                                                         |
| ---------- | ------------------------------------------------------------ |
| a          | 等价于appdomain                                              |
| appdomain  | 引发日志事件的应用程序域的友好名称。（使用中一般是可执行文件的名字。） |
| c          | 等价于 logger                                                |
| C          | 等价于 type                                                  |
| class      | 等价于 type                                                  |
| d          | 等价于 date                                                  |
| date       | 发生日志事件的本地时间。 使用 DE>%utcdate 输出UTC时间。date后面还可以跟一个日期格式，用大括号括起来。DE>例如：%date{HH:mm:ss,fff}或者%date{dd MMM yyyy HH:mm:ss,fff}。如果date后面什么也不跟，将使用ISO8601 格式 。日期格式和.Net中DateTime类的ToString方法中使用的格式是一样。另外log4net还有3个自己的格式Formatter。 它们是 "ABSOLUTE", "DATE"和"ISO8601"分别代表 AbsoluteTimeDateFormatter, DateTimeDateFormatter和Iso8601DateFormatter。例如：%date{ISO8601}或%date{ABSOLUTE}。它们的性能要好于ToString。 |
| exception  | 异常信息日志事件中必须存了一个异常对象，如果日志事件不包含没有异常对象，将什么也不输出。异常输出完毕后会跟一个换行。一般会在输出异常前加一个换行，并将异常放在最后。 |
| F          | 等价于 file                                                  |
| file       | 发生日志请求的源代码文件的名字。警告：只在调试的时候有效。调用本地信息会影响性能。 |
| identity   | 当前活动用户的名字(Principal.Identity.Name).警告：会影响性能。（我测试的时候%identity返回都是空的。） |
| l          | 等价于 location                                              |
| L          | 等价于 line                                                  |
| location   | 引发日志事件的方法（包括命名空间和类名），以及所在的源文件和行号。警告：会影响性能。没有pdb文件的话，只有方法名，没有源文件名和行号。 |
| level      | 日志事件等级                                                 |
| line       | 引发日志事件的行号警告：会影响性能。                         |
| logger     | 记录日志事件的Logger对象的名字。可以使用精度说明符控制Logger的名字的输出层级，默认输出全名。注意，精度符的控制是从右开始的。例如：logger 名为 "a.b.c"， 输出模型为%logger{2} ，将输出"b.c"。 |
| m          | 等价于 message                                               |
| M          | 等价于 method                                                |
| message    | 由应用程序提供给日志事件的消息。                             |
| mdc        | MDC (旧为：ThreadContext.Properties) 现在是事件属性的一部分。 保留它是为了兼容性，它等价于 property。 |
| method     | 发生日志请求的方法名（只有方法名而已）。警告：会影响性能。   |
| n          | 等价于 newline                                               |
| newline    | 换行符                                                       |
| ndc        | NDC (nested diagnostic context)                              |
| p          | 等价于 level                                                 |
| P          | 等价于 property                                              |
| properties | 等价于 property                                              |
| property   | 输出事件的特殊属性。例如： %property{user} 输出user属性。属性是由loggers或appenders添加到时间中的。 有一个默认的属性"DE>log4net:HostName"总是会有。DE>%property将输出所有的属性 。（扩展后可以使用） |
| r          | 等价于 timestamp                                             |
| t          | 等价于 thread                                                |
| timestamp  | 从程序启动到事件发生所经过的毫秒数。                         |
| thread     | 引发日志事件的线程，如果没有线程名就使用线程号。             |
| type       | 引发日志请求的类的全名。.可以使用精度控制符。例如： 类名是 "log4net.Layout.PatternLayout", 格式模型是%type{1} 将输出"PatternLayout"。（也是从右开始的。）警告：会影响性能。 |
| u          | 等价于 identity                                              |
| username   | 当前用户的WindowsIdentity。（类似：HostName/Username）警告：会影响性能。 |
| utcdate    | 发生日志事件的UTC时间。DE>后面还可以跟一个日期格式，用大括号括起来。DE>例如：%utcdate{HH:mm:ss,fff}或者%utcdate{dd MMM yyyy HH:mm:ss,fff}。如果utcdate后面什么也不跟，将使用ISO8601 格式 。日期格式和.Net中DateTime类的ToString方法中使用的格式是一样。另外log4net还有3个自己的格式Formatter。 它们是 "ABSOLUTE", "DATE"和"ISO8601"分别代表 AbsoluteTimeDateFormatter, DateTimeDateFormatter和Iso8601DateFormatter。例如：%date{ISO8601}或%date{ABSOLUTE}。它们的性能要好于ToString。 |
| w          | 等价于 username                                              |
| x          | 等价于 ndc                                                   |
| X          | 等价于 mdc                                                   |
| %          | %%输出一个百分号                                             |
