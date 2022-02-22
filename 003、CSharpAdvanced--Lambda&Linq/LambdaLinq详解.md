# LambdaLinq详解

## 一、Lambda

### 1、Lambda是什么

（1）形如：()=> { } 就是lambda表达式

（2）lambda表达式就是一个匿名方法，在底层会生成在一个"<>"类中，生成带有名称的方法

### 2、Lambda的演变过程

#### （1）.Netframework1.0/1.1，原始方法

```C#
/// <summary>
/// 声明委托
/// </summary>
/// <param name="x"></param>
/// <param name="y"></param>
public delegate void NoReturnWithPara(int x, string y);

/// <summary>
/// 声明方法
/// </summary>
/// <param name="x"></param>
/// <param name="y"></param>
private void PrintParam(int x, string y)
{
    Console.WriteLine(x);
    Console.WriteLine(y);
}
```

```C#
NoReturnWithPara method = new NoReturnWithPara(PrintParam);
```

#### （2）.NetFramework2.0，匿名方法

增加了一个delegate关键字,可以访问到除了参数以外的局部变量

```C#
int i = 0;
NoReturnWithPara method = new NoReturnWithPara(delegate (int x, string y)
{
    Console.WriteLine(x);
    Console.WriteLine(y);
    Console.WriteLine(i);
});
```

#### （3）.NetFramework3.0，=>

去掉delegate关键字，在参数的后增加了一个=>  goes to

```C#
int i = 0;
NoReturnWithPara method = new NoReturnWithPara((int x, string y) =>
{
    Console.WriteLine(x);
    Console.WriteLine(y);
    Console.WriteLine(i);
});
```

#### （4）.NetFramework3.0后期，简化参数类型

去掉了匿名方法中的参数类型，这个是编译器提供的语法糖，编译器可以根据委托类型定义的参数类型推导出参数类型

```C#
int i = 0;
NoReturnWithPara method = new NoReturnWithPara((x, y) =>
{
    Console.WriteLine(x);
    Console.WriteLine(y);
    Console.WriteLine(i);
});
```

#### （5）如果方法体中只有一行代码，可以省略方法体大括号

```C#
NoReturnWithPara method = (x, y) => Console.WriteLine(x);
```

#### （6）如果方法只有一个参数，省略参数小括号

```C#
Action<string> method = x => Console.WriteLine(x);
```

#### （7）如果方法体中只有一行代码，且有返回值，可以省略return

```C#
Func<int, string> method = i => i.ToString();
```

## 二、匿名类

### 1、匿名类是什么

形如new {}，new一个对象，不需要类名称了，NETFramework3.0出现的

### 2、匿名类+object

object去接匿名类，无法访问属性值，因为C#是强类型语言，object是在编译时确定类型，因为Object没有这个属性

```C#
object model = new  
{
    Id = 1,
    Name = "张三",
    Age = 30,
    ClassId = 2
};
//无法访问属性值
//model.Id = 134;
//Console.WriteLine(model.Id);
```

### 3、匿名类+dynamic

dynamic(动态类型)可以避开编译器检查，.NETFramework 4.0出现的
dynamic去接匿名类，可以访问属性值，因为dynamic是运行时才检查的，但是访问不存在的属性也不报错，运行时才报异常

```C#
dynamic dModel = new
{
    Id = 1,
    Name = "张三",
    Age = 30,
    ClassId = 2
};
//可以访问属性值
dModel.Id = 134;
Console.WriteLine(dModel.Id);
//但是访问不存在的属性也不报错，运行时才报异常
dModel.abccc = 1234;
```

#### 4、匿名类+var

var去接匿名类，可以读取属性，不能给属性重新赋值，只能在初始化的时候给定一个值
var是编译器的语法糖，由编译器自动推算类型
var声明的变量必须初始化，必须能推算出类型，var aa = null;或者var aa;都是不正确的
var缺陷：阅读麻烦，建议能明确类型的还是明确类型，优点：简化代码

```C#
var vmodel = new
{
    Id = 1,
    Name = "张三",
    Age = 30,
    ClassId = 2
};
//不能给属性重新赋值
//vmodel.Id = 134;
//可以读取属性
Console.WriteLine(vmodel.Id);
```

## 三、扩展方法

### 1、为什么使用扩展方法

需求：给一个类增加一个功能

#### （1）方案1：类上直接添加一个方法

需要修改原类，类一旦修改，类就需要重新发布编译，违背了开闭原则，如果要新增一个功能，尽量做到不去修改之前的代码

自己的类要修改还是可以修改的，但是系统框架的类无法去修改的

```C#
Student student = new Student()
{
    Id = 123,
    Name = "张三",
    Age = 25,
    ClassId = 1
};
student.StudyFramework();
```

#### （2）方案2：添加一个外部类方法，把当前类作为参数传入

不需要修改原类，就可以获取到传递过来的这个实体中的各种数据
调用方法需要调用其他类的方法，还要把当前类实例作为参数传进去，还是麻烦

```C#
/// <summary>
/// 方法封装
/// </summary>
/// <param name="student"></param>
public static void StudyFramework1(Student student)
{            
    Console.WriteLine($"{student.Id} {student.Name}方法封装。。。。");
}
```

```C#
Student student = new Student()
{
    Id = 123,
    Name = "张三",
    Age = 25,
    ClassId = 1
};
MethodExtension.StudyFramework1(student);
```

#### （3）方案3：扩展方法

把传入的当前实例参数放在第一个参数，参数前面加this关键字，就可以直接用当前实例调用扩展方法，就像调用实例自己的方法一样
扩展方法三要素：==静态类，静态方法，第一个参数this关键字==

```C#
/// <summary>
/// 扩展方法三要素
/// 静态类，静态方法，this关键字
/// </summary>
public static class MethodExtension2
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    /// <param name="student"></param>
    public static void StudyFramework2(this Student student)
    {
        Console.WriteLine($"{student.Id} {student.Name}扩展方法。。。。");
    }
}
```

### 2、可以为哪些类扩展方法

#### （1）普通类扩展方法

```C#
/// <summary>
/// 普通类扩展方法
/// </summary>
/// <param name="i"></param>
/// <returns></returns>
public static string IntToString(this int i)
{
    return i.ToString();
}
```

```C#
int aa = 0;
aa.IntToString();
```

#### （2）泛型类扩展方法

可以，但是扩展泛型类，会有侵入性，相当于让任何一个类型，都拥有了这个方法，覆盖的访问太广

```C#
/// <summary>
/// 泛型类扩展方法
/// </summary>
public static int GenericeExtend<T>(this T t)
{
    if (t is int)
    {
        return Convert.ToInt32(t);
    }
    else if (t is object)
    {

    }
    return 0;
}
```

```C#
int bb = 3;
bb.GenericeExtend<int>();
string str = "张三";
str.GenericeExtend<string>();
```

#### （3）Object类扩展方法

可以，但是扩展泛型类，会有侵入性，因为任何一个类型都是object的子类，扩展object，就相当于给所有的类型扩展了一个方法，可能会让一些类型，存在了一些不应该存在的行为

```C#
/// <summary>
/// object类扩展方法
/// </summary>
/// <param name="t"></param>
/// <returns></returns>
public static int ObjectToInt(this object t)
{
    if (t is int)
    {
        return Convert.ToInt32(t);
    }
    else if (t is object)
    {

    }
    return 0;
}
```

```C#
int cc = 3;
cc.ObjectToInt();
```

#### （4）扩展方法调用优先级

如果增加了扩展方法，同时也在类的内部增加了一个同样的方法，在调用的时候，会优先调用类内部的方法

### 3、扩展方法应用场景                    

#### （1）扩展第三方的类库

第三方类库通过dll方式引入进来的，我们是不能直接取修改代码的，可以通过扩展方法，给第三方的类库中的某个类型增加功能呢，扩展功能

#### （2）原有功能的扩展

在系统做维护的的时候，需要做到不修改之前的代码，想要增加功能的时候

## 四、Linq

### 1、Linq是什么

（1）Linq（Language Integrated Query）即语言集成查询。
（2）Linq是一组语言特性和API，使得你可以使用统一的方式编写各种查询。用于保存和检索来自不同数据源的数据，从而消除了编程语言和数据库之间的不匹配，以及为不同类型的数据源提供单个查询接口。
（3）Linq总是使用对象，因此你可以使用相同的查询语法来查询和转换XML、对象集合、SQL数据库、ADO.NET数据集以及任何其他可用的LINQ提供程序格式的数据。
（4）Linq主要包含以下部分

- Linq to Objects 主要负责对象的查询。
- Linq to XML 主要负责XML的查询。
- Linq to ADO.NET 主要负责数据库的查询。
- Linq to SQL
- Linq to DataSet
- Linq to Entities
- Linq to Everything

### 2、Linq的原理

 需求：存在一个集合，要过滤其中的数据

#### （1）方案1：循环 + 判断

```C#
//要求查询Student中年龄小于30的； 
List<Student> studentList = this.GetStudentList();
List<Student> list = new List<Student>();
foreach (var item in studentList)
{
    if (item.Age < 30)
    {
        list.Add(item);
    }
}
//要求Student名称长度大于2
List<Student> list2 = new List<Student>();
foreach (var item in studentList)
{
    if (item.Name.Length > 2)
    {
        list2.Add(item);
    }
}
//N个条件叠加
List<Student> list3 = new List<Student>();
foreach (var item in studentList)
{
    if (item.Id > 1
        && item.Name != null
        && item.ClassId == 1
        && item.Age > 20)
    {
        list.Add(item);
    }
}
```

#### （2）方案2：扩展方法 + Lambda

可以把不变的业务逻辑保留，把可变的，不固定的业务逻辑转移出去，就可以用委托包装一个方法传递过来，简化重复代码

```C#
/// <summary>
/// 泛型扩展方法
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="oldlist"></param>
/// <param name="func"></param>
/// <returns></returns>
public static List<T> CustomWhere<T>(this List<T> oldlist, Func<T, bool> func)
{
    List<T> newlist = new List<T>();
    foreach (var item in oldlist)
    {        
        if (func.Invoke(item))
        {
            newlist.Add(item);
        }
    }
    return newlist;
}
```

```C#
//要求查询Student中年龄小于30的； 
List<Student> studentList = this.GetStudentList();
List<Student> list = studentList.CustomWhere(item => item.Age < 30);
//要求Student名称长度大于2
List<Student> list2 = studentList.CustomWhere(item => item.Name.Length > 2);
//N个条件叠加
List<Student> list3 = studentList.CustomWhere(item => item.Id > 1
        && item.Name != null
        && item.ClassId == 1
        && item.Age > 20);
```

#### （3）方案3：Linq中的Where

- Linq实现原理和我们自己写的扩展方法类似
- Linq的底层都是通过迭代器来实现就是支持循环
- Linq的底层使用IEnumerable来承接数据

```C#
//来自于Linq的Where实现
public static IEnumerable<TSource> Where<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
{
    if (source == null)
    {
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source);
    }
    if (predicate == null)
    {
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.predicate);
    }
    Iterator<TSource> iterator = source as Iterator<TSource>;
    if (iterator != null)
    {
        return iterator.Where(predicate);
    }
    TSource[] array = source as TSource[];
    if (array != null)
    {
        if (array.Length != 0)
        {
            return new WhereArrayIterator<TSource>(array, predicate);
        }
        return Empty<TSource>();
    }
    List<TSource> list = source as List<TSource>;
    if (list != null)
    {
        return new WhereListIterator<TSource>(list, predicate);
    }
    return new WhereEnumerableIterator<TSource>(source, predicate);
}
```

```C#
//要求查询Student中年龄小于30的； 
List<Student> studentList = this.GetStudentList();
List<Student> list = studentList.Where(item => item.Age < 30).ToList();
//要求Student名称长度大于2
List<Student> list2 = studentList.Where(item => item.Name.Length > 2).ToList();
//N个条件叠加
List<Student> list3 = studentList.Where(item => item.Id > 1
        && item.Name != null
        && item.ClassId == 1
        && item.Age > 20).ToList();
```

### 3、Linq的优势

#### （1）减少编码

相比较传统的方式，LINQ减少了要编写的代码量。

#### （2）可读性强

LINQ增加了代码的可读性，开发人员可以很轻松地理解和维护。

#### （3）标准化的查询方式

可以使用相同的LINQ语法查询多个数据源。

#### （4）智能感知提示

LINQ为通用集合提供智能感知提示。

## 五、Linq语句使用

### 1、Linq写查询的两种形式

#### （1）查询语法

 使用标准的方法调用，这些方法是一组叫做标准查询运算符的方法

#### （2）方法语法

看上去和SQL语句很相似，使用查询表达式形式书写。微软推荐使用查询语法，因为它更易读

在编译时，CLR会将查询语法转换为方法语法

```C#
int[] num = { 2, 4, 6, 8, 10 };
var numQuery = from number in num //查询语法
               where number < 8
               select number;
var numMethod = num.Where(x => x < 8); //方法语法
```

### 2、Linq的延迟计算

一般步骤：获取数据源、创建查询、执行查询。

需要注意的是，尽管查询在语句中定义，但直到最后的foreach语句请求其结果的时候才会执行。

```C#
int[] number = { 2, 4, 6, 8, 10 }; //获取数据源
IEnumerable<int> lowNum = from n in number //创建并存储查询，不会执行操作
                          where n < 8
                          select n;
foreach (var val in lowNum) //执行查询
{
    Console.Write("{0} ", val);
}
```

### 3、Linq方法语法详解

查询表达式由查询体后的from子句组成，其子句必须按一定的顺序出现，并且from子句和select子句这两部分是必须的。

#### （1）from

from子句指定了要作为数据源使用的数据集合

#### （2）Select

投影：可以做一些自由组装new一个匿名类，也可以new具体类
select子句指定所选定的对象哪部分应该被选择。可以指定下面的任意一项a.整个数据项 b.数据项的一个字段c.数据项中几个字段组成的新对象

```C#
Console.WriteLine("*************Select****************");
{
    //a.整个数据项
    var lista = studentList.Where<Student>(s => s.Age < 30)
                         .Select(s => s);
    //b.数据项的一个字段
    var listb = studentList.Where<Student>(s => s.Age < 30)
                         .Select(s => s.Age);
    //c.数据项中几个字段组成的新对象
    var listc = studentList.Where<Student>(s => s.Age < 30)
                         .Select(s => new
                         {
                             Age = s.Age,
                             ClassId = s.ClassId
                         });

    var list = studentList.Where<Student>(s => s.Age < 30)
                         .Select(s => new
                         {
                             IdName = s.Id + s.Name,
                             ClassName = s.ClassId == 2 ? "数学" : "英语"
                         });
    foreach (var item in list)
    {
        Console.WriteLine("Name={0}  Age={1}", item.ClassName, item.IdName);
    }
}
Console.WriteLine("*************Select****************");
{
    //a.整个数据项
    var lista = from s in studentList
                select s;
    //b.数据项的一个字段
    var listb = from s in studentList
                select s.Age;
    //c.数据项中几个字段组成的新对象
    var listc = from s in studentList
                select new
                {
                   Age=s.Age,
                   ClassId=s.ClassId
                };

    var list = from s in studentList
               where s.Age < 30
               select new
               {
                   IdName = s.Id + s.Name,
                   ClassName = s.ClassId == 2 ? "数学" : "英语"
               };

    foreach (var item in list)
    {
        Console.WriteLine("Name={0}  Age={1}", item.ClassName, item.IdName);
    }
}
Console.WriteLine("*************Select****************");
```

#### （3）Where

条件筛选，where子句根据之后的运算来除去不符合要求的项，一个查询表达式可以有任意多个where子句，一个项必须满足所有的where条件才能避免被过滤

```C#
Console.WriteLine("*************Where****************");
{
    IEnumerable<Student> list = studentList.Where<Student>(s => s.Age < 30 && s.ClassId == 2);
    foreach (var item in list)
    {
        Console.WriteLine("Name={0}  Age={1}", item.Name, item.Age);
    }
}
Console.WriteLine("*************Where****************");
{
    IEnumerable<Student> list = from s in studentList
                                where s.Age < 30
                                where s.ClassId == 2
                                select s;
    foreach (var item in list)
    {
        Console.WriteLine("Name={0}  Age={1}", item.Name, item.Age);
    }
}
Console.WriteLine("*************Where****************");
```

#### （4）OrderBy

OrderBy排序，ThenBy再排序，OrderByDescending倒序排序

```C#
Console.WriteLine("*************OrderBy，ThenBy，OrderByDescending****************");
{
    var list = studentList.Where<Student>(s => s.Age < 30)
                           .Select(s => new
                           {
                               Id = s.Id,
                               ClassId = s.ClassId,
                               IdName = s.Id + s.Name,
                               ClassName = s.ClassId == 2 ? "数学" : "英语"
                           })
                           .OrderBy(s => s.Id)//排序 升序
                           .ThenBy(s => s.ClassName) //多重排序，可以多个字段排序都生效
                           .OrderByDescending(s => s.ClassId)//倒排
                           ;
    foreach (var item in list)
    {
        Console.WriteLine($"Id={item.Id}  ClassName={item.ClassName}  ClassId={item.ClassId}");
    }
}
Console.WriteLine("*************OrderBy，ThenBy，OrderByDescending****************");
{
    var list = from s in studentList
               where s.Age < 30
               orderby s.Id, s.ClassId
               orderby s.ClassId descending
               select new
               {
                   Id = s.Id,
                   ClassId = s.ClassId,
                   IdName = s.Id + s.Name,
                   ClassName = s.ClassId == 2 ? "数学" : "英语"
               };
    foreach (var item in list)
    {
        Console.WriteLine($"Id={item.Id}  ClassName={item.ClassName}  ClassId={item.ClassId}");
    }
}
Console.WriteLine("*************OrderBy，ThenBy，OrderByDescending****************");
```

#### （5）into

查询延续：查询延续子句可以接受查询的一部分结构并赋予一个名字，从而可以在查询的另一部分中使用

#### （6）GroupBy

分组，和into一起使用，分组数据可以Max，Min，Average，Sum，Count
这里，Key其实质是一个类的对象
group by 可以一个表达式，返回按照表达式区分的两个组

```C#
Console.WriteLine("*************GroupBy****************");
{
    var list = studentList.GroupBy(s => s.ClassId)
                          .Select(sg => new
                          {
                              key = sg.Key,
                              maxAge = sg.Max(t => t.Age),
                              minAge = sg.Min(t => t.Age),
                              avAge = sg.Average(t => t.Age),
                              sumAge = sg.Sum(t => t.Age),
                              ct = sg.Count()
                          });
    foreach (var item in list)
    {
        Console.WriteLine($"key={item.key}  maxAge={item.maxAge}");
    }
}
Console.WriteLine("*************GroupBy****************");
{
    var list = from s in studentList
               group s by s.ClassId into sg
               //group s by new { xx=s.ClassId >1} into sg//Linq使用Group By返回两个序列。第一个序列包含ClassId >1
第二个序列包含ClassId<=1的。
               select new
               {
                   key = sg.Key,//key是student对象
                   maxAge = sg.Max(t => t.Age),
                   minAge = sg.Min(t => t.Age),
                   avAge = sg.Average(t => t.Age),
                   sumAge = sg.Sum(t => t.Age),
                   ct = sg.Count()
               };
    foreach (var item in list)
    {
        Console.WriteLine($"key={item.key}  maxAge={item.maxAge}");
    }
}
Console.WriteLine("*************GroupBy****************");
```

#### （7）Join

可以使用join来结合两个或更多集合中的数据，它接受两个集合然后创建一个临时的对象集合
连接，相等只能使用equals不能使==

```C#
Console.WriteLine("*************Join****************");
{
    var list = studentList.Join(classList, s => s.ClassId, c => c.Id, (s, c) => new
    {
        Name = s.Name,
        CalssName = c.ClassName
    });
    foreach (var item in list)
    {
        Console.WriteLine($"Name={item.Name},CalssName={item.CalssName}");
    }
}
Console.WriteLine("*************Join****************");
{
    var list = from s in studentList
               join c in classList on s.ClassId equals c.Id
               select new
               {
                   Name = s.Name,
                   CalssName = c.ClassName
               };
    foreach (var item in list)
    {
        Console.WriteLine($"Name={item.Name},CalssName={item.CalssName}");
    }
}
Console.WriteLine("*************Join****************");
```

#### （8）let

let子句接受一个表达式的运算并且把它赋值给一个需要在其他运算中使用的标识符,它是from...let...where片段中的一部分

```C#
Console.WriteLine("*************Let****************");
{
    var list = from s in studentList
               join c in classList on s.ClassId equals c.Id
               let classx= s.ClassId+s.Name
               where classx.Length>5
               select new
               {
                   Name = s.Name,
                   CalssName = c.ClassName
               };
    foreach (var item in list)
    {
        Console.WriteLine($"Name={item.Name},CalssName={item.CalssName}");
    }
}
Console.WriteLine("*************Let****************");
```

