using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WPFValidationDemo.Models
{
    /// <summary>
    /// 产品模型，使用数据注解特性进行验证
    /// </summary>
    public class Product : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _name;
        private decimal _price;
        private DateTime _releaseDate = DateTime.Today;
        private int _stockLevel;
        private string _description;
        private string _category;

        #region INotifyPropertyChanged实现

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region 属性

        [Required(ErrorMessage = "产品名称不能为空")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "产品名称必须在3-50个字符之间")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        [Required(ErrorMessage = "产品价格不能为空")]
        [Range(0.01, 100000, ErrorMessage = "产品价格必须在0.01到100,000之间")]
        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        [Required(ErrorMessage = "发布日期不能为空")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate
        {
            get => _releaseDate;
            set
            {
                if (_releaseDate != value)
                {
                    _releaseDate = value;
                    OnPropertyChanged(nameof(ReleaseDate));
                }
            }
        }

        [Range(0, int.MaxValue, ErrorMessage = "库存数量不能为负数")]
        public int StockLevel
        {
            get => _stockLevel;
            set
            {
                if (_stockLevel != value)
                {
                    _stockLevel = value;
                    OnPropertyChanged(nameof(StockLevel));
                }
            }
        }

        [StringLength(500, ErrorMessage = "描述不能超过500个字符")]
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        [Required(ErrorMessage = "类别不能为空")]
        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }

        #endregion

        #region IDataErrorInfo实现，使用数据注解验证

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                var property = GetType().GetProperty(columnName);
                if (property == null) return null;
                
                var value = property.GetValue(this);
                var validationContext = new ValidationContext(this)
                {
                    MemberName = columnName
                };

                // 执行验证
                if (Validator.TryValidateProperty(value, validationContext, validationResults))
                    return null;

                // 返回第一个错误消息
                return validationResults.FirstOrDefault()?.ErrorMessage;
            }
        }

        #endregion

        /// <summary>
        /// 验证整个对象
        /// </summary>
        public bool IsValid()
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this);
            
            return Validator.TryValidateObject(this, validationContext, validationResults, true);
        }
    }
} 