using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the macro state class
    /// <br></br>
    /// <br></br>
    /// From PASS Doc: <i>A state that references a macro behavior that is executed upon entering this state. Only after executing the macro behavior this state is finished also.</i>
    /// </summary>

    public interface IMacroState : IState
    {
        /// <summary>
        /// Sets the macro behavior that is referenced by the macro state
        /// </summary>
        /// <param name="macroBehavior">the macro behavior</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setReferencedMacroBehavior(IMacroBehavior macroBehavior, int removeCascadeDepth = 0);

        /// <summary>
        /// Gets the macro behavior that is referenced by the macro state
        /// </summary>
        /// <returns>the macro behavior</returns>
        IMacroBehavior getReferencedMacroBehavior();

    
       
    }
}
