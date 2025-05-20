using alps.net.api.parsing;
using VDS.RDF;

namespace alps.net.api.util
{
    /// <summary>
    /// This interface simplifies the use of <see cref="Triple"/> when no <see cref="IGraph"/> is currently given to create a real triple,
    /// or as quick mock for a real triple.
    /// An incomplete triple is only valid when bound to a class that acts as subject,
    /// because the incomplete triple does not parse subject information (only object and predicate).
    /// An IncompleteTriple might either contain information about the object, or about "objectWithExtra" (<see cref="getObjectWithExtra"/>)
    /// Not both at the same time.
    /// </summary>
    public interface IPASSTriple
    {

        /// <summary>
        /// Get the predicate attribute of the incomplete triple
        /// </summary>
        string getPredicate();

        /// <summary>
        /// Get the predicate attribute of the incomplete triple
        /// </summary>
        string getSubject();

        /// <summary>
        /// Get the object attribute of the incomplete triple
        /// </summary>
        string getObject();

        /// <summary>
        /// Get the object attribute + extra attribute (language tag or datatype) of the incomplete triple
        /// </summary>
        IStringWithExtra getObjectWithExtra();

        string getObjLang();

        string getObjDataType();
    }
}
