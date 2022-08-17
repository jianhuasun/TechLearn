# MySql优化详解

MySql基础知识请参考[MySql基础详解](https://blog.csdn.net/liyou123456789/article/details/126023696)
MySql优化知识请参考[MySql优化详解](https://blog.csdn.net/liyou123456789/article/details/126297268)

## 一、慢查询分析

### 1、SQL性能差原因

- 索引失效：索引建了，但是没有用上。
- 关联查询太多 `join` （设计缺陷或者不得已的需求）。
- 服务器调优以及各个参数的设置（缓冲、线程数等）。

### 2、分析慢SQL的步骤

- 观察，至少跑1天，看看生产的慢SQL情况。
- 开启慢查询日志，设置阈值，比如超过5秒钟的就是慢SQL，并将它抓取出来。
- explain + 慢SQL分析。
- show Profile查询SQL在MySQL数据库中的执行细节和生命周期情况。
- 运维经理 OR DBA，进行MySQL数据库服务器的参数调优。

### 3、慢查询日志

#### （1）慢查询日志是什么？

- MySQL的慢查询日志是MySQL提供的一种日志记录，它用来记录在MySQL中响应时间超过阈值的语句，具体指运行时间超过 `long_query_time` 值的SQL，则会被记录到慢查询日志中。
- `long_query_time` 的默认值为10，意思是运行10秒以上的语句。
- 由慢查询日志来查看哪些SQL超出了我们的最大忍耐时间值，比如一条SQL执行超过5秒钟，我们就算慢SQL，希望能收集超过5秒钟的SQL，结合之前 explain 进行全面分析。

#### （2）特别说明

- **默认情况下，MySQL数据库没有开启慢查询日志，**需要我们手动来设置这个参数。
- **当然，如果不是调优需要的话，一般不建议启动该参数**，因为开启慢查询日志会或多或少带来一定的性能影响。慢查询日志支持将日志记录写入文件。

#### （3）查看慢查询日志是否开启以及如何开启

- 查看慢查询日志是否开启： `SHOW VARIABLES LIKE '%slow_query_log%';` 。
- 开启慢查询日志： `SET GLOBAL slow_query_log = 1;` 。**使用该方法开启MySQL的慢查询日志只对当前数据库生效，如果MySQL重启后会失效。**

```sql
# 1、查看慢查询日志是否开启
mysql> SHOW VARIABLES LIKE '%slow_query_log%';
+---------------------+--------------------------------------+
| Variable_name       | Value                                |
+---------------------+--------------------------------------+
| slow_query_log      | OFF                                  |
| slow_query_log_file | /var/lib/mysql/6d6724c8f6ef-slow.log |
+---------------------+--------------------------------------+
# 2、开启慢查询日志
SET GLOBAL slow_query_log = 1;
# 3、关闭慢查询日志
SET GLOBAL slow_query_log = 0;
```

- 如果要永久开启慢查询日志，需要修改 `my.cnf` 文件，在 `[mysqld]` 下增加修改参数。

```ini
# my.cnf
[mysqld]
# 1.这个是开启慢查询。注意ON需要大写
slow_query_log=ON  
# 2.这个是存储慢查询的日志文件。这个文件不存在的话，需要自己创建
slow_query_log_file=/var/lib/mysql/slow.log
```

#### （4）开启了慢查询日志后，什么样的SQL才会被记录到慢查询日志里面呢？

- 这个是由参数 `long_query_time` 控制的，默认情况下 `long_query_time` 的值为10秒。
- MySQL中查看 `long_query_time` 的时间： `SHOW VARIABLES LIKE 'long_query_time%';` 。

```sql
# 查看long_query_time 默认是10秒
# 只有SQL的执行时间>10才会被记录
mysql> SHOW VARIABLES LIKE 'long_query_time%';
+-----------------+-----------+
| Variable_name   | Value     |
+-----------------+-----------+
| long_query_time | 10.000000 |
+-----------------+-----------+
```

- 修改 `long_query_time` 的时间，需要在 `my.cnf` 修改配置文件

```ini
[mysqld]
# 这个是设置慢查询的时间，我设置的为1秒
long_query_time=1
```

- 查询慢查询日志的总记录条数： `SHOW GLOBAL STATUS LIKE '%Slow_queries%';` 。

```sql
mysql> SHOW GLOBAL STATUS LIKE '%Slow_queries%';
+---------------+-------+
| Variable_name | Value |
+---------------+-------+
| Slow_queries  | 3     |
+---------------+-------+
```

#### （5）日志分析工具mysqldumpslow 

日志分析工具 `mysqldumpslow` ：在生产环境中，如果要手工分析日志，查找、分析SQL，显然是个体力活，MySQL提供了日志分析工具 `mysqldumpslow` 。

```sql
# 1、mysqldumpslow --help 来查看mysqldumpslow的帮助信息
root@1dcb5644392c:/usr/bin# mysqldumpslow --help
Usage: mysqldumpslow [ OPTS... ] [ LOGS... ]
Parse and summarize the MySQL slow query log. Options are
 --verbose   verbose
 --debug     debug
 --help       write this text to standard output
 -v           verbose
 -d           debug
 -s ORDER     what to sort by (al, at, ar, c, l, r, t), 'at' is default # 按照何种方式排序
               al: average lock time # 平均锁定时间
               ar: average rows sent # 平均返回记录数
               at: average query time # 平均查询时间
                 c: count # 访问次数
                 l: lock time # 锁定时间
                 r: rows sent # 返回记录
                 t: query time # 查询时间
 -r           reverse the sort order (largest last instead of first)
 -t NUM       just show the top n queries # 返回前面多少条记录
 -a           don't abstract all numbers to N and strings to 'S'
 -n NUM       abstract numbers with at least n digits within names
 -g PATTERN   grep: only consider stmts that include this string  
 -h HOSTNAME hostname of db server for *-slow.log filename (can be wildcard),
               default is '*', i.e. match all
 -i NAME     name of server instance (if using mysql.server startup script)
 -l           don't subtract lock time from total time
  
# 2、 案例
# 2.1、得到返回记录集最多的10个SQL
mysqldumpslow -s r -t 10 /var/lib/mysql/slow.log
# 2.2、得到访问次数最多的10个SQL
mysqldumpslow -s c -t 10 /var/lib/mysql/slow.log
# 2.3、得到按照时间排序的前10条里面含有左连接的查询语句
mysqldumpslow -s t -t 10 -g "left join" /var/lib/mysql/slow.log
# 2.4、另外建议使用这些命令时结合|和more使用，否则出现爆屏的情况
mysqldumpslow -s r -t 10 /var/lib/mysql/slow.log | more
```

### 4、Show Profile

#### （1）数据准备

> 建表

```sql
#班级表
CREATE TABLE `class` (
`id` INT(11) NOT NULL AUTO_INCREMENT,
`className` VARCHAR(30) DEFAULT NULL,
`address` VARCHAR(40) DEFAULT NULL,
`monitor` INT NULL ,
PRIMARY KEY (`id`)
) ENGINE=INNODB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4;

#学员表
CREATE TABLE `student` (
`id` INT(11) NOT NULL AUTO_INCREMENT,
`stuno` INT NOT NULL ,
`name` VARCHAR(20) DEFAULT NULL,
`age` INT(3) DEFAULT NULL,
`classId` INT(11) DEFAULT NULL,
PRIMARY KEY (`id`)
#CONSTRAINT `fk_class_id` FOREIGN KEY (`classId`) REFERENCES `t_class` (`id`)
) ENGINE=INNODB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4;
```

> 由于开启过慢查询日志，开启了 bin-log ，我们就必须为 function 指定一个参数，否则使用函数会报错。不加global只是当前窗口有效。

```sql
# 在mysql中设置
# log_bin_trust_function_creators 默认是关闭的 需要手动开启
mysql> SHOW VARIABLES LIKE 'log_bin_trust_function_creators';
+---------------------------------+-------+
| Variable_name                   | Value |
+---------------------------------+-------+
| log_bin_trust_function_creators | OFF   |
+---------------------------------+-------+
1 row in set (0.00 sec)
mysql> SET GLOBAL log_bin_trust_function_creators=1;
Query OK, 0 rows affected (0.00 sec)
```

> 上述修改方式MySQL重启后会失败，在 my.cnf 配置文件下修改永久有效。

```ini
[mysqld]
log_bin_trust_function_creators=ON
```

> 创建函数

```sql
#随机产生字符串
DELIMITER //
CREATE FUNCTION rand_string(n INT) RETURNS VARCHAR(255)
BEGIN  
DECLARE chars_str VARCHAR(100) DEFAULT
'abcdefghijklmnopqrstuvwxyzABCDEFJHIJKLMNOPQRSTUVWXYZ';
DECLARE return_str VARCHAR(255) DEFAULT '';
DECLARE i INT DEFAULT 0;
WHILE i < n DO 
SET return_str =CONCAT(return_str,SUBSTRING(chars_str,FLOOR(1+RAND()*52),1)); 
SET i = i + 1;
END WHILE;
RETURN return_str;
END //
DELIMITER ;

#假如要删除
#drop function rand_string;

#用于随机产生多少到多少的编号
DELIMITER //
CREATE FUNCTION rand_num (from_num INT ,to_num INT) RETURNS INT(11)
BEGIN 
DECLARE i INT DEFAULT 0; 
SET i = FLOOR(from_num +RAND()*(to_num - from_num+1))  ;
RETURN i; 
END //
DELIMITER ;

#假如要删除
#drop function rand_num;
```

> 创建存储过程

```sql
#创建往stu表中插入数据的存储过程
DELIMITER //
CREATE PROCEDURE insert_stu(  START INT , max_num INT )
BEGIN 
	DECLARE i INT DEFAULT 0; 
	SET autocommit = 0;   #设置手动提交事务
	REPEAT  #循环
	SET i = i + 1;  #赋值
	INSERT INTO student (stuno, name ,age ,classId ) VALUES
	((START+i),rand_string(6),rand_num(1,50),rand_num(1,1000)); 
	UNTIL i = max_num 
	END REPEAT; 
	COMMIT;  #提交事务
END //
DELIMITER ;

#假如要删除
#drop PROCEDURE insert_stu;

#执行存储过程，往class表添加随机数据
DELIMITER //
CREATE PROCEDURE `insert_class`( max_num INT )
BEGIN 
	DECLARE i INT DEFAULT 0; 
	SET autocommit = 0;  
	REPEAT 
	SET i = i + 1; 
	INSERT INTO class ( classname,address,monitor ) VALUES
	(rand_string(8),rand_string(10),rand_num(1,100000)); 
	UNTIL i = max_num 
	END REPEAT; 
	COMMIT;
END //
DELIMITER ;

#假如要删除
#drop PROCEDURE insert_class;
```

> 调用存储过程

```sql
#执行存储过程，往class表添加1万条数据 
CALL insert_class(10000);
#执行存储过程，往stu表添加50万条数据 
CALL insert_stu(100000,500000);
```

#### （2）Show Profile是什么？

Show Profile ：MySQL提供可以用来分析当前会话中语句执行的资源消耗情况。可以用于SQL的调优的测量。**默认情况下，参数处于关闭状态，并保存最近15次的运行结果。**

#### （3）分析步骤

> 是否支持，看看当前的MySQL版本是否支持。

```sql
# 查看Show Profile功能是否开启
mysql> SHOW VARIABLES LIKE 'profiling';
+---------------+-------+
| Variable_name | Value |
+---------------+-------+
| profiling     | OFF   |
+---------------+-------+
```

> 开启 `Show Profile` 功能，默认是关闭的，使用前需要开启。

```sql
# 开启Show Profile功能
mysql> SET profiling=ON;
```

> 运行SQL

```sql
SELECT * FROM `student` GROUP BY `id`%10 LIMIT 150000;
SELECT * FROM `student` GROUP BY `id`%20 ORDER BY name;
```

> 查看结果，执行 `SHOW PROFILES;``Duration` ：持续时间。

```sql
SHOW PROFILES;
```

> 诊断SQL， `SHOW PROFILE cpu,block io FOR QUERY Query_ID;`

```sql
# 这里的3是第四步中的Query_ID。
# 可以在SHOW PROFILE中看到一条SQL中完整的生命周期。
mysql> SHOW PROFILE cpu,block io FOR QUERY 3;
```

#### （4）Show Profile 查询参数备注

- ALL ：显示所有的开销信息。
- BLOCK IO ：显示块IO相关开销（通用）。
- CONTEXT SWITCHES ：上下文切换相关开销。
- CPU ：显示CPU相关开销信息（通用）。
- IPC ：显示发送和接收相关开销信息。
- MEMORY ：显示内存相关开销信息。
- PAGE FAULTS ：显示页面错误相关开销信息。
- SOURCE ：显示和Source_function。
- SWAPS ：显示交换次数相关开销的信息。

#### （5）日常开发需要注意的结论

- converting HEAP to MyISAM ：查询结果太大，内存都不够用了，往磁盘上搬了。
- Creating tmp table ：创建临时表（拷贝数据到临时表，用完再删除），非常耗费数据库性能。
- Copying to tmp table on disk ：把内存中的临时表复制到磁盘，危险！
- locked ：死锁。

## 二、执行计划

### 1、概述

项目开发中，性能往往都是是我们重点关注的问题，其实很多时候一个SQL往往是整个请求中瓶颈最大的地方，因此我们必须了解SQL语句的执行过程来帮助我们做SQL语句的优化。

MySQL提供了 explain / desc 语句，来显示这条SQL语句的执行计划，执行计划可以帮助我们查看SQL语句的执行情况。explain对select，delete，update，insert，replace语句有效。

explain extended：会在 explain 的基础上额外提供一些查询优化的信息。紧随其后通过 show warnings 命令可以得到优化后的查询语句，从而看出优化器优化了什么。额外还有 filtered 列，是一个半分比的值，rows * filtered/100 可以估算出将要和 explain 中前一个表 进行连接的行数（前一个表指 explain 中的id值比当前表id值小的表）。

explain partitions：相比 explain 多了个 partitions 字段，如果查询是基于分区表的话，会显示查询将访问的分区。

### 2、数据准备

```sql
-- 创建数据库
create database test;

-- 创建数据表
CREATE TABLE `test`.`role` (
	`id` INT ( 11 ) NOT NULL,
	`name` VARCHAR ( 255 ) CHARACTER 
	SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
	PRIMARY KEY ( `id` ) USING BTREE 
) ENGINE = INNODB CHARACTER 
SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

CREATE TABLE `test`.`user` (
	`id` INT ( 11 ) NOT NULL,
	`name` VARCHAR ( 255 ) CHARACTER 
	SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
	`role_id` INT ( 11 ) NULL DEFAULT NULL,
	PRIMARY KEY ( `id` ) USING BTREE 
) ENGINE = INNODB CHARACTER 
SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;
```

### 3、desc和explain

#### （1）EXPLAIN是什么？

EXPLAIN：SQL的执行计划，使用EXPLAIN关键字可以模拟优化器执行SQL查询语句，从而知道MySQL是如何处理SQL语句的。

#### （2）EXPLAIN怎么使用？

语法： `explain + SQL` 。

explain 和 desc 效果一样，两个语句的效果是一模一样的，我们后面就统一使用 explain。

```sql
explain select * from user;
```

![image-20220707224229265](http://cdn.bluecusliyou.com/202207072242392.png)

```sql
desc select * from user;
```

![image-20220707224438879](http://cdn.bluecusliyou.com/202207072244948.png)

#### （3）EXPLAIN用途

- 表的读取顺序如何
- 数据读取操作有哪些操作类型
- 哪些索引可以使用
- 哪些索引被实际使用
- 表之间是如何引用
- 每张表有多少行被优化器查询

#### （4）EXPLAIN包含的字段

- id //select查询的序列号，包含一组数字，表示查询中执行select子句或操作表的顺序
- select_type //查询类型
- table //正在访问哪个表
- partitions //匹配的分区
- type //访问的类型
- possible_keys //显示可能应用在这张表中的索引，一个或多个，但不一定实际使用到
- key //实际使用到的索引，如果为NULL，则没有使用索引
- key_len //表示索引中使用的字节数，可通过该列计算查询中使用的索引的长度
- ref //显示索引的哪一列被使用了，如果可能的话，是一个常数，哪些列或常量被用于查找索引列上的值
- rows //根据表统计信息及索引选用情况，大致估算出找到所需的记录所需读取的行数
- filtered //查询的表行占表的百分比
- Extra //包含不适合在其它列中显示但十分重要的额外信息

### 4、 id字段

#### （1）id相同，越靠前的表越先执行

```sql
explain select * from user u left join role r on u.role_id=r.id;
```

![image-20220707224517551](http://cdn.bluecusliyou.com/202207072245622.png)

#### （2）id不同，id越大的表越先执行

```sql
explain select * from user u where u.role_id=(select id from role r where
r.id=1);
```

![image-20220707224548010](http://cdn.bluecusliyou.com/202207072245109.png)

#### （3）id有相同，也有不同，id越大的表越先执行，在id相同的表中，id越靠前的表越先执行

### 5、**select_type** 字段

#### （1）SIMPLE：简单的 select 查询，查询中不包含子查询或者 union

```sql
explain select * from user;
```

![image-20220708205418622](http://cdn.bluecusliyou.com/202207082054694.png)

#### （2）PRIMARY：查询条件中包含有子查询时最外层的表（u1）

#### （3）SUBQUERY：条件子查询中的表（u2）

```sql
explain select * from user u1 where u1.id =(select id from user u2 where u2.id=1);
```

![image-20220708205658380](http://cdn.bluecusliyou.com/202207082056454.png)

#### （3）SUBQUERY：条件中的子查询中的表（包括多重层级）（u2，u3）

```sql
explain select * from user u1 where u1.name =(select name from user u2 where u2.name=(select name from user u3 where u3.name='zs'));
```

![image-20220708211941133](http://cdn.bluecusliyou.com/202207082119208.png)

#### （4）UNION：使用到union关联时，union关联的表（u2）

#### （5）UNION RESULT：使用union时，最终的结果集表（<union1,2>）

```sql
explain select * from user u1 union select * from user u2;
```

![image-20220708205839409](http://cdn.bluecusliyou.com/202207082058488.png)

#### （6）DEPENDENT UNION：在子查询中使用到union的第二个以上的表（u3,u4）

#### （7）DEPENDENT SUBQUERY：在子查询中，使用到union的第一表（u2）

```sql
explain select * from user u1 where u1.id in (select id from user u2 where u2.id=1 union select id from user u3 where u3.id=2 union select id from user u4 where u4.id=3);
```

![image-20220708210703343](http://cdn.bluecusliyou.com/202207082107419.png)

#### （7）DEPENDENT SUBQUERY: 子查询中的条件依赖于外部的查询（r1）

```sql
explain select * from user u1 where u1.role_id=(select id from role r1 where u1.id=1);
```

![image-20220708212406272](http://cdn.bluecusliyou.com/202207082124351.png)

#### （8）DERIVED：衍生表的from子表（该子表必须使用union关联其他表）（u1）

```sql
explain select * from(select * from user u1 where u1.role_id=1 union select * from user u2 where u2.name='zs') temp;
```

![image-20220708213536390](http://cdn.bluecusliyou.com/202207082135465.png)

### 6、 **table** 字段

表示该SQL语句是作用于那张表的，取值为：表名、表别名、衍生表名等。

```sql
explain select * from user;
explain select * from user u1;
```

![image-20220708213715115](http://cdn.bluecusliyou.com/202207082137192.png)

### 7、**partitions** 字段

涉及到分区的表

准备数据：

```sql
create table goods_partitions (
 id int auto_increment primary key, 
 name varchar(12)
)
partition by range(id)
(
    partition p0 values less than (10000),
		partition p1 values less than MAXVALUE
);
```

#### （1）查看查询语句所使用到的分区

整个goods_partitions使用到了两个分区

```sql
explain select * from goods_partitions;
```

![image-20220708220336705](http://cdn.bluecusliyou.com/202207082203778.png)

#### （2）查询id<10000的记录（属于p0分区）

```sql
explain select * from goods_partitions where id<10000;
```

![image-20220708220438101](http://cdn.bluecusliyou.com/202207082204176.png)

### 8、type字段

反应一段SQL语句性能指标的重要参数，可以通过此参数来判断是否使用到了索引、是否全表扫描、是否范围查询等。

```bash
NULL>system>const>eq_ref>ref>fulltext>ref_or_null>index_merge>unique_subquery>index_subquery>range>index>ALL //最好到最差
备注：掌握以下10种常见的即可，一般来说，得保证查询至少达到 range 级别，最好达到 ref 。
NULL>system>const>eq_ref>ref>ref_or_null>index_merge>range>index>ALL
```

插入测试数据：

```sql
insert into role values(1,'保洁');
insert into role values(2,'保安');
insert into role values(3,'厨师');
insert into user values(1,'zs',1);
```

#### （1）**null**：代表不访问任何表

MySQL能够在优化阶段分解查询语句，在执行阶段用不着再访问表或索引

```sql
explain select 1;
```

![image-20220708220712596](http://cdn.bluecusliyou.com/202207082207694.png)

#### （2）**system**：表中只有一条记录,并且此表为系统表(一般很少出现)

#### （3）**const**：通过唯一索引或者主键查询到的数据,只查询一次就查询到了

因为只匹配一行数据，所以很快，如主键置于where列表中，MySQL就能将该查询转换为一个常量

```sql
-- id是主键
explain select * from user where id=1;
```

![image-20220708221043895](http://cdn.bluecusliyou.com/202207082210974.png)

> 根据name查询，类型不是const，给name加上唯一索引后，再查就是const了，测试完成再删除索引

```sql
-- 查看执行计划
explain select * from user where name='zs';
-- 创建唯一索引
create unique index user_name_unique on user(name);
-- 再查看执行计划
explain select * from user where name='zs';
-- 测试完成删除索引
drop index user_name_unique on user;
```

![image-20220708221944666](http://cdn.bluecusliyou.com/202207082219777.png)

#### （4）**eq_ref**：使用主键或者唯一键索引的关联查询

代表有其他表引用了r表的主键或者唯一键，这里需要提一下，eq_ref有时候会不准！

```sql
explain select * from user u left join role r on u.role_id=r.id;
```

![image-20220708222232293](http://cdn.bluecusliyou.com/202207082222403.png)

#### （5）**ref**：通过非唯一索引查询到的数据

非唯一性索引扫描，返回匹配某个单独值的所有行，本质上也是一种索引访问，应该属于查找和扫描的混合体

```sql
-- 创建普通索引
create index user_name_index on user(name);
-- 查询执行计划
explain select * from user where name='zs';
-- 测试完毕删除索引
drop index user_name_index on user;
```

![image-20220709202242432](http://cdn.bluecusliyou.com/202207092022514.png)

#### （6）ref_or_null：类似ref，但是可以搜索值为NULL的行

```sql
-- 创建普通索引
create index user_name_index on user(name);
-- 查询执行计划
explain select * from user where name='zs' or name is null;
-- 测试完毕删除索引
drop index user_name_index on user;
```

![image-20220717173525725](http://cdn.bluecusliyou.com/202207171735881.png)

#### （7）index_merge：表示使用了索引合并的优化方法

两个字段都有索引，优化器会合并索引

```sql
-- 创建普通索引
create index user_name_index on user(name);
-- 查询执行计划
explain select * from user where id=1 or name='zs';
-- 测试完毕删除索引
drop index user_name_index on user;
```

#### （8）range：使用索引的范围查询

只检索给定范围的行，使用一个索引来选择行，key列显示使用了哪个索引，一般就是在你的where语句中出现between、<>、in、<、>等的查询。

```sql
explain select * from user u where u.id>20; -- 使用索引列进行范围查询
explain select * from user u where u.role_id>20; -- 使用普通列进行范围查询不会是range
```

![image-20220709204230601](http://cdn.bluecusliyou.com/202207092042683.png)

```sql
-- 给role_id列添加索引，再次执行sql，查看执行计划
create index user_role_id_index on user(role_id);
explain select * from user u where u.role_id>20;
-- 测试完毕删除索引
drop index user_role_id_index on user;
```

![image-20220709204344222](http://cdn.bluecusliyou.com/202207092043303.png)

#### （9）**index**：查询的是索引列，遍历了索引树

全索引树扫描，Index与All区别：index只遍历索引树，通常比All快，因为索引文件通常比数据文件小，也就是虽然all和index都是读全表，但index是从索引文件中读取，而all是从数据文件读的。

```sql
explain select id from user;
```

![image-20220709210149659](http://cdn.bluecusliyou.com/202207092101738.png)

#### （8）ALL：效率最低，遍历全表

```sql
explain select * from user;
```

![image-20220709210220654](http://cdn.bluecusliyou.com/202207092102749.png)

### 9、 **possible_keys** 字段

显示可能应用在这张表中的索引，一个或者多个。查询涉及到的字段上若存在索引，则该索引将被列出，**但不一定被查询实际使用。**

```sql
-- 给name列加索引
create index idx_name on user(name); 
-- 可能用到了 idx_name 索引，但实际没有使用到。
explain select * from user where name='1' or name='2';
-- 测试完毕删除索引
drop index idx_name on user;
```

![image-20220709211546988](http://cdn.bluecusliyou.com/202207092115070.png)

### 10、key字段

实际使用的索引。如果为 NULL ，则没有使用索引。查询中如果使用了覆盖索引，则该索引仅仅出现在 key 列表中。

```sql
explain select * from user where id=1;
-- 根据普通列查询
explain select * from user where name='zs';
-- 给name列加上索引
create index idx_name on user(name);
-- 根据索引查询
explain select * from user where name='zs';
-- 测试完毕删除索引
drop index idx_name on user;
```

![image-20220709212125016](http://cdn.bluecusliyou.com/202207092121099.png)

### 11、key_len字段

表示索引中使用的字节数，可通过该列计算查询中使用的索引的长度。 key_len 显示的值为索引字段的最大可能长度，并非实际使用长度，即 key_len 是根据表定义计算而得，不是通过表内检索出的。在不损失精度的情况下，长度越短越好。

> key_len 计算规则
>
> **A、char和varchar**
>
> - 列长度
>
> - 列是否为空：NULL（+1），NULL(+0)
>
> - 字符集：如utf8mb4=4,utf8=3,gbk=2,lathin1=1
>
> - 列类型为字符：如varchar（+2），char（+0）
>
> - 计算公式：key_len=（表字符集长度）*列长度+1（null）+2（变长列）
>
> **B、数值类型**
>
> - tinyint    非空为1，可空为2
> - smallint  非空为2，可空为3
> - int            非空为4，可空为5
> - bigint　　非空为8，可空为9
>
> **C. 时间类型**
>
> - date：非空3字节，可空4字节
> - timestamp：非空4字节，可空5字节
> - datetime：非空8字节，可空9字节

```sql
-- 我的id类型为int类型，因此占用4个字节
explain select * from user where id=1;
```

![image-20220709212336527](http://cdn.bluecusliyou.com/202207092123609.png)

```sql
-- 我们把id类型改为bigint（Long），再次查看索引使用字节数
alter table user modify column id bigint;
explain select * from user where id=1;
-- 测试完毕更改回来
alter table user modify column id int;
```

![image-20220709212459351](http://cdn.bluecusliyou.com/202207092124436.png)

### 12、**ref** 字段

表示某表的某个字段引用到了本表的索引字段

```sql
-- 表示u表的role_id引用了本表（r表）的索引字段（PRIMARY），ref字段显示正常，type字段也显
示eq_ref（正常）.
explain select * from user u left join role r on u.role_id=r.id;
```

![image-20220709212735942](http://cdn.bluecusliyou.com/202207092127045.png)

### 13、**rows** 字段

根据表统计信息及选用情况，大致估算出找到所需的记录或所需读取的行数。

```sql
-- user表中有1条记录，role表中有3条记录
explain select * from user;
explain select * from role;
```

![image-20220709214403370](http://cdn.bluecusliyou.com/202207092144464.png)

### 14、**filtered** 字段

查询的表行占表的百分比。

```sql
-- 去掉user表和role表的主键，现在也没有索引
-- 现在表中数据如下
select * from user;
select * from role;
-- r表实际记录3条，上述sql语句关联查询出来的结果只能得出一条结果集，因此命中率为
33.33%。
select * from user u inner join role r on u.role_id=r.id;
explain select * from user u inner join role r on u.role_id=r.id;
-- 测试完成 恢复表的主键
```

![image-20220710093103502](http://cdn.bluecusliyou.com/202207100931595.png)

### 15、**extra** 字段

包含不适合在其它列中显示但十分重要的额外信息。

```bash
Using index > NULL > Using where >= Using temporary > Using filesort
```

#### （1）Using filesort：排序时无法使用到索引，效率低。

常见于order by和group by语句中。

```sql
explain select name from user order by name;
```

![image-20220710093348124](http://cdn.bluecusliyou.com/202207100933204.png)

#### （2）Using temporary：表示SQL语句的操作使用到了临时表。

MySQL在对结果排序时使用临时表，常见于排序order by 和分组查询group by。

```sql
explain select name from user group by name;
```

![image-20220710093532105](http://cdn.bluecusliyou.com/202207100935191.png)

#### （3）Using index：代表使用到了索引，效率高。

表示相应的 SELECT 操作中使用了覆盖索引，避免访问了表的数据行，效率不错！如果同时出现 Using where ，表示索引出现在where查找条件中。

覆盖索引：就是select的数据列只用从索引中就能够取得，不必从数据表中读取，换句话说查询列要被所使用的索引覆盖。注意：如果要使用覆盖索引，一定不能写SELECT *，要写出具体的字段。

```sql
-- 创建索引
create index user_name_index on user(name); 
-- 查看执行计划
explain select name from user order by name;
-- 测试完毕删除索引
drop index user_name_index on user;
```

![image-20220710094016562](http://cdn.bluecusliyou.com/202207100940652.png)

#### （4）Using where：扫描全表。通常是查询条件中不是索引字段。

```sql
explain select * from user where name='zs';
```

![image-20220710094122142](http://cdn.bluecusliyou.com/202207100941227.png)

#### （5）Using join buffer：使用了连接缓存

```sql
explain select * from user,role;
```

![image-20220717204006292](http://cdn.bluecusliyou.com/202207172040391.png)

#### （5）NULL：没有用到额外的附加条件

```sql
explain select * from user where id=1;
```

![image-20220710094603124](http://cdn.bluecusliyou.com/202207100946217.png)

#### （6）Impossible where ： WHERE 子句的值总是false，不能用来获取任何元组。

```sql
explain select *  from user where name='zs' and name='ls';
```

![image-20220710144545714](http://cdn.bluecusliyou.com/202207101445797.png)

#### （7）Distinct：去重

一旦mysql找到了与行相联合匹配的行，就不再搜索了

```sql
explain select distinct user.name from user left join role on user.id=role.id;
```

![image-20220717204540610](http://cdn.bluecusliyou.com/202207172045712.png)

#### （8）Select tables optimized away：MySQL根本没有遍历表或索引就返回数据

## 三、索引和查询优化

### 1、索引概述

#### （1）索引是什么

- MySQL官方对索引的定义为：索引（INDEX）是帮助MySQL高效获取数据的数据结构。所以说索引的本质是：数据结构。
- 数据库查询是数据库的最主要功能之一。我们都希望查询数据的速度能尽可能的快，因此数据库系统的设计者会从查询算法的角度进行优化。每种查找算法都只能应用于特定的数据结构之上，例如二分查找要求被检索数据有序，而二叉树查找只能应用于二叉查找树上，但是数据本身的组织结构不可能完全满足各种数据结构（例如，理论上不可能同时将两列都按顺序进行组织），所以，在数据之外，数据库系统还维护着满足特定查找算法的数据结构，这些数据结构以某种方式引用（指向）数据，这样就可以在这些数据结构上实现高级查找算法。这种数据结构，就是索引。

#### （2）索引优势和劣势

**优势：**

- 索引大大减少了服务器需要扫描的数据量（提高数据检索效率）
- 索引可以帮助服务器避免排序和临时表（降低数据排序成本，降低 CPU 的消耗）
- 索引可以将随机 I/O 变为顺序 I/O（降低数据库 IO 成本）

**劣势：**

- 实际上索引也是一张表，该表保存了索引字段，并指向实体表的记录，所以索引列也是要占用空间的。
- 虽然索引大大提高了查询速度，但是同时会降低表的更新速度，因为更新表的时候，MySQL不仅要保存数据，还要维护索引表，增加了开销。

#### （3）索引分类

- 从主键角度

  - 主键索引：不允许有空值，为主键建立的索引。
  - 辅助索引：非主键索引，允许在定义索引的列中插入重复值和空值。

- 从列数量角度

  - 单列索引：在单个字段上创建的索引。
  - 多列索引（复合索引、联合索引）：在多个字段上创建的索引，只有在查询条件中使用了创建索引时的第一个字段，索引才会被使用。

- 从字段值唯一性角度

  - 唯一索引：索引列中的值必须是唯一的，但是允许为空值。
  - 非唯一索引：索引列中的值可以不唯一。

- 字段类型特殊角度

  - 全文索引：它查找的是文本中的关键词，而不是直接比较索引中的值
  - 空间索引：空间索引是对空间数据类型的字段建立的索引，MySQL在5.7之后的版本支持了空间索引，而且支持OpenGIS几何数据模型。MySQL在空间索引这方面遵循OpenGIS几何数据模型规则。
  - 前缀索引：在文本类型如CHAR，VARCHAR，TEXT类列上创建索引时，可以指定索引列的长度，但是数值类型不能指定。

- 数据结构角度

  - Hash 索引：主要就是通过 Hash 算法，将数据库字段数据转换成定长的 Hash 值，与这条数据的行指针一并存入 Hash 表的对应位置；如果发生 Hash 碰撞，则在对应 Hash 键下以链表形式存储。

    查询时，就再次对待查关键字再次执行相同的 Hash 算法，得到 Hash 值，到对应 Hash 表对应位置取出数据即可。

    使用 Hash 索引的数据库并不多， 目前有 Memory 引擎和InnoDB引擎支持 Hash 索引。

    缺点是，只支持等值比较查询，像 = 、 in() 这种，不支持范围查找，比如 where id > 10 这种，也不能排序。

  - B+ 树索引（下文会详细讲）

- 从物理存储角度

  - 聚集索引（clustered index）：指索引项的排序方式和表中数据记录排序方式一致的索引，并不是一种单独的索引类型，而是一种数据存储方式。

    也就是说聚集索引的顺序就是数据的物理存储顺序。它会根据聚集索引键的顺序来存储表中的数据，即对表的数据按索引键的顺序进行排序，然后重新存储到磁盘上。因为数据在物理存放时只能有一种排列方式，所以一个表只能有一个聚集索引。

    聚集索引插入数据速度慢（时间花费在“物理存储的排序”上），查询数据比非聚集索引速度快。

    ![image-20220802132043324](http://cdn.bluecusliyou.com/202208021320547.png)

    InnoDB 也使用B+Tree作为索引结构，索引页大小16，和表数据页共同存放在表空间中。从InnoDB表数据存放方式可看出InnoDB表数据文件本身就是按B+Tree组织的一个索引结构，这棵树的叶节点data域保存了完整的数据记录。这个索引的key是数据表的主键，因此InnoDB表数据文件本身就是主索引。

    InnoDB默认对主键建立聚簇索引。如果你不指定主键，InnoDB会用一个具有唯一且非空值的索引来代替。如果不存在这样的索引，InnoDB会定义一个隐藏的主键，然后对其建立聚簇索引。一般来说，InnoDB 会以聚簇索引的形式来存储实际的数据，它是其它二级索引的基础。

  - 非聚集索引（non-clustered index）：索引顺序与物理存储顺序不同的索引。

    ![image-20220802142727853](http://cdn.bluecusliyou.com/202208021427955.png)

    MyIsam 索引文件和数据文件是分离的，索引文件仅保存数据记录的地址。主索引和辅助索引没有区别都是非聚集索引。索引页正常大小为1024字节，索引页存放在.MYI 文件中。MyISAM引擎使用B+Tree作为索引结构，叶节点的data域存放的是数据记录的地址。

#### （4）索引sql

```sql
-- 创建索引
CREATE [UNIQUE] INDEX indexName ON TableName(columnName(length));
-- 如：在article表上创建category_id的单个索引，名为：idx_article_c
CREATE INDEX idx_article_c ON article(category_id);
-- 如：在article表上创建category_id,views的联合索引，名为：idx_article_cv
CREATE INDEX idx_article_cv ON article(category_id,views);

-- 修改索引
/* 1、该语句添加一个主键，这意味着索引值必须是唯一的，并且不能为NULL */
ALTER TABLE TableName ADD PRIMARY KEY(column_list);
/* 2、该语句创建索引的键值必须是唯一的(除了NULL之外，NULL可能会出现多次) */
ALTER TABLE TableName ADD UNIQUE indexName(column_list);
/* 3、该语句创建普通索引，索引值可以出现多次 */
ALTER TABLE TableName ADD INDEX indexName(column_list);
--如：在article表中对views列添加索引，名为idx_article_v
alter table article add index idx_article_v(views);
/* 4、该语句指定了索引为FULLTEXT，用于全文检索 */
ALTER TABLE TableName ADD FULLTEXT indexName(column_list);

-- 删除索引
DROP INDEX [indexName] ON TableName;
-- 如：删除acticle表上的 idx_article_cv 索引
drop index idx_article_cv on article;

-- 查询索引
show index from TableName;
-- 如：查看article表的索引
show index from article;
```

#### （5）索引策略

**哪些情况需要建索引**

- 主键自动建立主键索引（唯一 + 非空）。
- 频繁作为查询条件的字段应该创建索引。
- 查询中与其他表关联的字段，外键关系建立索引。
- 查询中排序的字段，排序字段若通过索引去访问将大大提高排序速度。
- 查询中统计或者分组字段（group by也和索引有关）。

**哪些情况不要建索引**

- 记录太少的表不适合创建索引。

- 频繁更新的字段不适合创建索引。

- Where条件里用不到的字段不适合创建索引。

- 选择性很低的表不适合创建索引。

  `索引的选择性`是指索引列中不同值的数目与表中记录数的比。如果一个表中有2000条记录，表索引列有1980个不同的值，这个索引的选择性就是1980/2000=0.99。一个索引的选择性越接近于1，这个索引的效率就越高。

### 2、高效索引

#### （1）覆盖索引

> 什么是覆盖索引

- 索引是高效找到行的一个方法，当能通过读取索引就可以得到想要的数据，就不需要读取行了。一个`索引包含了满足查询结果的数据列就叫做覆盖索引`。

> 覆盖索引的好处

- 在覆盖索引中，二级索引的键值中可以获取所要的数据，避免了对主键的二次查询，减少了I0操作，提升了查询效率。
- 可以把随机IO变成顺序IO加快查询效率。

> 执行计划分析

```sql
/* 假设当前索引是index(`name`, `age`, `pos`) */
/* 在写SQL的不要使用 SELECT * ，用什么字段就查询什么字段 */
/* 没有用到覆盖索引 */
EXPLAIN SELECT * FROM `staffs` WHERE `name` = 'Ringo' AND `age` = 18 AND `pos` = 
'manager';
/* 用到了覆盖索引 */
EXPLAIN SELECT `name`, `age`, `pos` FROM `staffs` WHERE `name` = 'Ringo' AND `age` = 18 AND `pos` = 'manager';
```

![image-20220710221355755](http://cdn.bluecusliyou.com/202207102213103.png)

#### （2）前缀索引

> 前缀索引

前缀索引其实就是对文本的前几个字符（具体是几个字符在建立索引时指定）建立索引，这样建立起来的索引占用空间更小，所以查询更快。

对于内容很长的列，比如 blob, text 或者很长的 varchar 列，必须使用前缀索引，MySQL 不允许索引这些列的完整长度。

关键在于要选择合适长度的前缀，即 prefix_length。前缀太短，选择性太低，前缀太长，索引占用空间太大。为了决定前缀的合适长度，需要找到最常见的值的列表，然后和最常见的前缀列进行比较。

> 数据准备

```sql
-- 创建表
CREATE TABLE teacher ( ID BIGINT UNSIGNED PRIMARY KEY, email VARCHAR ( 64 ) ) ENGINE = INNODB;
```

> 前缀索引语法

```sql
-- 语法
ALTER TABLE table_name ADD KEY(column_name(prefix_length));
ALTER TABLE table_name ADD index index_name(column_name(prefix_length));
```

```sql
-- 默认包含整个字符串
alter table teacher add index index1(email);
-- 前缀索引
alter table teacher add index index2(email(6));
```

> 前缀索引数据结构

这两种不同的定义在数据结构和存储上有什么区别呢？下图就是这两个索引的示意图。

![image-20220721075640808](http://cdn.bluecusliyou.com/202207210756942.png)

![image-20220721075655283](http://cdn.bluecusliyou.com/202207210756430.png)

> 现在执行一个查询select id,email from teacher where email='xxx';

**如果使用的是index1**（即email整个字符串的索引结构），执行顺序是这样的：

- 从index1索引树找到满足索引值是’[zhangssxyz@xxx.com](https://link.juejin.cn?target=mailto%3Azhangssxyz%40xxx.com)’的这条记录，取得ID2的值；
- 到主键上查到主键值是ID2的行，判断email的值是正确的，将这行记录加入结果集；
- 取index1索引树上刚刚查到的位置的下一条记录，发现已经不满足email=’[zhangssxyz@xxx.com](https://link.juejin.cn?target=mailto%3Azhangssxyz%40xxx.com)’的条件了，循环结束。

这个过程中，只需要回主键索引取一次数据，所以系统认为只扫描了一行。

**如果使用的是index2**（即email(6)索引结构），执行顺序是这样的：

- 从index2索引树找到满足索引值是’zhangs’的记录，找到的第一个是ID1；
- 到主键上查到主键值是ID1的行，判断出email的值不是’[zhangssxyz@xxx.com](https://link.juejin.cn?target=mailto%3Azhangssxyz%40xxx.com)’，这行记录丢弃；
- 取index2上刚刚查到的位置的下一条记录，发现仍然是’zhangs’，取出ID2，再到ID索引上取整行然后判断，这次值对了，将这行记录加入结果集；
- 重复上一步，直到在idxe2上取到的值不是’zhangs’时，循环结束。

也就是说使用前缀索引，定义好长度，就可以做到既节省空间，又无需增加很多查询成本。

> 前缀索引缺点

- 无法使用前缀索引做 ORDER BY 和 GROUP BY
- 无法使用前缀索引做『覆盖索引』。

> 倒叙前缀

身份证号（前缀相同多，后缀相同少）这样的数据如何索引？

- **使用倒序存储**：如果你存储身份证号的时候把它倒过来存，每次查询的时候，你可以这么写。

```sql
select field_list from t where id_card = reverse('input_id_card_string');	
```

- **使用 hash 字段。**你可以在表上再创建一个整数字段，来保存身份证的校验码，同时在这个字段上创建索引。

```sql
-- 创建索引
alter table t add id_card_crc int unsigned, add index(id_card_crc);
-- 查询
select field_list from t where id_card_crc=crc32('input_id_card_string') and id_card='input_id_card_string' 
```

> 压缩前缀索引

MyISAM 使用前缀压缩来减少索引的大小，从而让更多的索引可以放入内存中，在某些情况下能极大地提高性能。

MyISAM 压缩每个索引块的方法是，先完全保存索引块中的第一个值，然后将其他值和第一个值进行比较得到相同前缀的字节数和剩余的不同后缀部分，把这部分存储起来即可。

例如，索引块中的第一个值是“perform“，第二个值是”performance“，那么第二个值的前缀压缩后存储的是类似”7,ance“这样的形式。MyISAM 对**行指针**也采用类似的前缀压缩方式。

压缩块使用更少的空间，代价是某些操作可能更慢。因为每个值的压缩前缀都依赖前面的值，所以 MyISAM 查找时无法在索引块使用二分查找而只能从头开始扫描。正序的扫描速度还不错，但是如果是倒序扫描，例如 ORDER BY DESC，就不是很好了。所有在块中查找某一行的操作平均都需要扫描半个索引块。

测试表明，对于 CPU 密集型应用，因为扫描需要随机查找，压缩索引使得 MyISAM 在索引查找上要慢好几倍。压缩索引的倒序扫描就更慢了。压缩索引需要在 CPU 内存资源与磁盘之间做权衡。压缩索引可能只需要十分之一大小的磁盘空间，如果是 I/O 密集型应用，对某些查询带来的好处会比成本多很多。

可以在 CREATE TABLE 语句中指定 PACK_KEYS 参数来控制索引压缩的方式。

如果您希望索引更小，则把此选项设置为1。这样做通常使更新速度变慢，同时阅读速度加快。把选项设置为0可以取消所有的关键字压缩。把此选项设置为DEFAULT时，存储引擎只压缩长的CHAR或VARCHAR列(仅限于MyISAM)。

如果您不使用PACK_KEYS，则默认操作是只压缩字符串，但不压缩数字。如果您使用PACK_KEYS=1，则对数字也进行压缩。

```sql
CREATE TABLE (
`id` INT NOT NULL ,
`name` VARCHAR(250) NULL ,
PRIMARY KEY (`id`) )
PACK_KEYS = 1;
ALTER TABLE table_name PACK_KEYS = 1;
```

#### （3）联合索引

> 索引合并

对于下面的查询 where 条件，这两个单列索引都是不好的选择：

```sql
SELECT user_id,user_name FROM mydb.sys_user where user_id = 1 or user_name = 'zhang3';
```

MySQL 5.0 版本之前，MySQL 会对这个查询使用全表扫描，除非改写成两个查询 UNION 的方式。

MySQL 5.0 和后续版本引入了一种叫做“**索引合并**”的策略，查询能够同时使用这两个单列索引进行扫描，并将结果合并。

这种算法有三个变种：OR 条件的联合（union），AND 条件的相交（intersection），组合前两种情况的联合及相交。索引合并策略有时候是一种优化的结果，但实际上更多时候说明了表上的索引建得很糟糕：

- 当出现多个AND条件，通常意味着需要一个包含所有相关列的多列索引，而不是多个独立的单列索引。
- 当出现多个OR条件，通常需要耗费大量的 CPU 和内存资源在算法的缓存、排序和合并操作上。特别是当其中有些索引的选择性不高，需要合并扫描返回的大量数据的时候。
- 如果在 explain 中看到有索引合并，应该好好检查一下查询和表的结构，看是不是已经是最优的。

> 最左前缀原则

索引项是按照索引定义里面出现的字段顺序排序的，不只是索引的全部定义，只要满足最左前缀，就可以利用索引来加速检索。这个最左前缀可以是联合索引的最左 N 个字段，也可以是字符串索引的最左 M 个字符。如果跳过最左前缀就无法使用索引来加速检索。

建立联合索引的时候，我们的评估标准是，索引的复用能力。所以当已经有了 (a,b) 这个联合索引后，一般就不需要单独在 a 上建立索引了。因此，**第一原则是，如果通过调整顺序，可以少维护一个索引，那么这个顺序往往就是需要优先考虑采用的。**

```sql
/* 假设当前索引是index(user_id,user_name) */
/*  使用索引*/
SELECT * FROM mydb.sys_user where user_name = 'zhang3' and user_id=1;
/*  使用索引*/
SELECT * FROM mydb.sys_user where user_name = 'zhang3';
/*  使用索引*/
SELECT * FROM mydb.sys_user where user_name like 'zha%';
/*  没有最左匹配不能使用索引*/
SELECT * FROM mydb.sys_user where user_id=1;
```

> 索引下推

如果现在有一个需求：检索出表中“名字第一个字是 B，而且年龄是 19 岁的所有男孩”。那么，SQL 语句是这么写的：

```sql
/* 假设当前索引是index(name,age,sex) */
select * from tuser where name like 'B%' and age=19 and sex=F;
```

根据最左前缀原则，这个语句在搜索索引树的时候，只能用 “B”，找到第一个满足条件的记录 ID = 2。

在 MySQL 5.6 之前，只能从 ID = 2 开始一个个回表。到主键索引上找出数据行，再对比字段值。

而 MySQL 5.6 引入的**索引下推优化**（index condition pushdown)， 可以在索引遍历过程中，对索引中包含的字段先做判断，直接过滤掉不满足条件的记录，减少回表次数，大大提升了查询的效率。

![image-20220721214329132](http://cdn.bluecusliyou.com/202207212143753.png)

#### （4）重复索引和冗余索引

**重复索引是指在相同的列上按照相同的顺序创建的相同类型的索引**。应该避免这样创建重复索引，发现以后也应该立即移除。

如果创建了索引(A,B)，再创建索引(A)就是冗余索引，索引(A,B)也可以当做索引(A)来使用（这种冗余只是对 B-Tree 索引来说的）。但是如果再创建索引(B,A)，则不是冗余索引，索引(B)也不是，因为B不是索引(A,B)的最左前缀。另外，其他不同类型的索引（例如哈希索引或者全文索引）也不会是 B-Tree 索引的冗余索引，而无论覆盖的索引列是什么。

#### （5）未使用的索引

除了冗余索引和重复索引，可能还会有一些服务器永远不使用的索引，这样的索引完全是累赘，建议考虑删除，有两个工具可以帮助定位未使用的索引：

- 在 percona server 或者 mariadb 中先打开 userstat=ON 服务器变量，默认是关闭的，然后让服务器运行一段时间，再通过查询`information_schema.index_statistics` 就能查到每个索引的使用频率。
- 使用 percona toolkit 中的 pt-index-usage 工具，该工具可以读取查询日志，并对日志中的每个查询进行explain 操作，然后打印出关于索引和查询的报告，这个工具不仅可以找出哪些索引是未使用的，还可以了解查询的执行计划。

### 3、索引是如何设计的

#### （1）定义问题

这里我们假设要解决的问题，只包含这样两个常用的需求：

- 根据某个值查找数据，比如 select * from user where id=1234；
- 根据区间值来查找某些数据，比如 select * from user where id > 1234 and id < 2345。

在执行效率方面，我们希望通过索引，查询数据的效率尽可能地高；在存储空间方面，我们希望索引不要消耗太多的内存空间。

推荐一个数据结构可视化网站：[https://www.cs.usfca.edu/~galles/visualization/Algorithms.html](https://www.cs.usfca.edu/~galles/visualization/Algorithms.html)

#### （2）尝试解决问题--散列表

> 原理

- 事先将索引通过 hash算法后得到的hash值(即磁盘文件指针）存到hash表中。
- 在进行查询时，将索引通过hash算法，得到hash值，与hash表中的hash值比对。通过磁盘文件指针，只要**一次磁盘IO**就能找到要的值，查询性能很好，时间复杂度是 O(1)。

例如：

　在第一个表中，要查找col=6的值。hash(6) 得到值，比对hash表，就能得到值。性能非常高。

> 存在的问题

- 散列表不能支持按照区间快速查找数据。
- 散列表不能支持模糊查询。

#### （3）尝试解决问题--二叉树

> 特点

- 左子节点值 < 节点值；
- 右子节点值 > 节点值；
- 当数据量非常大时，要查找的数据又非常靠后，和没有索引相比，那么二叉树结构的查询优势将非常明显。

> 存在的问题

- 如下图，可以看出，二叉树出现单边增长时，二叉树变成了“链”，查找效率依旧低下。


![image-20220804163630768](http://cdn.bluecusliyou.com/202208041636873.png)

#### （4）尝试解决问题--红黑树（一种平衡二叉树）

> 特点

- 根节点是黑色的；
- 每个叶子节点都是黑色的空节点（NIL），也就是说，叶子节点不存储数据；
- 任何相邻的节点都不能同时为红色，也就是说，红色节点是被黑色节点隔开的；
- 每个节点，从该节点到达其可达叶子节点的所有路径，都包含相同数目的黑色节点。

![img](http://cdn.bluecusliyou.com/202208041638486.png)

 

 

 

> 存在的问题

- 红黑树虽然和二叉树相比，一定程度上缓解了单边过长的问题，但是它依旧有存储高度问题。
- 不能支持按照区间快速查找数据问题。

#### （5）尝试解决问题--B-Tree（M叉树）

> 特点

B-Tree == B Tree，是同一个东西，没有 B 减树，就叫B树。

B-Tree索引能很好解决红黑树中遗留的高度问题，B-Tree 是一种平衡的多路查找（又称排序）树，在文件系统中和数据库系统有所应用，主要用作文件的索引，其中的B就表示平衡（Balance）。

为了描述B-Tree，首先定义一条数据记录为一个二元组 [key, data]，key为记录的键值key，对于不同数据记录，key是互不相同的；**data为数据记录除以key外的数据 （这里指的是聚集索引）**。那么B-Tree是满足下列条件的数据结构：

- d 为大于1的一个正整数，称为BTree的度；

-  h为一个正整数，称为BTree的高度；

- key和指针互相间隔，节点两端是指针；

-  叶子节点具有相同的深度，叶子节点的指针为空，节点中数据索引(下图中的key)从左往右递增排列。

> 存在问题

- 范围查找性能差。

- 每个节点中不仅包含数据的key值，还有data值。而每一个节点的存储空间是有限的(mysql默认设置一个节点的大小为16K)，如果data中存放的数据较大时，将会导致每个节点（即一个页）能存储的key的数量（索引的数量）很小，所以当数据量很多，且每行数据量很大的时候，同样会导致B-Tree的深度较大，增大查询时的磁盘I/O次数，进而影响查询效率。


#### （6）尝试解决问题--B+Tree（B-Tree优化）

> 特点

`B+Tree`是在`B-Tree`基础上的一种优化，使其更适合实现外存储索引结构。在B+Tree中，所有数据记录节点都是按照键值大小顺序存放在同一层的叶子节点上，而非叶子节点上只存储key值信息，这样可以大大加大每个节点存储的key值数量，降低树的高度。

- 非叶子节点不存储data，只存储索引，可以存放更多索引。
- 叶子节点不存储指针。
- 顺序访问指针，提高区间访问性能。
- 非叶子节点中的索引最终还是会在叶子节点上存储一份，也就是叶子节点会包含非叶子节点上的所有索引。
- 一个父节点，它的**左侧**子节点都**小于**父节点的值，**右侧**的子节点都**大于等于**父节点的值。
- 每一层节点从左往右都是递增排列，无论是数值型还是字符型。

#### （7）B-Tree 和 B+Tree比较

> B 树 和 B+ 树的结构差异

将 `[11,13,15,16,20,23,25,30,23,27]` 用 B 树 和 B+ 树存储，看下结构

![image-20220717220112190](http://cdn.bluecusliyou.com/202207172201561.png)



| 关键词                           | B-树                                                         | B+树                                                         | 备注                                |
| -------------------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ----------------------------------- |
| 最大分支，最小分支               | 每个结点最多有m个分支（子树），最少⌈m/2⌉（中间结点）个分支或者2个分支（是根节点非叶子结点）。 | 同左                                                         | m阶对应的就是就是最大分支           |
| n个关键字与分支的关系            | 分支等于n+1                                                  | 分支等于n                                                    | 无                                  |
| 关键字个数（B+树关键字个数要多） | 大于等于⌈m/2⌉-1小于等于m-1                                   | 大于等于⌈m/2⌉小于等于m                                       | B+树关键字个数要多，+体现在的地方。 |
| 叶子结点相同点                   | 每个节点中的元素互不相等且按照从小到大排列；所有的叶子结点都位于同一层。 | 同左                                                         | 无                                  |
| 叶子结点不相同                   | 不包含信息                                                   | 叶子结点包含信息，指针指向记录。                             | 无                                  |
| 叶子结点之间的关系               | 无                                                           | B+树上有一个指针指向关键字最小的叶子结点，所有叶子节点之间链接成一个线性链表 | 无                                  |
| 非叶子结点                       | 一个关键字对应一个记录的存储地址                             | 只起到索引的作用                                             | 无                                  |
| 存储结构                         | 相同                                                         | 同左                                                         | 无                                  |

> 看一个范围查找的例子比较 B 树和 B+ 树

B Tree 结构查询 [10-25] 的数据（从根节点开始，随机查找是一样的）

1. 加载根节点，第一个节点元素15，大于10【磁盘 I/O 操作第 1 次】
2. 通过根节点的左子节点地址加载，找到 11，13【磁盘 I/O 操作第 2 次】
3. 重新加载根节点，找到中间节点数据 16，20【磁盘 I/O 操作第 3 次】
4. 再次加载根节点，23 小于 25，再加载右子节点，找到 25，结束【磁盘 I/O 操作第 4 次】

![image-20220718162102244](http://cdn.bluecusliyou.com/202207181621462.png)

而 B+ 树对范围查找就简单了，数据都在最下边的叶子节点下，而且链起来了，我只需找到第一个然后遍历就行（暂且不考虑页分裂等其他问题）。

> 为什么 MySQL 索引要用 B+ 树不是B树

B+Tree 是在 B-Tree 基础上的一种优化，使其更适合实现外存储索引结构。

用 B+ 树不用考虑的是 IO 对性能的影响，B 树的每个节点都存储数据，而 B+ 树只有叶子节点才存储数据，所以查找相同数据量的情况下，B 树的高度更高，IO 更频繁。数据库索引是存储在磁盘上的，当数据量大时，就不能把整个索引全部加载到内存了，只能逐一加载每一个磁盘页（对应索引树的节点）。其中在 MySQL 底层对 B+ 树进行进一步优化：**在叶子节点中是双向链表，且在链表的头结点和尾节点也是循环指向的**。

B-Tree 结构图每个节点中不仅要包含数据的 key 值，还有 data 值。而每一个页的存储空间是有限的，如果 data 数据较大时将会导致每个节点（即一个页）能存储的 key 的数量很小，当存储的数据量很大时同样会导致 B-Tree 的深度较大，增大查询时的磁盘 I/O 次数，进而影响查询效率。在 B+Tree 中，**所有数据记录节点都是按照键值大小顺序存放在同一层的叶子节点上**，而非叶子节点上只存储 key 值信息，这样可以大大加大每个节点存储的 key 值数量，降低 B+Tree 的高度。

IO 次数取决于 B+ 数的高度 h，假设当前数据表的数据为 N，每个磁盘块的数据项的数量是 m，则有 `h=㏒(m+1)N`，当数据量 N 一定的情况下，m 越大，h 越小；而 `m = 磁盘块的大小 / 数据项的大小`，磁盘块的大小也就是一个数据页的大小，是固定的，如果数据项占的空间越小，数据项的数量越多，树的高度越低。这就是为什么每个数据项，即索引字段要尽量的小，比如 int 占 4 字节，要比 bigint 8 字节少一半。这也是为什么 B+ 树要求把真实的数据放到叶子节点而不是内层节点，一旦放到内层节点，磁盘块的数据项会大幅度下降，导致树增高。当数据项等于 1 时将会退化成线性表。

#### （8）B+Tree详细设计过程

> 解决区间查找问题

为了让二叉查找树支持按照区间来查找数据，我们可以对它进行这样的改造：树中的节点并不存储数据本身，而是只是作为索引。除此之外，**我们把每个叶子节点串在一条链表上**，链表中的数据是从小到大有序的。

![image-20220804171831848](http://cdn.bluecusliyou.com/202208041718963.png)

改造之后，如果我们要查找某个区间的数据。我们只需要拿区间的起始值，在树中进行查找，当查找到某个叶子节点之后，我们再顺着链表往后遍历，直到链表中的结点数据值大于区间的终止值为止。所有遍历到的数据，就是符合区间值的所有数据。

![image-20220804171933019](http://cdn.bluecusliyou.com/202208041719139.png)

> 减少内存的占用，同时也减少磁盘IO

但是，我们要为几千万、上亿的数据构建索引，如果将索引存储在内存中，尽管内存访问的速度非常快，查询的效率非常高，但是，占用的内存会非常多。

比如，我们给一亿个数据构建二叉查找树索引，那索引中会包含大约 1 亿个节点，每个节点假设占用 16 个字节，那就需要大约 1GB 的内存空间。如果我们要给 10 张表建立索引，那对内存的需求是无法满足的。

我们可以**借助时间换空间的思路**，把索引存储在硬盘中，但硬盘是一个非常慢速的存储设备，尽管减少了内存消耗，但是在数据查找的过程中，数据查询效率就相应降低很多。

```xml
磁盘IO是一个很慢的过程

磁盘读取数据靠的是机械运动，每次读取数据花费的时间可以分为寻道时间、旋转延迟、传输时间三个部分
- 寻道时间：是磁臂移动到指定磁道所需要的时间，主流磁盘一般在 5ms 以下；
- 旋转延迟：是我们经常听说的磁盘转速，比如一个磁盘 7200 转，表示每分钟能转 7200 次，也就是说 1 秒钟能转 120 次，旋转延迟就是 1/120/2 = 4.17ms；
- 传输时间：是从磁盘读出或将数据写入磁盘的时间，一般在零点几毫秒，相对于前两个时间可以忽略不计。

那么访问一次磁盘的时间，即一次磁盘 IO 的时间约等于 5+4.17 = 9ms 左右，听起来还挺不错的，但要知道一台 500 -MIPS 的机器每秒可以执行 5 亿条指令，因为指令依靠的是电的性质，换句话说执行一次 IO 的时间可以执行 40 万条指令，数据库动辄十万百万乃至千万级数据，每次 9 毫秒的时间，显然是个灾难。
```

每个节点的读取，都对应一次磁盘 IO 操作。树的高度就等于每次查询数据时磁盘 IO次数，我们优化的重点就是尽量减少磁盘 IO 操作，也就是，尽量降低树的高度。

`那如何降低树的高度呢？`如果我们把索引构建成 m 叉树，高度是不是比二叉树要小呢？如果给 16 个数据构建二叉树索引，树的高度是 4，查找一个数据，就需要 4 个磁盘 IO 操作（如果根节点存储在内存中，其他节点存储在磁盘中），如果对 16 个数据构建五叉树索引，那高度只有 2，查找一个数据，对应只需要 2 次磁盘IO操作。如果 m 叉树中的 m 是 100，那对一亿个数据构建索引，树的高度也只是 3，最多只要 3 次磁盘 IO 就能获取到数据。磁盘 IO 变少了，查找数据的效率也就提高了。

![image-20220805095523867](http://cdn.bluecusliyou.com/202208050955963.png)

> 如何确定M叉树的M值

对于相同个数的数据构建 m 叉树索引，m 叉树中的 m 越大，那树的高度就越小，那 m 叉树中的 m 是不是越大越好呢？到底多大才最合适呢？

不管是内存中的数据，还是磁盘中的数据，操作系统都是按页（一页大小通常是 4KB，这个值可以通过 getconfig PAGE_SIZE 命令查看）来读取的，一次会读一页的数据。如果要读取的数据量超过一页的大小，就会触发多次 IO 操作。所以，我们在选择 m 大小的时候，要尽量让每个节点的大小等于一个页的大小。读取一个节点，只需要一次磁盘 IO 操作。

```xml
局部性原理与磁盘预读

磁盘的存取速度往往是主存的几百分分之一，因此为了提高效率，要尽量减少磁盘I/O。为了达到这个目的，磁盘往往不是严格按需读取，而是每次都会预读，即使只需要一个字节，磁盘也会从这个位置开始，顺序向后读取一定长度的数据放入内存。

这样做的理论依据是计算机科学中著名的局部性原理：当一个数据被用到时，其附近的数据也通常会马上被使用。程序运行期间所需要的数据通常比较集中。由于磁盘顺序读取的效率很高（不需要寻道时间，只需很少的旋转时间），因此预读可以提高I/O效率。

预读的长度一般为页（page）的整倍数。页是计算机管理存储器的逻辑块，硬件及操作系统往往将主存和磁盘存储区分割为连续的大小相等的块，每个存储块称为一页（在许多操作系统中，页得大小通常为4k），主存和磁盘以页为单位交换数据。当程序要读取的数据不在主存中时，会触发一个缺页异常，此时系统会向磁盘发出读盘信号，磁盘会找到数据的起始位置并向后连续读取一页或几页载入内存中，然后返回，程序继续运行。
```

InnoDB 存储引擎中默认每个页的大小为16KB，可通过参数 `innodb_page_size` 将页的大小设置为 4K、8K、16K，可通过如下命令查看页的大小：`show variables like 'innodb_page_size';`

而系统一个磁盘块的存储空间往往没有这么大，因此 InnoDB 每次申请磁盘空间时都会是若干地址连续磁盘块来达到页的大小 16KB。InnoDB 在把磁盘数据读入到磁盘时会以页为基本单位，在查询数据时如果一个页中的每条数据都能有助于定位数据记录的位置，这将会减少磁盘 I/O 次数，提高查询效率。

> 增删改数据如何保持节点M值

对于一个 B+ 树来说，m 值是根据页的大小事先计算好的，也就是说，每个节点最多只能有 m 个子节点。在往数据库中写入数据的过程中，这样就有可能使索引中某些节点的子节点个数超过 m，这个节点的大小超过了一个页的大小，读取这样一个节点，就会导致多次磁盘 IO 操作。我们该如何解决这个问题呢？

我们只需要将这个节点分裂成两个节点。节点分裂之后，其上层父节点的子节点个数就有可能超过 m 个，可以用同样的方法，将父节点也分裂成两个节点。这种级联反应会从下往上，一直影响到根节点。（下图中的 B+ 树是一个三叉树。我们限定叶子节点中，数据的个数超过 2 个就分裂节点；非叶子节点中，子节点的个数超过 3 个就分裂节点）。

![image-20220805103745454](http://cdn.bluecusliyou.com/202208051037552.png)

正是因为要时刻保证 B+ 树索引是一个 m 叉树，所以，索引的存在会导致数据库写入的速度降低。实际上，不光写入数据会变慢，删除数据也会变慢。这是为什么呢？

频繁的数据删除，就会导致某些节点中，子节点的个数变得非常少，长此以往，如果每个节点的子节点都比较少，势必会影响索引的效率。

我们可以设置一个阈值。在 B+ 树中，这个阈值等于 m/2。如果某个节点的子节点个数小于 m/2，我们就将它跟相邻的兄弟节点合并。不过，合并之后节点的子节点个数有可能会超过 m。针对这种情况，我们可以借助插入数据时候的处理方法，再分裂节点。

下面举了一个删除操作的例子（下图中的 B+ 树是一个五叉树。我们限定叶子节点中，数据的个数少于 2 个就合并节点；非叶子节点中，子节点的个数少于 3 个就合并节点。）。

![image-20220805104433167](http://cdn.bluecusliyou.com/202208051044267.png)

### 4、MyISAM和InnoDB的索引结构对比

#### （1）MyISAM 索引结构

MyISAM 引擎的索引文件和数据文件是分离的。**MyISAM 引擎索引结构的叶子节点的数据域，存放的并不是实际的数据记录，而是数据记录的地址**。这样的索引称为“**非聚簇索引**”。MyISAM 的主索引与辅助索引区别并不大，主键索引就是一个名为 PRIMARY 的唯一非空索引。

在 MyISAM 中，索引（含叶子节点）存放在单独的 `.myi` 文件中，叶子节点存放的是数据的物理地址偏移量（通过偏移量访问就是随机访问，速度很快）。

通过索引查找数据的流程：先从索引文件中查找到索引节点，从中拿到数据的文件指针，再到数据文件中通过文件指针定位了具体的数据。

![image-20220719184522655](http://cdn.bluecusliyou.com/202207191845848.png)

#### （2）InnoDB 索引结构

**InnoDB 引擎索引结构的叶子节点的数据域，存放的就是实际的数据记录**（对于主索引，此处会存放表中所有的数据记录；对于辅助索引此处会引用主键，检索的时候通过主键到主键索引中找到对应数据行），或者说，**InnoDB 的数据文件本身就是主键索引文件**，这样的索引被称为"“**聚簇索引**”，一个表只能有一个聚簇索引。

**主键索引：**

我们知道 InnoDB 索引是聚集索引，它的索引和数据是存入同一个 `.idb` 文件中的，因此它的索引结构是在同一个树节点中同时存放索引和数据，如下图中最底层的叶子节点有三行数据，对应于数据表中的 id、name、score 数据项。

在 Innodb 中，索引分叶子节点和非叶子节点，非叶子节点就像新华字典的目录，单独存放在索引段中，叶子节点则是顺序排列的，在数据段中。

InnoDB 的数据文件可以按照表来切分（只需要开启`innodb_file_per_table)`，切分后存放在`xxx.ibd`中，不切分存放在 `xxx.ibdata`中。

从 MySQL 5.6.6 版本开始，它的默认值就是 ON 了。建议将这个值设置为 ON。因为，一个表单独存储为一个文件更容易管理，而且在你不需要这个表的时候，通过 drop table 命令，系统就会直接删除这个文件。而如果是放在共享表空间中，即使表删掉了，空间也是不会回收的。

![image-20220719185208740](http://cdn.bluecusliyou.com/202207191852912.png)



**辅助（非主键）索引：**

这次我们以示例中学生表中的 name 列建立辅助索引，它的索引结构跟主键索引的结构有很大差别，在最底层的叶子结点有两行数据，第一行的字符串是辅助索引，按照 ASCII 码进行排序，第二行的整数是主键的值。

这就意味着，对 name 列进行条件搜索，需要两个步骤：

1. 在辅助索引上检索 name，到达其叶子节点获取对应的主键；
2. 使用主键在主索引上再进行对应的检索操作

这也就是所谓的“**回表查询**”

![image-20220719190158296](http://cdn.bluecusliyou.com/202207191901444.png)

**InnoDB 索引结构需要注意的点**

1. 数据文件本身就是索引文件
2. 表数据文件本身就是按 B+Tree 组织的一个索引结构文件
3. 聚集索引中叶节点包含了完整的数据记录
4. InnoDB 表必须要有主键，并且推荐使用整型自增主键

正如我们上面介绍 InnoDB 存储结构，索引与数据是共同存储的，不管是主键索引还是辅助索引，在查找时都是通过先查找到索引节点才能拿到相对应的数据，如果我们在设计表结构时没有显式指定索引列的话，MySQL 会从表中选择数据不重复的列建立索引，如果没有符合的列，则 MySQL 自动为 InnoDB 表生成一个隐含字段作为主键，并且这个字段长度为 6 个字节，类型为整型。

### 5、索引执行过程（InnoDB）

#### （1）数据准备

以下面的student表为例，它的 id 是主键，age 列为普通索引。

```sql
CREATE TABLE `stu`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `age` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_age`(`age`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 66 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Compact;
```

表数据如下:

![image-20220719192043841](http://cdn.bluecusliyou.com/202207191920940.png)

#### （2）主键索引

> 为什么要有主键？
>
> 底层就是用B+Tree维护的，而B+Tree的结构就决定了必须有主键才能构建B+Tree树这个结构。
>
> 为什么推荐用整型主键？
>
> 假如使用类似 UUID 的字符串作为主键，那么在查找时，需要比较两个主键是否相同，这是一个相比整型比较 非常耗时的过程。需要一个字符，一个字符的比较，自然比较慢。 
>
> 为什么用自增主键
>
> - 后面的主键索引总是大于前面的主键索引，在做范围查询时，非常方便找到需要的数据。
> - 在添加的过程中，因为是自增的，每次添加**都是在后面插入**，树分裂的机会小；而UUID大小不确定，分裂机会大，**需要重新平衡树结构**，性能损耗大。

主键索引，它使用 B+ 树构建，叶子节点存储的是数据表的某一行数据。当表没有创建主键索引时，InnoDB 会自动创建一个 ROWID 字段用于构建聚簇索引。规则如下：

- 在表上定义主键 PRIMARY KEY，InnoDB 将主键索引用作聚簇索引。
- 如果表没有定义主键，InnoDB 会选择第一个不为 NULL 的唯一索引列用作聚簇索引。
- 如果以上两个都没有，InnoDB 会使用一个 6 字节长整型的隐式字段 ROWID 字段构建聚簇索引。该 ROWID 字段会在插入新行时自动递增。

> 主键索引数据结构

![image-20220721220402218](http://cdn.bluecusliyou.com/202207212204658.png)

> 主键索引等值查询执行过程

```sql
select * from stu where id = 38;
```

过程如下：

- 第一次磁盘 IO：从根节点检索，将数据块 1 加载到内存，比较 38 < 44，走左边。
- 第二次磁盘 IO：将左边数据块 2 加载到内存，比较 8<37<38，走右边。
- 第三次磁盘 IO：将右边数据块 6 加载到内存，比较 37<38，38=38。查询完毕，将数据返回客户端。

![image-20220721221025079](http://cdn.bluecusliyou.com/202207212210522.png)

> 主键索引范围查询执行过程

```sql
select * from stu where id between 38 and 44;
```

前面也介绍说了，B+ 树因为叶子节点有双向指针，范围查询可以直接利用双向有序链表。

过程如下：

- 第一次磁盘 IO：从根节点检索，将数据块 1 加载到内存，比较 38 < 44，走左边。
- 第二次磁盘 IO：将左边数据块 2 加载到内存，比较 8<37<38，走右边。
- 第三次磁盘 IO：将右边数据块 6 加载到内存，比较 37<38，38=38。走右边。
- 第四次磁盘 IO：将右边数据块 7 加载到内存，比较 38<44=44。查询完毕，将数据返回客户端。

![image-20220721220707180](http://cdn.bluecusliyou.com/202207212207645.png)

#### （3）普通单列索引

> 为什么非主键索引结构叶子节点存储的是主键值，而不是全部数据？
>
> - 节省空间：指向主键的节点，不用再存储一份相同的数据；（否则的话，如果建立多个非主键索引，每个上面都存储的完整数据，非常占用空间）
> -  数据一致性：如果修改索引15 的数据，那只要修改主键的 data，而如果非主键的data也存一份的话，那得修改两份，这样就涉及到事务一致性的问题，耗时，性能低。

> 普通索引数据结构

在 InnDB 中，B+ 树普通索引不存储数据，只存储数据的主键值。比如本表中的 age，它的索引结构就是这样的：

![image-20220721221610389](http://cdn.bluecusliyou.com/202207212216885.png)

> 普通索引等值查询执行过程

```sql
select * from stu where age = 48;
```

使用普通索引需要检索两次索引。第一次检索普通索引找出 age = 48 得到主键值，再使用主键到主键索引中检索获得数据。这个过程称为回表。也就是说，基于非主键索引的查询需要多扫描一遍索引树。因此，我们应该尽量使用主键查询。

过程如下：

- 第一次磁盘 IO：从根节点检索，将数据块1 加载到内存，比较 48 < 54，走左边。
- 第二次磁盘 IO：将左边数据块 2 加载到内存，比较 28<47<48，走右边。
- 第三次磁盘 IO：将右边数据块 6 加载到内存，比较 47<48，48=48。得到主键 38。
- 第四次磁盘 IO：从根节点检索，将根节点加载到内存，比较 38 < 44，走左边。
- 第五次磁盘 IO：将左边数据块 2 加载到内存，比较 8<37<38，走右边。
- 第六次磁盘 IO：将右边数据块 6 加载到内存，比较 37<38，38=38。查询完毕，将数据返回客户端。

![image-20220721222718511](http://cdn.bluecusliyou.com/202207212227989.png)

#### （4）联合索引

> 为什么希望使用覆盖索引？
>
> 如果非聚集索引中能索引覆盖，那么我们只需要遍历非聚集索引这个B+Tree从其中的Key里拿到索引值就结束了，只需要遍历一棵树。 如果不能索引覆盖，需要先遍历非聚集索引，然后拿到data中存储的主键值，再去聚集索引中遍历查找数据，相比索引覆盖的话，IO次数更多，性能相对低。

如果为每一种查询都设计一个索引，索引是不是太多了？如果我现在要根据学生的姓名去查它的年龄。假设这个需求出现的概览很低，但我们也不能让它走全表扫描吧？

但是为一个不频繁的需求创建一个（姓名）索引是不是有点浪费了？那该咋做呢？我们可以建个（name，age）的组合索引来解决呀。

> 组合索引的结构

![image-20220721223049884](http://cdn.bluecusliyou.com/202207212230355.png)

> 组合条件查询执行过程

```sql
select name,age from stu where name='二狗5' and age = 48;
```

过程如下：

- 第一次磁盘 IO：从根节点检索，将数据块1 加载到内存，比较 二狗5 < 二狗6，走左边。
- 第二次磁盘 IO：将左边数据块 2 加载到内存，比较 二狗2<二狗4<二狗5，走右边。
- 第三次磁盘 IO：将右边数据块 6 加载到内存，比较 二狗4<二狗5，二狗5=二狗5。得到主键 38。
- 第四次磁盘 IO：从根节点检索，将根节点加载到内存，比较 38 < 44，走左边。
- 第五次磁盘 IO：将左边数据块 2 加载到内存，比较 8<37<38，走右边。
- 第六次磁盘 IO：将右边数据块 6 加载到内存，比较 37<38，38=38。查询完毕，将数据返回客户端。

![image-20220721223208543](http://cdn.bluecusliyou.com/202207212232991.png)



### 6、索引失效情况

#### （1）数据准备

```sql
-- 创建表
CREATE TABLE `staffs`(
`id` INT(10) PRIMARY KEY AUTO_INCREMENT,
`name` VARCHAR(24) NOT NULL DEFAULT '' COMMENT '姓名',
`age` INT(10) NOT NULL DEFAULT 0 COMMENT '年龄',
`pos` VARCHAR(20) NOT NULL DEFAULT '' COMMENT '职位',
`add_time` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '入职时间'
)COMMENT '员工记录表';

-- 插入数据
INSERT INTO `staffs`(`name`,`age`,`pos`) VALUES('王五', 18, 'manager');
INSERT INTO `staffs`(`name`,`age`,`pos`) VALUES('张三', 20, 'dev');
INSERT INTO `staffs`(`name`,`age`,`pos`) VALUES('李四', 21, 'dev');

-- 创建索引
CREATE INDEX idx_staffs_name_age_pos ON `staffs`(`name`,`age`,`pos`);
```

#### （2）最佳左前缀法则

> 在MySQL建立联合索引时会遵守`最佳左前缀匹配原则`，即最左优先，在检索数据时从联合索引的最左边开始匹配。
>
> MySQL可以为多个字段创建索引，一个索引可以包括16个字段。对于多列索引，`过滤条件要使用索引必须 按照索引建立时的顺序，依次满足，一旦跳过某个字段，索引后面的字段都无法被使用`。如果查询条件中没有使用这些字段的第1个字段时，多列（或联合）索引不会被使用。

```sql
/* 用到了idx_staffs_name_age_pos索引中的name字段 */
EXPLAIN SELECT * FROM `staffs` WHERE `name` = '王五';
```

![image-20220710210715893](http://cdn.bluecusliyou.com/202207102107177.png)

```sql
/* 用到了idx_staffs_name_age_pos索引中的name, age字段 */
EXPLAIN SELECT * FROM `staffs` WHERE `name` = '王五' AND `age` = 18;
```

![image-20220710210832202](http://cdn.bluecusliyou.com/202207102108493.png)

```sql
/* 用到了idx_staffs_name_age_pos索引中的name，age，pos字段 这是属于全值匹配的情况*/
EXPLAIN SELECT * FROM `staffs` WHERE `name` = '王五' AND `age` = 18 AND `pos` = 
'manager';
```

![image-20220710213139429](http://cdn.bluecusliyou.com/202207102131713.png)

```sql
/* 索引没用上，跳过索引的第一个字段，ALL全表扫描 */
EXPLAIN SELECT * FROM `staffs` WHERE `age` = 18 AND `pos` = 'manager';
```

![image-20220710213316017](http://cdn.bluecusliyou.com/202207102133308.png)

```sql
/* 索引没用上，跳过索引的第一个字段第二个字段，ALL全表扫描 */
EXPLAIN SELECT * FROM `staffs` WHERE `pos` = 'manager';
```

![image-20220710213449427](http://cdn.bluecusliyou.com/202207102134717.png)

```sql
/* 用到了idx_staffs_name_age_pos索引中的name字段，pos字段索引失效 */
EXPLAIN SELECT * FROM `staffs` WHERE `name` = '王五' AND `pos` = 'manager';
```

![image-20220710213821237](http://cdn.bluecusliyou.com/202207102138534.png)

#### （3）主键插入不用自增列

对于一个 使用`InnoDB存储引擎`的表来说，在我们没有显式的创建索引时，表中的数据实际上都是存储`在聚簇索引`的叶子节点的。而记录又是存储在数据页中的，数据页和记录又是按照记录`主键值从小到大`的顺序进行排序， 所以如果我们`插入`的记录的`主键值是依次增大`的话，那我们每插满一个数据页就换到下一个数据页继续插，而如 果我们插入的`主键值忽大忽小`的话，就比较麻烦了，假设某个数据页存储的记录已经满了，它存储的主键值在1-100 之间：

![image-20220721185049959](http://cdn.bluecusliyou.com/202207211850089.png)

 如果此时再插入一条主键值为 9的记录，那它插入的位置就如下图：

![image-20220721185100238](http://cdn.bluecusliyou.com/202207211851369.png)

可这个数据页已经满了，我们需要把当前 `页面分裂` 成两个页面，把本页中的一些记录移动到新创建的这个页中。页面分裂和记录移位意味着什么？意味着： `性能损耗` ！所以如果我们想尽量避免这样无谓的性能损耗，最好让插入的记录的 `主键值依次递增` ，这样就不会发生这样的性能损耗了。 所以我们建议：让主键具有 `AUTO_INCREMENT` ，让存储引擎自己为表生成主键，而不是我们手动插入 ，比如： `person_info` 表：

```sql
CREATE TABLE person_info(
	 id INT UNSIGNED NOT NULL AUTO_INCREMENT,
	 name VARCHAR(100) NOT NULL,
	 birthday DATE NOT NULL,
	 phone_number CHAR(11) NOT NULL,
	 country varchar(100) NOT NULL,
	  PRIMARY KEY (id),
	  KEY idx_name_birthday_phone_number (name(10), birthday, phone_number)
);
```

我们自定义的主键列 `id` 拥有`AUTO_INCREMENT` 属性，在插入记录时存储引擎会自动为我们填入自增的主键值。这样的主键占用空间小，顺序写入，减少页分裂。

#### （4）计算、函数导致索引失效

> 在索引列上进行计算，会使索引失效。

```sql
explain SELECT * FROM `staffs` WHERE `name` = '王五';
```

![image-20220710215734941](http://cdn.bluecusliyou.com/202207102157238.png)

```sql
explain SELECT * FROM `staffs` WHERE LEFT(`name`, 2) = '王五';
```

![image-20220710215804742](http://cdn.bluecusliyou.com/202207102158044.png)

#### （5）类型转换导致索引失效

> 这里name = 2000在MySQL中会发生强制类型转换，将数字转成字符串，索引就会失效
>
> 设计实体类属性时，一定要与数据库字段类型相对应。否则，就会出现类型转换的情况

```sql
EXPLAIN SELECT * FROM `staffs` WHERE `name` = '2000';
EXPLAIN SELECT * FROM `staffs` WHERE `name` = 2000;
```

![image-20220711194849270](http://cdn.bluecusliyou.com/202207111948443.png)

#### （6）数据库和表的字符集统一使用utf8mb4

统一使用utf8mb4( 5.5.3版本以上支持)兼容性更好，统一字符集可以避免由于字符集转换产生的乱码。不同的 `字符集` 进行比较前需要进行`转换`会造成索引失效。

#### （7）范围条件右边的列索引失效

```sql
-- 全部索引生效
EXPLAIN SELECT * FROM `staffs` WHERE `name` = '王五' AND `age` = 18 AND `pos` = 
'manager';
-- `age` > 18 范围后面的索引失效
EXPLAIN SELECT * FROM `staffs` WHERE `name` = '张三' AND `age` > 18 AND `pos` = 
'dev';
```

![image-20220710221102209](http://cdn.bluecusliyou.com/202207102211519.png)

#### （8）不等于(!= 或者<>)索引失效

> !=或者<>会使索引失效，使用覆盖索引可以提高性能。
>

```sql
EXPLAIN SELECT * FROM `staffs` WHERE `name` != '王五';
EXPLAIN SELECT * FROM `staffs` WHERE `name` <> '王五';
```

![image-20220710222508861](http://cdn.bluecusliyou.com/202207102225177.png)

#### （9）is null可以使用索引，is not null无法使用索引

```sql
EXPLAIN SELECT name FROM staffs WHERE name is not null;
EXPLAIN SELECT name FROM staffs WHERE name is null;
```

![image-20220721194109586](http://cdn.bluecusliyou.com/202207211941722.png)

#### （10）like以通配符%开头索引失效

> like百分号加载左边会使索引失效

```sql
/* 索引失效 全表扫描 */
EXPLAIN SELECT * FROM `staffs` WHERE `name` LIKE '%王%';
/* 索引失效 全表扫描 */
EXPLAIN SELECT * FROM `staffs` WHERE `name` LIKE '%王';
/* 使用索引范围查询 */
EXPLAIN SELECT * FROM `staffs` WHERE `name` LIKE '王%';
```

![image-20220710222943801](http://cdn.bluecusliyou.com/202207102229149.png)

> 一定要在左边加%用的话，使用覆盖索引

```sql
/* 使用覆盖索引 */
EXPLAIN SELECT name,age,pos FROM `staffs` WHERE `name` LIKE '%王';
```

![image-20220804150626092](http://cdn.bluecusliyou.com/202208041506198.png)

#### （11）OR 前后存在非索引的列，索引失效

在 where 子句中，如果在 OR 前的条件列进行了索引，而在OR 后的条件列没有进行索引，那么索引失效，也就是，`让OR的前后条件都具备索引，如果缺少一个就会出现索引失效`

因为 OR 的含义就是两个只要满足一个即可，因此`只有一个条件列进行了索引时没有意义的`。只要`有条件列没有索引`，就会进行`全表扫描`，因此 所有的条件列也会失效。

```sql
-- 索引生效
EXPLAIN SELECT name,age FROM `staffs` WHERE `name`='zhangsan' or age=18;
-- 索引失效 add_time不在索引范围
EXPLAIN SELECT name,age FROM `staffs` WHERE `name`='zhangsan' or add_time='2022-07-20 02:28:58';
```

![image-20220721194546405](http://cdn.bluecusliyou.com/202207211945543.png)

#### （12）一般性总结建议

> 假设index(a,b,c)，案例总结

| Where语句                                              | 索引是否被使用                             |
| ------------------------------------------------------ | ------------------------------------------ |
| where a = 3                                            | Y，使用到a                                 |
| where a = 3 and b = 5                                  | Y，使用到a，b                              |
| where a = 3 and b = 5  and c=6                         | Y，使用到a，b，c                           |
| where b = 3 或者 where b = 3 and c = 4 或者 where c= 4 | N，没有用到a字段                           |
| where a = 3 and c = 5                                  | 使用到a，但是没有用到c，因为b断了          |
| where a = 3 and b > 4 and c = 5                        | 使用到a，b，但是没有用到c，因为c在范围之后 |
| where a = 3 and b like 'kk%' and c = 4                 | Y，a，b，c都用到                           |
| where a = 3 and b like '%kk' and c = 4                 | 只用到a                                    |
| where a = 3 and b like '%kk%' and c = 4                | 只用到a                                    |
| where a = 3 and b like 'k%kk%' and c = 4               | Y，a，b，c都用到                           |

> 一般性建议

- 对于单值索引，尽量选择针对当前 query 过滤性更好的索引。
- 在选择复合索引的时候，当前 query 中过滤性最好的字段在索引字段顺序中，位置越靠前越好。
- 在选择复合索引的时候，尽量选择可以能够包含当前 query 中的 where 子句中更多字段的索引。
- 尽可能通过分析统计信息和调整 query 的写法来达到选择合适索引的目的

### 7、连接详解

#### （1）连接的本质

> 数据准备

```sql
CREATE TABLE t1 (m1 int, n1 char(1));
CREATE TABLE t2 (m2 int, n2 char(1));
INSERT INTO t1 VALUES(1, 'a'), (2, 'b'), (3, 'c');
INSERT INTO t2 VALUES(2, 'b'), (3, 'c'), (4, 'd');
```

`连接`的本质就是把各个连接表中的记录都取出来依次匹配的组合加入结果集并返回给用户。所以我们把`t1`和`t2`两个表连接起来的过程如下图所示：

```sql
mysql> SELECT * FROM t1, t2;
+----+----+----+----+
| m1 | n1 | m2 | n2 |
+----+----+----+----+
|  3 | c  |  2 | b  |
|  2 | b  |  2 | b  |
|  1 | a  |  2 | b  |
|  3 | c  |  3 | c  |
|  2 | b  |  3 | c  |
|  1 | a  |  3 | c  |
|  3 | c  |  4 | d  |
|  2 | b  |  4 | d  |
|  1 | a  |  4 | d  |
+----+----+----+----+
```

![image-20220720072801499](http://cdn.bluecusliyou.com/202207200728713.png)

​        这个过程看起来就是把`t1`表的记录和`t2`的记录连起来组成新的更大的记录，所以这个查询过程称之为`连接查询`。`连接查询的结果集中包含一个表中的每一条记录与另一个表中的每一条记录相互匹配的组合`，像这样的结果集就可以称之为`笛卡尔积`。因为表`t1`中有3条记录，表`t2`中也有3条记录，所以这两个表连接之后的笛卡尔积就有`3×3=9`行记录。在`MySQL`中，连接查询的语法也很随意，只要在`FROM`语句后边跟多个表名就好了。

#### （2）连接过程

​        如果我们乐意，我们可以连接任意数量张表，但是如果没有任何限制条件的话，这些表连接起来产生的`笛卡尔积`可能是非常巨大的。比方说3个100行记录的表连接起来产生的`笛卡尔积`就有`100×100×100=1000000`行数据！所以在连接的时候过滤掉特定记录组合是有必要的。

- 涉及单表的条件

​         这种只设计单表的过滤条件我们之前都提到过一万遍了，我们之前也一直称为`搜索条件`，比如`t1.m1 > 1`是只针对`t1`表的过滤条件，`t2.n2 < 'd'`是只针对`t2`表的过滤条件。

```sql
SELECT * FROM t1, t2 where t1.m1 > 1;
SELECT * FROM t1, t2 where t2.n2 < 'd';
```

- 涉及两表的条件

​        这种过滤条件我们之前没见过，比如`t1.m1 = t2.m2`、`t1.n1 > t2.n2`等，这些条件中涉及到了两个表，我们稍后会仔细分析这种过滤条件是如何使用的。

```sql
SELECT * FROM t1, t2 where t1.m1 = t2.m2;
SELECT * FROM t1, t2 where t1.n1 > t2.n2;
```

> 连接查询的大致执行过程

```sql
SELECT * FROM t1, t2 WHERE t1.m1 > 1 AND t1.m1 = t2.m2 AND t2.n2 < 'd';
```

在这个查询中我们指明了这三个过滤条件：

- `t1.m1 > 1`
- `t1.m1 = t2.m2`
- `t2.n2 < 'd'`

那么这个连接查询的大致执行过程如下：

​        首先确定第一个需要查询的表，这个表称之为`驱动表`。怎样在单表中执行查询语句我们在前一章都介绍过了，只需要选取代价最小的那种访问方法去执行单表查询语句就好了（就是说从const、ref、ref_or_null、range、index、all这些执行方法中选取代价最小的去执行查询）。此处假设使用`t1`作为驱动表，那么就需要到`t1`表中找满足`t1.m1 > 1`的记录，因为表中的数据太少，我们也没在表上建立二级索引，所以此处查询`t1`表的访问方法就设定为`all`吧。

![image-20220720074449867](http://cdn.bluecusliyou.com/202207200744997.png)

​         我们可以看到，`t1`表中符合`t1.m1 > 1`的记录有两条。针对上一步骤中从驱动表产生的结果集中的每一条记录，分别需要到`t2`表中查找匹配的记录，所谓`匹配的记录`，指的是符合过滤条件的记录。因为是根据`t1`表中的记录去找`t2`表中的记录，所以`t2`表也可以被称之为`被驱动表`。上一步骤从驱动表中得到了2条记录，所以需要查询2次`t2`表。此时涉及两个表的列的过滤条件`t1.m1 = t2.m2`就派上用场了：

- 当`t1.m1 = 2`时，过滤条件`t1.m1 = t2.m2`就相当于`t2.m2 = 2`，所以此时`t2`表相当于有了`t2.m2 = 2`、`t2.n2 < 'd'`这两个过滤条件，然后到`t2`表中执行单表查询。

- 当`t1.m1 = 3`时，过滤条件`t1.m1 = t2.m2`就相当于`t2.m2 = 3`，所以此时`t2`表相当于有了`t2.m2 = 3`、`t2.n2 < 'd'`这两个过滤条件，然后到`t2`表中执行单表查询。

  所以整个连接查询的执行过程就如下图所示：

![image-20220720074731638](http://cdn.bluecusliyou.com/202207200747792.png)



​         也就是说整个连接查询最后的结果只有两条符合过滤条件的记录。从上面两个步骤可以看出来，我们上面介绍的这个两表连接查询共需要查询1次`t1`表，2次`t2`表。当然这是在特定的过滤条件下的结果，如果我们把`t1.m1 > 1`这个条件去掉，那么从`t1`表中查出的记录就有3条，就需要查询3次`t2`表了。也就是说在两表连接查询中，驱动表只需要访问一次，被驱动表可能多次。

#### （3）内连接和外连接

> 数据准备

```sql
-- 学生信息表
CREATE TABLE studentinfo (
    number INT NOT NULL COMMENT '学号',
    name VARCHAR(5) COMMENT '姓名',
    major VARCHAR(30) COMMENT '专业',
    PRIMARY KEY (number)
) Engine=InnoDB CHARSET=utf8 COMMENT '学生信息表';

-- 学生成绩表
CREATE TABLE score (
    number INT COMMENT '学号',
    subject VARCHAR(30) COMMENT '科目',
    score TINYINT COMMENT '成绩',
    PRIMARY KEY (number, score)
) Engine=InnoDB CHARSET=utf8 COMMENT '学生成绩表';
-- 插入数据
insert into studentinfo values
(20180101,'杜子腾','软件学院'),
(20180102,'范统','计算机科学与工程'),
(20180103,'史珍香','计算机科学与工程');
insert into score values
(20180101,'母猪的产后护理',78),
(20180101,'论萨达姆的战争准备',88),
(20180102,'论萨达姆的战争准备',98),
(20180102,'母猪的产后护理',100);

-- 数据如下
mysql> select *  from studentinfo;
+----------+--------+------------------+
| number   | name   | major            |
+----------+--------+------------------+
| 20180101 | 杜子腾 | 软件学院         |
| 20180102 | 范统   | 计算机科学与工程 |
| 20180103 | 史珍香 | 计算机科学与工程 |
+----------+--------+------------------+
3 rows in set (0.05 sec)

mysql> select *  from score;
+----------+--------------------+-------+
| number   | subject            | score |
+----------+--------------------+-------+
| 20180101 | 母猪的产后护理     |    78 |
| 20180101 | 论萨达姆的战争准备 |    88 |
| 20180102 | 论萨达姆的战争准备 |    98 |
| 20180102 | 母猪的产后护理     |   100 |
+----------+--------------------+-------+
4 rows in set (0.09 sec)
```

> 内连接和外连接

​         现在我们想把每个学生的考试成绩都查询出来就需要进行两表连接了（因为`score`中没有姓名信息，所以不能单纯只查询`score`表）。连接过程就是从`student`表中取出记录，在`score`表中查找`number`相同的成绩记录，所以过滤条件就是`student.number = socre.number`，整个查询语句就是这样：

```sql
mysql> SELECT s1.number, s1.name, s2.subject, s2.score FROM studentinfo AS s1, score AS s2 WHERE s1.number = s2.number;
+----------+--------+--------------------+-------+
| number   | name   | subject            | score |
+----------+--------+--------------------+-------+
| 20180101 | 杜子腾 | 母猪的产后护理     |    78 |
| 20180101 | 杜子腾 | 论萨达姆的战争准备 |    88 |
| 20180102 | 范统   | 论萨达姆的战争准备 |    98 |
| 20180102 | 范统 | 母猪的产后护理     |   100 |
+----------+--------+--------------------+-------+
```

​        从上述查询结果中我们可以看到，各个同学对应的各科成绩就都被查出来了，可是有个问题，`史珍香`同学，也就是学号为`20180103`的同学因为某些原因没有参加考试，所以在`score`表中没有对应的成绩记录。那如果老师想查看所有同学的考试成绩，即使是缺考的同学也应该展示出来，但是到目前为止我们介绍的`连接查询`是无法完成这样的需求的。我们稍微思考一下这个需求，其本质是想：驱动表中的记录即使在被驱动表中没有匹配的记录，也仍然需要加入到结果集。为了解决这个问题，就有了`内连接`和`外连接`的概念：

- 对于`内连接`的两个表，驱动表中的记录在被驱动表中找不到匹配的记录，该记录不会加入到最后的结果集，我们上面提到的连接都是所谓的`内连接`。
- 对于`外连接`的两个表，驱动表中的记录即使在被驱动表中没有匹配的记录，也仍然需要加入到结果集。在`MySQL`中，根据选取驱动表的不同，外连接仍然可以细分为2种：

- `左（外）连接`：选取左侧的表为驱动表。
- `右（外）连接`：选取右侧的表为驱动表。

​        可是这样仍然存在问题，即使对于`外连接`来说，有时候我们也并不想把驱动表的全部记录都加入到最后的结果集。这就犯难了，有时候匹配失败要加入结果集，有时候又不要加入结果集，把过滤条件分为两种就解决了这个问题了么，所以放在不同地方的过滤条件是有不同语义的：

- `WHERE`子句中的过滤条件

  `WHERE`子句中的过滤条件就是我们平时见的那种，不论是内连接还是外连接，凡是不符合`WHERE`子句中的过滤条件的记录都不会被加入最后的结果集。

- `ON`子句中的过滤条件

  对于外连接的驱动表的记录来说，如果无法在被驱动表中找到匹配`ON`子句中的过滤条件的记录，那么该记录仍然会被加入到结果集中，对应的被驱动表记录的各个字段使用`NULL`值填充。

​         需要注意的是，这个`ON`子句是专门为外连接驱动表中的记录在被驱动表找不到匹配记录时应不应该把该记录加入结果集这个场景下提出的，所以如果把`ON`子句放到内连接中，`MySQL`会把它和`WHERE`子句一样对待，也就是说：内连接中的WHERE子句和ON子句是等价的。

  一般情况下，我们都把只涉及单表的过滤条件放到`WHERE`子句中，把涉及两表的过滤条件都放到`ON`子句中，我们也一般把放到`ON`子句中的过滤条件也称之为`连接条件`。

> 左（外）连接的语法

```sql
SELECT * FROM t1 LEFT [OUTER] JOIN t2 ON 连接条件 [WHERE 普通过滤条件];
```

​         其中，中括号里的`OUTER`单词是可以省略的。对于`LEFT JOIN`类型的连接来说，我们把放在`左边的表称之为外表或者驱动表`，`右边的表称之为内表或者被驱动表`。所以上述例子中`t1`就是外表或者驱动表，`t2`就是内表或者被驱动表。需要注意的是，对于左（外）连接和右（外）连接来说，必须使用`ON`子句来指出连接条件。

​        了解了左（外）连接的基本语法之后，再次回到我们上面那个现实问题中来，看看怎样写查询语句才能把所有的学生的成绩信息都查询出来，即使是缺考的考生也应该被放到结果集中：

```sql
mysql> SELECT s1.number, s1.name, s2.subject, s2.score FROM studentinfo AS s1 LEFT JOIN score AS s2 ON s1.number = s2.number;
+----------+--------+--------------------+-------+
| number   | name   | subject            | score |
+----------+--------+--------------------+-------+
| 20180101 | 杜子腾 | 母猪的产后护理     |    78 |
| 20180101 | 杜子腾 | 论萨达姆的战争准备 |    88 |
| 20180102 | 范统   | 论萨达姆的战争准备 |    98 |
| 20180102 | 范统   | 母猪的产后护理     |   100 |
| 20180103 | 史珍香 | NULL               | NULL  |
+----------+--------+--------------------+-------+
```

​        从结果集中可以看出来，虽然`史珍香`并没有对应的成绩记录，但是由于采用的是连接类型为左（外）连接，所以仍然把她放到了结果集中，只不过在对应的成绩记录的各列使用`NULL`值填充而已。

> 右（外）连接的语法

```sql
SELECT * FROM t1 RIGHT [OUTER] JOIN t2 ON 连接条件 [WHERE 普通过滤条件];
```

​         右（外）连接和左（外）连接的原理是一样一样的，语法也只是把`LEFT`换成`RIGHT`而已，只不过驱动表是右边的表，被驱动表是左边的表。

> 内连接的语法

  内连接和外连接的根本区别就是在驱动表中的记录不符合`ON`子句中的连接条件时不会把该记录加入到最后的结果集，我们最开始介绍的那些连接查询的类型都是内连接。不过之前仅仅提到了一种最简单的内连接语法，就是直接把需要连接的多个表都放到`FROM`子句后边。其实针对内连接，MySQL提供了好多不同的语法，我们以`t1`和`t2`表为例看看：

```sql
SELECT * FROM t1 [INNER | CROSS] JOIN t2 [ON 连接条件] [WHERE 普通过滤条件];
```

也就是说在`MySQL`中，下面这几种内连接的写法都是等价的：

- SELECT * FROM t1 JOIN t2;
- SELECT * FROM t1 INNER JOIN t2;
- SELECT * FROM t1 CROSS JOIN t2;
- SELECT * FROM t1, t2;

​         现在我们虽然介绍了很多种`内连接`的书写方式，不过熟悉一种就好了，这里我们推荐`INNER JOIN`的形式书写内连接（因为`INNER JOIN`语义很明确嘛，可以和`LEFT JOIN`和`RIGHT JOIN`很轻松的区分开）。这里需要注意的是，由于在内连接中ON子句和WHERE子句是等价的，所以内连接中不要求强制写明ON子句。

​		  我们前面说过，连接的本质就是把各个连接表中的记录都取出来依次匹配的组合加入结果集并返回给用户。不论哪个表作为驱动表，两表连接产生的笛卡尔积肯定是一样的。而对于内连接来说，由于凡是不符合`ON`子句或`WHERE`子句中的条件的记录都会被过滤掉，所以对于内连接来说，驱动表和被驱动表是可以互换的，并不会影响最后的查询结果。但是对于外连接来说，由于驱动表中的记录即使在被驱动表中找不到符合`ON`子句连接条件的记录，所以外连接的驱动表和被驱动表不能轻易互换。

#### （4）join 语句原理

​			上面的介绍都只是为了唤醒大家对`连接`、`内连接`、`外连接`这些概念的记忆，这些基本概念是为了真正进入本章主题做的铺垫。真正的重点是MySQL采用了什么样的算法来进行表与表之间的连接，了解了这个之后，大家才能明白为什么有的连接查询运行的快如闪电，有的却慢如蜗牛。

> 驱动表和被驱动表

​		  join方式连接多个表，本质就是各个表之间数据的循环匹配。MySQL5.5版本之前，MySQL只支持一种表间关联方式，就是嵌套循环(Nested Loop Join)。如果关联表的数据量很大，则join关联的执行时间会非常长。在MySQL5.5 以后的版本中，MySQL通过引入BNLJ算法来优化嵌套执行。

```sql
-- 创建表
CREATE TABLE a(f1 INT, f2 INT, INDEX(f1))ENGINE=INNODB;
CREATE TABLE b(f1 INT, f2 INT)ENGINE=INNODB;

-- 插入数据
INSERT INTO a VALUES(1,1),(2,2),(3,3),(4,4),(5,5),(6,6);
INSERT INTO b VALUES(3,3),(4,4),(5,5),(6,6),(7,7),(8,8);

-- 内连接查询
explain select * from a join b on a.f1=b.f1;

-- 外连接查询
explain select * from a left join b on a.f1=b.f1 where a.f2=b.f2;

explain select * from a left join b on a.f1=b.f1 and a.f2=b.f2;
```

对于内连接来说，A一定是驱动表吗？不一定，优化器会根据你查询语句做优化，决定先查哪张表。先查询的是驱动表， 反之就是被驱动表。

对于外连接来说，A也不一定是驱动表。

![image-20220720153158999](http://cdn.bluecusliyou.com/202207201531241.png)

> 简单嵌套循环连接（Simple Nested-Loop Join）

  我们前面说过，对于两表连接来说，`驱动表只会被访问一遍，但被驱动表却要被访问到好多遍`，具体访问几遍取决于对驱动表执行单表查询后的结果集中的记录条数。对于`内连接来说，选取哪个表为驱动表都没关系`，而`外连接的驱动表是固定的`，也就是说`左（外）连接的驱动表就是左边的那个表`，`右（外）连接的驱动表就是右边的那个表`。我们上面已经大致介绍过`t1`表和`t2`表执行内连接查询的大致过程，我们温习一下：

- 步骤1：选取驱动表，使用与驱动表相关的过滤条件，选取代价最低的单表访问方法来执行对驱动表的单表查询。
- 步骤2：对上一步骤中查询驱动表得到的结果集中每一条记录，都分别到被驱动表中查找匹配的记录。

![image-20220720155305421](http://cdn.bluecusliyou.com/202207201553695.png)

  如果有3个表进行连接的话，那么`步骤2`中得到的结果集就像是新的驱动表，然后第三个表就成为了被驱动表，重复上面过程，也就是`步骤2`中得到的结果集中的每一条记录都需要到`t3`表中找一找有没有匹配的记录，用伪代码表示一下这个过程就是这样：

```sql
for each row in t1 {   #此处表示遍历满足对t1单表查询结果集中的每一条记录
    
    for each row in t2 {   #此处表示对于某条t1表的记录来说，遍历满足对t2单表查询结果集中的每一条记录
    
        for each row in t3 {   #此处表示对于某条t1和t2表的记录组合来说，对t3表进行单表查询
            if row satisfies join conditions, send to client
        }
    }
}
```

​         这个过程就像是一个嵌套的循环，所以这种`驱动表只访问一次`，但`被驱动表却可能被多次访问`，访问次数取决于对驱动表执行单表查询后的结果集中的记录条数的连接执行方式称之为`嵌套循环连接`（`Nested-Loop Join`），这是最简单，也是最笨拙的一种连接查询算法。

> 索引嵌套循环连接(Index Nested-Loop Join)

  我们知道在`嵌套循环连接`的`步骤2`中可能需要访问多次被驱动表，如果访问被驱动表的方式都是全表扫描的话，性能堪忧，查询`t2`表其实就相当于单表扫描，我们可以利用索引来加快查询速度。回顾一下最开始介绍的`t1`表和`t2`表进行内连接的例子：

```sql
SELECT * FROM t1, t2 WHERE t1.m1 > 1 AND t1.m1 = t2.m2 AND t2.n2 < 'd';
```

我们使用的其实是`嵌套循环连接`算法执行的连接查询，再把上面那个查询执行过程表拉下来给大家看一下：

![image-20220720155904494](http://cdn.bluecusliyou.com/202207201559997.png)

查询驱动表`t1`后的结果集中有两条记录，`嵌套循环连接`算法需要对被驱动表查询2次：

- 当`t1.m1 = 2`时，去查询一遍`t2`表，对`t2`表的查询语句相当于：

```sql
  SELECT * FROM t2 WHERE t2.m2 = 2 AND t2.n2 < 'd';
```

- 当`t1.m1 = 3`时，再去查询一遍`t2`表，此时对`t2`表的查询语句相当于：

```sql
  SELECT * FROM t2 WHERE t2.m2 = 3 AND t2.n2 < 'd';
```

  可以看到，原来的`t1.m1 = t2.m2`这个涉及两个表的过滤条件在针对`t2`表做查询时关于`t1`表的条件就已经确定了，所以我们只需要单单优化对`t2`表的查询了，上述两个对`t2`表的查询语句中利用到的列是`m2`和`n2`列，我们可以：

​        在`m2`列上建立索引，因为对`m2`列的条件是等值查找，比如`t2.m2 = 2`、`t2.m2 = 3`等，所以可能使用到`ref`的访问方法，假设使用`ref`的访问方法去执行对`t2`表的查询的话，需要回表之后再判断`t2.n2 < d`这个条件是否成立。

  这里有一个比较特殊的情况，就是假设`m2`列是`t2`表的主键或者唯一二级索引列，那么使用`t2.m2 = 常数值`这样的条件从`t2`表中查找记录的过程的代价就是常数级别的。我们知道在单表中使用主键值或者唯一二级索引列的值进行等值查找的方式称之为`const`，而设计`MySQL`的大佬把在连接查询中对被驱动表使用主键值或者唯一二级索引列的值进行等值查找的查询执行方式称之为：`eq_ref`。

​       在`n2`列上建立索引，涉及到的条件是`t2.n2 < 'd'`，可能用到`range`的访问方法，假设使用`range`的访问方法对`t2`表的查询的话，需要回表之后再判断在`m2`列上的条件是否成立。

  假设`m2`和`n2`列上都存在索引的话，那么就需要从这两个里边儿挑一个代价更低的去执行对`t2`表的查询。当然，建立了索引不一定使用索引，只有在`二级索引 + 回表`的代价比全表扫描的代价更低时才会使用索引。

  另外，有时候连接查询的查询列表和过滤条件中可能只涉及被驱动表的部分列，而这些列都是某个索引的一部分，这种情况下即使不能使用`eq_ref`、`ref`、`ref_or_null`或者`range`这些访问方法执行对被驱动表的查询的话，也可以使用索引扫描，也就是`index`的访问方法来查询被驱动表。所以我们建议在真实工作中最好不要使用`*`作为查询列表，最好把真实用到的列作为查询列表。

> 基于块的嵌套循环连接（Block Nested-Loop Join）

  扫描一个表的过程其实是先把这个表从磁盘上加载到内存中，然后从内存中比较匹配条件是否满足。现实生活中的表可不像`t1`、`t2`这种只有3条记录，成千上万条记录都是少的，几百万、几千万甚至几亿条记录的表到处都是。内存里可能并不能完全存放的下表中所有的记录，所以在扫描表前面记录的时候后边的记录可能还在磁盘上，等扫描到后边记录的时候可能内存不足，所以需要把前面的记录从内存中释放掉。我们前面又说过，采用`嵌套循环连接`算法的两表连接过程中，被驱动表可是要被访问好多次的，如果这个被驱动表中的数据特别多而且不能使用索引进行访问，那就相当于要从磁盘上读好几次这个表，这个`I/O`代价就非常大了，所以我们得想办法：`尽量减少访问被驱动表的次数`。

  当被驱动表中的数据非常多时，每次访问被驱动表，被驱动表的记录会被加载到内存中，在内存中的每一条记录只会和驱动表结果集的一条记录做匹配，之后就会被从内存中清除掉。然后再从驱动表结果集中拿出另一条记录，再一次把被驱动表的记录加载到内存中一遍，周而复始，驱动表结果集中有多少条记录，就得把被驱动表从磁盘上加载到内存中多少次。所以我们可不可以在`把被驱动表的记录加载到内存的时候，一次性和多条驱动表中的记录做匹配`，这样就可以大大`减少重复从磁盘上加载被驱动表的代价`了。

​		   所以设计`MySQL`的大佬提出了一个`join buffer`的概念，`join buffer`就是执行`连接查询前申请的一块固定大小的内存`，先把若干条驱动表结果集中的记录装在这个`join buffer`中，然后开始扫描被驱动表，每一条被驱动表的记录一次性和`join buffer`中的多条驱动表记录做匹配，因为匹配的过程都是在内存中完成的，所以这样可以显著减少被驱动表的`I/O`代价。使用`join buffer`的过程如下图所示：

![image-20220720161221462](http://cdn.bluecusliyou.com/202207201612094.png)

  最好的情况是`join buffer`足够大，能容纳驱动表结果集中的所有记录，这样只需要访问一次被驱动表就可以完成连接操作了。设计`MySQL`的大佬把这种加入了`join buffer`的嵌套循环连接算法称之为`基于块的嵌套连接`（Block Nested-Loop Join）算法。

  这个`join buffer`的大小是可以通过启动参数或者系统变量`join_buffer_size`进行配置，默认大小为`262144字节`（也就是`256KB`），最小可以设置为`128字节`。当然，对于优化被驱动表的查询来说，最好是为被驱动表加上效率高的索引，如果实在不能使用索引，并且自己的机器的内存也比较大可以尝试调大`join_buffer_size`的值来对连接查询进行优化。

  另外需要注意的是，驱动表的记录并不是所有列都会被放到`join buffer`中，只有查询列表中的列和过滤条件中的列才会被放到`join buffer`中，所以再次提醒我们，最好不要把`*`作为查询列表，只需要把我们关心的列放到查询列表就好了，这样还可以在`join buffer`中放置更多的记录呢。

> 总结

- `整体效率比较：INLJ>BNLJ>SNLJ`
- 永远用`小结果集驱动大结果集`（其本质就是减少外层循环的数据数量）（小的度量单位指的是表行数 * 每行大小）

- 为`被驱动表匹配的条件增加索引`（减少内层表的循环匹配次数）
- 增大`join buffer size`的大小（一次缓存的数据越多，那么内层包的扫表次数就越少）
- 减少驱动表不必要的字段查询（字段越少，`join buffer`所缓存的数据就越多）

#### （5）hash join

从MySQL的8.0.20版本开始将废弃`BNLJ`,因为从MySQL8.0.18版本开始就加入了hash join默认都会使用hash join

- `Nested Loop`: 对于被连接的数据子集较小的情况，`Nested Loop`是个较好的选择。
- `Hash Join`是做`大数据集连接时`的常用方式，优化器使用两个表中较小(相对较小)的表利用Join Key在内存中 建立`散列表`，然后扫描较大的表并探测散列表，找出与Hash表匹配的行。
- 这种方式适用于较小的表完全可以放于内存中的情况，这样总成本就是访问两个表的成本之和。
- 在表很大的情况下并不能完全放入内存，这时优化器会将它分割成若干不同的分区，不能放入内存的部分 就把该分区写入磁盘的临时段，此时要求有较大的临时段从而尽量提高I/O的性能。
- 它能够很好的工作于没有索引的大表和并行查询的环境中，并提供最好的性能。大多数人都说它是Join的 重型升降机。Hash Join只能应用于等值连接(如WHERE A.COL1 = B.COL2),这是由Hash的特点决定的。

### 8、IN子查询优化

#### （1）小表驱动大表

> IN和EXISTS

```sql
/* 优化原则：小表驱动大表，即小的数据集驱动大的数据集 */
/* IN适合B表比A表数据小的情况*/
SELECT * FROM `A` WHERE `id` IN (SELECT `id` FROM `B`)
/* EXISTS适合B表比A表数据大的情况 */
SELECT * FROM `A` WHERE EXISTS (SELECT 1 FROM `B` WHERE `B`.id = `A`.id);
```

> EXISTS语法

- 语法： SELECT....FROM tab WHERE EXISTS(subquery); 该语法可以理解为：
- 该语法可以理解为：将主查询的数据，放到子查询中做条件验证，根据验证结果（ true 或是false ）来决定主查询的数据结果是否得以保留。
- EXISTS(subquery) 子查询只返回 true 或者 false ，因此子查询中的 SELECT * 可以是SELECT 1 OR SELECT X ，它们并没有区别。
- EXISTS(subquery) 子查询的实际执行过程可能经过了优化而不是我们理解上的逐条对比，如果担心效率问题，可进行实际检验以确定是否有效率问题。
- EXISTS(subquery) 子查询往往也可以用条件表达式，其他子查询或者 JOIN 替代，何种最优需要具体问题具体分析。

#### （2）将子查询转换成连接查询

​			MySQL从4.1版本开始支持子查询，使用子查询可以进行SELECT语句的嵌套查询，即一个SELECT查询的结果作为另一个SELECT语句的条件。 子查询可以一次性完成很多逻辑上需要多个步骤才能完成的SQL操作。

​			子查询是 MySQL 的一项重要的功能，可以帮助我们通过一个 SQL 语句实现比较复杂的查询。但是，子查询的执行效率不高。

**原因：**

- 执行子查询时，MySQL需要为内层查询语句的查询结果`建立一个临时表` ，然后外层查询语句`从临时表中查询记录`。查询完毕后，再撤销这些临时表 。这样会消耗过多的CPU和IO资源，产生大量的慢查询。
- 子查询的结果集存储的临时表，不论是内存临时表还是磁盘临时表都 不会存在索引 ，所以查询性能会受到一定的影响。
- 对于返回结果集比较大的子查询，其对查询性能的影响也就越大。

​			在MySQL中，可以使用`连接（JOIN）查询来替代子查询`。 连接查询 不需要建立临时表，其 `速度比子查询要快` ，如果查询中使用索引的话，性能就会更好。尽量不要使用NOT IN 或者 NOT EXISTS，用LEFT JOIN xxx ON xx WHERE xx IS NULL替代。

### 9、ORDER BY排序优化

#### （1）ORDER BY字段索引原理

**在MySQL中，支持两种排序方式，分别是`FileSort`和`Index排序`。**

- Index排序中，索引可以保证数据的有序性，不需要再进行排序，`效率更高`。
- FileSort排序则一般在`内存中`进行排序，`占用CPU较多`。如果待排结果较大，会产生临时文件I/O，效率较低。

**优化建议：**

- 可以在ORDER BY 子句中使用索引，目的是在 ORDER BY 子句`避免使用 FileSort 排序` ，尽量使用 `Index`排序。当然，某些情况下全表扫描，或者 FileSort 排序不一定比索引慢。
-  如果 `WHERE` 和 `ORDER BY` 后面是相同的列就使用单索引列；如果不同就使用联合索引。
- `ORDER BY` 满足两情况，会使用 `Index` 方式排序：
  - `ORDER BY` 语句使用索引最左前列。

  - 使用 `WHERE` 子句与 `ORDER BY` 子句条件列组合满足索引最左前列。

#### （2）ORDER BY优化

> 不加索引，不管有没有加limit，都会使用filesort

```sql
explain select sql_no_cache * from student order by age,classid;
explain select sql_no_cache * from student order by age,classid limit 10;
```

![image-20220720202137935](http://cdn.bluecusliyou.com/202207202021041.png)

> 加limit限制索引生效，不加索引失效

```sql
-- 创建索引
CREATE  INDEX idx_age_classid_name ON student (age,classid,NAME);

-- 不加limit限制,索引失效
EXPLAIN  SELECT SQL_NO_CACHE * FROM student ORDER BY age,classid;

-- 加limit限制，索引生效
EXPLAIN  SELECT SQL_NO_CACHE * FROM student ORDER BY age,classid LIMIT 10;
```

![image-20220720203158354](http://cdn.bluecusliyou.com/202207202031468.png)

> order by 顺序错误，索引失效

```sql
-- 索引生效
EXPLAIN  SELECT * FROM student ORDER BY age LIMIT 10;
EXPLAIN  SELECT * FROM student ORDER BY age,classid LIMIT 10;
EXPLAIN  SELECT * FROM student ORDER BY age,classid,name LIMIT 10;

-- 索引失效
EXPLAIN  SELECT * FROM student ORDER BY classid LIMIT 10;
EXPLAIN  SELECT * FROM student ORDER BY classid,name LIMIT 10;
EXPLAIN  SELECT * FROM student ORDER BY age,name LIMIT 10;
EXPLAIN  SELECT * FROM student ORDER BY name,classid,age LIMIT 10;
```

![image-20220720211106736](http://cdn.bluecusliyou.com/202207202111897.png)

> order by排序不一致，索引失效

```sql
-- 排序不一致，索引失效
EXPLAIN  SELECT * FROM student ORDER BY age DESC, classid ASC LIMIT 10;
-- 排序一致，索引有效
EXPLAIN  SELECT * FROM student ORDER BY age DESC, classid DESC LIMIT 10;
```

![image-20220720212733129](http://cdn.bluecusliyou.com/202207202127274.png)

> WHERE子句中使用索引的最左前缀定义为常量，则ORDER BY能使用索引

```sql
-- 索引生效
EXPLAIN  SELECT * FROM student WHERE age=45 ORDER BY classid;
EXPLAIN  SELECT * FROM student WHERE age=45 ORDER BY classid,NAME; 
-- 索引失效
EXPLAIN  SELECT * FROM student WHERE classid=45 ORDER BY age;
-- 索引生效
EXPLAIN  SELECT * FROM student WHERE classid=45 ORDER BY age LIMIT 10;
```

![image-20220720213203367](http://cdn.bluecusliyou.com/202207202132528.png)

> 案例分析，查询年龄为30岁的，且学生编号小于101000的学生，按用户名称排序

```sql
-- 先删除索引排除干扰
DROP INDEX idx_age_classid_name ON student;
EXPLAIN SELECT SQL_NO_CACHE * FROM student WHERE age = 30 AND stuno <101000 ORDER BY NAME;
```

![image-20220720214721155](http://cdn.bluecusliyou.com/202207202147303.png)

```sql
-- 优化方案1，创建索引
CREATE INDEX idx_age_name ON student(age,NAME);
EXPLAIN SELECT SQL_NO_CACHE * FROM student WHERE age = 30 AND stuno <101000 ORDER BY NAME;
```

![image-20220720214736252](http://cdn.bluecusliyou.com/202207202147432.png)

```sql
-- 优化方案2，尽量让where的过滤条件和排序使用上索引 建一个三个字段的组合索引
DROP INDEX idx_age_name ON student;
CREATE INDEX idx_age_stuno_name ON student (age,stuno,NAME);
EXPLAIN SELECT SQL_NO_CACHE * FROM student WHERE age = 30 AND stuno <101000 ORDER BY NAME;
```

![image-20220720214921422](http://cdn.bluecusliyou.com/202207202149565.png)

结论：

- 两个索引同时存在，mysql自动选择最优的方案。但是， 随着数据量的变化，选择的索引也会随之变化的 。
- 当【范围条件】和【group by 或者 order by】的字段出现二选一时，优先观察条件字段的过滤数量，如果过滤的数据足够多，而需要排序的数据并不多时，优先把索引放在范围字段上。反之，亦然。

#### （3）FileSort有两种算法

如果实在是无法用上索引，只能FileSort，也要对FileSort进行优化，FileSort有两种算法：MySQL就要启动双路排序算法和单路排序算法。

**双路排序算法**

- MySQL4.1之前使用双路排序，字面意思就是两次扫描磁盘，最终得到数据，读取行指针和 `ORDER BY` 列，対他们进行排序，然后扫描已经排序好的列表，按照列表中的值重新从列表中读取对应的数据输出。
- 从磁盘取排序字段，在`buffer`中进行排序，再从磁盘取其他字段。

**单路排序算法**

- 对磁盘进行两次扫描，IO是很耗时的，所以在MySQL4.1之后，出现了改进的算法，就是单路排序算法。
- 从磁盘读取查询需要的所有列，按照 `ORDER BY` 列在 `buffer` 対它们进行排序，然后扫描排序后的列表进行输出，它的效率更快一些，避免了第二次读取数据。并且把随机IO变成了顺序IO，但是它会使用更多的空间，因为它把每一行都保存在内存中了。
- 但是单路排序算法有问题：如果 `SortBuffer` 缓冲区太小，导致从磁盘中读取所有的列不能完全保存在 `SortBuffer` 缓冲区中，这时候单路复用算法就会出现问题，反而性能不如双路复用算法。

**优化策略：**

- 增大 `sort_buffer_size` 参数的设置。

  ```sql
  SHOW VARIABLES LIKE '%sort_buffer_size%';
  ```

  不管用哪种算法，提高这个参数都会提高效率，要根据系统的能力去提高，因为这个参数是针对每个进程 (connection)的 1M-8M之间调整。MySQL5.7, InnoDB存储引擎默认值是 1048576字节，1MB。

- 增大 `max_length_for_sort_data` 参数的设置。

  ```sql
  SHOW VARIABLES LIKE '%max_length_for_sort_data%';  #默认 1024字节
  ```

  提高这个参数，会增加用单路排序算法的概率。但是如果设的太高，数据总容量超出`sort_buffer_size`的概率就增大，明显症状是高的磁盘I/O活动和低的处理器使用率。如果需要返回的列的总长度大于`max」ength_for_sort_data`，使用双路算法，否则使用单路算法。 1024・8192字节之间调整。

- Order by 时select * 是一个大忌。最好只Query需要的字段。
- 当Query的字段大小总和小于`max_length_for_sort_data` ，而且排序字段不是`TEXT|BLOB`类型时，会用改 进后的算法-一单路排序，否则用老算法一一多路排序。
- 两种算法的数据都有可能超出`sort_buffer_size`的容量，超出之后，会创建tmp文件进行合并排序，导致多次IO。但是用单路排序算法的风险会更大一些，所以要提高`sort_buffer_size`。

#### （4）GORUP BY分组优化

- `group by` 使用索引的原则几乎跟`order by`一致 ，`group by` 即使没有过滤条件用到索引，也可以直接使用索引。
- `group by` 先排序再分组，遵照索引建的最佳左前缀法则
- 当无法使用索引列，增大 `max_length_for_sort_data` 和 `sort_buffer_size` 参数的设置
- `where`效率高于`having`，能写在`where`限定的条件就不要写在`having`中
- 减少使用`order by`，和业务沟通能不排序就不排序，或将排序放到程序端去做。`Order by、group by、distinct`这些语句较为耗费CPU，数据库的CPU资源是极其宝贵的。
- 包含了`order by、group by、distinct`这些查询的语句，`where`条件过滤出来的结果集请保持在1000行以内，否则SQL会很慢。

#### （5）排序分组总结

```sql
/* 创建a b c三个字段的索引 */
INDEX a_b_c(a,b,c)
/* 1.ORDER BY 能使用索引最左前缀 */
ORDER BY a;
ORDER BY a, b;
ORDER BY a, b, c;
ORDER BY a DESC, b DESC, c DESC;
/* 2.如果WHERE子句中使用索引的最左前缀定义为常量，则ORDER BY能使用索引 */
WHERE a = 'Ringo' ORDER BY b, c;
WHERE a = 'Ringo' AND b = 'Tangs' ORDER BY c;
WHERE a = 'Ringo' AND b > 2000 ORDER BY b, c;
/* 3.不能使用索引进行排序 */
ORDER BY a ASC, b DESC, c DESC; /* 排序不一致 */
WHERE g = const ORDER BY b, c;   /* 丢失a字段索引 */
WHERE a = const ORDER BY c;     /* 丢失b字段索引 */
WHERE a = const ORDER BY a, d;   /* d字段不是索引的一部分 */
WHERE a IN (...) ORDER BY b, c; /* 对于排序来说，多个相等条件(a=1 or a=2)也是范围查询*/
```

### 10、分页查询优化

#### （1）没有查询条件，没有排序

一般分页查询时，通过创建覆盖索引能够比较好地提高性能。一个常见又非常头疼的问题就是limit 200000,10 ,此时需要MySQL排序前200010记录，仅仅返回200000到200010的10条记录，典他记录丢弃，查询排序的代价非常大。

```sql
EXPLAIN SELECT * FROM student LIMIT 200000,10;
```

![image-20220722074902931](http://cdn.bluecusliyou.com/202207220749042.png)

```sql
SELECT * FROM student LIMIT 200000,10;
```

![image-20220722074633097](http://cdn.bluecusliyou.com/202207220746241.png)

#### （2）优化：在索引上完成排序分页操作

最后根据主键关联回原表查询所需要的其他列内容。如果排序的字段不是主键，给排序字段加索引。

**问题1：如果不带排序条件，MySQL默认是什么排序？**

通常认为是主键，但通过查资料发现并不一定，这里有个物理顺序和逻辑顺序的区别，如：删除原有数据后再插入复用旧id的数据，可能会由于存放在不同页上造成物理顺序与逻辑顺序不一致，此时可以通过优化表改善：optimize table table_name。

**问题2：排序字段有索引就一定快吗？**

1w的时候速度较快，换成查询100w之后的数据呢（深分页）？通过执行计划发现，并没有走索引，为什么没有走索引？因为mysql优化器发现这条sql查询行数超过一定比例（据说是30%，但测试下来并不完全是）就会自动转换为全表扫描，能不能强制走索引呢？可以的，加force index(idx)。但是强制索引效果也不一定快了。

```sql
EXPLAIN SELECT * FROM student ORDER BY id LIMIT 200000,10;
```

![image-20220722074932626](http://cdn.bluecusliyou.com/202207220749732.png)

```sql
SELECT * FROM student ORDER BY id LIMIT 200000,10;
```

![image-20220722075308014](http://cdn.bluecusliyou.com/202207220753129.png)

#### （3）优化：连表子查询

直接通过索引树就能拿到查询字段的值，所以快的原因是子查询方式减少了回表查询操作，进而减少了大量数据的回表IO，因此更高效。

```sql
EXPLAIN SELECT * FROM student t,(SELECT id FROM student ORDER BY id LIMIT 200000,10) a 
WHERE t.id = a.id;
```

![image-20220722080020852](http://cdn.bluecusliyou.com/202207220800989.png)

```sql
SELECT * FROM student t,(SELECT id FROM student ORDER BY id LIMIT 200000,10) a WHERE t.id = a.id;
```

![image-20220722080042690](http://cdn.bluecusliyou.com/202207220800808.png)

#### （4）优化：该方案适用于主键自增的表，可以转换成条件查询 

```sql
EXPLAIN SELECT * FROM student WHERE id > 200000 LIMIT 10;
```

![image-20220722080436140](http://cdn.bluecusliyou.com/202207220804257.png)

```sql
SELECT * FROM student WHERE id > 200000 LIMIT 10;
```

![image-20220722080459139](http://cdn.bluecusliyou.com/202207220804272.png)

#### （5）分页优化总结

- 可以参考谷歌/百度搜索分页，每次只能跳转到当前页前后10页，也就是最多可以跳10页，要想达到深分页情况需要耐心。
- 对没有排序条件的分页查询增加主键排序
- 尽量对排序字段加索引
- 无论是否有索引，当分页页数达到一定阈值强制使用双路排序方式（通过子查询或代码发起两次查询）
- 适当调高sort_buffer_size大小
- 联合索引情况，避免跨列使用

### 11、count(*)查询优化

#### （1）执行计划分析

```sql
explain select count(*) from student;
explain select count(1) from student;
explain select count(id) from student;
explain select count(name) from student;

mysql> select count(*) from student;
+----------+
| count(*) |
+----------+
|  1000000 |
+----------+
1 row in set (0.09 sec)

mysql> select count(1) from student;
+----------+
| count(1) |
+----------+
|  1000000 |
+----------+
1 row in set (0.11 sec)

mysql> select count(id) from student;
+-----------+
| count(id) |
+-----------+
|   1000000 |
+-----------+
1 row in set (0.10 sec)

mysql> select count(name) from student;
+-------------+
| count(name) |
+-------------+
|     1000000 |
+-------------+
1 row in set (0.25 sec)
```

![image-20220807191200165](http://cdn.bluecusliyou.com/202208071912287.png)

id是主键，name是辅助索引中的字段，四个sql的执行计划一样，说明这四个sql执行效率应该差不多。

**最终耗费的时间大约应该是：count(1)>count(name) ≈ count(\*) > count(id) , 在mysql5.7及后续版本中，这4个count耗费的时间已经很相近了。**

#### （2）优化查询速度

- myisam存储引擎的表做不带where条件的count查询性能是很高的，因为myisam存储引擎的表的总行数会被 mysql存储在磁盘上，查询不需要计算。
- 可以增加一张计数表，增删的时候更新行数。

## 四、MySQL配置优化

### 1、配置详解

```ini
[client]
port = 3306
socket = /tmp/mysql.sock
 
[mysqld]
###############################基础设置#####################################
#Mysql服务的唯一编号 每个mysql服务Id需唯一
server-id = 1
 
#服务端口号 默认3306
port = 3306
 
#mysql安装根目录
basedir = /opt/mysql
 
#mysql数据文件所在位置
datadir = /opt/mysql/data
 
#临时目录 比如load data infile会用到
tmpdir  = /tmp
 
#设置socke文件所在目录
socket  = /tmp/mysql.sock
 
#主要用于MyISAM存储引擎,如果多台服务器连接一个数据库则建议注释下面内容
skip-external-locking
 
#只能用IP地址检查客户端的登录，不用主机名
skip_name_resolve = 1
#数据库默认字符集,主流字符集支持一些特殊表情符号（特殊表情符占用4个字节）
character-set-server = utf8mb4
 
#数据库字符集对应一些排序等规则，注意要和character-set-server对应
collation-server = utf8mb4_general_ci
 
#设置client连接mysql时的字符集,防止乱码
init_connect='SET NAMES utf8mb4'
 
#是否对sql语句大小写敏感，1表示不敏感
lower_case_table_names = 1
 
#最大连接数
max_connections = 400
#最大错误连接数
max_connect_errors = 1000
 
#TIMESTAMP如果没有显示声明NOT NULL，允许NULL值
explicit_defaults_for_timestamp = true
 
#SQL数据包发送的大小，如果有BLOB对象建议修改成1G
max_allowed_packet = 128M
 
#MySQL连接闲置超过一定时间后(单位：秒)将会被强行关闭
#MySQL默认的wait_timeout  值为8个小时, interactive_timeout参数需要同时配置才能生效
interactive_timeout = 1800
wait_timeout = 1800
 
#内部内存临时表的最大值 ，设置成128M。
#比如大数据量的group by ,order by时可能用到临时表，
#超过了这个值将写入磁盘，系统IO压力增大
tmp_table_size = 134217728
max_heap_table_size = 134217728


 
##----------------------------用户进程分配到的内存设置BEGIN-----------------------------##
##每个session将会分配参数设置的内存大小
#用于表的顺序扫描，读出的数据暂存于read_buffer_size中，当buff满时或读完，将数据返回上层调用者
#一般在128kb ~ 256kb,用于MyISAM
#read_buffer_size = 131072
#用于表的随机读取，当按照一个非索引字段排序读取时会用到，
#一般在128kb ~ 256kb,用于MyISAM
#read_rnd_buffer_size = 262144
#order by或group by时用到
#建议先调整为2M，后期观察调整
sort_buffer_size = 2097152
#一般数据库中没什么大的事务，设成1~2M，默认32kb
binlog_cache_size = 524288
 
############################日  志 设置##########################################
#数据库错误日志文件
log_error = error.log
 
#慢查询sql日志设置
slow_query_log = 1
slow_query_log_file = slow.log
#检查未使用到索引的sql
log_queries_not_using_indexes = 1
#针对log_queries_not_using_indexes开启后，记录慢sql的频次、每分钟记录的条数
log_throttle_queries_not_using_indexes = 5
#作为从库时生效,从库复制中如何有慢sql也将被记录
log_slow_slave_statements = 1
#慢查询执行的秒数，必须达到此值可被记录
long_query_time = 2
#检索的行数必须达到此值才可被记为慢查询
min_examined_row_limit = 100
 
#mysql binlog日志文件保存的过期时间，过期后自动删除
expire_logs_days = 5
 
############################主从复制 设置########################################
#开启mysql binlog功能
log-bin=mysql-bin
#binlog记录内容的方式，记录被操作的每一行
binlog_format = ROW

#作为从库时生效,想进行级联复制，则需要此参数
log_slave_updates
 
#作为从库时生效,中继日志relay-log可以自我修复
relay_log_recovery = 1
 
#作为从库时生效,主从复制时忽略的错误
slave_skip_errors = ddl_exist_errors
 
##---redo log和binlog的关系设置BEGIN---##
#(步骤1) prepare dml相关的SQL操作，然后将redo log buff中的缓存持久化到磁盘
#(步骤2)如果前面prepare成功，那么再继续将事务日志持久化到binlog
#(步骤3)如果前面成功，那么在redo log里面写上一个commit记录
#当innodb_flush_log_at_trx_commit和sync_binlog都为1时是最安全的，
#在mysqld服务崩溃或者服务器主机crash的情况下，binary log只有可能丢失最多一个语句或者一个事务。
#但是都设置为1时会导致频繁的io操作，因此该模式也是最慢的一种方式。
#当innodb_flush_log_at_trx_commit设置为0，mysqld进程的崩溃会导致上一秒钟所有事务数据的丢失。
#当innodb_flush_log_at_trx_commit设置为2，只有在操作系统崩溃或者系统掉电的情况下，上一秒钟所有事务数据才可能丢失。
#commit事务时,控制redo log buff持久化磁盘的模式 默认为1
innodb_flush_log_at_trx_commit = 2
#commit事务时,控制写入mysql binlog日志的模式 默认为 0
#innodb_flush_log_at_trx_commit和sync_binlog都为1时，mysql最为安全但性能上压力也是最大
sync_binlog = 1
##---redo log 和 binlog的关系设置END---##
 
############################Innodb设置##########################################
#数据块的单位8k，默认是16k，16kCPU压力稍小，8k对select的吞吐量大
#innodb_page_size的参数值也影响最大索引长度，8k比16k的最大索引长度小
#innodb_page_size = 8192
#一般设置物理存储的60% ~ 70%
innodb_buffer_pool_size = 1G
 
#5.7.6之后默认16M
#innodb_log_buffer_size = 16777216
#该参数针对unix、linux，window上直接注释该参数.默认值为NULL
#O_DIRECT减少操作系统级别VFS的缓存和Innodb本身的buffer缓存之间的冲突
innodb_flush_method = O_DIRECT
 
#此格式支持压缩, 5.7.7之后为默认值
innodb_file_format = Barracuda
 
#CPU多核处理能力设置，假设CPU是2颗4核的，设置如下
#读多，写少可以设成2:6的比例
innodb_write_io_threads = 4
innodb_read_io_threads = 4
 
#提高刷新脏页数量和合并插入数量，改善磁盘I/O处理能力
#默认值200（单位：页）
#可根据磁盘近期的IOPS确定该值
innodb_io_capacity = 500
 
#为了获取被锁定的资源最大等待时间，默认50秒，超过该时间会报如下错误:
# ERROR 1205 (HY000): Lock wait timeout exceeded; try restarting transaction
innodb_lock_wait_timeout = 30
 
#调整buffer pool中最近使用的页读取并dump的百分比,通过设置该参数可以减少转储的page数
innodb_buffer_pool_dump_pct = 40
 
#设置redoLog文件所在目录, redoLog记录事务具体操作内容
innodb_log_group_home_dir = /opt/mysql/redolog/
 
#设置undoLog文件所在目录, undoLog用于事务回滚操作
innodb_undo_directory = /opt/mysql/undolog/
 
#在innodb_log_group_home_dir中的redoLog文件数, redoLog文件内容是循环覆盖写入。
innodb_log_files_in_group = 3
 
#MySql5.7官方建议尽量设置的大些，可以接近innodb_buffer_pool_size的大小
#之前设置该值较大时可能导致mysql宕机恢复时间过长，现在恢复已经加快很多了
#该值减少脏数据刷新到磁盘的频次
#最大值innodb_log_file_size * innodb_log_files_in_group <= 512GB,单文件<=256GB
innodb_log_file_size = 1024M
 
#设置undoLog文件所占空间可以回收
#5.7之前的MySql的undoLog文件一直增大无法回收
innodb_undo_log_truncate = 1
innodb_undo_tablespaces = 3
innodb_undo_logs = 128
 
#5.7.7默认开启该参数 控制单列索引长度最大达到3072
#innodb_large_prefix = 1
 
#5.7.8默认为4个, Inodb后台清理工作的线程数
#innodb_purge_threads = 4
 
#通过设置配置参数innodb_thread_concurrency来限制并发线程的数量，
#一旦执行线程的数量达到这个限制，额外的线程在被放置到对队列中之前，会睡眠数微秒，
#可以通过设定参数innodb_thread_sleep_delay来配置睡眠时间
#该值默认为0,在官方doc上，对于innodb_thread_concurrency的使用，也给出了一些建议:
#(1)如果一个工作负载中，并发用户线程的数量小于64，建议设置innodb_thread_concurrency=0；
#(2)如果工作负载一直较为严重甚至偶尔达到顶峰，建议先设置innodb_thread_concurrency=128,
###并通过不断的降低这个参数，96, 80, 64等等，直到发现能够提供最佳性能的线程数
#innodb_thread_concurrency = 0
############################其他内容 设置##########################################
[mysqldump]
quick
max_allowed_packet = 128M
[mysql]
no-auto-rehash
[myisamchk]
key_buffer_size = 20M
sort_buffer_size = 256k
read_buffer = 2M
write_buffer = 2M
[mysqlhotcopy]
interactive-timeout
[mysqld_safe]
#增加每个进程的可打开文件数量.
open-files-limit = 28192
```

### 2、配置优化

#### （1）innodb_buffer_pool_size

这是你安装完InnoDB数据库后第一个应该设置的选项。缓冲池是数据和索引缓存的地方：这个值越大越好，这能保证你在大多数的读取操作时使用的是内存而不是硬盘。理论上可以设置为**内存的四分之三**，典型的值是5-6GB(8GB内存)，20-25GB(32GB内存)，100-120GB(128GB内存)。

 Innodb_buffer_pool_pages_free这个值如果是0，表示用光了设置的buffer_pool_size

```sql
mysql> show global status like 'innodb_buffer_pool_pages_%';
+----------------------------------+-------+
| Variable_name                    | Value |
+----------------------------------+-------+
| Innodb_buffer_pool_pages_data    | 7156  |
| Innodb_buffer_pool_pages_dirty   | 0     |
| Innodb_buffer_pool_pages_flushed | 10778 |
| Innodb_buffer_pool_pages_free    | 1017  |
| Innodb_buffer_pool_pages_misc    | 19    |
| Innodb_buffer_pool_pages_total   | 8192  |
+----------------------------------+-------+
```

#### （2）innodb_log_file_size

这是redo日志的大小。redo日志被用于确保写操作快速而可靠并且在崩溃时恢复。一直到MySQL 5.1，它都难于调整，因为一方面你想让它更大来提高性能，另一方面你想让它更小来使得崩溃后更快恢复。幸运的是从MySQL 5.5之后，崩溃恢复的性能的到了很大提升，这样你就可以同时拥有较高的写入性能和崩溃恢复性能了。一直到MySQL 5.5，redo日志的总尺寸被限定在4GB(默认可以有2个log文件)。这在MySQL 5.6里被提高。

**一般** **innodb_log_file_size** **设置成** **innodb_buffer_pool_size \* 0.25**

#### （3）max_connections

如果你经常看到‘Too many connections’错误，是因为max_connections的值太低了。这非常常见因为应用程序没有正确的关闭数据库连接，你需要比默认的151连接数更大的值。max_connection值被设高了(例如1000或更高)之后一个主要缺陷是当服务器运行1000个或更高的活动事务时会变的没有响应。在应用程序里使用连接池或者在MySQL里使用进程池有助于解决这一问题。

**对于innodb_buffer_pool_size 和 innodb_log_file_size 有时候以G为单位，设置成小数，不成功，比如2.5G，mysql可能无法启动，这个时候可以用M为单位，比如：2500M。**

## 五、表分区

### 1、什么是数据库分区

mysql数据库中的数据是以文件的形势存在磁盘上的，默认放在/mysql/data下面 （可以通过my.cnf中的datadir来查看），一张表主要对应着三个文件，一个是frm存放表结构的，一个是myd存放表数据的，一个是myi存表索引的。

如果一张表的数据量太大的话，那么myd、myi就会变的很大，查找数据就会变的很慢，这个时候我们可以利用mysql的分区功能，在物理上将这 一张表对应的三个文件，分割成许多个小块，我们查找一条数据时，就不用全部查找了，只要知道这条数据在哪一块，然后在那一块找就行了。

如果表的数据太大，可能一个磁盘放不下，这个时候，我们可以把数据分配到不同的磁盘里面去。

### 2、数据的两种分割方式

#### （1）横向分区（数据库的功能，可以建表的时候设置，可以细分多种类型）

就是横着来分区了，举例来说明一下，假如有100W条数据，分成十份，前10W条数据放到第一个分区，第二个10W条数据放到第二个分区，依此类推，也就是把表分成了十分。取出一条数据的时候，这条数据包含了表结构中的所有字段，也就是说横向分区，并没有改变表的结构。

#### （2）纵向分区（表的设计，不是数据库的功能）

什就是竖着来分区了，举例来说明，在设计用户表的时候，开始的时候没有考虑好，而把个人的所有信息都放到了一张表里面去，这样这个表里面就会有比较大的字段，如个人简介，而这些简介呢，也许不会有好多人去看，所以等到有人要看的时候，在去查找，分表的时候，可以把这样的大字段，分开来。

### 3、表分区-range

按照range分区，每个分区包含那些分区表达式的值位于一个给定的连续区间内的行。

```sql
-- 创建表和分区
create table if not exists `range_part` (
   `id` int not null auto_increment comment '用户id',
	 `name` varchar(50) not null default '' comment '名称',
	 `sex` int not null default '0' comment '0为男，1为女',
	 primary key (`id`)
	 )engine=innodb default charset=utf8 auto_increment=1
partition by range (id) (
   partition p0 values less than (3),
	 partition p1 values less than (6),
	 partition p2 values less than (9),
	 partition p3 values less than (12),
	 partition p4 values less than maxvalue
);

-- 插入表数据
insert into `range_part` (`name` ,`sex`) values
('tank', '0') ,('zhang',1),('ying',1),('张',1),
('映',0),('test1',1),('tank2',1),('tank1',1),
('test2',1),('test3',1),('test4',1),('test5',1),
('tank3',1),('tank4',1),('tank5',1),('tank6',1),
('tank7',1),('tank8',1),('tank9',1),('tank10',1),
('tank11',1),('tank12',1),('tank13',1),('tank21',1),('tank42',1);
```

分区的文件如下：

![image-20220723213819152](http://cdn.bluecusliyou.com/202207232138326.png)

```sql
-- 查看表分区信息
select * from information_schema.partitions where table_schema='test' and
table_name='range_part';
-- 查看表数据行数
mysql> select count(id) as count from range_part;
+-------+
| count |
+-------+
|    25 |
+-------+
## 删除第四个分区
alter table range_part drop partition p4;
## 查看表分区信息
select * from information_schema.partitions where table_schema='test' and
table_name='range_part';
## 查看数据【注意：存放在分区里面的数据丢失了，p4分区里面有14条数据，其他分区只有11条数据。】
mysql> select count(id) as count from range_part;
+-------+
| count |
+-------+
|    11 |
+-------+
## 可以对现有表进行分区,并且会按規则自动的将表中的数据分配相应的分区中
alter table range_part partition by range(id) (
   partition p1 values less than (1),
   partition p2 values less than (5),
   partition p3 values less than maxvalue
);
-- 查看表数据行数 没有变更
mysql> select count(id) as count from range_part;
+-------+
| count |
+-------+
|    11 |
+-------+

-- 查看具体分区的数据
mysql> select *  from range_part partition(p2);
+----+-------+-----+
| id | name  | sex |
+----+-------+-----+
|  1 | tank  |   0 |
|  2 | zhang |   1 |
|  3 | ying  |   1 |
|  4 | 张    |   1 |
+----+-------+-----+
```

新的分区文件如下：

![image-20220724160515208](http://cdn.bluecusliyou.com/202207241605456.png)

### 4、表分区-list

创建list分区时，如果有主銉的话，分区时主键必须在其中，不然就会报错。如果没有主键，分区就能创建成功，一般情况下，一张表肯定要有一个主键，这算是一个list分区的局限性。

```sql
create table if not exists `list_part` (
     `id` int(11) not null comment '用户id',
	 `province_id` int(2) not null default 0 comment '省',
	 `name` varchar(50) not null default '' comment '名称',
	 `sex` int(1) not null default '0' comment '0为男，1为女'
) engine=innodb default charset=utf8
partition by list (province_id) (
         partition p0 values in (1,2,3),
		 partition p1 values in (4,5,6),
		 partition p2 values in (7,8,9),
		 partition p3 values in (10,11)
);
```

分区文件如下：

![image-20220724161013422](http://cdn.bluecusliyou.com/202207241610647.png)

### 5、表分区-hash

hash分区主要用来确保数据在预先确定数目的分区中平均分布，你所要做的只是基于将要被哈希的列值指定一个列值或表达式，以及指定被分区的表将要被分割成的分区数量。

```sql
## 创建分区
create table if not exists `hash_part`(
   `id` int not null auto_increment comment '评论id', 
	 `comment` varchar(1000) not null default '' comment '评论',
	 `ip` varchar(25) not null default '' comment '来源ip',
	 primary key (`id`)
) engine=innodb default charset=utf8 auto_increment=1
partition by hash(id)
partitions 3;
```

### 6、表分区-key

KEY分区和HASH分区相似，但是KEY分区支持除text和BLOB之外的所有数据类型的分区，而HASH分区只支持数字分区，KEY分区不允许使用用户自定义的表达式进行分区，KEY分区使用系统提供的HASH函数进行分区。当表中存在主键或者唯一键时，如果创建key分区时没有指定字段系统默认会首选主键列作为分区字列,如果不存在主键列会选择非空唯一键列作为分区列,注意唯一列作为分区列唯一列不能为null。

```sql
CREATE TABLE key_part (
   id INT ,
   var CHAR(32) 
)
PARTITION BY KEY(var)
PARTITIONS 10;
```

### 7、表分区-linear key

同样key分区也存在线性KEY分区，概念和线性HASH分区一样。

```sql
CREATE TABLE keyline_part (
   id INT NOT NULL,
	 var CHAR(5))
PARTITION BY LINEAR KEY (var)
PARTITIONS 3;
```

### 8、分区管理

```sql
-- 删除两个分区
ALTER TABLE tb_key COALESCE PARTITION 2;
-- 增加三个分区
ALTER TABLE tb_key add PARTITION partitions 3;
-- 移除分区，使用remove移除分区是仅仅移除分区的定义，并不会删除数据和drop PARTITION不一样，后者会连同数据一起删除
ALTER TABLE tablename REMOVE PARTITIONING;
```

### 9、分区-range多字段

多字段的分区键比较是基于数组的比较。它先用插入的数据的第一个字段值和分区的第一个值进行比较，如果插入的第一个值小于分区的第一个值那么就不需要比较第二个值就属于该分区；如果第一个值等于分区的第一个值，开始比较第二个值同样如果第二个值小于分区的第二个值那么就属于该分区。多列分区第一列的分区值一定是顺序增长的，不能出现交叉值，第二列的值随便，否则报错。

```sql
CREATE TABLE range_part_cs (
   a INT,
	 b INT
)
PARTITION BY RANGE COLUMNS(a,b) (
     PARTITION p0 VALUES LESS THAN (5,10),
		 PARTITION p1 VALUES LESS THAN (10,20),
		 PARTITION p2 VALUES LESS THAN (15,30),
		 PARTITION p3 VALUES LESS THAN (MAXVALUE,MAXVALUE)
);

-- 插入数据
insert into range_part_cs(a,b)values(1,20),(10,15),(10,30);
```

第一组值：(1,20);1<5所以不需要再比较20了，该记录属于p0分区。

第二组值:(10,15)，10>5,10=10且15<20，所以该记录属于P1分区

第三组值:(10,30),10=10但是30>20，所以它不属于p1,它满足10<15所以它属于p2

### 10、分区-list多字段

由于分区是组合字段，filtered只有50%，对于组合分区索引也最好是建组合索引，其实如果能通过id字段刷选出数据，单独建id字段的索引也是有效果的，但是组合索引的效果是最好的，其实和非分区键索引的概念差不多。

```sql
CREATE TABLE list_part_cs (
   id INT NOT NULL, 
	 hired DATETIME NOT NULL
)
PARTITION BY LIST COLUMNS(id,hired) 
(
   PARTITION a VALUES IN ( (1,'1990-01-01 10:00:00'),(1,'1991-01-01 10:00:00') 
),
   PARTITION b VALUES IN ( (2,'1992-01-01 10:00:00') ),
	 PARTITION c VALUES IN ( (3,'1993-01-01 10:00:00') ),
	 PARTITION d VALUES IN ( (4,'1994-01-01 10:00:00') )
);
```

