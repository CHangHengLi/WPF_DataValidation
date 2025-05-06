using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace WPFValidationDemo.Models
{
    /// <summary>
    /// 用户注册模型，使用IDataErrorInfo接口实现验证
    /// </summary>
    public class UserRegistration : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _username;
        private string _password;
        private string _confirmPassword;
        private string _email;
        private DateTime _birthDate = DateTime.Today.AddYears(-18);
        private bool _acceptTerms;

        #region INotifyPropertyChanged实现

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region 属性

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    // 密码变化可能影响确认密码的验证
                    OnPropertyChanged(nameof(ConfirmPassword));
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    OnPropertyChanged(nameof(ConfirmPassword));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate != value)
                {
                    _birthDate = value;
                    OnPropertyChanged(nameof(BirthDate));
                    // 年龄变化可能影响条款同意的验证
                    OnPropertyChanged(nameof(AcceptTerms));
                }
            }
        }

        public bool AcceptTerms
        {
            get => _acceptTerms;
            set
            {
                if (_acceptTerms != value)
                {
                    _acceptTerms = value;
                    OnPropertyChanged(nameof(AcceptTerms));
                }
            }
        }

        #endregion

        #region IDataErrorInfo实现

        public string Error => null; // 不使用整体错误

        public string this[string columnName]
        {
            get
            {
                string error = null;

                switch (columnName)
                {
                    case nameof(Username):
                        if (string.IsNullOrEmpty(Username))
                            error = "用户名不能为空";
                        else if (Username.Length < 4)
                            error = "用户名长度不能少于4个字符";
                        else if (Username.Length > 20)
                            error = "用户名长度不能超过20个字符";
                        break;

                    case nameof(Password):
                        if (string.IsNullOrEmpty(Password))
                            error = "密码不能为空";
                        else if (Password.Length < 6)
                            error = "密码长度不能少于6个字符";
                        break;

                    case nameof(ConfirmPassword):
                        // 跨字段验证 - 密码确认
                        if (string.IsNullOrEmpty(ConfirmPassword))
                            error = "请确认密码";
                        else if (!string.Equals(Password, ConfirmPassword))
                            error = "两次输入的密码不一致";
                        break;

                    case nameof(Email):
                        if (string.IsNullOrEmpty(Email))
                            error = "邮箱不能为空";
                        else if (!Regex.IsMatch(Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                            error = "邮箱格式不正确";
                        break;

                    case nameof(BirthDate):
                        if (BirthDate > DateTime.Today)
                            error = "出生日期不能是未来日期";
                        else if (BirthDate < DateTime.Today.AddYears(-120))
                            error = "出生日期无效";
                        break;

                    case nameof(AcceptTerms):
                        // 未成年用户强制接受条款
                        if (BirthDate > DateTime.Today.AddYears(-18) && !AcceptTerms)
                            error = "未满18岁用户必须接受服务条款和家长同意声明";
                        // 成年用户必须接受服务条款
                        else if (!AcceptTerms)
                            error = "请接受服务条款才能注册";
                        break;
                }

                return error;
            }
        }

        #endregion

        /// <summary>
        /// 验证整个对象
        /// </summary>
        public bool IsValid()
        {
            string[] properties = { 
                nameof(Username), 
                nameof(Password),
                nameof(ConfirmPassword), 
                nameof(Email),
                nameof(BirthDate), 
                nameof(AcceptTerms) 
            };

            foreach (var property in properties)
            {
                if (!string.IsNullOrEmpty(this[property]))
                    return false;
            }

            return true;
        }
    }
} 