using alps.net.api.parsing;

namespace alps.net.api.FunctionalityCapsules
{
    public interface IFunctionalityCapsule<T>
    {
        bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element);
    }
}
