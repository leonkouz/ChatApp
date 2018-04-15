using System;
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

        private bool _loginIsRunning;

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

                // Go to chat page
                IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.GlobalChat);

                ChatClient.Connect("test");

                
                /*
                var email = Email;
                var pass = (parameter as IHavePassword).SecurePassword.Unsecure(); // MUST CHANGE! NEVER STORE UNSECURE PASSWORD IN VARIABLE, PASS DIRECTLY TO METHOD*/
            });
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

