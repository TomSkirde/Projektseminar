using alps.net.api.parsing;

/// <summary>
/// Provides graphs as data storage for models.
/// </summary>
public interface IGraphFactory
{
    /// <summary>
    /// Creates a new graph with the given base uri
    /// </summary>
    /// <param name="modelBaseUri">The base uri of the model</param>
    /// <returns></returns>
    public IPASSGraph createGraph(string modelBaseUri);

}