# WPF 数据验证演示

这是一个基于WPF的数据验证演示应用程序，展示了在MVVM架构下多种数据验证的实现方法。

## 项目简介

本项目演示了在WPF应用程序中如何实现数据验证，主要包含两个验证示例：
- 用户注册信息验证
- 产品信息验证

通过这两个示例，展示了多种验证技术和方法，包括：
- 基于`INotifyDataErrorInfo`接口的验证
- 数据注解验证
- 自定义验证规则
- 实时验证与提交验证
- 表单级别与字段级别的错误提示

## 技术特点

- 采用MVVM(Model-View-ViewModel)架构模式
- 使用C# 8.0及.NET 8.0特性
- 响应式UI设计
- 松耦合的验证逻辑
- 可扩展的验证框架

## 项目结构

```
WPF_DataValidation
├── Models/                  # 数据模型
│   ├── UserRegistration.cs  # 用户注册模型
│   └── Product.cs           # 产品模型
├── ViewModels/              # 视图模型
│   ├── MainViewModel.cs     # 主视图模型
│   ├── UserRegistrationViewModel.cs  # 用户注册视图模型
│   └── ProductViewModel.cs  # 产品视图模型
├── Views/                   # 视图
│   ├── MainView.xaml        # 主视图
│   ├── UserRegistrationView.xaml  # 用户注册视图
│   └── ProductView.xaml     # 产品视图
└── Infrastructure/          # 基础设施类
```

## 功能示例

### 用户注册验证

演示了常见的用户注册表单验证：
- 用户名验证（必填、长度限制、格式要求）
- 密码验证（强度检查、确认匹配）
- 电子邮件验证（格式检查）
- 手机号码验证
- 出生日期验证（年龄限制）

### 产品信息验证

演示了产品信息录入的验证：
- 产品名称验证
- 价格验证（范围检查、格式验证）
- 库存验证（非负数）
- 产品类别验证
- 上市日期验证

## 运行要求

- Windows操作系统
- .NET 8.0 SDK
- Visual Studio 2022或更高版本

## 如何使用

1. 克隆或下载此项目到本地
2. 使用Visual Studio 2022打开`WPF_DataValidation.sln`解决方案
3. 构建并运行项目
4. 在应用程序中，可以通过顶部的导航按钮切换不同的验证示例

## 学习要点

通过此演示项目，您可以学习：
- 如何在WPF中实现数据验证
- MVVM模式下的验证最佳实践
- 如何设计可重用的验证组件
- 如何提供友好的用户验证反馈
- 如何将验证逻辑与UI分离

## 许可证

MIT 