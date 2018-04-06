using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.ViewModels
{
    /// <summary>
    /// The View model for the custom window
    /// </summary>

    public class WindowViewModel : BaseViewModel
    {

        #region Private Fields

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window _window;

        /// <summary>
        /// The margin around the window to allow for drop shadow
        /// </summary>
        private int _outerMarginSize = 10;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int _windowRadius = 10;

        #endregion

        #region Public Properties

        /// <summary>
        /// The smallest width the window can be
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 400;

        /// <summary>
        /// The smallest height the window can be
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;

        /// <summary>
        /// This size of the resize border around the window
        /// </summary>
        public int ResizeBorder { get; set; } = 6;

        /// <summary>
        /// The size of the resize border around the window, taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }

        /// <summary>
        /// The padding of the inner content of the main window
        /// </summary>
        public Thickness InnerContentPadding { get { return new Thickness(ResizeBorder); } }

        /// <summary>
        /// The margin around the window to allow for drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                return _window.WindowState == WindowState.Maximized ? 0 : _outerMarginSize;
            }
            set
            {
                _outerMarginSize = value;
            }
        }

        /// <summary>
        /// The thickness of the margin around the window to allow for drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius
        {
            get
            {
                return _window.WindowState == WindowState.Maximized ? 0 : _windowRadius;
            }
            set
            {
                _windowRadius = value;
            }
        }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        /// <summary>
        /// The height of the title bar of the window
        /// </summary>
        public int TitleHeight { get; set; } = 42;

        /// <summary>
        /// The height of the title bar of the window
        /// </summary>
        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorder); } }

        #endregion

        #region Commands

        /// <summary>
        /// The command to minimise the window
        /// </summary>
        public ICommand MinimiseCommand { get; set; }

        /// <summary>
        /// The command to maximise the window
        /// </summary>
        public ICommand MaximiseCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to show the system menu
        /// </summary>
        public ICommand MenuCommand { get; set; }

        #endregion

        public WindowViewModel(Window window)
        {
            _window = window;

            //Listen for window state change
            _window.StateChanged += _window_StateChanged;

            MinimiseCommand = new DelegateCommand(() => _window.WindowState = WindowState.Minimized);
            MaximiseCommand = new DelegateCommand(() => _window.WindowState ^= WindowState.Maximized);
            CloseCommand = new DelegateCommand(() => _window.Close());
            MenuCommand = new DelegateCommand(() => SystemCommands.ShowSystemMenu(_window, GetMousePosition()));

            //Fix window resize issue
            var resizer = new WindowResizer(_window);

        }

        private void _window_StateChanged(object sender, EventArgs e)
        {
            //Fire events for all properties that are affected by a resize
            RaisePropertyChangedEvent(nameof(ResizeBorderThickness));
            RaisePropertyChangedEvent(nameof(OuterMarginSize));
            RaisePropertyChangedEvent(nameof(OuterMarginSizeThickness));
            RaisePropertyChangedEvent(nameof(WindowRadius));
            RaisePropertyChangedEvent(nameof(WindowCornerRadius));
        }

        #region Private Helpers
        
        /// <summary>
        /// Gets current mouse position on the screen
        /// </summary>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(_window);

            // Add the window position so its a "ToScreen"
            return new Point(position.X + _window.Left, position.Y + _window.Top);
        }

        #endregion

    }
}
