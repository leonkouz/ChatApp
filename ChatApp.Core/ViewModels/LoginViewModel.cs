using ChatServer.Shared;
using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatApp.Core
{
    /// <summary>
    /// The View Model for a login screen
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {

        #region Private Fields

        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        private bool _loginIsRunning;

        /// <summary>
        /// True if an error should be shown
        /// </summary>
        private bool _showError = false;

        /// <summary>
        /// The error to display if <see cref="_showError"/> is true
        /// </summary>
        private string _error;

        #endregion

        #region Public Properties

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        public bool LoginIsRunning
        {
            get { return _loginIsRunning; }
            set
            {
                _loginIsRunning = value;
                RaisePropertyChangedEvent("LoginIsRunning");
            }
        }

        /// <summary>
        /// True if an error should be shown
        /// </summary>
        public bool ShowError
        {
            get => _showError;
            set
            {
                _showError = value;
                RaisePropertyChangedEvent("ShowError");
            }
        }

        /// <summary>
        /// The error to display if <see cref="ShowError"/> is true
        /// </summary>
        public string Error
        {
            get => _error;
            set
            {
                _error = value;
                RaisePropertyChangedEvent("Error");
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// The command to register for a new account
        /// </summary>
        public ICommand RegisterCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            // Create commands
            LoginCommand = new DelegateParametrisedCommand(async (parameter) => await LoginAsync(parameter));
            RegisterCommand = new DelegateCommand(async () => await RegisterAsync());
        }

        #endregion

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> pass in from the view for the users password </param>
        /// <returns></returns>
        public async Task LoginAsync(object parameter)
        {
            await RunCommandAsync(() => LoginIsRunning, async () =>
            {
                await Task.Delay(1000);

                // Get the password from the LoginPage code behind
                var password = (parameter as IHavePassword).SecurePassword;

                if (!ChatClient.Connected)
                    ChatClient.Connect();

                if (!CheckIfAllFieldsAreFilled(password)) 
                    return;

                LoginToken token = new LoginToken
                {
                    Email = this.Email,
                    Password = password
                };

                var response = 


                // Go to chat page
                IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.GlobalChat);

                /*
                var email = Email;
                var pass = (parameter as IHavePassword).SecurePassword.Unsecure(); // MUST CHANGE! NEVER STORE UNSECURE PASSWORD IN VARIABLE, PASS DIRECTLY TO METHOD*/
            });
        }

        private bool CheckIfAllFieldsAreFilled(SecureString password)
        {
            if(String.IsNullOrWhiteSpace(Email) || password.Length == 0)
            {
                Error = "Please enter user credentails";
                ShowError = true;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Takes the user to the register page
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> pass in from the view for the users password </param>
        /// <returns></returns>
        public async Task RegisterAsync()
        {
            // TODO: Go to register page
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Register);

            await Task.Delay(1);
        }
    }
}

