using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WPFValidationDemo.Infrastructure
{
    /// <summary>
    /// 支持数据验证的ViewModel基类，实现INotifyDataErrorInfo接口
    /// </summary>
    public class ValidationViewModelBase : ObservableObject, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        
        /// <summary>
        /// 指示实体是否有验证错误
        /// </summary>
        public bool HasErrors => _errors.Any();
        
        /// <summary>
        /// 验证错误发生改变时触发
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        
        /// <summary>
        /// 获取指定属性的验证错误
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性的错误集合</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
                return null;
                
            return _errors[propertyName];
        }
        
        /// <summary>
        /// 触发ErrorsChanged事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }
        
        /// <summary>
        /// 添加错误
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="error">错误信息</param>
        protected void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();
                
            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }
        
        /// <summary>
        /// 清除指定属性的所有错误
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected void ClearErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        
        /// <summary>
        /// 清除所有错误
        /// </summary>
        protected void ClearAllErrors()
        {
            var propNames = _errors.Keys.ToList();
            _errors.Clear();
            
            foreach (var propName in propNames)
            {
                OnErrorsChanged(propName);
            }
        }
        
        /// <summary>
        /// 设置属性值，并在值变化时执行验证
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="field">字段引用</param>
        /// <param name="value">新值</param>
        /// <param name="validateFunc">验证函数</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>如果值发生变化则返回true</returns>
        protected bool SetProperty<T>(ref T field, T value, Action validateFunc, [CallerMemberName] string propertyName = null)
        {
            if (SetProperty(ref field, value, propertyName))
            {
                validateFunc?.Invoke();
                return true;
            }
            
            return false;
        }
    }
} 