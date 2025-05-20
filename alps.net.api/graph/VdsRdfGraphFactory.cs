using alps.net.api.parsing;

public class VdsRdfGraphFactory : IGraphFactory
{
    public IPASSGraph createGraph(string modelBaseUri)
    {
        return new VdsRdfGraph(modelBaseUri);
    }
}