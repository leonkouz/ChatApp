using ChatServer.Core;
using ChatServer.Shared;
using System;
using System.Security;
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

        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        private bool _registerIsRunning = false;

        /// <summary>
        /// The email of the user
        /// </summary>
        private string _email;

        /// <summary>
        /// The first name of the user
        /// </summary>
        private string _firstName;

        /// <summary>
        /// The last name of the user
        /// </summary>
        private string _lastName;

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
        /// A flag indicating if the login command is running
        /// </summary>
        public bool RegisterIsRunning
        {
            get => _registerIsRunning; 
            set
            {
                _registerIsRunning = value;
                RaisePropertyChangedEvent("RegisterIsRunning");
            }
        }

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email
        {
            get => _email; 
            set
            {
                _email = value;
                RaisePropertyChangedEvent("Email");
            }
        }

        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName
        {
            get => _firstName; 
            set
            {
                _firstName = value;
                RaisePropertyChangedEvent("FirstName");
            }
        }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName
        {
            get => _lastName; 
            set
            {
                _lastName = value;
                RaisePropertyChangedEvent("LastName");
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
                await Task.Delay(1000);

                // Get the password from the RegisterPage code behind
                var password = (parameter as IHavePassword).SecurePassword;

                if(!CheckIfAllFieldsAreFilled(password))
                    return;    

                // Validate email
                if (!IsValidEmail())
                    return;

                if(!ChatClient.Connected)
                    ChatClient.Connect();

                // Create user
                RegisterUserToken user = new RegisterUserToken
                {
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    Email = this.Email,
                    Password = password
                };

                // Attempt to register user
                var response = await ChatClient.RegisterUser(user);

                // If attempt to register user was not succesfful
                if(response.Status == StatusCode.Failure)
                {
                    //Show error
                    if(response.Error == null)
                    {
                        Error = "Something went wrong";
                    }
                    else
                    {
                        Error = response.Error;
                    }
                    
                    ShowError = true;
                }
                // otherwise log the user in
                else
                {
                    IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.GlobalChat);
                }
            });
        }

        /// <summary>
        /// Checks if all fields have data entered by the user
        /// </summary>
        /// <returns></returns>
        private bool CheckIfAllFieldsAreFilled(SecureString password)
        {
            if (String.IsNullOrWhiteSpace(FirstName) || String.IsNullOrWhiteSpace(LastName) || String.IsNullOrWhiteSpace(Email) || password.Length == 0) 
            {
                Error = "Please fill all fields";
                ShowError = true;

                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Check if the email is valid
        /// </summary>
        /// <returns></returns>
        private bool IsValidEmail()
        {
            if (!Validator.IsValidEmail(Email))
            {
                Error = "Invalid Email";
                ShowError = true;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Takes the user to the login page
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> pass in from the view for the users password </param>
        /// <returns></returns>
        public async Task LoginAsync()
        {
            // Go to login page
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Login);

            await Task.Delay(1);
        }
    }
}

