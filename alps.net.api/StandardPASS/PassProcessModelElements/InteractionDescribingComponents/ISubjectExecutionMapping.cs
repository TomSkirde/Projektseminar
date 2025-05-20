
namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// A separte object containing a (most likely) String representation of some formal description
    /// of how this subject is mapped to the users of an execution envoironment (whos is allowed to be responsible)
    /// Theoretically there is a difference between static mapping (e.g. a subject mapped to a user group) and
    /// dynamic mapping where the mapping is determined during runtim to e.g., find the current boss for one user
    /// for non of these things formal definitions exist yet.
    /// </summary>
    public interface ISubjectExecutionMapping : IInteractionDescribingComponent
    {
        string getExecutionMappingDefinition();

        /// <summary>
        /// Set the string that SHOULD contain The definition of how this subject is to be mapped to
        /// the users of an execution enviroment
        /// </summary>
        /// <param name="executionMappingDefintion">No specific format has been developed for this yet.</param>
        void setExecutionMappingDefinition(string executionMappingDefintion);

        SubjectExecutionMappingTypes executionMappingType { get; set; }
    }

    /// <summary>
    /// To represent the different types of mapping
    /// </summary>
    public enum SubjectExecutionMappingTypes
    {
        GeneralExecutionMapping,
        StaticExecutionMapping,
        DynamicExecutionMapping
    }

}
