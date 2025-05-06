using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFValidationDemo.Infrastructure;
using WPFValidationDemo.Models;
using System.Windows;

namespace WPFValidationDemo.ViewModels
{
    /// <summary>
    /// 产品ViewModel，使用ValidationViewModelBase（实现INotifyDataErrorInfo接口）
    /// </summary>
    public class ProductViewModel : ValidationViewModelBase
    {
        private Product _product;
        private string _saveResult;
        private List<string> _categories;

        public ProductViewModel()
        {
            _product = new Product();
            SaveCommand = new RelayCommand(ExecuteSave, CanSave);
            ResetCommand = new RelayCommand(ExecuteReset);
            
            // 初始化类别列表
            Categories = new List<string>
            {
                "电子产品",
                "服装",
                "家具",
                "图书",
                "食品",
                "其他"
            };
        }

        #region 属性

        public string Name
        {
            get => _product.Name;
            set 
            { 
                if (_product.Name != value)
                {
                    _product.Name = value;
                    OnPropertyChanged(nameof(Name));
                    ValidateName();
                }
            }
        }

        public decimal Price
        {
            get => _product.Price;
            set 
            { 
                if (_product.Price != value)
                {
                    _product.Price = value;
                    OnPropertyChanged(nameof(Price));
                    ValidatePrice();
                }
            }
        }

        public DateTime ReleaseDate
        {
            get => _product.ReleaseDate;
            set 
            { 
                if (_product.ReleaseDate != value)
                {
                    _product.ReleaseDate = value;
                    OnPropertyChanged(nameof(ReleaseDate));
                    ValidateReleaseDate();
                }
            }
        }

        public int StockLevel
        {
            get => _product.StockLevel;
            set 
            { 
                if (_product.StockLevel != value)
                {
                    _product.StockLevel = value;
                    OnPropertyChanged(nameof(StockLevel));
                    ValidateStockLevel();
                }
            }
        }

        public string Description
        {
            get => _product.Description;
            set 
            { 
                if (_product.Description != value)
                {
                    _product.Description = value;
                    OnPropertyChanged(nameof(Description));
                    ValidateDescription();
                }
            }
        }

        public string Category
        {
            get => _product.Category;
            set 
            { 
                if (_product.Category != value)
                {
                    _product.Category = value;
                    OnPropertyChanged(nameof(Category));
                    ValidateCategory();
                }
            }
        }
        
        public List<string> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public string SaveResult
        {
            get => _saveResult;
            set => SetProperty(ref _saveResult, value);
        }

        #endregion

        #region 验证方法

        private void ValidateName()
        {
            ClearErrors(nameof(Name));
            
            if (string.IsNullOrEmpty(Name))
            {
                AddError(nameof(Name), "产品名称不能为空");
            }
            else if (Name.Length < 3)
            {
                AddError(nameof(Name), "产品名称长度不能少于3个字符");
            }
            else if (Name.Length > 50)
            {
                AddError(nameof(Name), "产品名称长度不能超过50个字符");
            }
        }

        private void ValidatePrice()
        {
            ClearErrors(nameof(Price));
            
            if (Price <= 0)
            {
                AddError(nameof(Price), "产品价格必须大于0");
            }
            else if (Price > 100000)
            {
                AddError(nameof(Price), "产品价格不能超过100,000");
            }
        }

        private void ValidateReleaseDate()
        {
            ClearErrors(nameof(ReleaseDate));
            
            if (ReleaseDate > DateTime.Today.AddYears(1))
            {
                AddError(nameof(ReleaseDate), "发布日期不能超过一年后");
            }
        }

        private void ValidateStockLevel()
        {
            ClearErrors(nameof(StockLevel));
            
            if (StockLevel < 0)
            {
                AddError(nameof(StockLevel), "库存数量不能为负数");
            }
        }

        private void ValidateDescription()
        {
            ClearErrors(nameof(Description));
            
            if (Description?.Length > 500)
            {
                AddError(nameof(Description), "产品描述不能超过500个字符");
            }
        }

        private void ValidateCategory()
        {
            ClearErrors(nameof(Category));
            
            if (string.IsNullOrEmpty(Category))
            {
                AddError(nameof(Category), "请选择产品类别");
            }
        }

        #endregion

        #region 命令

        public ICommand SaveCommand { get; }
        public ICommand ResetCommand { get; }

        private bool CanSave(object parameter)
        {
            // 检查是否有验证错误
            return !HasErrors;
        }

        private async void ExecuteSave(object parameter)
        {
            // 模拟异步保存操作
            SaveResult = "正在保存产品...";
            
            await Task.Delay(1000); // 模拟网络延迟
            
            // 在实际应用中会调用服务层保存产品
            SaveResult = $"产品 \"{Name}\" 保存成功！";
            MessageBox.Show("产品保存成功！", "产品管理", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecuteReset(object parameter)
        {
            Name = string.Empty;
            Price = 0;
            ReleaseDate = DateTime.Today;
            StockLevel = 0;
            Description = string.Empty;
            Category = null;
            SaveResult = string.Empty;
            
            ClearAllErrors();
        }

        #endregion

        /// <summary>
        /// 执行所有验证
        /// </summary>
        public void ValidateAll()
        {
            ValidateName();
            ValidatePrice();
            ValidateReleaseDate();
            ValidateStockLevel();
            ValidateDescription();
            ValidateCategory();
        }
    }
} 