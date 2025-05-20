using alps.net.api.ALPS.ALPSModelElements.ALPSSIDComponents;
using alps.net.api.StandardPASS;
using System;
using System.Collections.Generic;
using System.Text;

namespace alps.net.api.ALPS
{
    public interface ISystemInterfaceSubject : IInterfaceSubject
    {
        /// <summary>
        /// Returns all contained InterfaceSubjects.
        /// </summary>
        /// <returns>all contained InterfaceSubjects</returns>
        IDictionary<string, IInterfaceSubject> getContainedInterfaceSubjects();

        /// <summary>
        /// Adds an InterfaceSubject to the list of contained InterfaceSubjects.
        /// </summary>
        /// <param name="subject">The new InterfaceSubject</param>
        /// <returns>a bool indicating whether the process of adding was a success</returns>
        bool addInterfaceSubject(IInterfaceSubject subject);

        /// <summary>
        /// Sets a set of InterfaceSubjects as contained subjects for this subject, overwriting old subjects.
        /// </summary>
        /// <param name="subjects">The set of InterfaceSubjects</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setInterfaceSubjects(ISet<IInterfaceSubject> subjects, int removeCascadeDepth = 0);

        /// <summary>
        /// Removes an InterfaceSubject from the list of contained subjects
        /// </summary>
        /// <param name="id">the id of the subject</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        /// <returns>a bool indicating whether the process of removal was a success</returns>
        bool removeInterfaceSubject(string id, int removeCascadeDepth = 0);
    }
}
