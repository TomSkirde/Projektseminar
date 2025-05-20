using alps.net.api.StandardPASS;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public interface IGroupState : IALPSSBDComponent, IState
    {
        /// <summary>
        /// Adds a component to the group of components grouped by this state
        /// </summary>
        /// <param name="component">the new component</param>
        /// <returns>whether the process of adding was successful</returns>
        bool addGroupedComponent(IBehaviorDescribingComponent component);

        /// <summary>
        /// Overrides the set of grouped components
        /// </summary>
        /// <param name="components">the new components</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setGroupedComponents(ISet<IBehaviorDescribingComponent> components, int removeCascadeDepth = 0);

        /// <summary>
        /// Removes a component from the group
        /// </summary>
        /// <param name="id">the id of the component</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        /// <returns></returns>
        bool removeGroupedComponent(string id, int removeCascadeDepth = 0);

        /// <returns>A dictionary of grouped states, mapped with their id</returns>
        IDictionary<string, IBehaviorDescribingComponent> getGroupedComponents();
    }
}
