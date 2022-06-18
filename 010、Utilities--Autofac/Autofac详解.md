# Autofac详解

## 一、Autofac详解

### 1、概述

- Autofac是第三方IOC容器，是当前最流行的IOC容器。
- 功能强大，比asp.netcore内置容器强大得多，支持属性注入和方法注入，支持AOP。
- 官网地址：[http://autofac.org/](http://autofac.org/)
- 源码下载地址：[https://github.com/autofac/Autofac](https://github.com/autofac/Autofac)

### 2、快速开始

#### （1）Nuget引入程序包

```bash
Autofac   基于版本6.3演示，Net5
```

#### （2）容器创建对象

```c#
//创建一个容器建造者
ContainerBuilder containerBuilder = new ContainerBuilder();
//注册普通类
containerBuilder.RegisterType<Honer>();
//build一下，得到一个容器
IContainer container = containerBuilder.Build();
//可以基于容器来获取对象的实例
Honer phone = container.Resolve<Honer>();
```

### 3、注册的类型

#### （1）注册普通类

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>();
IContainer container = containerBuilder.Build();
Honer phone = container.Resolve<Honer>();
```

#### （2）注册抽象与实现

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>().As<IPhone>();
IContainer container = containerBuilder.Build();
IPhone phone = container.Resolve<IPhone>();
```

#### （3）注册程序集

> - RegisterAssemblyTypes(程序集数组)，程序集必须是public的
> - AsImplementedInterfaces()：表示注册的类型，以接口的方式注册
> - PropertiesAutowired()：支持属性注入
> - Where：满足条件类型注册

```C#
var basePath = AppContext.BaseDirectory;
var dll = Path.Combine(basePath, "MyAutofac.dll");
ContainerBuilder containerBuilder = new ContainerBuilder();
var assemblysServices = Assembly.LoadFrom(dll);
containerBuilder.RegisterAssemblyTypes(assemblysServices)
    .Where(t => !t.Name.EndsWith("XXX"))
    .AsImplementedInterfaces()
    .PropertiesAutowired();                        
IContainer container = containerBuilder.Build();
ITeacher teacher = container.Resolve<ITeacher>();
```

![image-20220615171348335](http://cdn.bluecusliyou.com/202206151713394.png)

### 4、三种注入方式

#### （1）构造函数注入

> 默认支持，无法用特性进行筛选，默认选参数最多的构造函数进行注入

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>().As<IPhone>();
containerBuilder.RegisterType<Teacher>().As<ITeacher>();
containerBuilder.RegisterType<Student>().As<IStudent>();
IContainer container = containerBuilder.Build();
ITeacher teacher = container.Resolve<ITeacher>();
```

![image-20220507144924795](http://cdn.bluecusliyou.com/202206101644974.png)

#### （2）全部属性注入

> 关键词`PropertiesAutowired`，这个对象所有属性全部注入

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>().As<IPhone>();
containerBuilder.RegisterType<Teacher>().As<ITeacher>().PropertiesAutowired();
containerBuilder.RegisterType<Student>().As<IStudent>();
IContainer container = containerBuilder.Build();
ITeacher teacher = container.Resolve<ITeacher>();
```

![image-20220507145543126](http://cdn.bluecusliyou.com/202206101644016.png)

#### （3）标记特性的属性注入

> 关键词`PropertiesAutowired`，定义特性选择器`CustomPropertySelector`

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>().As<IPhone>();
containerBuilder.RegisterType<Teacher>().As<ITeacher>().PropertiesAutowired(new CustomPropertySelector());
containerBuilder.RegisterType<Student>().As<IStudent>();
IContainer container = containerBuilder.Build();
ITeacher teacher = container.Resolve<ITeacher>();
```

```C#
public class CustomPropertySelector : IPropertySelector
{
    public bool InjectProperty(PropertyInfo propertyInfo, object instance)
    {
        var flag = propertyInfo.CustomAttributes.Any(it => it.AttributeType == typeof(SelectPropAttribute));
        return flag;
    }
}
```

![image-20220507151030788](http://cdn.bluecusliyou.com/202206101644054.png)

#### （4）方法注入

> 关键词`OnActivated`，指定调用方法

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>().As<IPhone>();
containerBuilder.RegisterType<Teacher>().As<ITeacher>()
    .OnActivated(p =>
    {
        p.Instance.SetStudent1(p.Context.Resolve<IStudent>());
    });
containerBuilder.RegisterType<Student>().As<IStudent>();
IContainer container = containerBuilder.Build();
ITeacher teacher = container.Resolve<ITeacher>();
```

![image-20220507154431887](http://cdn.bluecusliyou.com/202206101644009.png)

### 5、对象生命周期

#### （1）瞬时生命周期

> 每次获取都是全新的实例，关键词`InstancePerDependency`，默认的生命周期

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>().As<IPhone>().InstancePerDependency();
IContainer container = containerBuilder.Build();
IPhone phone1 = container.Resolve<IPhone>();
IPhone phone2 = container.Resolve<IPhone>();
bool isflg1 = object.ReferenceEquals(phone1, phone2);
Console.WriteLine($"InstancePerDependency：phone1==phone2=>{isflg1}");
```

```bash
InstancePerDependency：phone1==phone2=>False
```

#### （2）单例生命周期

> 同一个进程内都是同一个实例，关键词`SingleInstance`

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>().As<IPhone>().SingleInstance();
IContainer container = containerBuilder.Build();
IPhone phone1 = container.Resolve<IPhone>();
IPhone phone2 = container.Resolve<IPhone>();
bool isflg1 = object.ReferenceEquals(phone1, phone2);
Console.WriteLine($"SingleInstance：phone1==phone2=>{isflg1}");
using (var scope = container.BeginLifetimeScope())
{
    IPhone phone3 = scope.Resolve<IPhone>();
    IPhone phone4 = scope.Resolve<IPhone>();
    bool isflg2 = object.ReferenceEquals(phone3, phone4);
    Console.WriteLine($"SingleInstance：phone3==phone4=>{isflg2}");
    bool isflg3 = object.ReferenceEquals(phone1, phone3);
    Console.WriteLine($"SingleInstance：phone1==phone3=>{isflg3}");
}
```

```bash
SingleInstance：phone1==phone2=>True
SingleInstance：phone3==phone4=>True
SingleInstance：phone1==phone3=>True
```

#### （3）作用域生命周期

> 同一个作用域内都是同一个实例，关键词`InstancePerLifetimeScope`

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>().As<IPhone>().InstancePerLifetimeScope();
IContainer container = containerBuilder.Build();
IPhone phone1 = container.Resolve<IPhone>();
IPhone phone2 = container.Resolve<IPhone>();
bool isflg1 = object.ReferenceEquals(phone1, phone2);
Console.WriteLine($"InstancePerLifetimeScope：phone1==phone2=>{isflg1}");
IPhone phone3 = null;
IPhone phone4 = null;
using (var scope = container.BeginLifetimeScope())
{
    phone3 = scope.Resolve<IPhone>();
    phone4 = scope.Resolve<IPhone>();
    bool isflg2 = object.ReferenceEquals(phone3, phone4);
    Console.WriteLine($"InstancePerLifetimeScope：phone3==phone4=>{isflg2}");
    bool isflg3 = object.ReferenceEquals(phone1, phone3);
    Console.WriteLine($"InstancePerLifetimeScope：phone1==phone3=>{isflg3}");
}
IPhone phone5 = null;
IPhone phone6 = null;
using (var scope = container.BeginLifetimeScope())
{
    phone5 = scope.Resolve<IPhone>();
    phone6 = scope.Resolve<IPhone>();
    bool isflg2 = object.ReferenceEquals(phone5, phone6);
    Console.WriteLine($"InstancePerLifetimeScope：phone5==phone6=>{isflg2}");
    bool isflg3 = object.ReferenceEquals(phone1, phone5);
    Console.WriteLine($"InstancePerLifetimeScope：phone1==phone5=>{isflg3}");
}
bool isflg4 = object.ReferenceEquals(phone3, phone5);
Console.WriteLine($"InstancePerLifetimeScope：phone3==phone5=>{isflg4}");
```

```bash
InstancePerLifetimeScope：phone1==phone2=>True
InstancePerLifetimeScope：phone3==phone4=>True
InstancePerLifetimeScope：phone1==phone3=>False
InstancePerLifetimeScope：phone5==phone6=>True
InstancePerLifetimeScope：phone1==phone5=>False
InstancePerLifetimeScope：phone3==phone5=>False
```

#### （4）作用域范围生命周期

> 在作用域范围外无法创建实例，在作用域范围里面，同一个作用域下面的对象是同一个，关键词`InstancePerMatchingLifetimeScope`

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>().As<IPhone>().InstancePerMatchingLifetimeScope("scope1", "scope2");
IContainer container = containerBuilder.Build();
IPhone phone1 = null;
IPhone phone2 = null;
using (var scope = container.BeginLifetimeScope("scope1"))
{
    phone1 = scope.Resolve<IPhone>();
    phone2 = scope.Resolve<IPhone>();
    bool isflg1 = object.ReferenceEquals(phone1, phone2);
    Console.WriteLine($"InstancePerMatchingLifetimeScope：phone1==phone2=>{isflg1}");
}
IPhone phone3 = null;
using (var scope = container.BeginLifetimeScope("scope2"))
{
    phone3 = scope.Resolve<IPhone>();
}                   
IPhone phone4 = null;
using (var scope = container.BeginLifetimeScope("scope2"))
{                        
    phone4 = scope.Resolve<IPhone>();
}
bool isflg2 = object.ReferenceEquals(phone3, phone4);
Console.WriteLine($"InstancePerMatchingLifetimeScope：phone3==phone4=>{isflg2}");
bool isflg3 = object.ReferenceEquals(phone1, phone3);
Console.WriteLine($"InstancePerMatchingLifetimeScope：phone1==phone3=>{isflg3}");
```

```bash
InstancePerMatchingLifetimeScope：phone1==phone2=>True
InstancePerMatchingLifetimeScope：phone3==phone4=>False
InstancePerMatchingLifetimeScope：phone1==phone3=>False
```

#### （5）一次请求同一个对象

> 关键词`InstancePerRequest`，只能在web项目中调试，控制台报错

```c#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>().As<IPhone>().InstancePerRequest();
IContainer container = containerBuilder.Build();
IPhone phone1 = container.Resolve<IPhone>();
IPhone phone2 = container.Resolve<IPhone>();
bool isflg1 = object.ReferenceEquals(phone1, phone2);
Console.WriteLine($"InstancePerRequest：phone1==phone2=>{isflg1}");
```

### 6、支持配置文件注册

#### （1）nuget引入程序集

```bash
Autofac
Autofac.Configuration
Microsoft.Extensions.Configuration.Json
```

#### （2）配置文件autofac.json，属性->始终复制

```json
{
  "components": [
    {
      //实现
      "type": "Net5.IOC.Honer,Net5.IOC",
      //抽象
      "services": [
        {
          "type": "Net5.IOC.IPhone,Net5.IOC"
        }
      ],
      //生命周期
      "instanceScope": "single-instance",
      //属性注入 
      "injectProperties": true
    }
  ]
}
```

#### （3）通过配置文件注册创建对象

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
IConfigurationBuilder config = new ConfigurationBuilder();
IConfigurationSource autofacJsonConfigSource = new JsonConfigurationSource()
{
    Path = "Autofac/autofac.json",
    Optional = false,//boolean,默认就是false,可不写
    ReloadOnChange = true,//同上
};
config.Add(autofacJsonConfigSource);
ConfigurationModule module = new ConfigurationModule(config.Build());
containerBuilder.RegisterModule(module);
IContainer container = containerBuilder.Build();
IPhone phone1 = container.Resolve<IPhone>();
IPhone phone2 = container.Resolve<IPhone>();
bool isflg1 = object.ReferenceEquals(phone1, phone2);
Console.WriteLine($"配置文件注册：phone1==phone2=>{isflg1}");
```

#### （4）运行结果

```bash
配置文件注册：phone1==phone2=>True
```

### 7、支持AOP切面编程

可以在不修改方法的前提下，在方法前后添加公共逻辑，日志，异常，缓存等

#### （1）nuget引入程序集

```bash
Castle.Core
Autofac.Extras.DynamicProxy
```

#### （2）自定义一个切面类实现`IInterceptor`接口

```C#
public class CustomInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        Console.WriteLine("方法执行前。。。");
        //执行当前方法
        invocation.Proceed();
        Console.WriteLine("方法执行后。。。");
    }
}
```

#### （3）在抽象/实现类上添加特性标记

```C#
[Intercept(typeof(CustomInterceptor))]
public interface IPhone
{
    string ShowName();
}
//实现类虚方法
public class Honer : IPhone
{
    public virtual string ShowName()
    {
        Console.WriteLine("Honer");
        return "Honer";
    }
}
```

#### （4）在容器中注册关系创建对象

> - EnableInterfaceInterceptors + 特性标记在抽象上，所有实现类都支持AOP
> - EnableInterfaceInterceptors + 特性标记到实现类上，标记的类就支持AOP
> - EnableClassInterceptors，要支持AOP的方法必须要是用virtual虚方法
> - EnableClassInterceptors + 特性标记在抽象上，所有实现类都支持AOP
> - EnableClassInterceptors + 特性标记到实现类上，标记的类就支持AOP

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>().As<IPhone>().EnableInterfaceInterceptors();
containerBuilder.RegisterType(typeof(CustomInterceptor));
IContainer container = containerBuilder.Build();
IPhone phone = container.Resolve<IPhone>();
phone.ShowName();
```

#### （5）运行结果

```bash
方法执行前。。。
Honer
方法执行后。。。
```

### 8、单抽象多实现问题

#### （1）在容器中注册关系创建对象

> 注册的时候不标记名字，后注册的会覆盖先注册的
> 注册的时候标记下名字，创建对象的时候用名称来区分

```C#
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<Honer>().Named<IPhone>("Honer");
containerBuilder.RegisterType<Huawei>().Named<IPhone>("Huawei");
IContainer container = containerBuilder.Build();
IPhone honer = container.ResolveNamed<IPhone>("Honer");
IPhone huawei = container.ResolveNamed<IPhone>("Huawei");
honer.ShowName();
huawei.ShowName();
```

#### （2）运行结果

```C#
Honer
Huawei
```

### 9、集成到Asp.NetCore5框架

#### （1）nuget引入程序集

```bash
Autofac
Autofac.Extensions.DependencyInjection
```

#### （2）定义实现类和抽象

```C#
public class UserService : IUserService
{    
    private IUserRepository UserRepositoryCtor { get; set; }
    public UserService(IUserRepository userRepository)
    {
        UserRepositoryCtor = userRepository;
    }    
    public string Login(string username, string password)
    {
        return "登录成功";
    }
}
```

#### （3）添加控制器和页面

```C#
public class FourthController : Controller
{
    private IUserService _userService;
    public FourthController(IUserService userService)
    {
        this._userService = userService;
    }
    public IActionResult Index()
    {
        object result = this._userService.Login("username", "password");
        return View(result);
    }
}
```

```C#
@model String
<h2>this is fourth index...</h2>
<h2>@Model</h2>
```

#### （4）在Program替换容器工厂

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
            .UseServiceProviderFactory(new AutofacServiceProviderFactory());
}
```

#### （5）在`Startup`类的`ConfigureServices`方法中替换创建控制器的类

```C#
//控制器默认是有IControllerActivator创建的，替换成由容器创建
services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
```

#### （6）注册抽象和实现的关系

> 在`Startup`类中专门增加一个方法`ConfigureContainer`，用于注册抽象和实现，可以把这些注册信息进行模块化封装到`AutofacModule`
>
> 当抽象类和实现非常多的时候，可以将整个dll注册，特殊的关系可以写在后面覆盖前面的注册关系。

```C#
/// <summary>
/// Autofac专用：注册抽象和细节之间的关系，使用autofac后原来内置注册的关系要注释掉
/// Autofac和ServiceCollection是二者并存的，Autofac会接管ServiceCollection的一切
/// </summary>
/// <param name="builder"></param>
public void ConfigureContainer(ContainerBuilder builder)
{
    builder.RegisterModule<AutofacModule>();
}
```

```C#
public class AutofacModule : Module
{
    /// <summary>
    /// 重写Autofac管道中的Load方法，在这里注入注册的内容
    /// </summary>
    /// <param name="builder"></param>
    protected override void Load(ContainerBuilder builder)
    {
        //注册抽象与实现
        builder.RegisterType<UserRepository>().As<IUserRepository>();
        builder.RegisterType<UserService>().As<IUserService>();

        //注册所有控制器类
        var controllersTypesInAssembly = typeof(Startup).Assembly.GetExportedTypes()
         .Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();
        //实现属性注入，这边无法实现方法注入
        builder.RegisterTypes(controllersTypesInAssembly).PropertiesAutowired();
    }
}
```

#### （7）运行结果

![image-20220617130527205](http://cdn.bluecusliyou.com/202206171305410.png)

![image-20220509140121017](http://cdn.bluecusliyou.com/202206101644744.png)



#### （8）在`Startup`的`Configure`方法中用容器创建对象

```C#
using (var container = host.Services.CreateScope())
{
    IUserService userService = container.ServiceProvider.GetService<IUserService>();
}
```

![image-20220617131016831](http://cdn.bluecusliyou.com/202206171310893.png)

#### （9）单抽象多实现集成到框架

注入的时候通过构造函数或者属性注入autofac上下文实例，再根据名称创建对应实例。

> 定义单抽象和多实例类

```C#
//抽象
public interface ITestService
{
    string Show();
}
//实现1
public class TestServiceA : ITestService
{
    public string Show()
    {
        return "TestServiceA";
    }
}
//实现2
public class TestServiceB : ITestService
{
    public string Show()
    {
        return "TestServiceB";
    }
}
```

> 在`AutofacModule`注册抽象和实现的关系

```C#
//单抽象多实现注册
builder.RegisterType<TestServiceA>().Named<ITestService>("a");
builder.RegisterType<TestServiceB>().Named<ITestService>("b");
```

> 添加控制器和页面

```C#
public class FifthController : Controller
{
    private IComponentContext _componentContext;

    public FifthController(IComponentContext componentContext)
    {
        this._componentContext = componentContext;
    }

    public IActionResult Index()
    {
        ITestService testServiceA = _componentContext.ResolveNamed<ITestService>("a");
        ITestService testServiceB = _componentContext.ResolveNamed<ITestService>("b");
        object result = $"{testServiceA.Show()}--{testServiceB.Show()}";
        return View(result);
    }
}
```

```html
@model String
<h2>this is Fifth index...</h2>
<h2>@Model</h2>
```

> 运行结果

![image-20220510102820041](http://cdn.bluecusliyou.com/202206101644999.png) 
