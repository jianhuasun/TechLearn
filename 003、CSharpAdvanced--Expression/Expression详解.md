# Expression详解

## 一、Expression是什么

### 1、如何定义

- Expression<Func<TSource, bool>>就是表达式目录树
- Expression不能带有大括号，只能有一行代码

### 2、和委托的区别

- 在委托外面包裹一层Expression<>就是表达式目录树

- 表达式目录树可以通过Compile()转换成一个委托

### 3、Expression本质

- 表达式目录树是一个类的封装，描述了一个结构，有身体部分和参数部分
- 身体部分分为左边和右边，内部描述了左边和右边之间的关系，可以不断的往下拆分，类似于二叉树
- 表达式目录树展开后的每一个节点也是一个表达式目录树

```C#
Expression<Func<People, bool>> expression = p => p.Id == 10;
Func<People, bool> func = expression.Compile();
bool bResult = func.Invoke(new People()
{
    Id = 10,
    Name = "张三"
});
```

## 二、Expression动态拼装

### 1、最基础版本

```C#
Expression<Func<int>> expression = () => 123 + 234;
//常量表达式
ConstantExpression expression1 = Expression.Constant(123);
ConstantExpression expression2 = Expression.Constant(234);
//二元表达式
BinaryExpression binaryExpression = Expression.Add(expression1, expression2);
Expression<Func<int>> expressionReslut = Expression.Lambda<Func<int>>(binaryExpression);
Func<int> func = expressionReslut.Compile();
int iResult = func.Invoke();
```

### 2、带参数版本

```C#
Expression<Func<int, int>> expression1 = m => m + 1;
Func<int, int> func = expression1.Compile();
int iResult = func.Invoke(5);
//参数表达式
ParameterExpression parameterExpression = Expression.Parameter(typeof(int), "m");
//常量表达式
ConstantExpression constant = Expression.Constant(1, typeof(int));
//二元表达式
BinaryExpression addExpression = Expression.Add(parameterExpression, constant);
Expression<Func<int, int>> expression = Expression.Lambda<Func<int, int>>(addExpression, new ParameterExpression[1]
{
      parameterExpression
});
Func<int, int> func1 = expression.Compile();
int iResult1 = func1.Invoke(5);
```

### 3、带有多个参数

```C#
Expression<Func<int, int, int>> expression = (m, n) => m * n + 2;
Func<int, int, int> func = expression.Compile();
int iResult = func.Invoke(10, 20);
//参数表达式
ParameterExpression parameterExpressionM = Expression.Parameter(typeof(int), "m");
ParameterExpression parameterExpressionN = Expression.Parameter(typeof(int), "n");
//二元表达式
BinaryExpression multiply = Expression.Multiply(parameterExpressionM, parameterExpressionN);
//常量表达式
ConstantExpression constantExpression = Expression.Constant(2);
//二元表达式
BinaryExpression plus = Expression.Add(multiply, constantExpression);
Expression<Func<int, int, int>> expression1 = Expression.Lambda<Func<int, int, int>>(plus, new ParameterExpression[2]
{
      parameterExpressionM,
      parameterExpressionN
});
Func<int, int, int> func1 = expression1.Compile();
int iResult1 = func1.Invoke(10, 20);
```

### 4、对象字段值比较

类似于这种比较复杂的，建议大家可以反编译看看

```C#
Expression<Func<People, bool>> predicate = c => c.Id == 10;
Func<People, bool> func = predicate.Compile();
bool bResult = func.Invoke(new People()
{
    Id = 10
});

//参数表达式
ParameterExpression parameterExpression = Expression.Parameter(typeof(People), "c");
//反射获取属性
FieldInfo fieldId = typeof(People).GetField("Id");
//通过parameterExpression来获取调用Id
MemberExpression idExp = Expression.Field(parameterExpression, fieldId);
//常量表达式
ConstantExpression constant10 = Expression.Constant(10, typeof(int));
//二元表达式
BinaryExpression expressionExp = Expression.Equal(idExp, constant10);
Expression<Func<People, bool>> predicate1 = Expression.Lambda<Func<People, bool>>(expressionExp, new ParameterExpression[1]
{
            parameterExpression
});

Func<People, bool> func1 = predicate1.Compile();
bool bResult1 = func1.Invoke(new People()
{
    Id = 10
});
```

### 5、多条件

如果遇到很长的表达式目录树，拼装建议从右往左拼装

```C#
Expression<Func<People, bool>> predicate = c =>
    c.Id.ToString() == "10"
    && c.Name.Equals("张三")
    && c.Age > 35;
Func<People, bool> func = predicate.Compile();
bool bResult = func.Invoke(new People()
{
    Id = 10,
    Name = "张三",
    Age = 36
});

ParameterExpression parameterExpression = Expression.Parameter(typeof(People), "c");
//c.Age > 35
ConstantExpression constant35 = Expression.Constant(35);
PropertyInfo propAge = typeof(People).GetProperty("Age");
MemberExpression ageExp = Expression.Property(parameterExpression, propAge);
BinaryExpression cagExp = Expression.GreaterThan(ageExp, constant35);
//c.Name.Equals("张三")
ConstantExpression constantrichard = Expression.Constant("张三");
PropertyInfo propName = typeof(People).GetProperty("Name");
MemberExpression nameExp = Expression.Property(parameterExpression, propName);
MethodInfo equals = typeof(string).GetMethod("Equals", new Type[] { typeof(string) });
MethodCallExpression NameExp = Expression.Call(nameExp, equals, constantrichard);
//c.Id.ToString() == "10"
ConstantExpression constantExpression10 = Expression.Constant("10", typeof(string));
FieldInfo fieldId = typeof(People).GetField("Id");
var idExp = Expression.Field(parameterExpression, fieldId);
MethodInfo toString = typeof(int).GetMethod("ToString", new Type[0]);
var toStringExp = Expression.Call(idExp, toString, Array.Empty<Expression>());
var EqualExp = Expression.Equal(toStringExp, constantExpression10);
//c.Id.ToString() == "10"&& c.Name.Equals("张三")&& c.Age > 35
var plus = Expression.AndAlso(EqualExp, NameExp);
var exp = Expression.AndAlso(plus, cagExp);
Expression<Func<People, bool>> predicate1 = Expression.Lambda<Func<People, bool>>(exp, new ParameterExpression[1]
{
     parameterExpression
});
Func<People, bool> func1 = predicate1.Compile();
bool bResult1 = func1.Invoke(new People()
{
    Id = 10,
    Name = "张三",
    Age = 36
});
```

## 三、Expression应用之Mapper映射

需求：需要把People字段值映射到PeopleCopy字段

### 1、硬编码

性能好，不灵活；不能共用

```C#
PeopleCopy peopleCopy0 = new PeopleCopy()
{
    Id = people.Id,
    Name = people.Name,
    Age = people.Age
};
```

### 2、反射

灵活，但是性能不好

```C#
using System;

namespace MyExpression.MappingExtend
{
    public class ReflectionMapper
    {
        /// <summary>
        /// 反射
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="tIn"></param>
        /// <returns></returns>
        public static TOut Trans<TIn, TOut>(TIn tIn)
        {
            TOut tOut = Activator.CreateInstance<TOut>();
            foreach (var itemOut in tOut.GetType().GetProperties())
            {
                var propIn = tIn.GetType().GetProperty(itemOut.Name);
                itemOut.SetValue(tOut, propIn.GetValue(tIn)); 
            }

            foreach (var itemOut in tOut.GetType().GetFields())
            {
                var fieldIn = tIn.GetType().GetField(itemOut.Name);
                itemOut.SetValue(tOut, fieldIn.GetValue(tIn)); 
            }
            return tOut;
        }
    }
}
```

调用

```C3
PeopleCopy peopleCopy1 = ReflectionMapper.Trans<People, PeopleCopy>(people);
```

### 3、序列化

灵活，但是性能不好

```C#
using Newtonsoft.Json;

namespace MyExpression.MappingExtend
{
    public class SerializeMapper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        public static TOut Trans<TIn, TOut>(TIn tIn)
        {
            string strJson = JsonConvert.SerializeObject(tIn); 
            return JsonConvert.DeserializeObject<TOut>(strJson);
        }
    }
}
```

调用

```C#
PeopleCopy peopleCopy2 = SerializeMapper.Trans<People, PeopleCopy>(people);
```

### 4、Expression动态拼接+普通缓存

- 把People变成PeopleCopy的过程封装在一个委托中，这个委托通过表达式目录树Compile出来，过程动态拼装适应不同的类型
- 第一次生成的时候，保存一个委托在缓存中，如果第二次来，委托就可以直接从缓存中获取到，直接运行委托，效率高

```C#
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MyExpression.MappingExtend
{    
    public class ExpressionMapper
    {
        /// <summary>
        /// 字典缓存，保存的是委托，委托内部是转换的动作
        /// </summary>
        private static Dictionary<string, object> _Dic = new Dictionary<string, object>();

        /// <summary>
        /// Expression动态拼接+普通缓存
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="tIn"></param>
        /// <returns></returns>
        public static TOut Trans<TIn, TOut>(TIn tIn)
        {
            string key = $"funckey_{typeof(TIn).FullName}_{typeof(TOut).FullName}";
            if (!_Dic.ContainsKey(key))
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
                List<MemberBinding> memberBindingList = new List<MemberBinding>();
                foreach (var item in typeof(TOut).GetProperties())
                {
                    MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                    MemberBinding memberBinding = Expression.Bind(item, property);
                    memberBindingList.Add(memberBinding);
                }
                foreach (var item in typeof(TOut).GetFields())
                {
                    MemberExpression property = Expression.Field(parameterExpression, typeof(TIn).GetField(item.Name));
                    MemberBinding memberBinding = Expression.Bind(item, property);
                    memberBindingList.Add(memberBinding);
                }
                MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
                Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[]
                {
                    parameterExpression
                });
                Func<TIn, TOut> func = lambda.Compile();//拼装是一次性的
                _Dic[key] = func;
            }
            return ((Func<TIn, TOut>)_Dic[key]).Invoke(tIn);
        }
    }
}
```

调用

```C#
PeopleCopy peopleCopy3 = ExpressionMapper.Trans<People, PeopleCopy>(people);
```

### 5、Expression动态拼接+泛型缓存

泛型缓存，就是为为每一组类型的组合，生成一个副本，性能最高

```C#
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MyExpression.MappingExtend
{
    /// <summary>
    /// Expression动态拼接+泛型缓存
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public class ExpressionGenericMapper<TIn, TOut>//Mapper`2
    {
        private static Func<TIn, TOut> _FUNC = null;
        static ExpressionGenericMapper()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();
            foreach (var item in typeof(TOut).GetProperties())
            {
                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }
            foreach (var item in typeof(TOut).GetFields())
            {
                MemberExpression property = Expression.Field(parameterExpression, typeof(TIn).GetField(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }
            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[]
            {
                    parameterExpression
            });
            _FUNC = lambda.Compile();//拼装是一次性的
        }
        public static TOut Trans(TIn t)
        {
            return _FUNC(t);
        }
    }
}
```

调用

```C#
PeopleCopy peopleCopy4 = ExpressionGenericMapper<People, PeopleCopy>.Trans(people);
```

### 6、性能比较

Expression动态拼接+泛型缓存性能高，而且灵活

```C#
long common = 0;
long generic = 0;
long cache = 0;
long reflection = 0;
long serialize = 0;
{
    Stopwatch watch = new Stopwatch();
    watch.Start();
    for (int i = 0; i < 1_000_000; i++)
    {
        PeopleCopy peopleCopy = new PeopleCopy()
        {
            Id = people.Id,
            Name = people.Name,
            Age = people.Age
        };
    }
    watch.Stop();
    common = watch.ElapsedMilliseconds;
}
{
    Stopwatch watch = new Stopwatch();
    watch.Start();
    for (int i = 0; i < 1_000_000; i++)
    {
        PeopleCopy peopleCopy = ReflectionMapper.Trans<People, PeopleCopy>(people);
    }
    watch.Stop();
    reflection = watch.ElapsedMilliseconds;
}
{
    Stopwatch watch = new Stopwatch();
    watch.Start();
    for (int i = 0; i < 1_000_000; i++)
    {
        PeopleCopy peopleCopy = SerializeMapper.Trans<People, PeopleCopy>(people);
    }
    watch.Stop();
    serialize = watch.ElapsedMilliseconds;
}
{

    Stopwatch watch = new Stopwatch();
    watch.Start();
    for (int i = 0; i < 1_000_000; i++)
    {
        PeopleCopy peopleCopy = ExpressionMapper.Trans<People, PeopleCopy>(people);
    }
    watch.Stop();
    cache = watch.ElapsedMilliseconds;
}
{
    Stopwatch watch = new Stopwatch();
    watch.Start();
    for (int i = 0; i < 1_000_000; i++)
    {
        PeopleCopy peopleCopy = ExpressionGenericMapper<People, PeopleCopy>.Trans(people);
    }
    watch.Stop();
    generic = watch.ElapsedMilliseconds;
}

Console.WriteLine($"common = { common} ms");
Console.WriteLine($"reflection = { reflection} ms");
Console.WriteLine($"serialize = { serialize} ms");
Console.WriteLine($"cache = { cache} ms");
Console.WriteLine($"generic = { generic} ms");
```

> 运行结果

```bash
common = 32 ms
reflection = 1026 ms
serialize = 2510 ms
cache = 236 ms
generic = 31 ms
```

## 四、ExpressionVisitor解析Expression

### 1、Expression解析

- Expression是通过访问者模式进行解析的，官方提供了ExpressionVisitor抽象类
- ExpressionVisitor的Visit方法是解析表达式目录树的一个入口，Visit方法判断Expression是一个什么表达式目录树，走不同的细分方法进行进一步解析
- ExpressionVisitor的VisitBinary方法是对二员表达式的解析，所有复杂的表达式都会拆解成二员表达式进行解析

### 2、Expression修改

自定义一个OperationsVisitor，继承自ExpressionVisitor，复写父类的VisitBinary方法，修改Expression的解析

> OperationsVisitor定义

```C#
using System.Linq.Expressions;

namespace MyExpression
{
    /// <summary>
    /// 自定义Visitor
    /// </summary>
    public class OperationsVisitor : ExpressionVisitor
    {
        /// <summary>
        /// 覆写父类方法；//二元表达式的访问
        /// 把表达式目录树中相加改成相减，相乘改成相除
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        { 
            if (b.NodeType == ExpressionType.Add)//相加
            {
                Expression left = this.Visit(b.Left);
                Expression right = this.Visit(b.Right);
                return Expression.Subtract(left, right);//相减
            }
            else if (b.NodeType==ExpressionType.Multiply) //相乘
            {
                Expression left = this.Visit(b.Left);
                Expression right = this.Visit(b.Right);
                return Expression.Divide(left, right); //相除
            } 
            return base.VisitBinary(b);
        }
    }
}
```

> Expression解析转换

```
Expression<Func<int, int, int>> exp = (m, n) => m * n + 2;
Console.WriteLine(exp.ToString());
OperationsVisitor visitor = new OperationsVisitor();
Expression expNew = visitor.Visit(exp);
Console.WriteLine(expNew.ToString());
```

> 运行结果

```bash
(m, n) => ((m * n) + 2)
(m, n) => ((m / n) - 2)
```

### 3、封装多条件连接扩展方法

> 扩展方法实现

```C#
/// <summary>
/// 合并表达式 And Or Not扩展方法
/// </summary>
public static class ExpressionExtend
{
    /// <summary>
    /// 合并表达式 expr1 AND expr2
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expr1"></param>
    /// <param name="expr2"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        if (expr1 == null || expr2 == null)
        {
            throw new Exception("null不能处理");
        }
        ParameterExpression newParameter = Expression.Parameter(typeof(T), "x");
        NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);
        Expression left = visitor.Visit(expr1.Body);
        Expression right = visitor.Visit(expr2.Body);
        BinaryExpression body = Expression.And(left, right);
        return Expression.Lambda<Func<T, bool>>(body, newParameter);
    }

    /// <summary>
    /// 合并表达式 expr1 or expr2
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expr1"></param>
    /// <param name="expr2"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        if (expr1 == null || expr2 == null)
        {
            throw new Exception("null不能处理");
        }
        ParameterExpression newParameter = Expression.Parameter(typeof(T), "x");
        NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);
        Expression left = visitor.Visit(expr1.Body);
        Expression right = visitor.Visit(expr2.Body);
        BinaryExpression body = Expression.Or(left, right);
        return Expression.Lambda<Func<T, bool>>(body, newParameter);
    }

    /// <summary>
    /// 表达式取非
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expr"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
    {
        if (expr == null)
        {
            throw new Exception("null不能处理");
        }
        ParameterExpression newParameter = expr.Parameters[0];
        UnaryExpression body = Expression.Not(expr.Body);
        return Expression.Lambda<Func<T, bool>>(body, newParameter);
    }
}
```

> 自定义Visitor

```C#
internal class NewExpressionVisitor : ExpressionVisitor
{
    public ParameterExpression _NewParameter { get; private set; }
    public NewExpressionVisitor(ParameterExpression param)
    {
        this._NewParameter = param;
    }
    
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return this._NewParameter;
    }
}
```

> 数据过滤方法定义

```C#
/// <summary>
/// 筛选数据执行
/// </summary>
/// <param name="func"></param>
private static void Do(Expression<Func<People, bool>> func)
{
    List<People> people = new List<People>()
    {
        new People(){Id=4,Name="123",Age=4},
        new People(){Id=5,Name="234",Age=5},
        new People(){Id=6,Name="345",Age=6},
    };

    List<People> peopleList = people.Where(func.Compile()).ToList();
}
```

> Expression拼接

```c#
Expression<Func<People, bool>> lambda1 = x => x.Age > 5;
Expression<Func<People, bool>> lambda2 = x => x.Id > 5;
Expression<Func<People, bool>> lambda3 = lambda1.And(lambda2);//且
Expression<Func<People, bool>> lambda4 = lambda1.Or(lambda2);//或
Expression<Func<People, bool>> lambda5 = lambda1.Not();//非
Do(lambda3);
Do(lambda4);
Do(lambda5);
```

## 五、ExpressionVisitor应用之ToSql

需求：实现ORM框架Expression映射成sql

> 自定义一个ConditionBuilderVisitor

继承自ExpressionVisitor，复写父类的方法，Expression解析过程中实现sql的拼接

```C#
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace MyExpression
{
    public class ConditionBuilderVisitor : ExpressionVisitor
    {
        private Stack<string> _StringStack = new Stack<string>();

        /// <summary>
        /// 返回拼装好的sql条件表达式
        /// </summary>
        /// <returns></returns>
        public string Condition()
        {
            string condition = string.Concat(this._StringStack.ToArray());
            this._StringStack.Clear();
            return condition;
        }

        /// <summary>
        /// 如果是二元表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node == null) throw new ArgumentNullException("BinaryExpression");

            this._StringStack.Push(")");
            base.Visit(node.Right);//解析右边
            this._StringStack.Push(" " + ToSqlOperator(node.NodeType) + " ");
            base.Visit(node.Left);//解析左边
            this._StringStack.Push("(");

            return node;
        }

        /// <summary>
        /// 解析属性
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node == null) throw new ArgumentNullException("MemberExpression");
            if (node.Expression is ConstantExpression)
            {
                var value1 = this.InvokeValue(node);
                var value2 = this.ReflectionValue(node);
                this._StringStack.Push("'" + value2 + "'");
            }
            else
            {
                this._StringStack.Push(" [" + node.Member.Name + "] ");
            }
            return node;
        }

        private string ToSqlOperator(ExpressionType type)
        {
            switch (type)
            {
                case (ExpressionType.AndAlso):
                case (ExpressionType.And):
                    return "AND";
                case (ExpressionType.OrElse):
                case (ExpressionType.Or):
                    return "OR";
                case (ExpressionType.Not):
                    return "NOT";
                case (ExpressionType.NotEqual):
                    return "<>";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case (ExpressionType.Equal):
                    return "=";
                default:
                    throw new Exception("不支持该方法");
            }
        }

        private object InvokeValue(MemberExpression member)
        {
            var objExp = Expression.Convert(member, typeof(object));//struct需要
            return Expression.Lambda<Func<object>>(objExp).Compile().Invoke();
        }

        private object ReflectionValue(MemberExpression member)
        {
            var obj = (member.Expression as ConstantExpression).Value;
            return (member.Member as FieldInfo).GetValue(obj);
        }

        /// <summary>
        /// 常量表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node == null) throw new ArgumentNullException("ConstantExpression");
            this._StringStack.Push("" + node.Value + "");
            return node;
        }
        /// <summary>
        /// 方法表达式
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m == null) throw new ArgumentNullException("MethodCallExpression");

            string format;
            switch (m.Method.Name)
            {
                case "StartsWith":
                    format = "({0} LIKE '{1}%')";
                    break;
                case "Contains":
                    format = "({0} LIKE '%{1}%')";
                    break;
                case "EndsWith":
                    format = "({0} LIKE '%{1}')";
                    break;
                default:
                    throw new NotSupportedException(m.NodeType + " is not supported!");
            }
            this.Visit(m.Object);
            this.Visit(m.Arguments[0]);
            string right = this._StringStack.Pop();
            string left = this._StringStack.Pop();
            this._StringStack.Push(String.Format(format, left, right));
            return m;
        }
    }
}
```

> ConstantSqlString泛型缓存缓存生成的sql

```C#
using System;
using System.Linq;

namespace MyExpression
{
    public class ConstantSqlString<T>
    {
        /// <summary>
        /// 泛型缓存，一个类型一个缓存
        /// </summary>
        private static string FindSql = null;

        /// <summary>
        /// 获取查询sql
        /// </summary>
        static ConstantSqlString()
        {
            Type type = typeof(T);
            FindSql = $"Select {string.Join(',', type.GetProperties().Select(c => $"[{c.Name}]").ToList())} from {type.Name}";
        }

        /// <summary>
        /// 获取查询sql+条件筛选
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string GetQuerySql(string exp)
        {
            return $"{FindSql} where {exp}";
        }
    }
}
```

> 普通多条件

```C#
Expression<Func<People, bool>> lambda = x => x.Age > 5
                                             && x.Id > 5
                                             && x.Name.StartsWith("1") //  like '1%'
                                             && x.Name.EndsWith("1") //  like '%1'
                                             && x.Name.Contains("1");//  like '%1%' 
ConditionBuilderVisitor vistor = new ConditionBuilderVisitor();
vistor.Visit(lambda);
string sql = ConstantSqlString<People>.GetQuerySql(vistor.Condition());
Console.WriteLine(sql);
```

> 外部参数变量

```C#
string name = "AAA";
Expression<Func<People, bool>> lambda = x => x.Age > 5 && x.Name == name || x.Id > 5;
ConditionBuilderVisitor vistor = new ConditionBuilderVisitor();
vistor.Visit(lambda);
string sql = ConstantSqlString<People>.GetQuerySql(vistor.Condition());
Console.WriteLine(sql);
```

> 内部常量多条件

```C#
Expression<Func<People, bool>> lambda = x => x.Age > 5 || (x.Name == "A" && x.Id > 5);
ConditionBuilderVisitor vistor = new ConditionBuilderVisitor();
vistor.Visit(lambda);
string sql = ConstantSqlString<People>.GetQuerySql(vistor.Condition());
Console.WriteLine(sql);
```

> 运行结果

```bash
Select [Age],[Name] from People where ((((( [Age]  > 5) AND ( [Id]  > 5)) AND ( [Name]  LIKE '1%')) AND ( [Name]  LIKE '%1')) AND ( [Name]  LIKE '%1%'))
Select [Age],[Name] from People where ((( [Age]  > 5) AND ( [Name]  = 'AAA')) OR ( [Id]  > 5))
Select [Age],[Name] from People where (( [Age]  > 5) OR (( [Name]  = A) AND ( [Id]  > 5)))
```

