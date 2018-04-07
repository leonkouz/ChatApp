using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    
    /// <summary>
    /// A base attached property to replace the vanilla WPF attached property
    /// </summary>
    /// <typeparam name="Parent">The parent class to be the attached property</typeparam>
    /// <typeparam name="PropertyType">The type of this attached property</typeparam>
    public abstract class BasedAttachedProperty<Parent, PropertyType>
        where Parent : BasedAttachedProperty<Parent, PropertyType>, new()
    {

        #region Public Events

        /// <summary>
        /// Fired when the value changes
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        #endregion

        #region Public Properties

        /// <summary>
        /// A singleton instance of our parent class
        /// </summary>
        public static Parent Instance { get; private set; } = new Parent();


        #endregion

        #region AttachedPropertyDefinitions

        /// <summary>
        /// The attached property for this class
        /// </summary>
        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.RegisterAttached("Value", typeof(PropertyType), typeof(BasedAttachedProperty<Parent, PropertyType>), new PropertyMetadata(new PropertyChangedCallback(OnValuePropertyChanged)));

        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is changed
        /// </summary>
        /// <param name="d">The UI element that has its property changed</param>
        /// <param name="e">The arguments for the vent</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Call the parent function
            Instance.OnValueChanged(d, e);

            // Call Event listerns
            Instance.ValueChanged(d, e);
        }
        
        /// <summary>
        /// Gets the attached property
        /// </summary>
        /// <param name="d">The elemtn to get the property from</param>
        /// <returns></returns>
        public static PropertyType GetValue(DependencyObject d) => (PropertyType)d.GetValue(ValueProperty);

        /// <summary>
        /// Sets the attached property
        /// </summary>
        /// <param name="d">The element to get the property from</param>
        /// <param name="value">The value to set the property to</param>
        public static void SetValue(DependencyObject d, PropertyType value) => d.SetValue(ValueProperty, value);

        #endregion

        #region Event Methods

        /// <summary>
        /// The method that is called when any attached property of this type is changed.
        /// </summary>
        /// <param name="sender">The UI element that this property was changed fro</param>
        /// <param name="e">The arguments for this event</param>
        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

        #endregion
    }
}
