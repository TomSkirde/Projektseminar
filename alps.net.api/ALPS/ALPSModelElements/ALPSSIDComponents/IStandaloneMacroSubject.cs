using alps.net.api.StandardPASS;
using System;
using System.Collections.Generic;
using System.Text;

namespace alps.net.api.ALPS
{
    public interface IStandaloneMacroSubject : IALPSSIDComponent, ISubject
    {
        /// <summary>
        /// Sets a behavior for the current subject.
        /// </summary>
        /// <param name="behavior">The behavior</param>
        /// <returns>a bool indicating whether the process of setting was a success</returns>
        void setBehavior(IMacroBehavior behavior, int removeCascadeDepth = 0);

        /// <summary>
        /// Get the behavior contained by this subject
        /// </summary>
        /// <returns>A macro behaviors</returns>
        IMacroBehavior getBehavior();
    }
}
