# Nlog详解

## 一、Nlog详解

### 1、概述

NLog是一个基于.NET平台编写的日志记录类库，我们可以使用NLog在应用程序中添加极为完善的跟踪调试代码。可以在任何一种.NET语言中输出带有上下文的（contextual information）调试诊断信息，根据喜好配置其表现样式之后发送到一个或多个输出目标（target）中。

官网地址：[https://nlog-project.org/](https://nlog-project.org/)

文档地址：[https://github.com/NLog/NLog/wiki](https://github.com/NLog/NLog/wiki)

### 2、快速开始

#### （1）nuget引入程序集

```bash
NLog  版本基于5.0版，Net5
```

#### （2）代码实现

```C#
//创建一个配置文件对象
var config = new NLog.Config.LoggingConfiguration();
//创建日志写入目的地
var logfile = new NLog.Targets.FileTarget("logfile") { FileName = $"logs/{DateTime.Now.ToString("yyyy-MM-dd")}.txt" };
//添加日志路由规则
config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
//配置文件生效
LogManager.Configuration = config;
//创建日志记录对象
Logger Logger = NLog.LogManager.GetCurrentClassLogger();
//打出日志
Logger.Debug("我打出了Nlog日志！");
```

#### （3）运行结果

![image-20220619151929015](http://cdn.bluecusliyou.com/202206191519108.png)

### 3、支持文件配置

#### （1）nuget引入程序集

```bash
NLog
NLog.Config  配置文件
```

#### （2）配置文件

> 引入NLog.Config之后，项目会自动添加一个NLog.Config配置文件，这个是默认的配置文件，这个文件不在项目文件夹下需要复制过来，配置文件右键属性一定要设置成`始终复制`。
>
> 程序运行的时候，会自动加载NLog.Config作为Nlog的配置。
>
> XML文件中有详细说明，rules配置路由规则, targets配置输出目标。

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
      autoReload="true"
      throwExceptions="false"
      internalLogLevel="Off" internalLogFile="c:\temp\nlog-internal.log">

  <!-- optional, add some variables
  https://github.com/nlog/NLog/wiki/Configuration-file#variables
  -->
  <variable name="myvar" value="myvalue"/>

  <!--
  See https://github.com/nlog/nlog/wiki/Configuration-file
  for information on customizing logging rules and outputs.
   -->
  <targets>

    <!--
    add your targets here
    See https://github.com/nlog/NLog/wiki/Targets for possible targets.
    See https://github.com/nlog/NLog/wiki/Layout-Renderers for the possible layout renderers.
    -->

    <!--
    Write events to a file with the date in the filename.
    <target xsi:type="File" name="f" fileName="${basedir}/logs/${shortdate}.log"
            layout="${longdate} ${uppercase:${level}} ${message}" />
    -->
  </targets>

  <rules>
    <!-- add your logging rules here -->

    <!--
    Write all events with minimal level of Debug (So Debug, Info, Warn, Error and Fatal, but not Trace)  to "f"
    <logger name="*" minlevel="Debug" writeTo="f" />
    -->
  </rules>
</nlog>
```

> 将注释去掉，最精简版本如下

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" >
  <targets>
	<!--输出目标:name名称f,xsi:type输出类型文件, fileName输出到程序根目录logs文件夹中, 以日期作为生成log文件名称, layout生成内容的格式-->
    <target name="f"
			xsi:type="File"
			fileName="${basedir}/logs/${shortdate}.log"
            layout="${longdate} ${uppercase:${level}} ${message}" />
  </targets>
  <rules>
	  <!--日志路由规则：最低级别Debug，输出到target目标f-->
    <logger name="*" minlevel="Debug" writeTo="f" />
  </rules>
</nlog>
```

#### （3）代码实现

```C#
//创建日志记录对象
Logger Logger = NLog.LogManager.GetCurrentClassLogger();
//打出日志
Logger.Debug("我打出了Nlog日志！");
```

#### （4）运行结果

![image-20220619152433357](http://cdn.bluecusliyou.com/202206191524420.png)

### 4、配置文件详解

#### （1）全局配置

```xml
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
      autoReload="true"
      throwExceptions="false"
      internalLogLevel="Off" internalLogFile="c:\temp\nlog-internal.log">
```

> 建议throwExceptions的值设为“false”，这样由于日志引发的问题不至于导致应用程序的崩溃。

```bash
autoReload 修改配置文件后是否允许自动加载无须重启程序
throwExceptions 内部日志系统抛出异常
internalLogLevel 可选Trace|Debug|Info|Warn|Error|Fatal决定内部日志的级别 Off 关闭
internalLogFile 把内部的调试和异常信息都写入指定文件里
```

#### （2）根元素

> 在配置文件的根元素中，我们可以指定如下的子元素。其中前两个是必须设定的，其余三个为可选设定。
>
> 1. targets：定义日志的输出目标
> 2. rules：定义对日志信息的路由规则
> 3. extensions：定义从其他dll文件中加载的NLog扩展模块
> 4. include：引入外部的配置文件
> 5. variable：定义配置文件中用到的变量

#### （3）targets定义日志的输出目标

```xml
<targets>
    <target name="f"
			xsi:type="File"
			fileName="${basedir}/logs/${shortdate}.log"
            layout="${longdate} ${uppercase:${level}} ${message}" 
            maxArchiveFiles="5"
        	archiveAboveSize="10240"
        	archiveEvery="Day"/>
</targets>
```

> Nlog允许用户配置单个文件大小, 放置在内容过长效率过慢,配置了大小之后, Nlog会自动创建一个新的文件副本,插入新的日志输出。
>
> maxArchiveFiles：允许生成的副本文件最大数量
>
> archiveAboveSize：允许单个文件得最大容量
>
> archiveEvery：按天生成

> name：输出目标的名称，用于rules中路由规则writeTo指定

> fileName：包含完整的生成文件的路径和文件名

> xsi:type：输出类型

```bash
Chainsaw
ColoredConsole 
Console
Database
Debug
Debugger
EventLog
File
LogReceiverService
Mail
Memory
MethodCall
Network
NLogViewer
Null
OutputDebugString
PerfCounter
Trace
WebService
```

> layout：用来规定输出内容格式，语法“${属性}”，可以把上下文信息插入到日志中。

```bash
$ {cached} -  将缓存应用于另一个布局输出。
$ {db-null} - 为数据库渲染DbNull
$ {exception} - 通过调用Logger方法之一提供的异常信息
$ {level} - 日志级别（例如ERROR，DEBUG）或级别序数（数字）
$ {literal} - 字符串文字。（文本）-有用，以逃避括号
$ {logger} - 记录器名称。GetLogger，GetCurrentClassLogger等
$ {message} - （格式化的）日志消息。
$ {newline} - 换行文字。
$ {object-path} -  渲染对象的（嵌套）属性
$ {onexception} -  仅在为日志消息定义了异常时才输出内部布局。
$ {var} - 渲染变量
呼叫站点和堆栈跟踪-------------------------------------------------------------
$ {callsite} - 调用站点（类名，方法名和源信息）
$ {callsite-linenumber} - 呼叫站点源行号。
$ {stacktrace} - 渲染堆栈跟踪
条件------------------------------------------------------------------------
$ {when} -  仅在满足指定条件时输出内部布局。
$ {whenempty} -  当内部布局产生空结果时，输出替代布局。
上下文信息-------------------------------------------------------------------
$ {activityid} - 将System.Diagnostics跟踪关联ID记录到日志中。
$ {all-event-properties} - 记录所有事件上下文数据。
$ {event-context} -  记录事件属性数据-替换为$ {event-properties}
$ {event-properties} - 记录事件属性数据-重命名$ {event-context}
$ {gdc} - 全局诊断上下文项。包含每个应用程序实例值的字典结构。
$ {install-context} - 安装参数（传递给InstallNLogConfig）。
$ {mdc} - 映射诊断上下文-线程局部结构。
$ {mdlc} - 异步映射诊断上下文-线程局部结构。MDC的异步版本
$ {ndc} - 嵌套诊断上下文-线程局部结构。
$ {ndlc} - 异步嵌套诊断上下文-线程本地结构。
专柜-----------------------------------------------------------------------
$ {counter} - 一个计数器值（在每个布局渲染中增加）
$ {guid} - 全局唯一标识符（GUID）。
$ {sequenceid} - 日志序列号
日期和时间------------------------------------------------------------------
$ {date} - 当前日期和时间。
$ {longdate} - 日期和时间，采用可排序的长格式`yyyy-MM-dd HH：mm：ss.ffff`。
$ {qpc} - 高精度计时器，基于QueryPerformanceCounter返回的值。
$ {shortdate} - 短日期，格式为yyyy-MM-dd。
$ {ticks} - 当前日期和时间的“ Ticks”值。
$ {时间} - 在24小时，可排序的格式HH的时间：MM：ss.mmm。
编码和字符串转换--------------------------------------------------------------
$ {json-encode} -  使用JSON规则转义另一个布局的输出。
$ {left} -  文字的左半部分
$ {小写} -  将另一个布局输出的结果转换为小写。
$ {norawvalue} -  防止将另一个布局渲染器的输出视为原始值
$ {pad} -  将填充应用于另一个布局输出。
$ {replace} -  将另一个布局的输出中的字符串替换为另一个字符串。正则表达式可选
$ {replace-newlines} -  用另一个字符串替换换行符。
$ {right} -  文字的右侧
$ {rot13} -  使用ROT-13解码“加密”的文本。
$ {substring} -  文本的子字符串
$ {trim-whitespace} -  从另一个布局渲染器的结果修剪空白。
$ {uppercase} -  将另一个布局输出的结果转换为大写。
$ {url-encode} -  编码另一个布局输出的结果，以供URL使用。
$ {wrapline} -  以指定的行长包装另一个布局输出的结果。
$ {xml-encode} -  将另一个布局输出的结果转换为XML兼容的。
环境和配置文件----------------------------------------------------------------
$ {appsetting} -. config文件 NLog.Extended中的应用程序配置设置
$ {configsetting} - 来自appsettings.json或ASP.NET Core和.NET Core中其他配置的值 NLog.Extensions.Logging NLog.Extensions.Hosting NLog.Web.AspNetCore
$ {environment} - 环境变量。（例如PATH，OSVersion）
$ {environment-user} - 用户身份信息（用户名）。
$ {}注册表 - 从Windows注册表中的值。
文件和目录--------------------------------------------------------------------
$ {basedir} - 当前应用程序域的基本目录。
$ {currentdir} - 应用程序的当前工作目录。
$ {file-contents} - 呈现指定文件的内容。
$ {filesystem-normalize} -  通过将文件名替换为安全字符来过滤文件名中不允许的字符。
$ {} nlogdir - 其中NLog.dll所在的目录。
$ {specialfolder} - 系统专用文件夹路径（包括“我的文档”，“我的音乐”，“程序文件”，“桌面”等）。
$ {tempdir} - 临时目录。
身分识别----------------------------------------------------------------------
$ {identity} - 线程身份信息（名称和身份验证信息）。
$ {windows-identity} - 线程Windows身份信息（用户名）
$ {windows-identity} - 线程Windows身份信息（用户名） Nlog.WindowsIdentity
整合方式----------------------------------------------------------------------
$ {gelf} - 将日志转换为GELF格式 NLog.GelfLayout 外部
$ {log4jxmlevent} - 与log4j，Chainsaw和NLogViewer兼容的XML事件描述。
进程，线程和程序集--------------------------------------------------------------
$ {appdomain} - 当前的应用程序域。
$ {assembly-version} - 默认应用程序域中可执行文件的版本。
$ {gc} - 有关垃圾收集器的信息。
$ {hostname} - 运行该进程的计算机的主机名。
$ {local-ip} - 来自网络接口的本地IP地址。
$ {machinename} - 运行进程的计算机名。
$ {performancecounter} - 性能计数器。
$ {processid} - 当前进程的标识符。
$ {processinfo} - 有关正在运行的进程的信息。例如StartTime，PagedMemorySize
$ {processname} - 当前进程的名称。
$ {processtime} - 格式为HH：mm：ss.mmm的处理时间。
$ {threadid} - 当前线程的标识符。
$ {threadname} - 当前线程的名称。
Silverlight------------------------------------------------------------------------
$ {document-uri} - 承载当前Silverlight应用程序的HTML页面的URI。
$ {sl-appinfo} - 有关Silverlight应用程序的信息。
Web，ASP.NET和ASP.NET Core----------------------------------------------------------
$ {ASPNET-appbasepath} - ASP.NET应用程序的基本路径（内容根） NLog.Web NLog.Web.AspNetCore
$ {ASPNET应用} - ASP.NET应用程序变量。 网络日志
$ {ASPNET环境} - ASP.NET环境名称 NLog.Web.AspNetCore
$ {ASPNET项} - ASP.NET`HttpContext`项变量。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET-MVC-行动} - ASP.NET MVC动作名称 NLog.Web NLog.Web.AspNetCore
$ {ASPNET-MVC控制器} - ASP.NET MVC控制器名称 NLog.Web NLog.Web.AspNetCore
$ {ASPNET请求} - ASP.NET请求变量。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET-请求的contentType} - ASP.NET Content-Type头（实施例应用/ JSON） NLog.Web.AspNetCore
$ {ASPNET请求，饼干} - ASP.NET请求的cookie的内容。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET请求形式} - ASP.NET请求表的内容。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET请求报头} - ASP.NET部首键/值对。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET请求主机} - ASP.NET请求主机。 NLog.Web NLog.Web.AspNetCore
$ {aspnet-request-ip} - 客户端IP。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET请求-方法} - ASP.NET请求方法（GET，POST等）。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET请求，贴体} - ASP.NET贴体/净荷 NLog.Web NLog.Web.AspNetCore
$ {ASPNET-请求的查询字符串} - ASP.NET请求的查询字符串。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET，请求引荐} - ASP.NET请求引用。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET请求的URL} - ASP.NET请求URL。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET请求，用户代理} - ASP.NET请求用户代理。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET-响应的StatusCode} - ASP.NET响应状态代码的内容。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET会话} - ASP.NET Session变量。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET-的SessionID} - ASP.NET会话ID的变量。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET-traceidentifier} - ASP.NET跟踪标识 NLog.Web NLog.Web.AspNetCore
$ {ASPNET用户-的authType} - ASP.NET用户验证。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET用户身份} - ASP.NET用户变量。 NLog.Web NLog.Web.AspNetCore
$ {ASPNET用户-isauthenticated} - ASP.NET用户身份验证？ NLog.Web NLog.Web.AspNetCore
$ {ASPNET-webrootpath} - ASP.NET Web根目录路径（wwwroot文件） NLog.Web NLog.Web.AspNetCore
$ {iis-site-name} - IIS网站的名称。 NLog.Web NLog.Web.AspNetCore
```

#### （4）rules定义对日志信息的路由规则

> 路由顺序会对日志打印产生影响。路由匹配逻辑为顺序匹配。
>
> 日志可以分不同级别进行输出，日志规则rules里面可以控制输出的日志级别。不同级别的日志代表日志的重要程度，比如一些debug级别的日志在生产环境就会被控制不输出，以减少日志文件的大小。

```bash
<rules>
    <logger name="*" minlevel="Debug" writeTo="file" />
</rules>
```

> name：记录者的名字。

> minlevel ：最低日志级别。

> maxlevel：最高日志级别。

> level：单一日志级别。

> levels：一系列日志级别，由逗号分隔。

> final：是否是最后的匹配路由，true表示匹配到这里就结束。

> writeTo：规则匹配时日志应该被写入的一系列目标，由逗号分隔。就是tagets对应的name。

> 日志级别有如下，自上而下，等级递增。

```bash
- Trace - 最常见的记录信息，一般用于普通输出
- Debug - 同样是记录信息，不过出现的频率要比Trace少一些，一般用来调试程序
- Info - 信息类型的消息
- Warn - 警告信息，一般用于比较重要的场合
- Error - 错误信息
- Fatal - 致命异常信息。一般来讲，发生致命异常之后程序将无法继续执行。
```

> 日志过滤器：可以在路由当中, 为每个路由配置自定义得日志过滤器fliter，如下所示

```xml
<rules>
    <logger name="*" writeTo="file">
        <filters>
            <when condition="length('${message}') > 100" action="Ignore" />
            <when condition="equals('${logger}','MyApps.SomeClass')" action="Ignore" />
            <when condition="(level >= LogLevel.Debug and contains('${message}','PleaseDontLogThis'))" action="Ignore" />
            <when condition="not starts-with('${message}','PleaseLogThis')" action="Ignore" />
        </filters>
    </logger>
</rules>
```

> 日志过滤器--条件语言
>
> 过滤器表达式以特殊的迷你语言编写。该语言包括：
> 关系运算符：==，!=，<，<=，>=和>
> 注意：一些预先定义的XML字符可能需要转义。例如，如果尝试使用'<'字符，则XML解析器会将其解释为开始标记，这会导致配置文件中的错误。而是<在这种情况下使用转义版本的<<（（））。
> 布尔运算符：and，or，not
> 始终被视为布局的字符串文字- ${somerenderer}
> 布尔文字- true和false
> 数值文字-例如12345（整数文字）和12345.678（浮点文字）
> 日志级别文字- LogLevel.Trace，LogLevel.Debug，...LogLevel.Fatal
> 预定义的关键字来访问最常用的日志事件属性- level，message和logger
> 花括号-一起覆盖默认优先级和分组表达式
> 条件函数-执行string和object测试
> 单引号应与另一个单引号转义。

> 日志过滤器--条件函数
>
> contains(s1,s2)确定第二个字符串是否是第一个的子字符串。返回：true当第二个字符串是第一个字符串的子字符串时，false否则返回。
> ends-with(s1,s2)确定第二个字符串是否是第一个字符串的后缀。返回：true当第二个字符串是第一个字符串的前缀时，false否则返回。
> equals(o1,o2)比较两个对象是否相等。返回：true当两个对象相等时，false否则返回。
> length(s) 返回字符串的长度。
> starts-with(s1,s2)确定第二个字符串是否是第一个字符串的前缀。返回：true当第二个字符串是第一个字符串的前缀时，false否则返回。
> regex-matches(input, pattern, options)在NLog 4.5中引入。指示正则表达式是否pattern在指定的input字符串中找到匹配项。options是一个可选的逗号分隔的RegexOptions枚举值列表。返回：true当在输入字符串中找到匹配项时，false否则返回

### 5、输出到多种存储介质

#### （1）配置文件

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" >
	<targets>
		<!--输出目标:name名称f,xsi:type输出类型文件, fileName输出到程序根目录logs文件夹中, 以日期作为生成log文件名称, layout生成内容的格式-->
		<target name="f"
				xsi:type="File"
				fileName="D:/logs/Nlog/${shortdate}/nlog.log"
				layout="${longdate} ${uppercase:${level}} ${message}"
				archiveAboveSize="102400"
				/>
		<!--输出到控制台-->
		<target name="c"
			  xsi:type="Console"
			  layout="${longdate} ${uppercase:${level}} ${message}" />

		<!--输出到csv文件-->
		<target name="e" xsi:type="File" fileName="D:/logs/Nlog/${shortdate}/nlog.csv">
			<layout xsi:type="CSVLayout">
				<column name="time" layout="${longdate}" />
				<column name="message" layout="${message}" />
				<column name="logger" layout="${logger}"/>
				<column name="level" layout="${level}"/>
			</layout>
		</target>
	</targets>
	<rules>
		<!--日志路由规则：最低级别Debug，输出到target目标f-->
		<logger name="*" minlevel="Debug" writeTo="f,c,e" />
	</rules>
</nlog>
```

#### （2）代码实现

```C#
Logger Logger = NLog.LogManager.GetCurrentClassLogger();
for (int i = 0; i < 10_000; i++)
{
    Logger.Debug($"我打出了Nlog日志！--{i.ToString()}");
}
```

#### （3）运行结果

> 日志文件超过设定大小就会增加新文件，这样后面查看的时候就不会因为日志文件太大而无法打开。名称后面的数字是按照内容从早到晚进行从小到大增长的，没有数字的那个文件始终是最新的日志内容。

![image-20220620145803220](http://cdn.bluecusliyou.com/202206201458332.png)

### 6、输出到数据库

#### （1）nuget引入程序集

```bash
NLog
NLog.Config
NLog.Database   NLog5.0+必须单独添加数据库支持
MySql.Data      数据库驱动
```

#### （2）安装数据库，创建数据库和表

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

> 创建数据库logmanager

> 创建表nlog

```sql
CREATE TABLE `nlog` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Application` varchar(50) DEFAULT NULL,
  `Logged` datetime DEFAULT NULL,
  `Level` varchar(50) DEFAULT NULL,
  `Message` varchar(512) DEFAULT NULL,
  `Logger` varchar(250) DEFAULT NULL,
  `Callsite` varchar(512) DEFAULT NULL,
  `Exception` varchar(512) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
```

#### （3）配置文件

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
       autoReload="true"
      throwExceptions="true"
      internalLogLevel="Off">
	<targets>
		<!--输出到数据库-->
		<target name="d" xsi:type="Database"
              dbProvider="MySql.Data.MySqlClient.MySqlConnection, MySql.Data"
              connectionString="server=服务器IP地址;Database=logmanager;user id=root;password=123456;SslMode=none">
			<commandText>
				insert into nlog (
				Application, Logged, Level, Message,
				Logger, CallSite, Exception
				) values (
				@Application, @Logged, @Level, @Message,
				@Logger, @Callsite, @Exception
				);
			</commandText>
			<parameter name="@application" layout="MyNlog" />
			<parameter name="@logged" layout="${date}" />
			<parameter name="@level" layout="${level}" />
			<parameter name="@message" layout="${message}" />
			<parameter name="@logger" layout="${logger}" />
			<parameter name="@callSite" layout="${callsite:filename=true}" />
			<parameter name="@exception" layout="${exception:tostring}" />
		</target>
	</targets>
	<rules>		
		<logger name="*" minlevel="Debug" writeTo="d" />
	</rules>
</nlog>
```

#### （4）代码实现

```C#
//创建日志记录对象
Logger Logger = NLog.LogManager.GetCurrentClassLogger();
//打出日志
Logger.Debug("我打出了Nlog日志！");
```

#### （5）运行结果

![image-20220621145850481](http://cdn.bluecusliyou.com/202206211458670.png)

### 7、集成到Asp.NetCore5框架

#### （1）Nuget引入程序集

```bash
NLog
NLog.Config
NLog.Web.AspNetCore  集成框架
```

#### （2）配置文件

> 放到文件夹CfgFile统一管理，配置文件右键属性`始终复制`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
      autoReload="true"
      throwExceptions="false"
      internalLogLevel="Off" internalLogFile="c:\temp\nlog-internal.log">
	<targets>
		<target xsi:type="File" name="f" fileName="D:/logs/Nlog/${shortdate}/nlog.log"
				layout="${longdate} ${uppercase:${level}} ${message}" />
	</targets>
	<rules>
		<!--Skip Microsoft logs and so log only own logs-->
		<!--以Microsoft打头的日志将进入此路由，由于此路由没有writeTo属性，所有会被忽略-->
		<!--且此路由设置了final，所以此路由被匹配到了不会再继续往下匹配。未匹配到的会继续匹配下一个路由-->
		<logger name="Microsoft.*" minlevel="Trace"  final="true" />
		<logger name="*" minlevel="Debug" writeTo="f" />
	</rules>
</nlog>

```

#### （3）Program添加对Nlog支持

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
                logBuilder.AddNLog("CfgFile/NLog.config");//支持nlog
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

![image-20220621154208469](http://cdn.bluecusliyou.com/202206211542542.png)

### 8、集成到Asp.NetCore6框架

#### （1）Nuget引入程序集

```bash
NLog
NLog.Config
NLog.Web.AspNetCore  集成框架
```

#### （2）配置文件

> 放到文件夹CfgFile统一管理，配置文件右键属性`始终复制`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
      autoReload="true"
      throwExceptions="false"
      internalLogLevel="Off" internalLogFile="c:\temp\nlog-internal.log">
	<targets>
		<target xsi:type="File" name="f" fileName="D:/logs/Nlog/${shortdate}/nlog.log"
				layout="${longdate} ${uppercase:${level}} ${message}" />
	</targets>
	<rules>
		<!--Skip Microsoft logs and so log only own logs-->
		<!--以Microsoft打头的日志将进入此路由，由于此路由没有writeTo属性，所有会被忽略-->
		<!--且此路由设置了final，所以此路由被匹配到了不会再继续往下匹配。未匹配到的会继续匹配下一个路由-->
		<logger name="Microsoft.*" minlevel="Trace"  final="true" />
		<logger name="*" minlevel="Debug" writeTo="f" />
	</rules>
</nlog>
```

#### （3）Program添加对Nlog支持

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
            logBuilder.AddNLog("CfgFile/NLog.config");//支持nlog
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
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

![image-20220621154326725](http://cdn.bluecusliyou.com/202206211543789.png)
