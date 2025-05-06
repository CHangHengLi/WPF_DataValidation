using System;
using System.Windows;
using System.Windows.Controls;
using WPFValidationDemo.ViewModels;
using System.ComponentModel;
using System.Collections;

namespace WPFValidationDemo.Views
{
    /// <summary>
    /// UserRegistrationView.xaml 的交互逻辑
    /// </summary>
    public partial class UserRegistrationView : UserControl
    {
        public UserRegistrationView()
        {
            InitializeComponent();
            
            // 注册事件
            this.Loaded += UserRegistrationView_Loaded;
            this.DataContextChanged += UserRegistrationView_DataContextChanged;
        }

        private void UserRegistrationView_Loaded(object sender, RoutedEventArgs e)
        {
            // 绑定密码框事件
            if (PasswordBox != null)
                PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
                
            if (ConfirmPasswordBox != null)
                ConfirmPasswordBox.PasswordChanged += ConfirmPasswordBox_PasswordChanged;
            
            // 清空密码框
            if (PasswordBox != null)
                PasswordBox.Password = string.Empty;
                
            if (ConfirmPasswordBox != null)
                ConfirmPasswordBox.Password = string.Empty;
            
            // 页面加载时订阅事件
            if (DataContext is UserRegistrationViewModel viewModel)
            {
                viewModel.PasswordReset += ViewModel_PasswordReset;
                viewModel.ErrorsChanged += ViewModel_ErrorsChanged;
                
                // 初始验证
                UpdatePasswordErrorDisplay(viewModel);
                UpdateErrorDisplay(UsernameErrorText, viewModel, "Username");
                UpdateErrorDisplay(EmailErrorText, viewModel, "Email");
                UpdateErrorDisplay(BirthDateErrorText, viewModel, "BirthDate");
                UpdateErrorDisplay(TermsErrorText, viewModel, "AcceptTerms");
            }
        }
        
        private void UserRegistrationView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is UserRegistrationViewModel oldViewModel)
            {
                oldViewModel.PasswordReset -= ViewModel_PasswordReset;
                oldViewModel.ErrorsChanged -= ViewModel_ErrorsChanged;
            }
            
            if (e.NewValue is UserRegistrationViewModel newViewModel)
            {
                newViewModel.PasswordReset += ViewModel_PasswordReset;
                newViewModel.ErrorsChanged += ViewModel_ErrorsChanged;
                
                // 初始验证
                UpdatePasswordErrorDisplay(newViewModel);
                UpdateErrorDisplay(UsernameErrorText, newViewModel, "Username");
                UpdateErrorDisplay(EmailErrorText, newViewModel, "Email");
                UpdateErrorDisplay(BirthDateErrorText, newViewModel, "BirthDate");
                UpdateErrorDisplay(TermsErrorText, newViewModel, "AcceptTerms");
            }
        }
        
        private void ViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            if (DataContext is UserRegistrationViewModel viewModel)
            {
                // 根据属性名称更新相应的错误显示
                if (e.PropertyName == "Password" || e.PropertyName == "ConfirmPassword" || e.PropertyName == null)
                {
                    UpdatePasswordErrorDisplay(viewModel);
                }
                
                if (e.PropertyName == "Username" || e.PropertyName == null)
                {
                    UpdateErrorDisplay(UsernameErrorText, viewModel, "Username");
                }
                
                if (e.PropertyName == "Email" || e.PropertyName == null)
                {
                    UpdateErrorDisplay(EmailErrorText, viewModel, "Email");
                }
                
                if (e.PropertyName == "BirthDate" || e.PropertyName == null)
                {
                    UpdateErrorDisplay(BirthDateErrorText, viewModel, "BirthDate");
                }
                
                if (e.PropertyName == "AcceptTerms" || e.PropertyName == null)
                {
                    UpdateErrorDisplay(TermsErrorText, viewModel, "AcceptTerms");
                }
            }
        }
        
        private void UpdatePasswordErrorDisplay(UserRegistrationViewModel viewModel)
        {
            // 更新密码错误显示
            if (PasswordErrorText != null)
            {
                var passwordErrors = viewModel.GetErrors("Password");
                if (passwordErrors != null)
                {
                    string errorMessage = null;
                    foreach (var error in passwordErrors)
                    {
                        errorMessage = error.ToString();
                        break;
                    }
                    
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        PasswordErrorText.Text = errorMessage;
                        PasswordErrorText.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        PasswordErrorText.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    PasswordErrorText.Visibility = Visibility.Collapsed;
                }
            }
            
            // 更新确认密码错误显示
            if (ConfirmPasswordErrorText != null)
            {
                var confirmPasswordErrors = viewModel.GetErrors("ConfirmPassword");
                if (confirmPasswordErrors != null)
                {
                    string errorMessage = null;
                    foreach (var error in confirmPasswordErrors)
                    {
                        errorMessage = error.ToString();
                        break;
                    }
                    
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        ConfirmPasswordErrorText.Text = errorMessage;
                        ConfirmPasswordErrorText.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ConfirmPasswordErrorText.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    ConfirmPasswordErrorText.Visibility = Visibility.Collapsed;
                }
            }
        }
        
        private void ViewModel_PasswordReset(object sender, EventArgs e)
        {
            // 当ViewModel触发密码重置事件时，清空两个密码框
            if (PasswordBox != null)
                PasswordBox.Password = string.Empty;
                
            if (ConfirmPasswordBox != null)
                ConfirmPasswordBox.Password = string.Empty;
                
            // 清除错误显示
            if (PasswordErrorText != null)
                PasswordErrorText.Visibility = Visibility.Collapsed;
                
            if (ConfirmPasswordErrorText != null)
                ConfirmPasswordErrorText.Visibility = Visibility.Collapsed;
                
            if (UsernameErrorText != null)
                UsernameErrorText.Visibility = Visibility.Collapsed;
                
            if (EmailErrorText != null)
                EmailErrorText.Visibility = Visibility.Collapsed;
                
            if (BirthDateErrorText != null)
                BirthDateErrorText.Visibility = Visibility.Collapsed;
                
            if (TermsErrorText != null)
                TermsErrorText.Visibility = Visibility.Collapsed;
        }
        
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserRegistrationViewModel viewModel)
            {
                viewModel.Password = PasswordBox.Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserRegistrationViewModel viewModel)
            {
                viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
            }
        }

        // 通用的错误显示更新方法
        private void UpdateErrorDisplay(TextBlock errorTextBlock, UserRegistrationViewModel viewModel, string propertyName)
        {
            if (errorTextBlock != null)
            {
                var errors = viewModel.GetErrors(propertyName);
                if (errors != null)
                {
                    string errorMessage = null;
                    foreach (var error in errors)
                    {
                        errorMessage = error.ToString();
                        break;
                    }
                    
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        errorTextBlock.Text = errorMessage;
                        errorTextBlock.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        errorTextBlock.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    errorTextBlock.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
} 