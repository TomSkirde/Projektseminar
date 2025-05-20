using alps.net.api.ALPS;
using alps.net.api.util;
using System.Collections.Generic;
namespace alps.net.api.StandardPASS
{

    /// <summary>
    /// Interface to the Subject behavior class
    /// </summary>
    public interface ISubjectBehavior : IPASSProcessModelElement, IContainableElement<IModelLayer>,
        IImplementingElement<ISubjectBehavior>, IExtendingElement<ISubjectBehavior>, IPrioritizableElement
    {

        // ######################## BehaviorDescribingComponents methods ########################

        /// <summary>
        /// Adds an <see cref="IBehaviorDescribingComponent"/> to the current Subject Behavior.
        /// </summary>
        /// <param name="component">the component that is being added</param>
        public bool addBehaviorDescribingComponent(IBehaviorDescribingComponent component);

        /// <summary>
        /// Sets all BehaviorDescribingComponents contained by the behavior.
        /// Overwrites all components contained before.
        /// </summary>
        /// <param name="components">The new components that will be set</param>
        public void setBehaviorDescribingComponents(ISet<IBehaviorDescribingComponent> components, int removeCascadeDepth = 0);

        /// <summary>
        /// Removes a BehaviorDescribingComponent from the SubjectBehavior
        /// </summary>
        /// <param name="id">The modelComponentID of the component that should be removed</param>
        public bool removeBehaviorDescribingComponent(string id, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the behavior description component attribute of the instance
        /// </summary>
        /// <returns>The behavior description component attribute of the instance</returns>
        public IDictionary<string, IBehaviorDescribingComponent> getBehaviorDescribingComponents();



        // ######################## Other getter and setter methods ########################


        /// <summary>
        /// Method that sets the initial state of behaviors attribute of the instance
        /// </summary>
        void setInitialState(IState initialStateOfBehavior, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the initial state of behaviors attribute of the instance
        /// </summary>
        /// <returns>The initial state of behaviors attribute of the instance</returns>
        IState getInitialStateOfBehavior();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subj"></param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setSubject(ISubject subj, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that gives access to the Subject the current behavior is connected with
        /// </summary>
        /// <returns>The subject this behavior is connected with</returns>
        ISubject getSubject();
    }

}
