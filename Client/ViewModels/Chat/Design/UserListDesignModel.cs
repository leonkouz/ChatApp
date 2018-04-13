using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{

    /// <summary>
    /// The design-time data for a <see cref="UserListViewModel"/>
    /// </summary>
    public class UserListDesignModel : UserListViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static UserListDesignModel Instance => new UserListDesignModel();

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserListDesignModel()
        {

            Items = new List<UserListItemViewModel>
            {
                new UserListItemViewModel
                {
                    Name = "Luke",
                    Initials = "LM",
                    StatusMessage = "this is my status message",
                    Colour = "3099c5"
                },

                new UserListItemViewModel
                {
                    Name = "Jess",
                    Initials = "JA",
                    StatusMessage = "this is my status message",
                    Colour = "fe4503"
                },

                new UserListItemViewModel
                {
                    Name = "Mark",
                    Initials = "ML",
                    StatusMessage = "this is my status message",
                    Colour = "00d405"
                },

                new UserListItemViewModel
                {
                    Name = "Luke",
                    Initials = "LM",
                    StatusMessage = "this is my status message",
                    Colour = "3099c5"
                },

                new UserListItemViewModel
                {
                    Name = "Jess",
                    Initials = "JA",
                    StatusMessage = "this is my status message",
                    Colour = "fe4503"
                },

                new UserListItemViewModel
                {
                    Name = "Mark",
                    Initials = "ML",
                    StatusMessage = "this is my status message",
                    Colour = "00d405"
                },

                new UserListItemViewModel
                {
                    Name = "Mark",
                    Initials = "ML",
                    StatusMessage = "this is my status message",
                    Colour = "00d405"
                },

                new UserListItemViewModel
                {
                    Name = "Luke",
                    Initials = "LM",
                    StatusMessage = "this is my status message",
                    Colour = "3099c5"
                },

                new UserListItemViewModel
                {
                    Name = "Jess",
                    Initials = "JA",
                    StatusMessage = "this is my status message",
                    Colour = "fe4503"
                },

                new UserListItemViewModel
                {
                    Name = "Mark",
                    Initials = "ML",
                    StatusMessage = "this is my status message",
                    Colour = "00d405"
                }
            };
        }
    }
}
