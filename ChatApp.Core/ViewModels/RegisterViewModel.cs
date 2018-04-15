using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatApp.Core
{
    /// <summary>
    /// The View Model for a register screen
    /// </summary>
    public class RegisterViewModel : BaseViewModel
    {
        #region Private Fields

        private bool _registerIsRunning;

        #endregion

        #region Public Properties

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        public bool RegisterIsRunning
        {
            get { return _registerIsRunning; }
            set
            {
                _registerIsRunning = value;
                RaisePropertyChangedEvent("RegisterIsRunning");
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
        public RegisterViewModel()
        {
            // Create commands
            RegisterCommand = new DelegateParametrisedCommand(async (parameter) => await RegisterAsync(parameter));
            LoginCommand = new DelegateCommand(async () => await LoginAsync());
        }

        #endregion

        /// <summary>
        /// Attempts to register a new user
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> pass in from the view for the users password </param>
        /// <returns></returns>
        public async Task RegisterAsync(object parameter)
        {
            await RunCommandAsync(() => RegisterIsRunning, async () =>
            {
                await Task.Delay(5000);
            });
        }

        /// <summary>
        /// Takes the user to the login page
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> pass in from the view for the users password </param>
        /// <returns></returns>
        public async Task LoginAsync()
        {
            // TODO: Go to register page
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Login);

            await Task.Delay(1);
        }
    }
}

