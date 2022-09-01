# Linux详解

## 一、Linux简介

### 1、简介

在服务器端，Linux是最流程的操作系统。Linux 有一个强大的服务器软件生态系统：Docker、Redis、MySQL、Nginx、Hadoop 等等。

Linux 内核最初只是由芬兰人林纳斯·托瓦兹（Linus Torvalds）在赫尔辛基大学上学时出于个人爱好而编写的。

Linux 是一套免费使用和自由传播的类 Unix 操作系统，是一个基于 POSIX 和 UNIX 的多用户、多任务、支持多线程和多 CPU 的操作系统。

Linux 能运行主要的 UNIX 工具软件、应用程序和网络协议。它支持 32 位和 64 位硬件。Linux 继承了 Unix 以网络为核心的设计思想，是一个性能稳定的多用户网络操作系统。

### 2、内核版本

- xx.yy.zz
  - xx主版本
  - yy次版本
  - zz末版本

- 小于2.6版本
  - 次版本奇数为开发版
  - 次版本偶数为稳定版

- 大于2.6版本
  - Longterm长期支持版本
  - stable稳定版本
  - mainline主线开发版本

### 3、发行版

Linux 的发行版说简单点就是将 Linux 内核与应用软件做一个打包。

![1511849829609658](http://cdn.bluecusliyou.com/202202171036271.jpg)



目前市面上较知名的发行版有：Ubuntu、RedHat、CentOS、Debian、Fedora、SuSE、OpenSUSE、Arch Linux、SolusOS 等。

### 4、linux和windows区别

- 使用 Windows 倾向于 GUI 操作，Linux 虽然也可以安装图形界面，但是 Linux 倾向于命令行操作（可以自动化、可以精确控制、可以组合命令）。
- windows 下主要根据文件的扩展名区分文件类型，linux 中可以没有扩展名。
- Linux 不同版本使用方法、配置文件可能变化很大，要自己查、反复试。
- 易错的地方：Linux 中文件名是区分大小写的，Windows不区分。

### 5、查看系统版本

```bash
## 查看系统版本
[root@bluecusliyou ~]# lsb_release -a
LSB Version:    :core-4.1-amd64:core-4.1-noarch
Distributor ID: CentOS
Description:    CentOS Linux release 7.9.2009 (Core)
Release:        7.9.2009
Codename:       Core
## 查看
[root@bluecusliyou ~]# cat /proc/version
Linux version 3.10.0-1160.53.1.el7.x86_64 (mockbuild@kbuilder.bsys.centos.org) (gcc version 4.8.5 20150623 (Red Hat 4.8.5-44) (GCC) ) #1 SMP Fri Jan 14 13:59:45 UTC 2022
```



## 二、Linux安装

### 1、安装Vmware虚拟机

#### （1）下载

- 官网下载：[https://download3.vmware.com/software/wkst/file/VMware-workstation-full-16.1.2-17966106.exe](https://download3.vmware.com/software/wkst/file/VMware-workstation-full-16.1.2-17966106.exe)
- 百度网盘链接：[https://pan.baidu.com/s/1PoKMuYpgjQ4o-yTcEcc8Ew](https://pan.baidu.com/s/1PoKMuYpgjQ4o-yTcEcc8Ew)  提取码：1234 

#### （2）安装

#### （3）无法安装Vmware虚拟机原因

- 你的电脑不支持虚拟化或者是没有开启虚拟化
- windows10系统默认有一个虚拟机Hyper-V，把windows自带虚拟机给干掉

#### （4）虚拟机可以进行克隆

![1635852953428](http://cdn.bluecusliyou.com/202202171036990.png)

### 2、虚拟机安装Linux

#### （1）下载

- 百度网盘链接：[https://pan.baidu.com/s/1uG_CYfRJyQVEy0CqvWS_Hg](https://pan.baidu.com/s/1uG_CYfRJyQVEy0CqvWS_Hg) 提取码：1234 

#### （2）安装在虚拟机上

### 3、配置虚拟机网络

#### （1）安装虚拟机网卡

> 编辑->虚拟网络编辑器

![image-20211010205112298](http://cdn.bluecusliyou.com/202202171037111.png)

> 修改宿主机Vmnet8网卡配置

![image-20211010205950898](http://cdn.bluecusliyou.com/202202171037859.png)

#### （2）配置虚拟机网络

![image-20211011222344126](http://cdn.bluecusliyou.com/202202171037922.png)

#### （3）配置虚拟机网卡信息

> 查看网卡是哪个文件

```bash
[liyou@localhost ~]$ ifconfig
ens33: flags=4163<UP,BROADCAST,RUNNING,MULTICAST>  mtu 1500
        inet 192.168.0.112  netmask 255.255.255.0  broadcast 192.168.0.255
        inet6 fe80::da9e:4ce3:6d3b:37b1  prefixlen 64  scopeid 0x20<link>
        ether 00:0c:29:00:38:2c  txqueuelen 1000  (Ethernet)
        RX packets 12892  bytes 19164992 (18.2 MiB)
        RX errors 0  dropped 0  overruns 0  frame 0
        TX packets 3762  bytes 254726 (248.7 KiB)
        TX errors 0  dropped 0 overruns 0  carrier 0  collisions 0

lo: flags=73<UP,LOOPBACK,RUNNING>  mtu 65536
        inet 127.0.0.1  netmask 255.0.0.0
        inet6 ::1  prefixlen 128  scopeid 0x10<host>
        loop  txqueuelen 1000  (Local Loopback)
        RX packets 68  bytes 5912 (5.7 KiB)
        RX errors 0  dropped 0  overruns 0  frame 0
        TX packets 68  bytes 5912 (5.7 KiB)
        TX errors 0  dropped 0 overruns 0  carrier 0  collisions 0

virbr0: flags=4099<UP,BROADCAST,MULTICAST>  mtu 1500
        inet 192.168.122.1  netmask 255.255.255.0  broadcast 192.168.122.255
        ether 52:54:00:93:7b:8c  txqueuelen 1000  (Ethernet)
        RX packets 0  bytes 0 (0.0 B)
        RX errors 0  dropped 0  overruns 0  frame 0
        TX packets 0  bytes 0 (0.0 B)
        TX errors 0  dropped 0 overruns 0  carrier 0  collisions 0
```

> 修改网卡信息，BOOTPROTO修改static，IP固定

先切换root，否则无权限修改网卡文件

```bash
[liyou@localhost ~]$ su root
Password:
```

```bash
[root@localhost liyou]# vi /etc/sysconfig/network-scripts/ifcfg-ens33
```

```bash
——————— 未修改的 ———————
TYPE=“Ethernet”
PROXY_METHOD=“none”
BROWSER_ONLY=“no”
BOOTPROTO=“dhcp”
DEFROUTE=“yes”
IPV4_FAILURE_FATAL=“no”
IPV6INIT=“yes”
IPV6_AUTOCONF=“yes”
IPV6_DEFROUTE=“yes”
IPV6_FAILURE_FATAL=“no”
IPV6_ADDR_GEN_MODE=“stable-privacy”
NAME=“ens32”
UUID=“928523cf-3506-431d-b7ae-b25572be9630”
DEVICE=“ens32”
ONBOOT=“yes”
———————- 修改后的 —————————
TYPE=“Ethernet”
PROXY_METHOD=“none”
BROWSER_ONLY=“no”
BOOTPROTO=“static”
DEFROUTE=“yes”
IPV4_FAILURE_FATAL=“no”
IPV6INIT=“yes”
IPV6_AUTOCONF=“yes”
IPV6_DEFROUTE=“yes”
IPV6_FAILURE_FATAL=“no”
IPV6_ADDR_GEN_MODE=“stable-privacy”
NAME=“ens32”
UUID=“928523cf-3506-431d-b7ae-b25572be9630”
DEVICE=“ens32”
ONBOOT=“yes”
IPADDR=192.168.119.185
NETMASK=255.255.255.0
GATEWAY=192.168.119.2
DNS1=192.168.119.2
```

修改完成保存重启网卡信息

```bash
[root@localhost liyou]# service network restart
```

#### （4）测试网络是否ping通

> 宿主机ping虚拟机

```bash
C:\Users\Administrator>ping -t 192.168.119.187

正在 Ping 192.168.119.187 具有 32 字节的数据:
来自 192.168.119.187 的回复: 字节=32 时间<1ms TTL=64
来自 192.168.119.187 的回复: 字节=32 时间=8ms TTL=64
来自 192.168.119.187 的回复: 字节=32 时间=2ms TTL=64
来自 192.168.119.187 的回复: 字节=32 时间=1ms TTL=64
来自 192.168.119.187 的回复: 字节=32 时间=2ms TTL=64
```

> 虚拟机ping宿主机

- 查看宿主机IP

```bash
无线局域网适配器 WLAN:

   连接特定的 DNS 后缀 . . . . . . . :
   IPv6 地址 . . . . . . . . . . . . : 2409:8a20:2273:5720:f93d:8d7e:1151:65a8
   临时 IPv6 地址. . . . . . . . . . : 2409:8a20:2273:5720:b533:4828:7790:859b
   本地链接 IPv6 地址. . . . . . . . : fe80::f93d:8d7e:1151:65a8%5
   IPv4 地址 . . . . . . . . . . . . : 192.168.1.227
   子网掩码  . . . . . . . . . . . . : 255.255.255.0
   默认网关. . . . . . . . . . . . . : fe80::1%5
                                       192.168.1.1
```

- ping宿主机

```bash
[root@localhost liyou]# ping 192.168.1.227
PING 192.168.1.227 (192.168.1.227) 56(84) bytes of data.
64 bytes from 192.168.1.227: icmp_seq=1 ttl=128 time=0.630 ms
64 bytes from 192.168.1.227: icmp_seq=2 ttl=128 time=2.07 ms
64 bytes from 192.168.1.227: icmp_seq=3 ttl=128 time=1.59 ms
64 bytes from 192.168.1.227: icmp_seq=4 ttl=128 time=2.36 ms
64 bytes from 192.168.1.227: icmp_seq=5 ttl=128 time=2.26 ms
64 bytes from 192.168.1.227: icmp_seq=6 ttl=128 time=2.41 ms
64 bytes from 192.168.1.227: icmp_seq=7 ttl=128 time=2.38 ms
```

#### （5）虚拟机网络通信原理

![image-20211012074709522](http://cdn.bluecusliyou.com/202202171037607.png)https://mirrors.aliyun.com/centos/8.4.2105/isos/x86_64/)

### 4、购买云服务器

**云服务器(Elastic Compute Service, ECS)**是一种简单高效、安全可靠、处理能力可弹性伸缩的计算服务。

云服务器管理方式比物理服务器更简单高效，我们无需提前购买昂贵的硬件，即可迅速创建或删除云服务器，云服务器费用一般在几十到几百不等，可以根据我们的需求配置。

#### （1）购买服务器

下载地址：https://www.aliyun.com/minisite/goods?userCode=0phtycgr

#### （2）配置服务器

> 登录阿里云控制台：[https://ecs.console.aliyun.com/#/home](https://ecs.console.aliyun.com/#/home)

> 获取服务器公网IP

![1626242612035](http://cdn.bluecusliyou.com/202202171037916.png)

#### （3）修改服务器登录密码

点击蓝色实例名，进入实例详情，可以重置密码，也可以修改主机名

![1626242708181](http://cdn.bluecusliyou.com/202202171037536.png)

#### （4）配置安全组开放端口号

![1626242868155](http://cdn.bluecusliyou.com/202202171038027.png)

授权对象 0000表示授权给所有人

![1626242999444](http://cdn.bluecusliyou.com/202202171038707.png)

### 5、宝塔面板安装

宝塔Linux面板是提升运维效率的服务器管理软件，支持一键LAMP/LNMP/集群/监控/网站/FTP/数据库/JAVA等100多项服务器管理功能。
有30个人的专业团队研发及维护，经过200多个版本的迭代，功能全，少出错且足够安全，已获得全球百万用户认可安装。运维要高效，装宝塔。

教程地址：[https://www.bt.cn/bbs/thread-19376-1-1.html](https://www.bt.cn/bbs/thread-19376-1-1.html)

### 6、常用连接工具

#### （1）连接工具

- FinalShell：[http://www.hostbuf.com/](http://www.hostbuf.com/)
- MobaXterm：[https://mobaxterm.mobatek.net/](https://mobaxterm.mobatek.net/)

#### （2）SSH服务

​			运维人员是怎么远程连接到机房上的服务器，很少使用图形界面(卡，无法自动化)，远程登录有专门的通信协议telnet，telnet就是通过网络进行命令行操作服务器。只有服务器端开始了远程登录服务，客户端才能通过telnet协议控制服务器端。
​			但telnet协议使用明文传输数据，这会造成严重的安全性问题，所以现在几乎不推荐使用，而替代协议是SSH，ssh命令可以让我们轻松的基于ssh加密协议进行远程主机访问，从而实现对远程服务器的管理工作。

#### （3）ssh命令

**语法格式:** ssh [参数] 远程主机

**常用参数：**

| -1           | 强制使用ssh协议版本1                                         |
| ------------ | ------------------------------------------------------------ |
| -2           | 强制使用ssh协议版本2                                         |
| -4           | 强制使用IPv4地址                                             |
| -6           | 强制使用IPv6地址                                             |
| -A           | 开启认证代理连接转发功能                                     |
| -a           | 关闭认证代理连接转发功能                                     |
| -b<IP地址>   | 使用本机指定的地址作为对位连接的源IP地址                     |
| -C           | 请求压缩所有数据                                             |
| -F<配置文件> | 指定ssh指令的配置文件，默认的配置文件为“/etc/ssh/ssh_config” |
| -f           | 后台执行ssh指令                                              |
| -g           | 允许远程主机连接本机的转发端口                               |
| -i<身份文件> | 指定身份文件（即私钥文件）                                   |
| -l<登录名>   | 指定连接远程服务器的登录用户名                               |
| -N           | 不执行远程指令                                               |
| -o<选项>     | 指定配置选项                                                 |
| -p<端口>     | 指定远程服务器上的端口                                       |
| -q           | 静默模式，所有的警告和诊断信息被禁止输出                     |
| -X           | 开启X11转发功能                                              |
| -x           | 关闭X11转发功能                                              |
| -y           | 开启信任X11转发功能                                          |

**参考实例**

```bash
# 基于ssh协议，远程访问服务器主机系统
[root@bluecusliyou ~]# ssh 链接的服务器IP地址
root@121.4.29.23's password: 服务器密码
Activate the web console with: systemctl enable --now cockpit.socket
Last login: Sun Jul 31 13:27:41 2022 from 服务器IP
[root@bluecusliyou ~]#
# 使用指定的用户身份登录远程服务器主机系统
[root@bluecusliyou ~]# ssh -l liyou 链接的服务器IP地址
root@121.4.29.23's password: 服务器密码
Activate the web console with: systemctl enable --now cockpit.socket
Last login: Sun Jul 31 13:27:41 2022 from 服务器IP
[root@bluecusliyou ~]$
# 登录远程服务器主机系统后执行一条命令
[root@bluecusliyou ~]# ssh 链接的服务器IP地址 "docker ps -a"
root@121.4.29.23's password: 
CONTAINER ID   IMAGE     COMMAND                  CREATED        STATUS                    PORTS                               NAMES
9319e4a0df36   nginx     "/docker-entrypoint.…"   5 months ago   Exited (1) 5 months ago                                       mynginxhttps
1c026f2f9a01   nginx     "/docker-entrypoint.…"   5 months ago   Up 5 months               80/tcp                              nginxforconf
e6bd5bdcba5d   nginx     "/docker-entrypoint.…"   5 months ago   Up 5 months               0.0.0.0:80->80/tcp, :::80->80/tcp   mynginx
[root@bluecusliyou ~]# 
# 强制使用v1版本的ssh加密协议连接远程服务器主机
[root@bluecusliyou ~]# ssh -1 链接的服务器IP地址
```

#### （4）图形连接			

以MobaXterm为例，输入host地址，输入账号密码就可以进行远程控制服务器了。实际上也是通过命令行连接的。

![image-20220902071103262](http://cdn.bluecusliyou.com/202209020711500.png)

![image-20220902071817574](http://cdn.bluecusliyou.com/202209020718687.png)

## 三、Linux基础

### 1、基础知识

#### （1）man(查看命令帮助信息)

man命令来自于英文单词manual的缩写，中文译为帮助手册，其功能是用于查看命令、配置文件及服务的帮助信息。网上搜索来的资料普遍不够准确，或者缺乏系统性，质量不高造成学习进度缓慢，而man命令作为权威的官方工具则很好的解决了上述两点弊病，一份完整的帮助信息包含以下信息。

| 结构名称    | 代表意义                 |
| ----------- | ------------------------ |
| NAME        | 命令的名称               |
| SYNOPSIS    | 参数的大致使用方法       |
| DESCRIPTION | 介绍说明                 |
| EXAMPLES    | 演示（附带简单说明）     |
| OVERVIEW    | 概述                     |
| DEFAULTS    | 默认的功能               |
| OPTIONS     | 具体的可用选项（带介绍） |
| ENVIRONMENT | 环境变量                 |
| FILES       | 用到的文件               |
| SEE ALSO    | 相关的资料               |
| HISTORY     | 维护历史与联系方式       |

**语法格式：**man [参数] 对象

**常用参数：**

| -a   | 在所有的man帮助手册中搜索                                    |
| ---- | ------------------------------------------------------------ |
| -d   | 主要用于检查，如果用户加入了一个新的文件，就可以用这个参数检查是否出错 |
| -f   | 显示给定关键字的简短描述信息                                 |
| -p   | 指定内容时使用分页程序                                       |
| -M   | 指定man手册搜索的路径                                        |
| -w   | 显示文件所在位置                                             |

**快捷键：**

| q       | 退出                 |
| ------- | -------------------- |
| Enter   | 按行下翻             |
| Space   | 按页下翻             |
| b       | 上翻一页             |
| /字符串 | 在手册页中查找字符串 |

**参考实例**

```bash
# 查看ls的帮助
man ls  
```

#### （2）help(获取内部帮助命令)

```bash
# 查看ls的帮助
ls --help  
```

#### （3）info比man更丰富的帮助信息

#### （4）history显示历史输入过的命令

#### （5）clear清屏

#### （6）快捷键

- ==ctrl + c==（停止当前进程）
- ==ctrl + l==（清屏，与clear命令作用相同）
- ==ctrl +滚动鼠标==调整窗口字体大小
- 按 ==上 ／ 下== 光标键可以在曾经使用过的命令之间来回切换
- 敲出 ==文件 ／ 目录 ／ 命令== 的前几个字母，按下 tab 键，无歧义，系统会自动补全
- 敲出 ==文件 ／ 目录 ／ 命令== 的前几个字母，按下 tab 键，有歧义，再按 tab 键，系统会提示可能存在的命令

#### （7）命令提示符

- "$"普通用户
- "#"root用户

#### （8）命令参考网站

- [https://www.linuxcool.com/](https://www.linuxcool.com/)
- [https://www.runoob.com/linux/linux-tutorial.html](https://www.runoob.com/linux/linux-tutorial.html)
- [https://gitee.com/jishupang/linux-memo](https://gitee.com/jishupang/linux-memo)

### 2、开关机

#### （1）开机

一般来说，用户的登录系统方式有三种：

- 命令行登录
- ssh登录
- 图形界面登录

![bg2013081706](http://cdn.bluecusliyou.com/202202171038279.png)

#### （2）关机

在linux领域内大多用在服务器上，很少遇到关机的操作。毕竟服务器上跑一个服务是永无止境的，除非特殊情况下，不得已才会关机。

可以运行如下命令关机：

```bash
sync #将数据由内存同步到硬盘中。

shutdown #关机指令，你可以man shutdown 来看一下帮助文档。

shutdown –h 10 #10分钟后关机

shutdown –h now #立马关机

shutdown –h 20:25 #系统会在今天20:25关机

shutdown –h +10 #10分钟后关机

shutdown –r now #系统立马重启

shutdown –r +10 #系统十分钟后重启

reboot #重启，等同于 shutdown –r now

halt #关闭系统，等同于shutdown –h now 和 poweroff
```

### 3、系统目录结构

#### （1）系统目录结构

- Linux里面一切皆文件，所有的东西都是文件的形式呈现
- /是系统的根目录，所有的东西都在这个目录下面

```bash
[root@bluecusliyou ~]# ls /
bin  boot  dev  etc  home  lib  lib64  media  mnt  opt  proc  root  run  sbin  srv  sys  tmp  usr  var
```

#### （2）目录结构说明

![d0c50-linux2bfile2bsystem2bhierarchy](http://cdn.bluecusliyou.com/202202171040724.jpg)

- ==**/bin**：bin 是 Binaries (二进制文件) 的缩写, 这个目录存放着最经常使用的命令。==
- **/boot：**这里存放的是启动 Linux 时使用的一些核心文件，包括一些连接文件以及镜像文件。
- **/dev ：**dev 是 Device(设备) 的缩写, 该目录下存放的是 Linux 的外部设备，在 Linux 中访问设备的方式和访问文件的方式是相同的。
- ==**/etc：**etc 是 Etcetera(等等) 的缩写,这个目录用来存放所有的系统管理所需要的配置文件和子目录。==
- ==**/home**：用户的主目录，在 Linux 中，每个用户都有一个自己的目录，一般该目录名是以用户的账号命名的，如上图中的 alice、bob 和 eve。==
- **/lib**：lib 是 Library(库) 的缩写这个目录里存放着系统最基本的动态连接共享库，其作用类似于 Windows 里的 DLL 文件。几乎所有的应用程序都需要用到这些共享库。
- **/lost+found**：这个目录一般情况下是空的，当系统非法关机后，这里就存放了一些文件。
- **/media**：linux 系统会自动识别一些设备，例如U盘、光驱等等，当识别后，Linux 会把识别的设备挂载到这个目录下。
- **/mnt**：系统提供该目录是为了让用户临时挂载别的文件系统的，我们可以将光驱挂载在 /mnt/ 上，然后进入该目录就可以查看光驱里的内容了。
- **/opt**：opt 是 optional(可选) 的缩写，这是给主机额外安装软件所摆放的目录。比如你安装一个ORACLE数据库则就可以放到这个目录下。默认是空的。
- **/proc**：proc 是 Processes(进程) 的缩写，/proc 是一种伪文件系统（也即虚拟文件系统），存储的是当前内核运行状态的一系列特殊文件，这个目录是一个虚拟的目录，它是系统内存的映射，我们可以通过直接访问这个目录来获取系统信息。
- ==**/root**：该目录为系统管理员，也称作超级权限者的用户主目录。==
- ==**/sbin**：s 就是 Super User 的意思，是 Superuser Binaries (超级用户的二进制文件) 的缩写，这里存放的是系统管理员使用的系统管理程序。==
- **/selinux**： 这个目录是 Redhat/CentOS 所特有的目录，Selinux 是一个安全机制，类似于 windows 的防火墙，但是这套机制比较复杂，这个目录就是存放selinux相关的文件的。
- **/srv**：该目录存放一些服务启动之后需要提取的数据。
- **/sys**：这是 Linux2.6 内核的一个很大的变化。该目录下安装了 2.6 内核中新出现的一个文件系统 sysfs 。
- **/tmp**：tmp 是 temporary(临时) 的缩写这个目录是用来存放一些临时文件的。
- ==**/usr**：usr 是 unix shared resources(共享资源) 的缩写，这是一个非常重要的目录，用户的很多应用程序和文件都放在这个目录下，类似于 windows 下的 program files 目录。==
- ==**/usr/bin：**系统用户使用的应用程序。==
- ==**/usr/sbin：**超级用户使用的比较高级的管理程序和系统守护程序。==
- **/usr/src：**内核源代码默认的放置目录。
- ==**/var**：var 是 variable(变量) 的缩写，这个目录中存放着在不断扩充着的东西，我们习惯将那些经常被修改的目录放在这个目录下。包括各种日志文件。==
- **/run**：是一个临时文件系统，存储系统启动以来的信息。当系统重启时，这个目录下的文件应该被删掉或清除。如果你的系统上有 /var/run 目录，应该让它指向 run。

## 四、文件与目录

### 1、前置概念

#### （1）通配符

- *：匹配任意字符
- ?：匹配单个字符
- [xyz]：匹配xyz任意一个字符
- [a-z]：匹配字符范围
- [!xyz]：匹配不在xyz中的任意字符

#### （2）重定向>和>>

- “>”：表示覆盖
- “>>”：表示追加

```bash
[root@bluecusliyou testdir]# ll
total 39820
drwxr-xr-x 2 root root       50 Nov 18 17:18 testdir1
drwxr-xr-x 2 root root        6 Nov 18 17:11 testdir2
-rw-r--r-- 1 root root 40764544 Nov 19 10:11 test.exe
-rw-r--r-- 1 root root        0 Nov 18 17:11 testfile1
-rw-r--r-- 1 root root        0 Nov 18 17:11 testfile2
-rw-r--r-- 1 root root     4855 Nov 19 10:09 test.png
[root@bluecusliyou testdir]# echo '123'>1.txt
[root@bluecusliyou testdir]# echo '456'>2.txt
[root@bluecusliyou testdir]# cat 1.txt
123
[root@bluecusliyou testdir]# cat 2.txt
456
[root@bluecusliyou testdir]# cat 1.txt 2.txt>3.txt
[root@bluecusliyou testdir]# cat 3.txt
123
456
[root@bluecusliyou testdir]# cat 1.txt>>3.txt
[root@bluecusliyou testdir]# cat 3.txt
123
456
123
[root@bluecusliyou testdir]# cat 1.txt>3.txt
[root@bluecusliyou testdir]# cat 3.txt
123
```

#### （3）管道符|

管道：**前面命令的输出**  可以通过管道做为  **后面命令的输入**。

管道我们可以理解现实生活中的管子，管子的一头塞东西进去，另一头取出来，这里“ | ”的左右分为两端，左端塞东西(写)，右端取东西(读)。

eg： 【ls -lh bin | more】表示：把bin下的内容分页输出

　　 【ps -ef|grep nginx】：查询nginx的进程

　　 【ifconfig|more】：将网络信息分页输出。 

### 2、文件目录管理

#### （1）ls (显示指定工作目录下的文件及属性信息)

ls是最常被使用到的Linux命令之一，来自于英文单词list的缩写，也正如list单词的英文意思，其功能是列举出指定目录下的文件名称及其属性。

默认不加参数的情况下，ls命令会列出当前工作目录中的文件信息，经常与cd和pwd命令搭配使用，十分方便。而带上参数后，我们则可以做更多的事情，作为最基础、最频繁使用的命令，有必要仔细了解下其常用功能。

**语法格式:** ls [参数] [文件]

**常用参数：**

| -a      | 显示所有文件及目录 (包括以“.”开头的隐藏文件)     |
| ------- | ------------------------------------------------ |
| -l      | 使用长格式列出文件及目录的详细信息               |
| -r      | 将文件以相反次序显示(默认依英文字母次序)         |
| -t      | 根据最后的修改时间排序                           |
| -A      | 同 -a ，但不列出 “.” (当前目录) 及 “..” (父目录) |
| -S      | 根据文件大小排序                                 |
| -R      | 递归列出所有子目录                               |
| -d      | 查看目录的信息，而不是里面子文件的信息           |
| -i      | 输出文件的inode节点信息                          |
| -m      | 水平列出文件，以逗号间隔                         |
| -X      | 按文件扩展名排序                                 |
| --color | 输出信息中带有着色效果                           |

**参考实例**

```bash
# 列出当前目录下的文件
[root@bluecusliyou testdir]# ls
testdir1  testdir2  test.exe  testfile1  testfile2  test.png
# 列出文件，包含隐藏文件
[root@bluecusliyou testdir]# ls -a
.  ..  testdir1  testdir2  test.exe  testfile1  testfile2  test.png
# 使用长格式列出文件及目录信息
[root@bluecusliyou testdir]# ls -l
total 39820
drwxr-xr-x 2 root root       50 Nov 18 17:18 testdir1
drwxr-xr-x 2 root root        6 Nov 18 17:11 testdir2
-rw-r--r-- 1 root root 40764544 Nov 19 10:11 test.exe
-rw-r--r-- 1 root root        0 Nov 18 17:11 testfile1
-rw-r--r-- 1 root root        0 Nov 18 17:11 testfile2
-rw-r--r-- 1 root root     4855 Nov 19 10:09 test.png
# 配合-l以人性化的方式显示文件大小
[root@bluecusliyou testdir]# ls -lh
total 39M
drwxr-xr-x 2 root root   50 Nov 18 17:18 testdir1
drwxr-xr-x 2 root root    6 Nov 18 17:11 testdir2
-rw-r--r-- 1 root root  39M Nov 19 10:11 test.exe
-rw-r--r-- 1 root root    0 Nov 18 17:11 testfile1
-rw-r--r-- 1 root root    0 Nov 18 17:11 testfile2
-rw-r--r-- 1 root root 4.8K Nov 19 10:09 test.png
# 以人性化的方式显示文件大小和隐藏文件
[root@bluecusliyou testdir]# ls -alh
total 39M
drwxr-xr-x  4 root root  104 Nov 19 10:11 .
drwxr-xr-x. 4 root root   37 Nov 18 17:09 ..
drwxr-xr-x  2 root root   50 Nov 18 17:18 testdir1
drwxr-xr-x  2 root root    6 Nov 18 17:11 testdir2
-rw-r--r--  1 root root  39M Nov 19 10:11 test.exe
-rw-r--r--  1 root root    0 Nov 18 17:11 testfile1
-rw-r--r--  1 root root    0 Nov 18 17:11 testfile2
-rw-r--r--  1 root root 4.8K Nov 19 10:09 test.png
# ll===ls -l
[root@bluecusliyou testdir]# ll
total 39820
drwxr-xr-x 2 root root       50 Nov 18 17:18 testdir1
drwxr-xr-x 2 root root        6 Nov 18 17:11 testdir2
-rw-r--r-- 1 root root 40764544 Nov 19 10:11 test.exe
-rw-r--r-- 1 root root        0 Nov 18 17:11 testfile1
-rw-r--r-- 1 root root        0 Nov 18 17:11 testfile2
-rw-r--r-- 1 root root     4855 Nov 19 10:09 test.png
# 通配符匹配
[root@bluecusliyou testdir]# ls -alh *dir*
testdir1:
total 0
drwxr-xr-x 2 root root  50 Nov 18 17:18 .
drwxr-xr-x 4 root root 104 Nov 19 10:11 ..
-rw-r--r-- 1 root root   0 Nov 18 17:18 testdir1_file1
-rw-r--r-- 1 root root   0 Nov 18 17:18 testdir1_file2

testdir2:
total 0
drwxr-xr-x 2 root root   6 Nov 18 17:11 .
drwxr-xr-x 4 root root 104 Nov 19 10:11 ..
```

#### （2）tree(以树状图形式列出目录内容)

tree命令的功能是用于以树状图形式列出目录内容，帮助运维人员快速了解到目录的层级关系。

不存在该命令直接安装 `yum install tree`

**语法格式：**tree [参数]

**常用参数：**

| -a           | 显示所有文件和目录                      |
| ------------ | --------------------------------------- |
| -A           | ASNI绘图字符形式                        |
| -C           | 彩色显示                                |
| -d           | 仅显示目录名称                          |
| -D           | 显示文件更改时间                        |
| -f           | 显示完整的相对路径名称                  |
| -g           | 显示文件所属群组名称                    |
| -i           | 不以阶梯状列出文件或目录名称            |
| -I<范本样式> | 不显示符合范本样式的文件或目录名称      |
| -l           | 直接显示连接文件所指向的原始目录        |
| -n           | 不在文件和目录清单上加上色彩            |
| -N           | 直接列出文件和目录名称                  |
| -p           | 列出权限标示                            |
| -P<范本样式> | 只显示符合范本像是的文件或目录名称      |
| -q           | 用“?”号取代控制字符，列出文件和目录名称 |
| -s           | 列出文件或目录大小                      |
| -t           | 用文件和目录的更改时间排序              |
| -u           | 列出文件或目录的拥有者名称              |
| -x           | 将范围局限在现行的文件系统中            |
| -L           | 层级显示                                |

**参考实例**

```bash
# 显示当前工作目录下的文件层级情况
[root@bluecusliyou testdir]# tree
.
├── 1.txt
├── 2.txt
├── 3.txt
├── 4.txt
├── grepfile1.txt
├── grepfile2.txt
├── grepfile3.txt
├── grepfile4.txt
├── testdir1
│?? ├── testdir1_file1
│?? └── testdir1_file2
├── testdir2
├── test.exe
├── testfile1
├── testfile2
└── test.png

2 directories, 14 files
```

#### （3）cd (切换目录)

cd命令来自于英文词组”change directory“的缩写，其功能是用于更改当前所处的工作目录，路径可以是绝对路径，也可以是相对路径，若省略不写则会跳转至当前使用者的家目录。

- 绝对路径：完整路径
- 相对路径：相对当前路径的路径
- ..：上级目录
- .或者./：当前目录

**语法格式：**cd [参数] [目录名]

**常用参数：**

| -P   | 如果切换的目标目录是一个符号链接，则直接切换到符号链接指向的目标目录 |
| ---- | ------------------------------------------------------------ |
| -L   | 如果切换的目标目录是一个符号链接，则直接切换到符号链接名所在的目录 |
| --   | 仅使用”-“选项时，当前目录将被切换到环境变量”OLDPWD”对应值的目录 |
| ~    | 切换至当前用户目录                                           |
| ..   | 切换至当前目录位置的上一级目录                               |

**参考实例**

```bash
[root@bluecusliyou /]# cd home/testdir     #相对路径
[root@bluecusliyou testdir]# cd /          #绝对路径根目录
[root@bluecusliyou /]# cd /home/testdir    #绝对路径
[root@bluecusliyou testdir]# cd ..         #上级路径
[root@bluecusliyou /]# cd ./               #当前路径
[root@bluecusliyou /]#
```

#### （4）pwd (显示当前工作目录的路径)

pwd命令来自于英文词组”print working directory“的缩写，其功能是用于显示当前工作目录的路径，即显示所在位置的绝对路径。

在实际工作中，我们经常会在不同目录之间进行切换，为了防止”迷路“，可以使用pwd命令快速查看当前所处的工作目录路径，方便开展后续工作。

**语法格式**：pwd [参数]

**常用参数：**

| -L   | 显示逻辑路径 |
| ---- | ------------ |

**参考实例**

```bash
[root@bluecusliyou bluecusliyou]# pwd
/home/bluecusliyou
```

#### （5）mkdir (创建目录文件)

mkdir命令来自于英文词组“make directories”的缩写，其功能是用来创建目录文件。使用简单，但需要注意若要创建的目标目录已经存在，则会提示已存在而不继续创建，不覆盖已有文件。而目录不存在，但具有嵌套的依赖关系，例如a/b/c/d/e/f，要想一次性创建则需要加入-p参数，进行递归操作。

**语法格式 :** mkdir [参数] 目录

**常用参数：**

| -p   | 递归创建多级目录             |
| ---- | ---------------------------- |
| -m   | 建立目录的同时设置目录的权限 |
| -z   | 设置安全上下文               |
| -v   | 显示目录的创建过程           |

**参考实例**

```bash
# 创建目录
[root@bluecusliyou home]# ls
[root@bluecusliyou home]# mkdir testdir
[root@bluecusliyou home]# ls
testdir
# 创建目录同时配置权限
[root@bluecusliyou home]# mkdir -m 711 testdir2
[root@bluecusliyou home]# ll
total 0
drwxr-xr-x 4 root root 240 Nov 19 13:06 testdir
drwxr-xr-x 2 root root   6 Nov 20 19:33 testdir2
# 递归创建目录
[root@bluecusliyou home]# mkdir testdir3/testdir/testdir
mkdir: cannot create directory ‘testdir3/testdir/testdir’: No such file or directory
[root@bluecusliyou home]# mkdir -p testdir3/testdir/testdir
[root@bluecusliyou home]# tree
.
├── testdir
│   ├── 1.txt
│   ├── 2.txt
│   ├── 3.txt
│   ├── 4.txt
│   ├── grepfile1.txt
│   ├── grepfile2.txt
│   ├── grepfile3.txt
│   ├── grepfile4.txt
│   ├── testdir1
│   │   ├── testdir1_file1
│   │   └── testdir1_file2
│   ├── testdir2
│   ├── test.exe
│   ├── testfile1
│   ├── testfile2
│   └── test.png
├── testdir2
└── testdir3
    └── testdir
        └── testdir

7 directories, 14 files
```

#### （6）rmdir (删除空目录文件)

rmdir命令来自于英文词组“remove directory”的缩写，其功能是用于删除空目录文件。rmdir命令仅能够删除空内容的目录文件，如需删除非空目录时，则需要使用带有-R参数的rm命令进行操作。而rmdir命令的-p递归删除操作亦不意味着能删除目录中已有的文件，而是要求每个子目录都必须是空的。

**语法格式 :** rmdir [参数] 目录

**常用参数：**

| -p            | 用递归的方式删除指定的目录路径中的所有父级目录，非空则报错 |
| ------------- | ---------------------------------------------------------- |
| -v            | 显示命令的详细执行过程                                     |
| -- -- help    | 显示命令的帮助信息                                         |
| -- -- version | 显示命令的版本信息                                         |

**参考实例**

```bash
# 删除目录
[root@bluecusliyou home]# ls
testdir testdir2 testdir3
[root@bluecusliyou home]# rmdir testdir2
[root@bluecusliyou home]# ls
testdir  testdir3
# 递归删除目录
[root@bluecusliyou home]# rmdir -p testdir3/testdir/testdir
[root@bluecusliyou home]# ls
testdir2
```

#### （7）cp (复制文件或目录)

cp命令来自于英文单词copy的缩写，用于将一个或多个文件或目录复制到指定位置，亦常用于文件的备份工作。-r参数用于递归操作，复制目录时若忘记加则会直接报错，而-f参数则用于当目标文件已存在时会直接覆盖不再询问，这两个参数尤为常用。

**语法格式：**cp [参数] 源文件 目标文件

**常用参数：**

| -f   | 若目标文件已存在，则会直接覆盖原文件                         |
| ---- | ------------------------------------------------------------ |
| -i   | 若目标文件已存在，则会询问是否覆盖                           |
| -p   | 保留源文件或目录的所有属性                                   |
| -r   | 递归复制文件和目录                                           |
| -d   | 当复制符号连接时，把目标文件或目录也建立为符号连接，并指向与源文件或目录连接的原始文件或目录 |
| -l   | 对源文件建立硬连接，而非复制文件                             |
| -s   | 对源文件建立符号连接，而非复制文件                           |
| -b   | 覆盖已存在的文件目标前将目标文件备份                         |
| -v   | 详细显示cp命令执行的操作过程                                 |
| -a   | 等价于“pdr”选项                                              |

**参考实例**

```bash
[root@bluecusliyou home]# ls
testdir  testfile1
# cp 源文件  目标地址
[root@bluecusliyou home]# cp testfile1 testfile2
[root@bluecusliyou home]# ls
testdir  testfile1  testfile2
# 目标文件已存在会询问是否覆盖
[root@bluecusliyou home]# cp testfile1 testfile2
cp: overwrite 'testfile2'? y
[root@bluecusliyou home]# ls
testdir  testfile1  testfile2
```

#### （8）rm (删除文件或目录)

rm命令来自于英文单词remove的缩写，其功能是用于删除文件或目录，一次可以删除多个文件，或递归删除目录及其内的所有子文件。

rm也是一个很危险的命令，使用的时候要特别当心，尤其对于新手更要格外注意，如执行rm -rf /*命令则会清空系统中所有的文件，甚至无法恢复回来。所以我们在执行之前一定要再次确认下在哪个目录中，到底要删除什么文件，考虑好后再敲击回车，时刻保持清醒的头脑。

**语法格式：**rm [参数] 文件

**常用参数：**

| -f   | 强制删除（不二次询问）   |
| ---- | ------------------------ |
| -i   | 删除前会询问用户是否操作 |
| -r/R | 递归删除                 |
| -v   | 显示指令的详细执行过程   |

**参考实例**

```bash
rm -rf /  #删除根目录下所有文件，传说中的删库跑路  
```

```bash
[root@bluecusliyou home]# ls
testdir  testfile1  testfile2
[root@bluecusliyou home]# rm -rf testfile2
[root@bluecusliyou home]# ls
testdir  testfile1
```

#### （9）mv (移动或改名文件)

mv命令来自于英文单词move的缩写，其功能与英文含义相同，用于对文件进行剪切和重命名。

这是一个高频使用的文件管理命令，我们需要留意它与复制命令的区别。cp命令是用于文件的复制操作，文件个数是增加的，而mv则为剪切操作，也就是对文件进行移动（搬家）操作，文件位置发生变化，但总个数并无增加。

在同一个目录内对文件进行剪切的操作，实际应理解成重命名操作，例如下面的实例一所示。

**语法格式：**mv [参数] 源文件 目标文件

**常用参数：**

| -i   | 若存在同名文件，则向用户询问是否覆盖                         |
| ---- | ------------------------------------------------------------ |
| -f   | 覆盖已有文件时，不进行任何提示                               |
| -b   | 当文件存在时，覆盖前为其创建一个备份                         |
| -u   | 当源文件比目标文件新，或者目标文件不存在时，才执行移动此操作 |

**参考实例**

```bash
[root@bluecusliyou home]# ls
testdir  testfile1
# 移动文件
[root@bluecusliyou home]# mv testfile1 testfile2
[root@bluecusliyou home]# ls
testdir  testfile2
# 移动文件夹
[root@bluecusliyou home]# mv testdir testdir2
[root@bluecusliyou home]# ls
testdir2  testfile2
```

#### （10）touch(创建空文件与修改时间戳)

touch命令的功能是用于创建空文件与修改时间戳。如果文件不存在，则会创建出一个空内容的文本文件；如果文件已经存在，则会对文件的Atime（访问时间）和Ctime（修改时间）进行修改操作，管理员可以完成此项工作，而普通用户只能管理主机的文件。

**语法格式：**touch [参数] 文件

**常用参数：**﻿

| -a          | 改变档案的读取时间记录                     |
| ----------- | ------------------------------------------ |
| -m          | 改变档案的修改时间记录                     |
| -r          | 使用参考档的时间记录，与 --file 的效果一样 |
| -c          | 不创建新文件                               |
| -d          | 设定时间与日期，可以使用各种不同的格式     |
| -t          | 设定档案的时间记录，格式与 date 命令相同   |
| --no-create | 不创建新文件                               |
| --help      | 显示帮助信息                               |
| --version   | 列出版本讯息                               |

﻿**参考实例**

```bash
[root@bluecusliyou home]# ls
testdir2  testfile2
[root@bluecusliyou home]# touch testfile1
[root@bluecusliyou home]# ls
testdir2  testfile1  testfile2
```

#### （11）echo(输出字符串或提取变量值)

echo是用于在终端设备上输出指定字符串或变量提取后值的命令，能够给用户一些简单的提醒信息，也可以将输出的指定字符串内容同管道符一起传递给后续命令作为标准输入信息再来进行二次处理，又或者同输出重定向符一起操作，将信息直接写入到文件中。

如需提取变量值，需在变量名称前加入$符号做提取，变量名称一般均为大写形式。

**语法格式：**echo [参数] 字符串/变量

**常用参数：**

| -n       | 不输出结尾的换行符               |
| -------- | -------------------------------- |
| -e “\a”  | 发出警告音                       |
| -e “\b”  | 删除前面的一个字符               |
| -e “\c”  | 结尾不加换行符                   |
| -e “\f”  | 换行，光标扔停留在原来的坐标位置 |
| -e “\n”  | 换行，光标移至行首               |
| -e “\r”  | 光标移至行首，但不换行           |
| -E       | 禁止反斜杠转移，与-e参数功能相反 |
| —version | 查看版本信息                     |
| --help   | 查看帮助信息                     |

**参考实例**

```bash
[root@bluecusliyou home]# echo $PATH
/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/root/.dotnet/tools:/root/bin
[root@bluecusliyou home]# echo '111'
111
```

#### （12）ln(为文件创建快捷方式)

ln命令来自于英文单词link的缩写，中文译为“链接”，其功能是用于为某个文件在另外一个位置建立同步的链接。Linux系统中的链接文件有两种形式，一种是硬链接（hard link），另一种是软链接（symbolic link）。软连接相当于Windows系统中的快捷方式文件，原始文件被移动或删除后，软连接文件也将无法使用，而硬链接则是通过将文件的inode属性块进行了复制 ，因此把原始文件移动或删除后，硬链接文件依然可以使用。

软链接 ：

- 以路径的形式存在，类似于Windows操作系统中的快捷方式。
- 可以跨文件系统 ，硬链接不可以。
- 可以对一个不存在的文件名进行链接，硬链接不可以。
- 可以对目录进行链接，硬链接不可以。

硬链接：

- 以文件副本的形式存在，但不占用实际空间。
- 不允许给目录创建硬链接。
- 只有在同一个文件系统中才能创建。

**语法格式：** ln [参数] 源文件 目标文件

**常用参数：**

| -b   | 为每个已存在的目标文件创建备份文件                   |
| ---- | ---------------------------------------------------- |
| -d   | 此选项允许“root”用户建立目录的硬链接                 |
| -f   | 强制创建链接，即使目标文件已经存在                   |
| -n   | 把指向目录的符号链接视为一个普通文件                 |
| -i   | 交互模式，若目标文件已经存在，则提示用户确认进行覆盖 |
| -s   | 对源文件建立符号链接，而非硬链接                     |
| -v   | 详细信息模式，输出指令的详细执行过程                 |

**参考实例**

```bash
# 创建文件，写入内容
[root@bluecusliyou home]# echo 'i have an apple'>>testfile
[root@bluecusliyou home]# cat testfile
i have an apple
# 创建文件的硬连接
[root@bluecusliyou home]# ln testfile testfile_ln
[root@bluecusliyou home]# cat testfile_ln
i have an apple
# 创建文件的软连接
[root@bluecusliyou home]# ln -s testfile testfile_lns
[root@bluecusliyou home]# cat testfile_lns
i have an apple
# 删除文件
[root@bluecusliyou home]# rm -f testfile
# 连接文件都还在
[root@bluecusliyou home]# ls
testfile_ln  testfile_lns
# 硬链接还可以访问，查看内容
[root@bluecusliyou home]# cat testfile_ln
i have an apple
# 软连接访问报错
[root@bluecusliyou home]# cat testfile_lns
cat: testfile_lns: No such file or directory
```

#### （13）find(根据路径和条件搜索指定文件)

find命令的功能是根据给定的路径和条件查找相关文件或目录，可以使用的参数很多，并且支持正则表达式，结合管道符后能够实现更加复杂的功能，是系统管理员和普通用户日常工作必须掌握的命令之一。

find命令通常进行的是从根目录（/）开始的全盘搜索，有别于whereis、which、locate……等等的有条件或部分文件的搜索。对于服务器负载较高的情况，建议不要在高峰时期使用find命令的模糊搜索，会相对消耗较多的系统资源。

**语法格式**：find [路径] [参数]

**常用参数**：

| -name             | 匹配名称                                                     |
| ----------------- | ------------------------------------------------------------ |
| -perm             | 匹配权限（mode为完全匹配，-mode为包含即可）                  |
| -user             | 匹配所有者                                                   |
| -group            | 匹配所有组                                                   |
| -mtime -n +n      | 匹配修改内容的时间（-n指n天以内，+n指n天以前）               |
| -atime -n +n      | 匹配访问文件的时间（-n指n天以内，+n指n天以前）               |
| -ctime -n +n      | 匹配修改文件权限的时间（-n指n天以内，+n指n天以前）           |
| -nouser           | 匹配无所有者的文件                                           |
| -nogroup          | 匹配无所有组的文件                                           |
| -newer f1 !f2     | 匹配比文件f1新但比f2旧的文件                                 |
| -type b/d/c/p/l/f | 匹配文件类型（后面的字幕字母依次表示块设备、目录、字符设备、管道、链接文件、文本文件） |
| -size             | 匹配文件的大小（+50KB为查找超过50KB的文件，而-50KB为查找小于50KB的文件） |
| -prune            | 忽略某个目录                                                 |
| -exec …… {}\;     | 后面可跟用于进一步处理搜索结果的命令                         |

**参考实例**

```bash
# 查找名字test开头文件
[root@bluecusliyou home]# find ./ -name "test*"
./testfile2
./testdir2
./testdir2/testfile1
./testdir2/testfile2
./testdir2/testdir1
./testdir2/testdir1/testdir1_file1
./testdir2/testdir1/testdir1_file2
./testdir2/testdir2
./testdir2/test.png
./testdir2/test.exe
./testfile1
# 查找大小超过1K文件
[root@bluecusliyou home]# find ./ -size +1k
./testdir2/test.png
./testdir2/test.exe
# 全盘搜索系统中所有类型为目录，且权限为1777的目录文件
[root@bluecusliyou ~]# find / -type d -perm 1777
/dev/mqueue
/dev/shm
/tmp
/tmp/.X11-unix
/tmp/systemd-private-43cf563dbcc645918281642fb6b168fe-chronyd.service-09HBh3/tmp
/tmp/.ICE-unix
/tmp/.Test-unix
/tmp/.font-unix
/tmp/.XIM-unix
/var/tmp
# 搜索当前工作目录中的所有近7天被修改过的文件
[root@bluecusliyou ~]# find . -mtime +7
.
./.ssh
./.ssh/authorized_keys
./.bash_profile
./.bash_logout
./.bashrc
./.nuget
./.nuget/packages
./.nuget/packages/system.numerics.vectors
./.nuget/packages/system.numerics.vectors/4.5.0
# 在/var/log目录下搜索所有后缀不是.log的文件
[root@bluecusliyou ~]# find /var/log ! -name "*.log"
/var/log
/var/log/grubby_prune_debug
/var/log/spooler-20220703
/var/log/dmesg
/var/log/secure-20220703
/var/log/messages-20220717
/var/log/cron-20220703
/var/log/cron-20220717
/var/log/btmp
/var/log/messages-20220724
/var/log/messages-20220703
/var/log/dmesg.old
/var/log/spooler-20220724
/var/log/chrony
# 全盘搜索系统中所有类型为普通文件，且可以执行的文件信息
find / -type f -perm /a=x 
# 全盘搜索系统中所有后缀为.mp4的文件，并删除所有查找到的文件
find / -name "*.mp4" -exec rm -rf {} \;
```

### 3、文件内容查看

#### （1）cat(在终端设备上显示文件内容)

cat命令来自于英文单词concatenate的缩写，其功能是用于查看文件内容。在Linux系统中有很多用于查看文件内容的命令，例如more、tail、head……等等，每个命令都有各自的特点。cat命令适合查看内容较少的、纯文本的文件。

对于内容较多的文件，使用cat命令查看后会在屏幕上快速滚屏，用户往往看不清所显示的具体内容，只好按Ctrl+c键中断命令的执行，所以对于大文件，干脆用more命令吧~

**语法格式：**cat [参数] 文件

**常用参数：**

| -n        | 显示行数（空行也编号）                  |
| --------- | --------------------------------------- |
| -s        | 显示行数（多个空行算一个编号）          |
| -b        | 显示行数（空行不编号）                  |
| -E        | 每行结束处显示$符号                     |
| -T        | 将TAB字符显示为 ^I符号                  |
| -v        | 使用 ^ 和 M- 引用，除了 LFD 和 TAB 之外 |
| -e        | 等价于”-vE”组合                         |
| -t        | 等价于”-vT”组合                         |
| -A        | 等价于 -vET组合                         |
| --help    | 显示帮助信息                            |
| --version | 显示版本信息                            |

**参考实例**

```bash
# 查看文件
[root@bluecusliyou testdir]# cat 3.txt
123
456
[root@bluecusliyou testdir]# cat 4.txt
123
456
# >覆盖
[root@bluecusliyou testdir]# cat 1.txt 2.txt>3.txt
# >>追加
[root@bluecusliyou testdir]# cat 1.txt 2.txt>>4.txt
[root@bluecusliyou testdir]# cat 3.txt
123
456
[root@bluecusliyou testdir]# cat 4.txt
123
456
123
456
# 搭配空设备文件和输出重定向操作符，将某个文件内容清空
[root@bluecusliyou testdir]# cat /dev/null >1.txt 
[root@bluecusliyou testdir]# cat 1.txt
```

#### （2）tac(反向列示文件内容)

tac命令就是将文件反向输出，刚好和cat输出相反。

**语法格式：**tac [参数] [文件]

**常用参数：**

| -b        | 在行前而非行尾添加分隔标志         |
| --------- | ---------------------------------- |
| -r        | 将分隔标志视作正则表达式来解析     |
| -s        | 使用指定字符串代替换行作为分隔标志 |
| --version | 显示版本信息并退出                 |
| --help    | 显示此帮助信息并退出               |

**参考实例**

```bash
[root@bluecusliyou home]# tac testfile2
10
9
8
7
6
5
4
3
2
1
```

#### （3）nl(显示的时候，输出行号)

nl命令是一个很好用的编号过滤工具。该命令可以读取 File 参数（缺省情况下标准输入），计算输入中的行号，将计算过的行号写入标准输出。

**语法格式：**nl [参数] [文件]

**常用参数：**

| -b   | 指定行号指定的方式             |
| ---- | ------------------------------ |
| -n   | 列出行号表示的方式             |
| -w   | 行号栏位的占用的位数           |
| -p   | 在逻辑定界符处不重新开始计算。 |

**参考实例**

```bash
[root@bluecusliyou home]# nl testfile2
     1	1
     2	2
     3	3
     4	4
     5	5
     6	6
     7	7
     8	8
     9	9
    10	10
```

#### （4）more(分页显示文本文件内容)

more命令的功能是用于分页显示文本文件内容。如果文本文件中的内容较多较长，使用cat命令读取后则很难看清，这时使用more命令进行分页查看就更加合适了，可以把文本内容一页一页的显示在终端界面上，用户每按一次回车即向下一行，每按一次空格即向下一页，直至看完为止。

命令内部操作：

- Space键：显示文本的下一屏内容
- Enter键：向下n行，需要定义，默认为1行
- 斜线符\：接着输入一个模式，可以在文本中寻找下一个相匹配的模式
- H键：显示帮助屏
- B键：显示上一屏内容
- Q键：退出more命令
- Ctrl+F、空格键：向下滚动一屏
- Ctrl+B：返回上一屏
- =： 输出当前的行号
- ：f：输出文件名和当前的行号
- V：调用vi编辑器
- !：调用Shell，并执行命令

**语法格式：**more [参数] 文件

**常用参数：**

| -num      | 指定每屏显示的行数                                           |
| --------- | ------------------------------------------------------------ |
| -l        | more在通常情况下把 **^L** 当作特殊字符, 遇到这个字符就会暂停,-l选项可以阻止这种特性 |
| -f        | 计算实际的行数，而非自动换行的行数                           |
| -p        | 先清除屏幕再显示文本文件的剩余内容                           |
| -c        | 与-p相似，不滚屏，先显示内容再清除旧内容                     |
| -s        | 多个空行压缩成一行显示                                       |
| -u        | 禁止下划线                                                   |
| +/pattern | 在每个文档显示前搜寻该字(pattern)，然后从该字串之后开始显示  |
| +num      | 从第 num 行开始显示                                          |

**参考实例**

```bash
[root@bluecusliyou home]# more testfile 
1
2
3
4
5
6
7
8
9
10
1
2
3
4
5
--More--(49%)
```

#### （5）less(可以往前翻页)

less命令的功能是用于分页显示文件内容。分页显示的功能与more命令很相像，但more命令只能从前向后浏览文件内容，而less命令则不仅能从前向后（PageDown键），还可以从后向前（PageUp键）浏览文件内容，更加灵活。

命令内部操作：

- b 向后翻一页
- d 向后翻半页
- h 显示帮助界面
- Q 退出less 命令
- u 向前滚动半页
- y 向前滚动一行
- 空格键 滚动一页
- 回车键 滚动一行

**语法格式：**less [参数] 文件

**常用参数：**

| -b   | 置缓冲区的大小                                       |
| ---- | ---------------------------------------------------- |
| -e   | 当文件显示结束后，自动离开                           |
| -f   | 强迫打开特殊文件，例如外围设备代号、目录和二进制文件 |
| -g   | 只标志最后搜索的关键词                               |
| -i   | 忽略搜索时的大小写                                   |
| -m   | 显示类似more命令的百分比                             |
| -N   | 显示每行的行号                                       |
| -o   | 将less 输出的内容在指定文件中保存起来                |
| -Q   | 不使用警告音                                         |
| -s   | 显示连续空行为一行                                   |
| -S   | 在单行显示较长的内容，而不换行显示                   |
| -x   | 将TAB字符显示为指定个数的空格字符                    |

```bash
[root@bluecusliyou home]# less testfile 
1
2
3
4
5
6
7
8
9
10
(END)
```

#### （6）head(显示文件开头的内容)

head命令的功能是显示文件开头的内容，默认为前10行。

**语法格式：**head [参数] 文件

**常用参数：**

| -n <数字> | 定义显示行数             |
| --------- | ------------------------ |
| -c <数字> | 指定显示头部内容的字符数 |
| -v        | 总是显示文件名的头信息   |
| -q        | 不显示文件名的头信息     |

**参考实例**

```bash
[root@bluecusliyou home]# head -3 testfile 
1
2
3
```

#### （7）tail(查看文件尾部内容)

tail命令的功能是用于查看文件尾部内容，例如默认会在终端界面上显示出指定文件的末尾十行，如果指定了多个文件，则会在显示的每个文件内容前面加上文件名来加以区分。

高阶玩法的-f参数作用是持续显示文件的尾部最新内容，类似于机场候机厅的大屏幕，总会把最新的消息展示给用户，对阅读日志文件尤为适合，而不需要手动刷新。

**语法格式：**tail [参数] 文件

**常用参数：**

| -c             | 输出文件尾部的N（N为整数）个字节内容                         |
| -------------- | ------------------------------------------------------------ |
| -f             | 持续显示文件最新追加的内容                                   |
| -F <N>         | 与选项“-follow=name”和“--retry”连用时功能相同                |
| -n <N>         | 输出文件的尾部N（N位数字）行内容                             |
| --retry        | 即是在tail命令启动时，文件不可访问或者文件稍后变得不可访问，都始终尝试打开文件。 |
| --pid=<进程号> | 与“-f”选项连用，当指定的进程号的进程终止后，自动退出tail命令 |
| --help         | 显示指令的帮助信息                                           |
| --version      | 显示指令的版本信息                                           |

**参考实例**

```bash
[root@bluecusliyou home]# tail -3 testfile 
8
9
10
```

#### （8）wc(统计文件的字节数、单词数、行数)

wc命令来自于英文词组“Word count”的缩写，其功能是用于统计文件的字节数、单词数、行数等信息，并将统计结果输出到终端界面。利用wc命令可以很快的计算出准确的单词数及行数，评估出文本的内容长度，要想了解一个文件，不妨先wc一下吧~

**语法格式：**wc [参数] 文件

**常用参数：**﻿

| -w        | 统计单词数       |
| --------- | ---------------- |
| -c        | 统计字节数       |
| -l        | 统计行数         |
| -m        | 统计字符数       |
| -L        | 显示最长行的长度 |
| --help    | 显示帮助信息     |
| --version | 显示版本信息     |

**参考实例**

```bash
[root@bluecusliyou home]# wc testfile_ln
 1  4 16 testfile_ln
```

#### （9）sort(对文件内容进行排序)

sort命令的功能是对文件内容进行排序。有时文本中的内容顺序不正确，一行行地手动修改实在太麻烦了。此时使用sort命令就再合适不过了，它能够对文本内容进行再次排序。

**语法格式：**sort [参数] 文件

**常用参数：**﻿

| -b            | 忽略每行前面开始出的空格字符              |
| ------------- | ----------------------------------------- |
| -c            | 检查文件是否已经按照顺序排序              |
| -d            | 除字母、数字及空格字符外，忽略其他字符    |
| -f            | 将小写字母视为大写字母                    |
| -i            | 除040至176之间的ASCII字符外，忽略其他字符 |
| -m            | 将几个排序号的文件进行合并                |
| -M            | 将前面3个字母依照月份的缩写进行排序       |
| -n            | 依照数值的大小排序                        |
| -o <输出文件> | 将排序后的结果存入制定的文件              |
| -r            | 以相反的顺序来排序                        |
| -t <分隔字符> | 指定排序时所用的栏位分隔字符              |
| -k            | 指定需要排序的栏位                        |

**参考实例**

```bash
# 对指定的文件内容按照字母顺序进行排序
[root@bluecusliyou testdir]# cat testfile1
aaa
banana
apple
water malen
file fruit
[root@bluecusliyou testdir]# sort testfile1
aaa
apple
banana
file fruit
water malen
# 对指定的文件内容按照数字大小进行排序
[root@bluecusliyou testdir]# cat testfile2
33
123
34
234
56
456
[root@bluecusliyou testdir]# sort testfile2
123
234
33
34
456
56
[root@bluecusliyou testdir]# sort -n testfile2
33
34
56
123
234
456
# 以冒号（：）为间隔符，对指定的文件内容按照数字大小对第3列进行排序
[root@bluecusliyou testdir]# cat testfile3
rtkit:x:172:172:RealtimeKit
unbound:x:996:991:Unbound DNS resolver
geoclue:x:997:995:User for geoclue
polkitd:x:998:996:User for polkitd
qemu:x:107:107:qemu user
usbmuxd:x:113:113:usbmuxd user
pulse:x:171:171:PulseAudio System Daemon
rpc:x:32:32:Rpcbind Daemon
tss:x:59:59:Account used by the trousers package to sandbox the tcsd daemon
gluster:x:995:990:GlusterFS daemons
[root@bluecusliyou testdir]# sort -t : -k 3 -n testfile3
rpc:x:32:32:Rpcbind Daemon
tss:x:59:59:Account used by the trousers package to sandbox the tcsd daemon
qemu:x:107:107:qemu user
usbmuxd:x:113:113:usbmuxd user
pulse:x:171:171:PulseAudio System Daemon
rtkit:x:172:172:RealtimeKit
gluster:x:995:990:GlusterFS daemons
unbound:x:996:991:Unbound DNS resolver
geoclue:x:997:995:User for geoclue
polkitd:x:998:996:User for polkitd
```

### 4、文本三剑客

#### （1）grep(强大的文本搜索工具)

grep来自于英文词组“global search regular expression and print out the line”的缩写，意思是用于全面搜索的正则表达式，并将结果输出。人们通常会将grep命令与正则表达式搭配使用，参数作为搜索过程中的补充或对输出结果的筛选，命令模式十分灵活。

与之容易混淆的是egrep命令和fgrep命令。如果把grep命令当作是标准搜索命令，那么egrep则是扩展搜索命令，等价于“grep -E”命令，支持扩展的正则表达式。而fgrep则是快速搜索命令，等价于“grep -F”命令，不支持正则表达式，直接按照字符串内容进行匹配。

**语法格式：** grep [参数] 文件

**常用参数：**

| -i   | 忽略大小写                                                 |
| ---- | ---------------------------------------------------------- |
| -c   | 只输出匹配行的数量                                         |
| -l   | 只列出符合匹配的文件名，不列出具体的匹配行                 |
| -n   | 列出所有的匹配行，显示行号                                 |
| -h   | 查询多文件时不显示文件名                                   |
| -s   | 不显示不存在、没有匹配文本的错误信息                       |
| -v   | 显示不包含匹配文本的所有行                                 |
| -w   | 匹配整词                                                   |
| -x   | 匹配整行                                                   |
| -r   | 递归搜索                                                   |
| -q   | 禁止输出任何结果，已退出状态表示搜索是否成功               |
| -b   | 打印匹配行距文件头部的偏移量，以字节为单位                 |
| -o   | 与-b结合使用，打印匹配的词据文件头部的偏移量，以字节为单位 |
| -F   | 匹配固定字符串的内容                                       |
| -E   | 支持扩展的正则表达式                                       |

**参考实例**

```bash
# 创建文件内容
[root@bluecusliyou testdir]# echo 'abcefg'>grepfile1.txt
[root@bluecusliyou testdir]# echo 'abc'>grepfile2.txt
[root@bluecusliyou testdir]# echo 'efg'>grepfile3.txt
[root@bluecusliyou testdir]# echo 'abchahahaha'>grepfile4.txt
# 输出匹配字符串内容
[root@bluecusliyou testdir]# grep abc grepfile*
grepfile1.txt:abcefg
grepfile2.txt:abc
grepfile4.txt:abchahahaha
# 输出匹配字符串行数
[root@bluecusliyou testdir]# grep -c abc grepfile*
grepfile1.txt:1
grepfile2.txt:1
grepfile3.txt:0
grepfile4.txt:1
# 输出匹配字符串行号+内容
[root@bluecusliyou testdir]# grep -n abc grepfile*
grepfile1.txt:1:abcefg
grepfile2.txt:1:abc
grepfile4.txt:1:abchahahaha
# 只输出匹配字符串内容
[root@bluecusliyou testdir]# grep -h abc grepfile*
abcefg
abc
abchahahaha
# 只输出匹配文件名
[root@bluecusliyou testdir]# grep -l abc grepfile*
grepfile1.txt
grepfile2.txt
grepfile4.txt
```

**正则匹配：**

| ^a    | 匹配以a开头     |
| ----- | --------------- |
| a$    | 匹配以a结尾     |
| [abc] | 匹配abc中的一个 |
| .     | .匹配一个字符   |

```bash
[root@bluecusliyou home]# cat testfile1
abc
def
aaa
bbb
ccc
ddd
eee
fff
txt
tnt
tpt
[root@bluecusliyou home]# grep '^a' testfile1
abc
aaa
[root@bluecusliyou home]# grep 'a$' testfile1
aaa
[root@bluecusliyou home]# grep 't[xn]t' testfile1
txt
tnt
[root@bluecusliyou home]# grep 't.t' testfile1
txt
tnt
tpt
```

#### （2）awk(文本和数据进行处理的编程语言)

awk命令来自于三位创始人”Alfred Aho，Peter Weinberger, Brian Kernighan “的姓氏缩写，其功能是用于对文本和数据进行处理的编程语言。使用awk命令可以让用户自定义函数或正则表达式对文本内容进行高效管理，与sed、grep并称为Linux系统中的文本三剑客。

**语法格式：**awk 参数 文件

**常用参数：**

| -F   | 指定输入时用到的字段分隔符 |
| ---- | -------------------------- |
| -v   | 自定义变量                 |
| -f   | 从脚本中读取awk命令        |
| -m   | 对val值设置内在限制        |

**内置变量：**

| 变量名称 | 说明                                  |
| -------- | ------------------------------------- |
| ARGC     | 命令行参数个数                        |
| ARGV     | 命令行参数排列                        |
| ENVIRON  | 支持队列中系统环境变量的使用          |
| FILENAME | awk浏览的文件名                       |
| FNR      | 浏览文件的记录数                      |
| FS       | 设置输入域分隔符，等价于命令行 -F选项 |
| NF       | 浏览记录的域的个数                    |
| NR       | 已读的记录数                          |
| OFS      | 输出域分隔符                          |
| ORS      | 输出记录分隔符                        |
| RS       | 控制记录分隔符                        |

**参考实例**

```sql
# 仅显示指定文件中第1、2列的内容（默认以空格为间隔符）
[root@bluecusliyou testdir]# awk ' {print $1,$2} ' /etc/passwd
root:x:0:0:root:/root:/bin/bash 
bin:x:1:1:bin:/bin:/sbin/nologin 
daemon:x:2:2:daemon:/sbin:/sbin/nologin 
adm:x:3:4:adm:/var/adm:/sbin/nologin 
lp:x:4:7:lp:/var/spool/lpd:/sbin/nologin 
sync:x:5:0:sync:/sbin:/bin/sync 
shutdown:x:6:0:shutdown:/sbin:/sbin/shutdown 
...
# 以冒号为间隔符，仅显示指定文件中第1列的内容
[root@bluecusliyou testdir]# awk -F : '{print $1}' /etc/passwd
root
bin
daemon
adm
lp
sync
shutdown
halt
mail
operator
...
# 以冒号为间隔符，显示系统中所有UID号码大于500的用户信息（第3列）
[root@bluecusliyou testdir]# awk -F : '$3>=500' /etc/passwd
polkitd:x:999:998:User for polkitd:/:/sbin/nologin
chrony:x:998:996::/var/lib/chrony:/sbin/nologin
mysql:x:997:1000::/home/mysql:/bin/bash
# 仅显示指定文件中含有指定关键词root的内容
[root@bluecusliyou testdir]# awk '/root/{print}' /etc/passwd
root:x:0:0:root:/root:/bin/bash
operator:x:11:0:operator:/root:/sbin/nologin
# 以冒号为间隔符，仅显示指定文件中最后一个字段的内容
[root@bluecusliyou testdir]# awk -F: '{print $NF}' /etc/passwd
/bin/bash
/sbin/nologin
/sbin/nologin
/sbin/nologin
/sbin/nologin
/bin/sync
/sbin/shutdown
/sbin/halt
/sbin/nologin
/sbin/nologin
/sbin/nologin
/sbin/nologin
...
```

#### （3）sed(批量编辑文本文件)

sed命令来自于英文词组“stream editor”的缩写，其功能是用于利用语法/脚本对文本文件进行批量的编辑操作。sed命令最初由贝尔实验室开发，后被众多Linux系统接纳集成，能够通过正则表达式对文件进行批量编辑，让需要重复的工作不再浪费时间。

**语法格式：**sed 参数 文件

**常用参数：**

| -e或--expression=<script>           | 以指定的脚本来处理输入的文本文件     |
| ----------------------------------- | ------------------------------------ |
| -f<script文件>或--file=<script文件> | 以指定的脚本文件来处理输入的文本文件 |
| -h或--help                          | 显示帮助                             |
| -n或--quiet或--silent               | 仅显示script处理后的结果             |
| -V或--version                       | 显示版本信息                         |

**参考实例**

```sql
# 查找指定文件中带有某个关键词的行
[root@bluecusliyou testdir]# cat -n test.cfg | sed -n '/root/p'
    45      <root>
    49      </root>
# 替换指定文件中某个关键词成大写形式
[root@bluecusliyou testdir]# sed 's/root/ROOT/g' test.cfg
...
# 读取指定文件，删除所有带有某个关键词的行
[root@bluecusliyou testdir]# sed '/root/d' test.cfg
...
# 读取指定文件，在第4行后插入一行新内容NewLine
[root@bluecusliyou testdir]# sed -e 4a\NewLine test.cfg
﻿<?xml version="1.0" encoding="utf-8" ?> 
 <log4net>
    <!-- Define some output appenders -->
    <appender name="rollingAppender" type="log4net.Appender.RollingFileAppender">
NewLine
...
# 读取指定文件，删除第2-5行的内容
[root@bluecusliyou testdir]# cat  -n test.cfg | sed '2,5d'
     1  ﻿<?xml version="1.0" encoding="utf-8" ?> 
     6        
     7        <!--追加日志内容-->
     8        <appendToFile value="true" />
 ...
# 指定读取某个文件的第3-7行
[root@bluecusliyou testdir]# sed -n '3,7p' test.cfg
    <!-- Define some output appenders -->
    <appender name="rollingAppender" type="log4net.Appender.RollingFileAppender">
      <file value="D:\logs\log4net\log4net.log" /> 
      
      <!--追加日志内容-->
```

### 5、压缩和解压

#### （1）tar(压缩和解压缩文件)

它能够将用户所指定的文件或目录打包成一个文件，但不做压缩。一般Linux上常用的压缩方式是选用tar将许多文件打包成一个文件，再以gzip压缩命令压缩成xxx.tar.gz(或称为xxx.tgz)的文件。

tar命令的功能是用于压缩和解压缩文件，能够制作出Linux系统中常见的.tar、.tar.gz、.tar.bz2等格式的压缩包文件。对于RHEL7、CentOS7版本以后的系统，解压时可以不加压缩格式参数（如z或j），系统能自动进行分析并解压。

把要传输的文件先进行压缩再进行传输，能够很好的提高工作效率，方便分享。

**语法格式：**tar 参数 文件或目录

**常用参数：**

| -A                     | 新增文件到以存在的备份文件                          |
| ---------------------- | --------------------------------------------------- |
| -B                     | 设置区块大小                                        |
| -c                     | 建立新的备份文件                                    |
| -C <目录>              | 仅压缩指定目录里的内容或解压缩到指定目录            |
| -d                     | 记录文件的差别                                      |
| -x                     | 从归档文件中提取文件                                |
| -t                     | 列出备份文件的内容                                  |
| -z                     | 通过gzip指令压缩/解压缩文件，文件名最好为*.tar.gz   |
| -Z                     | 通过compress指令处理备份文件                        |
| -f<备份文件>           | 指定备份文件                                        |
| -v                     | 显示指令执行过程                                    |
| -r                     | 添加文件到已经压缩的文件                            |
| -u                     | 添加改变了和现有的文件到已经存在的压缩文件          |
| -j                     | 通过bzip2指令压缩/解压缩文件，文件名最好为*.tar.bz2 |
| -v                     | 显示操作过程                                        |
| -l                     | 文件系统边界设置                                    |
| -k                     | 保留原有文件不覆盖                                  |
| -m                     | 保留文件不被覆盖                                    |
| -w                     | 确认压缩文件的正确性                                |
| -p                     | 保留原来的文件权限与属性                            |
| -P                     | 使用文件名的绝对路径，不移除文件名称前的“/”号       |
| -N <日期格式>          | 只将较指定日期更新的文件保存到备份文件里            |
| -- -exclude=<范本样式> | 排除符合范本样式的文件                              |
| -- -remove-files       | 归档/压缩之后删除源文件                             |

**参考实例**

```bash
# 将所有文件打包成 all.tar  多个文件的时候用空格隔开
[root@bluecusliyou home]# tar -cf all.tar *
[root@bluecusliyou home]# ls
all.tar  testdir2  testfile1  testfile2
# 打包文件后删除原文件
[root@bluecusliyou home]# tar -cvf new.tar all.tar --remove-files
all.tar
[root@bluecusliyou home]# ls
new.tar  testdir2  testfile1  testfile2
# 打包文件以后，以 gzip 压缩
[root@bluecusliyou home]# tar -zcvf newgz.tar.gz new.tar 
new.tar
[root@bluecusliyou home]# ls
newgz.tar.gz  new.tar  testdir2  testfile1  testfile2
# 解包
[root@bluecusliyou home]# tar -xvf new.tar
all.tar
[root@bluecusliyou home]# ls
all.tar  newgz.tar.gz  new.tar  testdir2  testfile1  testfile2
```

#### （2）zip(压缩文件)

zip命令的功能是用于压缩文件，解压命令为unzip。通过zip命令可以将文件打包成.zip格式的压缩包，里面会附含文件的名称、路径、创建时间、上次修改时间等等信息，与tar命令相似。

**语法格式：**zip 参数 文件

**常用参数：**

| -q             | 不显示指令执行过程                               |
| -------------- | ------------------------------------------------ |
| -r             | 递归处理，将指定目录下的所有文件和子目录一并处理 |
| -z             | 替压缩文件加上注释                               |
| -v             | 显示指令执行过程或显示版本信息                   |
| -d             | 更新压缩包内文件                                 |
| -n<字尾字符串> | 不压缩具有特定字尾字符串的文件                   |

**参考实例**

```bash
[root@bluecusliyou home]# ls
all.tar  testdir2  testfile1  testfile2
# 压缩文件
[root@bluecusliyou home]# zip test.zip testfile1 testfile2
  adding: testfile1 (deflated 9%)
  adding: testfile2 (stored 0%)
[root@bluecusliyou home]# ls
all.tar  testdir2  testfile1  testfile2  test.zip
# 解压文件
[root@bluecusliyou home]# unzip ./ test.zip
Archive:  test.zip
replace testfile1? [y]es, [n]o, [A]ll, [N]one, [r]ename: y
  inflating: testfile1               
replace testfile2? [y]es, [n]o, [A]ll, [N]one, [r]ename: y
 extracting: testfile2               
[root@bluecusliyou home]# ls
all.tar  testdir2  testfile1  testfile2  test.zip
```

#### （3）gzip(压缩和解压文件)

gzip命令来自于英文单词gunzip的缩写，其功能是用于压缩和解压文件。gzip是一款使用广泛的压缩工具，文件经过压缩后一般会以.gz后缀结尾，与tar命令合用后即为.tar.gz后缀。

据统计，gzip命令对文本文件的压缩比率通常能达到60%~70%，压缩后可以很好的提升存储空间的使用率，还能够在网络传输文件时减少等待时间。

**语法格式：**gzip [参数] 文件

**常用参数：**

| -a   | 使用ASCII文字模式                                  |
| ---- | -------------------------------------------------- |
| -d   | 解开压缩文件                                       |
| -f   | 强行压缩文件                                       |
| -k   | 保留原文件                                         |
| -l   | 列出压缩文件的相关信息                             |
| -c   | 把压缩后的文件输出到标准输出设备，不去更动原始文件 |
| -r   | 递归处理，将指定目录下的所有文件及子目录一并处理   |
| -q   | 不显示警告信息                                     |

**参考实例**

```bash
[root@bluecusliyou home]# ls
all.tar  testdir2  testfile1  testfile2
# 压缩完删除源文件
[root@bluecusliyou home]# gzip all.tar
[root@bluecusliyou home]# ls
all.tar.gz  testdir2  testfile1  testfile2
# 解压完删除源文件
[root@bluecusliyou home]# gzip -d all.tar.gz
[root@bluecusliyou home]# ls
all.tar  testdir2  testfile1  testfile2
```

#### （4）bzip2(bz2文件的压缩程序)

Linux系统中bzip2命令的英文是“bunzip2”，即.bz2文件格式的压缩程序； bzip2命令系统默认是没有安装的，需要安装bzip2库才可以使用此命令。

bzip2命令采用新的压缩演算法，压缩效果比传统的LZ77/LZ78压缩演算法来得好。若没有加上任何参数，bzip2压缩完文件后会产生.bz2的压缩文件，并删除原始的文件。

tar与bzip2命令结合使用实现文件打包、压缩(用法和gzip一样)。

tar只负责打包文件，但不压缩，用bzip2压缩tar打包后的文件，其扩展名一般用xxxx.tar.gz2。

在tar命令中增加一个选项(-j) 可以调用bzip2实现了一个压缩的功能，实行一个先打包后压缩的过程。

压缩用法：【tar -jcvf 压缩包包名 文件...(tar jcvf bk.tar.bz2 *.c)】  v可以省略

解压用法：【tar -jxvf 压缩包包名 (tar jxvf bk.tar.bz2)】 v可以省略

**语法格式：**bzip2 [参数] 文件系统

**常用参数：**

| -c   | 将压缩与解压缩的结果送到标准输出                             |
| ---- | ------------------------------------------------------------ |
| -d   | 执行解压缩                                                   |
| -f   | bzip2在压缩或解压缩时，若输出文件与现有文件同名，预设不会覆盖现有文件。若要覆盖，请使用此参数 |
| -k   | bzip2在压缩或解压缩后，会删除原始的文件。若要保留原始文件，请使用此参数 |
| -s   | 降低程序执行时内存的使用量                                   |
| -t   | 测试.bz2压缩文件的完整性                                     |
| -v   | 压缩或解压缩文件时，显示详细的信息                           |
| -z   | 强制执行压缩                                                 |

**参考实例**

```bash
# 压缩文件
bzip2 a.txt
# 检查文件完整性
bzip2 -t a.txt.bz2
```

## 五、VIM编辑器

### 1、VIM简介

vi 是老式的文字处理器，所有的 Unix Like 系统都会内建 vi 文本编辑器，功能已经很齐全了，但是还是有可以进步的地方。 

Vim是从 vi 发展出来的一个文本编辑器。代码补完、编译及错误跳转等方便编程的功能特别丰富，在程序员中被广泛使用。尤其是Linux中，必须要会使用Vim（查看内容，编辑内容，保存内容！） 

### 2、VIM键盘图

![b7886b96e7c1c4097d5cc76db5d979c8](http://cdn.bluecusliyou.com/202202171041678.gif)

### 3、三种命令模式

#### （1）命令模式

可以进行行删除、复制等命令的输入，相当于快键键。 默认进入的就是命令模式。

#### （2）输入模式

可以进行文本的输入。

#### （3）底线命令模式

是执行保存、退出等指令用。

![61f84794a56b208bbc83092670e384e5](http://cdn.bluecusliyou.com/202202171041571.png)

```bash
vim filename
```

![1637572297414](http://cdn.bluecusliyou.com/202202171041483.png)

### 4、命令模式说明

#### （1）命令模式

> 光标移动

- h：左移；
- j：下移；
- k：上移；
- l：右移；
- M：移到中间行；
- L：移到屏幕最后一行行首；
- G：移动到指定行   行号-G
- W：向后移动一个字；
- Ctrl+d：向下翻半屏；
- Ctrl+u：向上翻半屏；
- ctrl+b：向上翻一屏；b→backward；
- ctrl+f：向下翻一屏； f→forward；
- gg：光标移动到文件开头；
- shift+^：移到行首；
- shift +$：移到行尾；

> 删除

- x ：删除光标前一个字符；
- X：删除光标后一个字符；
- dw：删除一个单词；
- dl：删除一个字母；
- dd：删除一行；
- d5d：删除5行；

> 复制

- yw：复制一个单词；
- yl：复制一个字母；
- yy：复制一行；
- y5y：复制5行；

> 撤销，恢复

- u：撤销上一次的操作；
- ctrl+r：反撤销，撤销的命令重做；

> 剪切

- cw：剪切一个单词；
- cl：剪切一个字母；
- cc：剪切一行；
- c5c：剪切5行；

> 粘贴

- p：粘贴；

#### （2）插入模式

- i ：光标所在位置插入；
- I： 光标所在位置行首插入；
- o： 光标所在位置下方新开一行插入；
- O： 光标所在位置上方新开一行插入；
- a： 光标所在位置下一个字符的位置插入；a→append（附加）；
- A： 光标所在位置行行尾插入；

#### （3）底线命令模式

- w：写入文件
- w：文件名 另存文件
- q：退出
- q!：不保存退出
- !cmd：执行命令
- /：查找命令
- s/old/new/：替换命令
- set nu：设置命令

## 六、权限管理

- Linux系统是一个多用户多任务的分时操作系统，任何一个要使用系统资源的用户，都必须首先向系统管理员申请一个账号，然后以这个账号的身份进入系统。 

- 用户的账号一方面可以帮助系统管理员对使用系统的用户进行跟踪，并控制他们对系统资源的访问；另一方面也可以帮助用户组织文件，并为用户提供安全性保护。 

- 每个用户账号都拥有一个唯一的用户名和各自的口令。用户在登录时键入正确的用户名和口令后，就能够进入系统和自己的主目录。

- 你一般在公司中，用的应该都不是 root 账户！


### 1、用户管理

#### （1）whoami(打印当前登录用户)

whoami命令是打印与当前有效用户ID关联的用户名；这个工具可以用来获取本地系统上当前用户(访问令牌)的用户名和组信息，以及相应的安全标识符(SID)、声明、本地系统上当前用户的权限、登录标识符(登录 ID)。

whoami命令显示自身的用户名称，本指令相当于执行”id -un”指令。

**语法格式**：whoami [参数]

**常用参数**：

| --help    | 在线帮助     |
| --------- | ------------ |
| --version | 显示版本信息 |

**参考实例**

```bash
[root@bluecusliyou ~]# whoami
root
```

#### （2）who(查看当前登录用户信息)

who命令的功能是用于显示当前登录用户信息，包含登录的用户名、终端、日期时间、进程等信息，帮助运维人员了解当前系统的登入用户情况。

**语法格式：** who [参数]

**常用参数：**

| -a   | 全面信息                         |
| ---- | -------------------------------- |
| -b   | 系统最近启动时间                 |
| -d   | 死掉的进程                       |
| -l   | 系统登录进程                     |
| -H   | 带有列标题打印用户名，终端和时间 |
| -t   | 系统上次锁定时间                 |
| -u   | 已登录用户列表                   |

**参考实例**

```bash
# 查看当前账号
[root@bluecusliyou ~]# who
root     pts/0        2021-11-23 09:29 (112.87.128.140)
# 带有列标题打印
[root@bluecusliyou ~]# who -H
NAME     LINE         TIME             COMMENT
root     pts/0        2021-11-23 09:29 (112.87.128.140)
# 打印全部信息
[root@bluecusliyou ~]# who -H -a
NAME       LINE         TIME             IDLE          PID COMMENT  EXIT
           system boot  2021-11-03 23:58
           run-level 3  2021-11-03 15:58
LOGIN      ttyS0        2021-11-03 15:58              1024 id=tyS0
LOGIN      tty1         2021-11-03 15:58              1023 id=tty1
root     + pts/0        2021-11-23 09:29   .        204944 (112.87.128.140)
# 打印系统最近启动时间
[root@bluecusliyou ~]# who -b
         system boot  2021-11-03 23:58
# 打印系统登录进程
[root@bluecusliyou ~]# who -l
LOGIN    ttyS0        2021-11-03 15:58              1024 id=tyS0
LOGIN    tty1         2021-11-03 15:58              1023 id=tty1
```

#### （3）exit(退出终端)

- 如果是图形界面，退出当前终端；
- 如果是使用ssh远程登录，退出登陆账户；
- 如果是切换后的登陆用户，退出则返回上一个登陆账号。
- ctrl+d等价于exit。

**语法格式：**exit [状态值]

**常用参数：**

| 0    | 执行成功         |
| ---- | ---------------- |
| 1    | 执行失败         |
| $?   | 参照上一个状态值 |

#### （4）useradd(添加用户)

- Linux每个用户都要有一个主目录，主目录就是第一次登陆系统，用户的默认当前目录(/home/用户)；
- 每一个用户必须有一个主目录，所以用useradd创建用户的时候，一定给用户指定一个主目录(或者自动默认创建)；
- 用户的主目录一般要放到根目录的home目录下，用户的主目录和用户名是相同的；
- 如果创建用户的时候，不指定组名，那么系统会自动创建一个和用户名一样的组名。
- 已创建的用户则需使用chmod命令修改账户信息，passwd命令修改密码信息。

**语法格式：**useradd [参数] 用户名

**常用参数：**

| -D   | 改变新建用户的预设值                                         |
| ---- | ------------------------------------------------------------ |
| -c   | 添加备注文字                                                 |
| -d   | 新用户每次登陆时所使用的家目录                               |
| -e   | 用户终止日期，日期的格式为YYYY-MM-DD                         |
| -f   | 用户过期几日后永久停权。当值为0时用户立即被停权，而值为-1时则关闭此功能，预设值为-1 |
| -g   | 指定用户对应的用户组                                         |
| -G   | 定义此用户为多个不同组的成员                                 |
| -m   | 用户目录不存在时则自动创建                                   |
| -M   | 不建立用户家目录，优先于/etc/login.defs文件设定              |
| -n   | 取消建立以用户名称为名的群组                                 |
| -r   | 建立系统帐号                                                 |
| -u   | 指定用户id                                                   |

**参考实例**

```bash
# 创建用户名，并且创建与用户名相同的目录和用户组
[root@bluecusliyou ~]# useradd -m liyou1
[root@bluecusliyou home]# ls
liyou1
# 指定用户目录
[root@bluecusliyou home]# useradd -d /home/liyouxx -m liyou2
[root@bluecusliyou home]# ls
liyou1  liyouxx
# 指定用户组
[root@bluecusliyou home]# useradd -g ftp liyou3
[root@bluecusliyou home]# ls
liyou1  liyouxx  liyou3
# 指定用户密码
[root@bluecusliyou home]# useradd -p 123456 liyou4
[root@bluecusliyou home]# cat /etc/shadow
...
liyou1:!!:18954:0:99999:7:::
liyou2:!!:18954:0:99999:7:::
liyou3:!!:18954:0:99999:7:::
liyou4:123456:18954:0:99999:7:::
```

#### （5）passwd(修改用户的密码值)

passwd命令来自于英文单词password的缩写，其功能适用于修改用户的密码值。同时也可以对用户进行锁定等操作，但需要管理员身份才可以执行。

**常用格式：**passwd [参数] 用户名

**常用参数：**

| -d   | 删除已有密码                 |
| ---- | ---------------------------- |
| -l   | 锁定用户的密码值，不允许修改 |
| -u   | 解锁用户的密码值，允许修改   |
| -e   | 下次登陆强制修改密码         |
| -k   | 用户在期满后能仍能使用       |
| -S   | 查询密码状态                 |

**参考实例**

```bash
# 设定当前用户密码
[root@bluecusliyou home]# passwd
# 设置用户密码 两次确认
[root@bluecusliyou home]# passwd liyou1
Changing password for user liyou1.
New password: 
BAD PASSWORD: The password is shorter than 8 characters
Retype new password: 
passwd: all authentication tokens updated successfully.
# 锁定密码不允许用户修改
[root@bluecusliyou home]# passwd -l liyou1
Locking password for user liyou1.
passwd: Success
# 解除锁定密码，允许用户修改
[root@bluecusliyou home]# passwd -u liyou1
Unlocking password for user liyou1.
passwd: Success
# 下次登陆强制改密码
[root@bluecusliyou home]# passwd -e liyou1
Expiring password for user liyou1.
passwd: Success
# 移出用户密码
[root@bluecusliyou home]# passwd -d liyou1
Removing password for user liyou1.
passwd: Success
# 查看密码状态
[root@bluecusliyou home]# passwd -S liyou1
liyou1 NP 2021-11-23 0 99999 7 -1 (Empty password.)
```

#### （6）userdel(删除用户)

userdel命令来自于英文词组“user delete”的缩写，其功能是删除用户账户。Linux系统中一切都是文件，用户信息被保存到了/etc/passwd、/etc/shadow以及/etc/group文件中，因此使用userdel命令实际就是帮助我们删除了指定用户在上述三个文件中的对应信息。

**语法格式：**userdel [参数] 用户名

**常用参数：**

| -f   | 强制删除用户账号               |
| ---- | ------------------------------ |
| -r   | 删除用户主目录及其中的任何文件 |
| -h   | 显示命令的帮助信息             |

**参考实例**

```bash
#删除用户，但不删除其家目录及文件
[root@bluecusliyou home]# userdel liyou2
# 删除用户，并将其家目录及文件一并删除
[root@bluecusliyou home]# userdel -r liyou3
# 强制删除用户
[root@bluecusliyou home]# userdel -f liyou1
userdel: user liyou1 is currently used by process 208027
```

#### （7）sudo(授权普通用户执行管理员命令)

sudo命令来自于英文词组“super user do”的缩写，中文译为“超级用户才能干的事”，其功能是用于授权普通用户执行管理员命令。使用su命令变更用户身份虽然好用，但是需要将管理员的账户密码告诉他人，总感觉心里不踏实，幸好有了sudo服务。

使用sudo服务可以授权某个指定的用户去执行某些指定的命令，在满足工作需求的前提下尽可能少的放权，保证服务器的安全。配置sudo服务可以直接编辑配置文件/etc/sudoers，亦可以执行visudo命令进行设置，一切妥当后普通用户便能够使用sudo命令进行操作了。

**为新用户添加sudo权限**，接使用 vi 或者 vim 命名“vi /etc/sudoers”进入编辑模式(visudo命令)，在“root ALL=(ALL) ALL”这一行下面添加一行“liyou ALL=(ALL) ALL”，然后保存退出即可。（注明：liyou 为你的用户名）

**语法格式：**sudo [参数] 命令

**常用参数：**

| -v   | 本次需要验证当前用户的密码 |
| ---- | -------------------------- |
| -k   | 下次强制验证当前用户的密码 |
| -b   | 将要执行的指令放在后台执行 |
| -p   | 更改需要密码验证时的提示语 |
| -s   | 指定默认调用的SHELL解释器  |

**参考实例**

```bash
# 查看当前用户有哪些被sudo服务授权的命令
[root@bluecusliyou ~]# sudo -l
匹配 %2$s 上 %1$s 的默认条目：
    !visiblepw, always_set_home, match_group_by_gid, always_query_group_plugin, env_reset, env_keep="COLORS
    DISPLAY HOSTNAME HISTSIZE KDEDIR LS_COLORS", env_keep+="MAIL PS1 PS2 QTDIR USERNAME LANG LC_ADDRESS
    LC_CTYPE", env_keep+="LC_COLLATE LC_IDENTIFICATION LC_MEASUREMENT LC_MESSAGES", env_keep+="LC_MONETARY
    LC_NAME LC_NUMERIC LC_PAPER LC_TELEPHONE", env_keep+="LC_TIME LC_ALL LANGUAGE LINGUAS _XKB_CHARSET
    XAUTHORITY", secure_path=/sbin\:/bin\:/usr/sbin\:/usr/bin

用户 root 可以在 bluecusliyou 上运行以下命令：
    (ALL) ALL
# 使用某个被sudo服务允许的用户身份来执行管理员的重启命令
[root@bluecusliyou ~]# sudo -u liyou "reboot"
# 使用当前用户身份，基于sudo命令来执行管理员的重启命令
[root@bluecusliyou ~]# sudo reboot
```


#### （8）su(切换用户身份)

- su命令来自于英文单词“switch user”的缩写，其功能是用于切换用户身份。
- 普通用户切换到root用户，可以使用su -- 或su root,但是必须输入root密码才能完成切换。root用户切换到普通用户，可以使用su username,不需要输入任何密码即可完成切换。
- 添加单个减号（-）参数为完全的身份变更，不保留任何之前用户的环境变量信息。

**语法格式:** su [参数] 用户名

**常用参数：**

| 单个减号（-） | 完全身份变更                               |
| ------------- | ------------------------------------------ |
| -c            | 执行完指定的指令后，即恢复原来的身份       |
| -f            | 适用于csh与tsch，使shell不用去读取启动文件 |
| -l            | 改变身份时，也同时变更工作目录             |
| -m            | 变更身份时，不要变更环境变量               |
| -s            | 指定要执行的shell                          |
| --help        | 显示帮助信息                               |
| --version     | 显示版本信息                               |

**参考实例**

```bash
# 变更至指定用户身份
[root@bluecusliyou home]# su root
# 完全变更至指定用户身份
[root@bluecusliyou home]# su - root
Last login: Tue Nov 23 16:10:52 CST 2021 on pts/0
[root@bluecusliyou ~]#
```

#### （9）usermod(修改用户账号信息)

usermod命令来自于英文词组“user modify”的缩写，其功能是用于修改用户账号中的各项参数。

**语法格式：**usermod [参数] 用户名

**常用参数：**

| -c<备注>     | 修改用户账号的备注文字             |
| ------------ | ---------------------------------- |
| -d<登入目录> | 修改用户登入时的家目录             |
| -e<有效期限> | 修改账号的有效期限                 |
| -f<缓冲天数> | 修改在密码过期后多少天即关闭该账号 |
| -g<群组>     | 修改用户所属的群组                 |
| -G<群组>     | 修改用户所属的附加群组             |
| -l<账号名称> | 修改用户账号名称                   |
| -L           | 锁定用户密码，使密码无效           |
| -s<shell>    | 修改用户登入后所使用的shell        |
| -u<uid>      | 修改用户ID                         |
| -U           | 解除密码锁定                       |

**参考实例**

```bash
# 更改登陆目录
[root@bluecusliyou home]# usermod -d /home/liyouxx liyou4
#改变用户的uid
[root@bluecusliyou home]# usermod -u 777 liyou4
# 修改用户名liyou4为liyou
[root@bluecusliyou home]# usermod -l liyou liyou4
# 锁定密码
[root@bluecusliyou home]# usermod -L liyou
# 解锁密码
[root@bluecusliyou home]# usermod -U liyou
```

### 2、用户组管理

#### （1）groupadd(创建新的用户组)

groupadd命令来自于英文词组“group add”，其功能是用于创建新的用户组。每个用户在创建时都有一个与其同名的基本组，后期可以使用groupadd命令创建出新的用户组信息，让多个用户加入到指定的扩展组中，为后续的工作提供了良好的文档共享环境。

**语法格式：**groupadd [参数] 用户组

**常用参数：**

| -g   | 指定新建工作组的id            |
| ---- | ----------------------------- |
| -r   | 创建系统工作组                |
| -K   | 覆盖配置文件“/ect/login.defs” |
| -o   | 允许添加组ID号不唯一的工作组  |

**参考实例**

```bash
# 创建用户组
[root@bluecusliyou ~]# groupadd usergrouptest1
# 创建用户组，指定工作组id
[root@bluecusliyou ~]# groupadd -g 8563 usergrouptest2
# 使用-r创建系统工作组
[root@bluecusliyou ~]# groupadd -r usergrouptest3
```

#### （2）groupdel(删除用户组)

groupdel命令用于删除指定的工作组，本命令要修改的系统文件包括/ect/group和/ect/gshadow。

userdel修改系统账户文件，删除与 GROUP 相关的所有项目。给出的组名必须存在。若该群组中仍包括某些用户，则必须先删除这些用户后，方能删除群组。

**语法格式**：groupdel [参数] [群组名称]

**常用参数**：

| -h   | 显示帮助信息                                               |
| ---- | ---------------------------------------------------------- |
| -R   | 在chroot_dir目录中应用更改并使用chroot_dir目录中的配置文件 |

**参考实例**

```bash
[root@bluecusliyou ~]# groupdel usergrouptest1
```

#### （3）gpasswd(设置管理用户组)

gpasswd命令是Linux下工作组文件/etc/group和/etc/gshadow的管理工具 ，系统管理员可以使用-a选项定义组管理员，使用-m选项定义成员，由组管理员用组名调用的gpasswd只提示输入组的新密码。

**语法格式：**gpasswd [参数]

**常用参数：**

| -a   | 添加用户到组                                         |
| ---- | ---------------------------------------------------- |
| -d   | 从组删除用户                                         |
| -A   | 指定管理员                                           |
| -M   | 指定组成员和-A的用途差不多                           |
| -r   | 删除密码                                             |
| -R   | 限制用户登入组，只有组中的成员才可以用newgrp加入该组 |

**参考实例**

```bash
# 添加用户到组
[root@bluecusliyou home]# gpasswd -a liyou usergrouptest2
Adding user liyou to group usergrouptest2
# 移除用户出组
[root@bluecusliyou home]# gpasswd -d liyou usergrouptest2
Removing user liyou from group usergrouptest2
```

#### （4）groupmod(更改群组识别码或名称)

groupmod命令用于更改群组的识别码或名称时。不过大家还是要注意，用户名不要随意修改，组名和 GID 也不要随意修改，因为非常容易导致管理员逻辑混乱。如果非要修改用户名或组名，则建议大家先删除旧的，再建立新的。

**语法格式：**groupmod [参数]

**常用参数：**

| -g   | 设置欲使用的群组识别码 |
| ---- | ---------------------- |
| -o   | 重复使用群组识别码     |
| -n   | 设置欲使用的群组名称   |

**参考实例**

```bash
# 更改组ID
[root@bluecusliyou home]# groupmod -g 222 usergrouptest2
# 更改组名
[root@bluecusliyou home]# groupmod -n usergrouptestxx usergrouptest2
```

### 3、查看用户信息

```bash
# 查看用户信息
[root@bluecusliyou ~]# cat /etc/passwd
root:x:0:0:root:/root:/bin/bash
bin:x:1:1:bin:/bin:/sbin/nologin
daemon:x:2:2:daemon:/sbin:/sbin/nologin
adm:x:3:4:adm:/var/adm:/sbin/nologin
lp:x:4:7:lp:/var/spool/lpd:/sbin/nologin
sync:x:5:0:sync:/sbin:/bin/sync
...
# 查看用户密码  密码是加密的，没有密码就是！！
[root@bluecusliyou ~]#cat /etc/shadow
...
liyou1:!!:18954:0:99999:7:::
liyou2:!!:18954:0:99999:7:::
liyou3:!!:18954:0:99999:7:::
liyou4:123456:18954:0:99999:7:::
# 查看所有用户组
[root@bluecusliyou home]# cat /etc/group
...
liyou1:x:1000:
liyou2:x:1001:
liyou4:x:1003: 
```

### 4、文件权限

- Linux 系统是一种典型的多用户系统，不同的用户处于不同的地位，拥有不同的权限。

- 为了保护系统的安全性，Linux 系统对不同的用户访问同一文件（包括目录文件）的权限做了不同的规定。


#### （1）文件基本属性查看

在 Linux 中我们可以使用 ==**ll**== 或者 ==**ls –l**== 命令来显示一个文件的属性以及文件所属的用户和组，如：

```bash
[root@bluecusliyou /]# ls -l
total 24
dr-xr-xr-x.   5 root root 4096 Nov 20  2020 boot
drwxr-xr-x   19 root root 2960 Jul 14 14:27 dev
drwxr-xr-x.  94 root root 8192 Jul 14 14:24 etc
drwxr-xr-x.   2 root root    6 May 11  2019 home
lrwxrwxrwx.   1 root root    7 May 11  2019 lib -> usr/lib
lrwxrwxrwx.   1 root root    9 May 11  2019 lib64 -> usr/lib64
drwxr-xr-x.   2 root root    6 May 11  2019 media
drwxr-xr-x.   2 root root    6 May 11  2019 mnt
drwxr-xr-x.   2 root root    6 May 11  2019 opt
dr-xr-xr-x  102 root root    0 Jul 14 14:24 proc
dr-xr-x---.   5 root root  174 Jul 13 15:59 root
drwxr-xr-x   30 root root  880 Jul 14 14:24 run
lrwxrwxrwx.   1 root root    8 May 11  2019 sbin -> usr/sbin
drwxr-xr-x.   2 root root    6 May 11  2019 srv
dr-xr-xr-x   13 root root    0 Jul 14  2021 sys
drwxrwxrwt.   9 root root 4096 Jul 14 15:04 tmp
drwxr-xr-x.  12 root root  144 Nov 20  2020 usr
drwxr-xr-x.  21 root root 4096 Nov 20  2020 var
lrwxrwxrwx.   1 root root    7 May 11  2019 bin -> usr/bin
```

#### （2）文件基本属性说明

![file-llls22](http://cdn.bluecusliyou.com/202202171044165.jpg)

> 每个文件的属性由左边第一部分的 10 个字符来确定。

![363003_1227493859FdXT](http://cdn.bluecusliyou.com/202202171044712.png)

> 在 Linux 中第一个字符代表这个文件类型。

- ==当为 **d** 则是目录；==
- ==当为 **-** 则是文件；==
- ==若是 **l** 则表示为链接文档(link file)；==
- 若是 **b** 则表示为装置文件里面的可供储存的接口设备(可随机存取装置)；
- 若是 **c** 则表示为装置文件里面的串行端口设备，例如键盘、鼠标(一次性读取装置)。

> 接下来的字符中，以三个为一组，且均为 **rwx** 的三个参数的组合。其中， **r** 代表可读(read)、 **w** 代表可写(write)、 **x** 代表可执行(execute)。 要注意的是，这三个权限的位置不会改变，如果没有权限，就会出现减号 **-** 而已。

- 第**1-3**位确定属主（该文件的所有者）拥有该文件的权限。
- 第**4-6**位确定属组（所有者的同组用户）拥有该文件的权限。
- 第**7-9**位确定其他用户拥有该文件的权限。
- ==对于 root 用户来说，一般情况下，文件的权限对其不起作用。==

> 文件和目录权限格式相容，功能不同

- 文件：r代表可读(read)、 w 代表可写(write)、 x 代表可执行(execute)。
- 目录：rx(进入目录读取文件名)，xw(修改目录内文件名)，x(进入目录)。

#### （3）chmod(改变文件或目录权限)

- chmod命令来自于英文词组”change mode“的缩写，其功能是用于改变文件或目录权限的命令。默认只有文件的所有者和管理员可以设置文件权限，普通用户只能管理自己文件的权限属性。
- Linux文件属性有两种设置方法，一种是数字，一种是符号。
- Linux 文件的基本权限就有九个，分别是 **owner/group/others(拥有者/组/其他)** 三种身份各有自己的 **read/write/execute** 权限。

> 数字类型改变文件权限

文件的权限字符为： **-rwxrwxrwx** ， 这九个权限是三个三个一组的！其中，我们可以使用数字来代表各个权限，各权限的数值对照表如下：

- r:4
- w:2
- x:1

每种身份(owner/group/others)各自的三个权限(r/w/x)分数是需要累加的，例如当权限为： **-rwxrwx---** 分数则是：

- owner = rwx = 4+2+1 = 7
- group = rwx = 4+2+1 = 7
- others= --- = 0+0+0 = 0

> 符号类型改变文件权限

权限的所有者

| u    | user：用户      |
| ---- | --------------- |
| g    | group：组       |
| o    | others：其他    |
| a    | all表示三者都是 |

修改权限所用的符号

| +    | 增加权限 |
| ---- | -------- |
| -    | 减少权限 |
| =    | 设定权限 |

**语法格式：** chmod 参数 文件

**常用参数：**

| -c   | 若该文件权限确实已经更改，才显示其更改动作                   |
| ---- | ------------------------------------------------------------ |
| -f   | 若该文件权限无法被更改也不显示错误讯息                       |
| -v   | 显示权限变更的详细资料                                       |
| -R   | 对目前目录下的所有文件与子目录进行相同的权限变更(即以递回的方式逐个变更) |

**参考实例**

```bash
[root@bluecusliyou home]# ll
total 4
drwx------ 2 1003 liyou4           62 Nov 23 14:48 liyou4
-rw-r--r-- 1 root usergrouptest3    0 Nov 23 22:37 testfile1
-rw-r--r-- 1 root usergrouptest3 1333 Nov 23 19:40 test.log
# 数字类型改变文件权限
[root@bluecusliyou home]# chmod 777 liyou4
[root@bluecusliyou home]# ll
total 4
drwxrwxrwx 2 1003 liyou4           62 Nov 23 14:48 liyou4
-rw-r--r-- 1 root usergrouptest3    0 Nov 23 22:37 testfile1
-rw-r--r-- 1 root usergrouptest3 1333 Nov 23 19:40 test.log
# 数字类型改变文件权限
[root@bluecusliyou home]# chmod 761 testfile1
[root@bluecusliyou home]# ll
total 4
drwxrwxrwx 2 1003 liyou4           62 Nov 23 14:48 liyou4
-rwxrw---x 1 root usergrouptest3    0 Nov 23 22:37 testfile1
-rw-r--r-- 1 root usergrouptest3 1333 Nov 23 19:40 test.log
# 符号类型改变文件权限
[root@bluecusliyou home]# chmod u=r,g+w,o-r test.log
[root@bluecusliyou home]# ll
total 4
drwxrwxrwx 2 1003 liyou4           62 Nov 23 14:48 liyou4
-rwxrw---x 1 root usergrouptest3    0 Nov 23 22:37 testfile1
-r--rw---- 1 root usergrouptest3 1333 Nov 23 19:40 test.log
```

#### （4）chown(改变文件或目录用户和用户组)

chown命令来自于英文词组”Change owner“的缩写，其功能是用于改变文件或目录的用户和用户组信息。管理员可以改变一切文件的所属信息，而普通用户只能改变自己文件的所属信息。

**语法格式：**chown [参数] 所属主:所属组 文件

**常用参数：**

| -R        | 对目前目录下的所有文件与目录进行相同的变更 |
| --------- | ------------------------------------------ |
| -c        | 显示所属信息变更信息                       |
| -f        | 若该文件拥有者无法被更改也不要显示错误     |
| -h        | 只对于链接文件进行变更，而非真正指向的文件 |
| -v        | 显示拥有者变更的详细资料                   |
| --help    | 显示辅助说明                               |
| --version | 显示版本                                   |

**参考实例**

```bash
[root@bluecusliyou home]# ll
total 4
drwxrwxrwx 2 1003 liyou4           62 Nov 23 14:48 liyou4
-rwxrw---x 1 root usergrouptest3    0 Nov 23 22:37 testfile1
-r--rw---- 1 root usergrouptest3 1333 Nov 23 19:40 test.log
# 修改所有者
[root@bluecusliyou home]# chown liyou test.log
[root@bluecusliyou home]# ll
total 4
drwxrwxrwx 2  1003 liyou4           62 Nov 23 14:48 liyou4
-rwxrw---x 1 root  usergrouptest3    0 Nov 23 22:37 testfile1
-r--rw---- 1 liyou usergrouptest3 1333 Nov 23 19:40 test.log
# 修改所有者和用户组
[root@bluecusliyou home]# chown root:liyou4 test.log
[root@bluecusliyou home]# ll
total 4
drwxrwxrwx 2 1003 liyou4           62 Nov 23 14:48 liyou4
-rwxrw---x 1 root usergrouptest3    0 Nov 23 22:37 testfile1
-r--rw---- 1 root liyou4         1333 Nov 23 19:40 test.log
```

#### （5）chgrp(更改文件用户组)
chgrp是英语单词“change group”的缩写，命令的作用和其中文释义一样，为用于变更文件或目录的所属群组。

**语法格式:** chgrp [参数] [目录]

**常用参数：**﻿

| -c          | 效果类似”-v”参数，但仅回报更改的部分                         |
| ----------- | ------------------------------------------------------------ |
| -f          | 不显示错误信息                                               |
| -h          | 对符号连接的文件作修改，而不更动其他任何相关文件             |
| -R          | 递归处理，将指定目录下的所有文件及子目录一并处理             |
| -v          | 显示指令执行过程                                             |
| --reference | 把指定文件或目录的所属群组全部设成和参考文件或目录的所属群组相同 |

**参考实例**

```bash
[root@bluecusliyou home]# ll
total 4
drwxrwxrwx 2 1003 liyou4           62 Nov 23 14:48 liyou4
-rwxrw---x 1 root usergrouptest3    0 Nov 23 22:37 testfile1
-r--rw---- 1 root liyou4         1333 Nov 23 19:40 test.log
# 修改用户组
[root@bluecusliyou home]# chgrp liyou4 testfile1
[root@bluecusliyou home]# ll
total 4
drwxrwxrwx 2 1003 liyou4   62 Nov 23 14:48 liyou4
-rwxrw---x 1 root liyou4    0 Nov 23 22:37 testfile1
-r--rw---- 1 root liyou4 1333 Nov 23 19:40 test.log
```

## 七、系统管理

### 1、磁盘管理

#### （1）df(显示磁盘空间使用情况)

df命令来自于英文词组”Disk Free“的缩写，其功能是用于显示系统上磁盘空间的使用量情况。df命令显示的磁盘使用量情况含可用、已有及使用率等信息，默认单位为Kb，建议使用-h参数进行单位换算，毕竟135M比138240Kb更利于阅读对吧~

**语法格式：** df [参数] [对象磁盘/分区]

**常用参数：**

| -a                | 显示所有系统文件                     |
| ----------------- | ------------------------------------ |
| -B <块大小>       | 指定显示时的块大小                   |
| -h                | 以容易阅读的方式显示                 |
| -H                | 以1000字节为换算单位来显示           |
| -i                | 显示索引字节信息                     |
| -k                | 指定块大小为1KB                      |
| -l                | 只显示本地文件系统                   |
| -t <文件系统类型> | 只显示指定类型的文件系统             |
| -T                | 输出时显示文件系统类型               |
| -- -sync          | 在取得磁盘使用信息前，先执行sync命令 |

**参考实例**

```bash
# 显示磁盘分区使用情况
[root@bluecusliyou home]# df
Filesystem     1K-blocks    Used Available Use% Mounted on
devtmpfs         1823120       0   1823120   0% /dev
tmpfs            1838600       0   1838600   0% /dev/shm
tmpfs            1838600     672   1837928   1% /run
tmpfs            1838600       0   1838600   0% /sys/fs/cgroup
/dev/vda1       41931756 9725920  32205836  24% /
overlay         41931756 9725920  32205836  24% /var/lib/docker/overlay2/9f51bd9261e59dfee0d46bf819ab138f1e7d987d6273827c8dc930f493b69ae2/merged
overlay         41931756 9725920  32205836  24% /var/lib/docker/overlay2/a82ac32c675e55780142d55438e2df39895876f236a238c9488fa5ea58ccfc2a/merged
overlay         41931756 9725920  32205836  24% /var/lib/docker/overlay2/cc7094dd7f485ea8215be462c88ae3af286e2974576b1cc660886a44610081bd/merged
tmpfs             367720       0    367720   0% /run/user/0
# 以容易阅读的方式显示磁盘分区使用情况
[root@bluecusliyou home]# df -h
Filesystem      Size  Used Avail Use% Mounted on
devtmpfs        1.8G     0  1.8G   0% /dev
tmpfs           1.8G     0  1.8G   0% /dev/shm
tmpfs           1.8G  672K  1.8G   1% /run
tmpfs           1.8G     0  1.8G   0% /sys/fs/cgroup
/dev/vda1        40G  9.3G   31G  24% /
overlay          40G  9.3G   31G  24% /var/lib/docker/overlay2/9f51bd9261e59dfee0d46bf819ab138f1e7d987d6273827c8dc930f493b69ae2/merged
overlay          40G  9.3G   31G  24% /var/lib/docker/overlay2/a82ac32c675e55780142d55438e2df39895876f236a238c9488fa5ea58ccfc2a/merged
overlay          40G  9.3G   31G  24% /var/lib/docker/overlay2/cc7094dd7f485ea8215be462c88ae3af286e2974576b1cc660886a44610081bd/merged
tmpfs           360M     0  360M   0% /run/user/0
# 显示指定文件所在分区的磁盘使用情况
[root@bluecusliyou home]# df /etc/dhcp
Filesystem     1K-blocks    Used Available Use% Mounted on
/dev/vda1       41931756 9725920  32205836  24% /
# 显示文件类型为ext4的磁盘使用情况
[root@bluecusliyou home]# df -t ext4
df: no file systems processed
```

#### （2）du(查看文件或目录的大小)

du命令来自于英文词组“Disk Usage”的缩写，其功能是用于查看文件或目录的大小。人们经常会把df和du命令混淆，df是用于查看磁盘或分区使用情况的命令，而du命令则是用于按照指定容量单位来查看文件或目录在磁盘中的占用情况。

**语法格式：**du [参数] 文件

**常用参数：**﻿

| -a   | 显示目录中所有文件大小 |
| ---- | ---------------------- |
| -k   | 以KB为单位显示文件大小 |
| -m   | 以MB为单位显示文件大小 |
| -g   | 以GB为单位显示文件大小 |
| -h   | 以易读方式显示文件大小 |
| -s   | 仅显示总计             |

**参考实例**﻿

```bash
# 显示文件夹大小
[root@bluecusliyou home]# du /home
12	/home/liyou4
12	/home
# 以易读方式显示文件夹内及子文件夹大小
[root@bluecusliyou home]# du -h /home
12K	/home/liyou4
12K	/home
# 以易读方式显示文件夹内所有文件大小
[root@bluecusliyou home]# du -ah /home
4.0K	/home/liyou4/.bash_logout
4.0K	/home/liyou4/.bash_profile
4.0K	/home/liyou4/.bashrc
12K	/home/liyou4
12K	/home
# 显示文件大小
[root@bluecusliyou home]# du test.log
4	test.log
```

#### （3）fdisk(管理磁盘分区)

fdisk命令来自于英文词组“Partition table manipulator for Linux”的缩写，其功能是用于管理磁盘的分区信息。如果一套几百平米的房子内部没有墙壁，虽然看起来会很敞亮，但是各种声音、气味、物品会随意充斥在整个房子内，让人极不舒适，因此需要用墙壁按照功能进行划分，例如卧室、厕所、厨房、阳台等等。

fdisk命令可以用于对磁盘进行分区操作，用户可以根据实际情况进行合理划分，这样后期挂载和使用时会方便很多。

**语法格式：**fdisk [参数] [设备]

**常用参数：**

| -b   | 指定每个分区的大小                                           |
| ---- | ------------------------------------------------------------ |
| -l   | 列出指定的外围设备的分区表状况                               |
| -s   | 将指定的分区大小输出到标准输出上，单位为区块                 |
| -u   | 搭配”-l”参数列表，会用分区数目取代柱面数目，来表示每个分区的起始地址 |
| -v   | 显示版本信息                                                 |

**参考实例**

```bash
# 查看所有分区情况
[root@bluecusliyou home]# fdisk -l
Disk /dev/vda: 40 GiB, 42949672960 bytes, 83886080 sectors
Units: sectors of 1 * 512 = 512 bytes
Sector size (logical/physical): 512 bytes / 512 bytes
I/O size (minimum/optimal): 512 bytes / 512 bytes
Disklabel type: dos
Disk identifier: 0x319ba3a3

Device     Boot Start      End  Sectors Size Id Type
/dev/vda1  *     2048 83886046 83883999  40G 83 Linux
```

#### （4）mkfs(对设备进行格式化文件系统操作)

mkfs命令来自于英文词组“make file system”的缩写，其功能是用于对设备进行格式化文件系统操作。在挂载使用硬盘空间前的最后一步，运维人员需要对整块硬盘或指定分区进行格式化文件系统操作，Linux系统支持的文件系统包含ext2、ext3、ext4、xfs、fat、msdos、vfat、minix等多种格式。

**语法格式：** mkfs [参数] 设备名

**常用参数：**

| -V   | 详细显示模式         |
| ---- | -------------------- |
| -t   | 给定档案系统的型式   |
| -c   | 检查该设备是否有损坏 |

#### （5）mount(把文件系统挂载到目录)

mount命令的功能是用于把文件系统挂载到目录，文件系统指的是被格式化过的硬盘或分区设备，进行挂载操作后，用户便可以在挂载目录中使用硬盘资源了。

默认情况下Linux系统并不会像Windows系统那样自动的挂载光盘和U盘设备，需要自行完成。

**语法格式：**mount [参数] [设备] [挂载点]

**常用参数：**﻿

| -t   | 指定挂载类型                             |
| ---- | ---------------------------------------- |
| -l   | 显示已加载的文件系统列表                 |
| -h   | 显示帮助信息并退出                       |
| -V   | 显示程序版本                             |
| -n   | 加载没有写入文件“/etc/mtab”中的文件系统  |
| -r   | 将文件系统加载为只读模式                 |
| -a   | 加载文件“/etc/fstab”中描述的所有文件系统 |

**参考实例**﻿

```bash
umount -f [挂载位置] 强制卸载
```

### 2、进程管理

#### （1）基本概念

- 在Linux中，每一个程序都是有自己的一个进程，每一个进程都有一个id号！ 
- 每一个进程呢，都会有一个父进程！ 
- 进程可以有两种存在方式：前台！后台运行！ 
- 服务都是后台运行的，基本的程序都是前台运行的！ 

#### （2）ps(显示进程状态)

- ps命令是“process status”的缩写，ps命令用于显示当前系统的进程状态。可以搭配kill指令随时中断、删除不必要的程序。
- ps命令是最基本同时也是非常强大的进程查看命令，使用该命令可以确定有哪些进程正在运行和运行的状态、进程是否结束、进程有没有僵死、哪些进程占用了过多的资源等等。

**语法格式：**ps [参数]

**常用参数：**﻿

| a                   | 显示现行终端机下的所有程序，包括其他用户的程序               |
| ------------------- | ------------------------------------------------------------ |
| -A                  | 显示所有程序                                                 |
| c                   | 显示每个程序真正的指令名称，而不包含路径                     |
| -C <指令名称>       | 指定执行指令的名称，并列出该指令的程序的状况                 |
| -d                  | 显示所有程序，但不包括阶段作业管理员的程序                   |
| e                   | 列出程序时，显示每个程序所使用的环境变量                     |
| -f                  | 显示UID,PPIP,C与STIME栏位                                    |
| f                   | 用ASCII字符显示树状结构，表达程序间的相互关系                |
| g                   | 显示现行终端机下的所有程序，包括所属组的程序                 |
| -G <群组识别码>     | 列出属于该群组的程序的状况                                   |
| h                   | 不显示标题列                                                 |
| -H                  | 显示树状结构，表示程序间的相互关系                           |
| -j                  | 采用工作控制的格式显示程序状况                               |
| -l                  | 采用详细的格式来显示程序状况                                 |
| L                   | 列出栏位的相关信息                                           |
| -m                  | 显示所有的执行绪                                             |
| n                   | 以数字来表示USER和WCHAN栏位                                  |
| -N                  | 显示所有的程序，除了执行ps指令终端机下的程序之外             |
| -p <程序识别码>     | 指定程序识别码，并列出该程序的状况                           |
| r                   | 只列出现行终端机正在执行中的程序                             |
| -s <阶段作业>       | 列出隶属该阶段作业的程序的状况                               |
| s                   | 采用程序信号的格式显示程序状况                               |
| S                   | 列出程序时，包括已中断的子程序资料                           |
| -t <终端机编号>     | 列出属于该终端机的程序的状况                                 |
| -T                  | 显示现行终端机下的所有程序                                   |
| u                   | 以用户为主的格式来显示程序状况                               |
| -U <用户识别码>     | 列出属于该用户的程序的状况                                   |
| U <用户名称>        | 列出属于该用户的程序的状况                                   |
| v                   | 采用虚拟内存的格式显示程序状况                               |
| -V或V               | 显示版本信息                                                 |
| -w或w               | 采用宽阔的格式来显示程序状况                                 |
| x                   | 显示所有程序，不以终端机来区分                               |
| X                   | 采用旧式的Linux i386登陆格式显示程序状况                     |
| -y                  | 配合选项”-l”使用时，不显示F(flag)栏位，并以RSS栏位取代ADDR栏位 |
| --cols <每列字符数> | 设置每列的最大字符数                                         |
| --headers           | 重复显示标题列                                               |
| --help              | 在线帮助                                                     |
| --info              | 显示排错信息                                                 |
| --lines <显示列数>  | 设置显示画面的列数                                           |

**参考实例**

```bash
#显示所有当前进程
ps -ef 
# 显示系统中全部的进程信息，含详细信息
ps aux 
# 结合管道操作符，将当前系统运行状态中指定进程信息过滤出来
ps -ef | grep ssh
# 结合输出重定向，将当前进程信息保留备份至指定文件
ps aux > backup.txt
#显示所有当前进程
ps -ax 
#根据用户过滤进程
ps -u pungki 
#根据 CPU 使用来升序排序
ps -aux --sort -pcpu | less 
#根据用户过滤进程
ps -aux --sort -pmem | less 
#查询全前10个使用cpu和内存最高的应用
ps -aux --sort -pcpu,+pmem | head -n 10 
#通过进程名和PID过滤
ps -C getty 
#带格式显示的，通过进程名和PID过滤
ps -f -C getty 
#根据线程来过滤进程
ps -L 1213 
#树形显示进程
ps -axjf（或pstree）
# 显示安全信息
ps -eo pid,user,args 
#格式化输出 root 用户（真实的或有效的UID）创建的进程
ps -U root -u root u 
```

#### （3）top(实时显示系统运行状态)

top命令的功能是用于实时显示系统运行状态，包含处理器、内存、服务、进程等重要资源信息。运维工程师们常常会把top命令比作是“加强版的Windows任务管理器”，因为除了能看到常规的服务进程信息以外，还能够对处理器和内存的负载情况一目了然，实时感知系统全局的运行状态，非常适合作为接手服务器后执行的第一条命令。

**语法格式：**top [参数]

**常用参数：**

| -d <秒> | 改变显示的更新速度                   |
| ------- | ------------------------------------ |
| -c      | 切换显示模式                         |
| -s      | 安全模式，不允许交互式指令           |
| -i      | 不显示任何闲置或僵死的行程           |
| -n      | 设定显示的总次数，完成后将会自动退出 |
| -b      | 批处理模式，不进行交互式显示         |

**参考实例**

```bash
#显示进程信息
top
#显示完整的进程信息
top -c
#以批处理模式显示程序信息
top -b
#以累积模式显示程序信息
top -s
#设置信息更新次数
top -n 2
```

#### （4）kill(杀死进程)

- kill命令的功能是用于杀死（结束）进程，与英文单词的含义相同。Linux系统中如需结束某个进程，既可以使用如service或systemctl的管理命令来结束服务，也可以使用kill命令直接结束进程信息。
- kill命令可将指定的信号发送给相应的进程或工作。 kill命令默认使用信号为15，用于结束进程或工作。如果进程或工作忽略此信号，则可以使用信号9，强制杀死进程或作业。

**语法格式：**kill [参数] 进程号

**常用参数：**

| -l   | 列出系统支持的信号             |
| ---- | ------------------------------ |
| -s   | 指定向进程发送的信号           |
| -a   | 不限制命令名和进程号的对应关系 |
| -p   | 不发送任何信号                 |

**参考实例**

```bash
# 列出系统支持的全部信号列表
[root@bluecusliyou testdir]# kill -l
 1) SIGHUP       2) SIGINT       3) SIGQUIT      4) SIGILL       5) SIGTRAP
 6) SIGABRT      7) SIGBUS       8) SIGFPE       9) SIGKILL     10) SIGUSR1
11) SIGSEGV     12) SIGUSR2     13) SIGPIPE     14) SIGALRM     15) SIGTERM
16) SIGSTKFLT   17) SIGCHLD     18) SIGCONT     19) SIGSTOP     20) SIGTSTP
21) SIGTTIN     22) SIGTTOU     23) SIGURG      24) SIGXCPU     25) SIGXFSZ
26) SIGVTALRM   27) SIGPROF     28) SIGWINCH    29) SIGIO       30) SIGPWR
31) SIGSYS      34) SIGRTMIN    35) SIGRTMIN+1  36) SIGRTMIN+2  37) SIGRTMIN+3
38) SIGRTMIN+4  39) SIGRTMIN+5  40) SIGRTMIN+6  41) SIGRTMIN+7  42) SIGRTMIN+8
43) SIGRTMIN+9  44) SIGRTMIN+10 45) SIGRTMIN+11 46) SIGRTMIN+12 47) SIGRTMIN+13
48) SIGRTMIN+14 49) SIGRTMIN+15 50) SIGRTMAX-14 51) SIGRTMAX-13 52) SIGRTMAX-12
53) SIGRTMAX-11 54) SIGRTMAX-10 55) SIGRTMAX-9  56) SIGRTMAX-8  57) SIGRTMAX-7
58) SIGRTMAX-6  59) SIGRTMAX-5  60) SIGRTMAX-4  61) SIGRTMAX-3  62) SIGRTMAX-2
63) SIGRTMAX-1  64) SIGRTMAX    
# 结束某个指定的进程（数字为对应的PID值）
[root@bluecusliyou testdir]# kill 1518
# 强制结束某个指定的进程（数字为对应的PID值）
[root@bluecusliyou testdir]# kill -9 1518
```

### 3、服务管理

在linux中，service和systemctl都是用来进行服务管理，在centos7.x版本以后，推荐使用systemctl来处理，比如同样是查看防火墙状态，二者的指令如下：【service firewalld status】【systemctl status firewalld】。

#### （1）systemctl(管理系统服务)

systemctl命令来自于英文词组”system control“的缩写，其功能是用于管理系统服务。从RHEL/CentOS7版本之后初始化进程服务init被替代成了systemd服务，systemd初始化进程服务的管理是通过systemctl命令完成的，从功能上涵盖了之前service、chkconfig、init、setup等多条命令的大部分功能。

**语法格式**：systemctl 参数 服务

**常用参数：**

| start                      | 启动服务           |
| -------------------------- | ------------------ |
| stop                       | 停止服务           |
| restart                    | 重启服务           |
| enable                     | 使某服务开机自启   |
| disable                    | 关闭某服务开机自启 |
| status                     | 查看服务状态       |
| list -units --type=service | 列举所有已启动服务 |

**参考实例**

```bash
# 启动httpd服务
systemctl start httpd.service
# 停止httpd服务
systemctl stop httpd.service
# 重启httpd服务
systemctl restart httpd.service
# 查看httpd服务状态
systemctl status httpd.service
# 使httpd开机自启
systemctl enable httpd.service
# 取消httpd开机自启
systemctl disable httpd.service
# 列举所有已启动服务(unit单元)
systemctl list-units --type=service
```

#### （2）service(控制系统服务)

service命令是Redhat Linux兼容的发行版中用来控制系统服务的实用工具，它以启动、停止、重新启动和关闭系统服务，还可以显示所有系统服务的当前状态。

**语法格式**：service [参数]

**常用参数**：

| --status-all | 显示所服务的状态 |
| ------------ | ---------------- |
| -h           | 显示帮助信息     |

**参考实例**

```bash
# 启动httpd服务
service httpd.service start 
# 停止httpd服务
service httpd.service stop
# 重启httpd服务
service httpd.service restart
# 查看httpd服务状态
service httpd.service status
# 显示所有服务的状态
service --status-all
```

### 4、软件安装与更新

#### （1）rpm(RPM软件包管理器)

- rpm命令是Red-Hat Package Manager（RPM软件包管理器）的缩写， 该命令用于管理Linux 下软件包的软件。在 Linux 操作系统下，几乎所有的软件均可以通过RPM 进行安装、卸载及管理等操作。

- 概括的说，rpm命令包含了五种基本功能：安装、卸载、升级、查询和验证。

**语法格式：**rpm [参数] 软件包

**常用参数：**

| -a     | 查询所有的软件包                                 |
| ------ | ------------------------------------------------ |
| -b或-t | 设置包装套件的完成阶段，并指定套件档的文件名称； |
| -c     | 只列出组态配置文件，本参数需配合”-l”参数使用     |
| -d     | 只列出文本文件，本参数需配合”-l”参数使用         |
| -e     | 卸载软件包                                       |
| -f     | 查询文件或命令属于哪个软件包                     |
| -h     | 安装软件包时列出标记                             |
| -i     | 安装软件包                                       |
| -l     | 显示软件包的文件列表                             |
| -p     | 查询指定的rpm软件包                              |
| -q     | 查询软件包                                       |
| -R     | 显示软件包的依赖关系                             |
| -s     | 显示文件状态，本参数需配合”-l”参数使用           |
| -U     | 升级软件包                                       |
| -v     | 显示命令执行过程                                 |
| -vv    | 详细显示指令执行过程                             |

**参考实例**

```bash
# 直接安装软件包
rpm -ivh packge.rpm
# 忽略报错，强制安装
rpm --force -ivh package.rpm
# 列出所有安装过的包
rpm -qa
# 查询rpm包中的文件安装的位置
rpm -ql ls
# 卸载rpm包
rpm -e package.rpm
# 升级软件包
rpm -U file.rpm
```

#### （2）yum(基于RPM的软件包管理器)

- yum命令来自于英文词组”YellowdogUpdater,Modified“的缩写，其功能是用于在Linux系统中基于RPM技术进行软件包的管理工作。yum技术通用于RHEL、CentOS、Fedora、OpenSUSE等主流系统，可以让系统管理人员交互式的自动化更新和管理软件包，实现从指定服务器自动下载、更新、删除软件包的工作。

  yum软件仓库及命令能够自动处理软件依赖关系，一次性安装所需全部软件，无需繁琐的操作。

**语法格式：**yum [参数] 软件包

**常用参数：**

| -h           | 显示帮助信息                                   |
| ------------ | ---------------------------------------------- |
| -y           | 对所有的提问都回答“yes”                        |
| -c           | 指定配置文件                                   |
| -q           | 安静模式                                       |
| -v           | 详细模式                                       |
| -t           | 检查外部错误                                   |
| -d           | 设置调试等级（0-10）                           |
| -e           | 设置错误等级（0-10）                           |
| -R           | 设置yum处理一个命令的最大等待时间              |
| -C           | 完全从缓存中运行，而不去下载或者更新任何头文件 |
| install      | 安装rpm软件包                                  |
| update       | 更新rpm软件包                                  |
| check-update | 检查是否有可用的更新rpm软件包                  |
| remove       | 删除指定的rpm软件包                            |
| list         | 显示软件包的信息                               |
| search       | 检查软件包的信息                               |
| info         | 显示指定的rpm软件包的描述信息和概要信息        |
| clean        | 清理yum过期的缓存                              |
| shell        | 进入yum的shell提示符                           |
| resolvedep   | 显示rpm软件包的依赖关系                        |
| localinstall | 安装本地的rpm软件包                            |
| localupdate  | 显示本地rpm软件包进行更新                      |
| deplist      | 显示rpm软件包的所有依赖关系                    |

**参考实例**

```bash
# 清理原有的软件仓库信息缓存
yum clean all
# 建立最新的软件仓库信息缓存
yum makecache
# 安装指定的服务及相关软件包
yum install httpd
# 更新指定的服务及相关软件包
yum update httpd
# 卸载指定的服务及相关软件包
yum remove httpd
# 显示可安装的软件包组列表
yum grouplist
# 显示指定服务的软件信息
yum info httpd
```

#### （3）源码编译安装

　软件以源码工程的形式发布，需要获取到源码工程后用相应开发工具进行编译打包部署。

#### （4）二进制发布包

　软件已经针对具体平台编译打包发布，只要解压，修改配置即可。

### 5、网络配置

#### （1）hostname(显示和设置系统的主机名)

hostname命令的功能是用于显示和设置系统的主机名，Linux系统中的HOSTNAME环境变量对应保存了当前的主机名称，使用hostname命令能够查看和设置此环境变量的值，而要想永久修改主机名称则需要使用hostnamectl命令或直接编辑配置文件/etc/hostname才行。

**语法格式：**hostname [参数]

**常用参数：**

| -a   | 显示主机别名     |
| ---- | ---------------- |
| -d   | 显示DNS域名      |
| -f   | 显示FQDN名称     |
| -i   | 显示主机的ip地址 |
| -s   | 显示短主机名称   |
| -y   | 显示NIS域名      |

**参考实例**

```bash
# 显示主机名
hostname
# 使用-a参数显示主机别名
hostname -a
# 修改主机名(重启后无效)
hostname xxx
# 永久修改(重启生效)
hostnamectl set-hostname xxx
```

#### （2）ifconfig(显示或设置网络设备参数信息)

ifconfig命令来自于英文词组”network interfaces configuring“的缩写，其功能是用于显示或设置网络设备参数信息。在Windows系统中与之类似的命令叫做ipconfig，同样的功能可以使用ifconfig去完成。

通常不建议使用ifconfig命令配置网络设备的参数信息，因为一旦服务器重启，配置过的参数会自动失效，还是编写到配置文件中更稳妥。

永久修改ip：修改 /etc/sysconfig/network-scripts/ifcfg-eth0 文件 (注：不一定是ifcfg-eth0，要通过ifconfig命令查看 )

**语法格式：**ifconfig [参数] [网卡设备]

**常用参数：**

| add<地址> | 设置网络设备IPv6的IP地址 |
| --------- | ------------------------ |
| del<地址> | 删除网络设备IPv6的IP地址 |
| down      | 关闭指定的网络设备       |
| up        | 启动指定的网络设备       |
| IP地址    | 指定网络设备的IP地址     |

**参考实例**

```bash
# 查看IP地址
ifconfig
# 临时修改ip地址(重启失效) eth0看具体是哪个网卡
ifconfig eth0 192.168.12.22
# 启动关闭指定网卡
ifconfig eth0 down
ifconfig eth0 up
# 为网卡配置和删除IPv6地址
ifconfig eth0 add 33ffe:3240:800:1005::2/64
ifconfig eth0 del 33ffe:3240:800:1005::2/64
# 修改MAC地址
ifconfig eth0 hw ether 00:AA:BB:CC:DD:EE
# ARP协议关闭和开启
ifconfig eth0 -arp
ifconfig eth0 arp
# 配置IP地址
ifconfig eth0 192.168.1.56 
ifconfig eth0 192.168.1.56 netmask 255.255.255.0
ifconfig eth0 192.168.1.56 netmask 255.255.255.0 broadcast 192.168.1.255
```

#### （3）netstat(显示网络状态)

netstat命令来自于英文词组”network statistics“的缩写，其功能是用于显示各种网络相关信息，例如网络连接状态、路由表信息、接口状态、NAT、多播成员等等。

netstat命令不仅应用于Linux系统，而且在Windows XP、Windows 7、Windows 10及Windows 11中均已默认支持，并且可用参数也相同，有经验的运维人员可以直接上手。

**语法格式：**netstat [参数]

**常用参数：**

| -a   | 显示所有连线中的Socket                   |
| ---- | ---------------------------------------- |
| -p   | 显示正在使用Socket的程序识别码和程序名称 |
| -l   | 仅列出在监听的服务状态                   |
| -t   | 显示TCP传输协议的连线状况                |
| -u   | 显示UDP传输协议的连线状况                |
| -i   | 显示网络界面信息表单                     |
| -r   | 显示路由表信息                           |
| -n   | 直接使用IP地址，不通过域名服务器         |

**参考实例**

> 列出所有端口 (包括监听和未监听的)

```bash
## 列出所有端口 
netstat -a
## 列出所有 tcp 端口 
netstat -at
##列出所有 udp 端口 
netstat -au 
```

> 列出所有处于监听状态的 Sockets

```bash
### 只显示监听端口 
netstat -l
### 只列出所有监听 tcp 端口
netstat -lt
### 只列出所有监听 udp 端口 
netstat -lu
### 只列出所有监听 UNIX 端口 
netstat -lx
```

> 找出程序运行的端口

```bash
netstat -ap | grep ssh
```

> 找出端口占用情况

```bash
[root@worker3 ~]# netstat -tunlp |grep 80
tcp        0      0 0.0.0.0:80              0.0.0.0:*               LISTEN      1759906/docker-prox 
tcp6       0      0 :::80                   :::*                    LISTEN      1759910/docker-prox 
```

#### （4）域名映射

　/etc/hosts文件用于在通过主机名进行访问时做ip地址解析之用,相当于windows系统的C:\Windows\System32\drivers\etc\hosts文件的功能。

#### （5）ip addr(查看IP地址)

```bash
[root@bluecusliyou ~]# ip addr
1: lo: <LOOPBACK,UP,LOWER_UP> mtu 65536 qdisc noqueue state UNKNOWN group default qlen 1000
    link/loopback 00:00:00:00:00:00 brd 00:00:00:00:00:00
    inet 127.0.0.1/8 scope host lo
       valid_lft forever preferred_lft forever
    inet6 ::1/128 scope host 
       valid_lft forever preferred_lft forever
2: eth0: <BROADCAST,MULTICAST,UP,LOWER_UP> mtu 1500 qdisc fq_codel state UP group default qlen 1000
    link/ether 00:16:3e:16:fa:95 brd ff:ff:ff:ff:ff:ff
    inet 172.27.45.106/20 brd 172.27.47.255 scope global dynamic noprefixroute eth0
       valid_lft 312184993sec preferred_lft 312184993sec
    inet6 fe80::216:3eff:fe16:fa95/64 scope link 
       valid_lft forever preferred_lft forever
3: docker0: <BROADCAST,MULTICAST,UP,LOWER_UP> mtu 1500 qdisc noqueue state UP group default 
    link/ether 02:42:3f:30:cc:94 brd ff:ff:ff:ff:ff:ff
    inet 172.17.0.1/16 brd 172.17.255.255 scope global docker0
       valid_lft forever preferred_lft forever
    inet6 fe80::42:3fff:fe30:cc94/64 scope link 
       valid_lft forever preferred_lft forever
89: vethf0fc6b7@if88: <BROADCAST,MULTICAST,UP,LOWER_UP> mtu 1500 qdisc noqueue master docker0 state UP group default 
    link/ether 02:31:61:b2:ab:a9 brd ff:ff:ff:ff:ff:ff link-netnsid 0
    inet6 fe80::31:61ff:feb2:aba9/64 scope link 
       valid_lft forever preferred_lft forever
91: veth3f74a7c@if90: <BROADCAST,MULTICAST,UP,LOWER_UP> mtu 1500 qdisc noqueue master docker0 state UP group default 
    link/ether b6:07:90:98:e7:2b brd ff:ff:ff:ff:ff:ff link-netnsid 1
    inet6 fe80::b407:90ff:fe98:e72b/64 scope link 
       valid_lft forever preferred_lft forever
```

#### （6）ping(测试主机间网络连通性)

ping命令的功能是用于测试主机间网络连通性，发送出基于ICMP传输协议的数据包，要求对方主机予以回复，若对方主机的网络功能没有问题且防火墙放行流量，则就会回复该信息，我们也就可得知对方主机系统在线并运行正常了。

不过值得我们注意的是Linux与Windows相比有一定差异，Windows系统下的ping命令会发送出去4个请求后自动结束该命令；而Linux系统则不会自动终止，需要用户手动按下组合键“Ctrl+c”才能结束，或是发起命令时加入-c参数限定发送个数。

**语法格式：**ping [参数] 目标主机

**常用参数：**

| -d   | 使用Socket的SO_DEBUG功能                 |
| ---- | ---------------------------------------- |
| -c   | 指定发送报文的次数                       |
| -i   | 指定收发信息的间隔时间                   |
| -I   | 使用指定的网络接口送出数据包             |
| -l   | 设置在送出要求信息之前，先行发出的数据包 |
| -n   | 只输出数值                               |
| -p   | 设置填满数据包的范本样式                 |
| -q   | 不显示指令执行过程                       |
| -R   | 记录路由过程                             |
| -s   | 设置数据包的大小                         |
| -t   | 设置存活数值TTL的大小                    |
| -v   | 详细显示指令的执行过程                   |

**参考实例**

```bash
#检测网站的连通性
[root@bluecusliyou ~]# ping www.baidu.com
PING www.a.shifen.com (110.242.68.3) 56(84) bytes of data.
64 bytes from 110.242.68.3 (110.242.68.3): icmp_seq=1 ttl=50 time=10.7 ms
64 bytes from 110.242.68.3 (110.242.68.3): icmp_seq=2 ttl=50 time=10.7 ms
64 bytes from 110.242.68.3 (110.242.68.3): icmp_seq=3 ttl=50 time=10.7 ms
64 bytes from 110.242.68.3 (110.242.68.3): icmp_seq=4 ttl=50 time=10.7 ms
^C
--- www.a.shifen.com ping statistics ---
4 packets transmitted, 4 received, 0% packet loss, time 7ms
rtt min/avg/max/mdev = 10.695/10.717/10.743/0.076 ms
#ping4次
[root@bluecusliyou ~]# ping -c 4 www.baidu.com
PING www.a.shifen.com (110.242.68.4) 56(84) bytes of data.
64 bytes from 110.242.68.4 (110.242.68.4): icmp_seq=1 ttl=50 time=11.6 ms
64 bytes from 110.242.68.4 (110.242.68.4): icmp_seq=2 ttl=50 time=11.7 ms
64 bytes from 110.242.68.4 (110.242.68.4): icmp_seq=3 ttl=50 time=11.7 ms
64 bytes from 110.242.68.4 (110.242.68.4): icmp_seq=4 ttl=50 time=11.7 ms

--- www.a.shifen.com ping statistics ---
4 packets transmitted, 4 received, 0% packet loss, time 7ms
rtt min/avg/max/mdev = 11.636/11.660/11.685/0.133 ms
#ping4次间隔3秒
[root@bluecusliyou ~]# ping -c 4 -i 3 www.baidu.com
PING www.a.shifen.com (110.242.68.3) 56(84) bytes of data.
64 bytes from 110.242.68.3 (110.242.68.3): icmp_seq=1 ttl=50 time=10.7 ms
64 bytes from 110.242.68.3 (110.242.68.3): icmp_seq=2 ttl=50 time=10.7 ms
64 bytes from 110.242.68.3 (110.242.68.3): icmp_seq=3 ttl=50 time=10.7 ms
64 bytes from 110.242.68.3 (110.242.68.3): icmp_seq=4 ttl=50 time=10.8 ms

--- www.a.shifen.com ping statistics ---
4 packets transmitted, 4 received, 0% packet loss, time 17ms
rtt min/avg/max/mdev = 10.710/10.737/10.751/0.104 ms
#指定网站的IP
[root@bluecusliyou ~]# ping -c 1 www.baidu.com | grep from | cut -d " " -f 4
110.242.68.4
```

#### （7）wget(下载网络文件)

wget命令来自于英文词组”web get“的缩写，其功能是用于从指定网址下载网络文件。wget命令非常稳定，一般即便网络波动也不会导致下载失败，而是不断的尝试重连，直至整个文件下载完毕。

wget命令支持如HTTP、HTTPS、FTP等常见协议，可以在命令行中直接下载网络文件。

**语法格式：** wget [参数] 网址

**常用参数：**

| -V                  | 显示版本信息       |
| ------------------- | ------------------ |
| -h                  | 显示帮助信息       |
| -b                  | 启动后转入后台执行 |
| -c                  | 支持断点续传       |
| -O                  | 定义本地文件名     |
| -e <命令>           | 执行指定的命令     |
| --limit-rate=<速率> | 限制下载速度       |

**参考实例**

```bash
# 下载指定的网络文件
[root@bluecusliyou testdir]# wget https://www.linuxprobe.com/docs/LinuxProbe.pdf
# 下载指定的网络文件，并定义保存在本地的文件名称
[root@bluecusliyou testdir]# wget -O Book.pdf https://www.linuxprobe.com/docs/LinuxProbe.pdf
# 下载指定的网络文件，限速最高每秒300k
[root@bluecusliyou testdir]# wget --limit-rate=300k https://www.linuxprobe.com/docs/LinuxProbe.pdf
# 启用断点续传技术下载指定的网络文件
[root@bluecusliyou testdir]# wget -c https://www.linuxprobe.com/docs/LinuxProbe.pdf
# 下载指定的网络文件，将任务放至后台执行
[root@bluecusliyou testdir]# wget -b https://www.linuxprobe.com/docs/LinuxProbe.pdf
```

### 6、防火墙命令

firewall-cmd提供了一个动态管理的防火墙，支持网络/防火墙区域来定义网络连接或接口的信任级别。它支持IPv4、IPv6防火墙设置和以太网网桥，并将运行时和永久配置选项分开。它还支持服务或应用程序直接添加防火墙规则的接口。

#### （1）安装Firewall命令

```bash
yum install firewalld firewalld-config
```

#### （2）Firewall开启常见端口命令

```bash
firewall-cmd –zone=public –add-port=80/tcp –permanent
firewall-cmd –zone=public –add-port=443/tcp –permanent
firewall-cmd –zone=public –add-port=22/tcp –permanent
firewall-cmd –zone=public –add-port=21/tcp –permanent
firewall-cmd –zone=public –add-port=53/udp –permanent
```

#### （3）Firewall关闭常见端口命令

```bash
firewall-cmd –zone=public –remove-port=80/tcp –permanent
firewall-cmd –zone=public –remove-port=443/tcp –permanent
firewall-cmd –zone=public –remove-port=22/tcp –permanent
firewall-cmd –zone=public –remove-port=21/tcp –permanent
firewall-cmd –zone=public –remove-port=53/udp –permanent
```

#### （4）批量添加区间端口

```bash
firewall-cmd –zone=public –add-port=4400-4600/udp –permanent
firewall-cmd –zone=public –add-port=4400-4600/tcp –permanent
```

#### （5）重启防火墙命令

```bash
firewall-cmd –reload 或者 service firewalld restart
```

#### （6）查看端口列表

```bash
firewall-cmd –permanent –list-port
```

## 八、curl详解

参考阮一峰老师文章：

[https://www.ruanyifeng.com/blog/2011/09/curl.html](https://www.ruanyifeng.com/blog/2011/09/curl.html)

[http://www.ruanyifeng.com/blog/2019/09/curl-reference.html](http://www.ruanyifeng.com/blog/2019/09/curl-reference.html)

curl命令来自于英文词组”CommandLine URL“的缩写，其功能是用于在Shell终端界面中基于URL规则进行的文件传输工作。curl是一款综合的传输工具，可以上传也可以下载，支持HTTP、HTTPS、FTP等三十余种常见协议，完全可以取代 Postman 这一类的图形界面工具。

**语法格式：**curl [参数] 网址

**常用参数：**

| -o   | 指定新的本地文件名                   |
| ---- | ------------------------------------ |
| -O   | 保留远程文件的原始名                 |
| -u   | 通过服务端配置的用户名和密码授权访问 |
| -I   | 打印HTTP响应头信息                   |
| -u   | 指定登录账户密码信息                 |
| -A   | 设置用户代理标头信息                 |
| -b   | 设置用户cookie信息                   |
| -C   | 支持断点续传                         |
| -s   | 静默模式，不输出任何信息             |
| -T   | 上传文件                             |

### 1、无参数(查看网页源码)

直接在curl命令后加上网址，就可以看到网页源码。

```bash
[root@bluecusliyou ~]# curl www.baidu.com
<!DOCTYPE html>
<!--STATUS OK--><html> <head><meta http-equiv=content-type content=text/html;charset=utf-8><meta http-equiv=X-UA-Compatible content=IE=Edge><meta content=always name=referrer><link rel=stylesheet type=text/css href=http://s1.bdstatic.com/r/www/cache/bdorz/baidu.min.css><title>百度一下，你就知道</title></head>
...
```

### 2、**-o**(将服务器的回应保存成文件)

```bash
# 将`www.example.com`保存成`example.html`
curl -o example.html https://www.example.com
```

### 3、**-O**(保存成文件用URL最后部分做文件名)

```bash
# 将服务器回应保存成文件，文件名为`bar.html`
curl -O https://www.example.com/foo/bar.html
```

### 4、-i(显示头信息，连同网页代码一起)

`-i`参数可以显示http response的头信息，连同网页代码一起。收到服务器回应后，先输出服务器回应的标头，然后空一行，再输出网页的源码。

```bash
[root@bluecusliyou ~]# curl -i www.baidu.com
HTTP/1.1 200 OK
Accept-Ranges: bytes
Cache-Control: private, no-cache, no-store, proxy-revalidate, no-transform
Connection: keep-alive
Content-Length: 2381
Content-Type: text/html
Date: Sun, 31 Jul 2022 08:44:06 GMT
Etag: "588604c1-94d"
Last-Modified: Mon, 23 Jan 2017 13:27:29 GMT
Pragma: no-cache
Server: bfe/1.0.8.18
Set-Cookie: BDORZ=27315; max-age=86400; domain=.baidu.com; path=/

<!DOCTYPE html>
<!--STATUS OK--><html> <head><meta http-equiv=content-type content=text/html;charset=utf-8><meta http-equiv=X-UA-Compatible content=IE=Edge><meta content=always name=referrer><link rel=stylesheet type=text/css href=http://s1.bdstatic.com/r/www/cache/bdorz/baidu.min.css><title>百度一下，你就知道</title></head>
...
```

### 5、-I(只显示头信息)(--head)

```bash
[root@bluecusliyou ~]# curl -I www.baidu.com
HTTP/1.1 200 OK
Accept-Ranges: bytes
Cache-Control: private, no-cache, no-store, proxy-revalidate, no-transform
Connection: keep-alive
Content-Length: 277
Content-Type: text/html
Date: Sun, 31 Jul 2022 23:27:26 GMT
Etag: "575e1f59-115"
Last-Modified: Mon, 13 Jun 2016 02:50:01 GMT
Pragma: no-cache
Server: bfe/1.0.8.18
```

### 6、-v(显示通信过程)(--trace)

`-v`参数可以显示一次http通信的整个过程，包括端口连接和http request头信息。

```bash
[root@bluecusliyou ~]# curl -v www.baidu.com
* About to connect() to www.baidu.com port 80 (#0)
*   Trying 110.242.68.3...
* Connected to www.baidu.com (110.242.68.3) port 80 (#0)
> GET / HTTP/1.1
> User-Agent: curl/7.29.0
> Host: www.baidu.com
> Accept: */*
> 
< HTTP/1.1 200 OK
< Accept-Ranges: bytes
< Cache-Control: private, no-cache, no-store, proxy-revalidate, no-transform
< Connection: keep-alive
< Content-Length: 2381
< Content-Type: text/html
< Date: Sun, 31 Jul 2022 08:45:41 GMT
< Etag: "588604c1-94d"
< Last-Modified: Mon, 23 Jan 2017 13:27:29 GMT
< Pragma: no-cache
< Server: bfe/1.0.8.18
< Set-Cookie: BDORZ=27315; max-age=86400; domain=.baidu.com; path=/
< 
<!DOCTYPE html>
<!--STATUS OK--><html> <head><meta http-equiv=content-type content=text/html;charset=utf-8><meta http-equiv=X-UA-Compatible content=IE=Edge><meta content=always name=referrer><link rel=stylesheet type=text/css href=http://s1.bdstatic.com/r/www/cache/bdorz/baidu.min.css><title>百度一下，你就知道</title></head>
...
* Connection #0 to host www.baidu.com left intact
```

`--trace`参数也可以用于调试，还会输出原始的二进制数据。

```bash
# 运行后，请打开output.txt文件查看
curl --trace output.txt www.baidu.com
# 或者
curl --trace-ascii output.txt www.baidu.com
```

### 7、-X(指定方法类型)

#### （1）get方法

GET方法相对简单，只要把数据附在网址后面就行。

```bash
curl example.com/form.cgi?data=xxx
```

#### （2）post方法

POST方法必须把数据和网址分开，curl就要用到--data参数。

```bash
curl -X POST --data "data=xxx" example.com/form.cgi
```

如果你的数据没有经过表单编码，还可以让curl为你编码，参数是`--data-urlencode`。

```bash
curl -X POST--data-urlencode "date=April 1" example.com/form.cgi
```

### 8、-x(指定 HTTP 请求的代理)

```bash
# 指定 HTTP 请求通过myproxy.com:8080的 socks5 代理发出
curl -x socks5://james:cats@myproxy.com:8080 https://www.example.com
```

如果没有指定代理协议，默认为 HTTP。

```bash
# 请求的代理使用 HTTP 协议
curl -x james:cats@myproxy.com:8080 https://www.example.com
```

### 9、-d(用于发送 POST 请求的数据体)(--data-urlencode)

使用`-d`参数以后，HTTP 请求会自动加上标头`Content-Type : application/x-www-form-urlencoded`。并且会自动将请求转为 POST 方法，因此可以省略`-X POST`。

```bash
curl -d 'login=emma＆password=123'-X POST https://google.com/login
# 或者
curl -d 'login=emma' -d 'password=123' -X POST  https://google.com/login
```

`-d`参数可以读取本地文本文件的数据，向服务器发送。

```bash
# 读取`data.txt`文件的内容，作为数据体向服务器发送
curl -d '@data.txt' https://google.com/login
```

`--data-urlencode`参数等同于`-d`，发送 POST 请求的数据体，区别在于会自动将发送的数据进行 URL 编码。

```bash
# 发送的数据`hello world`之间有一个空格，需要进行 URL 编码
curl --data-urlencode 'comment=hello world' https://google.com/login
```

### 10、-H(添加 HTTP 请求的标头)(--header)

```bash
# 添加 HTTP 标头`Accept-Language: en-US`。
curl -H 'Accept-Language: en-US' https://google.com
# 添加两个 HTTP 标头
curl -H 'Accept-Language: en-US' -H 'Secret-Message: xyzzy' https://google.com
# 添加 HTTP 请求的标头是`Content-Type: application/json`，然后用`-d`参数发送 JSON 数据
curl -d '{"login": "emma", "pass": "123"}' -H 'Content-Type: application/json' https://google.com/login
```

### 11、--form(提交表单)

假定文件上传的表单是下面这样：

```html
<form method="POST" enctype='multipart/form-data' action="upload.cgi">
　　　<input type=file name=upload>
　　　<input type=submit name=press value="OK">
</form>
```

你可以用curl这样上传文件：

```bash
curl --form upload=@localfilename --form press=OK [URL]
```

### 12、-F(向服务器上传二进制文件)

`-F`参数用来向服务器上传二进制文件。

```bash
# 命令会给 HTTP 请求加上标头`Content-Type: multipart/form-data`，然后将文件`photo.png`作为`file`字段上传。
curl -F 'file=@photo.png' https://google.com/profile
```

`-F`参数可以指定 MIME 类型。

```bash
# 指定 MIME 类型为image/png，否则 curl 会把 MIME 类型设为application/octet-stream
curl -F 'file=@photo.png;type=image/png' https://google.com/profile
```

`-F`参数也可以指定文件名。

```bash
# 原始文件名为photo.png，但是服务器接收到的文件名为me.png
curl -F 'file=@photo.png;filename=me.png' https://google.com/profile
```

### 13、-e(表示请求的来源)(--referer)

```bash
# 上面命令将`Referer`标头设为`https://google.com?q=example`。
curl -e 'https://google.com?q=example' https://www.example.com
```

`-H`参数可以通过直接添加标头`Referer`，达到同样效果。

```bash
curl -H 'Referer: https://google.com?q=example' https://www.example.com
```

### 14、-A(向服务器发送客户端的设备信息)(--user-agent)

`-A`参数指定客户端的用户代理标头，即`User-Agent`。这个字段是用来表示客户端的设备信息。服务器有时会根据这个字段，针对不同设备，返回不同格式的网页，比如手机版和桌面版。

iPhone4的User Agent是

```html
Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_0 like Mac OS X; en-us) AppleWebKit/532.9 (KHTML, like Gecko) Version/4.0.5 Mobile/8A293 Safari/6531.22.7
```

curl可以这样模拟：

```bash
curl -A "[User Agent]" [URL]
```

```bash
# 命令将User-Agent改成 Chrome 浏览器
curl -A 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36' https://google.com
# 命令会移除User-Agent标头
curl -A '' https://google.com
```

### 15、-b(向服务器发送cookie)(--cookie)

```bash
# 生成一个标头`Cookie: foo=bar`，向服务器发送一个名为`foo`、值为`bar`的 Cookie
curl -b 'foo=bar' https://google.com
# 发送两个 Cookie
curl -b 'foo1=bar;foo2=bar2' https://google.com
# 保存服务器返回的cookie到文件
curl -c cookies.txt http://example.com
# 读取本地文件`cookies.txt`，里面是服务器设置的 Cookie，将其发送到服务器。
curl -b cookies.txt https://www.google.com
```

### 16、-c(将服务器设置的 Cookie 写入文件)

```bash
# 将服务器的 HTTP 回应所设置 Cookie 写入文本文件`cookies.txt`
curl -c cookies.txt https://www.google.com
```

### 17、-G(构造 URL 的查询字符串)

```bash
# 命令会发出一个 GET 请求，实际请求的 URL 为`https://google.com/search?q=kitties&count=20`。如果省略`--G`，会发出一个 POST 请求。
curl -G -d 'q=kitties' -d 'count=20' https://google.com/search
# 如果数据需要 URL 编码，可以结合`--data--urlencode`参数。
curl -G --data-urlencode 'comment=hello world' https://www.example.com
```

### 18、-k(跳过 SSL 检测)

```bash
# 不会检查服务器的 SSL 证书是否正确
curl -k https://www.example.com
```

### 19、**-L**(让 HTTP 请求跟随服务器的重定向)

curl 默认不跟随重定向。

```bash
curl -L -d 'tweet=hi' https://api.twitter.com/tweet
```

### 20、--limit-rate(限制 HTTP 请求和回应的带宽）

可以模拟慢网速的环境。

```bash
# 将带宽限制在每秒 200K 字节
curl --limit-rate 200k https://google.com
```

### 21、-s(不显示错误信息)

`-s`参数将不输出错误和进度信息。

```bash
# 命令一旦发生错误，不会显示错误信息。不发生错误的话，会正常显示运行结果
curl -s https://www.example.com
```

如果想让 curl 不产生任何输出，可以使用下面的命令。

```bash
curl -s -o /dev/null https://google.com
```

### 22、-S(只输出错误信息)

```bash
# 没有任何输出，除非发生错误
curl -s -o /dev/null https://google.com
```

### 23、-u(设置服务器认证的用户名和密码)(--user)

```bash
# 设置用户名为`bob`，密码为`12345`，然后将其转为 HTTP 标头`Authorization: Basic Ym9iOjEyMzQ1`
curl -u 'bob:12345' https://google.com/login
```

curl 能够识别 URL 里面的用户名和密码。

```bash
# 命令能够识别 URL 里面的用户名和密码，将其转为上个例子里面的 HTTP 标头
curl https://bob:12345@google.com/login
# 命令只设置了用户名，执行后，curl 会提示用户输入密码
curl -u 'bob' https://google.com/login
```

## 九、shell编程

参考菜鸟教程：[https://www.runoob.com/linux/linux-shell.html](https://www.runoob.com/linux/linux-shell.html)

### 1、shell是什么

#### （1）shell是什么

- Shell 是一个用 C 语言编写的程序，它是用户使用 Linux 的桥梁。Shell 既是一种命令语言，又是一种程序设计语言。
- Shell 是指一种应用程序，这个应用程序提供了一个界面，用户通过这个界面访问操作系统内核的服务。
- Ken Thompson 的 sh 是第一种 Unix Shell，Windows Explorer 是一个典型的图形界面 Shell。

![image-20220801112110021](http://cdn.bluecusliyou.com/202208011121162.png)

#### （2）shell脚本

- Shell 脚本（shell script），是一种为 shell 编写的脚本程序。
- 业界所说的 shell 通常都是指 shell 脚本，但读者朋友要知道，shell 和 shell script 是两个不同的概念。
- 由于习惯的原因，简洁起见，本文出现的 "shell编程" 都是指 shell 脚本编程，不是指开发 shell 自身。

#### （3）Shell 环境

Shell 编程跟 JavaScript、php 编程一样，只要有一个能编写代码的文本编辑器和一个能解释执行的脚本解释器就可以了。

Linux 的 Shell 种类众多，常见的有：

- Bourne Shell（/usr/bin/sh或/bin/sh）
- Bourne Again Shell（/bin/bash）
- C Shell（/usr/bin/csh）
- K Shell（/usr/bin/ksh）
- Shell for Root（/sbin/sh）

本教程关注的是 Bash，也就是 Bourne Again Shell，由于易用和免费，Bash 在日常工作中被广泛使用。同时，Bash 也是大多数Linux 系统默认的 Shell。

在一般情况下，人们并不区分 Bourne Shell 和 Bourne Again Shell，所以，像 **#!/bin/sh**，它同样也可以改为 **#!/bin/bash**。**#!** 告诉系统其后路径所指定的程序即是解释此脚本文件的 Shell 程序。

#### （4）第一个shell脚本

打开文本编辑器(可以使用 vi/vim 命令来创建文件)，新建一个文件 test.sh，扩展名为 sh（sh代表shell），扩展名并不影响脚本执行，见名知意就好，如果你用 php 写 shell 脚本，扩展名就用 php 好了。

**#!** 是一个约定的标记，它告诉系统这个脚本需要什么解释器来执行，即使用哪一种 Shell。echo 命令用于向窗口输出文本。

```sh
#!/bin/bash
echo "Hello World !"
```

> 运行 Shell 脚本方法1：**作为可执行程序**

将上面的代码保存为 test.sh，并 cd 到相应目录：

```bash
chmod +x ./test.sh  #使脚本具有执行权限
./test.sh  #执行脚本
```

注意，一定要写成 **./test.sh**，而不是 **test.sh**，运行其它二进制的程序也一样，直接写 test.sh，linux 系统会去 PATH 里寻找有没有叫 test.sh 的，而只有 /bin, /sbin, /usr/bin，/usr/sbin 等在 PATH 里，你的当前目录通常不在 PATH 里，所以写成 test.sh 是会找不到命令的，要用 ./test.sh 告诉系统说，就在当前目录找。

> 运行 Shell 脚本方法1：**作为解释器参数**

这种运行方式是，直接运行解释器，其参数就是 shell 脚本的文件名，如：

```bash
/bin/sh test.sh
```

这种方式运行的脚本，不需要在第一行指定解释器信息，写了也没用。

### 2、Shell变量

#### （1）定义变量

定义变量时，变量名不加美元符号（$，PHP语言中变量需要），如：

```sh
your_name="runoob.com"
```

注意，变量名和等号之间不能有空格，这可能和你熟悉的所有编程语言都不一样。同时，变量名的命名须遵循如下规则：

- 命名只能使用英文字母，数字和下划线，首个字符不能以数字开头。
- 中间不能有空格，可以使用下划线 **_**。
- 不能使用标点符号。
- 不能使用bash里的关键字（可用help命令查看保留关键字）。

有效的 Shell 变量名示例如下：

```sh
RUNOOB
LD_LIBRARY_PATH
_var
var2
```

无效的变量命名：

```sh
?var=123
user*name=runoob
```

除了显式地直接赋值，还可以用语句给变量赋值，如：

```bash
for file in ls /etc
或
for file in $(ls /etc)
```

 以上语句将 /etc 下目录的文件名循环出来。

#### （2）使用变量

使用一个定义过的变量，只要在变量名前面加美元符号即可，如：

```sh
your_name="qinjx"
echo $your_name
echo ${your_name}
```

变量名外面的花括号是可选的，加不加都行，加花括号是为了帮助解释器识别变量的边界，比如下面这种情况：

```sh
for skill in Ada Coffe Action Java; do
    echo "I am good at ${skill}Script"
done
```

如果不给skill变量加花括号，解释器就会把skillScript当成一个变量（其值为空），代码执行结果就不是我们期望的样子了。推荐给所有变量加上花括号，这是个好的编程习惯。

已定义的变量，可以被重新定义，如：

```sh
your_name="tom"
echo $your_name
your_name="alibaba"
echo $your_name
```

#### （3）只读变量

使用 readonly 命令可以将变量定义为只读变量，只读变量的值不能被改变。

下面的例子尝试更改只读变量，结果报错：

```sh
#!/bin/bash
myUrl="https://www.google.com"
readonly myUrl
myUrl="https://www.runoob.com"
```

运行脚本，结果如下：

```bash
/bin/sh: NAME: This variable is read only.
```

#### （4）删除变量

使用 unset 命令可以删除变量。语法：

```sh
unset variable_name
```

变量被删除后不能再次使用。unset 命令不能删除只读变量。

```sh
#!/bin/sh

myUrl="https://www.runoob.com"
unset myUrl
echo $myUrl
```

以上实例执行将没有任何输出。

#### （5）变量类型

运行shell时，会同时存在三种变量：

- **1) 局部变量** 局部变量在脚本或命令中定义，仅在当前shell实例中有效，其他shell启动的程序不能访问局部变量。
- **2) 环境变量** 所有的程序，包括shell启动的程序，都能访问环境变量，有些程序需要环境变量来保证其正常运行。必要的时候shell脚本也可以定义环境变量。

```sh
PATH：命令所示路径，以冒号为分割；
HOME：打印用户家目录；
SHELL：显示当前She1l类型，
USER：打印当前用户名；
ID：打印当前用户id信息；
PWD：显示当前所在路径；
TERM：打印当前终端类型；
HOSTNAME：显示当前主机名；
```

- **3) shell变量** shell变量是由shell程序设置的特殊变量。shell变量中有一部分是环境变量，有一部分是局部变量，这些变量保证了shell的正常运行

```sh
$0：当前脚本的名称；
$n：当前脚本的第n个参数，n=1,2,9;
$*：当前脚本的所有参数（不包括程序本身）；
$#：当前脚本的参数个数（不包括程序本身）；
$?：令或程序执行完后的状态，返回表示执行成功；
$$：程序本身的PID号。
```

### 3、Shell 字符串

字符串是shell编程中最常用最有用的数据类型（除了数字和字符串，也没啥其它类型好用了），字符串可以用单引号，也可以用双引号，也可以不用引号。

#### （1）单引号

```
str='this is a string'
```

单引号字符串的限制：

- 单引号里的任何字符都会原样输出，单引号字符串中的变量是无效的；
- 单引号字串中不能出现单独一个的单引号（对单引号使用转义符后也不行），但可成对出现，作为字符串拼接使用。

#### （2）双引号

```sh
your_name="runoob"
str="Hello, I know you are \"$your_name\"! \n"
echo -e $str
```

输出结果为：

```sh
Hello, I know you are "runoob"! 
```

双引号的优点：

- 双引号里可以有变量
- 双引号里可以出现转义字符

#### （3）拼接字符串

```sh
your_name="runoob"
# 使用双引号拼接
greeting="hello, "$your_name" !"
greeting_1="hello, ${your_name} !"
echo $greeting  $greeting_1

# 使用单引号拼接
greeting_2='hello, '$your_name' !'
greeting_3='hello, ${your_name} !'
echo $greeting_2  $greeting_3
```

输出结果为：

```bash
hello, runoob ! hello, runoob !
hello, runoob ! hello, ${your_name} !
```

#### （4）获取字符串长度

```sh
string="abcd"
echo ${#string}   # 输出 4
```

变量为数组时，**${#string}** 等价于 **${#string[0]}**:

```sh
string="abcd"
echo ${#string[0]}   # 输出 4
```

#### （5）提取子字符串

以下实例从字符串第 **2** 个字符开始截取 **4** 个字符：

```sh
string="runoob is a great site"
echo ${string:1:4} # 输出 unoo
```

**注意**：第一个字符的索引值为 **0**。

#### （6）查找子字符串

查找字符 **i** 或 **o** 的位置(哪个字母先出现就计算哪个)：

```sh
string="runoob is a great site"
echo `expr index "$string" io`  # 输出 4
```

**注意：** 以上脚本中 **`** 是反引号，而不是单引号 **'**，不要看错了哦。

### 4、Shell 注释

#### （1）单行注释

以 **#** 开头的行就是注释，会被解释器忽略。

通过每一行加一个 **#** 号设置多行注释，像这样：

```sh
#--------------------------------------------
# 这是一个注释
# slogan：学的不仅是技术，更是梦想！
#--------------------------------------------
##### 用户配置区 开始 #####
#
#
# 这里可以添加脚本描述信息
#
#
##### 用户配置区 结束  #####
```

如果在开发过程中，遇到大段的代码需要临时注释起来，过一会儿又取消注释，怎么办呢？

每一行加个#符号太费力了，可以把这一段要注释的代码用一对花括号括起来，定义成一个函数，没有地方调用这个函数，这块代码就不会执行，达到了和注释一样的效果。

#### （2）多行注释

多行注释还可以使用以下格式：

```sh
:<<EOF
注释内容...
注释内容...
注释内容...
EOF
```

EOF 也可以使用其他符号:

```sh
:<<'
注释内容...
注释内容...
注释内容...
'

:<<!
注释内容...
注释内容...
注释内容...
!
```

### 5、Shell 传递参数

我们可以在执行 Shell 脚本时，向脚本传递参数，脚本内获取参数的格式为：**$n**。**n** 代表一个数字，1 为执行脚本的第一个参数，2 为执行脚本的第二个参数，以此类推……

以下实例我们向脚本传递三个参数，并分别输出，其中 **$0** 为执行的文件名（包含文件路径）：

```sh
#!/bin/bash

echo "Shell 传递参数实例！";
echo "执行的文件名：$0";
echo "第一个参数为：$1";
echo "第二个参数为：$2";
echo "第三个参数为：$3";
```

为脚本设置可执行权限，并执行脚本，输出结果如下所示：

```bash
$ chmod +x test.sh 
$ ./test.sh 1 2 3
Shell 传递参数实例！
执行的文件名：./test.sh
第一个参数为：1
第二个参数为：2
第三个参数为：3
```

另外，还有几个特殊字符用来处理参数：

| 参数处理 | 说明                                                         |
| :------- | :----------------------------------------------------------- |
| $#       | 传递到脚本的参数个数                                         |
| $*       | 以一个单字符串显示所有向脚本传递的参数。 如"$*"用「"」括起来的情况、以"$1 $2 … $n"的形式输出所有参数。 |
| $$       | 脚本运行的当前进程ID号                                       |
| $!       | 后台运行的最后一个进程的ID号                                 |
| $@       | 与$*相同，但是使用时加引号，并在引号中返回每个参数。 如"$@"用「"」括起来的情况、以"$1" "$2" … "$n" 的形式输出所有参数。 |
| $-       | 显示Shell使用的当前选项，与[set命令](https://www.runoob.com/linux/linux-comm-set.html)功能相同。 |
| $?       | 显示最后命令的退出状态。0表示没有错误，其他任何值表明有错误。 |

```sh
#!/bin/bash

echo "Shell 传递参数实例！";
echo "第一个参数为：$1";

echo "参数个数为：$#";
echo "传递的参数作为一个字符串显示：$*";
```

执行脚本，输出结果如下所示：

```bash
$ chmod +x test.sh 
$ ./test.sh 1 2 3
Shell 传递参数实例！
第一个参数为：1
参数个数为：3
传递的参数作为一个字符串显示：1 2 3
```

$* 与 $@ 区别：

- 相同点：都是引用所有参数。
- 不同点：只有在双引号中体现出来。假设在脚本运行时写了三个参数 1、2、3，，则 " * " 等价于 "1 2 3"（传递了一个参数），而 "@" 等价于 "1" "2" "3"（传递了三个参数）。

```sh
#!/bin/bash

echo "-- \$* 演示 ---"
for i in "$*"; do
    echo $i
done

echo "-- \$@ 演示 ---"
for i in "$@"; do
    echo $i
done
```

执行脚本，输出结果如下所示：

```bash
$ chmod +x test.sh 
$ ./test.sh 1 2 3
-- $* 演示 ---
1 2 3
-- $@ 演示 ---
1
2
3
```

### 6、Shell 数组

bash支持一维数组（不支持多维数组），并且没有限定数组的大小。

类似于 C 语言，数组元素的下标由 0 开始编号。获取数组中的元素要利用下标，下标可以是整数或算术表达式，其值应大于或等于 0。

#### （1）定义数组

在 Shell 中，用括号来表示数组，数组元素用"空格"符号分割开。定义数组的一般形式为：

```sh
数组名=(值1 值2 ... 值n)
```

例如：

```sh
array_name=(value0 value1 value2 value3)
```

或者

```sh
array_name=(
value0
value1
value2
value3
)
```

还可以单独定义数组的各个分量：

```sh
array_name[0]=value0
array_name[1]=value1
array_name[n]=valuen
```

可以不使用连续的下标，而且下标的范围没有限制。

#### （2）读取数组

读取数组元素值的一般格式是：

```sh
${数组名[下标]}
```

以下实例通过数字索引读取数组元素：

```sh
#!/bin/bash

my_array=(A B "C" D)

echo "第一个元素为: ${my_array[0]}"
echo "第二个元素为: ${my_array[1]}"
echo "第三个元素为: ${my_array[2]}"
echo "第四个元素为: ${my_array[3]}"
```

执行脚本，输出结果如下所示：

```bash
$ chmod +x test.sh 
$ ./test.sh
第一个元素为: A
第二个元素为: B
第三个元素为: C
第四个元素为: D
```

#### （3）关联数组

Bash 支持关联数组，可以使用任意的字符串、或者整数作为下标来访问数组元素。

关联数组使用 **[declare](https://www.runoob.com/linux/linux-comm-declare.html)** 命令来声明，语法格式如下：

```sh
declare -A array_name
```

**-A** 选项就是用于声明一个关联数组。关联数组的键是唯一的。

以下实例我们创建一个关联数组 **site**，并创建不同的键值：

```sh
declare -A site=(["google"]="www.google.com", ["runoob"]="www.runoob.com", ["taobao"]="www.taobao.com")
```

我们也可以先声明一个关联数组，然后再设置键和值：

```sh
declare -A site
site["google"]="www.google.com"
site["runoob"]="www.runoob.com"
site["taobao"]="www.taobao.com"
```

也可以在定义的同时赋值：

访问关联数组元素可以使用指定的键，格式如下：

```sh
array_name["index"]
```

以下实例我们通过键来访问关联数组的元素：

```sh
declare -A site
site["google"]="www.google.com"
site["runoob"]="www.runoob.com"
site["taobao"]="www.taobao.com"

echo ${site["runoob"]}
```

执行脚本，输出结果如下所示：

```bash
www.runoob.com
```

#### （4）获取数组中的所有元素

使用 **@** 或 ***** 可以获取数组中的所有元素，例如：

```sh
#!/bin/bash

my_array[0]=A
my_array[1]=B
my_array[2]=C
my_array[3]=D

echo "数组的元素为: ${my_array[*]}"
echo "数组的元素为: ${my_array[@]}"
```

执行脚本，输出结果如下所示：

```bash
$ chmod +x test.sh 
$ ./test.sh
数组的元素为: A B C D
数组的元素为: A B C D
```

```sh
declare -A site
site["google"]="www.google.com"
site["runoob"]="www.runoob.com"
site["taobao"]="www.taobao.com"

echo "数组的元素为: ${site[*]}"
echo "数组的元素为: ${site[@]}"
```

执行脚本，输出结果如下所示：

```bash
$ chmod +x test.sh 
$ ./test.sh
数组的元素为: www.google.com www.runoob.com www.taobao.com
数组的元素为: www.google.com www.runoob.com www.taobao.com
```

在数组前加一个感叹号 **!** 可以获取数组的所有键，例如：

```sh
declare -A site
site["google"]="www.google.com"
site["runoob"]="www.runoob.com"
site["taobao"]="www.taobao.com"

echo "数组的键为: ${!site[*]}"
echo "数组的键为: ${!site[@]}"
```

执行脚本，输出结果如下所示：

```
数组的键为: google runoob taobao
数组的键为: google runoob taobao
```

#### （5）获取数组的长度

获取数组长度的方法与获取字符串长度的方法相同，例如：

```sh
#!/bin/bash

my_array[0]=A
my_array[1]=B
my_array[2]=C
my_array[3]=D

echo "数组元素个数为: ${#my_array[*]}"
echo "数组元素个数为: ${#my_array[@]}"
```

执行脚本，输出结果如下所示：

```
$ chmod +x test.sh 
$ ./test.sh
数组元素个数为: 4
数组元素个数为: 4
```

### 7、Shell 基本运算符

原生bash不支持简单的数学运算，但是可以通过其他命令来实现，例如 awk 和 expr，expr 最常用。

expr 是一款表达式计算工具，使用它能完成表达式的求值操作。

例如，两个数相加(**注意使用的是反引号 \**`\** 而不是单引号 \**'\****)：

```sh
#!/bin/bash

val=`expr 2 + 2`
echo "两数之和为 : $val"
```

执行脚本，输出结果如下所示：

```bash
两数之和为 : 4
```

两点注意：

- 表达式和运算符之间要有空格，例如 2+2 是不对的，必须写成 2 + 2，这与我们熟悉的大多数编程语言不一样。
- 完整的表达式要被`反引号`包含，注意这个字符不是常用的单引号，在 Esc 键下边。

#### （1）算术运算符

下表列出了常用的算术运算符，假定变量 a 为 10，变量 b 为 20：

| 运算符 | 说明                                          | 举例                          |
| :----- | :-------------------------------------------- | :---------------------------- |
| +      | 加法                                          | `expr $a + $b` 结果为 30。    |
| -      | 减法                                          | `expr $a - $b` 结果为 -10。   |
| *      | 乘法                                          | `expr $a \* $b` 结果为  200。 |
| /      | 除法                                          | `expr $b / $a` 结果为 2。     |
| %      | 取余                                          | `expr $b % $a` 结果为 0。     |
| =      | 赋值                                          | a=$b 把变量 b 的值赋给 a。    |
| ==     | 相等。用于比较两个数字，相同则返回 true。     | [ $a == $b ] 返回 false。     |
| !=     | 不相等。用于比较两个数字，不相同则返回 true。 | [ $a != $b ] 返回 true。      |

**注意：**条件表达式要放在方括号之间，并且要有空格。算术运算符实例如下：

```sh
#!/bin/bash

a=10
b=20

val=`expr $a + $b`
echo "a + b : $val"

val=`expr $a - $b`
echo "a - b : $val"

val=`expr $a \* $b`
echo "a * b : $val"

val=`expr $b / $a`
echo "b / a : $val"

val=`expr $b % $a`
echo "b % a : $val"

if [ $a == $b ]
then
   echo "a 等于 b"
fi
if [ $a != $b ]
then
   echo "a 不等于 b"
fi
```

执行脚本，输出结果如下所示：

```bash
a + b : 30
a - b : -10
a * b : 200
b / a : 2
b % a : 0
a 不等于 b
```

**注意：**

- 乘号(*)前边必须加反斜杠(\)才能实现乘法运算；
- if...then...fi 是条件语句，后续将会讲解。
- 在 MAC 中 shell 的 expr 语法是：**$((表达式))**，此处表达式中的 "*" 不需要转义符号 "\" 。

#### （2）关系运算符

关系运算符只支持数字，不支持字符串，除非字符串的值是数字。

下表列出了常用的关系运算符，假定变量 a 为 10，变量 b 为 20：

| 运算符 | 说明                                                  |
| :----- | :---------------------------------------------------- |
| -eq    | 检测两个数是否相等，相等返回 true。                   |
| -ne    | 检测两个数是否不相等，不相等返回 true。               |
| -gt    | 检测左边的数是否大于右边的，如果是，则返回 true。     |
| -lt    | 检测左边的数是否小于右边的，如果是，则返回 true。     |
| -ge    | 检测左边的数是否大于等于右边的，如果是，则返回 true。 |
| -le    | 检测左边的数是否小于等于右边的，如果是，则返回 true。 |

关系运算符实例如下：

```sh
#!/bin/bash

a=10
b=20

if [ $a -eq $b ]
then
   echo "$a -eq $b : a 等于 b"
else
   echo "$a -eq $b: a 不等于 b"
fi
if [ $a -ne $b ]
then
   echo "$a -ne $b: a 不等于 b"
else
   echo "$a -ne $b : a 等于 b"
fi
if [ $a -gt $b ]
then
   echo "$a -gt $b: a 大于 b"
else
   echo "$a -gt $b: a 不大于 b"
fi
if [ $a -lt $b ]
then
   echo "$a -lt $b: a 小于 b"
else
   echo "$a -lt $b: a 不小于 b"
fi
if [ $a -ge $b ]
then
   echo "$a -ge $b: a 大于或等于 b"
else
   echo "$a -ge $b: a 小于 b"
fi
if [ $a -le $b ]
then
   echo "$a -le $b: a 小于或等于 b"
else
   echo "$a -le $b: a 大于 b"
fi
```

执行脚本，输出结果如下所示：

```bash
10 -eq 20: a 不等于 b
10 -ne 20: a 不等于 b
10 -gt 20: a 不大于 b
10 -lt 20: a 小于 b
10 -ge 20: a 小于 b
10 -le 20: a 小于或等于 b
```

#### （3）布尔运算符

下表列出了常用的布尔运算符，假定变量 a 为 10，变量 b 为 20：

| 运算符 | 说明                                                |
| :----- | :-------------------------------------------------- |
| !      | 非运算，表达式为 true 则返回 false，否则返回 true。 |
| -o     | 或运算，有一个表达式为 true 则返回 true。           |
| -a     | 与运算，两个表达式都为 true 才返回 true。           |

布尔运算符实例如下：

```sh
#!/bin/bash

a=10
b=20

if [ $a != $b ]
then
   echo "$a != $b : a 不等于 b"
else
   echo "$a == $b: a 等于 b"
fi
if [ $a -lt 100 -a $b -gt 15 ]
then
   echo "$a 小于 100 且 $b 大于 15 : 返回 true"
else
   echo "$a 小于 100 且 $b 大于 15 : 返回 false"
fi
if [ $a -lt 100 -o $b -gt 100 ]
then
   echo "$a 小于 100 或 $b 大于 100 : 返回 true"
else
   echo "$a 小于 100 或 $b 大于 100 : 返回 false"
fi
if [ $a -lt 5 -o $b -gt 100 ]
then
   echo "$a 小于 5 或 $b 大于 100 : 返回 true"
else
   echo "$a 小于 5 或 $b 大于 100 : 返回 false"
fi
```

执行脚本，输出结果如下所示：

```bash
10 != 20 : a 不等于 b
10 小于 100 且 20 大于 15 : 返回 true
10 小于 100 或 20 大于 100 : 返回 true
10 小于 5 或 20 大于 100 : 返回 false
```

#### （4）逻辑运算符

以下介绍 Shell 的逻辑运算符，假定变量 a 为 10，变量 b 为 20:

| 运算符 | 说明       |
| :----- | :--------- |
| &&     | 逻辑的 AND |
| \|\|   | 逻辑的 OR  |

逻辑运算符实例如下：

```sh
#!/bin/bash

a=10
b=20

if [[ $a -lt 100 && $b -gt 100 ]]
then
   echo "返回 true"
else
   echo "返回 false"
fi

if [[ $a -lt 100 || $b -gt 100 ]]
then
   echo "返回 true"
else
   echo "返回 false"
fi
```

执行脚本，输出结果如下所示：

```bash
返回 false
返回 true
```

#### （5）字符串运算符

下表列出了常用的字符串运算符，假定变量 a 为 "abc"，变量 b 为 "efg"：

| 运算符 | 说明                                         |
| :----- | :------------------------------------------- |
| =      | 检测两个字符串是否相等，相等返回 true。      |
| !=     | 检测两个字符串是否不相等，不相等返回 true。  |
| -z     | 检测字符串长度是否为0，为0返回 true。        |
| -n     | 检测字符串长度是否不为 0，不为 0 返回 true。 |
| $      | 检测字符串是否不为空，不为空返回 true。      |

字符串运算符实例如下：

```bash
#!/bin/bash

a="abc"
b="efg"

if [ $a = $b ]
then
   echo "$a = $b : a 等于 b"
else
   echo "$a = $b: a 不等于 b"
fi
if [ $a != $b ]
then
   echo "$a != $b : a 不等于 b"
else
   echo "$a != $b: a 等于 b"
fi
if [ -z $a ]
then
   echo "-z $a : 字符串长度为 0"
else
   echo "-z $a : 字符串长度不为 0"
fi
if [ -n "$a" ]
then
   echo "-n $a : 字符串长度不为 0"
else
   echo "-n $a : 字符串长度为 0"
fi
if [ $a ]
then
   echo "$a : 字符串不为空"
else
   echo "$a : 字符串为空"
fi
```

执行脚本，输出结果如下所示：

```bash
abc = efg: a 不等于 b
abc != efg : a 不等于 b
-z abc : 字符串长度不为 0
-n abc : 字符串长度不为 0
abc : 字符串不为空
```

#### （6）文件测试运算符

文件测试运算符用于检测 Unix 文件的各种属性。

属性检测描述如下：

| 操作符  | 说明                                                         | 举例                      |
| :------ | :----------------------------------------------------------- | :------------------------ |
| -b file | 检测文件是否是块设备文件，如果是，则返回 true。              | [ -b $file ] 返回 false。 |
| -c file | 检测文件是否是字符设备文件，如果是，则返回 true。            | [ -c $file ] 返回 false。 |
| -d file | 检测文件是否是目录，如果是，则返回 true。                    | [ -d $file ] 返回 false。 |
| -f file | 检测文件是否是普通文件（既不是目录，也不是设备文件），如果是，则返回 true。 | [ -f $file ] 返回 true。  |
| -g file | 检测文件是否设置了 SGID 位，如果是，则返回 true。            | [ -g $file ] 返回 false。 |
| -k file | 检测文件是否设置了粘着位(Sticky Bit)，如果是，则返回 true。  | [ -k $file ] 返回 false。 |
| -p file | 检测文件是否是有名管道，如果是，则返回 true。                | [ -p $file ] 返回 false。 |
| -u file | 检测文件是否设置了 SUID 位，如果是，则返回 true。            | [ -u $file ] 返回 false。 |
| -r file | 检测文件是否可读，如果是，则返回 true。                      | [ -r $file ] 返回 true。  |
| -w file | 检测文件是否可写，如果是，则返回 true。                      | [ -w $file ] 返回 true。  |
| -x file | 检测文件是否可执行，如果是，则返回 true。                    | [ -x $file ] 返回 true。  |
| -s file | 检测文件是否为空（文件大小是否大于0），不为空返回 true。     | [ -s $file ] 返回 true。  |
| -e file | 检测文件（包括目录）是否存在，如果是，则返回 true。          | [ -e $file ] 返回 true。  |

其他检查符：

- **-S**: 判断某文件是否 socket。
- **-L**: 检测文件是否存在并且是一个符号链接。

变量 file 表示文件 **/var/www/runoob/test.sh**，它的大小为 100 字节，具有 **rwx** 权限。下面的代码，将检测该文件的各种属性：

```sh
#!/bin/bash

file="/var/www/runoob/test.sh"
if [ -r $file ]
then
   echo "文件可读"
else
   echo "文件不可读"
fi
if [ -w $file ]
then
   echo "文件可写"
else
   echo "文件不可写"
fi
if [ -x $file ]
then
   echo "文件可执行"
else
   echo "文件不可执行"
fi
if [ -f $file ]
then
   echo "文件为普通文件"
else
   echo "文件为特殊文件"
fi
if [ -d $file ]
then
   echo "文件是个目录"
else
   echo "文件不是个目录"
fi
if [ -s $file ]
then
   echo "文件不为空"
else
   echo "文件为空"
fi
if [ -e $file ]
then
   echo "文件存在"
else
   echo "文件不存在"
fi
```

执行脚本，输出结果如下所示：

```bash
文件可读
文件可写
文件可执行
文件为普通文件
文件不是个目录
文件不为空
文件存在
```

### 8、Shell echo命令

Shell 的 echo 指令与 PHP 的 echo 指令类似，都是用于字符串的输出。命令格式：

```sh
echo string
```

您可以使用echo实现更复杂的输出格式控制。

#### （1）显示普通字符串

```sh
echo "It is a test"
```

这里的双引号完全可以省略，以下命令与上面实例效果一致：

```sh
echo It is a test
```

#### （2）显示转义字符

```sh
echo "\"It is a test\""
```

结果将是:

```bash
"It is a test"
```

同样，双引号也可以省略

#### （3）显示变量

read 命令从标准输入中读取一行,并把输入行的每个字段的值指定给 shell 变量

```sh
#!/bin/sh
read name 
echo "$name It is a test"
```

以上代码保存为 test.sh，name 接收标准输入的变量，结果将是:

```bash
[root@www ~]# sh test.sh
OK                     #标准输入
OK It is a test        #输出
```

#### （4）显示换行

```sh
echo -e "OK! \n" # -e 开启转义
echo "It is a test"
```

输出结果：

```bash
OK!

It is a test
```

#### （5）显示不换行

```sh
#!/bin/sh
echo -e "OK! \c" # -e 开启转义 \c 不换行
echo "It is a test"
```

输出结果：

```bash、
OK! It is a test
```

#### （6）显示结果定向至文件

```sh
echo "It is a test" > myfile
```

#### （7）原样输出字符串，不进行转义或取变量(用单引号)

```sh
echo '$name\"'
```

输出结果：

```bash
$name\"
```

#### （8）显示命令执行结果

```sh
echo `date`
```

**注意：** 这里使用的是反引号 **`**, 而不是单引号 **'**。结果将显示当前日期：

```bash
Thu Jul 24 10:08:46 CST 2022
```

### 9、Shell printf 命令

printf 命令模仿 C 程序库（library）里的 printf() 程序。

printf 由 POSIX 标准所定义，因此使用 printf 的脚本比使用 echo 移植性好。

printf 使用引用文本或空格分隔的参数，外面可以在 **printf** 中使用格式化字符串，还可以制定字符串的宽度、左右对齐方式等。默认的 printf 不会像 **echo** 自动添加换行符，我们可以手动添加 **\n**。

printf 命令的语法：

```sh
printf  format-string  [arguments...]
```

**参数说明：**

- **format-string:** 为格式控制字符串
- **arguments:** 为参数列表。

接下来,我来用一个脚本来体现 printf 的强大功能：

```sh
#!/bin/bash
 
printf "%-10s %-8s %-4s\n" 姓名 性别 体重kg  
printf "%-10s %-8s %-4.2f\n" 郭靖 男 66.1234
printf "%-10s %-8s %-4.2f\n" 杨过 男 48.6543
printf "%-10s %-8s %-4.2f\n" 郭芙 女 47.9876
```

执行脚本，输出结果如下所示：

```bash
姓名     性别   体重kg
郭靖     男      66.12
杨过     男      48.65
郭芙     女      47.99
```

**%s %c %d %f** 都是格式替代符，**％s** 输出一个字符串，**％d** 整型输出，**％c** 输出一个字符，**％f** 输出实数，以小数形式输出。

**%-10s** 指一个宽度为 10 个字符（**-** 表示左对齐，没有则表示右对齐），任何字符都会被显示在 10 个字符宽的字符内，如果不足则自动以空格填充，超过也会将内容全部显示出来。

**%-4.2f** 指格式化为小数，其中 **.2** 指保留2位小数。

```sh
#!/bin/bash
 
# format-string为双引号
printf "%d %s\n" 1 "abc"

# 单引号与双引号效果一样
printf '%d %s\n' 1 "abc"

# 没有引号也可以输出
printf %s abcdef

# 格式只指定了一个参数，但多出的参数仍然会按照该格式输出，format-string 被重用
printf %s abc def

printf "%s\n" abc def

printf "%s %s %s\n" a b c d e f g h i j

# 如果没有 arguments，那么 %s 用NULL代替，%d 用 0 代替
printf "%s and %d \n"
```

执行脚本，输出结果如下所示：

```bash
1 abc
1 abc
abcdefabcdefabc
def
a b c
d e f
g h i
j  
 and 0
```

> printf 的转义序列

| 序列  | 说明                                                         |
| :---- | :----------------------------------------------------------- |
| \a    | 警告字符，通常为ASCII的BEL字符                               |
| \b    | 后退                                                         |
| \c    | 抑制（不显示）输出结果中任何结尾的换行字符（只在%b格式指示符控制下的参数字符串中有效），而且，任何留在参数里的字符、任何接下来的参数以及任何留在格式字符串中的字符，都被忽略 |
| \f    | 换页（formfeed）                                             |
| \n    | 换行                                                         |
| \r    | 回车（Carriage return）                                      |
| \t    | 水平制表符                                                   |
| \v    | 垂直制表符                                                   |
| \\    | 一个字面上的反斜杠字符                                       |
| \ddd  | 表示1到3位数八进制值的字符。仅在格式字符串中有效             |
| \0ddd | 表示1到3位的八进制值字符                                     |

```bash
$ printf "a string, no processing:<%s>\n" "A\nB"
a string, no processing:<A\nB>

$ printf "a string, no processing:<%b>\n" "A\nB"
a string, no processing:<A
B>

$ printf "www.runoob.com \a"
www.runoob.com $                  #不换行
```

### 10、Shell test 命令

Shell中的 test 命令用于检查某个条件是否成立，它可以进行数值、字符和文件三个方面的测试。

数值测试

```sh
num1=100
num2=100
if test $[num1] -eq $[num2]
then
    echo '两个数相等！'
else
    echo '两个数不相等！'
fi
```

输出结果：

```bash
两个数相等！
```

字符串测试

```sh
num1="ru1noob"
num2="runoob"
if test $num1 = $num2
then
    echo '两个字符串相等!'
else
    echo '两个字符串不相等!'
fi
```

输出结果：

```bash
两个字符串不相等!
```

### 11、Shell 流程控制

sh 的流程控制不可为空，如果 else 分支没有语句执行，就不要写这个 else

#### （1）if 语句语法格式：

```sh
if condition
then
    command1 
    command2
    ...
    commandN 
fi
```

写成一行（适用于终端命令提示符）：

```sh
if [ $(ps -ef | grep -c "ssh") -gt 1 ]; then echo "true"; fi
```

#### （2）if else 语法格式：

```sh
if condition
then
    command1 
    command2
    ...
    commandN
else
    command
fi
```

#### （3）if else-if else 语法格式：

```sh
if condition1
then
    command1
elif condition2 
then 
    command2
else
    commandN
fi
```

#### （4）使用 **[...]** 判断语句

if else 的 **[...]** 判断语句中大于使用 **-gt**，小于使用 **-lt**。

```sh
if [ "$a" -gt "$b" ]; then
    ...
fi
```

```sh
a=10
b=20
if [ $a == $b ]
then
   echo "a 等于 b"
elif [ $a -gt $b ]
then
   echo "a 大于 b"
elif [ $a -lt $b ]
then
   echo "a 小于 b"
else
   echo "没有符合的条件"
fi
```

输出结果：

```bash
a 小于 b
```

#### （5）使用 **((...))** 判断语句

如果使用 **((...))** 作为判断语句，大于和小于可以直接使用 **>** 和 **<**。

```sh
if (( a > b )); then
    ...
fi
```

```sh
a=10
b=20
if (( $a == $b ))
then
   echo "a 等于 b"
elif (( $a > $b ))
then
   echo "a 大于 b"
elif (( $a < $b ))
then
   echo "a 小于 b"
else
   echo "没有符合的条件"
fi
```

输出结果：

```bash
a 小于 b
```

#### （6）for 循环

与其他编程语言类似，Shell支持for循环。for循环一般格式为：

```sh
for var in item1 item2 ... itemN
do
    command1
    command2
    ...
    commandN
done
```

写成一行：

```sh
for var in item1 item2 ... itemN; do command1; command2… done;
```

当变量值在列表里，for 循环即执行一次所有命令，使用变量名获取列表中的当前取值。命令可为任何有效的 shell 命令和语句。in 列表可以包含替换、字符串和文件名。in列表是可选的，如果不用它，for循环使用命令行的位置参数。

例如，顺序输出当前列表中的数字：

```sh
for loop in 1 2 3 4 5
do
    echo "The value is: $loop"
done
```

输出结果：

```bash
The value is: 1
The value is: 2
The value is: 3
The value is: 4
The value is: 5
```

顺序输出字符串中的字符：

```sh
#!/bin/bash

for str in This is a string
do
    echo $str
done
```

输出结果：

```bash
This
is
a
string
```

#### （7）while 语句

while 循环用于不断执行一系列命令，也用于从输入文件中读取数据。其语法格式为：

```sh
while condition
do
    command
done
```

以下是一个基本的 while 循环，测试条件是：如果 int 小于等于 5，那么条件返回真。int 从 1 开始，每次循环处理时，int 加 1。运行上述脚本，返回数字 1 到 5，然后终止。

```sh
#!/bin/bash
int=1
while(( $int<=5 ))
do
    echo $int
    let "int++"
done
```

运行脚本，输出：

```bash
1
2
3
4
5
```

while循环可用于读取键盘信息。下面的例子中，输入信息被设置为变量FILM，按<Ctrl-D>结束循环。

```sh
echo '按下 <CTRL-D> 退出'
echo -n '输入你最喜欢的网站名: '
while read FILM
do
    echo "是的！$FILM 是一个好网站"
done
```

运行脚本，输出类似下面：

```bash
按下 <CTRL-D> 退出
输入你最喜欢的网站名:菜鸟教程
是的！菜鸟教程 是一个好网站
```

无限循环

```sh
while :
do
    command
done

# 或者

while true
do
    command
done

# 或者
for (( ; ; ))
```

#### （8）until 循环

until 循环执行一系列命令直至条件为 true 时停止。

until 循环与 while 循环在处理方式上刚好相反。一般 while 循环优于 until 循环，但在某些时候—也只是极少数情况下，until 循环更加有用。

until 语法格式:

```sh
until condition
do
    command
done
```

condition 一般为条件表达式，如果返回值为 false，则继续执行循环体内的语句，否则跳出循环。

以下实例我们使用 until 命令来输出 0 ~ 9 的数字：

```sh
#!/bin/bash

a=0

until [ ! $a -lt 10 ]
do
   echo $a
   a=`expr $a + 1`
done
```

输出结果为：

```bash
0
1
2
3
4
5
6
7
8
9
```

#### （9）case ... esac

**case ... esac** 为多选择语句，与其他语言中的 switch ... case 语句类似，是一种多分支选择结构，每个 case 分支用右圆括号开始，用两个分号 **;;** 表示 break，即执行结束，跳出整个 case ... esac 语句，esac（就是 case 反过来）作为结束标记。

可以用 case 语句匹配一个值与一个模式，如果匹配成功，执行相匹配的命令。

**case ... esac** 语法格式如下：

```sh
case 值 in
模式1)
    command1
    command2
    ...
    commandN
    ;;
模式2)
    command1
    command2
    ...
    commandN
    ;;
esac
```

case 工作方式如上所示，取值后面必须为单词 **in**，每一模式必须以右括号结束。取值可以为变量或常数，匹配发现取值符合某一模式后，其间所有命令开始执行直至 **;;**。

取值将检测匹配的每一个模式。一旦模式匹配，则执行完匹配模式相应命令后不再继续其他模式。如果无一匹配模式，使用星号 * 捕获该值，再执行后面的命令。

下面的脚本提示输入 1 到 4，与每一种模式进行匹配：

```sh
echo '输入 1 到 4 之间的数字:'
echo '你输入的数字为:'
read aNum
case $aNum in
    1)  echo '你选择了 1'
    ;;
    2)  echo '你选择了 2'
    ;;
    3)  echo '你选择了 3'
    ;;
    4)  echo '你选择了 4'
    ;;
    *)  echo '你没有输入 1 到 4 之间的数字'
    ;;
esac
```

输入不同的内容，会有不同的结果，例如：

```bash
输入 1 到 4 之间的数字:
你输入的数字为:
3
你选择了 3
```

#### （10）跳出循环

在循环过程中，有时候需要在未达到循环结束条件时强制跳出循环，Shell 使用两个命令来实现该功能：**break** 和 **continue**。

> break 命令

break 命令允许跳出所有循环（终止执行后面的所有循环）。

下面的例子中，脚本进入死循环直至用户输入数字大于5。要跳出这个循环，返回到shell提示符下，需要使用break命令。

```sh
#!/bin/bash
while :
do
    echo -n "输入 1 到 5 之间的数字:"
    read aNum
    case $aNum in
        1|2|3|4|5) echo "你输入的数字为 $aNum!"
        ;;
        *) echo "你输入的数字不是 1 到 5 之间的! 游戏结束"
            break
        ;;
    esac
done
```

执行以上代码，输出结果为：

```bash
输入 1 到 5 之间的数字:3
你输入的数字为 3!
输入 1 到 5 之间的数字:7
你输入的数字不是 1 到 5 之间的! 游戏结束
```

> continue

continue 命令与 break 命令类似，只有一点差别，它不会跳出所有循环，仅仅跳出当前循环。对上面的例子进行修改：

```sh
#!/bin/bash
while :
do
    echo -n "输入 1 到 5 之间的数字: "
    read aNum
    case $aNum in
        1|2|3|4|5) echo "你输入的数字为 $aNum!"
        ;;
        *) echo "你输入的数字不是 1 到 5 之间的!"
            continue
            echo "游戏结束"
        ;;
    esac
done
```

运行代码发现，当输入大于5的数字时，该例中的循环不会结束，语句 **echo "游戏结束"** 永远不会被执行。

### 12、Shell 函数

linux shell 可以用户定义函数，然后在shell脚本中可以随便调用。

shell中函数的定义格式如下：

```sh
[ function ] funname [()]

{

    action;

    [return int;]

}
```

说明：

- 1、可以带function fun() 定义，也可以直接fun() 定义,不带任何参数。
- 2、参数返回，可以显示加：return 返回，如果不加，将以最后一条命令运行结果，作为返回值。 return后跟数值n(0-255

> 下面的例子定义了一个函数并进行调用：

```sh
#!/bin/bash

demoFun(){
    echo "这是我的第一个 shell 函数!"
}
echo "-----函数开始执行-----"
demoFun
echo "-----函数执行完毕-----"
```

输出结果：

```bash
-----函数开始执行-----
这是我的第一个 shell 函数!
-----函数执行完毕-----
```

> 下面定义一个带有return语句的函数：

```sh
#!/bin/bash

funWithReturn(){
    echo "这个函数会对输入的两个数字进行相加运算..."
    echo "输入第一个数字: "
    read aNum
    echo "输入第二个数字: "
    read anotherNum
    echo "两个数字分别为 $aNum 和 $anotherNum !"
    return $(($aNum+$anotherNum))
}
funWithReturn
echo "输入的两个数字之和为 $? !"
```

输出类似下面：

```bash
这个函数会对输入的两个数字进行相加运算...
输入第一个数字: 
1
输入第二个数字: 
2
两个数字分别为 1 和 2 !
输入的两个数字之和为 3 !
```

函数返回值在调用该函数后通过 $? 来获得。

注意：所有函数在使用前必须定义。这意味着必须将函数放在脚本开始部分，直至shell解释器首次发现它时，才可以使用。调用函数仅使用其函数名即可。

> 函数参数

在Shell中，调用函数时可以向其传递参数。在函数体内部，通过 `$n` 的形式来获取参数的值，例如，`$1`表示第一个参数，`$2`表示第二个参数...

带参数的函数示例：

```sh
#!/bin/bash

funWithParam(){
    echo "第一个参数为 $1 !"
    echo "第二个参数为 $2 !"
    echo "第十个参数为 $10 !"
    echo "第十个参数为 ${10} !"
    echo "第十一个参数为 ${11} !"
    echo "参数总数有 $# 个!"
    echo "作为一个字符串输出所有参数 $* !"
}
funWithParam 1 2 3 4 5 6 7 8 9 34 73
```

输出结果：

```bash
第一个参数为 1 !
第二个参数为 2 !
第十个参数为 10 !
第十个参数为 34 !
第十一个参数为 73 !
参数总数有 11 个!
作为一个字符串输出所有参数 1 2 3 4 5 6 7 8 9 34 73 !
```

注意，`$10` 不能获取第十个参数，获取第十个参数需要`${10}`。当n>=10时，需要使用`${n}`来获取参数。

另外，还有几个特殊字符用来处理参数：

| 参数处理 | 说明                                                         |
| :------- | :----------------------------------------------------------- |
| $#       | 传递到脚本或函数的参数个数                                   |
| $*       | 以一个单字符串显示所有向脚本传递的参数                       |
| $$       | 脚本运行的当前进程ID号                                       |
| $!       | 后台运行的最后一个进程的ID号                                 |
| $@       | 与$*相同，但是使用时加引号，并在引号中返回每个参数。         |
| $-       | 显示Shell使用的当前选项，与set命令功能相同。                 |
| $?       | 显示最后命令的退出状态。0表示没有错误，其他任何值表明有错误。 |

### 13、Shell 输入/输出重定向

大多数 UNIX 系统命令从你的终端接受输入并将所产生的输出发送回到您的终端。一个命令通常从一个叫标准输入的地方读取输入，默认情况下，这恰好是你的终端。同样，一个命令通常将其输出写入到标准输出，默认情况下，这也是你的终端。

重定向命令列表如下：

| 命令            | 说明                                               |
| :-------------- | :------------------------------------------------- |
| command > file  | 将输出重定向到 file。                              |
| command < file  | 将输入重定向到 file。                              |
| command >> file | 将输出以追加的方式重定向到 file。                  |
| n > file        | 将文件描述符为 n 的文件重定向到 file。             |
| n >> file       | 将文件描述符为 n 的文件以追加的方式重定向到 file。 |
| n >& m          | 将输出文件 m 和 n 合并。                           |
| n <& m          | 将输入文件 m 和 n 合并。                           |
| << tag          | 将开始标记 tag 和结束标记 tag 之间的内容作为输入。 |

*需要注意的是文件描述符 0 通常是标准输入（STDIN），1 是标准输出（STDOUT），2 是标准错误输出（STDERR）。*

> 输出重定向

重定向一般通过在命令间插入特定的符号来实现。特别的，这些符号的语法如下所示:

```sh
command1 > file1
```

上面这个命令执行command1然后将输出的内容存入file1。

注意任何file1内的已经存在的内容将被新内容替代。如果要将新内容添加在文件末尾，请使用>>操作符。

执行下面的 who 命令，它将命令的完整的输出重定向在用户文件中(users):

```bash
$ who > users
```

执行后，并没有在终端输出信息，这是因为输出已被从默认的标准输出设备（终端）重定向到指定的文件。

你可以使用 cat 命令查看文件内容：

```bash
$ cat users
_mbsetupuser console  Oct 31 17:35 
tianqixin    console  Oct 31 17:35 
tianqixin    ttys000  Dec  1 11:33 
```

输出重定向会覆盖文件内容，请看下面的例子：

```bash
$ echo "菜鸟教程：www.runoob.com" > users
$ cat users
菜鸟教程：www.runoob.com
```

如果不希望文件内容被覆盖，可以使用 >> 追加到文件末尾，例如：

```bash
$ echo "菜鸟教程：www.runoob.com" >> users
$ cat users
菜鸟教程：www.runoob.com
菜鸟教程：www.runoob.com
```

> 输入重定向

和输出重定向一样，Unix 命令也可以从文件获取输入，语法为：

```sh
command1 < file1
```

这样，本来需要从键盘获取输入的命令会转移到文件读取内容。

注意：输出重定向是大于号(>)，输入重定向是小于号(<)。

接着以上实例，我们需要统计 users 文件的行数,执行以下命令：

```bash
$ wc -l users
       2 users
```

也可以将输入重定向到 users 文件：

```bash
$  wc -l < users
       2 
```

注意：上面两个例子的结果不同：第一个例子，会输出文件名；第二个不会，因为它仅仅知道从标准输入读取内容。

```sh
command1 < infile > outfile
```

同时替换输入和输出，执行command1，从文件infile读取内容，然后将输出写入到outfile中。

> 重定向深入讲解

一般情况下，每个 Unix/Linux 命令运行时都会打开三个文件：

- 标准输入文件(stdin)：stdin的文件描述符为0，Unix程序默认从stdin读取数据。
- 标准输出文件(stdout)：stdout 的文件描述符为1，Unix程序默认向stdout输出数据。
- 标准错误文件(stderr)：stderr的文件描述符为2，Unix程序会向stderr流中写入错误信息。

默认情况下，command > file 将 stdout 重定向到 file，command < file 将stdin 重定向到 file。

如果希望 stderr 重定向到 file，可以这样写：

```bash
$ command 2>file
```

如果希望 stderr 追加到 file 文件末尾，可以这样写：

```bash
$ command 2>>file
```

**2** 表示标准错误文件(stderr)。

如果希望将 stdout 和 stderr 合并后重定向到 file，可以这样写：

```bash
$ command > file 2>&1
或者
$ command >> file 2>&1
```

如果希望对 stdin 和 stdout 都重定向，可以这样写：

```bash
$ command < file1 >file2
```

command 命令将 stdin 重定向到 file1，将 stdout 重定向到 file2。

> Here Document

Here Document 是 Shell 中的一种特殊的重定向方式，用来将输入重定向到一个交互式 Shell 脚本或程序。

它的基本的形式如下：

```sh
command << delimiter
    document
delimiter
```

它的作用是将两个 delimiter 之间的内容(document) 作为输入传递给 command。

注意：

- 结尾的delimiter 一定要顶格写，前面不能有任何字符，后面也不能有任何字符，包括空格和 tab 缩进。
- 开始的delimiter前后的空格会被忽略掉。

在命令行中通过 **wc -l** 命令计算 Here Document 的行数：

```bash
$ wc -l << EOF
    欢迎来到
    菜鸟教程
    www.runoob.com
EOF
3          # 输出结果为 3 行
```

我们也可以将 Here Document 用在脚本中，例如：

```sh
#!/bin/bash

cat << EOF
欢迎来到
菜鸟教程
www.runoob.com
EOF
```

执行以上脚本，输出结果：

```bash
欢迎来到
菜鸟教程
www.runoob.com
```

> /dev/null 文件

如果希望执行某个命令，但又不希望在屏幕上显示输出结果，那么可以将输出重定向到 /dev/null：

```bash
$ command > /dev/null
```

/dev/null 是一个特殊的文件，写入到它的内容都会被丢弃；如果尝试从该文件读取内容，那么什么也读不到。但是 /dev/null 文件非常有用，将命令的输出重定向到它，会起到"禁止输出"的效果。

如果希望屏蔽 stdout 和 stderr，可以这样写：

```bash
$ command > /dev/null 2>&1
```

**注意：**0 是标准输入（STDIN），1 是标准输出（STDOUT），2 是标准错误输出（STDERR）。

这里的 **2** 和 **>** 之间不可以有空格，**2>** 是一体的时候才表示错误输出。

### 14、Shell 文件包含

和其他语言一样，Shell 也可以包含外部脚本。这样可以很方便的封装一些公用的代码作为一个独立的文件。

Shell 文件包含的语法格式如下：

```sh
. filename   # 注意点号(.)和文件名中间有一空格
或
source filename
```

创建两个 shell 脚本文件。

test1.sh 代码如下：

```sh
#!/bin/bash

url="http://www.runoob.com"
```

test2.sh 代码如下：

```sh
#!/bin/bash

#使用 . 号来引用test1.sh 文件
. ./test1.sh

# 或者使用以下包含文件代码
# source ./test1.sh

echo "菜鸟教程官网地址：$url"
```

接下来，我们为 test2.sh 添加可执行权限并执行：

```bash
$ chmod +x test2.sh 
$ ./test2.sh 
菜鸟教程官网地址：http://www.runoob.com
```

注：被包含的文件 test1.sh 不需要可执行权限。
