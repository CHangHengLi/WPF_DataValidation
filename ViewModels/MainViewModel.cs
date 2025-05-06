using System.Windows.Input;
using WPFValidationDemo.Infrastructure;

namespace WPFValidationDemo.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private object _currentViewModel;

        public MainViewModel()
        {
            // 初始化子ViewModel
            UserRegistrationVM = new UserRegistrationViewModel();
            ProductVM = new ProductViewModel();
            
            // 默认显示用户注册视图
            CurrentViewModel = UserRegistrationVM;
            
            // 初始化命令
            ShowUserRegistrationCommand = new RelayCommand(p => CurrentViewModel = UserRegistrationVM);
            ShowProductCommand = new RelayCommand(p => CurrentViewModel = ProductVM);
        }

        public UserRegistrationViewModel UserRegistrationVM { get; }
        public ProductViewModel ProductVM { get; }

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ICommand ShowUserRegistrationCommand { get; }
        public ICommand ShowProductCommand { get; }
    }
} 