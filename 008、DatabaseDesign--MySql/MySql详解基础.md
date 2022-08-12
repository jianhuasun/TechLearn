# MySql详解基础

## 一、简介

### 1、概念

是现在**流行**的**开源**的,**免费**的**关系型**数据库。

### 2、特点

- 免费 , 开源数据库
- 小巧 , 功能齐全
- 使用便捷
- 可运行于Windows或Linux操作系统
- 可适用于中小型甚至大型网站应用

### 3、参考网站

**官网** **:** [https://www.mysql.com/](https://www.mysql.com/)

**参考网站**：[https://www.runoob.com/mysql/mysql-tutorial.html](https://www.runoob.com/mysql/mysql-tutorial.html)

### 4、MySQL分支

![image-20220807104722045](http://cdn.bluecusliyou.com/202208071047196.png)

### 5、MySQL的起源

　　MySQL数据库的历史可以追溯到1979年，那时Bill Gates退学没多久，微软公司也才刚刚起步，而Larry Ellison的Oracle公司也才成立不久。那个时候有一个天才程序员Monty Widenius为一个名为TcX的小公司打工，并且用BASIC设计了一个报表工具，使其可以在4MHz主频和16KB内存的计算机上运行。没过多久，Monty又将此工具用C语言进行了重新编写并移植到了UNIX平台上。当时，这只是一个很底层且仅面向报表的存储引擎，名叫UNIREG。最初的UNIREG是运行在瑞典人制造的ABC800计算机上的。ABC800的内存只有32KB，CPU是频率只有4MHz的Z80。在1983年Monty Widenius遇到了David Axmark，两人相见恨晚，开始合作运营TcX，Monty Widenius负责技术，David Axmark负责搞管理。后来TcX将UNIREG移植到其他更加强大的硬件平台，主要是Sun的平台。虽然TcX这个小公司资源有限，但Monty Widenius天赋极高，面对资源有限的不利条件，反而更能发挥他的潜能。Monty Widenius总是力图写出最高效的代码，并因此养成了习惯。与Monty Widenius在一起的还有一些别的同事，很少有人能坚持把那些代码持续写到20年后，而Monty Widenius却做到了。

　　1990年，一次Monty接到了一个项目，客户需要为当时的UNIREG提供更加通用的SQL接口，当时有人提议直接使用商用数据库，但是Monty Widenius觉得商用数据库的速度难以令人满意。于是Monty Widenius找到了David Hughes(mSQL的发明人)商讨合作事宜。想借助于mSQL的代码，将它集成到自己的存储引擎中。然而令人失望的是，在经过一番测试后，他们发现mSQL的速度并不尽如人意，无法满足客户的需求。于是Monty Widenius雄心大起，决心自己重写一个SQL支持。从此MySQL就开始诞生了。

　　MySQL命名的由来：Monty Widenius有一个女儿，名叫My Widenius，因此他将自己开发的数据库命名为MySQL。Monty还有一个儿子，名为Max，因此在2003年，SAP公司与MySQL公司建立合作伙伴关系后，Monty Widenius又将与SAP合作开发的数据库命名为MaxDB。而现在的MariaDB中的Maria便是Monty Widenius的小孙女的名字。

　　（MaxDB：MaxDB是一种企业级数据库管理系统(DBMS)，以前称为SAPDB，是著名的企业管理软件供应商SAP公司的自有数据库技术，并由SAP公司开发和支持。2003年，SAP AG和MySQL AB确立了合作伙伴关系，并将数据库系统重命名为MaxDB。自此以后，MaxDB的开发一直由SAP开发者团队负责，MaxDB是能够承受高负载的开源数据库，它适合于OLAP和OLTP应用，并能提供高可靠性、可用性、扩展性和非常完善的特性集。）

　　（MariaDB：MariaDB数据库管理系统是MySQL的一个分支，主要由开源社区在维护，采用GPL授权许可。MariaDB的目的是完全兼容MySQL，包括API和命令行，使之能轻松成为MySQL的代替品。在存储引擎方面，使用XtraDB来代替MySQL的InnoDB。）

　　（MySQL的小海豚标志名叫：sakila(塞拉)，它是由MySQL AB的创始人从用户在“海豚命名”的竞赛中建议的大量的名字表中选出的。获胜的名字是由来自非洲斯威士兰的开源软件开发者Ambrose Twebaze提供的。根据Ambrose所说，Sakila来自一种叫SiSwati的斯威士兰方言，也是在Ambrose的家乡乌干达附近的坦桑尼亚的Arusha的一个小镇的名字）

![img](http://cdn.bluecusliyou.com/202208071043487.png)

### 6、MySQL的历史

　　1995年5月23日，MySQL的第一个内部版本发行了。

　　1996年10月，MySQL 3.11.1发布(MySQL没有2.x版本)，有趣的是，第一个MySQL正式版恰巧只能运行在Sun Solaris上，仿佛昭示了它日后被Sun收购的命运。一个月后，Linux版本出现了。在接下来的两年里，MySQL被依次移植到各个平台，同时加入了不少新的特性。在发布时，MySQL数据库采用的许可策略有些与众不同：允许免费使用，但是不能将MySQL与自己的产品绑定在一起发布。如果想一起发布，就必须使用特殊许可，意味着要花“银子”。当然，商业支持也是需要花“银子”的。其他方面，随用户怎么用都可以。这种特殊许可为MySQL带来了一些收入，从而为它的持续发展打下了良好的基础。

　　1998年1月，MySQL关系型数据库发行了第一个版本。它使用系统核心的多线程机制提供完全的多线程运行模式，并提供了面向C、C++、Eiffel、Java、Perl、PHP、Python及Tcl等编程语言的编程接口(API)，且支持多种字段类型，并且提供了完整的操作符支持。而且MySQL已经能够运行在10多种操作系统之上，其中包括应用非常广泛的 FreeBSD、Linux、Windows 95和Windows NT等。很快MySQL 3.22也发布了，但它仍然存在很多问题--如不支持事务操作、子查询、外键、存储过程和视图等功能。正因为这些缺陷，当时许多Oracle和SQL Server的用户对MySQL根本不屑一顾。

　　1999-2000年，MySQL AB公司在瑞典成立。Monty Widenius雇了几个人与Sleepycat合作，开发出了Berkeley DB引擎, 因为BDB支持事务处理，从此MySQL开始支持事务处理了。

　　2000年4月，MySQL对旧的存储引擎ISAM进行了整理，将其命名为MyISAM。

　　2001年，Heikki Tuuri向MySQL提出建议，希望能集成他的存储引擎InnoDB，这个引擎同样支持事务处理，还支持行级锁。该引擎之后被证明是最为成功的MySQL事务存储引擎。

　　2003年12月，MySQL 5.0版本发布，提供了视图、存储过程等功能。

　　2008年1月，MySQL AB公司被Sun公司以10亿美金收购，MySQL数据库进入Sun时代。在Sun时代，Sun公司对其进行了大量的推广、优化、Bug修复等工作。

　　2008年11月，MySQL 5.1发布，它提供了分区、事件管理，以及基于行的复制和基于磁盘的NDB集群系统，同时修复了大量的Bug。

　　2009年4月20日，Oracle公司以74亿美元收购Sun公司，自此MySQL数据库进入Oracle时代，而其第三方的存储引擎InnoDB早在2005年就被Oracle公司收购。

　　2010年12月，MySQL 5.5发布，其主要新特性包括半同步的复制及对SIGNAL/RESIGNAL的异常处理功能的支持，最重要的是InnoDB存储引擎终于变为当前MySQL的默认存储引擎。MySQL 5.5不是时隔两年后的一次简单的版本更新，而是加强了MySQL各个方面在企业级的特性。Oracle公司同时也承诺MySQL 5.5和未来版本仍是采用GPL授权的开源产品。

　　2013年2月，MySQL5.6发布。Oracle最近宣布将于2021年2月停止5.6版本的更新，结束其生命周期。

　　2015年12月，MySQL5.7发布，其性能、新特性、性能分析带来了质的改变。

　　2016年9月，MySQL开始了8.0版本，Oracle宣称该版本速度是5.7的两倍，性能更好。

　　2018年4月，MySQL8.0.11发布。

## 二、安装

### 1、Navicat安装

#### （1）navicat简介

navicat是一款强大的数据库可视化操作工具，支持各种不同的数据库，支持不同的平台。

官网地址：[http://www.navicat.com.cn/](http://www.navicat.com.cn/)

文档地址：[http://www.navicat.com.cn/support/online-manual](http://www.navicat.com.cn/support/online-manual)

#### （2）navicat安装

navicat premium 15 科学安装教程：[https://blog.csdn.net/weixin_51560103/article/details/120894983](https://blog.csdn.net/weixin_51560103/article/details/120894983)

#### （3）navicat运行命令行

这里可以开启命令行输入，和在服务器上直接登录效果是一样的

![image-20220627203702810](http://cdn.bluecusliyou.com/202206272037978.png)

工具->选项->常规 可以设置背景色

![image-20220725070531741](http://cdn.bluecusliyou.com/202207250705906.png)

### 2、windows安装mysql

#### （1）下载程序

> 下载地址：[https://downloads.mysql.com/archives/community/](https://downloads.mysql.com/archives/community/)
>
> 百度网盘连接送上：[https://pan.baidu.com/s/1dJwIwT_q4wdBUNDx_llIfg?pwd=1234](https://pan.baidu.com/s/1dJwIwT_q4wdBUNDx_llIfg?pwd=1234)

![image-20220622072257340](http://cdn.bluecusliyou.com/202206220723494.png)

#### （2）解压并配置环境变量

> 获取到解压路径：D:\developtools\mysql-8.0.28-winx64

![cb8b09ed8fd247848c28349e50866446](http://cdn.bluecusliyou.com/202208101708686.png)

> 将安装包路径配置到环境变量中
>

![](http://cdn.bluecusliyou.com/202206220747336.png)

![image-20220622074925633](http://cdn.bluecusliyou.com/202206220749686.png)

![image-20220622075106984](http://cdn.bluecusliyou.com/202206220751041.png)

#### （3）在程序根目录创建my.ini配置文件

> 配置文件参数根目录basedir和数据目录datadir要和mysql解压的目录一致

```ini
[mysqld]
# 设置3306端口
port=3306
# 设置mysql的安装目录
basedir=D:/developtools/mysql-8.0.28-winx64
# 设置mysql数据库的数据的存放目录
datadir=D:/developtools/mysql-8.0.28-winx64/data
# 允许最大连接数
max_connections=200
# 允许连接失败的次数。这是为了防止有人从该主机试图攻击数据库系统
max_connect_errors=10
# 服务端使用的字符集默认为UTF8
character-set-server = UTF8MB4
# 创建新表时将使用的默认存储引擎
default-storage-engine=INNODB
# 默认使用“mysql_native_password”插件认证
default_authentication_plugin=mysql_native_password
[mysql]
# 设置mysql客户端默认字符集
default-character-set=utf8mb4
[client]
# 设置mysql客户端连接服务端时默认使用的端口
port=3306
default-character-set=utf8mb4
```

#### （4）mysql初始化操作，记录下临时密码，登录用（管理员身份）

> 初始化MySQL，运行完成之后会生成一个临时密码，mysql目录下会生成data文件夹

```bash
mysqld --defaults-file=D:\developtools\mysql-8.0.28-winx64\my.ini --initialize –-console
```

![image-20220624221834186](http://cdn.bluecusliyou.com/202206242218275.png)

#### （5）启动MySQL服务

> 安装mysql服务，services.msc打开服务列表，看到已经安装成功

```bash
mysqld --install mysql8
```

![image-20220624222035512](http://cdn.bluecusliyou.com/202206242220556.png)

> 启动MySQL服务，查看服务已经启动

```bash
net start mysql8
```

![image-20220624222318376](http://cdn.bluecusliyou.com/202206242223424.png)

#### （6）通过临时密码登录MYSQL并修改密码

> 登录MySQL，输入上面安装服务时候生成的临时密码，注意-p后面不能有空格
>
> 使用上面方式无法登陆的解决方案
> 1、停止mysql8 net stop mysql8
>
> 2、无密码启动 mysqld --console --skip-grant-tables --shared-memory
>
> 3、前面窗口不能关闭，在开启列外一个新窗口进行无密码登陆 mysql -u root -p 
>
> 4、清空密码 update mysql.user set authentication_string='' where user='root' and
> host='localhost'
>
> 5、刷新 FLUSH privileges; 
>
> 6、重新启动mysql服务，在以无密码登陆mysql

```bash
mysql -u root -p?OL)dOdiH46x                  --登录mysql
alter user root@localhost identified by '123';--修改密码
```

#### （7）开启远程访问

```bash
CREATE USER 'root'@'%' IDENTIFIED BY '123'; --创建一个用户
GRANT ALL ON *.* TO 'root'@'%';             --将权限授予创建的用户
flush privileges;                           --保存
```

#### （8）连接成功

> 图形界面连接

![image-20220625075245176](http://cdn.bluecusliyou.com/202206250752281.png)

> 命令行连接
>
> 注意 : -p后面不能加空格,否则会被当做密码的内容,导致登录失败 !

```bash
## mysql -h 服务器主机地址 -u 用户名 -p 用户密码
## 连接完成退出 exit
```

### 3、Linux安装mysql

#### （1）linux详解

linux相关知识请参考[linux详解](https://blog.csdn.net/liyou123456789/article/details/121548156)

#### （2）下载程序

> 下载地址：[https://downloads.mysql.com/archives/community/](https://downloads.mysql.com/archives/community/)
>
> 百度网盘连接送上：[https://pan.baidu.com/s/1xt3fe42dnMHcEibsGU0c9A?pwd=1234](https://pan.baidu.com/s/1xt3fe42dnMHcEibsGU0c9A?pwd=1234) 
>
> 选择自己系统的版本包下载，我安装的linux系统是centos7.8

#### （3）上传到linux服务器，并配置文件权限

![image-20220625151234327](http://cdn.bluecusliyou.com/202206251512386.png)

> 解压上传的tar.gz

```bash
tar -zxvf mysql-8.0.28-el7-x86_64.tar.gz
```

> 把解压的文件移动到`/usr/local`目录下面

```bash
mv mysql-8.0.28-el7-x86_64 /usr/local/mysql
```

> 添加MySQL组和用户(默认会添加，没有添加就自己添加)

```bash
groupadd mysql
useradd -r -g mysql mysql
```

> 进入/usr/local/mysql目录下，修改相关权限

```C#
cd /usr/local/mysql
chown -R mysql:mysql ./
```

#### （4）mysql初始化操作，记录下临时密码，登录用

> 有的系统会缺少东西，所以报
>
> error while loading shared libraries: libaio.so.1: cannot open shared object file: No such file or directory
>
> 就运行命令yum install libaio*安装一下

```bash
cd /usr/local/mysql/bin
./mysqld --initialize --user=mysql --basedir=/usr/local/mysql --datadir=/usr/local/mysql/data

rp*Jp=+GO1jj
```

#### （5）创建MySQL配置文件/etc/my.cnf

```ini
[mysqld]
port=3306
basedir=/usr/local/mysql
datadir=/usr/local/mysql/data
max_connections=200
max_connect_errors=10
character-set-server = UTF8MB4
default-storage-engine=INNODB
default_authentication_plugin=mysql_native_password
[mysql]
default-character-set=utf8mb4
[client]
port=3306
default-character-set=utf8mb4
```

#### （6）启动MySQL服务

```C#
cd /usr/local/mysql/support-files
./mysql.server start
```

#### （7）通过临时密码登录MYSQL并修改密码

```bash
cd /usr/local/mysql/bin
./mysql -u root -p*Jp=+GO1jj
alter user 'root'@'localhost' identified by '123';
```

#### （8）开启远程访问

```bash
CREATE USER 'root'@'%' IDENTIFIED BY '123';
GRANT ALL ON *.* TO 'root'@'%';
flush privileges;
```

#### （9）连接成功

### 4、docker安装mysql

#### （1）docker详解

docker相关知识请参考[docker详解](https://blog.csdn.net/liyou123456789/article/details/122292877)

#### （2）linux系统安装docker

#### （3）运行mysql容器

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

#### （4）连接成功

## 三、权限管理

### 1、MySQL权限

权限系统的作用是授予来自**某个主机**的**某个用户**可以查询、插入、修改、删除等数据库操作的权限。

- 权限控制(授权与回收)的执行语句包括create user, grant, revoke。
- 授权后的权限都会存放在MySQL的内部数据库中（数据库名叫mysql）,并在数据库启动之后把权限信息复制到内存中
- MySQL用户的认证信息不光包括用户名，还要包含连接发起的主机

### 2、权限级别详解

- MySQL权限级别：

  - 全局性的管理权限，作用于整个MySQL实例级别.

  - 数据库级别的权限，作用于某个指定的数据库上或者所有的数据库上.

  - 数据库对象级别的权限，作用于指定的数据库对象上（表、视图等）或者 所有的数据库对象上.

- 权限存储在`mysql库`的user, db, tables_priv, columns_priv, and procs_priv这几个系统表中，待MySQL实例启动后就加载到内存中。

  - User表：存放用户账户信息以及全局级别（所有数据库）权限，决定了来自哪些主机的哪些用户可以访问数据库实例，如果有全局权限则意味着对所有数据库都有此权限。

  - Db表：存放数据库级别的权限，决定了来自哪些主机的哪些用户可以访问此数据库。

  - Tables_priv表：存放表级别的权限，决定了来自哪些主机的哪些用户可以访问数据库的这个表。

  - Columns_priv表：存放列级别的权限，决定了来自哪些主机的哪些用户可以访问数据库表的这个字段。

  - Procs_priv表：存放存储过程和函数级别的权限

```sql
-- 查看root用户权限
show grants for root@localhost;
```

### 3、权限项解释

```sql
ALL [PRIVILEGES] -- 设置除GRANT OPTION之外的所有简单权限 
ALTER -- 允许使用ALTER TABLE 
ALTER ROUTINE -- 更改或取消已存储的子程序 
CREATE -- 允许使用CREATE TABLE 
CREATE ROUTINE -- 创建已存储的子程序 
CREATE TEMPORARY TABLES -- 允许使用CREATE TEMPORARY TABLE 
CREATE USER -- 允许使用CREATE USER, DROP USER, RENAME USER和REVOKE ALL PRIVILEGES。 
CREATE VIEW -- 允许使用
CREATE VIEW DELETE -- 允许使用
DELETE DROP -- 允许使用
DROP TABLE EXECUTE -- 允许用户运行已存储的子程序
FILE -- 允许使用SELECT...INTO OUTFILE和LOAD DATA INFILE 
INDEX -- 允许使用CREATE INDEX和DROP INDEX 
INSERT -- 允许使用INSERT 
LOCK TABLES -- 允许对您拥有SELECT权限的表使用
LOCK TABLES PROCESS -- 允许使用SHOW FULL PROCESSLIST 
REFERENCES -- 未被实施 
RELOAD -- 允许使用FLUSH 
REPLICATION CLIENT -- 允许用户询问从属服务器或主服务器的地址 
REPLICATION SLAVE -- 用于复制型从属服务器（从主服务器中读取二进制日志事件）
SELECT -- 允许使用SELECT 
SHOW DATABASES -- 显示所有数据库 
SHOW VIEW -- 允许使用SHOW CREATE VIEW 
SHUTDOWN -- 允许使用mysqladmin shutdown 
SUPER -- 允许使用CHANGE MASTER, KILL, PURGE MASTER LOGS和SET GLOBAL语句， mysqladmin debug命令；允许您连接（一次），即使已达到max_connections。 
UPDATE -- 允许使用UPDATE 
USAGE -- “无权限”的同义词 
GRANT OPTION -- 允许授予权限
```

### 4、命令管理权限

#### （1）查看用户权限

```sql
SHOW GRANTS; 
SHOW GRANTS FOR CURRENT_USER; 
SHOW GRANTS FOR CURRENT_USER();
SHOW GRANTS FOR '用户名'@'%';
```

#### （2）创建用户

```sql
/* 用户和权限管理 */ 
-- 用户信息表：mysql.user 

-- 增加用户 CREATE USER ly IDENTIFIED BY '123456'   相当于在user表添加一行记录
CREATE USER 用户名 IDENTIFIED BY [PASSWORD] 密码(字符串) 
-- 必须拥有mysql数据库的全局CREATE USER权限，或拥有INSERT权限。 
-- 只能创建用户，不能赋予权限。 
-- 用户名，注意引号：如 'user_name'@'192.168.1.1' 
-- 密码也需引号，纯数字密码也要加引号 
-- 要在纯文本中指定密码，需忽略PASSWORD关键词。要把密码指定为由PASSWORD()函数返回的 混编值，需包含关键字PASSWORD

-- 重命名用户 RENAME USER ly TO ly2 
RENAME USER old_user TO new_user 

-- 设置密码 
SET PASSWORD = PASSWORD('密码') -- 为当前用户设置密码 
SET PASSWORD FOR 用户名 = PASSWORD('密码') -- 为指定用户设置密码

-- 删除用户 DROP USER ly2 
DROP USER 用户名
```

#### （3）给用户授权

```sql
-- 分配权限/添加用户
grant 权限 on 目标 to 用户名 (identified by 密码) (with grant option)
-- 权限： all privilege代表所有权限，或者其他权限，比如增删改查：insert、delete、update、select
-- 目标： '.' 代表作用于整个mysql实例，也可以作用于数据级别、或者表级别等
-- 用户名： 这里需要和Host一块写，格式为 xxx@xxx，如果没有特殊符号，可以不加单引号，但是如果有特殊服务，必须加单引号
-- indentified by 设置密码，当指令中带着 identified by xxx 的时候，会先创建该用户再授权，如果该用户已经存在，则直接授权
-- with grant option 允许授权和回收

-- 刷新权限 
FLUSH PRIVILEGES 
```

**mysql权限的生效规则**

- 执行Grant,revoke,set password,rename user命令修改权限之后，MySQL会自动将修改后的权限信息同步加载到系统内存中
- 如果执行insert/update/delete操作上述的系统权限表之后，则必须再执行刷新权限命令才能同步到系统内存中，刷新权限命令包括：flush privileges / mysqladmin flush-privileges / mysqladmin reload
- 如果是修改tables和columns级别的权限，则客户端的下次操作新权限就会生效
- 如果是修改database级别的权限，则新权限在客户端执行use database命令后生效
- 如果是修改global级别的权限，则需要重新创建连接新权限才能生效
- --skip-grant-tables可以跳过所有系统权限表而允许所有用户登录，只在特殊情况下暂时使用

#### （4）允许远程访问

允许某个已经存在的用户可以远程访问mysql服务器，**本质就是修改User表中的Host字段，比如改为 %**，代表允许所有地址访问即可。

```sql
-- 方式一、grant允许所有ip访问
grant all privileges on *.* to '用户名'@'%' identified by '123456' with grant option;
flush privileges; 

-- 方式二、直接update所有ip
update user set host = '%' where user = '用户名'; 
flush privileges; 
```

#### （5）收回用户权限

```sql
-- 撤消权限
REVOKE 权限列表 ON 表名 FROM 用户名
REVOKE ALL PRIVILEGES, GRANT OPTION FROM 用户名 -- 撤销所有权限
```

#### （6）设置密码过期策略

> 全局配置，作用于所有密码，在mysql配置文件中做如下配置

```ini
default_password_lifetime=180  设置180天过期
default_password_lifetime=0  设置密码不过期
```

> 为每个用户设置过期策略，会覆盖上面的全局配置

```sql
-- 设置密码过期时间为90天
ALTER USER '用户名'@'%' PASSWORD EXPIRE INTERVAL 90 DAY;
-- 设置密码永不过期
ALTER USER '用户名'@'%' PASSWORD EXPIRE NEVER; 
-- 设置密码为默认过期策略
ALTER USER '用户名'@'%' PASSWORD EXPIRE DEFAULT; 
-- 设置密码马上过期
ALTER USER '用户名'@'%' PASSWORD EXPIRE;
```

#### （7）设置用户资源限制

**如果要取消限制，将对应参数的值改为0即可**

```sql
-- 每小时最大查询数为10，每小时最大更新数为20，每小时最大连接数为30，最大用户连接数为40
ALTER USER 用户名@'%' WITH
MAX_QUERIES_PER_HOUR 10
MAX_UPDATES_PER_HOUR 20
MAX_CONNECTIONS_PER_HOUR 30
MAX_USER_CONNECTIONS 40;
```

#### （8）锁定用户

**用户锁定后，则不能登录mysql**

```sql
-- 默认创建用户名是不带锁的
create user abc2@localhost identified by '123456';
-- 创建用户名的时候加锁
create user abc2@localhost identified by '123456' account lock;
-- 加锁
alter user 'mysql.sys'@localhost account lock;
-- 解锁
alter user 'mysql.sys'@localhost account unlock;
```

### 5、Navicat界面权限

#### （1）新建用户

![image-20220807142848713](http://cdn.bluecusliyou.com/202208071428806.png)

#### （2）配置用户信息

用户名、哪些地址可以访问、密码、密码过期策略。

![image-20220807143007225](http://cdn.bluecusliyou.com/202208071430316.png)

#### （3）配置访问次数

这里一般不做特殊配置，保持默认即可，也就是不限制。

![image-20220807143057787](http://cdn.bluecusliyou.com/202208071430885.png)

#### （4）配置服务器权限

这里配置的权限是针对整个MySQL实例而言的。比如配置 增删改查 权限。

![image-20220807143155528](http://cdn.bluecusliyou.com/202208071431617.png)

#### （5）配置详细的权限

这里指配置 数据库级别、表级别、列级别、存储过程级别等的权限，比如配置 表级别的权限。

![image-20220807143818412](http://cdn.bluecusliyou.com/202208071438525.png)

#### （6）SQL预览

SQL预览可以看到对应的SQL语句

![image-20220807144318622](http://cdn.bluecusliyou.com/202208071443709.png)

## 四、数据库操作

### 1、创建数据库

```sql
CREATE DATABASE db_name DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
```

### 2、查询数据库

#### （1）查询所有数据库

```sql
SHOW DATABASES;
```

#### （2）查询数据库建表时的sql

```sql
SHOW CREATE DATABASE db_name;
```

### 3、删除数据库

```sql
DROP DATABASE db_name;
```

### 4、修改数据库

```sql
-- 修改数据库的字符编码和排序方式
ALTER DATABASE db_name DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
```

### 5、选择数据库

```sql
USE db_name;
```

### 6、其他规则

- 可用反引号（`）为标识符（库名、表名、字段名、索引、别名）包裹，以避免与关键字重名！中文 也可以作为标识符！ 
- 每个库目录存在一个保存当前数据库的选项文件db.opt。 
- 注释： 单行注释 # 注释内容 多行注释 /* 注释内容 */ 单行注释 -- 注释内容 (标准SQL注释风格，要求双破折号后加一空格符（空格、TAB、 换行等）) 
- 模式通配符： _ 任意单个字符 % 任意多个字符，甚至包括零字符 单引号需要进行转义 \' 
- CMD命令行内的语句结束符可以为 ";", "\G", "\g"，仅影响显示结果。其他地方还是用分号结 束。delimiter 可修改当前对话的语句结束符。 
- SQL对大小写不敏感 （关键字）
- 清除已有语句：\c

### 7、字符集和排序规则

#### （1）字符集

MySQL提供了多种字符集和排序规则选择，其中字符集设置和数据存储以及客户端与MySQL实例的交互相关，排序规则和字符串的对比规则相关

- 字符集的设置可以在MySQL实例、数据库、表、列四个级别
- MySQL设置字符集支持在InnoDB, MyISAM, Memory三个存储引擎
- 查看当前MySQL支持的字符集的方式有两种，一种是通过查看information_schema.character_set系统表，一种是通过命令【 show character set; 】查看。

```sql
mysql> show character set;
+----------+---------------------------------+---------------------+--------+
| Charset  | Description                     | Default collation   | Maxlen |
+----------+---------------------------------+---------------------+--------+
| armscii8 | ARMSCII-8 Armenian              | armscii8_general_ci |      1 |
| ascii    | US ASCII                        | ascii_general_ci    |      1 |
| big5     | Big5 Traditional Chinese        | big5_chinese_ci     |      2 |
| binary   | Binary pseudo charset           | binary              |      1 |
| cp1250   | Windows Central European        | cp1250_general_ci   |      1 |
| cp1251   | Windows Cyrillic                | cp1251_general_ci   |      1 |
| cp1256   | Windows Arabic                  | cp1256_general_ci   |      1 |
| cp1257   | Windows Baltic                  | cp1257_general_ci   |      1 |
| cp850    | DOS West European               | cp850_general_ci    |      1 |
| cp852    | DOS Central European            | cp852_general_ci    |      1 |
| cp866    | DOS Russian                     | cp866_general_ci    |      1 |
| cp932    | SJIS for Windows Japanese       | cp932_japanese_ci   |      2 |
| dec8     | DEC West European               | dec8_swedish_ci     |      1 |
| eucjpms  | UJIS for Windows Japanese       | eucjpms_japanese_ci |      3 |
| euckr    | EUC-KR Korean                   | euckr_korean_ci     |      2 |
| gb18030  | China National Standard GB18030 | gb18030_chinese_ci  |      4 |
| gb2312   | GB2312 Simplified Chinese       | gb2312_chinese_ci   |      2 |
| gbk      | GBK Simplified Chinese          | gbk_chinese_ci      |      2 |
...
```

#### （2）排序规则

每个指定的字符集都会有一个或多个支持的排序规则，可以通过两种方式查看，一种是查看information_schema.collations表，另一种是通过【show collation】命令查看

- 当仅指定了字符集而没有指定排序规则时，则会使用该字符集的默认排序规则
- 当仅指定了排序规则而没有字符集时，则在该排序规则名称上含有的字符集会被使用
- 当数据库创建时没有指定这两项，则使用实例级别的字符集和排序规则

```sql
mysql> show collation where charset ='utf8mb4';
+----------------------------+---------+-----+---------+----------+---------+---------------+
| Collation                  | Charset | Id  | Default | Compiled | Sortlen | Pad_attribute |
+----------------------------+---------+-----+---------+----------+---------+---------------+
| utf8mb4_0900_ai_ci         | utf8mb4 | 255 | Yes     | Yes      |       0 | NO PAD        |
| utf8mb4_0900_as_ci         | utf8mb4 | 305 |         | Yes      |       0 | NO PAD        |
| utf8mb4_0900_as_cs         | utf8mb4 | 278 |         | Yes      |       0 | NO PAD        |
| utf8mb4_0900_bin           | utf8mb4 | 309 |         | Yes      |       1 | NO PAD        |
| utf8mb4_bin                | utf8mb4 |  46 |         | Yes      |       1 | PAD SPACE     |
| utf8mb4_croatian_ci        | utf8mb4 | 245 |         | Yes      |       8 | PAD SPACE     |
| utf8mb4_cs_0900_ai_ci      | utf8mb4 | 266 |         | Yes      |       0 | NO PAD        |
| utf8mb4_cs_0900_as_cs      | utf8mb4 | 289 |         | Yes      |       0 | NO PAD 
...
```

#### （3）查看数据库编码

```sql
mysql> show variables like '%character%';
+--------------------------+--------------------------------+
| Variable_name            | Value                          |
+--------------------------+--------------------------------+
| character_set_client     | utf8mb4                        |
| character_set_connection | utf8mb4                        |
| character_set_database   | utf8mb3                        |
| character_set_filesystem | binary                         |
| character_set_results    | utf8mb4                        |
| character_set_server     | utf8mb4                        |
| character_set_system     | utf8mb3                        |
| character_sets_dir       | /usr/share/mysql-8.0/charsets/ |
+--------------------------+--------------------------------+
```

- 每个数据库客户端连接都有自己的字符集和排序规则属性，客户端发送的语句的字符集是由character_set_client决定，而与服务端交互时会根据character_set_connection和collation_connection两个参数将接收到的语句转化。当涉及到显示字符串的比较时，由collation_connection参数决定，而当比较的是字段里的字符串时则根据字段本身的排序规则决定
- character_set_result 参数决定了语句的执行结果以什么字符集返回给客户端
- 客户端可以很方便的调整字符集和排序规则，比如使用SET NAMES'charset_name' [COLLATE 'collation_name']表明后续的语句都以该字符集格式传送给服务端，而执行结果也以此字符集格式返回。

```ini
SET character_set_client = utf8mb4;
SET character_set_results = utf8mb4;
SET character_set_connection = utf8mb4;
```

## 五、数据库表操作

### 1、创建表

#### （1）语法

```sql
CREATE TABLE tb_name (建表的字段、类型、长度、约束、默认、注释)
```

#### （2）约束

- 非空	NOT NULL		指定列在插入数据时候必须有值
- 非负	UNSIGNED 		插入数字不能是负数
- 主键	PRIMARY KEY		这个列值必须是唯一，不能重复
- 自增	AUTO_INCREMENT		只应用于整型的主键列
- 默认	DEFAULT		指定列默认值
- 注释	COMMENT		说明字段
- 0填充的  ZEROFILL  不足位数的用0来填充 , 如int(3),5则为005

#### （3）常用类型

> 数值类型

![image-20220725155350399](http://cdn.bluecusliyou.com/202207251553584.png)

> 字符串类型

![image-20220725155416014](http://cdn.bluecusliyou.com/202207251554156.png)

> 日期和时间型数值类型

![image-20220725161144972](http://cdn.bluecusliyou.com/202207251611491.png)

> NULL值

- 理解为 "没有值" 或 "未知值"
- 不要用NULL进行算术运算 , 结果仍为NULL

#### （4）案例

```sql
##判断数据库存在就删除
drop database if exists testdb;
##创建数据库
create database testdb;
##使用数据库
use testdb;
##判断创建表是否存在  见名知意 tb开头就是表 v开头就是视图
drop table if exists tb_user;
##创建表
create table tb_user
(
    user_id int auto_increment primary key comment '用户id',
	user_name varchar(30) not null,
	user_birthday date,
	user_gender char(3),
	user_state tinyint(1) not null,
	user_desc text
);
```

### 2、表字段修改

#### （1）添加表字段

```sql
##添加表字段语法：ALERT TABLE tb_name ADD 添加字段 字段类型 <约束,默认,注释>
alter table tb_user add user_phone varchar(11) not null comment '用户电话';
```

#### （2）修改表字段类型

```sql
##修改表字段类型语法：ALERT TABLE tb_name MODIFY 字段名称 新的字段类型 <约束,默认,注释>
alter table tb_user modify user_phone int(11) not null comment '用户电话';
```

#### （3）修改字段名称

```sql
##修改字段名称语法：ALTER TABLE tb_name CHANGE 旧的字段名 新的字段名 新的类型 <约束,默认,注释>
alter table tb_user change user_phone tel_phone varchar(11) not null comment '座机';
```

#### （4）显示表字段

```sql
##显示表字段语法：desc tb_name;
desc tb_user;
```

#### （5）删除表字段

```sql
##字段删除语法：ALTER TABLE tb_name DROP 删除的字段名
alter table tb_user drop tel_phone; 
```

### 3、表修改

#### （1）表名修改

```sql
##修改表名语法：ALTER TABLE 旧表名 RENAME TO 新表名
alter table tb_user rename t_user;
```

#### （2）引擎修改

```sql
##修改表引擎语法：ALTER TABLE 表名 ENGINE = 新的引擎名称
alter table tb_user engine=myisam;
```

### 4、删除表

> drop删除数据无法恢复，慎重

```sql
##删除表语法：drop table tb_name；
drop table tb_user;
```

### 5、查询表

#### （1）查询所有表

```sql
show tables;
```

#### （2）查询建表的sql语句

```sql
show create table tb_user;
```

### 6、外键

#### （1）外键概念

如果公共关键字在一个关系中是主关键字，那么这个公共关键字被称为另一个关系的外键。由此可见，外键表示了两个关系之间的相关联系。以另一个关系的外键作主关键字的表被称为**主表**，具有此外键的表被称为主表的**从表**。

在实际操作中，将一个表的值放入第二个表来表示关联，所使用的值是第一个表的主键值(在必要时可包括复合主键值)。此时，第二个表中保存这些值的属性称为外键(**foreign key**)。

**外键作用**

保持数据**一致性**，**完整性**，主要目的是控制存储在外键表中的数据,**约束**。 使两张表形成关联，外键只能引用外表中的列的值或使用空值。

#### （2）创建外键

> 建表时指定外键约束

```sql
-- 年级表 (id\年级名称) 
CREATE TABLE `grade` ( 
	`gradeid` INT ( 10 ) NOT NULL AUTO_INCREMENT COMMENT '年级ID', 
	`gradename` VARCHAR ( 50 ) NOT NULL COMMENT '年级名称', 
	PRIMARY KEY ( `gradeid` ) 
) 
ENGINE = INNODB DEFAULT CHARSET = utf8;

-- 学生信息表 (学号,姓名,性别,年级,手机,地址,出生日期,邮箱,身份证号)
CREATE TABLE `stu` (
	`studentno` INT ( 4 ) NOT NULL COMMENT '学号',
	`studentname` VARCHAR ( 20 ) NOT NULL DEFAULT '匿名' COMMENT '姓名',
	`sex` TINYINT ( 1 ) DEFAULT '1' COMMENT '性别',
	`gradeid` INT ( 10 ) DEFAULT NULL COMMENT '年级',
	`phoneNum` VARCHAR ( 50 ) NOT NULL COMMENT '手机',
	`address` VARCHAR ( 255 ) DEFAULT NULL COMMENT '地址',
	`borndate` DATETIME DEFAULT NULL COMMENT '生日',
	`email` VARCHAR ( 50 ) DEFAULT NULL COMMENT '邮箱',
	`idCard` VARCHAR ( 18 ) DEFAULT NULL COMMENT '身份证号',
	PRIMARY KEY ( `studentno` ),
	KEY `FK_gradeid` ( `gradeid` ),
CONSTRAINT `FK_gradeid` FOREIGN KEY ( `gradeid` ) REFERENCES `grade` ( `gradeid` ) 
) ENGINE = INNODB DEFAULT CHARSET = utf8;
```

> 建表后修改

```sql
ALTER TABLE `stu` ADD CONSTRAINT `FK_gradeid` FOREIGN KEY (`gradeid`) REFERENCES `grade` (`gradeid`);
```

> 删除外键
>
> 删除具有主外键关系的表时 , 要先删子表 , 后删主表，直接删除主表报错

```sql
-- 删除外键 
ALTER TABLE stu DROP FOREIGN KEY FK_gradeid; 
-- 发现执行完上面的,索引还在,所以还要删除索引 
-- 注:这个索引是建立外键的时候默认生成的 
ALTER TABLE stu DROP INDEX FK_gradeid;
```

## 六、表数据操作

### 1、数据准备

```sql
create table t_user
(
    user_id int auto_increment primary key comment '用户id',
	user_name varchar(30) not null,
	user_birthday date,
	user_gender char(3),
	user_state tinyint(1) not null,
	user_desc text
);
```

### 2、DML语言

DML语言（Data Manipulation Language）：数据操作语言，用于操作数据库对象中所包含的数据，包括 :

- INSERT (添加数据语句)
- UPDATE (更新数据语句) 
- DELETE (删除数据语句) 

### 3、增加表数据

#### （1）语法

- 字段或值之间用英文逗号隔开
- ' 字段1,字段2...' 该部分可省略 , 但添加的值务必与表结构,数据列,顺序相对应,且数量一致
- 可同时插入多条数据 , values 后用英文逗号隔开

```sql
INSERT INTO 表名[(字段1,字段2,字段3,...)] VALUES('值1','值2','值3')
```

#### （2）增加单条数据

```sql
##Demo1: 向`testdb`中的`t_user`表插入一条数据(针对所有字段而言)
insert into t_user (user_name,user_birthday,user_gender,user_state,user_height,user_decribe) values ('haha','2017-09-05','男',1,174.7,'haha');
##简写: (前提插入的数据对应表中所有的字段)
insert into t_user values (2,'haha2','2017-09-05','男',1,174.7,'haha');
##Demo2:  向`testdb`中的`t_user`表插入指定列的值【注意指定列插入数据前提是其他列没有非空的约束】
insert into t_user (user_name,user_state,user_height,user_decribe) values ('tom',1,178.2,'tomcat');
```

#### （3）增加多条数据

```sql
INSERT INTO tb_name(`field1`,`field2`,....)VALUES('value1','value2',.....),('value1','value2',.....),('value1','value2',.....),....;
```

### 4、修改表数据

#### （1）语法

- column_name 为要更改的数据列
- value 为修改后的数据 , 可以为变量 , 具体指 , 表达式或者嵌套的SELECT结果
- condition 为筛选条件 , 如不指定则修改该表的所有列数据

```sql
UPDATE 表名 SET column_name=value [,column_name2=value2,...] [WHERE condition];
```

#### （2）案例

```sql
##Demo1：修改t_user表中的生日字段
update t_user set user_birthday='2000-10-20' where user_id = 1;
##Demo2：修改t_user表user_id为2的 身高 176.3，状态字段 2
update t_user set user_height = 176.3,user_state=2 where user_id=2;
##Demo3: 修改t_user表中user_id 大于 1的 user_state字段的值为7
update t_user set user_state = 7 where user_id > 1;
##Demo4: 如果修改数据的时候不加条件，就会导致所有数据都会被更改
update t_user set user_state = 5;
```

### 5、删除表数据

#### （1）delete语法

```sql
delete from 表名 where 删除的条件
```

#### （2）delete案例

```sql
##Demo1: 删除user_id为3的数据
delete from t_user where user_id = 3;
##Demo2: 删除性别为女，状态为5的用户信息 (多个条件同事满足就删除)
delete from t_user where user_gender='女' and user_state=5;
##Demo3： 删除user_state为10或者性别为男的记录
delete from t_user where user_state = 10 or user_gender='男';
```

#### （3）truncate语法

```sql
truncate table tb_name;
```

#### （4）truncate和delete区别

- truncate table：只能删除表中全部数据；delete from table where……,可以删除表中全部数据,也可以删除部分数据。 
- delete from记录是一条条删的，所删除的每行记录都会进日志,而truncate一次性删掉整个页,因此日志里面只记录页的释放。   
- truncate的执行速度比delete快。
- delete执行后，删除的数据占用的存储空间还在，还可以恢复数据；truncate删除的数据占用的存储空间不在，不可以恢复数据。因此truncate删除后，不能回滚，delete可以回滚。
- 使用truncate重新设置AUTO_INCREMENT计数器。

### 6、DQL语言

DQL( Data Query Language)：数据查询语言

- 查询数据库数据 , 如**SELECT**语句
- 简单的单表查询或多表的复杂查询和嵌套查询
- 是数据库语言中最核心,最重要的语句
- 使用频率最高的语句

```sql
SELECT [ALL | DISTINCT] 
{* | table.* | [table.field1[as alias1][,table.field2[as alias2]][,...]]} 
FROM table_name [as table_alias] 
[left | right | inner join table_name2] -- 联合查询 
[WHERE ...] -- 指定结果需满足的条件 
[GROUP BY ...] -- 指定结果按照哪几个字段来分组 
[HAVING] -- 过滤分组的记录必须满足的次要条件 
[ORDER BY ...] -- 指定查询记录按一个或多个条件排序 
[LIMIT {[offset,]row_count | row_countOFFSET offset}];-- 指定查询的记录从哪条至哪条
```

### 7、指定查询字段

#### （1）指定查询字段

```sql
-- 查询所有信息
select * from t_user; 

-- 查询指定字段
select user_id,user_name from t_user; 
```

#### （2）AS 子句作为别名

- 可给数据列取一个新别名
- 可给表取一个新别名
- 可把经计算或总结的结果用另一个新名称来代替

```sql
select u.user_id as id,CONCAT(u.user_name,'test') as test from tb_user as u;
```

#### （3）distinct去重

作用 : 去掉SELECT查询返回的记录结果中重复的记录，只返回一条所有列的值都相同数据

```sql
select distinct user_id,user_name from tb_user; 
```

#### （4）使用表达式的列

数据库中的表达式 : 一般由文本值 , 列值 , NULL , 函数和操作符等组成

- SELECT语句返回结果列中使用
- SELECT语句中的ORDER BY , HAVING等子句中使用
- DML语句中的 where 条件语句中使用表达式
- 避免SQL返回结果中包含 ' . ' , ' * ' 和括号等干扰开发语言程序

```sql
SELECT @@auto_increment_increment; -- 查询自增步长 
SELECT VERSION(); -- 查询版本号 
SELECT 100*3-1 AS 计算结果; -- 表达式
```

### 8、where条件查询

作用：用于检索数据表中 符合条件 的记录

搜索条件可由一个或多个逻辑表达式组成 , 结果一般为真或假. 

![image-20220727151943027](http://cdn.bluecusliyou.com/202207271519293.png)

> 比较运算符，大于、小于、等于、不等于、大于等于、小于等于

```sql
-- 比较运算 > < >= <=,!=,<>,=
select * from t_user where user_gender='男'; 
select * from t_user where user_id >= 4; 
select * from t_user where user_state <= 1;
-- 不等于
select * from t_user where user_state != 1; 
select * from t_user where user_state <> 1; 
select * from t_user where user_name != 'Lilly'; 
select * from t_user where user_name <> 'Lilly'; 
```

> 逻辑运算符，逻辑运算符是用来拼接其他条件的。用and或者or来连接两个条件，如果用or来连接的时候必须使用小括号

```sql
-- 逻辑运算符[and, or]
select * from t_user where user_gender='男' and user_name='gerry'; 
select * from t_user where user_gender='男' or user_name='gerry'; 
```

> LIKE模糊查询-通配符
>
> - %（百分号）匹配零个或者多个任意字符
>- _（下划线）匹配一个任意字符

```sql
-- 模糊匹配 【like "_"占位，"%"代表通配符】 
select * from t_user where user_name like 'han%'; 
select * from t_user where user_name like '_han%'; 
select * from t_user where user_name like '%hanmei%'; 
```

> IN字段指定多个值查询

```sql
select * from t_user where user_id in (5,6,9); 
-- 查询结果等价于下面的结果集
select * from t_user where user_id = 5 UNION ALL
select * from t_user where user_id = 6 UNION ALL 
select * from t_user where user_id = 9 
```

> BETWEEN AND 区间查询

```sql
-- between and 查询 
select * from t_user where user_id between 6 and 9; 
select * from t_user where user_id >=6 and user_id <=9;
```

> IN子查询
>
> 什么是子查询? 
>
> - 在查询语句中的WHERE条件子句中,又嵌套了另一个查询语句 
> - 嵌套查询可由多个子查询组成,求解的方式是由里及外; 
> - 子查询返回的结果一般都是集合,故而建议使用IN关键字; 

```sql
select * from t_user where user_id in
(select user_id from t_user where user_id where user_id>9)
```

### 9、group by 分组查询

> 配合函数
>
> - count(field)获取符合条件出现的非null值的次数
> - sum(field)获取所有符合条件的数据的总和
> - avg(field)或者平均值

```sql
-- 统计所有用户信息中男女的平均身高
-- 使用group by注意在查询中出现字段必须是group by后面字段
select user_gender as 性别,avg(user_height) 平均身高,sum(user_height) 总身高,count(user_gender) 总数 from t_user group by user_gender;
```

> having 分组之后的条件，一般和group by联合使用

```sql
select user_gender,count(*) as 性别统计 from t_user group by user_gender having 性别统计>1;
```

### 10、order by排序查询

> ORDER BY field DESC;降序查询
>
> ORDER BY field ASC;升序查询（默认的排序）可以省略不写

```sql
select * from t_user order by user_id asc;
select * from t_user order by user_id desc;
-- order by 放置在group by的后面
select user_gender as 性别,avg(user_height) 平均身高,sum(user_height) 总身高,count(user_gender) 总数 from t_user group by user_gender order by 总身高 desc;
```

### 11、limit分页语句

> LIMIT 后边可以跟两个参数，如果只写一个表示从零开始查询指定长度，如果两个参数就是从第一个参数开始查询查询长度是第二个参数的值，俩个参数必须是整形。
>
> 分页查询一般会全表扫描，优化的目的应尽可能减少扫描；第一种思路：在索引上完成排序分页的操作，最后根据主键关联回原表查询原来所需要的其他列。这种思路是使用覆盖索引尽快定位出需要的记录的id，覆盖索引效率高些第二中思路：limit m,n 转换为 n之前分页查询是传pageNo页码, pageSize分页数量，当前页的最后一行对应的id即last_row_id，以及pageSize，这样先根据条件过滤掉last_row_id之前的数据，然后再取n挑记录,此种方式只能用于排序字段不重复唯一的列，如果用于重复的列，那么分页数据将不准确

```sql
-- limit查询
create table t_score
(
	id int primary key auto_increment,
	stu_id int not null,
	cou_id int not null,
	score decimal(4,1) not null
);
-- 插入测试数据
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (1, 1, 89.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (1, 2, 78.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (1, 3, 94.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (1, 4, 77.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (1, 5, 99.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (3, 1, 90.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (3, 2, 88.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (3, 3, 69.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (3, 4, 83.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (3, 5, 92.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (2, 1, 77.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (2, 2, 84.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (2, 3, 91.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (2, 4, 80.0);
INSERT INTO `testdb`.`t_score`(`stu_id`, `cou_id`, `score`) VALUES (2, 5, 99.0);
-- 查询科目id为1的最高成绩
select max(score) from t_score where cou_id = 1;
select * from t_score where cou_id = 1 order by score desc limit 1;
-- 查询课程id为4的前五名成绩信息
select * from t_score where cou_id = 4 order by score desc limit 5;
-- 使用limit做分页 0 = (当前页 - 1) * 每页显示条数
select * from t_score limit 0,4;
-- 8 代表的是下条记录从什么地方开始往下查询，4 表示查询的长度。
select * from t_score limit 8,4;  
```

### 12、连接查询

#### （1）连接查询

![image-20220727153124494](http://cdn.bluecusliyou.com/202207271531599.png)

```sql
-- 准备关联查询需要使用的表
create table t_dept
(
	dept_id int primary key,
	dept_name varchar(30) not null
);
create table t_emp 
(
	emp_id int primary key,
	emp_name varchar(20) not null,
	emp_salary DECIMAL(5,1) not null,
	dept_id int not null
);
-- 插入测试数据
insert into t_dept values (10, '研发部'),(20, '市场部'),(30, '销售部');
insert into t_emp values(1,'zhangsan1',5550,10);
insert into t_emp values(2,'zhangsan2',5550,10);
insert into t_emp values(3,'zhangsan3',5550,10);
insert into t_emp values(4,'wangwu1',3000,20);
insert into t_emp values(5,'wangwu2',3000,20);
insert into t_emp values(6,'lisi',3433,30);
```

```sql
-- 左关联(查询的数据是根据主表中数据限制的，哪怕是没有主表中对应部门的员工信息也会关联空的数据（from后面的是主表）
select * from t_dept d left join t_emp e on d.dept_id = e.dept_id
-- 中关联(查询存在关联的数据，不存在关联信息自动去掉)
select * from t_dept d join t_emp e on d.dept_id = e.dept_id
-- 右关联（join 后面的表是主表）
select * from t_dept d right join t_emp e on e.dept_id=d.dept_id 
-- 内关联
select * from t_dept e,t_emp d where e.dept_id=d.dept_id
```

#### （2）联合查询

> UNION用于把两个或者多个select查询的结果集合并成一个
>
> UNION = UNION DISTINCT，去重，UNION ALL不去掉结果集中重复的行
>
> - 进行合并的两个查询，其SELECT列表必须在数量和对应列的数据类型上保持一致；
>
> - 默认会去掉两个查询结果集中的重复行；默认结果集不排序；
>
> - 最终结果集的列名来自于第一个查询的SELECT列表
> - 在去重操作时，如果列值中包含NULL值，认为它们是相等的
> - 如果要对合并后的整个结果集进行排序，ORDER BY子句只能出现在最后面的查询中

```sql
-- [UNION 排除重复的数据集，UNION ALL 不会排除重复的数据]
select * from t_emp where dept_id in (10,20)
UNION ALL
select * from t_emp where dept_id in (20,30);
```

#### （3）七种JOIN理论

![image-20220710145156300](http://cdn.bluecusliyou.com/202207101451443.png)

数据准备：

```sql
-- 创建表
CREATE TABLE `tbl_emp` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(22) DEFAULT NULL,
  `deptId` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE tbl_dep (
	id INT ( 11 ) NOT NULL AUTO_INCREMENT,
	deptName VARCHAR ( 22 ) DEFAULT NULL,
	addr VARCHAR ( 22 ) DEFAULT NULL,
PRIMARY KEY ( id ) 
) ENGINE = INNODB DEFAULT CHARSET = utf8;

-- 插入数据
INSERT INTO tbl_dep(deptName,addr) VALUES('hr','111');
INSERT INTO tbl_dep(deptName,addr) VALUES('bd','112');
INSERT INTO tbl_dep(deptName,addr) VALUES('vb','113');
INSERT INTO tbl_dep(deptName,addr) VALUES('sd','114');
INSERT INTO tbl_dep(deptName,addr) VALUES('yy','115');
 
INSERT INTO tbl_emp(`name`,deptId) VALUES('k8',1); 
INSERT INTO tbl_emp(`name`,deptId) VALUES('k6',2); 
INSERT INTO tbl_emp(`name`,deptId) VALUES('k4',3); 
INSERT INTO tbl_emp(`name`,deptId) VALUES('k4',11);
```

其中join

```sql
-- 内连接(两表的共有部分)
SELECT * FROM tbl_dep d INNER JOIN tbl_emp e ON d.id=e.deptId;

-- 左连接（左表的全部，右表不满足补NULL）
SELECT * FROM tbl_dep d LEFT JOIN tbl_emp e ON d.id=e.deptId;

-- 右连接（右表的全部，左表不满足的补NULL）
SELECT * FROM tbl_dep d RIGHT JOIN tbl_emp e ON d.id=e.deptId;

-- 特殊的左连接，（显示为左表的独有的数据）
-- 说明：查询tbl_dep 表中跟tbl_emp 表无关联关系的数据，即tbl_dep 独占，且tbl_emp 表的显示列补NULL；
SELECT * FROM tbl_dep d LEFT JOIN tbl_emp e ON d.id=e.deptId WHERE e.deptId IS NULL;

-- 特殊的右连接（显示为右表的独有的数据 ）
-- 说明：查询tbl_emp 表中跟tbl_dep 表无关联关系的数据，即tbl_emp 独占，且tbl_dep 表的显示列补NULL；
SELECT * FROM tbl_dep d RIGHT JOIN tbl_emp e ON d.id=e.deptId WHERE d.id IS NULL;

-- 全连接（显示全部数据）（mysql 不支持 full outer join）
-- UNION ：有去重的功能。
SELECT * FROM tbl_dep d LEFT JOIN tbl_emp e ON d.id=e.deptId UNION
SELECT * FROM tbl_dep d RIGHT JOIN tbl_emp e ON d.id=e.deptId;

-- 显示两表的独有的数据
SELECT * FROM tbl_dep d LEFT JOIN tbl_emp e ON d.id=e.deptId WHERE e.deptId IS NULL UNION
SELECT * FROM tbl_dep d RIGHT JOIN tbl_emp e ON d.id=e.deptId WHERE d.id IS NULL;
```

## 七、常用对象

### 1、视图

#### （1）什么是视图

- MySQL 从 5.0 开始就提供了视图功能，下面我们对视图功能进行介绍。

- 视图的英文名称是 view，它是一种虚拟存在的表。视图对于用户来说是透明的，它并不在数据库中实际存在，视图是使用数据库行和列动态组成的表。


#### （2）视图的优势

视图相对于普通的表来说，优势包含下面这几项

- 使用视图可以简化操作：使用视图我们不用关注表结构的定义，我们可以把经常使用的数据集合定义成视图，这样能够简化操作。

- 安全性：用户对视图不可以随意的更改和删除，可以保证数据的安全性。
- 数据独立性：一旦视图的结构 确定了， 可以屏蔽表结构变化对用户的影响， 数据库表增加列对视图没有影响；具有一定的独立性

#### （3）增删改查视图

```sql
-- 创建表
create table product(id int(11),name varchar(20),price float(10,2));
-- 插入数据
insert into product values(1, "apple","3.5"),(2,"banana","4.2"),(3,"melon","1.2");
-- 创建视图
create or replace view v1 as select * from product;
-- 查询视图
mysql> select *  from v1;
+----+--------+-------+
| id | name   | price |
+----+--------+-------+
|  1 | apple  |  3.50 |
|  2 | banana |  4.20 |
|  3 | melon  |  1.20 |
+----+--------+-------+
-- 查看视图结构
mysql> describe v1;
+-------+-------------+------+-----+---------+-------+
| Field | Type        | Null | Key | Default | Extra |
+-------+-------------+------+-----+---------+-------+
| id    | int         | YES  |     | NULL    |       |
| name  | varchar(20) | YES  |     | NULL    |       |
| price | float(10,2) | YES  |     | NULL    |       |
+-------+-------------+------+-----+---------+-------+
-- 删除视图
drop view v1;
```

### 2、存储过程

#### （1）什么是存储过程

- MySQL 从 5.0 开始起就支持存储过程和函数了。

- **存储过程是在数据库系统中完成一组特定功能的 SQL 语句集**，它存储在数据库系统中，一次编译后永久有效。

#### （2）存储过程的优劣

**优点：**

- 使用存储过程具有可封装性，能够隐藏复杂的 SQL 逻辑。
- 存储过程可以接收参数，并返回结果
- 存储过程性能非常高，一般用于批量执行语句

**缺点：**

- 存储过程编写复杂
- 存储过程对数据库的依赖性比较强，可移植性比较差

#### （3）增删改查存储过程

```sql
-- delimiter 用于自定义结束符
-- 创建存储过程
delimiter $$
create procedure sp_product()
begin
  select * from product;
end $$
-- 执行存储过程
mysql> call sp_product();
+----+--------+-------+
| id | name   | price |
+----+--------+-------+
|  1 | apple  |  3.50 |
|  2 | banana |  4.20 |
|  3 | melon  |  1.20 |
+----+--------+-------+
-- 删除存储过程
drop procedure sp_product;
-- 创建带参数存储过程
delimiter $$
create procedure sp_product(in p_id int)
begin
  select * from product where id=p_id;
end $$
-- 执行存储过程
mysql> call sp_product(2);
+----+--------+-------+
| id | name   | price |
+----+--------+-------+
|  2 | banana |  4.20 |
+----+--------+-------+
-- 查看存储过程定义
show create procedure sp_product;
```

### 3、触发器

#### （1）什么是触发器

- MySQL 从 5.0 开始支持触发器，触发器一般作用在表上，在满足定义条件时触发，并执行触发器中定义的语句集合，下面我们就来一起认识一下触发器。

- 举个例子来认识一下触发器：比如你有一个日志表和金额表，你每录入一笔金额就要进行日志表的记录，你会怎么样？同时在金额表和日志表插入数据吗？如果有了触发器，你可以直接在金额表录入数据，日志表会自动插入一条日志记录，当然，触发器不仅只有新增操作，还有更新和删除操作。


#### （2）触发器作用

- 在添加一条数据前，检查数据是否合理，例如检查邮件格式是否正确
- 删除数据后，相当于数据备份的作用
- 可以记录数据库的操作日志，也可以作为表的执行轨迹

#### （3）增删改查触发器

```sql
-- 语法
create trigger triggername triggertime triggerevent on tbname for each row triggerstmt
```

- triggername：这个指的就是触发器的名字
- triggertime：这个指的就是触发器触发时机，是 BEFORE 还是 AFTER
- triggerevent: 这个指的就是触发器触发事件，一共有三种事件：INSERT、UPDATE 或者 DELETE。
- tbname：这个参数指的是触发器创建的表名，在哪个表上创建
- triggerstmt: 触发器的程序体，也就是 SQL 语句
-  `for each now` 表示任何一条记录上的操作都会触发触发器。

可以创建六种触发器

**BEFORE INSERT、AFTER INSERT、BEFORE UPDATE、AFTER UPDATE、BEFORE DELETE、AFTER DELETE**

```sql
-- 创建表
create table product_info(p_info varchar(20));
-- 创建触发器
delimiter $$
create trigger tg_pinfo after insert
on product for each row
begin
insert into product_info values("after insert product");
end $$
-- 表插入数据
insert into product values(4,"pineapple",15.3);
-- 操作表多了一行记录
mysql> select *  from product;
+----+-----------+-------+
| id | name      | price |
+----+-----------+-------+
|  1 | apple     |  3.50 |
|  2 | banana    |  4.20 |
|  3 | melon     |  1.20 |
|  4 | pineapple | 15.30 |
+----+-----------+-------+
-- 触发器也写入成功
mysql> select *  from product_info;
+----------------------+
| p_info               |
+----------------------+
| after insert product |
+----------------------+
-- 查看触发器定义
show triggers;
select *  from information_schema.triggers;
--  删除触发器
drop trigger tg_pinfo;
```

## 八、常用函数

### 1、数值函数

用来处理很多数值方面的运算，使用数值函数，可以免去很多繁杂的判断求值的过程，能够大大提高用户的工作效率。

#### （1）ABS(x)：返回 x 的绝对值

```sql
mysql> select abs(-0.8),abs(0.8);
+-----------+----------+
| abs(-0.8) | abs(0.8) |
+-----------+----------+
|       0.8 |      0.8 |
+-----------+----------+
```

#### （2）CEIL(x)：返回不小于 x 的最小整数

> 也就是说得大于或等于x的最小整数，向上取整，同义词：ceiling(x)

```sql
mysql> select ceil(1);
+---------+
| ceil(1) |
+---------+
|       1 |
+---------+

mysql> select ceil(1.23),ceiling(-1.23);
+------------+----------------+
| ceil(1.23) | ceiling(-1.23) |
+------------+----------------+
|          2 |             -1 |
+------------+----------------+
```

#### （3）FLOOR(x)：返回不大于 x 的最大整数

> 与CEIL的用法刚好相反，向下取整

```sql
mysql> select floor(1.23),floor(-1.23);
+-------------+--------------+
| floor(1.23) | floor(-1.23) |
+-------------+--------------+
|           1 |           -2 |
+-------------+--------------+
```

#### （4）MOD(x，y)：返回数字x除以y后的余数

> 和 x%y 的结果相同；和x mod y的结果相同；
>
> 模数和被模数任何一个为NULL(无效数)结果都为 NULL；
>
> 余数可以有小数；除数为0不抛出异常；

```sql
mysql> select mod(123,10),234%7,3 mod 2;
+-------------+-------+---------+
| mod(123,10) | 234%7 | 3 mod 2 |
+-------------+-------+---------+
|           3 |     3 |       1 |
+-------------+-------+---------+
mysql> select mod(3.14,3),mod(3,0);
+-------------+----------+
| mod(3.14,3) | mod(3,0) |
+-------------+----------+
|        0.14 |     NULL |
+-------------+----------+
```

#### （5）ROUND(X[,D])：将数字X四舍五入到指定的小数位数D

> 如果不指定D，则默认为0
>
> 如果D是负数，表示从小数点的左边进行四舍五入

```sql
mysql> select round(1.58),round(1.298,1);
+-------------+----------------+
| round(1.58) | round(1.298,1) |
+-------------+----------------+
|           2 |            1.3 |
+-------------+----------------+
mysql> select round(1.58,0),round(1.298,-1);
+---------------+-----------------+
| round(1.58,0) | round(1.298,-1) |
+---------------+-----------------+
|             2 |               0 |
+---------------+-----------------+
```

#### （6）TRUNCATE(X,D)：将数字X截断到指定的小数位数D（不四舍五入）

> 如果D为0，表示不要小数
>
> 如果D是负数，表示从小数点的左边进行截断
>
> TRUNCATE 和 ROUND 的区别在于 TRUNCATE 仅仅是截断，而不进行四舍五入

```sql
mysql> select truncate(1.999,1),truncate(1.999,0);
+-------------------+-------------------+
| truncate(1.999,1) | truncate(1.999,0) |
+-------------------+-------------------+
|               1.9 |                 1 |
+-------------------+-------------------+

mysql> select truncate(-1.999,1),truncate(123,-2);
+--------------------+------------------+
| truncate(-1.999,1) | truncate(123,-2) |
+--------------------+------------------+
|               -1.9 |              100 |
+--------------------+------------------+
mysql> select round(1.235,2),truncate(1.235,2);
+----------------+-------------------+
| round(1.235,2) | truncate(1.235,2) |
+----------------+-------------------+
|           1.24 |              1.23 |
+----------------+-------------------+
```

#### （7）RAND()：返回一个随机浮点数v(0<=v<1.0)

```sql
mysql> select rand(),rand();
+--------------------+---------------------+
| rand()             | rand()              |
+--------------------+---------------------+
| 0.7085628693071779 | 0.19879874978102627 |
+--------------------+---------------------+
```

> RAND(x)：指定整数x，则用作种子值，产生一个可重复的数字序列

```sql
mysql> select rand(1),rand(2),rand(1);
+---------------------+--------------------+---------------------+
| rand(1)             | rand(2)            | rand(1)             |
+---------------------+--------------------+---------------------+
| 0.40540353712197724 | 0.6555866465490187 | 0.40540353712197724 |
+---------------------+--------------------+---------------------+　　
```

> 利用RAND()函数可以取任意指定范围内的随机数
>
> 比如：产生 0～100 内的任意随机整数
>
> 若要得到一个随机整数R，i <= R < j，可以用FLOOR(i + RAND() * (j - i))
>
> 比如：取随机整数R，7<=R<12，select floor(7+(rand()*5));

```sql
mysql> select ceil(100*rand()),ceil(100*rand());
+------------------+------------------+
| ceil(100*rand()) | ceil(100*rand()) |
+------------------+------------------+
|               87 |               75 |
+------------------+------------------+
```

> - 当在 WHERE 子句中使用RAND()时，每次当WHERE执行时都要重新计算 RAND()
>
>
> - 可以以随机的顺序从表中检索行，例如：SELECT * FROM players ORDER BY RAND();
>
>
> - ORDER BY RAND()常和LIMIT子句一起使用，例如：SELECT * FROM table1,table2 WHERE a=b AND c<d ORDER BY RAND() LIMIT 1000;
>

### 2、分组聚合函数

> 分组SELECT的基本格式：select [聚合函数] 字段名 from 表名[where 查询条件] [group by 字段名] [having 过滤条件]

> 聚合函数（aggregation function），也就是组函数，在一个行的集合（一组行）上进行操作，对每个组给一个结果。
>
> - 每个组函数接收一个参数，默认情况下，组函数忽略列值为null的行，不参与计算
>
> - 当使用组函数的select语句中没有group by子句时，中间结果集中的所有行自动形成一组，然后计算组函数；
>
> - 组函数不允许嵌套，例如：count(max(…))；
>
> - 组函数的参数可以是列或是函数表达式；
>
> - 一个SELECT子句中可出现多个聚集函数。

常用的聚合函数：

| [AVG([distinct\] expr)](https://www.cnblogs.com/geaozhang/p/6745147.html#sum-avg) | 求平均值     |
| ------------------------------------------------------------ | ------------ |
| [COUNT({*\|[distinct\] } expr)](https://www.cnblogs.com/geaozhang/p/6745147.html#count) | 统计行的数量 |
| [MAX([distinct\] expr)](https://www.cnblogs.com/geaozhang/p/6745147.html#max-min) | 求最大值     |
| [MIN([distinct\] expr)](https://www.cnblogs.com/geaozhang/p/6745147.html#max-min) | 求最小值     |
| [SUM([distinct\] expr)](https://www.cnblogs.com/geaozhang/p/6745147.html#sum-avg) | 求累加和     |

```sql
-- 准备实验表
create table salary_tab
(
	userId int primary key auto_increment,
	salary DECIMAL(5,1)
);
insert into salary_tab(salary) values(1000),(2000),(3000),(NULL),(1000);

mysql> select * from salary_tab;
+--------+---------+
| userid | salary  |
+--------+---------+
|      1 | 1000.00 |
|      2 | 2000.00 |
|      3 | 3000.00 |
|      4 |    NULL |
|      5 | 1000.00 |
+--------+---------+
# 统计数据条数
select count(userId) 总记录数 from salary_tab;
# 计算表中薪资总和
select sum(salary) as 薪资总和 from salary_tab;
# 计算平均薪资
select avg(salary) 平均薪资 from salary_tab;
# 计算最高薪资和最低薪资
select max(salary) 最高薪资,min(salary) 最低薪资 from salary_tab;
# 聚合函数不能直接嵌套
# select max(sum(salary)) from salary_tab;
```

#### （1）count函数

> count(*)：返回表中满足where条件的行的数量

```sql
mysql> select count(*) from salary_tab where salary='1000';
+----------+
| count(*) |
+----------+
|        2 |
+----------+
mysql> select count(*) from salary_tab;　　#没有条件，默认统计表数据行数
+----------+
| count(*) |
+----------+
|        5 |
+----------+
```

> count(列)：返回列值非空的行的数量

```sql
mysql> select count(salary) from salary_tab;
+---------------+
| count(salary) |
+---------------+
|             4 |
+---------------+
```

> count(distinct 列)：返回列值非空的、并且列值不重复的行的数量

```sql
mysql> select count(distinct salary) from salary_tab;
+------------------------+
| count(distinct salary) |
+------------------------+
|                      3 |
+------------------------+
```

> count(expr)：根据表达式统计数据

```sql
-- 准备实验表
create table tt
(
   unit varchar(10),
	 date date
);
insert into tt values
('a','2022-04-03'),
('a','2022-06-27'),
('b','2022-01-01'),
('b','2022-06-27'),
('c','2022-06-06'),
('d','2022-03-03');
mysql> select * from tt;
+------+------------+
| unit | date       |
+------+------------+
| a    | 2022-04-03 |
| a    | 2022-06-27 |
| b    | 2022-01-01 |
| b    | 2022-06-27 |
| c    | 2022-06-06 |
| d    | 2022-03-03 |
+------+------------+

mysql> select UNIT as '单位',COUNT(TO_DAYS(DATE)=TO_DAYS(NOW()) or null) as '今日统计',COUNT(YEAR(DATE)=YEAR(NOW()) or null) as '今年统计' from tt group by UNIT;
+------+----------+----------+
| 单位 | 今日统计 | 今年统计 |
+------+----------+----------+
| a    |        1 |        2 |
| b    |        1 |        2 |
| c    |        0 |        1 |
| d    |        0 |        1 |
+------+----------+----------+
```

#### （2）max和min函数

> 统计列中的最大最小值
>
> 注意：如果统计的列中只有NULL值，那么MAX和MIN就返回NULL

```sql
mysql> select max(salary) from salary_tab;
+-------------+
| max(salary) |
+-------------+
|     3000.00 |
+-------------+

mysql> select min(salary) from salary_tab;
+-------------+
| min(salary) |
+-------------+
|     1000.00 |
+-------------+
```

#### （3）sum和avg函数

> 求和与求平均
>
> 注意：表中列值为null的行不参与计算，要想列值为NULL的行也参与组函数的计算，必须使用IFNULL函数对NULL值做转换。

```sql
mysql> select sum(salary) from salary_tab;
+-------------+
| sum(salary) |
+-------------+
|     7000.00 |
+-------------+

mysql> select avg(salary) from salary_tab;
+-------------+
| avg(salary) |
+-------------+
| 1750.000000 |
+-------------+

mysql> select avg(ifnull(salary,0)) from salary_tab;
+-----------------------+
| avg(ifnull(salary,0)) |
+-----------------------+
|           1400.000000 |
+-----------------------+
```

#### （4）group by

> group by子句：根据给定列或者表达式的每一个不同的值将表中的行分成不同的组，使用组函数返回每一组的统计信息
>
> - 出现在SELECT子句中的单独的列，必须出现在GROUP BY子句中作为分组列
> - 分组列可以不出现在SELECT子句中
> - 分组列可出现在SELECT子句中的一个复合表达式中
> - 如果GROUP BY后面是一个复合表达式，那么在SELECT子句中，它必须整体作为一个表达式的一部分才能使用。
>
> 对于分组聚合注意：
>
> 　　通过select在返回集字段中，这些字段要么就要包含在group by语句后面，作为分组的依据，要么就要被包含在聚合函数中。我们可以将group by操作想象成如下的一个过程：首先系统根据select语句得到一个结果集，然后根据分组字段，将具有相同分组字段的记录归并成了一条记录。这个时候剩下的那些不存在与group by语句后面作为分组依据的字段就很有可能出现多个值，但是目前一种分组情况只有一条记录，一个数据格是无法放入多个数值的，所以这个时候就需要通过一定的处理将这些多值的列转化成单值，然后将其放在对应的数据格中，那么完成这个步骤的就是前面讲到的聚合函数，这也就是为什么这些函数叫聚合函数了。

> 指定一个列进行分组

```sql
mysql> select salary,count(*) from salary_tab where salary>=2000 group by salary;
+---------+----------+
| salary  | count(*) |
+---------+----------+
| 2000.00 |        1 |
| 3000.00 |        1 |
+---------+----------+
```

> 指定多个分组列，‘大组中再分小组’

```sql
mysql> select userid,count(salary) from salary_tab where salary>=2000 group by salary,userid;
+--------+---------------+
| userid | count(salary) |
+--------+---------------+
|      2 |             1 |
|      3 |             1 |
+--------+---------------+
```

> 根据表达式分组

```sql
mysql> select year(payment_date),count(*) from PENALTIES group by year(payment_date);
+--------------------+----------+
| year(payment_date) | count(*) |
+--------------------+----------+
|               1980 |        3 |
|               1981 |        1 |
|               1982 |        1 |
|               1983 |        1 |
|               1984 |        2 |
+--------------------+----------+
```

> 带有排序的分组：如果分组列和排序列相同，则可以合并group by和order by子句

```sql
mysql> select teamno,count(*) from MATCHES group by teamno order by teamno desc;
+--------+----------+
| teamno | count(*) |
+--------+----------+
|      2 |        5 |
|      1 |        8 |
+--------+----------+

mysql> select teamno,count(*) from MATCHES group by teamno desc;　　#可以把desc(或者asc)包含到group by子句中简化
+--------+----------+
| teamno | count(*) |
+--------+----------+
|      2 |        5 |
|      1 |        8 |
+--------+----------+
```

#### （5）group_concat()

> 将分组数据合并连接，用逗号隔开

~~~sql
select UNIT,group_concat(DATE) from TT group by UNIT;
+------+-----------------------+
| UNIT | group_concat(DATE)    |
+------+-----------------------+
| a    | 2020-04-03,2020-09-14 |
| b    | 2020-01-01,2020-09-14 |
| c    | 2020-06-06            |
| d    | 2020-03-03            |
+------+-----------------------+
~~~

#### （6）with rollup

> 用来要求在一条group by子句中进行多个不同的分组，用的较少，有的时候需要需要。
>
> 例如：GROUP BY E1,E2,E3,E4 WITH ROLLUP，那么将分别执行以下分组：[E1,E2,E3,E4]、[E1,E2,E3]、[E1,E2]、[E1]、[]，[ ]表示所有行都分在一组中

```sql
-- 示例：按照球员的性别和居住城市，统计球员的总数；统计每个性别球员的总数；统计所有球员的总数
mysql> select sex,town,count(*) from PLAYERS group by sex,town with rollup;
+-----+-----------+----------+
| sex | town      | count(*) |
+-----+-----------+----------+
| F   | Eltham    |        2 |
| F   | Inglewood |        1 |
| F   | Midhurst  |        1 |
| F   | Plymouth  |        1 |
| F   | NULL      |        5 |
| M   | Douglas   |        1 |
| M   | Inglewood |        1 |
| M   | Stratford |        7 |
| M   | NULL      |        9 |
| NULL | NULL      |       14 |
+-----+-----------+----------+
```

####  （7）having

> 对分组结果进行过滤
>
> - 不能使用WHERE子句对分组后的结果进行过滤
>
> - 不能在WHERE子句中使用组函数，仅用于过滤行
> - 因为WHERE子句比GROUP BY先执行，而组函数必须在分完组之后才执行，且分完组后必须使用having子句进行结果集的过滤。
>
> having vs where 
>
> - where子句在分组前对记录进行过滤；
>
> - having子句在分组后对记录进行过滤

```sql
mysql> select playerno from PENALTIES where count(*)>1 group by playerno;
ERROR 1111 (HY000): Invalid use of group function

### 对where过滤后的结果进行再次筛选必须使用having来完成
select salary,count(*) from salary_tab where salary>=2000 group by salary having count(*)>=1;
+---------+----------+
| salary  | count(*) |
+---------+----------+
| 2000.00 |        1 |
| 3000.00 |        1 |
+---------+----------+
```

> having的使用场景
>
> - HAVING可以单独使用而不和GROUP BY配合，如果只有HAVING子句而没有GROUP BY，表中所有的行分为一组。
> - HAVING子句中可以使用组函数。
> - HAVING子句中的列，要么出现在一个组函数中，要么出现在GROUP BY子句中(否则出错)。

```sql
mysql> select town,count(*) from PLAYERS group by town having birth_date>'1970-01-01';
ERROR 1054 (42S22): Unknown column 'birth_date' in 'having clause'
mysql> select town,count(*) from PLAYERS group by town having town in ('Eltham','Midhurst');
+----------+----------+
| town     | count(*) |
+----------+----------+
| Eltham   |        2 |
| Midhurst |        1 |
+----------+----------+
```

### 3、字符串函数

> 字符串函数是最常用的的一种函数，在一个具体应用中通常会综合几个甚至几类函数来实现相应的应用。
>

#### （1）LOWER(column|str)：将字符串参数值转换为全小写字母后返回

```sql
mysql> select lower('SQL Course');
+---------------------+
| lower('SQL Course') |
+---------------------+
| sql course          |
+---------------------+
```

#### （2）UPPER(column|str)：将字符串参数值转换为全大写字母后返回

```sql
mysql> select upper('Use MYsql');
+--------------------+
| upper('Use MYsql') |
+--------------------+
| USE MYSQL          |
+--------------------+
```

#### （3）CONCAT(column|str1, column|str2,...)：将多个字符串参数首尾相连后返回

```sql
mysql> select concat('My','S','QL');
+-----------------------+
| concat('My','S','QL') |
+-----------------------+
| MySQL                 |
+-----------------------+
```

> 如果有任何参数为null，则函数返回null

```sql
mysql> select concat('My',null,'QL');
+------------------------+
| concat('My',null,'QL') |
+------------------------+
| NULL                   |
+------------------------+
```

> 如果参数是数字，则自动转换为字符串

```sql
mysql> select concat(14.3,'mysql');
+----------------------+
| concat(14.3,'mysql') |
+----------------------+
| 14.3mysql            |
+----------------------+
```

#### （4）CONCAT_WS(separator,str1,str2,...)：将多个字符串参数以给定的分隔符separator首尾相连后返回

```sql
mysql> select concat_ws(';','First name','Second name','Last name');
+-------------------------------------------------------+
| concat_ws(';','First name','Second name','Last name') |
+-------------------------------------------------------+
| First name;Second name;Last name                      |
+-------------------------------------------------------+
```

> 如果有任何参数为null，则函数不返回null，而是直接忽略它

```
mysql> select concat_ws(',','id',null,'name');
+---------------------------------+
| concat_ws(',','id',null,'name') |
+---------------------------------+
| id,name                         |
+---------------------------------+
```

> 打开和关闭管道符号“|”的连接功能PIPES_AS_CONCAT，将“||”视为字符串的连接操作符而非或运算符。
>
> - 基本格式：select 列名1 || 列名2 || 列名3  from  表名;
>
> - 在mysql中，进行上式连接查询之后，会将查询结果集在一列中显示(字符串连接)，列名是‘列名1 || 列名2 || 列名3’；
> - 如果不显示结果，是因为sql_mode参数中没有PIPES_AS_CONCAT，只要给sql_mode参数加入PIPES_AS_CONCAT，就可以实现像CONCAT一样的功能；
>
> - 如果不给sql_mode参数加入的话，|| 默认是or的意思，查询结果是一列显示是1。

```sql
mysql> select s_no || s_name || s_age from student;
+-------------------------+
| s_no || s_name || s_age |
+-------------------------+
| 1001张三23              |
| 1002李四19              |
| 1003马五20              |
| 1004甲六17              |
| 1005乙七22              |
+-------------------------+
```

#### （5）SUBSTR(str,pos[,len])：从源字符串str中的指定位置pos开始取一个字串返回

> len指定子串的长度，如果省略则一直取到字符串的末尾；len为负值表示从源字符串的尾部开始取起。
>
> 函数SUBSTR()是函数SUBSTRING()的同义词。

```sql
mysql> select substring('hello world',5);
+----------------------------+
| substring('hello world',5) |
+----------------------------+
| o world                    |
+----------------------------+
mysql> select substr('hello world',5,3);
+---------------------------+
| substr('hello world',5,3) |
+---------------------------+
| o w                       |
+---------------------------+
mysql> select substr('hello world',-5);
+--------------------------+
| substr('hello world',-5) |
+--------------------------+
| world                    |
+--------------------------+
```

#### （6）LENGTH(str)：返回字符串的存储长度【字节】

> 编码方式不同字符串的存储长度就不一样(‘你好’:utf8是6，gbk是4)

```sql
mysql> select length('text'),length('你好');
+----------------+------------------+
| length('text') | length('你好')   |
+----------------+------------------+
|              4 |                6 |
+----------------+------------------+
```

#### （7）CHAR_LENGTH(str)：返回字符串中的字符个数

```sql
mysql> select char_length('text'),char_length('你好');
+---------------------+-----------------------+
| char_length('text') | char_length('你好')   |
+---------------------+-----------------------+
|                   4 |                     2 |
+---------------------+-----------------------+
```

#### （8）INSTR(str, substr)：从源字符串str中返回子串substr第一次出现的位置

```sql
mysql> select instr('foobarbar','bar');
+--------------------------+
| instr('foobarbar','bar') |
+--------------------------+
|                        4 |
+--------------------------+
```

#### （9）LPAD(str, len, padstr)：在源字符串的左边填充给定的字符padstr到指定的长度len，返回填充后的字符串

```sql
mysql> select lpad('hi',5,'??');
+-------------------+
| lpad('hi',5,'??') |
+-------------------+
| ???hi             |
+-------------------+
```

####  （10）RPAD(str, len, padstr)：在源字符串的右边填充给定的字符padstr到指定的长度len，返回填充后的字符串

```sql
mysql> select rpad('hi',6,'??');
+-------------------+
| rpad('hi',6,'??') |
+-------------------+
| hi????            |
+-------------------+
```

####  （11）TRIM([{BOTH | LEADING | TRAILING} [remstr] FROM] str), TRIM([remstr FROM] str)：从源字符串str中去掉两端、前缀或后缀字符remstr并返回

> 如果不指定remstr，则去掉str两端的空格；
>
> 不指定BOTH(两端)、LEADING(前缀)、TRAILING(后缀) ，则默认为 BOTH。

```sql
mysql> select trim('  bar  ');
+-----------------+
| trim('  bar  ') |
+-----------------+
| bar             |
+-----------------+

mysql> select trim(leading 'x' from 'xxxbarxxx');
+------------------------------------+
| trim(leading 'x' from 'xxxbarxxx') |
+------------------------------------+
| barxxx                             |
+------------------------------------+

mysql> select trim('x' from 'xxxbarxxx');
+---------------------------------+
| trim(both 'x' from 'xxxbarxxx') |
+---------------------------------+
| bar                             |
+---------------------------------+

mysql> select trim(trailing 'xyz' from 'barxxyz');
+-------------------------------------+
| trim(trailing 'xyz' from 'barxxyz') |
+-------------------------------------+
| barx                                |
+-------------------------------------+
```

#### （12）REPLACE(str, from_str, to_str)：在源字符串str中查找所有的子串form_str（大小写敏感），找到后使用替代字符串to_str替换它并返回

```sql
mysql> select replace('www.mysql.com','w','Ww');
+-----------------------------------+
| replace('www.mysql.com','w','Ww') |
+-----------------------------------+
| WwWwWw.mysql.com                  |
+-----------------------------------+
```

#### （13）LTRIM(str)，RTRIM(str)：去掉字符串的左边或右边的空格(左对齐、右对齐)

```sql
mysql> SELECT  ltrim('   barbar   ') rs1, rtrim('   barbar   ') rs2;
+-----------+-----------+
| rs1       | rs2       |
+-----------+-----------+
| barbar    |    barbar |
+-----------+-----------+
```

#### （14）REPEAT(str, count)：将字符串str重复count次后返回

```sql
mysql> select repeat('MySQL',3);
+-------------------+
| repeat('MySQL',3) |
+-------------------+
| MySQLMySQLMySQL   |
+-------------------+
```

####  （15）REVERSE(str)：将字符串str反转后返回

```sql
mysql> select reverse('abcdef');
+-------------------+
| reverse('abcdef') |
+-------------------+
| fedcba            |
+-------------------+
```

####  （16）CHAR(N,... [USING charset_name])：将每个参数N解释为整数（字符的编码），并返回每个整数对应的字符所构成的字符串(NULL值被忽略)

```sql
mysql> select char(77,121,83,81,'76'),char(77,77.3,'77.3');
+-------------------------+----------------------+
| char(77,121,83,81,'76') | char(77,77.3,'77.3') |
+-------------------------+----------------------+
| MySQL                   | MMM                  |
+-------------------------+----------------------+
```

> 默认情况下，函数返回二进制字符串，若想返回针对特定字符集的字符串，使用using选项

```sql
mysql> SELECT charset(char(0x65)), charset(char(0x65 USING utf8));
+---------------------+--------------------------------+
| charset(char(0x65)) | charset(char(0x65 USING utf8)) |
+---------------------+--------------------------------+
| binary              | utf8                           |
+---------------------+--------------------------------+
```

####  （17）FORMAT(X,D[,locale])：以格式‘#,###,###.##’格式化数字X

> D指定小数位数，locale指定国家语言(默认的locale为en_US)

```sql
mysql> SELECT format(12332.123456, 4),format(12332.2,0);
+-------------------------+-------------------+
| format(12332.123456, 4) | format(12332.2,0) |
+-------------------------+-------------------+
| 12,332.1235             | 12,332            |
+-------------------------+-------------------+

mysql> SELECT format(12332.2,2,'de_DE');
+---------------------------+
| format(12332.2,2,'de_DE') |
+---------------------------+
| 12.332,20                 |
+---------------------------+
```

####  （18）SPACE(N)：返回由N个空格构成的字符串

```sql
mysql> select space(3);
+----------+
| space(3) |
+----------+
|          |
+----------+
```

#### （19）LEFT(str, len)：返回最左边的len长度的子串

```sql
mysql> select left('chinaitsoft',5);
+-----------------------+
| left('chinaitsoft',5) |
+-----------------------+
| china                 |
+-----------------------+
```

####  （20）RIGHT(str, len)：返回最右边的len长度的子串

```sql
mysql> select right('chinaitsoft',5);
+------------------------+
| right('chinaitsoft',5) |
+------------------------+
| tsoft                  |
+------------------------+
```

####  （21）STRCMP(expr1,expr2)：如果两个字符串是一样的则返回0；如果第一个小于第二个则返回-1；否则返回1

```sql
mysql> select strcmp('text','text');
+-----------------------+
| strcmp('text','text') |
+-----------------------+
|                     0 |
+-----------------------+

mysql> SELECT strcmp('text', 'text2'),strcmp('text2', 'text');
+-------------------------+-------------------------+
| strcmp('text', 'text2') | strcmp('text2', 'text') |
+-------------------------+-------------------------+
|                      -1 |                       1 |
+-------------------------+-------------------------+
```

### 4、日期和时间函数

####  （1）NOW([fsp])：返回服务器的当前日期和时间(fsp指定小数秒的精度，取值0--6)

> 格式：‘YYYY-MM-DD HH:MM:SS’或者‘YYYYMMDDHHMMSS’
>
> - now()的显示格式是‘YYYY-MM-DD HH:MM:SS’
> - now()+0的显示格式是‘YYYYMMDDHHMMSS’

```sql
mysql> select now();
+---------------------+
| now()               |
+---------------------+
| 2022-06-28 13:31:08 |
+---------------------+

mysql> select now()+0;
+----------------+
| now()+0        |
+----------------+
| 20220628133126 |
+----------------+

mysql> select now(6);
+----------------------------+
| now(6)                     |
+----------------------------+
| 2022-06-28 13:31:45.630304 |
+----------------------------+
```

> now()函数的同义词有：CURRENT_TIMESTAMP 、 CURRENT_TIMESTAMP()、LOCALTIMESTAMP 、 LOCALTIMESTAMP()、LOCALTIME 、 LOCALTIME()
>
> 注意：
>
> 　　SYSDATE( )：返回服务器的当前日期和时间
>
> 与now的不同点：(一般使用NOW而不用SYSDATE)
>
> 　　①SYSDATE()返回的是函数执行时的时间
>
> 　　②now()返回的是语句执行时的时间

```sql
mysql> select now(),sleep(2),now();
+---------------------+----------+---------------------+
| now()               | sleep(2) | now()               |
+---------------------+----------+---------------------+
| 2022-06-28 13:33:07 |        0 | 2022-06-28 13:33:07 |
+---------------------+----------+---------------------+
1 row in set (2.19 sec)

mysql> select sysdate(),sleep(2),sysdate();
+---------------------+----------+---------------------+
| sysdate()           | sleep(2) | sysdate()           |
+---------------------+----------+---------------------+
| 2022-06-28 13:33:29 |        0 | 2022-06-28 13:33:31 |
+---------------------+----------+---------------------+
1 row in set (2.05 sec)
```

####  （2）CURTIME([fsp])：返回当前时间，只包含时分秒(fsp指定小数秒的精度，取值0--6)

> 格式：‘YYYY-MM-DD HH:MM:SS’或者‘YYYYMMDDHHMMSS’
>
> 同义词有：CURRENT_TIME 、 CURRENT_TIME() 

```sql
mysql> select curtime(),curtime(2);
+-----------+-------------+
| curtime() | curtime(2)  |
+-----------+-------------+
| 14:35:23  | 14:35:23.90 |
+-----------+-------------+
```

#### （3）CURDATE()：返回当前日期，只包含年月日

> 格式：‘YYYY-MM-DD’或者‘YYYYMMDD’
>
> 同义词有： CURRENT_DATE 、CURRENT_DATE()

```
mysql> select curdate(),curdate()+2;
+------------+-------------+
| curdate()  | curdate()+2 |
+------------+-------------+
| 2022-06-28 |    20220630 |
+------------+-------------+
1 row in set (0.05 sec)
```

#### （4）TIMEDIFF(expr1, expr2)：返回两个日期相减（expr1 − expr2 ）相差的时间数（两个参数类型必须相同）

```sql
mysql> select timediff('18:32:59','60000');
+------------------------------+
| timediff('18:32:59','60000') |
+------------------------------+
| 12:32:59                     |
+------------------------------+


mysql> select timediff('18:32:59','2017-1-1 60000');
+---------------------------------------+
| timediff('18:32:59','2017-1-1 60000') |
+---------------------------------------+
| NULL                                  |
+---------------------------------------+
```

> DATEDIFF(expr1, expr2)：返回两个日期相减（expr1 − expr2 ）相差的天数

```sql
mysql> select datediff('2017-3-24 18:32:59','2016-9-1');
+-------------------------------------------+
| datediff('2017-3-24 18:32:59','2016-9-1') |
+-------------------------------------------+
|                                       204 |
+-------------------------------------------+
```

####  （5）日期时间运算函数：分别为给定的日期date加上(add)或减去(sub)一个时间间隔值expr

> 格式：　DATE_ADD(date, INTERVAL expr unit);DATE_SUB(date, INTERVAL expr unit);
>
> interval是间隔类型关键字。
>
> expr是一个表达式，对应后面的类型，增减的数量。
>
> unit是时间间隔的单位(间隔类型)（20个），如下：

| HOUR          | 小时     |
| ------------- | -------- |
| MINUTE        | 分       |
| SECOND        | 秒       |
| MICROSECOND   | 毫秒     |
| YEAR          | 年       |
| MONTH         | 月       |
| DAY           | 日       |
| WEEK          | 周       |
| QUARTER       | 季       |
| YEAR_MONTH    | 年和月   |
| DAY_HOUR      | 日和小时 |
| DAY_MINUTE    | 日和分钟 |
| DAY_ SECOND   | 日和秒   |
| HOUR_MINUTE   | 小时和分 |
| HOUR_SECOND   | 小时和秒 |
| MINUTE_SECOND | 分钟和秒 |

```sql
mysql> select now(),date_add(now(),interval 1 day);
+---------------------+--------------------------------+
| now()               | date_add(now(),interval 1 day) |
+---------------------+--------------------------------+
| 2022-06-28 14:21:43 | 2022-06-29 14:21:43            |
+---------------------+--------------------------------+
1 row in set (0.04 sec)

mysql> SELECT date_sub('2005-01-01 00:00:00',INTERVAL '1 1:1:1' DAY_SECOND);　　#减1天1小时1分1秒
+---------------------------------------------------------------+
| date_sub('2005-01-01 00:00:00',INTERVAL '1 1:1:1' DAY_SECOND) |
+---------------------------------------------------------------+
| 2004-12-30 22:58:59                                           |
+---------------------------------------------------------------+
```

> 不使用函数，也可以写表达式进行日期的加减：
>
> 　　date + INTERVAL expr unit
>
> 　　date - INTERVAL expr unit

```sql
mysql> SELECT '2008-12-31 23:59:59' + INTERVAL 1 SECOND;
+-------------------------------------------+
| '2008-12-31 23:59:59' + INTERVAL 1 SECOND |
+-------------------------------------------+
| 2009-01-01 00:00:00                       |
+-------------------------------------------+
1 row in set (0.00 sec)

mysql> SELECT '2005-01-01' - INTERVAL 1 SECOND;
+----------------------------------+
| '2005-01-01' - INTERVAL 1 SECOND |
+----------------------------------+
| 2004-12-31 23:59:59              |
+----------------------------------+
1 row in set (0.00 sec)
```

#### （6）选取日期时间的各个部分：日期、时间、年、季度、月、日、小时、分钟、秒、微秒（常用）

```sql
SELECT now(),date(now()); -- 日期

SELECT now(),time(now()); -- 时间

SELECT now(),year(now()); -- 年

SELECT now(),quarter(now()); -- 季度

SELECT now(),month(now()); -- 月

SELECT now(),week(now()); -- 周

SELECT now(),day(now()); -- 日

SELECT now(),hour(now()); -- 小时

SELECT now(),minute(now()); -- 分钟

SELECT now(),second(now()); -- 秒

SELECT now(),microsecond(now()); -- 微秒
```

> EXTRACT(unit FROM date)：从日期中抽取出某个单独的部分或组合

```sql
SELECT now(),extract(YEAR FROM now()); -- 年

SELECT now(),extract(QUARTER FROM now()); -- 季度

SELECT now(),extract(MONTH FROM now()); -- 月

SELECT now(),extract(WEEK FROM now()); -- 周

SELECT now(),extract(DAY FROM now()); -- 日

SELECT now(),extract(HOUR FROM now()); -- 小时

SELECT now(),extract(MINUTE FROM now()); -- 分钟

SELECT now(),extract(SECOND FROM now()); -- 秒

SELECT now(),extract(YEAR_MONTH FROM now()); -- 年月

SELECT now(),extract(HOUR_MINUTE FROM now()); -- 时分
```

#### （7）个性化显示时间日期

> dayofweek(date)，dayofmonth(date)，dayofyear(date)分别返回日期在一周、一月、一年中是第几天

```sql
mysql> SELECT now(),dayofweek(now());
+---------------------+------------------+
| now()               | dayofweek(now()) |
+---------------------+------------------+
| 2022-06-28 14:22:28 |                3 |
+---------------------+------------------+
1 row in set (0.04 sec)

mysql> SELECT now(),dayofmonth(now());
+---------------------+-------------------+
| now()               | dayofmonth(now()) |
+---------------------+-------------------+
| 2022-06-28 14:22:50 |                28 |
+---------------------+-------------------+
1 row in set (0.06 sec)

mysql> select now(),dayofyear(now());
+---------------------+------------------+
| now()               | dayofyear(now()) |
+---------------------+------------------+
| 2022-06-28 14:23:04 |              179 |
+---------------------+------------------+
1 row in set (0.16 sec)
```

> dayname()，monthname()分别返回日期的星期和月份名称
>
> 名称是中文or英文的由系统变量lc_time_names控制(默认值是'en_US')

```sql
mysql> show variables like 'lc_time_names';
+---------------+-------+
| Variable_name | Value |
+---------------+-------+
| lc_time_names | en_US |
+---------------+-------+
1 row in set (0.00 sec)

mysql> select dayname(now()),monthname(now());
+----------------+------------------+
| dayname(now()) | monthname(now()) |
+----------------+------------------+
| Wednesday      | April            |
+----------------+------------------+
1 row in set (0.00 sec)

mysql> set lc_time_names='zh_CN';
Query OK, 0 rows affected (0.00 sec)

mysql> select dayname(now()),monthname(now());
+----------------+------------------+
| dayname(now()) | monthname(now()) |
+----------------+------------------+
| 星期三         | 四月             |
+----------------+------------------+
1 row in set (0.00 sec)
```

### 5、加密函数

#### （1）MD5(str) md5加密

```sql
mysql> SELECT MD5('hello');
+----------------------------------+
| MD5('hello')                     |
+----------------------------------+
| 5d41402abc4b2a76b9719d911017c592 |
+----------------------------------+
```

#### （2）sha(str) sha加密

```sql
mysql> SELECT SHA('hello');
+------------------------------------------+
| SHA('hello')                             |
+------------------------------------------+
| aaf4c61ddcc5e8a2dabede0f3b482cd9aea9434d |
+------------------------------------------+
```

#### （3）sha1(str) sha1加密

    mysql> SELECT SHA1('hello');
    +------------------------------------------+
    | SHA1('hello')                            |
    +------------------------------------------+
    | aaf4c61ddcc5e8a2dabede0f3b482cd9aea9434d |
    +------------------------------------------+

#### （4）encode(str,key) 和 decode(str,key) 使用key作为密钥加密解密字符串str（MYSQL8已经弃用，建议MD5）

    SELECT DECODE(ENCODE("hello","password"),"password")
    hello12

#### （5）AES_ENCRYPT(str,key)  返回用密钥key对字符串str利用高级标准加密算法加密后的结果

> 调用AES_ENCRYPT的结果是一个二进制字符串，以BLOB类型存储，AES_DECRYPT(密文,key)返回用密钥key对字符串str利用高级加密标准算法解密后的结果。

```sql
mysql> SELECT TO_BASE64(AES_ENCRYPT('HelloWorld','test'));
+---------------------------------------------+
| TO_BASE64(AES_ENCRYPT('HelloWorld','test')) |
+---------------------------------------------+
| QeJ1iOA5Z2vNJBxsRVeLeQ==                    |
+---------------------------------------------+

mysql> SELECT AES_DECRYPT(FROM_BASE64('QeJ1iOA5Z2vNJBxsRVeLeQ=='),'test');
+-------------------------------------------------------------+
| AES_DECRYPT(FROM_BASE64('QeJ1iOA5Z2vNJBxsRVeLeQ=='),'test') |
+-------------------------------------------------------------+
| HelloWorld                                                  |
+-------------------------------------------------------------+
```

### 6、流程控制函数

#### （1）case when then else end

```sql
mysql> select case 1 when 1 then 1
    -> when 2 then 2
    -> when 3 then 3
    -> else 4 end as number;
+--------+
| number |
+--------+
|      1 |
+--------+
```

#### （2）IF(expr1,expr2,expr3)

> 如果 expr1 是TRUE (expr1 <> 0 and expr1 <> NULL)，则 IF()的返回值为expr2; 否则返回值则为 expr3。

```sql
mysql> SELECT IF(1>2,2,3);
+-------------+
| IF(1>2,2,3) |
+-------------+
|           3 |
+-------------+

mysql> SELECT IF(1<2,'yes ','no');
+---------------------+
| IF(1<2,'yes ','no') |
+---------------------+
| yes                 |
+---------------------+

mysql> SELECT IF(STRCMP('test','test1'),'no','yes');
+---------------------------------------+
| IF(STRCMP('test','test1'),'no','yes') |
+---------------------------------------+
| no                                    |
+---------------------------------------+

mysql> SELECT IF(0.1,1,0);
+-------------+
| IF(0.1,1,0) |
+-------------+
|           1 |
+-------------+

mysql> SELECT IF(0,1,0);
+-----------+
| IF(0,1,0) |
+-----------+
|         0 |
+-----------+

mysql> SELECT IF(null,1,0);
+--------------+
| IF(null,1,0) |
+--------------+
|            0 |
+--------------+
```

#### （3）IFNULL(expr1,expr2)

> 假如expr1 不为 NULL，则 IFNULL() 的返回值为 expr1; 否则其返回值为 expr2。

```sql
mysql> SELECT IFNULL(1,10);
+--------------+
| IFNULL(1,10) |
+--------------+
|            1 |
+--------------+

mysql> SELECT IFNULL(null,10);
+-----------------+
| IFNULL(null,10) |
+-----------------+
|              10 |
+-----------------+

-- 1/0计算结果为null
mysql> SELECT IFNULL(1/0,10);
+----------------+
| IFNULL(1/0,10) |
+----------------+
| 10.0000        |
+----------------+
```

#### （4）NULLIF(expr1,expr2)

> 如果expr1 = expr2 成立，那么返回值为NULL，否则返回值为 expr1。这和CASE WHEN expr1 = expr2 THEN NULL ELSE expr1 END相同。

```sql
mysql> SELECT NULLIF(1,1);
+-------------+
| NULLIF(1,1) |
+-------------+
| NULL        |
+-------------+

mysql> SELECT NULLIF(1,2);
+-------------+
| NULLIF(1,2) |
+-------------+
|           1 |
+-------------+
```

### 7、格式化函数

#### （1）DATE_FORMAT(date,fmt) 

> 依照字符串fmt格式化日期date值

```sql
mysql> SELECT DATE_FORMAT(NOW(),'%W,%D %M %Y %r');
+-------------------------------------+
| DATE_FORMAT(NOW(),'%W,%D %M %Y %r') |
+-------------------------------------+
| Tuesday,28th June 2022 04:36:02 PM  |
+-------------------------------------+

mysql> SELECT DATE_FORMAT(NOW(),'%Y-%m-%d');
+-------------------------------+
| DATE_FORMAT(NOW(),'%Y-%m-%d') |
+-------------------------------+
| 2022-06-28                    |
+-------------------------------+

mysql> SELECT DATE_FORMAT(19990330,'%Y-%m-%d');
+----------------------------------+
| DATE_FORMAT(19990330,'%Y-%m-%d') |
+----------------------------------+
| 1999-03-30                       |
+----------------------------------+

mysql> SELECT DATE_FORMAT(NOW(),'%h:%i %p');
+-------------------------------+
| DATE_FORMAT(NOW(),'%h:%i %p') |
+-------------------------------+
| 04:36 PM                      |
+-------------------------------+
```

#### （2）FORMAT(x,y)   把x格式化为以逗号隔开的数字序列，y是结果的小数位数

```sql
mysql> SELECT FORMAT(34234.34323432,3);
+--------------------------+
| FORMAT(34234.34323432,3) |
+--------------------------+
| 34,234.343               |
+--------------------------+
```

#### （3）INET_ATON(ip)   返回IP地址的数字表示

```sql
mysql> SELECT INET_ATON('10.122.89.47');
+---------------------------+
| INET_ATON('10.122.89.47') |
+---------------------------+
|                 175790383 |
+---------------------------+
```

#### （4）INET_NTOA(num)   返回数字所代表的IP地址

```sql
mysql> SELECT INET_NTOA(175790383);
+----------------------+
| INET_NTOA(175790383) |
+----------------------+
| 10.122.89.47         |
+----------------------+
```

#### （5）TIME_FORMAT(time,fmt)  依照字符串fmt格式化时间time值

```sql
mysql> SELECT TIME_FORMAT(now(),'%h:%i %p');
+-------------------------------+
| TIME_FORMAT(now(),'%h:%i %p') |
+-------------------------------+
| 04:55 PM                      |
+-------------------------------+
```

### 8、类型转换函数

> MySQL 的CAST()和CONVERT()函数可用来转换一个类型的值成另一个类型的值。
>
> 但是要特别注意，可以转换的数据类型是有限的。这个类型可以是以下值其中的一个：
>
> - 二进制，同带binary前缀的效果 : BINARY
> - 字符型，可带参数 : CHAR()
> - 日期 : DATE
> - 时间: TIME
> - 日期时间型 : DATETIME
> - 浮点数 : DECIMAL
> - 整数 : SIGNED
> - 无符号整数 : UNSIGNED

#### （1）CAST(value as type) 

```sql
mysql> select cast('360.5' as signed);
+-------------------------+
| cast('360.5' as signed) |
+-------------------------+
|                     360 |
+-------------------------+
```

#### （2）CONVERT(value, type) 

```sql
mysql> select convert('360.5',signed);
+-------------------------+
| convert('360.5',signed) |
+-------------------------+
|                     360 |
+-------------------------+
```

### 9、系统信息函数

#### （1）VERSION()查看当前MySQL版本号

```sql
mysql> SELECT VERSION();
+-----------+
| VERSION() |
+-----------+
| 8.0.28    |
+-----------+
```

#### （2）CONNECTION_ID()查看当前用户的连接数

```sql
mysql> SELECT CONNECTION_ID();
+-----------------+
| CONNECTION_ID() |
+-----------------+
|             132 |
+-----------------+
```

#### （3）SHOW PROCESSLIST输出当前用户的连接信息

```sql
mysql> SHOW PROCESSLIST;
```

![image-20220713135926262](http://cdn.bluecusliyou.com/202207131359487.png)

#### （4）DATABASE()查看当前使用的数据库

```sql
mysql> SELECT DATABASE(),SCHEMA();
+------------+----------+
| DATABASE() | SCHEMA() |
+------------+----------+
| testdb     | testdb   |
+------------+----------+
```

#### （5）USER()获取当前登录用户名称

```sql
mysql> SELECT USER(), CURRENT_USER(), SYSTEM_USER();
+--------------------+----------------+--------------------+
| USER()             | CURRENT_USER() | SYSTEM_USER()      |
+--------------------+----------------+--------------------+
| root@112.81.21.210 | root@%         | root@112.81.21.210 |
+--------------------+----------------+--------------------+
```

#### （6）CHARSET()返回字符串使用的字符集

```sql
mysql> SELECT CHARSET('abc'),CHARSET(CONVERT('abc' USING latin1)),CHARSET(VERSION());
+----------------+--------------------------------------+--------------------+
| CHARSET('abc') | CHARSET(CONVERT('abc' USING latin1)) | CHARSET(VERSION()) |
+----------------+--------------------------------------+--------------------+
| utf8mb4        | latin1                               | utf8mb3            |
+----------------+--------------------------------------+--------------------+
```

#### （7）COLLATION()返回字符串排列方式

```sql
mysql> SELECT COLLATION('abc'),COLLATION(CONVERT('abc' USING utf8));
+--------------------+--------------------------------------+
| COLLATION('abc')   | COLLATION(CONVERT('abc' USING utf8)) |
+--------------------+--------------------------------------+
| utf8mb4_0900_ai_ci | utf8_general_ci                      |
+--------------------+--------------------------------------+
```

#### （8）LAST_INSERT_ID()查看最后一个自动生成的列值

```sql
-- 创建表
mysql> CREATE TABLE worker (Id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,Name VARCHAR(30));
Query OK, 0 rows affected (0.08 sec)

-- 插入数据
mysql> INSERT INTO worker VALUES(NULL, 'jimy');
Query OK, 1 row affected (0.04 sec)

mysql> INSERT INTO worker VALUES(NULL, 'Tom');
Query OK, 1 row affected (0.03 sec)

-- 查看最后一个自动生成的列值
mysql> SELECT LAST_INSERT_ID();
+------------------+
| LAST_INSERT_ID() |
+------------------+
|                2 |
+------------------+
```

## 九、数据备份还原

### 1、数据库备份必要性

- 保证重要数据不丢失
- 数据转移

### 2、Navicat备份还原

#### （1）Navicat导入导出sql文件

![image-20220727160403166](http://cdn.bluecusliyou.com/202208070954844.png)

#### （2）Navicat备份

![image-20220807101022831](http://cdn.bluecusliyou.com/202208071010919.png)

#### （3）Navicat还原

![image-20220807101126450](http://cdn.bluecusliyou.com/202208071011536.png)

### 3、文件备份（物理备份）

​        物理备份是指通过拷贝数据库文件的方式完成备份，这种备份方式**适用于数据库很大**，数据重要且**需要快速恢复的数据库。**

​         通常情况下物理备份的速度要快于逻辑备份，另外物理备份的备份和恢复粒度范围为整个数据库或者是单个文件。对单表是否有恢复能力取决于存储引擎，比如在MyISAM存储引擎下每个表对应了独立的文件，可以单独恢复；但对于InnoDB存储引擎表来说，可能每个表对应了独立的文件，也可能表使用了共享数据文件。

​         物理备份通常要求在数据库关闭的情况下执行，但如果是在数据库运行情况下执行，则要求备份期间数据库不能修改。

**注：MyISAM的表天生就分成了三个独立的数据文件（\*.frm, \*.MYD, and \*.MYI），可以直接拷贝；InnoDB不支持拷贝表级别或者DB级别的拷贝，但是InnoDB可以直接把Data文件夹整体拷贝，也就是将所有的数据库都拷贝了。**

​        逻辑备份的速度要慢于物理备份，是因为逻辑备份需要访问数据库并将内容转化成逻辑备份需要的格式；通常输出的备份文件大小也要比物理备份大；另外逻辑备份也不包含数据库的配置文件和日志文件内容；备份和恢复的粒度可以是所有数据库，也可以是单个数据库，也可以是单个表；逻辑备份需要再数据库运行的状态下执行；它的执行工具可以是mysqldump或者是select … into outfile两种方式。

### 4、mysqldump（逻辑备份）

#### （1）数据准备

创建数据库testdb和表tb_test1、tb_test2

![image-20220629205613142](http://cdn.bluecusliyou.com/202206292056271.png)

```sql
DROP TABLE IF EXISTS `tb_test1`;
CREATE TABLE `tb_test1` (
  `id` int NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
INSERT INTO `tb_test1` VALUES (1001,'ly'),(1002,'zjl');

DROP TABLE IF EXISTS `tb_test2`;
CREATE TABLE `tb_test2` (
  `id` int NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
INSERT INTO `tb_test2` VALUES (1001,'ly');
```

#### （2）导出单张表结构+数据

```bash
## cmd命令窗口下执行
mysqldump -u[用户名] -h[ip] -p[密码] -P[端口号] 数据库名 表名>导出文件名.sql
mysqldump -uroot -h127.0.0.1 -p123 -P3306 testdb tb_test1>d:/sqls/testdb.sql
```

#### （3）导出多张表结构+数据

```bash
mysqldump -uroot -h127.0.0.1 -p123 -P3306 testdb --tables tb_test1 tb_test2>d:/sqls/testdb2.sql
```

#### （4）只导出表结构不导表数据，添加`-d`命令参数

```bash
mysqldump -uroot -h127.0.0.1 -p123 -P3306 -d testdb tb_test1>d:/sqls/testdb3.sql
```

#### （5）只导出表数据不导表结构，添加`-t`命令参数

```bash
mysqldump -uroot -h127.0.0.1 -p123 -P3306 -t testdb tb_test1>d:/sqls/testdb4.sql
```

#### （6）导出指定数据库的全部表结构+数据

```bash
##导出命令
mysqldump -uroot -h127.0.0.1 -p123 -P3306 testdb>d:/sqls/testdb5.sql

##导入命令 导入需要确保数据库存在
mysql -uroot -h127.0.0.1 -p123 -P3306 testdb<d:/sqls/testdb5.sql
```

#### （7）导出指定数据库+全部表结构+数据`--databases`

```bash
##导出命令
mysqldump -uroot -h127.0.0.1 -p123 -P3306 --databases testdb>d:/sqls/testdb6.sql

##导入命令 导入不需要存在数据库
mysql -uroot -h127.0.0.1 -p123 -P3306<d:/sqls/testdb6.sql
```

#### （8）导出所有数据库+全部表结构+数据`--all-databases`

```bash
mysqldump -uroot -h127.0.0.1 -p123 -P3306  --all-databases>d:/sqls/testdb7.sql
```

#### （9）只导出数据库+全部表结构`--no-data`

```bash
mysqldump -uroot -h127.0.0.1 -p123 -P3306  --no-data --databases testdb>d:/sqls/testdb8.sql
```

## 十、存储引擎

### 1、mysql的体系结构

MySQL Server架构自顶向下大致可以分网络连接层、服务层、存储引擎层和系统文件层。

![image-20220705141820481](http://cdn.bluecusliyou.com/202207051418690.png)

#### （1）**网络连接层**

> 客户端连接器（Client Connectors）：提供与MySQL服务器建立的支持。目前几乎支持所有主流的服务端编程技术，例如常见的 Java、C、Python、.NET等，它们通过各自API技术与MySQL建立连接。

#### （2）服务层

> 服务层是MySQL Server的核心，主要包含系统管理和控制工具、连接池、SQL接口、解析器、查询优化器和缓存六个部分。
>
> - 连接池（Connection Pool）：负责存储和管理客户端与数据库的连接，一个线程负责管理一个连接。
>
> ```sql
> mysql> show variables like "%max_connections%";
> +------------------------+-------+
> | Variable_name          | Value |
> +------------------------+-------+
> | max_connections        | 151   |
> | mysqlx_max_connections | 100   |
> +------------------------+-------+
> ```
>
> - 系统管理和控制工具（Management Services & Utilities）：例如备份恢复、安全管理、集群管理等
> - SQL接口（SQL Interface）：用于接受客户端发送的各种SQL命令，并且返回用户需要查询的结果。比如DML、DDL、存储过程、视图、触发器等。
> - 解析器（Parser）：负责将请求的SQL解析生成一个"解析树"。然后根据一些MySQL规则进一步检查解析树是否合法。
> - 查询优化器（Optimizer）：SQL语句在查询之前会使用查询优化器对查询进行优化。根据客户端请求的 query 语句，和数据库中的一些统计信息，在一系列算法的基础上进行分析，得出一个最优的策略，告诉后面的程序如何取得这个 query 语句的结果。
> - 缓存（Cache&Buffer）： 缓存机制是由一系列小缓存组成的。比如表缓存，记录缓存，权限缓存，引擎缓存等。如果查询缓存有命中的查询结果，查询语句就可以直接去查询缓存中取数据。

#### （4）存储引擎层

> 存储引擎负责MySQL中数据的存储与提取，与底层系统文件进行交互。MySQL存储引擎是插件式的，服务器中的查询执行引擎通过接口与存储引擎进行通信，接口屏蔽了不同存储引擎之间的差异 。现在有很多种存储引擎，各有各的特点，最常见的是MyISAM和InnoDB。

#### （5）**系统文件层**

> 数据存储层， 主要是将数据存储在文件系统之上，并完成与存储引擎的交互。主要包含日志文件，数据文件，配置文件，pid 文件，socket 文件等。
>

### 2、存储引擎

#### （1）概述

> 查看数据库支持的存储引擎 `show engines;`

MySQL5.7支持的存储引擎包含 ： InnoDB 、MyISAM 、BDB、MEMORY、MERGE、EXAMPLE、NDBCluster、ARCHIVE、CSV、BLACKHOLE、FEDERATED等，其中InnoDB和BDB提供事务安全表，其他存储引擎是非事务安全表。Mysql5.5之前的默认存储引擎是MyISAM，5.5之后改为InnoDB。

![image-20220713144659946](http://cdn.bluecusliyou.com/202207131447052.png)

> 查看MySQL数据库默认的存储引擎

```sql
mysql> show variables like '%default_storage_engine%';
+------------------------+--------+
| Variable_name          | Value  |
+------------------------+--------+
| default_storage_engine | InnoDB |
+------------------------+--------+
```

要修改默认的存储引擎可以在配置文件中设置default_storage_engine

#### （2）各种常用存储引擎的特性

![image-20220705144941899](http://cdn.bluecusliyou.com/202207051449973.png)

#### （3）MyISAM存储引擎

MyISAM是mysql5.5之前的默认存储引擎，MyISAM既不支持事务，也不支持外键，每个MyISAM在磁盘上存储成3个文件，其文件名都和表名相同，但拓展名分别是 ：

- .frm(存储表定义)
- .MYD(MYData，存储数据);
- .MYI(MYIndex，存储索引)

数据文件和索引文件可以放置在不同的目录，平均分布IO，获得更快的速度。

MyISAM的表还支持3种不同的存储格式

- 静态表(固定长度)表(默认的存储格式)
- 动态表
- 压缩表

#### （4）InnoDB存储引擎

InnoDb是MySQL5.5之后的默认存储引擎，提供了具有提交，回滚和崩溃恢复能力的事务安全保障，同时提供了更小的锁粒度和更强的并发能力，拥有自己独立的缓存和日志。但是对比MyISAM的存储引擎，InnoDB写的处理效率差一些，并且会占用更多的磁盘空间以保留数据和索引。

> InnoDB的自动增长列

- 对于InnoDB表，自动增长列必须被索引，如果是组合索引，也必须是组合索引的第一列。
- 自动增长默认是从1开始，可以通过' alter table table_name auto_increment = n'语句强制设置自动增长列的初始值。
- 在mysql8.0以前对于InnoDB引擎来说自动增长值是保存在内存中的，如果数据库重新启动，这个值就会丢失，数据库会自动将auto_increment重置为自增列当前存储的最大值+1可以通过LAST_INSERT_ID()查询当前线程最后插入记录的值，如果一次插入多条记录，那么返回的是第一条记录使用的自动增长值，但是如果人为指定自增列的值，LAST_INSERT_ID()的值不会更新。

> InnoDB引擎存在事务

> InnoDB的外键约束

MySQL支持外键的存储引擎只有InnoDB ， 在创建外键的时候， 要求父表必须有对应的索引 (一般关联主表的主键，因为主键非空且唯一)。

```sql
-- 如下两张表，子表(city_innodb)的country_id为外键，关联主表(country_innodb)的country_id字段，并且设置了外键之间的级联关系
CREATE TABLE country_innodb (
	country_id INTEGER ( 10 ) AUTO_INCREMENT PRIMARY KEY,
	country_name VARCHAR ( 30 ) NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
   
CREATE TABLE city_innodb (
	city_id INTEGER ( 10 ) AUTO_INCREMENT PRIMARY KEY,
	city_name VARCHAR ( 30 ) NOT NULL,
	country_id INTEGER ( 10 ) NOT NULL,
	KEY idx_fk_country_id ( country_id ),
	CONSTRAINT `fk_city_country` FOREIGN KEY ( country_id ) REFERENCES
	country_innodb ( country_id ) ON DELETE RESTRICT ON UPDATE CASCADE
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
```

在创建索引时， 可以指定在删除、更新父表时，对子表进行的相应操作，包括 RESTRICT、CASCADE、SET NULL 和 NO ACTION。

- NO ACTION和RESTRICT：是指限制在子表有关联记录的情况下， 父表不能更新；
- CASCADE：父表在更新或者删除时，更新或者删除子表对应的记录；
- SET NULL：表示父表在更新或者删除的时候，子表的对应字段被SET NULL 。

针对上面创建的两个表， 子表的外键指定是ON DELETE RESTRICT ON UPDATE CASCADE 方式的， 那么在主表删除记录的时候， 如果子表有对应记录， 则不允许删除， 主表在更新记录的时候， 如果子表有对应记录， 则子表对应更新 。

> InnoDB主键和索引

InnoDB的数据文件本身就是以聚簇索引的形式保存，这个聚簇索引也被成为主索引(主键),InnoDb的每行数据都保存在主索引的叶子节点上，所以InnoDB表必须存在索引，没有索引会自动创建一个长度为6个字节的long类型的隐藏字段作为索引，除了主键外的索引都叫辅助索引或者二级索引，他们会指向主索引，并通过主索引获取最终的数据。

> InnoDB的存储方式

InnoDB存储表和索引有以下两种方式

- 使用共享表空间存储， 这种方式创建的表的表结构保存在.frm文件中， 数据和索引保存在innodb_data_home_dir 和 innodb_data_file_path定义的表空间中，可以是多个文件。
- 使用多表空间存储， 这种方式创建的表的表结构仍然存在 .frm 文件中，但是每个表的数据和索引单独保存在 .ibd 中。如果是分区表，则每个分区表对应单独的.ibd文件，文件名是‘表名+分区名’

要设置多表空间的存储方式，需要设置参数'innodb_file_per_table'为on(5.7默认也是多表空间的存储方式)

#### （5）MEMORY存储引擎

```sql
CREATE TABLE `city_memory` (
 `city_id` int NOT NULL AUTO_INCREMENT primary key,
 `city_name` varchar(50) NOT NULL,
 `country_id` int NOT NULL
) ENGINE=MEMORY AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
```

Memory存储引擎将表的数据存放在内存中。每个MEMORY表实际对应一个磁盘文件，格式是.frm ，该文件中只存储表的结构，而其数据文件，都是存储在内存中，这样有利于数据的快速处理，提高整个表的效率。MEMORY 类型的表访问非常地快，因为他的数据是存放在内存中的，并且默认使用HASH索引 ， 但是服务一旦关闭，表中的数据就会丢失。

#### （6）MERGE存储引擎

MERGE存储引擎是一组MyISAM表的组合，这些MyISAM表必须结构完全相同，MERGE表本身并没有存储数据，对MERGE类型的表可以进行查询、更新、删除操作，这些操作实际上是对内部的MyISAM表进行的。

对于MERGE类型表的插入操作，是通过INSERT_METHOD子句定义插入的表，可以有3个不同的值，使用FIRST 或 LAST 值使得插入操作被相应地作用在第一或者最后一个表上，不定义这个子句或者定义为NO，表示不能对这个MERGE表执行插入操作。

可以对MERGE表进行DROP操作，但是这个操作只是删除MERGE表的定义，对内部的表是没有任何影响的。

![image-20220705181254422](http://cdn.bluecusliyou.com/202207051812509.png)

MERGE表在磁盘上保留两个文件，文件名以表的名字开始，一个.frm文件存储表定义，另一个.MRG文件包含组合表的信息。

Merge存储示例，创建3个测试表 order_1990, order_1991, order_all , 其中order_all是前两个表的MERGE表 ：

```sql
create table order_1990(
 order_id int ,
 order_money double(10,2),
 order_address varchar(50),
 primary key (order_id)
)engine = myisam default charset=utf8;
create table order_1991(
 order_id int ,
 order_money double(10,2),
 order_address varchar(50),
 primary key (order_id)
)engine = myisam default charset=utf8;
-- 前边两张表的merge表
create table order_all(
 order_id int ,
 order_money double(10,2),
 order_address varchar(50),
 primary key (order_id)
)engine = merge union = (order_1990,order_1991) 
-- 表示向merge表插入数据时，插入到最后一个表上
INSERT_METHOD=LAST default charset=utf8;

-- 向order_1990表插入两条数据
insert into order_1990 values(1,100.0,'北京');
insert into order_1990 values(2,100.0,'上海');
-- 向order_1991插入两条数据
insert into order_1991 values(10,200.0,'北京');
insert into order_1991 values(11,200.0,'上海');
```

> 查看合并表数据

```sql
mysql> select *  from order_all;
+----------+-------------+---------------+
| order_id | order_money | order_address |
+----------+-------------+---------------+
|        1 |      100.00 | 北京          |
|        2 |      100.00 | 上海          |
|       10 |      200.00 | 北京          |
|       11 |      200.00 | 上海          |
+----------+-------------+---------------+
```

> 往order_all中插入一条记录 ，由于在MERGE表定义时，INSERT_METHOD 选择的是LAST，那么插入的数据会想最后一张表中插入。

```sql
mysql> select *  from order_1990;
+----------+-------------+---------------+
| order_id | order_money | order_address |
+----------+-------------+---------------+
|        1 |      100.00 | 北京          |
|        2 |      100.00 | 上海          |
+----------+-------------+---------------+
mysql> select *  from order_1991;
+----------+-------------+---------------+
| order_id | order_money | order_address |
+----------+-------------+---------------+
|       10 |      200.00 | 北京          |
|       11 |      200.00 | 上海          |
+----------+-------------+---------------+

mysql> insert into order_all values(100,10000.0,'苏州');
mysql> select *  from order_1990;
+----------+-------------+---------------+
| order_id | order_money | order_address |
+----------+-------------+---------------+
|        1 |      100.00 | 北京          |
|        2 |      100.00 | 上海          |
+----------+-------------+---------------+
mysql> select *  from order_1991;
+----------+-------------+---------------+
| order_id | order_money | order_address |
+----------+-------------+---------------+
|       10 |      200.00 | 北京          |
|       11 |      200.00 | 上海          |
|      100 |    10000.00 | 苏州          |
+----------+-------------+---------------+
```

### 3、存储引擎的选择

在选择存储引擎时，应该根据应用系统的特点选择合适的存储引擎。对于复杂的应用系统，还可以根据实际情况选择多种存储引擎进行组合。以下是几种常用的存储引擎的使用环境。

- InnoDB : 是Mysql的默认存储引擎，用于事务处理应用程序，支持外键。如果应用对事务的完整性有比较高的要求，在并发条件下要求数据的一致性，数据操作除了插入和查询意外，还包含很多的更新、删除操作，那么InnoDB存储引擎是比较合适的选择。InnoDB存储引擎除了有效的降低由于删除和更新导致的锁定， 还可以确保事务的完整提交和回滚，对于类似于计费系统或者财务系统等对数据准确性要求比较高的系统，InnoDB是最合适的选择。
- MyISAM ： 如果应用是以读操作和插入操作为主，只有很少的更新和删除操作，并且对事务的完整性、并发性要求不是很高，那么选择这个存储引擎是非常合适的。
- MEMORY：将所有数据保存在RAM中，在需要快速定位记录和其他类似数据环境下，可以提供几块的访问。MEMORY的缺陷就是对表的大小有限制，太大的表无法缓存在内存中，其次是要确保表的数据可以恢复，数据库异常终止后表中的数据是可以恢复的。MEMORY表通常用于更新不太频繁的小表，用以快速得到访问结果。
- MERGE：用于将一系列等同的MyISAM表以逻辑方式组合在一起，并作为一个对象引用他们。MERGE表的优点在于可以突破对单个MyISAM表的大小限制，并且通过将不同的表分布在多个磁盘上，可以有效的改善MERGE表的访问效率。这对于存储诸如数据仓储等VLDB环境十分合适。

### 4、一条SQL查询语句怎么运行

![image-20220724214227751](http://cdn.bluecusliyou.com/202207242142052.png)

### 5、sql语句执行顺序

```sql
select             # 5
 ... 
from               # 1
 ... 
where               # 2
 .... 
group by           # 3
 ... 
having             # 4
 ... 
order by           # 6
 ... 
limit               # 7
 [offset]
```

## 十一、事务

### 1、事务概念

事务会把数据库从一种一致状态转换为另一种一致状态。在数据库提交工作时，可以确保要么所有修改都已经保存了，那么所有修改都不保存。

### 2、事务的四大特性

我们都知道，提到事务，就不能不提事务的四大特性，ACID，即原子性，一致性，隔离性，持久性。

- 原子性（Atom）：事务的一组操作是原子的不可再分割的，这组操作要么同时完成要么同时不完成。
- 一致性（Consistency）: 事务在执行前后数据的完整性保持不变。数据库在某个状态下符合所有的完整性约束的状态叫做数据库具有完整性。在解散一个部门时应该同时处理员工表中的员工保证这个事务结束后，仍然保证所有的员工能找到对应的部门，满足外键约束。
- 隔离性（Isolation）：当多个事务同时操作一个数据库时，可能存在并发问题，此时应保证各个事务要进行隔离，事务之间不能互相干扰。
- 持久性（Durability）：持久性是指一个事务一旦被提交，它对数据库中数据的改变就是永久性的，不能再回滚。

### 3、事务引发的问题

#### （1）脏读（Dirty Reads）

事务A读取到了事务B已经修改但尚未提交的数据，还在这个数据基础上做了操作。此时，如果B事务回滚，A读取的数据无效，不符合一致性要求。

#### （2）幻读（Phantom Reads）

事务A读取到了事务B提交的新增数据，不符合隔离性。

#### （3）不可重复读（Non-Repeatable Reads）

事务A读取到了事务B已经提交的修改数据，不符合隔离性。

####  （4）更新丢失（Lost Update）

当两个或多个事务选择同一行，然后基于最初选定的值更新该行时，由于每个事务都不知道其他事务的存在，就会发生丢失更新问题–最后的更新覆盖了由其他事务所做的更新。

### 4、事务的隔离级别

在数据库操作中，为了有效保证并发读取数据的正确性，提出的事务隔离级别，在标准SQL规范中，定义了4个事务隔离级别，不同的隔离级别对事务的处理不同。

#### （1）未授权读取（Read Uncommitted）

也称为读未提交（Read Uncommitted）：会引发脏读取、不可重复读和虚读，但避免了更新丢失。如果一个事务已经开始写数据，则另外一个事务则不允许同时进行写操作，但允许其他事务读此行数据。该隔离级别可以通过“排他写锁”实现。

```sql
-- A窗口
set transaction isolation level  read uncommitted;--设置A用户的数据库隔离级别为Read uncommitted(读未提交)
start transaction;--开启事务
select * from account;--查询A账户中现有的钱，转到B窗口进行操作

-- B窗口
start transaction;--开启事务
update account set money=money+100 where name='A';--不要提交，转到A窗口查询

-- A窗口
select * from account;--发现a多了100元，这时候A读到了B未提交的数据（脏读）
```

#### （2）授权读取（Read Committed）

也称为读提交（Read Committed）：会引发不可重复读取和虚读，但避免脏读取。这可以通过“瞬间共享读锁”和“排他写锁”实现。读取数据的事务允许其他事务继续访问该行数据，但是未提交的写事务将会禁止其他事务访问该行。

```sql
-- A窗口
set transaction isolation level  read committed;
start transaction;
select * from account;--发现a帐户是1000元，转到b窗口

-- B窗口
start transaction;
update account set money=money+100 where name='aaa';
commit;--转到a窗口

-- A窗口
select * from account;--发现a帐户多了100,这时候，a读到了别的事务提交的数据，两次读取a帐户读到的是不同的结果（不可重复读）
```

#### （3）可重复读取（Repeatable Read）(mysql默认级别)

可重复读取（Repeatable Read）：禁止不可重复读取和脏读取，但是有时可能出现幻读数据和虚读。这可以通过“共享读锁”和“排他写锁”实现。读取数据的事务将会禁止写事务（但允许读事务），写事务则禁止任何其他事务。

```sql
-- A窗口
set transaction isolation level repeatable read;
start transaction;
select * from account;--发现表有4个记录，转到b窗口

-- B窗口
start transaction;
insert into account(name,money) values('ggg',1000);
commit;--转到a窗口

-- A窗口
select * from account;--可能发现表有5条记录，这时候发生了a读取到另外一个事务插入的数据（虚读）
```

#### （4）序列化（Serializable）

序列化（Serializable）：提供严格的事务隔离。它要求事务序列化执行，事务只能一个接着一个地执行，不能并发执行。仅仅通过“行级锁”是无法实现事务序列化的，必须通过其他机制保证新插入的数据不会被刚执行查询操作的事务访问到。

```sql
-- A窗口
set transaction isolation level Serializable;
start transaction;
select * from account;--转到b窗口

-- B窗口
start transaction;
insert into account(name,money) values('ggg',1000);--发现不能插入，只能等待a结束事务才能插入
```


从上面可以看出来，通过选择事务的隔离级别，可以很好的解决上面的4中事务问题

#### （5）常见数据库的默认级别

**注：4 种事务隔离级别从上往下，级别越高，并发性越差，安全性就越来越高。一般数据默认级别是读已提交或可重复读。**

常见数据库的默认级别：

- MySQL 数据库的默认隔离级别是 REPEATABLE_READ(可重复读) 级别。所以mysql中不会出现脏读、不可重复读，但是会出现幻读。
- Oracle数据库中，只支持 SERIALIZABLE 和 READ_COMMITTED级别，默认的是 READ_COMMITTED 级别。
- SQL Server 数据库中，默认的是 READ_COMMITTED(读已提交) 级别。

MySQL下的指令：

查看事务隔离级别

```sql
show variables like 'tx_isolation';
```

![img](http://cdn.bluecusliyou.com/202208061942519.png)

设置事务隔离级别**(仅仅针对当前会话有效)**

```sql
set tx_isolation='REPEATABLE-READ';
```

###  5、问题的解决

#### （1）脏读

设置事务级别为Read conmiitted或者repeatable read都是可以的。

- A客户端的级别是数据库默认的Repeatable read

- B客户端的级别更改为效率最高的Read committed级别

#### （2）幻读

修改事务的隔离级别为Repeatable Read，或者是Serializable。

- Repeatable Read从理论的角度是会出现幻读的，这也就是限制了Repeatable Read这个事务隔离级别使用。一个事务隔离级别推出使用发现其自身带有缺陷，开发者自然会想到完善的方法，所以MySQL内部通过多版本控制机制【实际上就是对读取到的数据加锁】解决这个问题。最后，用户才可以放心大胆使用Repeatable Read这个事务隔离级别。

- Serializable 和 Repeatable Read都可以防止幻读。但是Serializable 事务隔离级别效率低下，比较耗数据库性能，一般不使用。

#### （3）不重复读

设置事务级别为repeatable read，Serializable太耗费性能了，不推荐

#### （4）更新丢失

Serializable虽然可以防止更新丢失，但是效率太低，通常数据库不会用这个隔离级别，所以我们需要其他的机制来防止更新丢失.

- 使用排它锁(悲观锁)。


经过上面基于数据库锁的介绍可知，丢失更新可以使用写锁(排它锁)进行控制。因为排它锁添加到某个表的时候，事务未经提交，其他的事务根本没法获取修改权，因此排它锁可以用来控制丢失更新。需要说明的是有时候，当知道某一行会发生并发修改的时候，可以把锁定的范围缩小。例如使用select * from t_account t wheret.id='1' for update; 这样能够比较好地把控上锁的粒度，这种基于行级上锁的方法叫"行级锁"。

- 使用乐观锁。


乐观锁的原理是：认为事务不一定会产生丢失更新，让事务进行并发修改，不对事务进行锁定。发现并发修改某行数据时，乐观锁抛出异常。让用户解决。可以通过给数据表添加自增的version字段或时间戳timestamp。进行数据修改时，数据库会检测version字段或者时间戳是否与原来的一致。若不一致，抛出异常。

校验事务B与version值,事务B提交前的version字段值为1，但当前version值为2，禁止事务B提交.抛出异常让用户处理。

![image-20220705193518291](http://cdn.bluecusliyou.com/202207051935375.png)

### 6、事务实现

#### （1）基本语法

```sql
-- 使用set语句来改变自动提交模式 
SET autocommit = 0; /*关闭*/ 
SET autocommit = 1; /*开启*/ 
-- 注意: 
--- 1.MySQL中默认是自动提交 
--- 2.使用事务时应先关闭自动提交 

-- 开始一个事务,标记事务的起始点 
START TRANSACTION

-- 提交一个事务给数据库 
COMMIT 

-- 将事务回滚,数据回到本次事务的初始状态 
ROLLBACK 

-- 还原MySQL数据库的自动提交 
SET autocommit =1; 

-- 保存点 
SAVEPOINT 保存点名称 -- 设置一个事务保存点 
ROLLBACK TO SAVEPOINT 保存点名称 -- 回滚到保存点 
RELEASE SAVEPOINT 保存点名称 -- 删除保存点
```

#### （2）事务处理的步骤

![image-20220705195721216](http://cdn.bluecusliyou.com/202207051957379.png)



#### （3）案例实现

```sql
/*
A在线买一款价格为500元商品,网上银行转账. A的银行卡余额为2000,然后给商家B支付500. 商家B一开始的银行卡余额为10000 创建数据库shop和创建表account并插入2条数据 
*/

-- 创建数据库
CREATE DATABASE `shop` CHARACTER 
SET utf8 COLLATE utf8_general_ci;
USE `shop`;

-- 创建数据表
CREATE TABLE `account` (
	`id` INT ( 11 ) NOT NULL AUTO_INCREMENT,
	`name` VARCHAR ( 32 ) NOT NULL,
	`cash` DECIMAL ( 9, 2 ) NOT NULL,
PRIMARY KEY ( `id` ) 
) ENGINE = INNODB DEFAULT CHARSET = utf8;

-- 插入测试数据
INSERT INTO account ( `name`, `cash` ) VALUES ( 'A', 2000.00 ),( 'B', 10000.00 );

-- 转账实现
SET autocommit = 0;-- 关闭自动提交
START TRANSACTION;-- 开始一个事务,标记事务的起始点
UPDATE account 
SET cash = cash - 500 
WHERE
	`name` = 'A';
UPDATE account 
SET cash = cash + 500 
WHERE
	`name` = 'B';
COMMIT;-- 提交事务 
# rollback;
SET autocommit = 1;-- 恢复自动提交
```

###  7、补充说明

- SQL规范所规定的标准，不同的数据库具体的实现可能会有些差异
- mysql中默认事务隔离级别是可重复读时并不会锁住读取到的行

- 事务隔离级别为读提交时，写数据只会锁住相应的行

- 事务隔离级别为可重复读时，如果有索引（包括主键索引）的时候，以索引列为条件更新数据，会存在间隙锁间隙锁、行锁、下一键锁的问题，从而锁住一些行；如果没有索引，更新数据时会锁住整张表。

- 事务隔离级别为串行化时，读写数据都会锁住整张表。

- 隔离级别越高，越能保证数据的完整性和一致性，但是对并发性能的影响也越大，鱼和熊掌不可兼得啊。对于多数应用程序，可以优先考虑把数据库系统的隔离级别设为Read Committed，它能够避免脏读取，而且具有较好的并发性能。尽管它会导致不可重复读、幻读这些并发问题，在可能出现这类问题的个别场合，可以由应用程序采用悲观锁或乐观锁来控制。

## 十二、锁机制

### 1、锁定义

- 锁是计算机协调多个进程或线程并发访问某一资源的机制。
- 在数据库中，除了传统的计算资源（如CPU、RAM、I/O等）的争用以外，数据也是一种供需要用户共享的资源。如何保证数据并发访问的一致性、有效性是所有数据库必须解决的一个问题，锁冲突也是影响数据库并发访问性能的一个重要因素 。

### 2、锁分类

#### （1）从操作的粒度可分为表级锁、行级锁和页级锁

- 表级锁 :每次操作锁住整张表。锁定粒度大，发生锁冲突的概率最高，并发度最低。应用在MyISAM、InnoDB、BDB 等存储引擎中。

- 行级锁 :每次操作锁住一行数据。锁定粒度最小，发生锁冲突的概率最低，并发度最高。应用在InnoDB 存储引擎中。

- 页级锁 :每次锁定相邻的一组记录，锁定粒度界于表锁和行锁之间，开销和加锁时间界于表锁和行锁之间，并发度一般。应用在BDB存储引擎中。

  ![image-20220707161545012](http://cdn.bluecusliyou.com/202207071615097.png)

#### （2）从操作的类型可分为读锁和写锁

- 读锁(S锁)：共享锁，针对同一份数据，多个读操作可以同时进行而不会互相影响。
- 写锁(X锁)：排他锁，当前写操作没有完成前，它会阻断其他写锁和读锁。

- IS锁、IX锁：意向读锁、意向写锁，属于表级锁，S和X主要针对行级锁。在对表记录添加S或X锁之前，会先对表添加IS或IX锁。
- S锁：事务A对记录添加了S锁，可以对记录进行读操作，不能做修改，其他事务可以对该记录追加S锁，但是不能追加X锁，需要追加X锁，需要等记录的S锁全部释放。
- X锁：事务A对记录添加了X锁，可以对记录进行读和修改操作，其他事务不能对记录做读和修改操作。

#### （3）从操作的性能可分为乐观锁和悲观锁

- 乐观锁 :一般的实现方式是对记录数据版本进行比对，在数据更新提交的时候才会进行冲突 检测，如果发现冲突了，则提示错误信息。
- 悲观锁 :在对一条数据修改的时候，为了避免同时被其他人修改，在修改数据之前先锁定， 再修改的控制方式。共享锁和排他锁是悲观锁的不同实现，但都属于悲观锁范畴。

### 3、行锁原理

#### （1）主要实现算法

在InnoDB引擎中，我们可以使用行锁和表锁，其中行锁又分为共享锁和排他锁。InnoDB行锁是通过对索引数据页上的记录加锁实现的，主要实现算法有 3 种：Record Lock、Gap Lock 和 Next-key Lock。

- RecordLock锁：锁定单个行记录的锁。（记录锁，RC、RR隔离级别都支持）

- GapLock锁：间隙锁，锁定索引记录间隙，确保索引记录的间隙不变。（范围锁，RR隔离级别支持）

- Next-key Lock 锁：记录锁和间隙锁组合，同时锁住数据，并且锁住数据前后范围。（记录锁+范围锁，RR隔离级别支持）

#### （2）常见场景

在RR隔离级别，InnoDB对于记录加锁行为都是先采用Next-Key Lock，但是当SQL操作含有唯一索引时，Innodb会对Next-Key Lock进行优化，降级为RecordLock，仅锁住索引本身而非范围。

- select … from 语句：InnoDB引擎采用MVCC机制实现非阻塞读，所以对于普通的select语句，InnoDB不加锁

- select … from lock in share mode语句：追加了共享锁，InnoDB会使用Next-Key Lock锁进行处理，如果扫描发现唯一索引，可以降级为RecordLock锁。

- select … from for update语句：追加了排他锁，InnoDB会使用Next-Key Lock锁进行处理，如果扫描发现唯一索引，可以降级为RecordLock锁。

- update … where 语句：InnoDB会使用Next-Key Lock锁进行处理，如果扫描发现唯一索引，可以降级为RecordLock锁。

- delete … where 语句：InnoDB会使用Next-Key Lock锁进行处理，如果扫描发现唯一索引，可以降级为RecordLock锁。

- insert语句：InnoDB会在将要插入的那一行设置一个排他的RecordLock锁。


#### （3）案例分析

下面以“update t1 set name=‘XX’ where id=10”操作为例，举例子分析下 InnoDB 对不同索引的加锁行为，以RR隔离级别为例。

> 主键加锁
>
> 加锁行为：仅在id=10的主键索引记录上加X锁。

![image-20220707162410682](http://cdn.bluecusliyou.com/202207071624775.png)

> 唯一键加锁
>
> 加锁行为：现在唯一索引id上加X锁，然后在id=10的主键索引记录上加X锁。

![image-20220707162456243](http://cdn.bluecusliyou.com/202207071624391.png)

> 非唯一键加锁
>
> 加锁行为：对满足id=10条件的记录和主键分别加X锁，然后在(6,c)-(10,b)、(10,b)-(10,d)、(10,d)-(11,f)范围分别加Gap Lock。

![image-20220707162708862](http://cdn.bluecusliyou.com/202207071627960.png)

> 无索引加锁
>
> 加锁行为：表里所有行和间隙都会加X锁。（当没有索引时，会导致全表锁定，因为InnoDB引擎锁机制是基于索引实现的记录锁定）。

![image-20220707162849834](http://cdn.bluecusliyou.com/202207071628947.png)

### 4、悲观锁

悲观锁（Pessimistic Locking），是指在数据处理过程，将数据处于锁定状态，一般使用数据库的锁机制实现。从广义上来讲，前面提到的行锁、表锁、读锁、写锁、共享锁、排他锁等，这些都属于悲观锁范畴。

#### （1）表级锁

表级锁每次操作都锁住整张表，并发度最低。常用命令如下：

```sql
-- 手动增加表锁
lock table 表名称 read|write,表名称2 read|write;

-- 查看表上加过的锁
show open tables;

-- 删除表锁
unlock tables;
```

- 
  表级读锁：当前表追加read锁，当前连接和其他的连接都可以读操作；但是当前连接增删改操作会报错，其他连接增删改会被阻塞。
- 表级写锁：当前表追加write锁，当前连接可以对表做增删改查操作，其他连接对该表所有操作都被阻塞（包括查询）。


- 总结：表级读锁会阻塞写操作，但是不会阻塞读操作。而写锁则会把读和写操作都阻塞。


#### （2）共享锁（行级锁-读锁）

- 共享锁又称为读锁，简称S锁。共享锁就是多个事务对于同一数据可以共享一把锁，都能访问到数据，但是只能读不能修改。使用共享锁的方法是在select … lock in share mode，只适用查询语句。
- 总结：事务使用了共享锁（读锁），只能读取，不能修改，修改操作被阻塞， 本事务和其他事务的修改操作都会被阻塞。


#### （3）排他锁（行级锁-写锁）

- 排他锁又称为写锁，简称X锁。排他锁就是不能与其他锁并存，如一个事务获取了一个数据行的排他锁，其他事务就不能对该行记录做其他操作，也不能获取该行的锁，但是同一个事务内还可以进行写操作。
- 使用排他锁的方法是在SQL末尾加上for update，innodb引擎默认会在update，delete语句加上for update。行级锁的实现其实是依靠其对应的索引，所以如果操作没用到索引的查询，那么会锁住全表记录。

### 5、乐观锁

#### （1）概述

- 乐观锁是相对于悲观锁而言的，它不是数据库提供的功能，需要开发者自己去实现。在数据库操作时，想法很乐观，认为这次的操作不会导致冲突，因此在数据库操作时并不做任何的特殊处理，即不加锁，而是在进行事务提交时再去判断是否有冲突了。
- 乐观锁实现的关键点：冲突的检测。

- 悲观锁和乐观锁都可以解决事务写写并发，在应用中可以根据并发处理能力选择区分，比如对并发率要求高的选择乐观锁；对于并发率要求低的可以选择悲观锁。


#### （2）实现原理

> 使用版本字段（version）

先给数据表增加一个版本(version) 字段，每操作一次，将那条记录的版本号加 1。version是用来查看被读的记录有无变化，作用是防止记录在业务处理期间被其他事务修改。

> 使用时间戳（Timestamp）

与使用version版本字段相似，同样需要给在数据表增加一个字段，字段类型使用timestamp时间戳。也是在更新提交的时候检查当前数据库中数据的时间戳和自己更新前取到的时间戳进行对比，如果一致则提交更新，否则就是版本冲突，取消操作。

> orm框架封装的乐观锁

除了自己手动实现乐观锁之外，许多数据库访问框架也封装了乐观锁的实现，比如hibernate框架。MyBatis框架大家可以使用OptimisticLocker插件来扩展。

#### （3）乐观锁案例

下面我们使用下单过程作为案例，描述下乐观锁的使用。

```sql
-- 第一步：查询商品信息
select (quantity,version) from products where id=1;

-- 第二部：根据商品信息生成订单
insert into orders ...	
insert into items ...

-- 第三部：修改商品库存
update products set quantity=quantity-1,version=version+1where id=1 and version=#{version};
```

### 6、死锁与解决方案

下面介绍几种常见的死锁现象和解决方案：

#### （1）表锁死锁

> 产生原因

用户A访问表A（锁住了表A），然后又访问表B；另一个用户B访问表B（锁住了表B），然后企图访问表A；这时用户A由于用户B已经锁住表B，它必须等待用户B释放表B才能继续，同样用户B要等用户A释放表A才能继续，这就死锁就产生了。
用户A–》A表（表锁）–》B表（表锁）
用户B–》B表（表锁）–》A表（表锁）

> 解决方案

这种死锁比较常见，是由于程序的BUG产生的，除了调整的程序的逻辑没有其它的办法。仔细分析程序的逻辑，对于数据库的多表操作时，尽量按照相同的顺序进行处理，尽量避免同时锁定两个资源，如操作A和B两张表时，总是按先A后B的顺序处理， 必须同时锁定两个资源时，要保证在任何时刻都应该按照相同的顺序来锁定资源。

#### （2）行级锁死锁

> 产生原因1：

InnoDB的行锁是针对索引加的锁，不是针对记录加的锁。并且该索引不能失效，否则都会从行锁升级为表锁。

如果在事务中执行了一条没有索引条件的查询，引发全表扫描，把行级锁上升为全表记录锁定（等价于表级锁），多个这样的事务执行后，就很容易产生死锁和阻塞，最终应用系统会越来越慢，发生阻塞或死锁。

> 解决方案1：

SQL语句中不要使用太复杂的关联多表的查询；使用explain“执行计划"对SQL语句进行分析，对于有全表扫描和全表锁定的SQL语句，建立相应的索引进行优化。

> 产生原因2：

两个事务分别想拿到对方持有的锁，互相等待，于是产生死锁。

![image-20220707183609045](http://cdn.bluecusliyou.com/202207071836140.png)

> 解决方案2：

- 在同一个事务中，尽可能做到一次锁定所需要的所有资源
- 按照id对资源排序，然后按顺序进行处理

#### （3）共享锁转换为排他锁

> 产生原因：

事务A 查询一条纪录，然后更新该条纪录；此时事务B 也更新该条纪录，这时事务B 的排他锁由于事务A 有共享锁，必须等A 释放共享锁后才可以获取，只能排队等待。事务A 再执行更新操作时，此处发生死锁，因为事务A 需要排他锁来做更新操作。但是，无法授予该锁请求，因为事务B 已经有一个排他锁请求，并且正在等待事务A 释放其共享锁。

```sql
事务A: select * from dept where deptno=1 lock in share mode; //共享锁,1
			update dept set dname='java' where deptno=1;//排他锁,3
事务B: update dept set dname='Java' where deptno=1;//由于1有共享锁，没法获取排他锁，需等待，2
```

> 解决方案：

- 对于按钮等控件，点击立刻失效，不让用户重复点击，避免引发同时对同一条记录多次操作；

- 使用乐观锁进行控制。乐观锁机制避免了长事务中的数据库加锁开销，大大提升了大并发量下的系统性能。需要注意的是，由于乐观锁机制是在我们的系统中实现，来自外部系统的用户更新操作不受我们系统的控制，因此可能会造成脏数据被更新到数据库中；


#### （4）死锁排查

MySQL提供了几个与锁有关的参数和命令，可以辅助我们优化锁操作，减少死锁发生。

> 查看死锁日志

```sql
show engine innodb status;
```

- 查看近期死锁日志信息
- 使用explain查看下SQL执行计划

> 查看锁状态变量

```sql
show status like'innodb_row_lock%;
```

- Innodb_row_lock_current_waits：当前正在等待锁的数量

- Innodb_row_lock_time：从系统启动到现在锁定总时间长度

- Innodb_row_lock_time_avg： 每次等待锁的平均时间

- Innodb_row_lock_time_max：从系统启动到现在等待最长的一次锁的时间

- Innodb_row_lock_waits：系统启动后到现在总共等待的次数

如果等待次数高，而且每次等待时间长，需要分析系统中为什么会有如此多的等待，然后着手定制优化。

#### （5）优化建议

- 尽可能让所有数据检索都通过索引来完成，避免无索引行锁升级为表锁
- 合理设计索引，尽量缩小锁的范围
- 尽量控制事务大小，减少锁定资源量和时间长度，涉及事务加锁的sql
- 尽可能降低事务隔离级别

