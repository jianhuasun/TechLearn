# docsify详解

## 一、docsify入门

### 1、简介

docsify 可以快速帮你生成文档网站。不同于 GitBook、Hexo 的地方是它不会生成静态的 `.html` 文件，所有转换工作都是在运行时。如果你想要开始使用它，只需要创建一个 `index.html` 就可以开始编写文档并直接部署。

官网地址：[https://docsify.js.org/#/zh-cn/](https://docsify.js.org/#/zh-cn/)

### 2、特性

- 无需构建，写完文档直接发布
- 容易使用并且轻量 (压缩后 ~21kB)
- 智能的全文搜索
- 提供多套主题
- 丰富的 API
- 支持 Emoji
- 兼容 IE11
- 支持服务端渲染

## 二、快速开始

### 1、安装node环境

安装`docsify-cli`之前，我们需要安装`npm`包管理器，而安装了`node.js`就会自动安装`npm`。

#### （1）安装node

官网下载安装程序，双击下载的exe安装，下一步下一步直到完成。

官网地址：[https://nodejs.org/en/](https://nodejs.org/en/)

![image-20220224154043147](http://rc4mudd0q.hd-bkt.clouddn.com/202202241540214.png)

#### （2）验证安装

输入node -v，npm -v输出版本就是安装成功了

```bash
#验证node
node -v

#验证npm
npm -v
```

### 2、安装docsify-cli工具

推荐全局安装 `docsify-cli` 工具，可以方便地创建及在本地预览生成的文档。

```bash
#用npm安装全局工具
npm i docsify-cli -g
```

### 3、初始化项目

如果想在项目的 `.` 当前目录里写文档，直接通过 `init` 初始化项目

```bash
docsify init .
```

初始化成功后，可以看到 `.` 目录下创建的几个文件

- `index.html` 入口文件
- `README.md` 会做为主页内容渲染
- `.nojekyll` 用于阻止 GitHub Pages 忽略掉下划线开头的文件

![image-20220420213225360](http://rc4mudd0q.hd-bkt.clouddn.com/202204202132493.png)

如果不喜欢 npm 或者觉得安装工具太麻烦，我们可以直接手动创建一个 `index.html` 文件

```html
<!DOCTYPE html>
<html>
<head>
  <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
  <meta name="viewport" content="width=device-width,initial-scale=1">
  <meta charset="UTF-8">
  <link rel="stylesheet" href="//cdn.jsdelivr.net/npm/docsify/themes/vue.css">
</head>
<body>
  <div id="app"></div>
  <script>
    window.$docsify = {
      //...
    }
  </script>
  <script src="//cdn.jsdelivr.net/npm/docsify/lib/docsify.min.js"></script>
</body>
</html>
```

### 4、本地预览

通过运行 `docsify serve .` 启动一个本地服务器，这里的点就是当前目录的意思，可以方便地实时预览效果。默认访问地址 [http://localhost:3000](http://localhost:3000/) 。也可以用-p指定端口。

```bash
docsify serve -p 80 .
```

![image-20220420213417980](http://rc4mudd0q.hd-bkt.clouddn.com/202204202134029.png)

### 5、Loading 提示

初始化时会显示 `Loading...` 内容，你可以自定义提示信息。直接修改`index.html`文件。

```html
<div id="app">加载中</div>
```

如果更改了 `el` 的配置，需要将该元素加上 `data-app` 属性。

```html
<!-- index.html -->
  <div data-app id="main">加载中</div>

  <script>
    window.$docsify = {
      el: '#main'
    }
  </script>
```

## 三、页面配置

### 1、定制单页面

服务器启动，渲染的就是`README.md`的内容，要改变页面内容，只需要修改它，语法就是MarkDown语法，保存就会自动渲染刷新，不需要重启服务。

![image-20220420214446208](http://rc4mudd0q.hd-bkt.clouddn.com/202204202144263.png)

### 2、定制多页面

如果需要创建多个页面，或者需要多级路由的网站，在 docsify 里也能很容易的实现。例如创建一个 `guide.md` 文件，那么对应的路由就是 `/#/guide`。

假设你的目录结构如下：

```text
.
└── docs
    ├── README.md
    ├── guide.md
    └── zh-cn
        ├── README.md
        └── guide.md
```

那么对应的访问页面将是：

```tex
docs/README.md        => http://domain.com
docs/guide.md         => http://domain.com/guide
docs/zh-cn/README.md  => http://domain.com/zh-cn/
docs/zh-cn/guide.md   => http://domain.com/zh-cn/guide
```

#### （1）定制侧边栏

> 首先修改index.html配置文件，配置`loadSidebar` 选项

```html
<script>
  window.$docsify = {
    loadSidebar: true
  }
</script>
```

> 创建 `_sidebar.md` 文件，内容如下

需要创建 `.nojekyll` 命名的空文件，阻止 GitHub Pages 忽略命名是下划线开头的文件。

```markdown
* [首页](first/first)
* [指南](guide/guide)
```

> 创建文件夹first，里面添加first.md，再创建文件夹guide，文件guide.md

![image-20220420222713983](http://rc4mudd0q.hd-bkt.clouddn.com/202204202227044.png)

> 默认显示主页，点击各页面显示各页面

![image-20220420222820984](http://rc4mudd0q.hd-bkt.clouddn.com/202204202228056.png)





![image-20220420222909923](http://rc4mudd0q.hd-bkt.clouddn.com/202204202229966.png)

#### （2）嵌套的侧边栏

你可能想要浏览到一个目录时，只显示这个目录自己的侧边栏，这可以通过在每个文件夹中添加一个 `_sidebar.md` 文件来实现。`_sidebar.md` 的加载逻辑是从每层目录下获取文件，如果当前目录不存在该文件则回退到上一级目录。

> 在first文件夹添加`_sidebar.md` 文件和其他两个页面

```markdown
* [首页](first/first)
* [首页1](first/first1)
* [首页2](first/first2)
```

![image-20220423151325087](http://rc4mudd0q.hd-bkt.clouddn.com/202204231513170.png)

> 默认显示主目录，点击首页进入首页文件夹目录，点击指南，还是显示主目录。

![image-20220423151438485](http://rc4mudd0q.hd-bkt.clouddn.com/202204231514558.png)

![image-20220423151517197](http://rc4mudd0q.hd-bkt.clouddn.com/202204231515277.png)





![image-20220423151927526](http://rc4mudd0q.hd-bkt.clouddn.com/202204231519570.png)







#### （3）页面标题显示设置

一个页面的 `title` 标签是由侧边栏中选中条目的名称所生成的。你可以修改`_sidebar.md`在文件名后面指定页面标题。

```markdown
* [首页](first/first)
* [指南](guide/guide "最牛逼的指南")
```

![image-20220423152845850](http://rc4mudd0q.hd-bkt.clouddn.com/202204231528918.png)

#### （4）页面显示目录层级设置

> 首页内容如下，内容有4层目录级别

![image-20220423153608214](http://rc4mudd0q.hd-bkt.clouddn.com/202204231536323.png)

> 在`index.html`种配置参数`subMaxLevel`为2

```html
<script>
  window.$docsify = {
    loadSidebar: true,
    subMaxLevel: 2
  }
</script>
```

> 默认显示主目录，点击首页进入首页文件夹目录，显示两层内容级别

![image-20220423154128769](http://rc4mudd0q.hd-bkt.clouddn.com/202204231541842.png)

![image-20220423154107689](http://rc4mudd0q.hd-bkt.clouddn.com/202204231541751.png)

> 在`index.html`种配置参数`subMaxLevel`为4，首页显示全部4层级别目录

![image-20220423154327471](http://rc4mudd0q.hd-bkt.clouddn.com/202204231543558.png)

#### （5）设置不显示目录

设置了 `subMaxLevel` 时，默认情况下每个标题都会自动添加到目录中。如果你想忽略特定的标题，可以修改`_sidebar.md`给它添加 `<!-- {docsify-ignore} -->`，要忽略特定页面上的所有标题，你可以在页面的第一个标题上使用 `<!-- {docsify-ignore-all} -->`。

```markdown
# 忽略全部标题 <!-- {docsify-ignore-all} -->
## 忽略部分标题 <!-- {docsify-ignore} -->
```

### 3、定制导航栏

#### （1）HTML创建导航栏

>  直接在`index.html`加上导航标签

```html
<body>
  <nav>
    <a href="#/first/first">首页</a>
    <a href="#/guide/guide">指南</a>
  </nav>
  <div id="app"></div>
  <script>
    window.$docsify = {
      loadSidebar: true,
      subMaxLevel: 4
    }
  </script>
  <!-- Docsify v4 -->
  <script src="//cdn.jsdelivr.net/npm/docsify@4"></script>
</body>
```

> 导航点击效果和侧边栏效果差不多，跳转到对应页面

![image-20220423160918576](http://rc4mudd0q.hd-bkt.clouddn.com/202204231609652.png)

#### （2）通过配置文件来配置

> 在`index.html`配置导航栏参数`loadNavbar`

```html
<body>  
  <div id="app"></div>
  <script>
    window.$docsify = {
      loadSidebar: true,
      subMaxLevel: 4,
      loadNavbar: true
    }
  </script>
  <!-- Docsify v4 -->
  <script src="//cdn.jsdelivr.net/npm/docsify@4"></script>
</body>
```

> 添加配置文件`_navbar.md`来配置导航栏，内容和侧边栏配置文件是一样的，效果同上

```markdown
* [首页](first/first)
* [指南](guide/guide "最牛逼的指南")
```

#### （3）导航嵌套

如果导航内容过多，可以写成嵌套的列表，会被渲染成下拉列表的形式。

> 配置文件`_navbar.md`如下

```markdown
* 首页

  * [首页](first/first)
  * [首页1](first/first1)
  * [首页2](first/first2)


* 指南
  * [指南](guide/guide)
```

> 显示效果如下

![image-20220423162536303](http://rc4mudd0q.hd-bkt.clouddn.com/202204231625362.png)

#### （4）导航中用emoji表情

> 在`index.html`引入emoji包

```html
<body>  
  <div id="app"></div>
  <script>
    window.$docsify = {
      loadSidebar: true,
      subMaxLevel: 4,
      loadNavbar: true
    }
  </script>
  <!-- Docsify v4 -->
  <script src="//cdn.jsdelivr.net/npm/docsify@4"></script>
  <script src="//cdn.jsdelivr.net/npm/docsify/lib/plugins/emoji.min.js"></script>
</body>
```

> 在导航文件直接使用，表情可参考网站[https://www.emojiall.com/zh-hans](https://www.emojiall.com/zh-hans)

```markdown
* 首页

  * [:cn:首页](first/first)
  * [:us:首页1](first/first1)
  * [:ru:首页2](first/first2)


* 指南
  * [指南](guide/guide)
```

> 效果如下

![image-20220423164413478](http://rc4mudd0q.hd-bkt.clouddn.com/202204231644601.png)



### 4、定制封面

#### （1）基本用法

> 在`index.html`中配置参数 `coverpage`开启封面。通常封面和首页是同时出现的，设置了`onlyCover=true`之后封面就独立成封面。

```html
<body>  
  <div id="app"></div>
  <script>
    window.$docsify = {
      loadSidebar: true,
      subMaxLevel: 4,
      loadNavbar: true,
      coverpage: true,
      onlyCover: true 
    }
  </script>
  <!-- Docsify v4 -->
  <script src="//cdn.jsdelivr.net/npm/docsify@4"></script>
  <script src="//cdn.jsdelivr.net/npm/docsify/lib/plugins/emoji.min.js"></script>
</body>
```

> 添加配置文件`_coverpage.md` 来配置封面，添加logo文件夹media里面放logo图片logo.jpg

```markdown
![logo](media/logo.jpg)

# 李宥的个人网站

> 分享技术，热爱技术

- 指尖有改变世界的力量
- 开源成就美好
- 书山有路勤为径

[GitHub](https://github.com/bluecusliyou)
[Gitee](https://gitee.com/bluecusliyou)
[Get Started](first/first)
```

> 效果如下

![image-20220423203237518](http://rc4mudd0q.hd-bkt.clouddn.com/202204232032756.png)

#### （2）自定义背景

目前的背景是随机生成的渐变色，我们自定义背景色或者背景图。在文档末尾用添加图片的 Markdown 语法设置背景。

```markdown
![logo](media/logo.jpg)

# 李宥的个人网站

> 分享技术，热爱技术

- 指尖有改变世界的力量
- 开源成就美好
- 书山有路勤为径

[GitHub](https://github.com/bluecusliyou)
[Gitee](https://gitee.com/bluecusliyou)
[Get Started](first/first)

<!-- 背景图片 -->

![](media/bg.png)

<!-- 背景色 -->

![color](#f0f0f0)
```

## 四、定制化

### 1、更换主题

> 如果我们要更换主题，只需要替换`index.html`中 css 样式文件即可。

```html
<link rel="stylesheet" href="//cdn.jsdelivr.net/npm/docsify/themes/vue.css">
<link rel="stylesheet" href="//cdn.jsdelivr.net/npm/docsify/themes/buble.css">
<link rel="stylesheet" href="//cdn.jsdelivr.net/npm/docsify/themes/dark.css">
<link rel="stylesheet" href="//cdn.jsdelivr.net/npm/docsify/themes/pure.css">
<link rel="stylesheet" href="//cdn.jsdelivr.net/npm/docsify/themes/dolphin.css">
```

> 这里我们更换成dark样式

```html
<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="UTF-8">
  <title>Document</title>
  <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
  <meta name="description" content="Description">
  <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0">
  <link rel="stylesheet" href="//cdn.jsdelivr.net/npm/docsify@4/lib/themes/dark.css">
</head>

<body>  
  <div id="app"></div>
  <script>
    window.$docsify = {
      loadSidebar: true,
      subMaxLevel: 4,
      loadNavbar: true,
      coverpage: true,
      onlyCover: true
    }
  </script>
  <!-- Docsify v4 -->
  <script src="//cdn.jsdelivr.net/npm/docsify@4"></script>
  <script src="//cdn.jsdelivr.net/npm/docsify/lib/plugins/emoji.min.js"></script>
</body>

</html>
```

> 效果如下

![image-20220427074756836](http://rc4mudd0q.hd-bkt.clouddn.com/202204270748961.png)

### 2、搜索插件

全文搜索插件会根据当前页面上的超链接获取文档内容，在 `localStorage` 内建立文档索引。默认过期时间为一天。

> 在`index.html`配置搜索插件

```html
<script>
  window.$docsify = {
    search: 'auto', // 默认值
    // 完整配置参数
	search: {
        maxAge: 86400000,//过期时间,单位毫秒,默认一天
        paths: [], // or 'auto'
        placeholder: '请输入搜索关键字',
        noData: '没有搜到呦!',
        depth:2
	}
  }
```

> 在`index.html`添加js

```html
<script src="//cdn.jsdelivr.net/npm/docsify/lib/plugins/search.min.js"></script>
```

> 效果如下

![image-20220428220220569](http://rc4mudd0q.hd-bkt.clouddn.com/202204282202714.png)

### 3、剪贴板插件

在所有的代码块上添加一个简单的`Click to copy`按钮来允许用户从你的文档中轻易地复制代码。

> 只需要在`index.html`中添加js

```html
<script src="//cdn.jsdelivr.net/npm/docsify-copy-code/dist/docsify-copy-code.min.js"></script>
```

> 效果如下

![image-20220428223022825](http://rc4mudd0q.hd-bkt.clouddn.com/202204282230900.png)

### 4、分页导航插件

> 只需要在`index.html`中添加js

```html
<script src="//cdn.jsdelivr.net/npm/docsify-pagination/dist/docsify-pagination.min.js"></script>
```

> 效果如下

![image-20220428223317942](http://rc4mudd0q.hd-bkt.clouddn.com/202204282233857.png)



### 5、字数统计插件

> 在`index.html`添加配置

```html
window.$docsify = {
  count:{
    countable:true,
    fontsize:'0.9em',
    color:'rgb(90,90,90)',
    language:'chinese'
  }
}
```

> 在`index.html`添加js

```html
<script src="//unpkg.com/docsify-count/dist/countable.js"></script>
```

> 效果如下

![image-20220428224128755](http://rc4mudd0q.hd-bkt.clouddn.com/202204282241860.png)



## 五、Gitee Pages部署

### 1、windows安装git，管理页面

window上主要是日常博客的编写，然后用git来管理，上传到gitee后，可以配置生成GiteePages。具体的git操作和配置可以参考[git详解文章](https://blog.csdn.net/liyou123456789/article/details/121411053)，这里创建了一个gitee仓库，专门用来做个人网站。下面红框框出来的是docsify生成的几个配置文件。

![image-20220226154353324](http://rc4mudd0q.hd-bkt.clouddn.com/202202261544599.png)



### 2、Gitee配置Gitee Pages

第一次配置需要实名认证，上传身份证正反面，手持身份证照片。

部署选择你要部署的分支，部署的目录就是docsify对应仓库中的目录，我这边是整个仓库作为docsify目录，建议强制使用https勾选，然后就可以启动。

![image-20220226154808159](http://rc4mudd0q.hd-bkt.clouddn.com/202202261548225.png)

![image-20220226154932118](http://rc4mudd0q.hd-bkt.clouddn.com/202202261549171.png)

### 3、页面效果请参考

生成的giteePages地址是[https://bluecusliyou.gitee.io/techlearn](https://bluecusliyou.gitee.io/techlearn)

![image-20220429215828321](http://rc4mudd0q.hd-bkt.clouddn.com/202204292158465.png)

### 4、页面修改更新

如果页面内容有修改更新到仓库了，可以点击更新个人页面

![image-20220226155356585](http://rc4mudd0q.hd-bkt.clouddn.com/202202261553662.png)



