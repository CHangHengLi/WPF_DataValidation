using System;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using WPFValidationDemo.Infrastructure;
using WPFValidationDemo.Models;
using System.ComponentModel;
using System.Collections.Generic;

namespace WPFValidationDemo.ViewModels
{
    /// <summary>
    /// 用户注册ViewModel，包装UserRegistration模型
    /// </summary>
    public class UserRegistrationViewModel : ValidationViewModelBase
    {
        private readonly UserRegistration _userRegistration;
        private string _registrationResult;
        
        // 密码框重置事件
        public event EventHandler PasswordReset;
        
        // 电子邮箱验证正则表达式
        private static readonly Regex EmailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

        public UserRegistrationViewModel()
        {
            _userRegistration = new UserRegistration();
            RegisterCommand = new RelayCommand(ExecuteRegister, CanRegister);
            ResetCommand = new RelayCommand(ExecuteReset);
            
            // 注册属性变化事件以触发验证
            PropertyChanged += UserRegistrationViewModel_PropertyChanged;
        }
        
        private void UserRegistrationViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // 当属性变化时执行验证
            if (e.PropertyName == nameof(Username))
            {
                ValidateUsername();
            }
            else if (e.PropertyName == nameof(Email))
            {
                ValidateEmail();
            }
            else if (e.PropertyName == nameof(Password) || e.PropertyName == nameof(ConfirmPassword))
            {
                ValidatePassword();
                ValidateConfirmPassword();
            }
            else if (e.PropertyName == nameof(AcceptTerms) || e.PropertyName == nameof(BirthDate))
            {
                ValidateTerms();
            }
            
            // 重新验证命令可用性
            CommandManager.InvalidateRequerySuggested();
        }
        
        #region 验证方法
        
        private void ValidateUsername()
        {
            ClearErrors(nameof(Username));
            
            if (string.IsNullOrEmpty(Username))
            {
                AddError(nameof(Username), "用户名不能为空");
            }
            else if (Username.Length < 4)
            {
                AddError(nameof(Username), "用户名长度不能少于4个字符");
            }
            else if (Username.Length > 20)
            {
                AddError(nameof(Username), "用户名长度不能超过20个字符");
            }
        }
        
        private void ValidatePassword()
        {
            ClearErrors(nameof(Password));
            
            if (string.IsNullOrEmpty(Password))
            {
                AddError(nameof(Password), "密码不能为空");
            }
            else if (Password.Length < 6)
            {
                AddError(nameof(Password), "密码长度不能少于6个字符");
            }
        }
        
        private void ValidateConfirmPassword()
        {
            ClearErrors(nameof(ConfirmPassword));
            
            if (string.IsNullOrEmpty(ConfirmPassword) && !string.IsNullOrEmpty(Password))
            {
                AddError(nameof(ConfirmPassword), "请确认密码");
            }
            else if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword) &&
                     Password != ConfirmPassword)
            {
                AddError(nameof(ConfirmPassword), "两次输入的密码不一致");
            }
        }
        
        private void ValidateEmail()
        {
            ClearErrors(nameof(Email));
            
            if (string.IsNullOrEmpty(Email))
            {
                AddError(nameof(Email), "邮箱不能为空");
            }
            else if (!EmailRegex.IsMatch(Email))
            {
                AddError(nameof(Email), "邮箱格式不正确");
            }
        }
        
        private void ValidateTerms()
        {
            ClearErrors(nameof(AcceptTerms));
            
            if (!AcceptTerms)
            {
                // 未成年用户强制接受条款
                if (BirthDate > DateTime.Today.AddYears(-18))
                {
                    AddError(nameof(AcceptTerms), "未满18岁用户必须接受服务条款和家长同意声明");
                }
                // 成年用户必须接受服务条款
                else
                {
                    AddError(nameof(AcceptTerms), "请接受服务条款才能注册");
                }
            }
        }
        
        // 执行所有验证
        private void ValidateAll()
        {
            ValidateUsername();
            ValidatePassword();
            ValidateConfirmPassword();
            ValidateEmail();
            ValidateTerms();
        }
        
        #endregion

        #region 属性包装

        public string Username
        {
            get => _userRegistration.Username;
            set
            {
                if (_userRegistration.Username != value)
                {
                    _userRegistration.Username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get => _userRegistration.Password;
            set
            {
                if (_userRegistration.Password != value)
                {
                    _userRegistration.Password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string ConfirmPassword
        {
            get => _userRegistration.ConfirmPassword;
            set
            {
                if (_userRegistration.ConfirmPassword != value)
                {
                    _userRegistration.ConfirmPassword = value;
                    OnPropertyChanged(nameof(ConfirmPassword));
                }
            }
        }

        public string Email
        {
            get => _userRegistration.Email;
            set
            {
                if (_userRegistration.Email != value)
                {
                    _userRegistration.Email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public DateTime BirthDate
        {
            get => _userRegistration.BirthDate;
            set
            {
                if (_userRegistration.BirthDate != value)
                {
                    _userRegistration.BirthDate = value;
                    OnPropertyChanged(nameof(BirthDate));
                }
            }
        }

        public bool AcceptTerms
        {
            get => _userRegistration.AcceptTerms;
            set
            {
                if (_userRegistration.AcceptTerms != value)
                {
                    _userRegistration.AcceptTerms = value;
                    OnPropertyChanged(nameof(AcceptTerms));
                }
            }
        }

        public string RegistrationResult
        {
            get => _registrationResult;
            set => SetProperty(ref _registrationResult, value);
        }

        #endregion

        #region 命令

        public ICommand RegisterCommand { get; }
        public ICommand ResetCommand { get; }

        private bool CanRegister(object parameter)
        {
            // 执行所有验证
            ValidateAll();
            
            // 检查是否有任何错误
            return !HasErrors;
        }

        private void ExecuteRegister(object parameter)
        {
            // 在实际应用中，这里会调用服务来执行用户注册逻辑
            RegistrationResult = $"注册成功！用户名: {Username}, 邮箱: {Email}";
            MessageBox.Show("注册成功！", "用户注册", MessageBoxButton.OK, MessageBoxImage.Information);
            
            // 注册成功后可以清除表单
            ResetForm();
        }

        private void ExecuteReset(object parameter)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            Username = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            Email = string.Empty;
            BirthDate = DateTime.Today.AddYears(-18);
            AcceptTerms = false;
            RegistrationResult = string.Empty;
            
            // 清除所有验证错误
            ClearAllErrors();
            
            // 触发密码框重置事件
            PasswordReset?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
} 