

# AspNetCore5基础

## 一、.NET概述

### 1、.NET的历史脚步

![image-20210711212728240](assets/image-20210711212728240.png)

### 2、.NET5之前

![02f7d91e19e3f533acaaa6bd3e59816d](assets/02f7d91e19e3f533acaaa6bd3e59816d.jpeg)

### 3、.NET5统一了.NETCore和.NET Frameowork 

![4d87bb6e6a6bea6f70821f09b212cd2a](assets/4d87bb6e6a6bea6f70821f09b212cd2a.jpeg)

如果是.NET Core 3.1 升级到.NET5，只需要调整框架的版本，平滑升级，应用层变化小，主要是底层变化。

如果是Net Framwork升级NetCore就是比较大的变化，应用层底层都变化很大。

### 4、Net6进一步完善跨平台

![019f924c2f5e7f1bbccc6c17b3e7f772](assets/019f924c2f5e7f1bbccc6c17b3e7f772-16365548689231.jpeg)



### 5、源码地址

Github地址：[https://github.com/dotnet/aspnetcore/tree/main](https://github.com/dotnet/aspnetcore/tree/main)

### 6、安装Visual Studio

Visual Studio2019  16.8.2版本+   现在安装默认带有.NET5+的 CLR+依赖包

下载网址： [https://visualstudio.microsoft.com/zh-hans/vs/](https://visualstudio.microsoft.com/zh-hans/vs/)

## 二、新建项目

### 1、创建MVC项目

#### （1）创建MVC项目

![image-20211110221856248](assets/image-20211110221856248.png)

#### （2）设置项目名称位置

![image-20211110224012095](assets/image-20211110224012095.png)

#### （3）其他信息配置

![image-20211110224040031](assets/image-20211110224040031.png)

#### （4）项目创建完成

![image-20211110224129940](assets/image-20211110224129940.png)

#### （5）运行结果

![image-20211110224205208](assets/image-20211110224205208.png)

![image-20211110224327086](assets/image-20211110224327086.png)

#### （6）项目结构解读

- Connected Service：服务引用

- properties->launchSettings.json：项目启动需要的一些配置：包含了端口号，IP地址

- wwwroot：静态文件目录，js/css等

- 依赖项：项目依赖的程序集

- MVC：模型试图控制器
- appsettings：配置文件
- Program：控制台
- Startup：支持网站运行的一些相关配置（Net6已经没有了）

#### （7）MVC模式解读

- C: Controller：控制器，负责业务逻辑计算
- V: 表现层：用来展示各种结果，和用户互动
- M: 模型：串联在C--V之间，保存数据

#### （8）视图即时生效

- Nuget安装：Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation

- 在ConfigureServices方法中增加配置服务


~~~c#
//添加支持Razor即时编译
services.AddRazorPages().AddRazorRuntimeCompilation();
~~~

### 4、当前页面传值

#### （1）创建一个控制器

![image-20210711152633417](assets/image-20210711152633417.png)

![image-20210711152717913](assets/image-20210711152717913.png)

![image-20210711152826825](assets/image-20210711152826825.png)

#### （2）添加视图页面

![image-20210711152848563](assets/image-20210711152848563.png)

#### （3）控制器添加数据

```C#
using Microsoft.AspNetCore.Mvc;

namespace DemoNet5Mvc.Controllers
{
    public class FirstController : Controller
    {
        /// <summary>
        /// 当前页面传值
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {            
            ViewBag.User1 = "User1";
            ViewData["User2"] = "User2";
            TempData["User3"] = "User3";
            object User4 = "User4";
            return View(User4);
        }
    }
}
```

==User4如果用String类型定义的话，会报找不到视图，被当成了视图，所以一定要Object类型==

==ViewBag.User1和ViewData["User1"]指向的是同一个地址的数据，后者的赋值会覆盖前者的赋值==

==TempData是一次性的，第二次访问就没有了==

#### （4）Index视图页面代码

```C#
@model string
<h3> ViewBag.User1=  @base.ViewBag.User1 </h3>
<h3> ViewData["User2"]= @base.ViewData["User2"] </h3>
<h3> TempData["User3"]= @base.TempData["User3"] </h3>
<h3> Model= @Model</h3>
```

#### （5）修改默认路由

![image-20210719070225277](assets/image-20210719070225277.png)

#### （6）运行结果

![image-20210711153711334](assets/image-20210711153711334.png)

### 5、跨页面传值

#### （1）控制器添加Action--Second

```C#
using Microsoft.AspNetCore.Mvc;

namespace DemoNet5Mvc.Controllers
{
    public class FirstController : Controller
    {
        /// <summary>
        /// 当前页面传值
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {            
            ViewBag.User1 = "User1";
            ViewData["User2"] = "User2";
            TempData["User3"] = "User3";
            object User4 = "User4";
            return View(User4);
        }

        /// <summary>
        /// 跨页面传值
        /// </summary>
        /// <returns></returns>
        public IActionResult Second()
        {            
            return View();
        }
    }
}
```

#### （2）Second视图页面代码

```C#
@model string
<h3> ViewBag.User1=  @base.ViewBag.User1 </h3>
<h3> ViewData["User2"]= @base.ViewData["User2"] </h3>
<h3> TempData["User3"]= @base.TempData["User3"] </h3>
<h3> Model= @Model</h3>
```

#### （3）请求Second，运行结果

![image-20210711155957324](assets/image-20210711155957324.png)

#### （4）页面之间传值需要使用Session

```C#
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoNet5Mvc.Controllers
{
    public class FirstController : Controller
    {
        public IActionResult Index()
        {
            //数据如何传输给视图
            ViewBag.User1 = "User1";
            ViewData["User2"] = "User2";
            TempData["User3"] = "User3";
            object User4 = "User4";

            //Session 
            HttpContext.Session.SetString("User5", "User5");
            
            return View(User4);
        }

        public IActionResult Second()
        {            
            return View();
        }
    }
}
```

#### （5）Index,Second视图页面都添加Session展示， 需要引入命名空间

```c#
@using Microsoft.AspNetCore.Http
@model string
<h3> ViewBag.User1=  @base.ViewBag.User1 </h3>
<h3> ViewData["User2"]= @base.ViewData["User2"] </h3>
<h3> TempData["User3"]= @base.TempData["User3"] </h3>
<h3> Model= @Model</h3>
<h3> Session-User5= @Context.Session.GetString("User5")</h3>
```

#### （6）配置Session

不配置会直接报错，无法处理

![image-20210711161408404](assets/image-20210711161408404.png)

在ConfigureServices中添加Session服务

```C#
services.AddSession();
```

在Configure中使用Session服务

```C#
app.UseSession();
```

![image-20210711161754887](assets/image-20210711161754887.png)

#### （7）先请求Index，再请求Second，运行结果

![image-20210719074933774](assets/image-20210719074933774.png)

### 6、页面传值的原理

#### （1）修改页面跳转方式

```C#
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DemoNet5Mvc.Controllers
{
    public class FirstController : Controller
    {
        /// <summary>
        /// 当前页面传值
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            ViewBag.User1 = "User1";
            ViewData["User2"] = "User2";
            TempData["User3"] = "User3";
            object User4 = "User4";

            //Session
            HttpContext.Session.SetString("User5", "User5");

            //return View(User4);
            ////修改页面的跳转方式
            return RedirectToAction("Second");
        }
        
        /// <summary>
        /// 跨页面传值
        /// </summary>
        /// <returns></returns>
        public IActionResult Second()
        {            
            return View();
        }
    }
}
```

#### （2）运行结果

![image-20210719075304486](assets/image-20210719075304486.png)

#### （3）结论  

==**TempData底层是基于Session实现的**==

## 三、项目部署

### 1、IISExpress部署，调试用

#### （1）直接点击IISExpress

![image-20210711210531420](assets/image-20210711210531420.png)

#### （2）运行结果

运行默认的端口是44394，默认HTTPS

![image-20210711192459009](assets/image-20210711192459009.png)

#### （3）说明

这个是在启动配置文件里面配置的，我们也可以使用HTTP访问  http://localhost:62635

![image-20210711192609477](assets/image-20210711192609477.png)

有一个中间件会将HTTP转到HTTPS，注释掉就不会跳转了

```C#
app.UseHttpsRedirection();
```

### 2、独立部署，无需安装SDK

发布的包包含运行时、托管、SDK等，服务器不需要单独安装

#### （1）发布独立包

![image-20210712070401637](assets/image-20210712070401637.png)

![image-20210712070628580](assets/image-20210712070628580.png)

![image-20210712070706575](assets/image-20210712070706575.png)

![image-20210712070819713](assets/image-20210712070819713.png)

![image-20210712071029670](assets/image-20210712071029670.png)

#### （2）点击exe运行

![image-20210712071218956](assets/image-20210712071218956.png)

![image-20210712071410199](assets/image-20210712071410199.png)

#### （3）运行结果

![image-20210719075304486](assets/image-20210719075304486.png)

### 3、IIS部署，Windows专用

#### （1）配置IIS网站

物理路径D:\NetDemos\DemoNet5MvcIIS

![image-20210711211735627](assets/image-20210711211735627.png)

#### （2）安装AspNetCoreModule托管模块

下载网址 https://dotnet.microsoft.com/download/dotnet/5.0

![image-20210712072241555](assets/image-20210712072241555.png)

![image-20210712073521740](assets/image-20210712073521740.png)



![image-20210712073542649](assets/image-20210712073542649.png)



#### （3）发布程序到IIS网站设定的物理路径

![image-20210712074800722](assets/image-20210712074800722.png)

（4）运行结果

![image-20210712211448238](assets/image-20210712211448238.png)



#### （4）依赖部署和独立部署比较

对比独立部署和依赖部署文件夹大小差别巨大

![image-20210712074950070](assets/image-20210712074950070.png)

#### （5）生成和发布文件夹比较

对比直接生成的文件夹和发布的文件夹，差异就是一个web.config文件

![image-20210712075405793](assets/image-20210712075405793.png)

web.config内容就是告诉IIS怎么运行项目

```c#
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" arguments=".\DemoNet5Mvc.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess" />
    </system.webServer>
  </location>
</configuration>
```

### 4、脚本运行（Linux&Windows）

#### （1）Linux环境安装NetSDK

参考微软文档：https://docs.microsoft.com/zh-cn/dotnet/core/install/linux，不同系统的运行命令不同，我使用的阿里云CentOS8运行命令如下

```bash
[root@bluecusliyou ~]# sudo dnf install dotnet-runtime-5.0
Last metadata expiration check: 2:55:38 ago on Wed 21 Jul 2021 06:07:57 PM CST.
Dependencies resolved.
====================================================================================
 Package                  Architecture Version                Repository       Size
====================================================================================
Installing:
 dotnet-runtime-5.0       x86_64       5.0.8-1.el8_4          AppStream        27 M
Installing dependencies:
 dotnet-host              x86_64       5.0.8-1.el8_4          AppStream       111 k
 dotnet-hostfxr-5.0       x86_64       5.0.8-1.el8_4          AppStream       156 k
 libicu                   x86_64       60.3-2.el8_1           BaseOS          8.8 M
 lttng-ust                x86_64       2.8.1-11.el8           AppStream       259 k
 userspace-rcu            x86_64       0.10.1-4.el8           BaseOS          101 k

Transaction Summary
====================================================================================
Install  6 Packages

Total download size: 36 M
Installed size: 124 M
Is this ok [y/N]: y
Downloading Packages:
...
Installed:
  dotnet-host-5.0.8-1.el8_4.x86_64                  dotnet-hostfxr-5.0-5.0.8-1.el8_4.x86_64                  dotnet-runtime-5.0-5.0.8-1.el8_4.x86_64                  libicu-60.3-2.el8_1.x86_64                  lttng-ust-2.8.1-11.el8.x86_64                  userspace-rcu-0.10.1-4.el8.x86_64                 

Complete!
[root@bluecusliyou ~]# 
```

#### （2）项目目录下直接运行

```bash
D:\Codes\NetLearnNote\013、NetX--NET5Advanced\DemoNet5Mvc>dotnet run
正在生成...
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: D:\Codes\NetLearnNote\013、NetX--NET5Advanced\DemoNet5Mvc
```

这边的端口是配置文件==launchSettings.json==里面的端口

```C#
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:62635",
      "sslPort": 44394
    }
  },
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "DemoNet5Mvc": {
      "commandName": "Project",
      "dotnetRunMessages": "true",
      "launchBrowser": true,
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

#### （3）bin目录下运行指定dll

```C#
D:\Codes\NetLearnNote\013、NetX--NET5Advanced\DemoNet5Mvc\bin\Debug\net5.0>dotnet DemoNet5Mvc.dll
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: D:\Codes\NetLearnNote\013、NetX--NET5Advanced\DemoNet5Mvc\bin\Debug\net5.0
```

也可以指定端口号和urls,这里urls的优先级大于端口优先级

```bash
D:\Codes\NetLearnNote\013、NetX--NET5Advanced\DemoNet5Mvc\bin\Debug\net5.0>dotnet DemoNet5Mvc.dll --urls="http://*:8888"
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://[::]:8888
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: D:\Codes\NetLearnNote\013、NetX--NET5Advanced\DemoNet5Mvc\bin\Debug\net5.0
```

运行结果，样式丢失，需要手动指定静态文件路径，并将wwwroot复制到bin目录

```c#
//可以在这里配置css路径
app.UseStaticFiles(new StaticFileOptions()
{
FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))  //执行文件下的wwwroot文件夹
}); 
```

![image-20210713080330527](assets/image-20210713080330527.png)

#### （4）获取传入的参数

添加控制器，添加Configuration依赖注入

```C#
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DemoNet5Mvc.Controllers
{
    public class FirstController : Controller
    {
        public IConfiguration _Configuration { get; }

        /// <summary>
        /// 构造函数注入 
        /// </summary>
        /// <param name="configuration"></param>
        public FirstController(IConfiguration configuration)
        {
            this._Configuration = configuration;
        }

        public IActionResult Third()
        {
            ViewBag.port = _Configuration["port"];
            return View();
        }
    }
}
```

视图显示端口号数据

```C#
<h2>Port:@ViewBag.port</h2>
```

运行命令

```bash
D:\Codes\NetLearnNote\013、NetX--NET5Advanced\DemoNet5Mvc\bin\Debug\net5.0>dotnet DemoNet5Mvc.dll --urls="http://*:8888" --port=8889
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://[::]:8888
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: D:\Codes\NetLearnNote\013、NetX--NET5Advanced\DemoNet5Mvc\bin\Debug\net5.0
```

运行结果

![image-20210714224033374](assets/image-20210714224033374.png)

### 5、安装成Windows服务

#### （1）下载nssm

地址：[http://www.nssm.cc/download](http://www.nssm.cc/download)

#### （2）运行nssm

根据系统位数选择文件夹，运行cmd，执行命令 ==nssm install==

#### （3）在弹出的窗口进行配置　　　

- 　Path：dotnet所在的目录，一般默认是在C:\Program Files\dotnet\dotnet.exe；

- 　Startup directory：程序所在的目录，就是最后程序dll所在的目录；

- 　Arguments：程序dll的名称，一般是项目名加上.dll；

- 　Service name：在此写上服务的名称即可。

（4）最后点击install service 完成windows服务安装

#### （5）CMD下输入 sc delete 服务名称来卸载服务

### 6、Linux+Docker

#### （1）linux安装Docker

Linux安装请参考官网文档：https://docs.docker.com/engine/install/

CentOS8.2版本安装命令

```bash
#1.卸载旧版本 
sudo yum remove docker \
                  docker-client \
                  docker-client-latest \
                  docker-common \
                  docker-latest \
                  docker-latest-logrotate \
                  docker-logrotate \
                  docker-engine

#2.需要的安装包 
sudo yum install -y yum-utils

#3.设置镜像的仓库 
sudo yum-config-manager \
    --add-repo \
    https://download.docker.com/linux/centos/docker-ce.repo 
#默认是从国外的，不推荐，国内访问可能会失败
#推荐使用国内的
sudo yum-config-manager \
    --add-repo \
    http://mirrors.aliyun.com/docker-ce/linux/centos/docker-ce.repo

#更新yum软件包索引 
sudo yum makecache

#4.安装docker相关的 docker-ce 社区版 而ee是企业版 
sudo yum install docker-ce docker-ce-cli containerd.io 

#6. 使用docker version查看是否按照成功 
docker version

#7. 启动Docker服务
sudo service docker restart

#8. 测试运行
docker run hello-world

#9.查看一下下载的镜像
docker images

#10.查看一下容器
docker ps -a
```

#### （2）项目添加dockerfile支持

![image-20210722070030666](assets/image-20210722070030666.png)

也可以手工添加

```bash
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DemoNet5Mvc/DemoNet5Mvc.csproj", "DemoNet5Mvc/"]
RUN dotnet restore "DemoNet5Mvc/DemoNet5Mvc.csproj"
COPY . .
WORKDIR "/src/DemoNet5Mvc"
RUN dotnet build "DemoNet5Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoNet5Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoNet5Mvc.dll"]
```

注意：系统生成的Dockerfile文件的路径是在Mvc项目下面的，需要拷贝到解决方案目录下

![image-20210723071559254](assets/image-20210723071559254.png)

#### （3）ftp上传文件到服务器

#### （4）构建镜像，本地运行

切换到dockerfile所在目录

```bash
[root@bluecusliyou DemoNet5]# docker build -t bluecusliyou/demonet5:0.1 .
```

这个命令是在Dockerfile文件所在目录下执行的，如果构建说明文件名不是Dockerfile，需要使用 -f 参数进行执行。-t 参数指定生成镜像的名称及版本，两者使用冒号（:）隔开，例如 dotnetcore:v5

另外，**命令最后面有一个点号**， 它是构建执行的上下文路径，docker build时会将这个路径所有内容打包，上面的Dockerfile中的ADD和COPY命令相对的目录也就是这个上下文目录。

构建完镜像，查看创建的镜像

```bash
[root@bluecusliyou DemoNet5]# docker images
REPOSITORY                        TAG       IMAGE ID       CREATED          SIZE
bluecusliyou/demonet5             0.1       3bce3c20524c   9 seconds ago    233MB
<none>                            <none>    62c19c136fc2   11 seconds ago   812MB
mcr.microsoft.com/dotnet/sdk      5.0       fa98367f9017   30 hours ago     631MB
mcr.microsoft.com/dotnet/aspnet   5.0       44ffd671d35d   30 hours ago     205MB
```

运行镜像到容器

```bash
[root@bluecusliyou DemoNet5]# docker run -it -p 80:80 bluecusliyou/demonet5:0.1
warn: Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository[60]
      Storing keys in a directory '/root/.aspnet/DataProtection-Keys' that may not be persisted outside of the container. Protected data will be unavailable when container is destroyed.
warn: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[35]
      No XML encryptor configured. Key {c7c87139-690c-4af4-929a-d1ddf21e79d8} may be persisted to storage in unencrypted form.
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://[::]:80
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /app
```

查看运行的容器

```bash
[root@bluecusliyou ~]# docker ps -a
CONTAINER ID   IMAGE                       COMMAND                  CREATED          STATUS          PORTS                                        NAMES
7d455ff839b4   bluecusliyou/demonet5:0.1   "dotnet DemoNet5Mvc.…"   59 seconds ago   Up 58 seconds   0.0.0.0:80->80/tcp, :::80->80/tcp, 443/tcp   competent_mcclintock
```

请求访问项目，内部访问

```bash
[root@bluecusliyou ~]# curl localhost
[root@bluecusliyou ~]#
```

请求访问项目，外部访问

![1627037120076](assets/1627037120076.png)

#### （5）上传构建好的镜像到dockerhub

1、注册dockerhub账号，创建仓库

dockerhub仓库地址：https://hub.docker.com/

镜像仓库就是一个镜像托管网站，类似于代码托管网站github一样的效果，方便你共享开源和管理你的镜像和代码

![image-20210722074635738](assets/image-20210722074635738.png)

2、本地登录账号

```bash
[root@bluecusliyou ~]# docker login -u bluecusliyou
Password: 
WARNING! Your password will be stored unencrypted in /root/.docker/config.json.
Configure a credential helper to remove this warning. See
https://docs.docker.com/engine/reference/commandline/login/#credentials-store

Login Succeeded
```

3、上传镜像

```bash
[root@bluecusliyou ~]# docker push bluecusliyou/demonet5:0.1
The push refers to repository [docker.io/bluecusliyou/demonet5]
22d2fb7b9dbd: Pushed 
e572b1212da9: Layer already exists 
971da11eb099: Pushed 
24321fe445f7: Layer already exists 
ce4856c27fe6: Pushed 
59fa6c56c4c6: Pushed 
814bff734324: Pushed 
0.1: digest: sha256:9a2200bfb4f762ce79eeaa3156fabb9724005efabc78bf321c1001de110ea70e size: 1789
```

#### （5）上传构建好的镜像到阿里云镜像

1、阿里云开通镜像服务、个人的是免费的。

地址：https://cr.console.aliyun.com/cn-hangzhou/instances

![image-20210724142119310](assets/image-20210724142119310.png)

创建命名空间，创建仓库

![image-20210724142614405](assets/image-20210724142614405.png)

创建完成之后，详情里面会有详细的命令帮助

![image-20210724142809479](assets/image-20210724142809479.png)

2、本地登录账号

```bash
[root@bluecusliyou ~]# docker login --username=591071179@qq.com registry.cn-hangzhou.aliyuncs.com
Password: 
WARNING! Your password will be stored unencrypted in /root/.docker/config.json.
Configure a credential helper to remove this warning. See
https://docs.docker.com/engine/reference/commandline/login/#credentials-store

Login Succeeded
```

3、推送镜像到阿里镜像平台

```bash
[root@bluecusliyou ~]# docker images
REPOSITORY                        TAG       IMAGE ID       CREATED        SIZE
bluecusliyou/demonet5             0.1       3bce3c20524c   20 hours ago   233MB
<none>                            <none>    62c19c136fc2   20 hours ago   812MB
mcr.microsoft.com/dotnet/sdk      5.0       fa98367f9017   2 days ago     631MB
mcr.microsoft.com/dotnet/aspnet   5.0       44ffd671d35d   2 days ago     205MB
[root@bluecusliyou ~]# docker tag 3bce3c20524c registry.cn-hangzhou.aliyuncs.com/bluecusliyou/demonet5:0.1
[root@bluecusliyou ~]# docker push registry.cn-hangzhou.aliyuncs.com/bluecusliyou/demonet5:0.1
The push refers to repository [registry.cn-hangzhou.aliyuncs.com/bluecusliyou/demonet5]
22d2fb7b9dbd: Pushed 
e572b1212da9: Pushed 
971da11eb099: Pushed 
24321fe445f7: Pushed 
ce4856c27fe6: Pushed 
59fa6c56c4c6: Pushed 
814bff734324: Pushed 
0.1: digest: sha256:9a2200bfb4f762ce79eeaa3156fabb9724005efabc78bf321c1001de110ea70e size: 1789
```

#### （6）删除本地容器镜像，拉取镜像，运行

拉取dockerhub镜像，运行成功

```bash
[root@bluecusliyou ~]# docker pull bluecusliyou/demonet5:0.1
0.1: Pulling from bluecusliyou/demonet5
33847f680f63: Already exists 
d6365b3570ba: Already exists 
f44097ee8bfd: Already exists 
eb300617f13a: Already exists 
cfb966bdcda1: Already exists 
53a9659145eb: Pull complete 
b434faf45d5b: Pull complete 
Digest: sha256:9a2200bfb4f762ce79eeaa3156fabb9724005efabc78bf321c1001de110ea70e
Status: Downloaded newer image for bluecusliyou/demonet5:0.1
docker.io/bluecusliyou/demonet5:0.1
[root@bluecusliyou ~]# docker run -it -p 80:80 bluecusliyou/demonet5:0.1
warn: Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository[60]
      Storing keys in a directory '/root/.aspnet/DataProtection-Keys' that may not be persisted outside of the container. Protected data will be unavailable when container is destroyed.
warn: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[35]
      No XML encryptor configured. Key {67aeef92-4223-4832-aa00-1bf12d49d7ce} may be persisted to storage in unencrypted form.
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://[::]:80
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /app
```

拉取阿里镜像，运行成功

```bash
[root@bluecusliyou ~]# docker pull registry.cn-hangzhou.aliyuncs.com/bluecusliyou/demonet5:0.1
0.1: Pulling from bluecusliyou/demonet5
33847f680f63: Already exists 
d6365b3570ba: Already exists 
f44097ee8bfd: Already exists 
eb300617f13a: Already exists 
cfb966bdcda1: Already exists 
53a9659145eb: Pull complete 
b434faf45d5b: Pull complete 
Digest: sha256:9a2200bfb4f762ce79eeaa3156fabb9724005efabc78bf321c1001de110ea70e
Status: Downloaded newer image for registry.cn-hangzhou.aliyuncs.com/bluecusliyou/demonet5:0.1
registry.cn-hangzhou.aliyuncs.com/bluecusliyou/demonet5:0.1
[root@bluecusliyou ~]# docker images
REPOSITORY                                                TAG       IMAGE ID       CREATED        SIZE
registry.cn-hangzhou.aliyuncs.com/bluecusliyou/demonet5   0.1       3bce3c20524c   23 hours ago   233MB
<none>                                                    <none>    62c19c136fc2   23 hours ago   812MB
mcr.microsoft.com/dotnet/sdk                              5.0       fa98367f9017   2 days ago     631MB
mcr.microsoft.com/dotnet/aspnet                           5.0       44ffd671d35d   2 days ago     205MB
[root@bluecusliyou ~]# docker run -it -p 80:80 3bce3c20524c
warn: Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository[60]
      Storing keys in a directory '/root/.aspnet/DataProtection-Keys' that may not be persisted outside of the container. Protected data will be unavailable when container is destroyed.
warn: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[35]
      No XML encryptor configured. Key {d01022ba-23e8-4131-b4a3-8fe586b47111} may be persisted to storage in unencrypted form.
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://[::]:80
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /app
```

