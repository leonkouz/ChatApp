using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client
{
    /// <summary>
    /// The View Model for the custom flat window
    /// </summary>
    public class WindowViewModel : BaseViewModel
    {
        #region Private Member

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window _window;

        /// <summary>
        /// The window resizer helper that keeps the window size correct in various states
        /// </summary>
        private WindowResizer _windowResizer;

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        private Thickness _outerMarginSize = new Thickness(5);

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int _windowRadius = 10;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition _dockPosition = WindowDockPosition.Undocked;

        #endregion

        #region Public Properties

        /// <summary>
        /// The smallest width the window can go to
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 800;

        /// <summary>
        /// The smallest height the window can go to
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 500;

        /// <summary>
        /// True if the window is currently being moved/dragged
        /// </summary>
        public bool BeingMoved { get; set; }


        /// <summary>
        /// True if the window should be borderless because it is docked or maximized
        /// </summary>
        public bool Borderless => (_window.WindowState == WindowState.Maximized || _dockPosition != WindowDockPosition.Undocked);

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder => _window.WindowState == WindowState.Maximized ? 0 : 4;

        /// <summary>
        /// The size of the resize border around the window, taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness => new Thickness(OuterMarginSize.Left + ResizeBorder,
                                                                OuterMarginSize.Top + ResizeBorder,
                                                                OuterMarginSize.Right + ResizeBorder,
                                                                OuterMarginSize.Bottom + ResizeBorder);

        /// <summary>
        /// The padding of the inner content of the main window
        /// </summary>
        public Thickness InnerContentPadding { get; set; } = new Thickness(0);

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSize
        {
            // If it is maximized or docked, no border
            get => _window.WindowState == WindowState.Maximized ? _windowResizer.CurrentMonitorMargin : (Borderless ? new Thickness(0) : _outerMarginSize);
            set => _outerMarginSize = value;
        }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius
        {
            // If it is maximized or docked, no border
            get => Borderless ? 0 : _windowRadius;
            set => _windowRadius = value;
        }

        
        /// <summary>
        /// The current page displayed in the window
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.Login;

        /// <summary>
        /// The rectangle border around the window when docked
        /// </summary>
        public int FlatBorderThickness => Borderless && _window.WindowState != WindowState.Maximized ? 1 : 0;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public int TitleHeight { get; set; } = 42;
        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);

        /// <summary>
        /// True if we should have a dimmed overlay on the window
        /// such as when a popup is visible or the window is not focused
        /// </summary>
        public bool DimmableOverlayVisible { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to minimize the window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to show the system menu of the window
        /// </summary>
        public ICommand MenuCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowViewModel(Window window)
        {
            _window = window;

            // Listen out for the window resizing
            _window.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResized();
            };

            // Create commands
            MinimizeCommand = new DelegateCommand(() => _window.WindowState = WindowState.Minimized);
            MaximizeCommand = new DelegateCommand(() => _window.WindowState ^= WindowState.Maximized);
            CloseCommand = new DelegateCommand(() => _window.Close());
            MenuCommand = new DelegateCommand(() => SystemCommands.ShowSystemMenu(_window, GetMousePosition()));

            // Fix window resize issue
            _windowResizer = new WindowResizer(_window);

            // Listen out for dock changes
            _windowResizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                _dockPosition = dock;

                // Fire off resize events
                WindowResized();
            };

            // On window being moved/dragged
            _windowResizer.WindowStartedMove += () =>
            {
                // Update being moved flag
                BeingMoved = true;
            };

            // Fix dropping an undocked window at top which should be positioned at the
            // very top of screen
            _windowResizer.WindowFinishedMove += () =>
            {
                // Update being moved flag
                BeingMoved = false;

                // Check for moved to top of window and not at an edge
                if (_dockPosition == WindowDockPosition.Undocked && _window.Top == _windowResizer.CurrentScreenSize.Top)
                    // If so, move it to the true top (the border size)
                    _window.Top = -OuterMarginSize.Top;
            };
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            return _windowResizer.GetCursorPosition();
        }

        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            // Fire off events for all properties that are affected by a resize
            RaisePropertyChangedEvent(nameof(Borderless));
            RaisePropertyChangedEvent(nameof(FlatBorderThickness));
            RaisePropertyChangedEvent(nameof(ResizeBorderThickness));
            RaisePropertyChangedEvent(nameof(OuterMarginSize));
            RaisePropertyChangedEvent(nameof(WindowRadius));
            RaisePropertyChangedEvent(nameof(WindowCornerRadius));
        }


        #endregion
    }
}
