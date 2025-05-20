using alps.net.api.StandardPASS;
using System;
using System.Collections.Generic;
using System.Text;

namespace alps.net.api.ALPS.ALPSModelElements.ALPSSIDComponents
{
    public interface ISubjectGroup : ISubject
    {
        /// <summary>
        /// Returns all contained Subjects.
        /// </summary>
        /// <returns>all contained Subjects</returns>
        IDictionary<string, ISubject> getContainedSubjects();

        /// <summary>
        /// Sets a set of Subjects as contained subjects for this Group subject, overwriting old subjects.
        /// </summary>
        /// <param name="subjects">The set of Subjects</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setSubjects(ISet<ISubject> subjects, int removeCascadeDepth = 0);


        /// <summary>
        /// Removes an Subject from the list of contained subjects
        /// </summary>
        /// <param name="id">the id of the subject</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        /// <returns>a bool indicating whether the process of removal was a success</returns>
        bool removeSubject(string id, int removeCascadeDepth = 0);
    }
}
