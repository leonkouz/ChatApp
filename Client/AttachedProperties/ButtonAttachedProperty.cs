using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChatApp
{
    /// <summary>
    /// The IsBusy attached property for anythign that wants to flag if control is busy
    /// </summary>
    public class IsBusyProperty : BasedAttachedProperty<HasTextProperty, bool>
    {
    }
}
