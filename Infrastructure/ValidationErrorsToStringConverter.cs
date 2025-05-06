using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFValidationDemo.Infrastructure
{
    /// <summary>
    /// 将ValidationErrors集合安全地转换为字符串的转换器
    /// </summary>
    public class ValidationErrorsToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // 检查输入值
            if (values == null || values.Length == 0 || values[0] == null)
            {
                return null;
            }

            // 获取验证错误集合
            var errors = values[0] as IEnumerable;
            if (errors == null)
            {
                return null;
            }

            // 收集所有错误消息
            var errorMessages = new List<string>();
            foreach (var error in errors)
            {
                if (error is ValidationError validationError && validationError.ErrorContent != null)
                {
                    errorMessages.Add(validationError.ErrorContent.ToString());
                }
            }

            // 返回格式化的错误消息
            if (errorMessages.Count > 0)
            {
                return string.Join(Environment.NewLine, errorMessages);
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack方法未实现");
        }
    }
} 