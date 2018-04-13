using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    /// <summary>
    /// Styles of page animation for appearing/disappearing
    /// </summary>
    public enum PageAnimation
    {

        /// <summary>
        /// No animation
        /// </summary>
        None = 0,

        /// <summary>
        /// The page slides in and fades from the right
        /// </summary>
        SlideAndFadeFromRight = 1,

        /// <summary>
        /// The pages slides out and fades to the left
        /// </summary>
        SlideAndFadeOutToLeft = 2,

    }
}
