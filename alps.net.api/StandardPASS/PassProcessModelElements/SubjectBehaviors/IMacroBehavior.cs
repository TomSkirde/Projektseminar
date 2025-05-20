using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the macro bahvior class
    /// </summary>

    public interface IMacroBehavior : ISubjectBehavior
    {

        /// <summary>
        /// Method that returns all state references held by the instance
        /// </summary>
        /// <returns>a list of state references</returns>
        IDictionary<string, IStateReference> getStateReferences();

        /// <summary>
        /// Method that returns all return to origin references held by the instance
        /// </summary>
        /// <returns>a list of return to origin references</returns>
        IDictionary<string, IGenericReturnToOriginReference> getReturnReferences();

    }

}
