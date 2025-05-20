using alps.net.api.parsing;

public class Neo4JGraphFactory : IGraphFactory
{

    private string dbUri;
    private string user;
    private string password;

    public Neo4JGraphFactory(string dbUri, string user, string password)
    {
        this.dbUri = dbUri;
        this.user = user;
        this.password = password;
    }
    public IPASSGraph createGraph(string modelBaseUri)
    {
        return new Neo4JGraph(modelBaseUri, dbUri, user, password);
    }
}