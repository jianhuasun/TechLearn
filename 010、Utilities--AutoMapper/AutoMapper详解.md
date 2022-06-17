# AutoMapper详解

## 一、AutoMapper详解

### 1、概述

#### （1）什么是AutoMapper

简单来说，AutoMapper是以.NET(C#)语言开发的一个轻量的处理一个实体对象到另一个实体对象之间映射关系的组件库。开发人员需要做的是通过AutoMapper配置两个实体对象之间的一些映射关系。就可以直接实现映射关系的复用，提高开发效率，减少重复代码。

官网地址：[http://automapper.org/](http://automapper.org/)

文档地址：[https://docs.automapper.org/en/latest/](https://docs.automapper.org/en/latest/)

#### （2）为什么要做对象之间的映射

为了降低现代开发框架的复杂度，往往需要做代码分层，分层之后，必然出现不同的数据承载对象VO、BO、Entity ，从设计的角度来说，VO、BO、Entity 的设计思路并不违反 DRY 原则，为了分层清晰、减少耦合，多维护几个类的成本也并不是不能接受的，对于代码重复的问题，我们可以通过继承或者组合来解决。

层与层之间数据对象的转换工作是一个非常常见的情况，最简单的方式就是手动复制。当然，使用数据对象转化工具，可以大大简化繁琐的对象转化工作，AutoMapper就是一种很好的转换工具。

### 2、快速开始

#### （1）nuget引入程序集

```bash
AutoMapper   基于版本11.0.1演示，Net5
```

#### （2）配置映射关系

```C#
public  class MyFirstProfile: Profile
{
    public MyFirstProfile()
    {
        CreateMap<Src01, Dest01>();
    }
}
```

#### （3）对象转换

```C#
//创建配置对象
var configuration = new MapperConfiguration(cfg =>
{ 
    cfg.AddProfile<MyFirstProfile>();
});
//创建映射对象
var mapper = configuration.CreateMapper();//或者var mapper=new Mapper(configuration);
//映射动作，将对象转换成另一个对象
var dest = mapper.Map<Dest01>(new Src01() { Name = "zhangsan", Age = 18 });
```

### 3、常用配置

#### （1）加载Profile配置

> 可以直接添加对象的映射关系

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Src01, Dest01>();
});
```

> 将映射关系添加到Profile，再加载Profile，类似于模块化分割业务，让项目结构更加清晰

```C#
var configuration = new MapperConfiguration(cfg =>
{ 
    cfg.AddProfile<MyFirstProfile>();
    //或者cfg.AddProfile(new MyFirstProfile());
});
```

> 加载程序集中的所有Profile

```C#
var basePath = AppContext.BaseDirectory;
var assemblys = Assembly.LoadFrom(Path.Combine(basePath, "Net5.AutoMapper.dll"));
var configuration = new MapperConfiguration(cfg => cfg.AddMaps(assemblys));
```

#### （2）兼容不同的命名方式

- 作用：Camel命名与Pascal命名的兼容。
- 配置之后会映射property_name到PropertyName

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
    cfg.DestinationMemberNamingConvention = new PascalCaseNamingConvention();

    cfg.AddProfile<MyFirstProfile>();
});
```

#### （3）配置的生效范围

> 在全局生效

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
    cfg.DestinationMemberNamingConvention = new PascalCaseNamingConvention();

    cfg.AddProfile<MyFirstProfile>();
});
```

> 在某个Profile生效

```C#
public  class MyFirstProfile: Profile
{
    public MyFirstProfile()
    {        
        SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
        DestinationMemberNamingConvention = new PascalCaseNamingConvention();

        CreateMap<Src01, Dest01>();
    }
}
```

#### （4）映射时替换字符串

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.ReplaceMemberName("Src", "Dest");
    cfg.CreateMap<Src02, Dest02>();
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<Dest02>(new Src02() { NameSrc = "zhangsan" });
```

![image-20220511172330749](http://rc4mudd0q.hd-bkt.clouddn.com/202205111723858.png)

#### （5）映射时匹配前缀或后缀

> RecognizePrefixes匹配前缀，RecognizePostfixes匹配后缀

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.RecognizePrefixes("before");//前缀
    cfg.RecognizePostfixes("after");//后缀
    cfg.CreateMap<Src03,Dest03>();
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<Dest03>(new Src03() { Nameafter = "zhangsan", beforeAge = 18 });
```

![image-20220512101442414](http://rc4mudd0q.hd-bkt.clouddn.com/202205121014324.png)

> Automapper默认匹配了Get前缀，如果不需要可以清除

```c#
cfg.ClearPrefixes();//清除所有前缀
```

#### （6）控制映射字段和属性范围

> ShouldMapField设置字段映射范围，ShouldMapProperty设置属性范围

```C#
var configuration = new MapperConfiguration(cfg => {
    cfg.ShouldMapField = fi => false;
    cfg.ShouldMapProperty = pi => pi.GetMethod != null && (pi.GetMethod.IsPublic || pi.GetMethod.IsPrivate);
});
```

#### （7）提前编译

```C#
var configuration = new MapperConfiguration(cfg => { });
configuration.CompileMappings();
```

### 4、映射初级

#### （1）一些默认行为

> 类型映射配置之后，集合的映射就自动完成了，集合之间可以相互映射

- `IEnumerable`
- `IEnumerable<T>`
- `ICollection`
- `ICollection<T>`
- `IList`
- `IList<T>`
- `List<T>`
- `Arrays`

> 当映射集合时，如果src的成员为null，则dest的成员会new一个默认值，而不是直接复制为null。
>
> 如果真要映映射为null，则如下修改。这个功能可以配置成全局的、某个profile的或某个member的。

```C#
cfg.AllowNullCollections = true;
```

> 假如某个成员的名称为`NameAAA`，则名为`NameAAA`的field，与名为`NameAAA`的property，与名为`GetNameAAA`的方法，三者之间可以自动相互映射

> AutoMapper支持.NET Framework自带的一些基本的类型转换，对于那些不支持的类型转换，或者特殊处理的转换，需要自己定义`TypeConverter`

#### （2）字段相同的会自动映射

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Dest01, Src01>();
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<Dest01>(new Src01() { Name = "zhangsan", Age = 18 });
```

![image-20220512102857657](http://rc4mudd0q.hd-bkt.clouddn.com/202205121028725.png)

#### （3）字段不同要手动配置映射

> 同一个字段的映射，后面的会覆盖前面的，不同的字段，没有做映射的，不会进行赋值

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Src02, Dest02>()
    .ForMember(dest => dest.NameDest, opt => opt.MapFrom(src => src.NameSrc));
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<Dest02>(new Src02() { NameSrc = "zhangsan" });
```

![image-20220512102945919](http://rc4mudd0q.hd-bkt.clouddn.com/202205121029986.png)

#### （4）内部类嵌套类映射

> 类内部嵌套一个类，需要将嵌套的类也进行映射

```c#
public class SrcOuter
{
    public string OutName { get; set; }
    public int OutAge { get; set; }
    public SrcInner Inner { get; set; }
}
public class SrcInner
{
    public string Name { get; set; }
    public int Age { get; set; }
}
public class DestOuter
{
    public string OutName { get; set; }
    public int OutAge { get; set; }
    public DestInner Inner { get; set; }
}
public class DestInner
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<SrcOuter, DestOuter>();
    cfg.CreateMap<SrcInner, DestInner>();
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<DestOuter>(new SrcOuter(){OutName = "zhangsan",OutAge = 18,Inner = new SrcInner() { Name = "lisi", Age = 20 }});
```

![image-20220512103220439](http://rc4mudd0q.hd-bkt.clouddn.com/202205121032508.png)

> 内部类作为一个属性需要配置映射，不能直接在容器类映射内部类的属性，属性类的字段映射还是需要在属性类本身配置

```C#
public class SrcOuterComplex
{
    public string NameSrc { get; set; }
    public int AgeSrc { get; set; }
    public SrcInnerComplex InnerSrc { get; set; }
}
public class SrcInnerComplex
{
    public string NameSrc { get; set; }
    public int AgeSrc { get; set; }
}
public class DestOuterComplex
{
    public string NameDest { get; set; }
    public int AgeDest { get; set; }
    public DestInnerComplex InnerDest { get; set; }
}
public class DestInnerComplex
{
    public string NameDest { get; set; }
    public int AgeDest { get; set; }
}

var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<SrcOuterComplex, DestOuterComplex>()
    .ForMember(dest => dest.NameDest, opt => opt.MapFrom(src => src.NameSrc))
    .ForMember(dest => dest.AgeDest, opt => opt.MapFrom(src => src.AgeSrc))
    //属性类本身作为一个字段需要映射
    .ForMember(dest => dest.InnerDest, opt => opt.MapFrom(src => src.InnerSrc))
    //不能直接在容器类映射内部类的属性
    //.ForMember(dest => dest.InnerDest.NameDest, opt => opt.MapFrom(src => src.InnerSrc.NameSrc))
    ;
    //属性类的字段映射还是需要在属性类本身配置
    cfg.CreateMap<SrcInnerComplex, DestInnerComplex>()
    .ForMember(dest => dest.NameDest, opt => opt.MapFrom(src => src.NameSrc))
    .ForMember(dest => dest.AgeDest, opt => opt.MapFrom(src => src.AgeSrc));
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<DestOuterComplex>(new SrcOuterComplex() { NameSrc = "zhangsan", AgeSrc = 18, InnerSrc = new SrcInnerComplex() { NameSrc = "lisi", AgeSrc = 20 } });
```

![image-20220512132145017](http://rc4mudd0q.hd-bkt.clouddn.com/202205121321103.png)

#### （5）映射继承和多态

> 父类映射包含子类映射Include，子类映射包含父类映射IncludeBase，子类如果有多个直接包含所有IncludeAllDerived

```C#
public class SrcParent
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class SrcChild : SrcParent
{
    public string ChildName { get; set; }
    public int ChildAge { get; set; }
}
public class DestParent
{
    public string Name { get; set; }
    public int Age { get; set; }
}
public class DestChild: DestParent
{
    public string ChildName { get; set; }
    public int ChildAge { get; set; }
}

var configuration = new MapperConfiguration(cfg =>
{
    ////父类映射包含子类映射Include
    //cfg.CreateMap<SrcParent, DestParent>()
    // .Include<SrcChild, DestChild>();
    //cfg.CreateMap<SrcChild, DestChild>();

    ////子类映射包含父类映射IncludeBase
    //cfg.CreateMap<SrcParent, DestParent>();
    //cfg.CreateMap<SrcChild, DestChild>()
    //.IncludeBase<SrcParent, DestParent>();

    //子类如果有多个直接包含所有IncludeAllDerived
    cfg.CreateMap<SrcParent, DestParent>().IncludeAllDerived();
    cfg.CreateMap<SrcChild, DestChild>();
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<DestChild>(new SrcChild() { Name = "zhangsan", ChildName = "zhangsanchild", Age = 35, ChildAge = 3 });
```

![image-20220512110715719](http://rc4mudd0q.hd-bkt.clouddn.com/202205121107801.png)

#### （6）构造函数映射

> 如果dest构造函数的入参名和src的某个member一致，automapper会自动映射

```C#
public class SrcCtor
{
    public string Name { get; set; }
}
public class DestCtor
{
    private string DestName { get; set; }

    public DestCtor(string name)
    { 
        this.DestName = name;
    }
}

var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<SrcCtor, DestCtor>();
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<DestCtor>(new SrcCtor() { Name = "zhangsan" });
```

![image-20220512134543231](http://rc4mudd0q.hd-bkt.clouddn.com/202205121345302.png)

> 如果dest构造函数的入参名和src的某个member不一致，需要手动配置映射

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<SrcCtor02, DestCtor02>()
    .ForCtorParam("destname", opt => opt.MapFrom(src => src.Name));
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<DestCtor02>(new SrcCtor02() { Name = "zhangsan" });
```

![image-20220512135242903](http://rc4mudd0q.hd-bkt.clouddn.com/202205121352984.png)

> 也可以禁用构造函数映射

```C#
var configuration = new MapperConfiguration(cfg => cfg.DisableConstructorMapping());
```

> 也可以配置什么情况下不用构造函数映射

```C#
var configuration = new MapperConfiguration(cfg => cfg.ShouldUseConstructor = ci => !ci.IsPrivate);//不匹配私有构造函数
```

#### （7）复杂类映射成扁平类

> 自动映射
>
> 映射时AutoMapper发现，src里没有`CustomerName`这个成员，则会将dest的`CustomerName`按照PascalCase的命名方式拆分为独立的单词。所以`CustomerName`会映射到src的`Customer.Name`。

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<SrcComplex, DestSimple>();
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<DestSimple>(new SrcComplex() { Customer =new Customer() { Name= "zhangsan" }});
```

![image-20220512142058402](http://rc4mudd0q.hd-bkt.clouddn.com/202205121420482.png)

> 可以禁用这种自动映射

```C#
cfg.DestinationMemberNamingConvention = new ExactMatchNamingConvention();
```

> 可以手工配置映射
>
> `IncludeMembers`参数的顺序很重要，这也就是dest的`Description`为“description”而不是“description2”的原因，因为`InnerSource`的`Description`属性最先匹配到了`Destination`的`Description`属性。
>
> `IncludeMembers`相当于把子类打平添加到了src里，并进行映射。

```C#
public class SrcComplex02
{
    public string Name { get; set; }
    public InnerSource InnerSource { get; set; }
    public InnerSource02 InnerSource02 { get; set; }
}
public class InnerSource
{
    public string Name { get; set; }
    public string Description { get; set; }
}
public class InnerSource02
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
}
public class DestSimple02
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
}

var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<SrcComplex02, DestSimple02>().IncludeMembers(s => s.InnerSource, s => s.InnerSource02);
    cfg.CreateMap<InnerSource, DestSimple02>();
    cfg.CreateMap<InnerSource02, DestSimple02>();
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<DestSimple02>(new SrcComplex02()
{
    Name = "name",
    InnerSource = new InnerSource { Description = "description" },
    InnerSource02 = new InnerSource02 { Title = "title", Description = "descpripiton2" }
});
```

![image-20220512152122915](http://rc4mudd0q.hd-bkt.clouddn.com/202205121521997.png)

#### （8）映射反转

> ReverseMap一般在CreateMap方法或者ForMember等方法之后，相当于src和dest根据你自己的配置反向映射

```C#
cfg.CreateMap<Order, OrderDto>().ReverseMap();
//等同于以下两句
cfg.CreateMap<Order,OrderDto>();
cfg.CreateMap<OrderDto,Order>();
```

> 反向映射可以用ForPath配置

```C#
cfg.CreateMap<Order, OrderDto>()
  .ForMember(d => d.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
  .ReverseMap()
  .ForPath(s => s.Customer.Name, opt => opt.MapFrom(src => src.CustomerName));
```

> `ReverseMap`到底能Reverse哪些东西
>
> - 可以Reverse像PascalCase的命名方式拆分的member，就像CreateMap可以自动处理Customer.Name与CustomerName的映射一样。即：CreateMap不需要额外配置正向就能映射的，那 ReverseMap也可以自动反向映射。
> - opt.MapForm()操作可以被reverse，如CreateMap<Person2, Person>().ForMember(dest => dest.Name2, opt => opt.MapFrom(src => src.Name)).ReverseMap();，当从Person映射到Person2的时候，Name2也可以直接映射到Name上。
> - 不支持opt.Ignore()反向映射，即CreateMap<Person2, Person>().ForMember(dest => dest.Name, opt => opt.Ignore()).ReverseMap()支持Person2->Person时，忽略Name属性，但是反过来不会忽略。

#### （9）使用特性映射

> 等同于`CreateMap<Order,OrderDto>()`，然后配置的时候用AddMaps方法加载程序集

```C#
[AutoMap(typeof(Order))]
public class OrderDto {}

var configuration = new MapperConfiguration(cfg => cfg.AddMaps("MyAssembly"));
```

> 还能标记如下特性

- ReverseMap (bool)
- ConstructUsingServiceLocator (bool)
- MaxDepth (int)
- PreserveReferences (bool)
- DisableCtorValidation (bool)
- IncludeAllDerived (bool)
- TypeConverter (Type)

#### （10）动态类型到普通类型映射

```C#
var configuration = new MapperConfiguration(cfg =>{ });
var mapper = configuration.CreateMapper();
dynamic src = new ExpandoObject();//ExpandoObject对象包含可在运行时动态添加或移除的成员
src.Name = "zhangsan";
src.Age = 18;
var dest = mapper.Map<Dest01>(src);
```

![image-20220512170034283](http://rc4mudd0q.hd-bkt.clouddn.com/202205121700365.png)

#### （11）泛型映射

> CreateMap`不需要传具体的`T

```C#
public class SrcGeneric<T>
{
    public T TValue { get; set; }
}
public class DestGeneric<T>
{
    public T TValue { get; set; }
}

var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap(typeof(SrcGeneric<>), typeof(DestGeneric<>));
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<DestGeneric<int>>(new SrcGeneric<int>() { TValue = 18 });
```

![image-20220512171841888](http://rc4mudd0q.hd-bkt.clouddn.com/202205121718974.png)

#### （12）Dictonary到普通类型映射

> 不需要配置映射关系，配置之后反而无法映射成功

```C#
var configuration = new MapperConfiguration(cfg =>{});
var mapper = configuration.CreateMapper();
Dictionary<string, object> dic = new Dictionary<string, object>();
dic.Add("Name","zhangsan");
dic.Add("Age",18);
var dest = mapper.Map<Dest01>(dic);
```

![image-20220513130521860](http://rc4mudd0q.hd-bkt.clouddn.com/202205131305950.png)

### 5、映射进阶

#### （1）自定义类型转换

> 可以直接转换，也可以定义转换器转换

```C#
public class SrcConverter
{
    public string Age { get; set; }
    public string Time { get; set; }
}
public class DestConverter
{
    public int Age { get; set; }
    public DateTime Time { get; set; }
}
public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
{
    public DateTime Convert(string source, DateTime destination, ResolutionContext context)
    {
        return System.Convert.ToDateTime(source);
    }
}

var configuration = new MapperConfiguration(cfg => {
    //可以直接转换
    cfg.CreateMap<string, int>().ConvertUsing(s => Convert.ToInt32(s));
    //也可以定义转换器转换
    cfg.CreateMap<string, DateTime>().ConvertUsing(new DateTimeTypeConverter());
    cfg.CreateMap<SrcConverter, DestConverter>();
});
var src = new SrcConverter
{
    Age = "5",
    Time = "05/12/2022"
};
var mapper = configuration.CreateMapper();
var dest = mapper.Map<DestConverter>(src);
```

![image-20220512181153395](http://rc4mudd0q.hd-bkt.clouddn.com/202205121811483.png)

#### （2）自定义值解析

> 这种与一般的`MapFrom()`区别是可以添加更加复杂的逻辑

- `MapFrom<CustomResolver>`
- `MapFrom(new CustomResolver())`

```C#
public class SrcResolver
{
    public string Name { get; set; }

    public int Age { get; set; }
}
public class DestResolver
{
    public string Info { get; set; }
}
public class CustomResolver : IValueResolver<SrcResolver, DestResolver, string>
{
    public string Resolve(SrcResolver source, DestResolver destination, string destMember, ResolutionContext context)
    {		
		return $"{source.Name}--{source.Age}";
	}
}

var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<SrcResolver, DestResolver>()
    //针对的是某一个map的某一个member的两种写法
    //.ForMember(dest => dest.Info, opt => opt.MapFrom<CustomResolver>())
    .ForMember(dest => dest.Info, opt => opt.MapFrom(new CustomResolver()));
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<DestResolver>(new SrcResolver() { Name = "zhangsan", Age = 18 });
```

![image-20220512190115817](http://rc4mudd0q.hd-bkt.clouddn.com/202205121901906.png)

> `ITypeConverter`、`IValueConverter`、`IValueResolver`、`IMemberValueResover`比较
>
> - Type converter = Func<TSource, TDestination, TDestination>
> - Value resolver = Func<TSource, TDestination, TDestinationMember>
> - Member value resolver = Func<TSource, TDestination, TSourceMember, TDestinationMember>
> - Value converter = Func<TSourceMember, TDestinationMember>
> - 这四个的使用方式都是使用ConvertUsing()方法，区别是type converter 是针对全局的，其它三个是针对某个member的。 入参出参也不一样。

#### （3）条件映射

> 符合某些条件时才映射
>
> Condition方法会在MapFrom方法后判断,PreCondition会在MapFrom前判断

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<SrcCondition, DestCondition>()
    //src.Name.Length>=3&&(src.Name+"XXX").Length >= 5 两个条件都满足才映射
    .ForMember(dest => dest.Name, opt => opt.PreCondition(src => src.Name.Length >= 3))
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name + "XXX"))
    .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name.Length >= 5))
    //src.Age <= 15&&src.Age * 3>=30 两个条件都满足才映射
    .ForMember(dest => dest.Age, opt => opt.PreCondition(src => src.Age <= 15))
    .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age * 3))
    .ForMember(dest => dest.Age, opt => opt.Condition(src => src.Age >= 30));
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<DestCondition>(new SrcCondition() { Name = "zhangsan", Age = 18 });
```

![image-20220512192749459](http://rc4mudd0q.hd-bkt.clouddn.com/202205121927558.png)

#### （4）空值处理

> 如果src为null的话，就给dest一个默认值

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Src01, Dest01>()
    .ForMember(dest => dest.Name, opt => opt.NullSubstitute("XXX"));
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<Dest01>(new Src01() { Age = 18 });
```

![image-20220512193615084](http://rc4mudd0q.hd-bkt.clouddn.com/202205121936172.png)

#### （5）类型映射处理

> 针对某种类型的映射做处理，可以应用到全局、某个Profile、某个Map或某个member

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.ValueTransformers.Add<string>(val => "fawaikuangtu "+val + " !!!");
    cfg.ValueTransformers.Add<int>(val => val + 2);
    cfg.CreateMap<Src01, Dest01>();
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<Dest01>(new Src01() { Name = "zhangsan",Age=17 });
```

![image-20220513094812182](http://rc4mudd0q.hd-bkt.clouddn.com/202205130948267.png)

#### （6）映射前后干点事

> 映射关系配置时配置

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Src01, Dest01>()
    //转换前对dest的处理会被转换结果覆盖
    .BeforeMap((src, dest) => { Console.WriteLine($"转换前处理前{src.Name}--{dest.Name}"); src.Name = src.Name + " !!!"; dest.Name = "转换前赋值"; Console.WriteLine($"转换前处理后{src.Name}--{dest.Name}"); })
    //转换后对src的处理是没意义的
    .AfterMap((src, dest) => { Console.WriteLine($"转换后处理前{src.Name}--{dest.Name}"); dest.Name = "fawaikuangtu " + dest.Name; src.Name = "转换后赋值"; Console.WriteLine($"转换后处理后{src.Name}--{dest.Name}"); });
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<Dest01>(new Src01() { Name = "zhangsan" });
```

![image-20220513104353197](http://rc4mudd0q.hd-bkt.clouddn.com/202205131043298.png)

> 映射关系转换时配置

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Src01, Dest01>();                    
});
var mapper = configuration.CreateMapper();
var src = new Src01() { Name = "zhangsan" };
var dest = mapper.Map<Src01,Dest01>(src, opt => {
    //转换前dest是null
    opt.BeforeMap((src, dest) => {src.Name = src.Name + " !!!"; });
    //转换后对src的处理是没意义的
    opt.AfterMap((src, dest) => {dest.Name = "fawaikuangtu " + dest.Name;});
});
```

![image-20220513105759467](http://rc4mudd0q.hd-bkt.clouddn.com/202205131057544.png)

#### （7）忽略某字段的映射

> 映射时配置忽略字段

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Src01, Dest01>()
    .ForMember(dest => dest.Name, opt => opt.Ignore());
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<Dest01>(new Src01() { Name = "zhangsan",Age=18 });
```

![image-20220513111247047](http://rc4mudd0q.hd-bkt.clouddn.com/202205131112140.png)

> 忽略属性和字段

```C#
var configuration = new MapperConfiguration(cfg =>
{
    cfg.ShouldMapField = fi => false;
    cfg.ShouldMapProperty = fi => false;
    cfg.CreateMap<Src01, Dest01>();
});
var mapper = configuration.CreateMapper();
var dest = mapper.Map<Dest01>(new Src01() { Name = "zhangsan", Age = 18 });
```

![image-20220513112320300](http://rc4mudd0q.hd-bkt.clouddn.com/202205131123393.png)

#### （8）多个源映射到一个目标

```C#
public class SrcPartA
{
    public string FiledPartA { get; set; }
    public string FiledPartB { get; set; }
}
public class SrcPartB
{
    public string FiledPartC { get; set; }
    public string FiledPartD { get; set; }
}
public class SrcPartC
{
    public string FiledPartE { get; set; }
    public string FiledPartF { get; set; }
}
public class DestCombine
{
    public string FiledA { get; set; }
    public string FiledB { get; set; }
    public string FiledC { get; set; }
    public string FiledD { get; set; }
    public string FiledE { get; set; }
    public string FiledF { get; set; }
}

var configuration = new MapperConfiguration(cfg =>
{                    
    cfg.CreateMap<SrcPartA, DestCombine>()
    .ForMember(dest => dest.FiledA, opt => opt.MapFrom(src => src.FiledPartA))
    .ForMember(dest => dest.FiledB, opt => opt.MapFrom(src => src.FiledPartB));

    cfg.CreateMap<SrcPartB, DestCombine>()
    .ForMember(dest => dest.FiledC, opt => opt.MapFrom(src => src.FiledPartC))
    .ForMember(dest => dest.FiledD, opt => opt.MapFrom(src => src.FiledPartD));

    cfg.CreateMap<SrcPartC, DestCombine>()
    .ForMember(dest => dest.FiledE, opt => opt.MapFrom(src => src.FiledPartE))
    .ForMember(dest => dest.FiledF, opt => opt.MapFrom(src => src.FiledPartF));
});
var mapper = configuration.CreateMapper();
var srcPartA = new SrcPartA() { FiledPartA="A",FiledPartB="B"};
var srcPartB = new SrcPartB() { FiledPartC="C",FiledPartD="D"};
var srcPartC = new SrcPartC() { FiledPartE="E",FiledPartF="F"};
var dest=new DestCombine();
dest=mapper.Map(srcPartA,dest);
dest=mapper.Map(srcPartB,dest);
dest=mapper.Map(srcPartC,dest);
```

![image-20220513135115911](http://rc4mudd0q.hd-bkt.clouddn.com/202205131351018.png)

### 6、性能测试

> 转换百万对象，耗时414毫秒

```C#
public class SrcPart
{
    public SrcPartA SrcPartA { get; set; }
    public SrcPartB SrcPartB { get; set; }
    public SrcPartC SrcPartC { get; set; }
}

//配置映射关系
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<SrcPart, DestCombine>()
    .ForMember(dest => dest.FiledA, opt => opt.MapFrom(src => src.SrcPartA.FiledPartA))
    .ForMember(dest => dest.FiledB, opt => opt.MapFrom(src => src.SrcPartA.FiledPartB))
    .ForMember(dest => dest.FiledC, opt => opt.MapFrom(src => src.SrcPartB.FiledPartC))
    .ForMember(dest => dest.FiledD, opt => opt.MapFrom(src => src.SrcPartB.FiledPartD))
    .ForMember(dest => dest.FiledE, opt => opt.MapFrom(src => src.SrcPartC.FiledPartE))
    .ForMember(dest => dest.FiledF, opt => opt.MapFrom(src => src.SrcPartC.FiledPartF));                    
});
var mapper = configuration.CreateMapper();
//创建数据
List<SrcPart> srcParts = new List<SrcPart>();
for (int i = 0; i < 1000_000; i++)
{
    srcParts.Add(new SrcPart()
    {
        SrcPartA = new SrcPartA() { FiledPartA = "FiledPartA" + Guid.NewGuid(), FiledPartB = "FiledPartA" + Guid.NewGuid() },
        SrcPartB = new SrcPartB() { FiledPartC = "FiledPartC" + Guid.NewGuid(), FiledPartD = "FiledPartD" + Guid.NewGuid() },
        SrcPartC = new SrcPartC() { FiledPartE = "FiledPartE" + Guid.NewGuid(), FiledPartF = "FiledPartF" + Guid.NewGuid() }
    });
}
//转换数据
Stopwatch sw = new Stopwatch();
sw.Start();
List<DestCombine> dest=mapper.Map<List<DestCombine>>(srcParts);
sw.Stop();
Console.WriteLine("耗时：" + sw.ElapsedMilliseconds);
```

```bash
耗时：414
```

### 7、扩展`IQueryable`

#### （1）概述

>这个功能存在的意义是为了解决一些orm框架返回的是`IQueryable`类型，使用一般的mapper.Map做转换时，会查询出来整行数据，然后再挑选出来某些字段做映射，会降低性能的问题。解决方法是使用`ProjectTo`

#### （2）nuget引入程序集

```bash
AutoMapper
AutoMapper.EF6
```

#### （3）代码实现

```C#
public class OrderLine
{
  public int Id { get; set; }
  public int OrderId { get; set; }
  public Item Item { get; set; }
  public decimal Quantity { get; set; }
}
public class Item
{
  public int Id { get; set; }
  public string Name { get; set; }
}
public class OrderLineDTO
{
  public int Id { get; set; }
  public int OrderId { get; set; }
  public string Item { get; set; }
  public decimal Quantity { get; set; }
}

var configuration = new MapperConfiguration(cfg =>
    cfg.CreateMap<OrderLine, OrderLineDTO>()
    .ForMember(dto => dto.Item, conf => conf.MapFrom(ol => ol.Item.Name)));

//这样查Item表的时候，只会select name字段。
public List<OrderLineDTO> GetLinesForOrder(int orderId)
{
  using (var context = new orderEntities())
  {
    return context.OrderLines.Where(ol => ol.OrderId == orderId)
             .ProjectTo<OrderLineDTO>(configuration).ToList();
  }
}
```

### 8、集成到Asp.NetCore5框架

#### （1）nuget引入程序集

```bash
AutoMapper
AutoMapper.Extensions.Microsoft.DependencyInjection
```

#### （2）创建映射关系

```C#
public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateMap<User, UserDto>()
        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => "fawaikuangtu " + src.Name));
    }
}
```

#### （3）在控制器中注入并使用

```C#
public class UserRepository : IUserRepository
{
    public User GetUser()
    {
        return new User() { Name = "zhangsan", Age = 18, Sex = "male", Password = "SDF" };
    }
}
```

```C#
public class UserService : IUserService
{    
    private IUserRepository UserRepositoryCtor { get; set; }
    private IMapper _mapper;
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        UserRepositoryCtor = userRepository;
        this._mapper = mapper;
    }    
    public string Login(string username, string password)
    {
        UserDto userdto=_mapper.Map<UserDto>(UserRepositoryCtor.GetUser());
        return userdto.Name+"登录成功";
    }
}
```

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

```html
@model String
<h2>this is fourth index...</h2>
<h2>@Model</h2>
```

#### （4）在`Startup`的`ConfigureServices`中注册到容器

```C#
services.AddAutoMapper(typeof(OrganizationProfile));
services.AddTransient<IUserService, UserService>();
services.AddTransient<IUserRepository, UserRepository>();
```

#### （5）运行效果

![image-20220513152702540](http://rc4mudd0q.hd-bkt.clouddn.com/202205131527646.png)



