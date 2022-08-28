# Quartz.Net详解

## 一、QuartZ.Net详解（3.X）

### 1、概述

#### （1）背景

业务中总是会有需要定时执行的任务，我们可以用timer实现最简单的定时需求，也可以借助Quartz.NET框架实现复杂定时任务的功能。

```C#
//2秒后每隔3秒执行一次，传入参数"1"
Timer timer = new Timer((n) =>
{
    Console.WriteLine("我是定时器中的业务逻辑{0}", n);
}, "1", 2000, 3000);
```

System.Timers.Timer 类具有“内置”定时器功能，为什么有人会使用 Quartz 而不是这些标准功能？主要有几个原因：

- 定时器没有持久化机制。
- 定时器具有不灵活的调度（只能设置开始时间和重复间隔，不能基于日期、时间等）。
- 定时器不使用线程池（每个定时器一个线程）
- 定时器没有真正的管理方案——你必须编写自己的机制来记忆、组织和检索任务名称等。

#### （2）概述

- Quartz.NET 是一个功能齐全的开源作业调度系统，可用于从最小的应用程序到大型企业系统。

- Quartz 非常灵活，包含多个可以单独或一起使用的使用范例，以实现您想要的行为，并使您能够以对您的项目最“自然”的方式编写代码。

- Quartz 非常轻巧，需要很少的设置/配置 - 如果您的需求相对基本，它实际上可以“开箱即用”使用。

- Quartz 是容错的，并且可以在系统重新启动之间保留（“记住”）您计划的作业。

- 尽管 Quartz 对于在给定的时间表上简单地运行某些系统进程非常有用，但是当您学习如何使用它来驱动应用程序的业务流程流时，可以充分发挥 Quartz 的潜力。

- 官网：[https://www.quartz-scheduler.net/](https://www.quartz-scheduler.net/)

- 源码：[https://github.com/quartznet/quartznet](https://github.com/quartznet/quartznet)

- 文档地址：[https://www.quartz-scheduler.net/documentation/](https://www.quartz-scheduler.net/documentation/)

- API文档地址：https://quartznet.sourceforge.io/apidoc/3.0/html/


#### （3）特性

- 运行时环境：可以嵌入在应用程序中运行，甚至可以作为独立程序集群实例化（具有负载平衡和故障转移功能）
- 作业调度：作业被安排在给定触发器发生时运行，触发器支持多种调度选项
- 作业执行：作业可以是任何实现简单 IJob 接口的 .NET 类，从而为作业可以执行的工作留下无限可能
- 工作持久化：可以实现作业存储以提供各种存储作业的机制，开箱即用地支持内存和多个关系数据库
- 故障转移：内置支持负载平衡您的工作和优雅的故障转移
- 监听器和插件：应用程序可以通过实现一个或多个侦听器接口来捕获调度事件以监视或控制作业/触发行为。

#### （4）Quartz五大元素

- Scheduler：调度器，quartz工作时的独立容器
- Trigger：触发器，定义了调度任务的时间规则—定义什么时间去执行
- Job：调度的任务---具体要做的什么事儿---刷数据库的数据
- ThreadPool：线程池（不是clr中的线程池），任务最终交给线程池中的线程执行
- JobStore：RAWStore和DbStore两种，job和trigger都存放在JobStore中

#### （4）工作流程

​        scheduler是quartz的独立运行容器，trigger和job都可以注册在scheduler容器中，一个job可以有多个触发器，而一个触发器只能属于一个job。

​         Quartz中有一个调度线程QuartzSchedulerThread，调度线程可以找到将要被触发的trigger和job，然后在ThreadPool中获取一个线程来执行这个job。

​         JobStore主要作用是存放job和trigger的信息。

### 2、快速开始

#### （1）引入nuget包

```bash
Quartz  基于3.4.0 Net6
```

#### （2）创建一个作业类，继承`IJob`接口

```C#
public class MyJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"【任务执行】：{DateTime.Now}");
        Console.WriteLine($"【触发时间】：{context.ScheduledFireTimeUtc?.LocalDateTime}");
        Console.WriteLine($"【下次触发时间】：{context.NextFireTimeUtc?.LocalDateTime}"); 
        await Task.CompletedTask;
    }
}
```

#### （3）创建调度器，作业，触发器

```C#
//实例化调度器
IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

//开启调度器
scheduler.Start();

//创建一个作业
IJobDetail job1 = JobBuilder.Create<MyJob>()
    .WithIdentity("job1", "groupa")//名称，分组
    .Build();

//创建一个触发器
ITrigger trigger1 = TriggerBuilder.Create()
    .WithIdentity("trigger1", "groupa")//名称，分组
    .StartNow()//从启动的时候开始执行
    .WithSimpleSchedule(b =>
    {
        b.WithIntervalInSeconds(2)//2秒执行一次
         .WithRepeatCount(3);//重复执行3+1次
    })
    .Build();

//把作业，触发器加入调度器
scheduler.ScheduleJob(job1, trigger1);
```

#### （4）运行结果

```bash
【任务执行】：2022/8/11 13:47:02
【触发时间】：2022/8/11 13:47:02
【下次触发时间】：2022/8/11 13:47:04
【任务执行】：2022/8/11 13:47:04
【触发时间】：2022/8/11 13:47:04
【下次触发时间】：2022/8/11 13:47:06
【任务执行】：2022/8/11 13:47:06
【触发时间】：2022/8/11 13:47:06
【下次触发时间】：2022/8/11 13:47:08
【任务执行】：2022/8/11 13:47:08
【触发时间】：2022/8/11 13:47:08
【下次触发时间】：
```

### 3、时间类型

#### （1）时间类型

- DateTime：表示的时区有限，国内采用这个时间。
- DateTimeOffset：可以表示任何时区，通过偏移量来控制。**（Quartz中提供DateBuilder类来创建DateTimeOffset类型）**

#### （2）两种类型相互转换

- DateTime→DateTimeOffset 利用DateTimeOffset的构造函数
- DateTimeOffset→DateTime 利用Convert.ToDateTime方法

```C#
DateTime date1 = DateTime.Parse("2022-01-01 12:00:00");
DateTimeOffset date2 = DateBuilder.DateOf(12, 00, 00, 1, 1, 2022);
//DateTime 转换成 DateTimeOffset
DateTimeOffset date3 = new DateTimeOffset(date1, TimeSpan.Zero);
//DateTimeOffset 转换成 DateTime
DateTime date4 = Convert.ToDateTime(date2);
```

#### （3）一些常用时间表示API

```C#
//表示固定时间
DateTime date5 = DateTime.Parse("2022-01-01 12:00:00");
DateTime date6 = new DateTime(2022, 1, 1, 12, 0, 0);
DateTimeOffset date7 = DateBuilder.DateOf(12, 00, 00, 1, 1, 2022);
//2022-01-01 12:00:00  往后增加6天5小时4分3秒
DateTimeOffset date8 = new DateTimeOffset(12, 00, 00, 1, 1, 2022, new TimeSpan(6,5,4,3));
//今天的3点2分1秒
DateTimeOffset date9=DateBuilder.TodayAt(3,2,1);
//明天的3点2分1秒
DateTimeOffset date10 = DateBuilder.TomorrowAt(3, 2, 1);

//四舍五入
DateTimeOffset date11 = DateBuilder.TodayAt(6, 5, 4);
DateTimeOffset date12 = DateBuilder.EvenHourDate(date11);           //小时维度上入：7:00:00
DateTimeOffset date13 = DateBuilder.EvenHourDateBefore(date11);     //小时维度上舍：6:00:00

//时间周期
//第一个参数传入null以当前时间为依据，假设当前时间为：14:43:29
//第一个参数传入时间以传入时间为基准
//第二个参数传入10表示以整10分钟作为一个周期，10,20,30,40,50,60
//第二个参数传入20表示以整20分钟作为一个周期，20,40,60
DateTimeOffset date14 = DateBuilder.NextGivenMinuteDate(null, 10);                              //14:50:00
DateTimeOffset date15 = DateBuilder.NextGivenMinuteDate(null, 20);                              //15:00:00
DateTimeOffset date16 = DateBuilder.NextGivenMinuteDate(DateBuilder.TodayAt(1, 45, 30), 10);    //1:50:00

//增加时间
DateTime date17 = DateTime.Now.AddYears(1);//当前时间+1年
DateTime date18 = DateTime.Now.AddMonths(1);//当前时间+1月
DateTime date19 = DateTime.Now.AddDays(1);//当前时间+1天
DateTime date20 = DateTime.Now.AddHours(1);//当前时间+1小时
DateTime date21 = DateTime.Now.AddMinutes(1);//当前时间+1分钟
DateTime date22 = DateTime.Now.AddSeconds(1);//当前时间+1秒
```

### 4、调度器Scheduler

#### （1）创建方式

- 直接通过StdSchedulerFactory类的GetDefaultScheduler方法创建
- 先创建StdSchedulerFactory，然后通过GetScheduler方法创建。该方式可以在实体化StdSchedulerFactory的时候配置一些额外的信息，比如：配置SimpleThreadPool的个数、RemoteScheduler的远程控制、数据库的持久化等。
- 通过SchedulerBuilder.Create()创建。
- 通过DirectSchedulerFactory创建，需要传入线程池对象和jobstore对象，配置硬编码。

```C#
//方式1
IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
//方式2
ISchedulerFactory schedulefactory = new StdSchedulerFactory();
IScheduler scheduler2 = schedulefactory.GetScheduler().Result;
//方式2 传入参数
NameValueCollection pars = new NameValueCollection
{
    //scheduler名字
    ["quartz.scheduler.instanceName"] = "MySchedulerAdvanced",
    //线程池个数
    ["quartz.threadPool.threadCount"] = "20"
};
ISchedulerFactory schedulefactory2 = new StdSchedulerFactory(pars);
IScheduler scheduler3 = schedulefactory2.GetScheduler().Result;
//方式3
IScheduler scheduler4 = SchedulerBuilder.Create().BuildScheduler().Result;
//方式4
var serializer = new JsonObjectSerializer();
serializer.Initialize();
JobStoreTX jobStore = new JobStoreTX
{
    DataSource = "default",
    TablePrefix = "QRTZ_",
    InstanceId = "AUTO",
    DriverDelegateType = typeof(MySQLDelegate).AssemblyQualifiedName,
    ObjectSerializer = serializer,
};
DirectSchedulerFactory.Instance.CreateScheduler("myScheduler", "AUTO", new DefaultThreadPool(), jobStore);
IScheduler scheduler5 = await SchedulerRepository.Instance.Lookup("myScheduler");
```

#### （2）单例封装

单例封装，可以控制所有地方操作的都是同一个实例。

```C#
public class MySchedulerFactory
{
    /// <summary>
    /// 由CLR保证，在程序第一次使用该类之前被调用，而且只调用一次
    /// </summary>
    private static IScheduler _Scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
    public static IScheduler GetScheduler()
    {
        return _Scheduler;
    }
}
```

#### （3）常用方法

- 开启：Start
- 关闭：ShutDown
- 暂停job或Trigger：PauseAll、PauseJob、PauseJobs、PauseTrigger、PauseTriggers
- 恢复job或Trigger：ResumeAll、ResumeJob、ResumeJobs、ResumeTrigger、ResumeTriggers
- 将job和trigger加入Scheduler中：ScheduleJob
- 添加Job：AddJob

```C#
//常用API
//实例化调度器
IScheduler scheduler =MySchedulerFactory.GetScheduler();

//开启调度器
scheduler.Start();

//创建一个作业
IJobDetail job1 = JobBuilder.Create<MyJob>()
    .WithIdentity("job1", "groupa")//名称，分组
    .Build();

//创建一个触发器
ITrigger trigger1 = TriggerBuilder.Create()
    .WithIdentity("trigger1", "groupa")//名称，分组
    .StartNow()//从启动的时候开始执行
    .WithSimpleSchedule(b =>
    {
        b.WithIntervalInSeconds(2)//2秒执行一次
         .WithRepeatCount(3);//重复执行3+1次
    })
    .Build();

//把作业，触发器加入调度器
scheduler.ScheduleJob(job1, trigger1);

//添加作业
scheduler.AddJob(job1,true);

//暂停作业
scheduler.PauseJobs(GroupMatcher<JobKey>.GroupEquals("groupa"));

//恢复作业
scheduler.ResumeJobs(GroupMatcher<JobKey>.GroupEquals("groupa"));

//停止调度
scheduler.Shutdown();
```

#### （4）日志记录

从 Quartz.NET 3.1 开始可以配置`Microsoft.Extensions.Logging.Abstractions` 用来代替 LibLog。

Quartz.NET 使用`LibLog` 库来满足其日志记录需求。Quartz 不会产生太多的日志信息，通常只是初始化期间的一些信息，然后只有在 Jobs 执行时记录严重问题的消息。为了调整日志设置（例如输出量和输出去向），需要配置选择的日志框架，因为 LibLog 主要将工作委托给更成熟的日志框架，如 log4net， serilog 等。

**手动配置**

```C#
// obtain your logger factory, for example from IServiceProvider
ILoggerFactory loggerFactory = ...;

// Quartz 3.1
Quartz.LogContext.SetCurrentLogProvider(loggerFactory);

// Quartz 3.2 onwards
Quartz.Logging.LogContext.SetCurrentLogProvider(loggerFactory);
```

**使用 Microsoft DI 集成进行配置**

```C#
services.AddQuartz(q =>
{
    // this automatically registers the Microsoft Logging
});
```

### 5、作业Job

#### （1）几个重要类型

- JobBuilder：用来创建JobDetail。
- IJob：具体作业任务需要实现该接口，并实现里面的方法
- IJobDetail：用来定义工作实例，添加到调度器中运行

您可以创建单个作业类实现`IJob`接口，并通过创建多个 JobDetails 实例，每个都有自己的一组属性和 JobDataMap，并将它们全部添加到调度程序。

如下，SalesReportJob实现IJob接口，创建了两个JobDetails实例reportForJoe，reportForMike，两个任务运行互不影响。

```C#
IJobDetail reportForJoe = JobBuilder.Create<SalesReportJob>()
	.WithIdentity("myJob", "group1")
    .UsingJobData("salename", "Joe")
	.Build();

IJobDetail reportForMike = JobBuilder.Create<SalesReportJob>()
	.WithIdentity("myJob", "group1")
    .UsingJobData("salename", "Mike")
	.Build();
```

#### （2）IJobDetail两种创建方式

- Create的泛型方式：写起来代码简洁方便。
- 反射+OfType的方式：用于实现动态绑定，通过程序集的反射。

```C#
//方式1
IJobDetail job1 = JobBuilder.Create<MyJob>()
    .WithIdentity("job1", "groupa")//名称，分组
    .Build();

//方式2
var type = Assembly.Load("MyQuartZ.Net.QuartZJob").CreateInstance("MyJob");
IJobDetail job2 = JobBuilder.Create().OfType(type.GetType())
    .WithIdentity("job2", "groupa")//名称，分组
    .Build();
```

#### （3）常用方法

- UsingJobData：给Job添加一些附加值，存储在JobDataMap里，可以在具体的Job中获取。（通过context.JobDetail.JobDataMap获取）
- StoreDurably：让该job持久化，不被销毁.(默认情况下为false，即job没有对应的trigger的话，job就被销毁)
- WithIdentity：身份标记，给job起个名称，便于和Trigger关联的时候使用.
- WithDescription：用来对job进行描述，并没有什么实际作用

#### （4）JobDataMap

`JobDataMap`可用于保存任何数量的（可序列化的）对象，您希望在作业实例执行时可以使用这些对象。`JobDataMap`是`IDictionary`接口的一个实现，并且增加了一些方便的方法来存储和检索原始类型的数据。

以下是在将作业添加到调度程序之前将数据放入 JobDataMap 的一些快速片段：

```C#
IJobDetail job = JobBuilder.Create<DumbJob>()
	.WithIdentity("myJob", "group1") 
	.UsingJobData("jobSays", "Hello World!")
	.UsingJobData("myFloatValue", 3.141f)
	.Build();
```

下面是一个在作业执行期间从 JobDataMap 获取数据的快速示例：

```C#
public class DumbJob : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		JobKey key = context.JobDetail.Key;
		JobDataMap dataMap = context.JobDetail.JobDataMap;
		string jobSays = dataMap.GetString("jobSays");
		float myFloatValue = dataMap.GetFloat("myFloatValue");
		await Console.Error.WriteLineAsync("Instance " + key + " of DumbJob says: " + jobSays + ", and val is: " + myFloatValue);
	}
}
```

#### （5）防止作业完成后被删除

设置属性 JobDetail.Durable = true - 指示 Quartz 在 Job 成为“孤儿”时不要删除 Job（当 Job 不再有 Trigger 引用它时）。

#### （6）作业状态和并发

`[DisallowConcurrentExecution]`是一个可以添加到 Job 类的特性，它告诉 Quartz 不要同时执行给定作业定义（引用给定作业类）的多个实例。请注意措辞，在上一节的示例中，如果“SalesReportJob”具有此属性，则在给定时间只能执行一个“SalesReportForJoe”实例，但它可以与“SalesReportForMike”实例同时执行。

`[PersistJobDataAfterExecution]`是一个可以添加到 Job 类中的特性，它告诉 Quartz 在 Execute() 方法成功完成后（不抛出异常）更新 JobDetail 的 JobDataMap 的存储副本，以便下一次执行相同的作业（JobDetail）接收更新的值而不是最初存储的值。与`[DisallowConcurrentExecution]`属性一样，这适用于作业定义实例，而不是作业类实例。

如果您使用[PersistJobDataAfterExecution]特性，您应该强烈考虑使用`[DisallowConcurrentExecution]`特性，以避免在同时执行同一作业 (JobDetail) 的两个实例时可能会留下存储的数据的混淆（竞争条件）。

定义job类，标记特性，让job运行时间超过trigger的间隔时间

```C#
[DisallowConcurrentExecution, PersistJobDataAfterExecution]
public class ConcurrentJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        Thread.Sleep(3000);
        JobDataMap dataMap = context.JobDetail.JobDataMap;
        string testdata = dataMap.GetString("testdata");
        dataMap.Put("testdata", testdata+"1");
        await Console.Error.WriteLineAsync($"testdata：{testdata} time:{DateTime.Now.ToString()}");
    }
}
```

定义调度器，触发器

```C#
//实例化调度器工厂
ISchedulerFactory schedulefactory = new StdSchedulerFactory();
//实例化调度器
IScheduler scheduler = schedulefactory.GetScheduler().Result;
scheduler.Start();

//创建一个作业
IJobDetail job1 = JobBuilder.Create<ConcurrentJob>()
    .WithIdentity("job1", "groupa")//名称，分组
    .UsingJobData("testdata", "Hello World!")
    .Build();

//创建一个触发器
ITrigger trigger1 = TriggerBuilder.Create()
    .WithIdentity("trigger1", "groupa")//名称，分组
    .StartNow()//从启动的时候开始执行
    .WithSimpleSchedule(b =>
    {
        b.WithIntervalInSeconds(2)//2秒执行一次
         .WithRepeatCount(3);//重复执行3+1次
    })
    .Build();

//把作业，触发器加入调度器
scheduler.ScheduleJob(job1, trigger1);
```

运行效果

```bash
testdata：Hello World! time:2022/8/11 19:31:45
testdata：Hello World!1 time:2022/8/11 19:31:48
testdata：Hello World!11 time:2022/8/11 19:31:51
testdata：Hello World!111 time:2022/8/11 19:31:54
```

#### （7）作业执行异常

您应该从 execute 方法中抛出的唯一异常类型是 JobExecutionException。因此，您通常应该使用“try-catch”块包装执行方法的全部内容。

#### （8）停止正在执行的作业

`IJobExecutionContext`见`CancellationToken.IsCancellationRequested`

### 6、触发器Trigger

#### （1）几个重要的类

- TriggerBuilder：用来创建ITrigger实例
- ITrigger：触发器实例

#### （2）常用方法

- StartNow：Trigger马上触发.
- StartAt和EndAt：设置Trigger触发的开始时间和结束时间 （省略设置开始时间的话，默认从当前时间开始执行）
- UsingJobData：给Trigger添加一些附加值（通过context.Trigger.JobDataMap获取）
- WithDescription：用来描述该触发器，并没有什么实际左右
- WithPriority：设置Trigger的优先级，默认为5，数字越大，优先级越高.（该优先级用于一个job对应多个Trigger，且Trigger的触发时间相同，优先级越大的越先执行）
- ForJob：将job和trigger进行关联，该方法有多个重载，关联后ScheduleJob方法进行调度时，只需将trigger传入进去即可

#### （3）常用属性

- JobKey：指示触发器触发时应执行的作业的标识。
- StartTimeUtc：指示触发器的计划何时首次生效。该值是一个 DateTimeOffset 对象，用于定义给定日历日期的某个时刻。对于某些触发器类型，触发器实际上会在开始时间触发，而对于其他触发器类型，它只是标记应该开始遵循计划的时间。这意味着您可以在 1 月期间使用诸如“每月第 5 天”之类的计划存储触发器，并且如果 StartTimeUtc 属性设置为 4 月 1 日，则它将在第一次触发前几个月。
- EndTimeUtc：指示触发器的计划何时不再有效。换句话说，计划为“每月第 5 天”且结束时间为 7 月 1 日的触发器将在 6 月 5 日最后一次触发。

#### （4）优先级

当你有许多触发器同时触发，Quartz.NET 可能没有足够的资源来同时执行。在这种情况下，您可能希望控制哪些触发器优先获得可用的工作线程。

您可以在触发器上设置优先级，调度器首先执行优先级高的Trigger，默认优先级 5。优先级允许使用任何整数值，正数或负数。数字越大表示优先级越高。

仅当触发器具有相同的触发时间时才比较优先级。计划在 10:59 触发的触发器总是会在计划在 11:00 触发的触发器之前触发。

当触发器的作业需要恢复时，它的恢复将按照与原始触发器相同的优先级进行调度。

```C#
ITrigger trigger1 = TriggerBuilder.Create()
    .WithIdentity("trigger1", "groupa")//名称，分组
    .WithPriority(10)//设置优先级，默认5，数值越大优先级越高
    .WithSimpleSchedule(b =>
    {
        b.WithIntervalInSeconds(2);//2秒执行一次
    })
    .Build();
```

#### （5）SimpleTrigger

用途：时、分、秒上的轮询(和timer类似)，实际开发中，该场景占绝大多数。

执行间隔：

- WithInterval(TimeSpan timeSpan)：通用的间隔执行方法
- WithIntervalInHours(int hours)：以小时为间隔单位进行执行
- WithIntervalInMinutes(int minutes)：以分钟为间隔单位进行执行
- WithIntervalInSeconds(int seconds)：以秒为间隔单位进行执行

 执行时间：

- WithRepeatCount(int repeatCount)：执行多少次以后结束
- RepeatForever()：永远执行
- repeatMinutelyForever()：一分钟执行一次(永远执行)
- repeatMinutelyForever(int minutes)：每隔几分钟执行一次(永远执行)
- repeatMinutelyForTotalCount(int count, int minutes)：每隔几分钟执行一次(执行次数为count)类似的还有秒、小时。

**为特定时刻构建触发器，不重复：**

```C#
ISimpleTrigger trigger = (ISimpleTrigger) TriggerBuilder.Create()
    .WithIdentity("trigger1", "group1")
    .StartAt(myStartTime)
    .ForJob("job1", "group1") 
    .Build();
```

**为特定时刻构建触发器，然后每十秒重复十次：**

```C#
ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("trigger3", "group1")
    .StartAt(myStartTime)
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(10)
        .WithRepeatCount(10))
    .ForJob(myJob)     
    .Build();
```

**构建一个将在未来五分钟触发一次的触发器：**

```C#
ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("trigger5", "group1")
    .StartAt(DateBuilder.FutureDate(5, IntervalUnit.Minute))
    .ForJob(myJobKey)
    .Build();
```

**构建一个立即触发的触发器，然后每五分钟重复一次，直到 22:00 小时：**

```C#
ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("trigger7", "group1")
    .WithSimpleSchedule(x => x
        .WithIntervalInMinutes(5)
        .RepeatForever())
    .EndAt(DateBuilder.DateOf(22, 0, 0))
    .Build();
```

**构建一个将在下一小时开始触发的触发器，然后每 2 小时重复一次，直到永远：**

```C#
ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("trigger8")
    .StartAt(DateBuilder.EvenHourDate(null))
    .WithSimpleSchedule(x => x
        .WithIntervalInHours(2)
        .RepeatForever())
    .Build();
```

#### （6）CronTrigger

用途：使用cron表达式代替硬编码，可以代替其他类型的trigger

**构建一个触发器，该触发器将在每天上午 8 点到下午 5 点之间每隔一分钟触发一次：**

```C#
ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("trigger3", "group1")
    .WithCronSchedule("0 0/2 8-17 * * ?")
    .ForJob("myJob", "group1")
    .Build();
```

**构建一个每天上午 10:42 触发的触发器：**

```C#
ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("trigger3", "group1")
    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(10, 42))
    .ForJob(myJobKey)
    .Build();
```

**构建一个触发器，该触发器将在周三上午 10:42 触发，在系统默认的 TimeZone 中：**

```C#
ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("trigger3", "group1")
    .WithSchedule(CronScheduleBuilder
        .WeeklyOnDayAndHourAndMinute(DayOfWeek.Wednesday, 10, 42)
        .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time")))
    .ForJob(myJobKey)
    .Build();
```

或者 -

```C#
ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("trigger3", "group1")
    .WithCronSchedule("0 42 10 ? * WED", x => x
        .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time")))
    .ForJob(myJobKey)
    .Build();
```

#### （7）DailyTimeInterval

用途：解决时间点的增、减、排除。

核心函数：

- OnEveryDay：每天
- OnMondayThroughFriday:周一至周五,即工作日
- OnSaturdayAndSunday:周六至周天，即休息日
- OnDaysOfTheWeek:用数组的形式单独来指定一周中的哪几天
- StartingDailyAt：表示开始于几点 （区别于前面的StartAt）
- EndingDailyAt：表示结束于几点 （区别于前面的EndAt）

**实现周四周五的早上8点到晚上20点之间每隔2秒执行一次，一共执行4次**

```C#
ITrigger trigger1 = TriggerBuilder.Create()
    .WithIdentity("trigger1", "groupa")
    .WithDailyTimeIntervalSchedule(x =>
    {
        //周四和周五
        x.OnDaysOfTheWeek(new DayOfWeek[] { DayOfWeek.Thursday, DayOfWeek.Friday }) 
         .StartingDailyAt(TimeOfDay.HourMinuteAndSecondOfDay(8, 00, 00)) //8点开始
         .EndingDailyAt(TimeOfDay.HourMinuteAndSecondOfDay(20, 00, 00))  //20点结束
         .WithIntervalInSeconds(2) //两秒执行一次，可设置时分秒维度
         .WithRepeatCount(3);  //一共执行3+1次
    })
    .Build();
```

#### （8）CalendarInterval

用途：与日历相关

参数中的几个函数：

- WithInterval(TimeSpan timeSpan)：通用的间隔执行方法
- WithIntervalInHours(int hours)：以小时为间隔单位进行执行
- WithIntervalInMinutes(int minutes)：以分钟为间隔单位进行执行
- WithIntervalInSeconds(int seconds)：以秒为间隔单位进行执行
- WithIntervalInDays(int days)：以天为间隔单位进行执行
- WithIntervalInMonths(int months)：以月为间隔单位进行执行

```C#
ITrigger trigger1 = TriggerBuilder.Create()
    .WithIdentity("trigger1", "groupa")//名称，分组
    .StartNow()//从启动的时候开始执行
    .WithCalendarIntervalSchedule(x =>
    {
        x.WithIntervalInSeconds(3);//每3秒执行一次
    })
    .Build();
```

#### （9）Job和Trigger关联问题

- 1个job对应1个trigger：调用ScheduleJob(IJobDetail jobDetail, ITrigger trigger)，直接关联即可，无须做特别处理
- 1个job对应多个trigger： 将job持久化（StoreDurably(true)），然后通过AddJob方法加入调度池中，Trigger上通过ForJob方法和指定job进行关联，然后调用ScheduleJob(ITrigger trigger)方法，将trigger全部加入调度池中
- 2个job对应1个trigger (不常用)：利用JobChainingJobListener实现

```C#
{
    //job和trigger关联问题
    //1个job对应1个trigger
    //实例化调度器
    IScheduler scheduler = MySchedulerFactory.GetScheduler();

    //开启调度器
    scheduler.Start();

    //创建一个作业
    IJobDetail job1 = JobBuilder.Create<MyJob>()
        .WithIdentity("job1", "groupa")//名称，分组
        .Build();

    //创建一个触发器
    ITrigger trigger1 = TriggerBuilder.Create()
        .WithIdentity("trigger1", "groupa")//名称，分组
        .StartNow()//从启动的时候开始执行
        .WithSimpleSchedule(b =>
        {
            b.WithIntervalInSeconds(2)//2秒执行一次
             .WithRepeatCount(3);//重复执行3+1次
        })
        .Build();

    //把作业，触发器加入调度器
    scheduler.ScheduleJob(job1, trigger1);
}

{
    //job和trigger关联问题
    //1个job对应2个trigger
    //实例化调度器
    IScheduler scheduler = MySchedulerFactory.GetScheduler();

    //开启调度器
    scheduler.Start();

    //创建一个作业
    IJobDetail job1 = JobBuilder.Create<MyJob>()
        .WithIdentity("job1", "groupa")//名称，分组
        .StoreDurably(true)//持久化job
        .Build();

    //创建一个触发器1
    ITrigger trigger1 = TriggerBuilder.Create()
        .WithIdentity("trigger1", "groupa")//名称，分组
        .StartNow()//从启动的时候开始执行
        .WithSimpleSchedule(b =>
        {
            b.WithIntervalInSeconds(2)//2秒执行一次
             .WithRepeatCount(3);//重复执行3+1次
        })
        .ForJob("job1", "groupa")//通过表名和组名进行关联
        .Build();

    //创建一个触发器2
    ITrigger trigger2 = TriggerBuilder.Create()
        .WithIdentity("trigger1", "groupa")//名称，分组
        .StartNow()//从启动的时候开始执行
        .WithSimpleSchedule(b =>
        {
            b.WithIntervalInSeconds(2)//2秒执行一次
             .WithRepeatCount(3);//重复执行3+1次
        })
        .ForJob(job1)//直接IJobDetail关联
        .Build();

    //把作业，触发器加入调度器
    scheduler.AddJob(job1, true);
    scheduler.ScheduleJob(trigger1);
    scheduler.ScheduleJob(trigger2);
}

{
    //job和trigger关联问题
    //2个job对应1个trigger
    //实例化调度器
    IScheduler scheduler = MySchedulerFactory.GetScheduler();

    //开启调度器
    scheduler.Start();

    //创建一个作业1
    IJobDetail job1 = JobBuilder.Create<MyJob>()
        .WithIdentity("job1", "groupa")//名称，分组
        .Build();

    //创建一个作业1
    IJobDetail job2 = JobBuilder.Create<MyJob>()
        .WithIdentity("job2", "groupa")//名称，分组
        .Build();

    //创建一个触发器
    ITrigger trigger1 = TriggerBuilder.Create()
        .WithIdentity("trigger1", "groupa")//名称，分组
        .StartNow()//从启动的时候开始执行
        .WithSimpleSchedule(b =>
        {
            b.WithIntervalInSeconds(2)//2秒执行一次
             .WithRepeatCount(3);//重复执行3+1次
        })
        .ForJob("job1", "groupa")//通过表名和组名进行关联
        .Build();

    //创建监听，添加到调度器
    JobChainingJobListener listener = new JobChainingJobListener("mytest");
    listener.AddJobChainLink(job1.Key, job2.Key);
    scheduler.ListenerManager.AddJobListener(listener);

    //把作业，触发器加入调度器
    scheduler.AddJob(job2, true);
    scheduler.ScheduleJob(job1, trigger1);
}
```

### 7、Canlander

Quartz.NET 日历对象实现`ICalendar`接口可以在触发器存储在调度程序中时与触发器相关联。日历对于从触发器的触发时间表中排除时间块很有用。

Quartz.NET预置了有六种，也可以自定义。日历必须通过`AddCalendar(..)`方法注册到调度程序。同一个日历实例可以与多个触发器一起使用。

```C#
namespace Quartz
{
	public interface ICalendar
	{
		string Description { get; set; }
		ICalendar CalendarBase { set; get; }
		bool IsTimeIncluded(DateTimeOffset timeUtc);
		DateTime GetNextIncludedTimeUtc(DateTimeOffset timeUtc);
		ICalendar Clone();
	}
} 
```

#### （1）DailyCalendar：一天的某个时间段不执行

（需求：21-22点这个区间不执行）

```C#
//实例化调度器
IScheduler scheduler = MySchedulerFactory.GetScheduler();

//开启调度器
scheduler.Start();

//实例化日历
DailyCalendar calendar = new DailyCalendar(DateBuilder.DateOf(21, 0, 0).DateTime,DateBuilder.DateOf(22, 0, 0).DateTime);

//将日历添加到调度器
scheduler.AddCalendar("mycalendar", calendar, true, true);

//创建一个作业
var job1 = JobBuilder.Create<MyJob>().Build();

//创建一个触发器
ITrigger trigger1 = TriggerBuilder.Create()
    .StartNow()//从启动的时候开始执行                    
    .WithSimpleSchedule(b =>
    {
        b.WithIntervalInSeconds(2).RepeatForever();//2秒执行一次
    })
    .ModifiedByCalendar("mycalendar") // but not on holidays
    .Build();

//把作业，触发器加入调度器
scheduler.ScheduleJob(job1, trigger1);
```

#### （2）WeeklyCalendar：一个星期的某一天不执行

（需求：周五这一天不执行）

```C#
WeeklyCalendar calendar = new WeeklyCalendar();
calendar.SetDayExcluded(DayOfWeek.Friday, true);
```

#### （3）HolidayCalendar：当年的某一天不能执行

（需求：今年的6月16号这一天不执行）

```C#
HolidayCalendar calendar = new HolidayCalendar();
calendar.AddExcludedDate(DateTime.Parse("06-16"));
```

#### （4）MonthlyCalendar：一个月的某一天不能执行

（需求：每月的27号不执行）

```C#
MonthlyCalendar calendar = new MonthlyCalendar();
calendar.SetDayExcluded(27, true);
```

#### （5）AnnualCalendar：每年的某一天不能执行

（需求：每年的6月16号这一天不执行）

```C#
AnnualCalendar calendar = new AnnualCalendar();
calendar.SetDayExcluded(DateTime.Parse("06-16"), true);
```

#### （7）CronCalendar：Corn表达式来排除时间不能执行

 (需求：2月27号这天不执行)

```C#
CronCalendar calendar = new CronCalendar("* * * 27 2 ?");
scheduler.AddCalendar("mycalendar", calendar, true, true);
```

### 8、misfire指令失效策略

#### （1）misfire简介

由于某些原因，导致作业在应该执行的时间没有执行，此时这个Trigger变为misfire，当下次调度器启动或者有可以线程时，会检查处于misfire状态的Trigger。而misfire的状态值决定了调度器如何处理这个Trigger。

不同类型触发器有不同的失效恢复策略。所有触发器不指定都是默认策略`MisfirePolicy.SmartPolicy`。

#### （2）misfire产生的原因

- 当job达到触发时间时，所有线程都被其他job占用，没有可用线程。
- 在job需要触发的时间点，scheduler停止了（可能是意外停止的）。
- job使用了@DisallowConcurrentExecution注解，job不能并发执行，当达到下一个job执行点的时候，上一个任务还没有完成。
- job指定了过去的开始执行时间，例如当前时间是8点00分00秒，指定开始时间为7点00分00秒。

#### （3）SimpleTrigger的Misfire策略

SimpleTrigger 的 Misfire策略常量

- `MisfireInstruction.IgnoreMisfirePolicy`
- `MisfirePolicy.SimpleTrigger.FireNow`
- `MisfirePolicy.SimpleTrigger.RescheduleNowWithExistingRepeatCount`
- `MisfirePolicy.SimpleTrigger.RescheduleNowWithRemainingRepeatCount`
- `MisfirePolicy.SimpleTrigger.RescheduleNextWithRemainingCount`
- `MisfirePolicy.SimpleTrigger.RescheduleNextWithExistingCount`

如果使用`MisfirePolicy.SmartPolicy`，SimpleTrigger 根据给定 SimpleTrigger 实例的配置和状态，在其各种 MISFIRE 指令之间动态选择。该`SimpleTrigger.UpdateAfterMisfire()`方法的文档解释了这种动态行为的确切细节。

这里分为三种情况，第一是只执行一次的job，第二是固定次数执行的job，第三是无限次数执行的job。

> 只执行一次的job

设置job只执行一次，开始时间设置设置为当前时间的前10秒，代码片段如下：

```C#
Date next = DateUtils.addSeconds(new Date(), -10);
SimpleTrigger trigger = TriggerBuilder.newTrigger()
         .withIdentity("trigger", "g1")
         .startAt(next)
         .withSchedule(SimpleScheduleBuilder.simpleSchedule()
         .withMisfireHandlingInstructionFireNow()/*可以指定为任意一个可用的misfire策略*/)
                .build();
```

假设job设定的执行时间是8点00分00秒，而当前时间是8点00分10秒，由于misfireThreshold设置为1秒，则发生了misfire。各misfire策略如下：

| 命令                                                         | 说明                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| MISFIRE_INSTRUCTION_SMART_POLICY--default                    | 默认策略等同于MISFIRE_INSTRUCTION_FIRE_NOW。                 |
| MISFIRE_INSTRUCTION_IGNORE_MISFIRE_POLICY                    | Quartz不会判断job发生misfire，但是当Quartz有可用资源的时候，会尽可能早的执行所有发生misfire的任务，结果等同于MISFIRE_INSTRUCTION_FIRE_NOW。 |
| withMisfireHandlingInstructionFireNow<br/>MISFIRE_INSTRUCTION_FIRE_NOW | 立即执行job，即在8点00分10秒发现了misfire以后立即执行job。   |
| withMisfireHandlingInstructionNowWithExistingCount MISFIRE_INSTRUCTION_RESCHEDULE_NOW_WITH_EXISTING_REPEAT_COUNT | 等同于MISFIRE_INSTRUCTION_FIRE_NOW。                         |
| withMisfireHandlingInstructionNowWithRemainingCount<br/>MISFIRE_INSTRUCTION_RESCHEDULE_NOW_WITH_REMAINING_REPEAT_COUNT | 等同于MISFIRE_INSTRUCTION_FIRE_NOW。                         |
| withMisfireHandlingInstructionNextWithExistingCount<br/>MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_EXISTING_COUNT | 不会执行job。此命令会等待下一次执行时间来执行job，但是只执行一次的job，在发生misfire以后没有下次的执行时间，因此使用此命令不会再执行job。 |
| withMisfireHandlingInstructionNextWithRemainingCount<br/>MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_REMAINING_COUNT | 等同于MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_EXISTING_COUNT。 |

> 固定次数执行的job

设置job开始执行时间是早上8点，执行间隔是1小时，执行次数是5次，那么job总的执行次数是6次，则计划的执行时间是8:00，9:00，10:00，11:00，12:00，13:00，代码片段如下：

```C#
SimpleTrigger trigger = TriggerBuilder.newTrigger()
      .withIdentity("trigger1", "g1")
      .startAt(nextOne)
      .withSchedule(simpleSchedule()
            .withIntervalInHours(1)
            .withRepeatCount(5)             .withMisfireHandlingInstructionNowWithRemainingCount()/*可以指定为任意可用的策略*/)
       .build();
```

假设8:00的任务执行了，但是由于某些原因，scheduler没有执行9:00和10:00的任务，在10:15分的时候scheduler发现job有两次没有执行，这两次的延迟执行时间分别是1小时15分和15分，都大于设置的misfireThreshold=1秒，因此发生了两次misfire。各misfire策略如下：

| 命令                                                         | 说明                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| MISFIRE_INSTRUCTION_SMART_POLICY--default                    | 默认执行策略，在固定次数执行的情况下，等同于MISFIRE_INSTRUCTION_RESCHEDULE_NOW_WITH_EXISTING_REPEAT_COUNT |
| MISFIRE_INSTRUCTION_IGNORE_MISFIRE_POLICY                    | Quartz不会判断发生misfire，在Quartz资源可用时会尽可能早的执行所有发生misfire的任务。<br/>例如：Quartz会在10:15执行9:00和10:00的任务，然后按照原计划继续执行剩下的任务。最后任务执行完成时间还是13:00。 |
| withMisfireHandlingInstructionFireNow<br/>MISFIRE_INSTRUCTION_FIRE_NOW | 等同于MISFIRE_INSTRUCTION_RESCHEDULE_NOW_WITH_REMAINING_REPEAT_COUNT。 |
| withMisfireHandlingInstructionNowWithExistingCount<br/>MISFIRE_INSTRUCTION_RESCHEDULE_NOW_WITH_EXISTING_REPEAT_COUNT | 立即执行第一个发生misfire的任务，并且修改startTime为当前时间，然后按照设定的间隔时间执行下一次任务，直到所有的任务执行完成，此命令不会遗漏任务的执行次数。<br/>例如：10:15会立即执行9:00的任务，startTime修改为10:15，然后后续的任务执行时间为,11:15,12:15,13:15,14:15，也就是说任务完成时间延迟到了14:15，但是任务的执行次数还是总共的6次。 |
| withMisfireHandlingInstructionNowWithRemainingCount<br/>MISFIRE_INSTRUCTION_RESCHEDULE_NOW_WITH_REMAINING_REPEAT_COUNT | 立即执行第一个发生misfire的任务，并且修改startTime为当前时间，然后按照设定的间隔时间执行下一个任务，直到所有剩余任务执行完成，此命令会忽略已经发生misfire的任务（第一个misfire任务除外，因为会被立即执行），继续执行剩余的正常任务。<br/>例如：10:15会立即执行9:00的任务，并且修改startTime为10:15，然后Quartz会忽略10:00发生的misfire的任务，然后后续的执行时间为：11:15,12:15,13:15，由于10:00的任务被忽略了，因此总的执行次数实际上是5次。 |
| withMisfireHandlingInstructionNextWithExistingCount<br/>MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_EXISTING_COUNT | 不会立即执行任务，会等到下一次的计划执行时间开始执行，然后按照设定的间隔时间执行直到执行到计划的任务结束时间。<br/>这个地方需要注意一下，不要被命令的名字所迷惑，第一眼印象可能觉得这个命令会把已经misfire的任务也执行了，而且好多博文也是这么讲解的，实际上并没有，我也是在自己测试的时候发现的，其实这个命令在发现存在misfire以后，后续并没有再执行发生misfire的任务，而是继续执行剩下的任务，直到结束时间，因此此命令与MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_REMAINING_COUNT的执行结果相同，至于原因后面会讲。<br/>例如：10:15发现9:00和10:00发生了misfire，并不会立即执行，由于原计划的下一次执行时间是11:00，因此Quartz会等到11:00执行任务，然后在原计划的13:00执行最后一个任务结束，因此实际上总的执行次数是4次。 |
| withMisfireHandlingInstructionNextWithRemainingCount<br/>MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_REMAINING_COUNT | 不会立即执行任务，会等到下一次计划执行时间开始执行，忽略已经发生了misfire的任务，然后按照设定的间隔时间执行直到计划的任务结束时间。<br/>例如：10:15发现9:00和10:00发生了misfire，并不会立即执行，忽略掉发生misfire的9:00和10:00的任务，按照计划在11:00执行任务，直到13:00执行最后一个任务结束，因此总的执行次数是4次。 |

> 无限次数执行的job

设定一个job开始执行时间是早上8点，执行间隔是1小时，无限执行次数，代码片段如下：

```C#
SimpleTrigger trigger = TriggerBuilder.newTrigger()
      .withIdentity("trigger", "g")
      .startAt(next) 
      .withSchedule(SimpleScheduleBuilder.simpleSchedule()
                        .withIntervalInHours(1)
                        .repeatForever())
      .build();
```

假设8:00的任务执行了，但是由于某些原因，scheduler没有执行9:00和10:00的任务，在10:15分的时候scheduler发现job有两次没有执行，这两次的延迟执行时间分别是1小时15分和15分，都大于设置的misfireThreshold=1秒，因此发生了两次misfire。各misfire策略如下：

| 命令                                                         | 说明                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| MISFIRE_INSTRUCTION_SMART_POLICY--default                    | 等同于MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_REMAINING_COUNT。 |
| MISFIRE_INSTRUCTION_IGNORE_MISFIRE_POLICY                    | Quartz不会判断发生misfire，在Quartz资源可用时会尽可能早的执行所有发生misfire的任务。<br/>例如：Quartz会在10:15执行9:00和10:00的任务，然后按照原计划继续执行下去。 |
| withMisfireHandlingInstructionFireNow<br/>MISFIRE_INSTRUCTION_FIRE_NOW | 等同于MISFIRE_INSTRUCTION_RESCHEDULE_NOW_WITH_REMAINING_REPEAT_COUNT。 |
| withMisfireHandlingInstructionNowWithExistingCount<br/>MISFIRE_INSTRUCTION_RESCHEDULE_NOW_WITH_EXISTING_REPEAT_COUNT | 因为执行次数为无限次，所以等同于MISFIRE_INSTRUCTION_RESCHEDULE_NOW_WITH_REMAINING_REPEAT_COUNT。 |
| withMisfireHandlingInstructionNowWithRemainingCount<br/>MISFIRE_INSTRUCTION_RESCHEDULE_NOW_WITH_REMAINING_REPEAT_COUNT | 立即执行第一个发生misfire的任务，并且修改startTime为当前时间，然后按照设定的间隔时间执行下一个任务，一直执行下去，执行次数是无限的，但是计划的执行时间会被改变，因为此策略会修改startTime。<br/>例如：10:15会立即执行9:00的任务，并且修改startTime为10:15，后续的执行时间被修改为了11:15，12:15，13:15以此类推。 |
| withMisfireHandlingInstructionNextWithExistingCount<br/>MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_EXISTING_COUNT | 等同于MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_REMAINING_COUNT。 |
| withMisfireHandlingInstructionNextWithRemainingCount<br/>MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_REMAINING_COUNT | 不会立即执行任务，会等到下一次计划执行时间开始执行，忽略已经发生了misfire的任务，然后按照原计划执行时间继续执行下去。实际上就相当于不管有没有发生misfire，就按照原计划继续执行下去。<br/>例如：10:15发现9:00和10:00发生了misfire，并不会立即执行，忽略掉发生misfire的9:00和10:00的任务，按照计划在11:00执行任务，然后一直按照原计划执行下去。 |

> 几个重要策略实现原理

**先讲解一下SimpleTrigger中几个比较重要的属性：**

- startTime：SimpleTrigger的开始执行时间。
- endTime：SimpleTrigger的结束执行时间，可以不指定。
- repeatCount：重复执行的次数，如果指定为无限次数，则此值被设置为-1。
- repeatInterval：执行的时间间隔。
- finalFireTime：SimpleTrigger的最后触发时间，这个属性很重要，下面讲解的几个策略都跟这个属性有关。

**finalFireTime的计算方法：**

- repeatCount=0，则finalFireTime等于startTime。
- repeatCount为无限次数即-1，则先判断是否存在endTime，如果不存在则finalFireTime为null。如果存在endiTime，则会根据starTime和repeatInterval计算小于或者等于endiTime的最后一次触发时间，此时间作为finalFireTime。
- repeatCount为固定次数，则finalFireTime=startTime+(repeatCount*repeatInterval)，计算结果与endTime比较，如果比endTime小，则直接返回，否则会根据starTime和repeatInterval计算小于或者等于endTime的最后一次触发时间并返回。

**四个策略的实现原理：**

用固定次数例子来进行讲解，8点00分开始执行，执行间隔是1小时，执行次数是5次，计划执行时间是：8:00,9:00,10:00,11:00,12:00,13:00。8:00正常执行，在10:15发现了9:00和10:00的任务发生了misfire。

- MISFIRE_INSTRUCTION_RESCHEDULE_NOW_WITH_EXISTING_REPEAT_COUNT：在10:15分立即执行9:00任务，然后修改starTime为10:15，并且会修改repeatCount为4（原计划中10:00,11:00,12:00,13:00这4个任务），因此计算的finalFireTime为10:15 + (1 * 4) = 14:15，所以最后一次执行时间为14:15，与上诉讲解吻合。
- MISFIRE_INSTRUCTION_RESCHEDULE_NOW_WITH_REMAINING_REPEAT_COUNT：在10:15分立即执行9:00任务，然后修改starTime为10:15，并且会修改repeatCount为3（原计划中11:00,12:00，13:00,10:00的任务会被忽略掉），因此计算的finalFireTime为10:15 + (1 * 3) = 13:15，所以最后一次执行时间为13:15，与上诉讲解吻合。
- MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_EXISTING_COUNT：在10:15分不会执行job，等待下一次执行计划，即在11:00执行任务。这个策略不会修改starTime，也不会修改repeatCount，因此finalFireTime并没有改变，从当前时间到finalFireTime还是剩余原计划中的执行次数。所以说这个策略与MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_REMAINING_COUNT相同，即使发生了misfire也还是按照原计划来执行。
- MISFIRE_INSTRUCTION_RESCHEDULE_NEXT_WITH_REMAINING_COUNT：在10:15分不会执行job，等待下一次执行计划，即在11:00执行任务。这个策略不会修改starTime，也不会修改repeatCount，因此finalFireTime并没有改变，忽略misfire任务，按照原计划继续执行下去。

#### （4）CronTrigger的Misfire策略

CronTrigger的 Misfire策略常量

- `MisfireInstruction.IgnoreMisfirePolicy`
- `MisfireInstruction.CronTrigger.DoNothing`
- `MisfireInstruction.CronTrigger.FireOnceNow`

设定一个job，开始时间为早上8:00，每一个小时执行一次job，代码片段如下：

```C#
CronTrigger trigger = TriggerBuilder.newTrigger()
     .withIdentity("trigger", "g")
     .startAt(next)
     .withSchedule(
            CronScheduleBuilder.cronSchedule("0 0 0/1 * * ?"))
      .build();
```

假设8:00的任务执行了，但是由于某些原因，scheduler没有执行9:00和10:00的任务，在10:15分的时候scheduler发现job有两次没有执行，这两次的延迟执行时间分别是1小时15分和15分，都大于设置的misfireThreshold=1秒，因此发生了两次misfire。各misfire策略如下：

| 命令                                                         | 说明                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| MISFIRE_INSTRUCTION_SMART_POLICY--default                    | 等同于MISFIRE_INSTRUCTION_FIRE_ONCE_NOW。                    |
| MISFIRE_INSTRUCTION_IGNORE_MISFIRE_POLICY                    | Quartz不会判断发生了misfire，立即执行所有发生了misfire的任务，然后按照原计划进行执行。<br/>例如：10:15分立即执行9:00和10:00的任务，然后等待下一个任务在11:00执行，后续按照原计划执行。 |
| withMisfireHandlingInstructionFireAndProceed<br/>MISFIRE_INSTRUCTION_FIRE_ONCE_NOW | 立即执行第一个发生misfire的任务，忽略其他发生misfire的任务，然后按照原计划继续执行。<br/>例如：在10:15立即执行9:00任务，忽略10:00任务，然后等待下一个任务在11:00执行，后续按照原计划执行。 |
| withMisfireHandlingInstructionDoNothing<br/>MISFIRE_INSTRUCTION_DO_NOTHING | 所有发生misfire的任务都被忽略，只是按照原计划继续执行。      |

### 9、Cron表达式

#### （1）介绍

cron 是一个已经存在很长时间的 UNIX 工具，因此它的调度功能强大且经过验证。CronTrigger 类基于 cron 的调度功能。

CronTrigger 使用“cron 表达式”，它能够创建触发时间表，例如：“每周一至周五上午 8:00”或“每月最后一个周五上午 1:30”。

Cron 表达式很强大，但可能会很混乱。本教程旨在揭开创建 cron 表达式的一些神秘面纱，为用户提供在论坛或邮件列表中询问之前可以访问的资源。

#### （2）格式

在线生成网站：[https://www.pppet.net/](https://www.pppet.net/)

cron 表达式是由 6 或 7 个由空格分隔的字段组成的字符串。字段可以包含任何允许的值，以及该字段允许的特殊字符的各种组合。字段如下：

| **字段名称**     | **强制的** | **允许值**      | **允许的特殊字符** |
| ---------------- | ---------- | --------------- | ------------------ |
| 秒               | 是的       | 0-59            | , - * /            |
| 分钟             | 是的       | 0-59            | , - * /            |
| 小时             | 是的       | 0-23            | , - * /            |
| 一个月中的哪一天 | 是的       | 1-31            | , - * ? / L W      |
| 月               | 是的       | 1-12 或 1-12 月 | , - * /            |
| 星期几           | 是的       | 1-7 或 SUN-SAT  | , - * ? /L#        |
| 年               | 不         | 空的，1970-2099 | , - * /            |

所以 cron 表达式可以像这样简单：`* * * * ? *`

或更复杂，像这样：`0/5 14,18,3-39,52 * ? JAN,MAR,SEP MON-FRI 2002-2010`

#### （3）特殊字符

- `*`("all values") - 用于选择字段中的所有值。例如，`*`在分钟字段中表示“每分钟”。
- `?`（“无特定值”） - 当您需要在允许该字符的两个字段之一中指定某些内容时很有用，但另一个字段中不允许。例如，如果我希望触发器在一个月中的特定日期（例如，10 号）触发，但不关心恰好是星期几，我会`10`在 day-of-month 字段中输入，并`?`在星期几字段中。请参阅下面的示例进行说明。
- `-`- 用于指定范围。例如，`10-12`在小时字段中表示“10、11 和 12 小时”。
- `,`- 用于指定附加值。例如，`MON,WED,FRI`在星期几字段中表示“星期一、星期三和星期五”。
- `/`- 用于指定增量。例如，`0/15`在 seconds 字段中表示“秒 0、15、30 和 45”。而`5/15`在 seconds 字段中的意思是“秒 5、20、35 和 50”。您还可以`/`在“字符 - 在这种情况下”之后指定等同于在 '/' 之前有 '0'。 `1/3`在 day-of-month 字段中的意思是“从每月的第一天开始每 3 天触发一次”。
- `L`("last") - 在允许的两个字段中的每个字段中都有不同的含义。例如，`L`day-of-month 字段中的值表示“该月的最后一天” - 1 月的第 31 天，非闰年的 2 月的第 28 天。如果单独在星期几字段中使用，它仅表示“7”或“SAT”。但如果在星期几字段中使用另一个值，则表示“本月的最后 xxx 天” - 例如`6L`表示“本月的最后一个星期五”。您还可以指定与该月最后一天的偏移量，例如`L-3`表示日历月的倒数第三天。使用该`L`选项时，重要的是不要指定列表或值范围，因为您会得到令人困惑/意外的结果。
- `W`("weekday") - 用于指定最接近给定日期的工作日（周一至周五）。例如，如果您要指定`15W`为 day-of-month 字段的值，则其含义是：“距每月 15 日最近的工作日”。因此，如果 15 日是星期六，触发器将在 14 日星期五触发。如果 15 日是星期日，触发器将在 16 日星期一触发。如果 15 号是星期二，那么它将在 15 号星期二触发。但是，如果您指定`1W`日期的值，并且第 1 天是星期六，则触发器将在第 3 天的星期一触发，因为它不会“跳过”一个月的日期边界。`W`仅当月份中的某天是一天，而不是日期范围或日期列表时，才能指定该字符。
- 和字符`L`也`W`可以在 day-of-month 字段中组合为 yield `LW`，它转换为 *"该月的最后一个工作日"。

- `#`- 用于指定一个月中的“第 n 个”XXX 天。例如，`6#3`星期几字段中的值表示“本月的第三个星期五”（第 6 天 = 星期五，“#3” = 本月的第三个星期五）。其他示例：`2#1`= 每月的第一个星期一和`4#5`= 每月的第五个星期三。请注意，如果您指定`#5`并且该月没有 5 个给定的星期几，则该月不会发生触发。
- 合法字符以及月份和星期几的名称不区分大小写。MON 与 mon 相同。

#### （4）常用表达式

| **表达**                 | **意义**                                                     |
| :----------------------- | :----------------------------------------------------------- |
| 0 0 12 * * ?             | 每天中午 12 点（中午）开火                                   |
| 0 15 10 ? * *            | 每天上午 10:15 开火                                          |
| 0 15 10 * * ?            | 每天上午 10:15 开火                                          |
| 0 15 10 * * ? *          | 每天上午 10:15 开火                                          |
| 0 15 10 * * ? 2005年     | 2005 年每天上午 10:15 开火                                   |
| 0 * 14 * * ?             | 每天从下午 2 点开始到下午 2:59 结束，每分钟触发一次          |
| 0 0/5 14 * * ?           | 每天从下午 2 点开始到下午 2:55 结束，每 5 分钟触发一次       |
| 0 0/5 14,18 * * ?        | 从下午 2 点开始每 5 分钟发射一次，到下午 2:55 结束，并且从下午 6 点开始每 5 分钟发射一次，到下午 6:55 结束，每天 |
| 0 0-5 14 * * ?           | 每天从下午 2 点开始到下午 2:05 结束，每分钟触发一次          |
| 0 10,44 14 ? 3 WED       | 在 3 月的每个星期三下午 2:10 和下午 2:44 开火。              |
| 0 15 10 ? * MON-FRI      | 每周一、二、三、四、五上午 10:15 开火                        |
| 0 15 10 15 * ?           | 每月 15 日上午 10:15 开火                                    |
| 0 15 10 升 * ?           | 每月最后一天上午 10:15 开火                                  |
| 0 15 10 L-2 * ?          | 每月倒数最后一天上午 10 点 15 分开火                         |
| 0 15 10 ? * 6L           | 每个月的最后一个星期五上午 10:15 开火                        |
| 0 15 10 ? * 6L           | 每个月的最后一个星期五上午 10:15 开火                        |
| 0 15 10 ? * 6L 2002-2005 | 在 2002 年、2003 年、2004 年和 2005 年期间每个月的最后一个星期五上午 10:15 开火 |
| 0 15 10 ? * 6#3          | 每个月的第三个星期五上午 10:15 开火                          |
| 0 0 12 1/5 * ?           | 从每月的第一天开始，每月每 5 天在中午 12 点（中午）触发一次。 |
| 0 11 11 11 11 ?          | 每年 11 月 11 日上午 11:11 开火。                            |

### 10、监听器Listener实现AOP

注意：确保监听器永远不会抛出异常（使用 try-catch）并且它们可以处理内部问题。当监听器失败时，Quartz 无法确定监听器中所需的逻辑是否成功完成后，作业可能会卡住。

侦听器在运行时向调度程序注册，`并且不与作业和触发器一起存储在 JobStore 中`。这是因为侦听器通常是与您的应用程序的集成点。因此，每次您的应用程序运行时，侦听器都需要重新注册到调度程序。

#### （1）TriggerListener

TriggerListeners 接收与触发器相关的事件，用于根据调度程序中发生的事件执行操作。

> ITriggerListener 接口

```C#
public interface ITriggerListener
{
	 string Name { get; }	 
	 Task TriggerFired(ITrigger trigger, IJobExecutionContext context);	 
	 Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context);
	 
	 Task TriggerMisfired(ITrigger trigger);
	 
	 Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, int triggerInstructionCode);
}
```

> 定义一个类，实现ITriggerListener 接口或者扩展TriggerListenerSupport类简单地覆盖您感兴趣的事件，而不是实现这些接口

```C#
public class CustomTriggerListener : ITriggerListener
{
    public string Name => "CustomTriggerListener";

    /// <summary>
    /// 触发
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
    {

        Console.WriteLine("【***************************************************************************************************************】");
        Console.WriteLine($"【{Name}】---【TriggerFired】-【触发】");
        await Task.CompletedTask;
    }

    /// <summary>
    /// 判断作业是否继续
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
    {

        Console.WriteLine($"【{Name}】---【VetoJobExecution】-【判断作业是否继续】-{true}");
        return await Task.FromResult(cancellationToken.IsCancellationRequested);
    }


    /// <summary>
    ///  触发完成
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="context"></param>
    /// <param name="triggerInstructionCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"【{Name}】---【TriggerComplete】-【触发完成】");
        await Task.CompletedTask;
    }

    /// <summary>
    /// 触发作业
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"【{Name}】---【TriggerMisfired】【触发作业】");
        await Task.CompletedTask;
    }
}
```

> 注册到调度器

```C#
//实例化调度器工厂
ISchedulerFactory schedulefactory = new StdSchedulerFactory();
//实例化调度器
IScheduler scheduler = schedulefactory.GetScheduler().Result;
scheduler.Start();

//创建一个作业
IJobDetail job1 = JobBuilder.Create<MyJob>()
    .WithIdentity("job1", "groupa")//名称，分组
    .Build();

//创建一个触发器
ITrigger trigger1 = TriggerBuilder.Create()
    .WithIdentity("trigger1", "groupa")//名称，分组
    .StartNow()//从启动的时候开始执行
    .WithSimpleSchedule(b =>
    {
        b.WithIntervalInSeconds(2);//2秒执行一次
    })
    .Build();

//将trigger监听器注册到调度器
scheduler.ListenerManager.AddTriggerListener(new CustomTriggerListener());

//把作业，触发器加入调度器
scheduler.ScheduleJob(job1, trigger1);
```

> 运行结果

```bash
【***************************************************************************************************************】
【CustomTriggerListener】---【TriggerFired】-【触发】
【CustomTriggerListener】---【VetoJobExecution】-【判断作业是否继续】-True
【任务执行】：2022/8/15 10:47:19
【触发时间】：2022/8/15 10:47:19
【下次触发时间】：2022/8/15 10:47:21
【CustomTriggerListener】---【TriggerComplete】-【触发完成】
```

#### （2）JobListener

 JobListeners 接收与作业相关的事件，用于根据调度程序中发生的事件执行操作。

> IJobListener 接口

```C#
public interface IJobListener
{
	string Name { get; }
	Task JobToBeExecuted(IJobExecutionContext context);
	Task JobExecutionVetoed(IJobExecutionContext context);
	Task JobWasExecuted(IJobExecutionContext context,       JobExecutionException jobException);
}
```

> 定义一个类，实现IJobListener接口或者扩展JobListenerSupport类简单地覆盖您感兴趣的事件，而不是实现这些接口

```C#
public class CustomJobListener : IJobListener
{
    public string Name => "CustomJobListener";

    /// <summary>
    /// 任务执行前
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"【{Name}】-【JobToBeExecuted】-【要执行的任务】");
        await Task.CompletedTask;
    }


    /// <summary>
    /// 任务执行后
    /// </summary>
    /// <param name="context"></param>
    /// <param name="jobException"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"【{Name}】-【JobWasExecuted】-【作业已执行】");
        await Task.CompletedTask;
    }

    /// <summary>
    /// 任务被拒绝执行的时候
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"【{Name}】-【JobExecutionVetoed】-【工作执行被否决】");
        await Task.CompletedTask;
    }
}
```

> 注册到调度器

```C#
//实例化调度器工厂
ISchedulerFactory schedulefactory = new StdSchedulerFactory();
//实例化调度器
IScheduler scheduler = schedulefactory.GetScheduler().Result;
scheduler.Start();

//创建一个作业
IJobDetail job1 = JobBuilder.Create<MyJob>()
    .WithIdentity("job1", "groupa")//名称，分组
    .Build();

//创建一个触发器
ITrigger trigger1 = TriggerBuilder.Create()
    .WithIdentity("trigger1", "groupa")//名称，分组
    .StartNow()//从启动的时候开始执行
    .WithSimpleSchedule(b =>
    {
        b.WithIntervalInSeconds(2);//2秒执行一次
    })
    .Build();

//将job监听器注册到调度器
scheduler.ListenerManager.AddJobListener(new CustomJobListener());

//把作业，触发器加入调度器
scheduler.ScheduleJob(job1, trigger1);
```

> 运行结果

```C#
【CustomJobListener】-【JobToBeExecuted】-【要执行的任务】
【任务执行】：2022/8/15 11:02:25
【触发时间】：2022/8/15 11:02:25
【下次触发时间】：
【CustomJobListener】-【JobWasExecuted】-【作业已执行】
```

> 多种使用场景

**添加一个对特定工作感兴趣的 JobListener：**

```C#
scheduler.ListenerManager.AddJobListener(myJobListener, KeyMatcher<JobKey>.KeyEquals(new JobKey("myJobName", "myJobGroup")));
```

**添加一个对特定组的所有作业感兴趣的 JobListener：**

```c#
scheduler.ListenerManager.AddJobListener(myJobListener, GroupMatcher<JobKey>.GroupEquals("myJobGroup"));
```

**添加一个对两个特定组的所有作业感兴趣的 JobListener：**

```C#
scheduler.ListenerManager.AddJobListener(myJobListener,
	OrMatcher<JobKey>.Or(GroupMatcher<JobKey>.GroupEquals("myJobGroup"), GroupMatcher<JobKey>.GroupEquals("yourGroup")));
```

**添加一个对所有作业感兴趣的 JobListener：**

```C#
scheduler.ListenerManager.AddJobListener(myJobListener, GroupMatcher<JobKey>.AnyGroup());
```

#### （3）SchedulerListener

`SchedulerListener `，它们只接收调度程序本身的事件通知 - 不一定是与特定触发器或作业相关的事件。

与调度器相关的事件包括：作业/触发器的添加、作业/触发器的移除、调度器内部的严重错误、调度器被关闭的通知等。

> ISchedulerListener 接口

```C#
public interface ISchedulerListener
{
	Task JobScheduled(Trigger trigger);
	Task JobUnscheduled(string triggerName, string triggerGroup);
	Task TriggerFinalized(Trigger trigger);
	Task TriggersPaused(string triggerName, string triggerGroup);
	Task TriggersResumed(string triggerName, string triggerGroup);
	Task JobsPaused(string jobName, string jobGroup);
	Task JobsResumed(string jobName, string jobGroup);
	Task SchedulerError(string msg, SchedulerException cause);
	Task SchedulerShutdown();
} 
```

**添加调度器监听器：**

```C#
scheduler.ListenerManager.AddSchedulerListener(mySchedListener);
```

**删除 SchedulerListener：**

```C#
scheduler.ListenerManager.RemoveSchedulerListener(mySchedListener);
```

### 11、线程池ThreadPool

#### （1）简介

SimpleThreadPool是Quartz.Net中自带的线程池，默认个数为10个，代表一个Scheduler同一时刻并发的最多只能执行10个job，超过10个的job需要排队等待。

下面通过四种配置方式来实现线程的配置，同时了解下有四种参数配置的方式。

**4种方式的优先级为：quartz.config < app.config < 环境变量 < namevaluecollection**

#### （2）NameValueCollection方式配置

需要利用StdSchedulerFactory的构造函数进行传进去，向哪个Sheduler中传，即配置哪个Sheduler的对应的线程池。

```C#
NameValueCollection pars = new NameValueCollection
{                    
    //线程池个数20
    ["quartz.threadPool.threadCount"] = "20"
};
ISchedulerFactory schedulefactory = new StdSchedulerFactory(pars);
IScheduler scheduler = schedulefactory.GetScheduler().Result;
```

#### （3）系统配置文件的方式配置

**App.config/web.config**

在.net framwork程序中，可以配置在App.config/web.config文件中，该模式代码中不需要进行任何的额外配置，应用于所有的Sheduler。

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <!--线程池个数设置   开始-->

  <configSections>
    <section name="quartz" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0,Culture=neutral, PublicKeyToken=b77a5c561934e089"/>
  </configSections>
  <quartz>
    <!--设置Sheduler的线程池个数为22-->
    <add key="quartz.threadPool.threadCount" value="22"/>
  </quartz>

  <!--线程池个数设置   结束-->
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6"/>
  </startup>
</configuration>
```

**appsettings.json**

在net core程序中，可以配置在appsettings.json文件中，该模式代码中不需要进行任何的额外配置，应用于所有的Sheduler。

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "Quartz": {
    "quartz.scheduler.instanceName": "Quartz ASP.NET Core Sample Scheduler"
  }
}
```

#### （3）quartz.config的方式进行配置

添加一个配置文件quartz.config，属性设置成`始终复制`，该模式代码中不需要进行任何的额外配置，应用于所有的Sheduler。

```ini
quartz.threadPool.threadCount=15
```

#### （4）设置环境变量来实现

应用于所有的Sheduler。

```C#
Environment.SetEnvironmentVariable("quartz.threadPool.threadCount", "26");
var factory = new StdSchedulerFactory();
var scheduler = factory.GetScheduler();
```

### 12、工作存储JobStore

#### （1）RAMJobStore

`RAMJobStore`是最简单的 JobStore，它也是性能最高，速度最快。它将所有数据保存在 RAM 中。缺点是当您的应用程序结束（或崩溃）时，所有调度信息都会丢失 - 这意味着 RAMJobStore 无法遵守作业和触发器的“非易失性”设置。

**配置 Quartz 以使用 RAMJobStore**

```ini
quartz.jobStore.type = Quartz.Simpl.RAMJobStore, Quartz
```

如果使用`StdSchedulerFactory`构建Schedule，不需要做任何特别的配置。Quartz.NET 的默认配置`RAMJobStore`用作作业存储实现。

#### （2）AdoJobStore

AdoJobStore通过 ADO.NET 将所有数据保存在数据库中。它的配置要复杂一些，而且速度也没有那么快。

**首先创建数据库表**

创建表的SQL[https://github.com/quartznet/quartznet/tree/main/database/tables](https://github.com/quartznet/quartznet/tree/main/database/tables)

- qrtz_blob_triggers : 以Blob 类型存储的触发器。 
- qrtz_calendars：日历信息， quartz可配置一个日历来指定一个时间范围。 
- qrtz_cron_triggers：cron类型的触发器。 
- qrtz_fired_triggers：已触发的触发器。 
- qrtz_job_details：jobDetail信息。 
- qrtz_locks： 程序的悲观锁的信息(假如使用了悲观锁)。 
- qrtz_paused_trigger_graps：暂停掉的触发器。 
- qrtz_scheduler_state：调度器状态。 
- qrtz_simple_triggers：简单触发器的信息。 
- qrtz_simprop_triggers：简单触发器的一些其他信息。
- qrtz_triggers：将Trigger和job进行关联的表。

**配置使用 AdoJobStore**

目前，作业存储内部实现的唯一选择是`JobStoreTX`自己创建事务。

```ini
quartz.jobStore.type = Quartz.Impl.AdoJobStore.JobStoreTX, Quartz
```

**配置使用 DriverDelegate**

选择一个`IDriverDelegate`实现供 JobStore 使用， `StdAdoDelegate`是一个通用委托。但是针对不同类型数据库的特殊委托通常具有更好的性能或针对性。可以在`Quartz.Impl.AdoJobStore`命名空间中找到这些委托，具体的可以参考配置参考。

```ini
quartz.jobStore.driverDelegateType = Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz
```

**配置表前缀**

提供的sql脚本中所有表格都以前缀`QRTZ_`开头，这个前缀实际上可以配置的，使用不同的前缀可能有助于在同一数据库中为多个调度程序实例创建多组表。

```ini
quartz.jobStore.tablePrefix = QRTZ_
```

**配置数据源名称**

```ini
quartz.jobStore.dataSource = myDS
```

**配置数据源的连接字符串和数据库提供者**

具体的配置可以查看配置参考

```ini
quartz.dataSource.myDS.connectionString = Server=localhost;Database=quartz;Uid=quartznet;Pwd=quartznet
 quartz.dataSource.myDS.provider = MySql
```

如果您的调度程序非常繁忙（即几乎总是执行与线程池大小相同数量的作业，那么您可能应该将数据源中的连接数设置为大约线程池大小 + 1。这通常在 ADO.NET 连接字符串中配置。

**配置使用字符串作为 JobDataMap 值**

可以设置为“ `quartz.jobStore.useProperties=true`（默认为 false），以指示 AdoJobStore JobDataMaps 中的所有值都是字符串，因此可以存储为键值对，而不是以序列化形式存储更复杂的对象在 BLOB 列。大大降低了类型序列化问题的可能性。

```ini
quartz.jobStore.useProperties = true
```

**配置存储数据用的序列化程序**

Quartz.NET 支持二进制和 JSON 序列化来存储数据到数据库。JSON序列化来自单独的NuGet 包 `Quartz.Serialization.Json`。建议使用JSON序列化。

```ini
quartz.serializer.type = json
```

#### （3）实现Quartz持久化

- nuget引入mysql驱动程序`MySql.Data`
- nuget引入序列化包`Quartz.Serialization.Json`
- 安装数据库：这里使用docker安装数据库`docker run --name mysqlserver -v /data/mysql/conf:/etc/mysql/conf.d -v /data/mysql/logs:/logs -v /data/mysql/data:/var/lib/mysql -e MYSQL_ROOT_PASSWORD=123456 -d -i -p 3306:3306 mysql:latest --lower_case_table_names=1`，linux相关知识请参考[linux详解](https://blog.csdn.net/liyou123456789/article/details/121548156)，docker相关知识请参考[docker详解](https://blog.csdn.net/liyou123456789/article/details/122292877)，mysql相关知识请参考[mysql详解](https://blog.csdn.net/liyou123456789/article/details/126023696)，创建数据库`quartzmanager`。
- 数据库`quartzmanager`中执行mysql数据库对应的sql脚本[https://github.com/quartznet/quartznet/tree/main/database/tables](https://github.com/quartznet/quartznet/tree/main/database/tables)
- 连接字符串参考：[https://www.connectionstrings.com/](https://www.connectionstrings.com/)，这里填上自己的数据库服务器IP地址。
- 运行程序

```C#
NameValueCollection pars = new NameValueCollection
{
    //scheduler名字
    ["quartz.scheduler.instanceName"] = "MyAdoJobStoreScheduler",
    //类型为JobStoreXT,事务
    ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
    //数据源名称
    ["quartz.jobStore.dataSource"] = "QuartzDb",
    //使用mysql的Ado操作代理类
    ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz",
    //数据源连接字符串
    ["quartz.dataSource.QuartzDb.connectionString"] = @"server=数据库服务器IP地址;Database=quartzmanager;user id=root;password=123456;SslMode=none;",
    //数据源的数据库
    ["quartz.dataSource.QuartzDb.provider"] = "MySql",
    //序列化类型
    ["quartz.serializer.type"] = "json",
    //自动生成scheduler实例ID，主要为了保证集群中的实例具有唯一标识
    ["quartz.scheduler.instanceId"] = "AUTO"
};
//实例化调度器
ISchedulerFactory schedulefactory = new StdSchedulerFactory(pars);
IScheduler scheduler = schedulefactory.GetScheduler().Result;

//开启调度器
scheduler.Start();

//创建一个作业
IJobDetail job1 = JobBuilder.Create<MyJob>()
    .WithIdentity("job1", "groupa")//名称，分组
    .Build();

//创建一个触发器
ITrigger trigger1 = TriggerBuilder.Create()
    .WithIdentity("trigger1", "groupa")//名称，分组
    .StartNow()//从启动的时候开始执行
    .WithSimpleSchedule(b =>
    {
        b.WithIntervalInSeconds(2)//2秒执行一次
         .RepeatForever();
    })
    .Build();

//把作业，触发器加入调度器
scheduler.ScheduleJob(job1, trigger1);
```

- 查看数据库，数据已经自动存入

![image-20220818101455324](http://cdn.bluecusliyou.com/202208181014525.png)

- 注释掉job和trigger，重启Scheduler，程序自动继续运行，说明调度器运行的是数据库中取出的任务，持久化成功了。

```C#
NameValueCollection pars = new NameValueCollection
{
    //scheduler名字
    ["quartz.scheduler.instanceName"] = "MyAdoJobStoreScheduler",
    //类型为JobStoreXT,事务
    ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
    //数据源名称
    ["quartz.jobStore.dataSource"] = "QuartzDb",
    //使用mysql的Ado操作代理类
    ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz",
    //数据源连接字符串
    ["quartz.dataSource.QuartzDb.connectionString"] = @"server=数据库服务器IP地址;Database=quartzmanager;user id=root;password=123456;SslMode=none;",
    //数据源的数据库
    ["quartz.dataSource.QuartzDb.provider"] = "MySql",
    //序列化类型
    ["quartz.serializer.type"] = "json",
    //自动生成scheduler实例ID，主要为了保证集群中的实例具有唯一标识
    ["quartz.scheduler.instanceId"] = "AUTO"
};
//实例化调度器
ISchedulerFactory schedulefactory = new StdSchedulerFactory(pars);
IScheduler scheduler = schedulefactory.GetScheduler().Result;

//开启调度器
scheduler.Start();
```

### 13、集群cluster

#### （1）集群配置

**配置使用 AdoJobStore**

集群目前仅适用于 AdoJobstore ( `JobStoreTX`)。功能包括负载平衡和作业故障转移（如果 JobDetail 的“请求恢复”标志设置为 true）。

```ini
quartz.jobStore.type = Quartz.Impl.AdoJobStore.JobStoreTX, Quartz
```

**配置使用集群**

```ini
quartz.jobStore.clustered=true
```

**配置调度器实例id**

集群中的每个实例都应该使用相同的属性副本。只有线程池大小（不同机器硬件不同可以配置不同）和`quartz.scheduler.instanceId`可以不同，集群中的每个节点都必须有一个唯一的 instanceId，可以设置成`AUTO`让系统自动分配。

```ini
quartz.scheduler.instanceId=AUTO
```

#### （2）注意事项

- 切勿针对任何其他正在运行的实例的同一组数据库表启动非集群实例，可能会遇到严重的数据损坏，并且肯定会遇到不稳定的行为。

- 监控并确保您的节点有足够的 CPU 资源来完成作业。当某些节点处于 100% CPU 时，它们可能无法更新作业存储，而其他节点可以认为这些作业丢失并通过重新运行来恢复它们。

#### （3）实现Quartz集群热备

集群热备：即一主多备，高可用，主挂掉了，备会自动顶上去， Quartz.Net集群采用的就是这种形式。

其他操作同Quartz持久化，多一个配置开启集群`["quartz.jobStore.clustered"] = "true"`

```C#
NameValueCollection pars = new NameValueCollection
{
    //scheduler名字
    ["quartz.scheduler.instanceName"] = "MyAdoJobStoreScheduler",
    //类型为JobStoreXT,事务
    ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
    //数据源名称
    ["quartz.jobStore.dataSource"] = "QuartzDb",
    //使用mysql的Ado操作代理类
    ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz",
    //数据源连接字符串
    ["quartz.dataSource.QuartzDb.connectionString"] = @"server=数据库服务器IP地址;Database=quartzmanager;user id=root;password=123456;SslMode=none;",
    //数据源的数据库
    ["quartz.dataSource.QuartzDb.provider"] = "MySql",
    //序列化类型
    ["quartz.serializer.type"] = "json",
    //自动生成scheduler实例ID，主要为了保证集群中的实例具有唯一标识
    ["quartz.scheduler.instanceId"] = "AUTO",
    //添加一个开启集群的配置，默认是不开启的
    ["quartz.jobStore.clustered"] = "true"
};
//实例化调度器
ISchedulerFactory schedulefactory = new StdSchedulerFactory(pars);
IScheduler scheduler = schedulefactory.GetScheduler().Result;

//开启调度器
scheduler.Start();
```

开启两个exe，先开的执行任务，后开的等待，过几分钟，后开的执行，先开的等待，循环往复，实现负载均衡，关掉一个，另一个继续运行，实现热备。

![image-20220818110346639](http://cdn.bluecusliyou.com/202208181103754.png)

### 14、配置文件调度

#### （1）配置文件

新建配置文件`quartz_jobs.xml`，右键属性`始终复制`

```xml
<job-scheduling-data xmlns="http://quartznet.sourceforge.net/JobSchedulingData" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" version="2.0">
	<processing-directives>
		<overwrite-existing-data>true</overwrite-existing-data>
	</processing-directives>

	<schedule>
		<!--开始执行一个调度-->
		<!--任务，可添加多个-->
		<job>
			<!--任务名称【必填】：同一group中多个job的name名不能相同-->
			<name>jobName1</name>
			<!--任务所属分组【选填】-->
			<group>jobGroup1</group>
			<!--任务描述【选填】-->
			<description>jobDescription1</description>
			<!--任务类型【必填】：实现IJob接口完整命名空间的类名，程序集名-->
			<job-type>MyQuartZ.Net.QuartZJob.MyJob, MyQuartZ.Net</job-type>
			<durable>true</durable>
			<recover>false</recover>
			<!--任务数据【选填】-->
			<job-data-map>
				<entry>
					<key>key0</key>
					<value>value0</value>
				</entry>
				<entry>
					<key>key1</key>
					<value>value1</value>
				</entry>
				<entry>
					<key>key2</key>
					<value>value2</value>
				</entry>
			</job-data-map>
		</job>

		<!--触发器，可添加多个-->
		<trigger>
			<!--simple:简单触发器类型-->
			<simple>
				<!--触发器名称【必填】：同一group中多个trigger的name不能相同-->
				<name>simpleName</name>
				<!--触发器组【选填】-->
				<group>simpleGroup</group>
				<!--触发器描述【选填】-->
				<description>SimpleTriggerDescription</description>
				<!--要调度的任务名称【必填】：必须和对应job节点中的name相同-->
				<job-name>jobName1</job-name>
				<!--要调度的任务分组【必填】：必须和对应job节点中的name相同-->
				<job-group>jobGroup1</job-group>
				<!--任务的开始时间-->
				<start-time>2022-01-01T18:15:00.0Z</start-time>
				<!--任务的结束时间-->
				<end-time>2040-05-04T18:13:51.0Z</end-time>
				<!--失效策略-->
				<misfire-instruction>SmartPolicy</misfire-instruction>
				<!--重复次数-->
				<repeat-count>100</repeat-count>
				<!--间隔时间 单位毫秒-->
				<repeat-interval>3000</repeat-interval>
			</simple>
		</trigger>

		<!--触发器，可添加多个-->
		<trigger>
			<!--cron:复杂触发器类型【推荐】-->
			<cron>
				<!--触发器名称【必填】：同一group中多个trigger的name不能相同-->
				<name>cronName</name>
				<!--触发器组【选填】-->
				<group>cronGroup</group>
				<!--触发器描述【选填】-->
				<description>CronTriggerDescription</description>
				<!--要调度的任务名称【必填】：必须和对应job节点中的name相同-->
				<job-name>jobName1</job-name>
				<!--要调度的任务分组【必填】：必须和对应job节点中的name相同-->
				<job-group>jobGroup1</job-group>
				<!--任务cron表达式-->
				<cron-expression>0/2 * * * * ?</cron-expression>
			</cron>
		</trigger>
		<!--结束一个调度-->
	</schedule>
</job-scheduling-data>
```

#### （2）代码实现

nuget引入程序集`Quartz.Plugins`

```C#
NameValueCollection pars = new NameValueCollection
{                
    //配置读取配置文件
    ["quartz.plugin.jobInitializer.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins",
    ["quartz.plugin.jobInitializer.fileNames"] = "quartz_jobs.xml",
    ["quartz.plugin.jobInitializer.failOnFileNotFound"] = "true"
};
//实例化调度器
ISchedulerFactory schedulefactory = new StdSchedulerFactory(pars);
IScheduler scheduler = schedulefactory.GetScheduler().Result;

//开启调度器
scheduler.Start();
```

### 15、配置参考

默认情况下，`StdSchedulerFactory`加载一个名为`quartz.config`的属性文件。如果失败，则Quartz使用默认值，如果您希望使用`quartz.config`以外的文件，则必须定义系统属性`quartz.properties`以指向您想要的文件名称。

#### （1）主要配置

这些属性配置调度程序的标识，以及各种其他“顶级”设置。

| 属性名称                                                    | 必需 | 类型    | 默认值                                         |
| ----------------------------------------------------------- | ---- | ------- | ---------------------------------------------- |
| quartz.scheduler.instanceName                               | no   | string  | 'QuartzScheduler'                              |
| quartz.scheduler.instanceId                                 | no   | string  | 'NON_CLUSTERED'                                |
| quartz.scheduler.instanceIdGenerator.type                   | no   | string  | Quartz.Simpl.SimpleInstanceIdGenerator, Quartz |
| quartz.scheduler.threadName                                 | no   | string  | instanceName + '_QuartzSchedulerThread'        |
| quartz.scheduler.makeSchedulerThreadDaemon                  | no   | boolean | false                                          |
| quartz.scheduler.idleWaitTime                               | no   | long    | 30000                                          |
| quartz.scheduler.typeLoadHelper.type                        | no   | string  | Quartz.Simpl.SimpleTypeLoadHelper              |
| quartz.scheduler.jobFactory.type                            | no   | string  | Quartz.Simpl.PropertySettingJobFactory         |
| quartz.context.key.SOME_KEY                                 | no   | string  | none                                           |
| quartz.scheduler.wrapJobExecutionInUserTransaction          | no   | boolean | false                                          |
| quartz.scheduler.batchTriggerAcquisitionMaxCount            | no   | int     | 1                                              |
| quartz.scheduler.batchTriggerAcquisitionFireAheadTimeWindow | no   | long    | 0                                              |

`quartz.scheduler.instanceName`

可以是任何字符串，并且该值对调度程序本身没有意义——而是作为客户端代码在同一程序中使用多个实例时区分调度程序的一种机制。如果您使用集群功能，则必须为集群中“逻辑上”相同的调度程序的每个实例使用相同的名称。

`quartz.scheduler.instanceId`

可以是任何字符串，但对于所有工作的调度程序必须是唯一的，就好像它们是集群内的同一个“逻辑”调度程序一样。如果您希望为您生成 Id，则可以使用值“AUTO”作为 instanceId。如果您希望值来自系统属性“quartz.scheduler.instanceId”，则为值“SYS_PROP”。

`quartz.scheduler.instanceIdGenerator.type`

仅在quartz.scheduler.instanceId 设置为`AUTO`时使用。默认为“Quartz.Simpl.SimpleInstanceIdGenerator”，它根据主机名和时间戳生成实例 id。其他`InstanceIdGenerator`实现包括`SystemPropertyInstanceIdGenerator` （从系统属性“quartz.scheduler.instanceId”获取实例ID，并`HostnameInstanceIdGenerator`使用本地主机名（`Dns.GetHostEntry(Dns.GetHostName())`）。您也可以自己实现InstanceIdGenerator接口。

`quartz.scheduler.threadName`

可以是主调度程序线程的有效名称的任何字符串。如果未指定此属性，线程将接收调度程序的名称（“quartz.scheduler.instanceName”）加上附加的字符串“_QuartzSchedulerThread”。

`quartz.scheduler.makeSchedulerThreadDaemon`

一个布尔值（“true”或“false”），指定调度程序的主线程是否应该是守护线程。另请参阅`quartz.scheduler.makeSchedulerThreadDaemon`属性以调整`DefaultThreadPool`是否是您正在使用的线程池实现（很可能是这种情况）。

`quartz.scheduler.idleWaitTime`

是当调度程序空闲时，调度程序在重新查询可用触发器之前等待的时间量（以毫秒为单位）。通常，您不必“调整”此参数，除非您正在使用 XA 事务，并且在延迟触发应立即触发的触发器时遇到问题。不建议使用小于 5000 毫秒的值，因为它会导致过多的数据库查询。小于 1000 的值是不合法的。

`quartz.scheduler.typeLoadHelper.type`

默认使用最健壮的方法，即使用“Quartz.Simpl.SimpleTypeLoadHelper”类型 - 只需使用`Type.GetType()`.

`quartz.scheduler.jobFactory.type`

要使用的 IJobFactory 的类型名称。作业工厂负责生成`IJob`实现的实例。默认值为“Quartz.Simpl.PropertySettingJobFactory”，`Activator.CreateInstance`每次执行即将发生时，它都会以给定类型简单地调用以生成一个新实例。 `PropertySettingJobFactory`还使用调度程序上下文和作业的内容反射性地设置作业的属性并触发 JobDataMaps。

`quartz.context.key.SOME_KEY`

表示将作为字符串放入“调度程序上下文”的名称-值对（请参阅 IScheduler.Context）。因此，例如，设置“quartz.context.key.MyKey = MyValue”将执行相当于`scheduler.Context.Put("MyKey", "MyValue")`.

`quartz.scheduler.batchTriggerAcquisitionMaxCount`

允许调度程序节点一次获取（用于触发）的最大触发器数。默认值为 1。数字越大，触发效率越高（在需要一次触发的触发器非常多的情况下）——但代价是集群节点之间的负载可能不平衡。

如果此属性的值设置为 > 1，并且使用了 AdoJobStore，则必须将属性“quartz.jobStore.acquireTriggersWithinLock”设置为“true”以避免数据损坏。

`quartz.scheduler.batchTriggerAcquisitionFireAheadTimeWindow`

允许在预定触发时间之前获取和触发触发器的时间量（以毫秒为单位）。默认为 0。数字越大，触发的批量获取触发器就越有可能一次选择并触发超过 1 个触发器 - 代价是触发器计划没有被精确遵守（触发器可能会提前触发这个数量）。

在调度程序有大量触发器需要同时或几乎同时触发的情况下，这可能很有用（出于性能考虑）。

#### （2） 线程池ThreadPool

| 属性名称                         | 必需的 | 类型   | 默认值                         |
| -------------------------------- | ------ | ------ | ------------------------------ |
| quartz.threadPool.type           | no     | string | Quartz.Simpl.DefaultThreadPool |
| quartz.threadPool.maxConcurrency | no     | int    | 10                             |

`quartz.threadPool.type`

是您希望使用的 ThreadPool 实现的名称。Quartz 附带的线程池是“Quartz.Simpl.DefaultThreadPool”，应该可以满足几乎所有用户的需求。

它的行为非常简单，并且经过了很好的测试。它将任务分派到 .NET 任务队列，并确保遵守配置的最大并发任务数量限制。你应该学习`CLR的托管线程池`如果您想在 CLR 级别微调线程池。

`quartz.threadPool.maxConcurrency`

这是可以分派到 CLR 线程池的并发任务数。如果你只有几份工作每天解雇几次，那么 1 个任务就足够了！如果您有数以万计的作业，每分钟触发许多作业，那么您可能希望最大并发数更像 50 或 100（这在很大程度上取决于您的作业执行的工作的性质以及您的系统资源！）。还要注意 CLR 线程池配置与 Quartz 本身分开。

如果您使用自己的线程池实现，您可以通过简单地命名属性来反射地设置属性，如下所示：

```ini
quartz.threadPool.type = MyLibrary.FooThreadPool, MyLibrary
quartz.threadPool.somePropOfFooThreadPool = someValue
```

#### （3）监听器Listener

全局监听器可以通过实例化和配置`StdSchedulerFactory`，或者你的应用程序可以在运行时自己做，然后向调度器注册监听器。“全局”侦听器侦听每个作业/触发器的事件，而不仅仅是直接引用它们的作业/触发器。

通过配置文件配置侦听器包括给定一个名称，然后指定类型名称以及要在实例上设置的任何其他属性。该类型必须有一个无参数的构造函数，并且属性是反射设置的。仅支持原始数据类型值（包括字符串）。

因此，定义“全局”TriggerListener 的一般模式是：

```ini
quartz.triggerListener.NAME.type = MyLibrary.MyListenerType, MyLibrary
quartz.triggerListener.NAME.propName = propValue
quartz.triggerListener.NAME.prop2Name = prop2Value
```

定义“全局” JobListener 的一般模式是：

```ini
quartz.jobListener.NAME.type = MyLibrary.MyListenerType, MyLibrary
quartz.jobListener.NAME.propName = propValue
quartz.jobListener.NAME.prop2Name = prop2Value
```

#### （4）插件Plugin

就像侦听器通过配置文件配置插件一样，包括给定一个名称，然后指定类型名称以及要在实例上设置的任何其他属性。该类型必须有一个无参数的构造函数，并且属性是反射设置的。仅支持原始数据类型值（包括字符串）。

因此，定义插件的一般模式是：

```ini
quartz.plugin.NAME.type = MyLibrary.MyPluginType, MyLibrary
quartz.plugin.NAME.propName = propValue
quartz.plugin.NAME.prop2Name = prop2Value
```

Quartz 附带了几个插件，可以在`Quartz.Plugins`包裹。配置其中几个的示例如下：

**Logging Trigger History 插件的示例配置**

日志触发历史插件捕获触发事件（它也是一个触发侦听器），然后使用日志基础设施进行日志记录。

```ini
quartz.plugin.triggHistory.type = Quartz.Plugin.History.LoggingTriggerHistoryPlugin, Quartz.Plugins
quartz.plugin.triggHistory.triggerFiredMessage = Trigger {1}.{0} fired job {6}.{5} at: {4:HH:mm:ss MM/dd/yyyy}
quartz.plugin.triggHistory.triggerCompleteMessage = Trigger {1}.{0} completed firing job {6}.{5} at {4:HH:mm:ss MM/dd/yyyy} with resulting trigger instruction code: {9}
```

**XML调度数据处理器插件的示例配置**

作业初始化插件从 XML 文件中读取一组作业和触发器，并在初始化期间将它们添加到调度程序中。它还可以删除现有数据。

该文件的 XML 模式定义可以在[https://github.com/quartznet/quartznet/blob/master/src/Quartz/Xml/job_scheduling_data_2_0.xsd](https://github.com/quartznet/quartznet/blob/master/src/Quartz/Xml/job_scheduling_data_2_0.xsd)找到

```ini
quartz.plugin.jobInitializer.type = Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins
quartz.plugin.jobInitializer.fileNames = data/my_job_data.xml
quartz.plugin.jobInitializer.failOnFileNotFound = true
```

**Shutdown Hook插件的示例配置**

shutdown-hook 插件捕获 CLR 终止的事件，并在调度程序上调用 shutdown。

```ini
quartz.plugin.shutdownhook.type = Quartz.Plugin.Management.ShutdownHookPlugin, Quartz.Plugins
quartz.plugin.shutdownhook.cleanShutdown = true
```

**作业中断监视器插件的示例配置**

该插件捕获作业长时间运行（超过配置的最大时间）的事件，并告诉调度程序“尝试”在启用时中断它。插件默认在 5 分钟后发出信号中断，可以配置不同的值，配置中的值以毫秒为单位。

```ini
quartz.plugin.jobAutoInterrupt.type = Quartz.Plugin.Interrupt.JobInterruptMonitorPlugin, Quartz.Plugins
quartz.plugin.jobAutoInterrupt.defaultMaxRunTime = 3000000
```

#### （5）远程服务器和客户端

| 属性名称                                       | 必需的 | 类型    | 默认值            |
| ---------------------------------------------- | ------ | ------- | ----------------- |
| quartz.scheduler.exporter.type                 | yes    | string  |                   |
| quartz.scheduler.exporter.port                 | yes    | int     |                   |
| quartz.scheduler.exporter.bindName             | no     | string  | 'QuartzScheduler' |
| quartz.scheduler.exporter.channelType          | no     | string  | 'tcp'             |
| quartz.scheduler.exporter.channelName          | no     | string  | 'http'            |
| quartz.scheduler.exporter.typeFilterLevel      | no     | string  | 'Full'            |
| quartz.scheduler.exporter.rejectRemoteRequests | no     | boolean | false             |

如果您希望 Quartz 调度程序通过远程处理将自身导出为服务器，则将 'quartz.scheduler.exporter.type' 设置为 "Quartz.Simpl.RemotingSchedulerExporter, Quartz"。

`quartz.scheduler.exporter.type`

的类型`ISchedulerExporter`，目前仅支持“Quartz.Simpl.RemotingSchedulerExporter, Quartz”。

`quartz.scheduler.exporter.port`

要监听的端口。

`quartz.scheduler.exporter.bindName`

绑定到远程基础结构时使用的名称。

`quartz.scheduler.exporter.channelType`

无论是“tcp”还是“http”，TCP 的性能都更高。

`quartz.scheduler.exporter.channelName`

绑定到远程基础结构时使用的通道名称。

`quartz.scheduler.exporter.typeFilterLevel`

**Low**：.NET Framework 远程处理的低反序列化级别。它支持与基本远程功能相关的类型

**Full**.NET Framework 远程处理的完整反序列化级别。它支持所有情况下远程支持的所有类型

`quartz.scheduler.exporter.rejectRemoteRequests`

一个布尔值（真或假），指定是否拒绝来自其他计算机的请求。指定 true 仅允许来自本地计算机的远程调用。

#### （6）内存运行RAMJobStore

RAMJobStore 用于在内存中存储调度信息（作业、触发器和日历）。RAMJobStore 快速且轻量级，但当进程终止时，所有调度信息都会丢失。

**将 Scheduler 的 JobStore 设置为 RAMJobStore**

```ini
quartz.jobStore.type = Quartz.Simpl.RAMJobStore, Quartz
```

RAMJobStore 可以使用以下属性进行调整：

| 属性名称                         | 必需的 | 类型 | 默认值 |
| -------------------------------- | ------ | ---- | ------ |
| quartz.jobStore.misfireThreshold | no     | int  | 60000  |

`quartz.jobStore.misfireThreshold`

在被视为“未触发”之前，调度程序将“容忍”触发器通过其下一次触发时间的毫秒数。默认值（如果您没有在配置中输入此属性）是 60000（60 秒）。

#### （7）事务JobStoreTX (ADO.NET)

AdoJobStore 用于在关系数据库中存储调度信息（作业、触发器和日历）。实际上有两个单独的 AdoJobStore 实现可供您选择，具体取决于您需要的事务行为。

JobStoreTX 通过在每次操作（例如添加作业）后调用`Commit()`（或）数据库连接来管理所有事务本身。`Rollback()`这是您通常应该使用的作业存储，除非您想集成到某些事务感知框架。

JobStoreTX 是通过如下设置`quartz.jobStore.type`属性来选择的：

**将 Scheduler 的 JobStore 设置为 JobStoreTX**

```ini
quartz.jobStore.type = Quartz.Impl.AdoJobStore.JobStoreTX, Quartz
```

JobStoreTX 可以使用以下属性进行调整：

| 属性名称                                     | 必需的 | 类型    | 默认值                                                       |
| -------------------------------------------- | ------ | ------- | ------------------------------------------------------------ |
| quartz.jobStore.dbRetryInterval              | no     | long    | 15000 (15 seconds)                                           |
| quartz.jobStore.driverDelegateType           | yes    | string  | null                                                         |
| quartz.jobStore.dataSource                   | yes    | string  | null                                                         |
| quartz.jobStore.tablePrefix                  | no     | string  | "QRTZ_"                                                      |
| quartz.jobStore.useProperties                | no     | boolean | false                                                        |
| quartz.jobStore.misfireThreshold             | no     | int     | 60000                                                        |
| quartz.jobStore.clustered                    | no     | boolean | false                                                        |
| quartz.jobStore.clusterCheckinInterval       | no     | long    | 15000                                                        |
| quartz.jobStore.maxMisfiresToHandleAtATime   | no     | int     | 20                                                           |
| quartz.jobStore.selectWithLockSQL            | no     | string  | "SELECT * FROM {0}LOCKS WHERE SCHED_NAME = {1} AND LOCK_NAME = ? FOR UPDATE" |
| quartz.jobStore.txIsolationLevelSerializable | no     | boolean | false                                                        |
| quartz.jobStore.acquireTriggersWithinLock    | no     | boolean | false (or true - see doc below)                              |
| quartz.jobStore.lockHandler.type             | no     | string  | null                                                         |
| quartz.jobStore.driverDelegateInitString     | no     | string  | null                                                         |

`quartz.scheduler.dbRetryInterval`

是调度程序在检测到 JobStore 中的连接丢失（例如到数据库）时将在重试之间等待的时间量（以毫秒为单位）。在使用 RamJobStore 时，这个参数显然不是很有意义。

`quartz.jobStore.driverDelegateType`

驱动程序代表了解不同数据库系统的特定“方言”。可能的内置选项包括：

- Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz - 没有特定实现时的默认值
- Quartz.Impl.AdoJobStore.SqlServerDelegate，Quartz - 适用于 Microsoft SQL Server
- Quartz.Impl.AdoJobStore.PostgreSQLDelegate, Quartz
- Quartz.Impl.AdoJobStore.OracleDelegate, Quartz
- Quartz.Impl.AdoJobStore.SQLiteDelegate, Quartz
- Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz

`quartz.jobStore.dataSource`

此属性的值必须是配置属性文件中定义的数据源之一的名称。

`quartz.jobStore.tablePrefix`

AdoJobStore 的“表前缀”属性是一个字符串，它等于在您的数据库中创建的 Quartz 表的前缀。如果它们使用不同的表前缀，您可以在同一个数据库中拥有多组 Quartz 的表。

**在 tablePrefix 中包含模式名称**

对于支持模式的后备数据库（例如 Microsoft SQL Server），您可以使用 tablePrefix 来包含模式名称。即对于名为`foo`前缀的模式可以设置为：

```ini
[foo].QRTZ_
```

**注意：**任何使用显式架构（例如`dbo`）运行的数据库表创建脚本都需要修改以反映此配置。

`quartz.jobStore.useProperties`

“使用属性”标志指示 AdoJobStore JobDataMaps 中的所有值都是字符串，因此可以存储为名称-值对，而不是将更复杂的对象以其序列化形式存储在 BLOB 列中。这很方便，因为您避免了将非字符串类型序列化为 BLOB 时可能出现的类型版本控制问题。

`quartz.jobStore.misfireThreshold`

在被视为“未触发”之前，调度程序将“容忍”触发器通过其下一次触发时间的毫秒数。默认值（如果您没有在配置中输入此属性）是 60000（60 秒）。

`quartz.jobStore.clustered`

设置为“true”以打开聚类功能。如果您有多个 Quartz 实例使用同一组数据库表，则此属性必须设置为“true”......否则您将遇到严重破坏。有关更多信息，请参阅集群的配置文档。

`quartz.jobStore.clusterCheckinInterval`

设置此实例与集群的其他实例“签入”* 的频率（以毫秒为单位）。影响检测失败实例的速度。

`quartz.jobStore.maxMisfiresToHandleAtATime`

作业存储在给定通道中将处理的最大未触发触发器数。一次处理许多（超过几十个）可能会导致数据库表被锁定足够长的时间，以至于触发其他（尚未触发的）触发器的性能可能会受到阻碍。

`quartz.jobStore.selectWithLockSQL`

必须是选择“LOCKS”表中的一行并在该行上放置锁的 SQL 字符串。如果未设置，默认为“SELECT * FROM {0}LOCKS WHERE SCHED_NAME = {1} AND LOCK_NAME = ? FOR UPDATE”，适用于大多数数据库。“{0}”在运行时被您在上面配置的 TABLE_PREFIX 替换。“{1}”替换为调度程序的名称。

`quartz.jobStore.txIsolationLevelSerializable`

“true”值告诉 Quartz（使用 JobStoreTX 或 CMT 时）设置事务级别以在 ADO.NET 连接上进行序列化。这有助于防止某些数据库在高负载和“持久”事务下发生锁定超时。

`quartz.jobStore.acquireTriggersWithinLock`

获取下一个要触发的触发器是否应在显式数据库锁内发生。这曾经是必要的（在以前的 Quartz 版本中）以避免特定数据库的死锁，但不再被认为是必要的，因此默认值为“false”。

如果 "quartz.scheduler.batchTriggerAcquisitionMaxCount" 设置为 > 1，并且使用 AdoJobStore，则必须将此属性设置为 "true" 以避免数据损坏（从 Quartz 2 开始，如果设置了 batchTriggerAcquisitionMaxCount，则默认设置为 "true" > 1）。

`quartz.jobStore.lockHandler.type`

用于生成`Quartz.Impl.AdoJobStore.ISemaphore`用于锁定作业存储数据控制的实例的类型名称。这是一项高级配置功能，大多数用户不应该使用它。

默认情况下，Quartz 会选择最合适的（预先捆绑的）信号量实现来使用。

**自定义StdRowLockSemaphore** 

如果您明确选择使用此 DB Semaphore，您可以进一步自定义轮询 DB 锁的频率。

**使用自定义 StdRowLockSemaphore 实现的示例**

```ini
quartz.jobStore.lockHandler.type = Quartz.Impl.AdoJobStore.StdRowLockSemaphore
quartz.jobStore.lockHandler.maxRetry = 7     # Default is 3
quartz.jobStore.lockHandler.retryPeriod = 3000  # Default is 1000 millis
```

`quartz.jobStore.driverDelegateInitString`

可以在初始化期间传递给 DriverDelegate 的以竖线分隔的属性（及其值）列表。字符串的格式如下：

```ini
settingName=settingValue|otherSettingName=otherSettingValue|...
```

StdAdoDelegate 及其所有后代（Quartz 附带的所有委托）都支持一个名为“triggerPersistenceDelegateTypes”的属性，该属性可以设置为以逗号分隔的类型列表，这些类型实现了`ITriggerPersistenceDelegate`用于存储自定义触发器类型的接口。请参阅实现`SimplePropertiesTriggerPersistenceDelegateSupport`和`SimplePropertiesTriggerPersistenceDelegateSupport`为自定义触发器编写持久性委托的示例。

#### （8）持久化AdoJobstore

如果您使用的是 AdoJobstore，则需要一个 DataSource 供其使用（或两个 DataSource，如果您使用的是 JobStoreCMT）。

您定义的每个 DataSource（通常是一个或两个）都必须指定一个名称，并且您为每个定义的属性必须包含该名称，如下所示。DataSource 的“NAME”可以是任何你想要的，除了在分配给 AdoJobStore 时能够识别它之外没有任何意义。

Quartz 创建的数据源定义了以下属性：

| 属性名称                                       | 必需的 | 类型   | 默认值 |
| ---------------------------------------------- | ------ | ------ | ------ |
| quartz.dataSource.NAME.provider                | yes    | string |        |
| quartz.dataSource.NAME.connectionString        |        | string |        |
| quartz.dataSource.NAME.connectionStringName    |        | string |        |
| quartz.dataSource.NAME.connectionProvider.type |        | string |        |

`quartz.dataSource.NAME.provider`

目前支持以下数据库提供程序：

- `SqlServer`- 微软 SQL 服务器
- `OracleODP`- 甲骨文的甲骨文驱动程序
- `OracleODPManaged`- 适用于 Oracle 11 的 Oracle 托管驱动程序
- `MySql`- MySQL 连接器/.NET
- `SQLite`- SQLite ADO.NET 提供程序
- `SQLite-Microsoft`- Microsoft SQLite ADO.NET 提供程序
- `Firebird`- Firebird ADO.NET 提供程序
- `Npgsql`- PostgreSQL Npgsql

`quartz.dataSource.NAME.connectionString`

要使用的 ADO.NET 连接字符串。如果您在下面使用 connectionStringName，则可以跳过此步骤。

`quartz.dataSource.NAME.connectionStringName`

要使用的连接字符串名称。在 app.config 或 appsettings.json 中定义。

`quartz.dataSource.NAME.connectionProvider.type`

允许您定义实现 IDbProvider 接口的自定义连接提供程序。

**Quartz 定义的数据源示例**

```ini
quartz.dataSource.myDS.provider = SqlServer
quartz.dataSource.myDS.connectionString = Server=localhost;Database=quartznet;User Id=quartznet;Password=quartznet;
```

#### （9）集群cluster

Quartz 的集群功能通过故障转移和负载平衡功能为您的调度程序带来高可用性和可扩展性。

集群目前仅适用于 AdoJobstore (`JobStoreTX`或`JobStoreCMT`)，并且本质上是通过让集群的每个节点共享相同的数据库来工作的。

负载平衡会自动发生，集群的每个节点都会尽快触发作业。当触发器的触发时间发生时，获取它的第一个节点（通过对其加锁）就是将触发它的节点。

每次触发时只有一个节点会触发作业。我的意思是，如果作业有一个重复触发器，告诉它每 10 秒触发一次，那么在 12:00:00 恰好一个节点将运行该作业，而在 12:00:10 恰好一个节点将运行作业等。它不一定每次都是同一个节点 - 哪个节点运行它或多或少是随机的。负载平衡机制对于繁忙的调度程序（很多触发器）是近乎随机的，但对于非繁忙（例如，很少触发器）调度程序有利于相同的节点。

当其中一个节点在执行一项或多项作业期间发生故障时，就会发生故障转移。当一个节点发生故障时，其他节点会检测该状况并识别数据库中故障节点中正在进行的作业。任何标记为要恢复的作业（在 JobDetail 上具有“requests recovery”属性）将由其余节点重新执行。未标记为恢复的作业将在下次触发相关触发器时被释放以供执行。

集群功能最适合扩展长时间运行和/或 CPU 密集型作业（将工作负载分配到多个节点）。如果您需要向外扩展以支持数千个短期运行（例如 1 秒）的作业，请考虑使用多个不同的调度程序（包括用于 HA 的多个集群调度程序）对作业集进行分区。调度程序使用集群范围的锁，这种模式会随着您添加更多节点而降低性能（当超过大约三个节点时 - 取决于您的数据库的功能等）。

通过将属性`quartz.jobStore.clustered`设置为“true”来启用集群。集群中的每个实例都应该使用相同的属性副本。只有线程池大小和`quartz.scheduler.instanceId`可以不同，集群中的每个节点都必须有一个唯一的 instanceId，可以设置成`AUTO`让系统自动分配。

**集群调度程序的示例属性**

```ini
#============================================================================
# Configure Main Scheduler Properties
#============================================================================

quartz.scheduler.instanceName = MyClusteredScheduler
quartz.scheduler.instanceId = AUTO

#============================================================================
# Configure ThreadPool
#============================================================================

quartz.threadPool.type = Quartz.Simpl.DefaultThreadPool, Quartz
quartz.threadPool.threadCount = 25
quartz.threadPool.threadPriority = 5

#============================================================================
# Configure JobStore
#============================================================================

quartz.jobStore.misfireThreshold = 60000

quartz.jobStore.type = Quartz.Impl.AdoJobStore.JobStoreTX
quartz.jobStore.driverDelegateType = Quartz.Impl.AdoJobStore.SqlServerDelegate
quartz.jobStore.useProperties = true
quartz.jobStore.dataSource = myDS
quartz.jobStore.tablePrefix = QRTZ_

quartz.jobStore.clustered = true
quartz.jobStore.clusterCheckinInterval = 20000

#============================================================================
# Configure Datasources
#============================================================================

quartz.dataSource.myDS.provider = SqlServer
quartz.dataSource.myDS.connectionString = Server=localhost;Database=quartznet;User Id=quartznet;Password=quartznet;
```

### 16、最佳实践

#### （1）JobDataMap

> 仅在 JobDataMap 中存储原始数据类型（包括字符串）

仅在 JobDataMap 中存储原始数据类型（包括字符串）以避免短期和长期的数据序列化问题。

> 使用合并的 JobDataMap

在 Job 执行期间找到的 JobDataMap`JobExecutionContext`起到了方便的作用。它是在 JobDetail 上找到的 JobDataMap 和在 Trigger 上找到的 JobDataMap 的合并，后者中的值会覆盖前者中的任何同名值。

如果您有一个作业存储在调度程序中以供多个触发器定期/重复使用，但对于每个独立的触发，您希望为作业提供不同的数据输入，则在触发器上存储 JobDataMap 值可能很有用。

鉴于上述所有情况，我们推荐以下最佳实践：`IJob.Execute(..)`方法中的代码通常应从 JobExecutionContext 上的 JobDataMap 中检索值，而不是直接从 JobDetail 上的那个中检索值。

#### （2）触发器trigger

> 使用 TriggerUtils

触发器工具：

- 提供一种创建日期的简单方法（用于开始/结束日期）
- 提供分析触发器的助手（例如计算未来的触发时间）

#### （4）持久化AdoJobStore

> 永远不要直接写入 Quartz 的表

将调度数据直接写入数据库（通过 SQL）而不是使用调度 API：

- 导致数据损坏（已删除数据、加扰数据）
- 当触发器的触发时间到达时，导致作业看似“消失”而不执行
- 当触发器的触发时间到达时，导致作业不执行“只是坐在那里”
- 可能导致：死锁
- 其他奇怪的问题和数据损坏

> 切勿将非集群调度程序与具有相同调度程序名称的另一个调度程序指向同一数据库

如果您将多个调度程序实例指向同一组数据库表，并且其中一个或多个实例未配置为集群，则可能会发生以下任何情况：

- 导致数据损坏（已删除数据、加扰数据）
- 当触发器的触发时间到达时，导致作业看似“消失”而不执行
- 当触发器的触发时间到达时，导致作业未执行，“只是坐在那里”
- 可能导致：死锁
- 其他奇怪的问题和数据损坏

> 确保足够的数据源连接大小

建议您的 Datasource 最大连接大小至少配置为线程池中的工作线程数加 3。如果您的应用程序还频繁调用调度程序 API，您可能需要额外的连接。

#### （5）夏令时

> 避免在夏令时转换时间附近安排作业

注意：转换时间的细节和时钟向前或向后移动的时间量因地区而异，请参阅：[https ://secure.wikimedia.org/wikipedia/en/wiki/Daylight_saving_time_around_the_world](https://secure.wikimedia.org/wikipedia/en/wiki/Daylight_saving_time_around_the_world).

SimpleTriggers 不受夏令时的影响，因为它们总是以精确的毫秒时间触发，并重复精确的毫秒数。

因为 CronTriggers 在给定的小时/分钟/秒触发，所以当 DST 转换发生时它们会受到一些奇怪的影响。

作为可能问题的示例，在美国的时区/位置安排遵守夏令时，如果在凌晨 1:00 和凌晨 2:00 使用 CronTrigger 并安排火灾时间，可能会出现以下问题：

- 凌晨 1:05 可能会出现两次！- 可能在 CronTrigger 上重复触发
- 凌晨 2:05 可能永远不会发生！- 可能错过 CronTrigger 上的触发

同样，调整的具体时间和数量因地区而异。

基于沿日历滑动（而不是精确的时间量）的其他触发器类型，例如 CalenderIntervalTrigger，将受到类似影响 - 但不是错过一次触发或触发两次，最终可能会使其触发时间偏移一个小时。

#### （6）作业job

> 等待条件

长时间运行的作业会阻止其他作业运行（如果 ThreadPool 中的所有线程都忙）。

如果您觉得需要在执行作业的工作线程上调用 Thread.sleep()，这通常表明作业尚未准备好完成其余工作，因为它需要等待某些条件（例如数据记录的可用性）成为真实。

更好的解决方案是释放工作线程（退出作业）并允许其他作业在该线程上执行。该作业可以在退出之前重新安排自己或其他作业。

> 抛出异常

Job 的执行方法应该包含一个处理所有可能异常的 try-catch 块。

如果作业抛出异常，Quartz 通常会立即重新执行它，这意味着该作业可以并且很可能会再次抛出相同的异常。这可能会导致资源浪费，在最坏的情况下，还会导致应用程序不稳定或崩溃。如果作业捕获它可能遇到的所有异常、处理它们并重新安排自己或其他作业以解决该问题，那就更好了。

> 可恢复性和幂等性

在调度程序失败后，标记为“可恢复”的进行中作业会自动重新执行。这意味着某些作业的“工作”将被执行两次。

这意味着作业应该以使其工作是幂等的方式编码。

#### （7）监听器Listener

> 保持监听器中的代码简洁高效

不鼓励执行大量工作，因为将执行作业（或完成触发器并继续触发另一个作业等）的线程将被绑定在侦听器中。

> 处理异常

每个侦听器方法都应包含一个处理所有可能异常的 try-catch 块。

如果某个监听器抛出异常，可能会导致其他监听器无法被通知和/或阻止作业的执行等。

#### （8）公开调度程序功能

一些用户通过应用程序用户界面公开 Quartz 的调度程序功能。这可能非常有用，尽管它也可能非常危险。

确保您不会错误地允许用户使用他们想要的任何参数来定义他们想要的任何类型的作业。例如，Quartz.Jobs 包附带了一个预制作业`NativeJob`，它将执行它定义的任意本机（操作系统）系统命令。恶意用户可以使用它来控制或破坏您的系统。

同样，其他作业（例如`SendEmailJob`，以及几乎任何其他作业）都可能被用于恶意目的。

允许用户定义他们想要的任何作业，从而有效地打开您的系统，从而使您的系统容易受到与 OWASP 和 MITRE 定义的命令注入攻击相当/等效的各种漏洞的攻击。

### 17、实现UI管理任务调度

​			代码第一条，不要自己造轮子，懂了原理之后，就去github上面找开源的项目，如果有找那个写的最好的拿过来修改下为我所用，如果找不到，那就恭喜你，你的想法可能是个没人想过的创意。

​         推荐一个好的开源项目：[https://github.com/zhaopeiym/quartzui](https://github.com/zhaopeiym/quartzui)

**本项目实现了如下需求**

- 基于.NET6和Quartz.NET3.2.4的任务调度Web界面管理。
- docker方式开箱即用
- 内置SQLite持久化
- 支持 RESTful风格接口
- 业务代码零污染
- 语言无关
- 傻瓜式配置
- 异常请求邮件通知

**更换数据源** 

默认使用的是SQLite，如果需要使用其他数据源请自行在appsettings.json进行正确配置。如：  

```json
"dbProviderName":"OracleODPManaged",
"connectionString": "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=xe)));User Id=system;Password=oracle;";

"dbProviderName":"SqlServer",
"connectionString": "Server=localhost;Database=quartznet;User Id={SqlServerUser};Password={SqlServerPassword};";

"dbProviderName":"SQLServerMOT",
"connectionString": "Server=localhost,1444;Database=quartznet;User Id={SqlServerUser};Password={SqlServerPassword};"

"dbProviderName":"MySql", // MySql 测试通过
"connectionString": "Server = localhost; Database = quartznet; Uid = quartznet; Pwd = quartznet";

"dbProviderName":"Npgsql", // Npgsql 测试通过
"connectionString": "Server=127.0.0.1;Port=5432;Userid=quartznet;Password=quartznet;Pooling=true;MinPoolSize=1;MaxPoolSize=20;Timeout=15;SslMode=Disable;Database=quartznet";

"dbProviderName":"SQLite",
"connectionString": "Data Source=test.db;Version=3;";

"dbProviderName":"SQLite-Microsoft", // SQLite-Microsoft 测试通过
"connectionString": "Data Source=test.db;";

"dbProviderName":"Firebird",
"connectionString": "User=SYSDBA;Password=masterkey;Database=/firebird/data/quartz.fdb;DataSource=localhost;Port=3050;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;";
```

#### （1）下载源码

git相关知识请参考[git详解](https://blog.csdn.net/liyou123456789/article/details/121411053)

![image-20220828170513962](http://cdn.bluecusliyou.com/202208281705097.png)

#### （2）前端项目编译打包

​			进入前端项目，右键vscode打开，进入命令终端，`npm install`先还原包，再`npm run build`将代码打包进dist文件夹，相关前端工程化的知识请自行学习。

​			将生成的dist中的前端文件全部拷贝到后台接口项目的wwwroot文件夹下

#### （3）配置首次登录的用户密码

密码是存储在文件中的，可以自己设置一个初始密码

![image-20220828171929580](http://cdn.bluecusliyou.com/202208281719660.png)

#### （4）发布启动项目

发布项目到一个具体的路径`D:\NetDemos\quartzui`下，然后在发布的目录下执行命令，启动项目。

```bash
dotnet Host.dll --urls=http://*:8100
```

#### （5）运行结果

浏览器输入[http://localhost:8100/](http://localhost:8100/)

![image-20220828210655730](http://cdn.bluecusliyou.com/202208282106802.png)

输入密码admin登录系统

![image-20220828210814929](http://cdn.bluecusliyou.com/202208282108035.png)

可以进行任务的添加，修改，执行，删除，暂停等操作

![image-20220828210959207](http://cdn.bluecusliyou.com/202208282109286.png)

![image-20220828211102298](http://cdn.bluecusliyou.com/202208282111381.png)

### 18、部署程序

#### （1）IIS自动回收问题解决

如果是netframwork的web程序是需要挂载在IIS里面的，但是IIS会进行自动回收，默认回收是1740分钟，也就是29小时。IIS自动回收相当于IIS重启，应用程序池内存清空，所有数据被清除，为了减小数据库负担，内存中暂存了很多信息，不适合频繁的回收，如果没有及时保存到数据库中，可能导致程序出现问题。

**解决方案：关闭该项目在IIS上对应的进程池的回收机制。**

选中IIS中部署的项目对应的进程池，点击【高级设置】，里面有5个核心参数：

- 发生配置更改时禁止回收：如果为True,应用程序池在发生配置更改时将不会回收。
- 固定时间间隔（分钟）：超过设置的时间后，应用程序池回收，设置为0意味着应用程序池不回收。系统默认设置的时间是1740（29小时）。
- 禁用重叠回收：如果为true，将发生应用程序池回收，以便在创建另一个工作进程之前退出现有工作进程
- 请求限制：应用程序池在回收之前可以处理的最大请求数。如果值为0，则表示应用程序池可以处理的请求数没有限制。
- 生成回收事件日志条目：每发生一次指定的回收事件时便产生一个事件日志条目。

![image-20220818151235976](http://cdn.bluecusliyou.com/202208181512112.png)

**即使可以将IIS进程池回收关掉，仍然不建议把Quartz挂到IIS下，长时间不回收，会存在其他问题。所以推荐NetCore实质上是控制台程序，不存在内存回收问题。**

#### （2）部署成服务

​			如果是exe程序或者是netcore的web程序可以部署成系统服务。可以借助nssm工具来实现，nssm相关知识可以参考[nssm详解](https://blog.csdn.net/liyou123456789/article/details/123094277)，现在将上面的ui管理程序做成一个服务。

![image-20220828211910313](http://cdn.bluecusliyou.com/202208282119380.png)

![image-20220828212022053](http://cdn.bluecusliyou.com/202208282120125.png)

浏览器输入[http://localhost:8100/](http://localhost:8100/)正常访问

![image-20220828212132338](http://cdn.bluecusliyou.com/202208282121410.png)

#### （3）容器化运行

docker相关知识请参考[docker详解](https://blog.csdn.net/liyou123456789/article/details/122292877)，Linux相关知识请参考[Linux详解](https://blog.csdn.net/liyou123456789/article/details/121548156)

```bash
docker run -v /fileData/quartzuifile:/app/File  --restart=unless-stopped --privileged=true --name quartzui -dp 5088:80 bennyzhao/quartzui

一行命令开箱即用，赶快体验下docker的便捷吧！
1、其中/fileData/quartzuifile为映射的文件地址，如SQLite数据库和log日志
2、5088为映射到主机的端口
3、直接在浏览器 ip:5088 即可访问。（注意防火墙是否打开了5088端口，或者在主机测试 curl 127.0.0.1:5088）
```

![image-20220828213156709](http://cdn.bluecusliyou.com/202208282131871.png)

浏览器输入`http://服务器IP:5088/`正常访问

![image-20220828213252889](http://cdn.bluecusliyou.com/202208282132961.png)
