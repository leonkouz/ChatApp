﻿using ChatServer.Shared;
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

        #endregion

        #region Public Properties

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

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email
        {
            get { return _email; }
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
            get { return _firstName; }
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
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChangedEvent("LastName");
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
                

                ChatClient.Connect();

                // Create user
                User user = new User
                {
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    Email = this.Email,
                    Password = (parameter as IHavePassword).SecurePassword,
                };

                // Attempt to register user
                var response = await ChatClient.RegisterUser(user);

                // If attempt to register user was not succesfful
                if(!response == true)
                {
                    //show error
                }
                else
                {
                    IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.GlobalChat);
                }

            });
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

