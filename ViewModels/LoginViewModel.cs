using System.Windows.Input;
using UsenetProgram.Services;
using UsenetProgram.Utilities;

namespace UsenetProgram.ViewModels
{
    public class LoginViewModel : Bindable, IWindowService
    {
        public Action Close { get; set; }
        public ICommand CancelCMD { get; set; }
        public ICommand LoginCMD { get; set; }

        private string _host;

        public string Host
        {
            get { return _host; }
            set
            {
                _host = value;
                OnPropertyChanged();
            }
        }

        private int _port;

        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        private string _login;

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel()
        {
            this._login = "";
            this._password = "";
            this._host = "news.sunsite.dk";
            this._port = 119;

            this.LoginCMD = new RelayCommand(LoginUser, CanLoginUser);
            this.CancelCMD = new RelayCommand(CloseProgram, CanCloseProgram);
        }

        private async void LoginUser()
        {
            bool isConnected = await NetworkManager.Instance.MakeConnectionAsync(Host, Port);
            if (!isConnected)
            {
                NetworkManager.Instance.Reset();
                return;
            }

            bool isAuthenticated = await LoginManager.Authenticate(Login, Password);
            if (isAuthenticated)
            {
                ViewManager.OpenWindow(ViewType.NEWSFEED);
                Close?.Invoke();
            }
            else
                NetworkManager.Instance.Reset();
        }

        private bool CanLoginUser()
        {
            return !string.IsNullOrEmpty(Login) &&
                   !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(Host) &&
                   Port > 0;
        }

        private void CloseProgram()
        {
            NetworkManager.Instance.Reset();
            Close?.Invoke();
        }

        private bool CanCloseProgram()
        {
            return true;
        }
    }
}
