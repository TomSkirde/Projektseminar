using alps.net.api.StandardPASS;
using System;
using System.Collections.Generic;
using System.Reflection;
using VDS.RDF;

namespace alps.net.api.parsing
{
    public interface IPASSReaderWriter
    {
        /// <summary>
        /// Loads in all the required files given by filepaths.
        /// The files must be written in correct owl format.
        /// If the structure defining owl files are passed alongside the model defining ones,
        /// it must be declared via boolean that the current parsing structure should be overwritten.
        /// It is advised to only load the structure defining owl files once via <see cref="loadOWLParsingStructure"/>,
        /// because the creation of the parsing structure is likely to be an expensive operation.
        /// </summary>
        /// <param name="filepaths">The list of filepaths to valid formatted owl files</param>
        /// <param name="overrideOWLParsingStructure">Default false, should be set true when the structure defining owl files
        ///     are passed alongside the model defining ones, and the current parsing structure should be overwritten.</param>
        /// <returns>A list of <see cref="IPASSProcessModel"/> the were created from the given owl</returns>
        IList<IPASSProcessModel> loadModels(IList<string> filepaths, IGraphFactory modelGraphFactory = null, bool overrideOWLParsingStructure = false);

        /*
        /// <summary>
        /// during parsing the reader needs to analyze if a model is just a simple PASS model and not an ALPS mode
        /// this only matters if it is a standard model where a base subject contains multiple behaviors
        /// In that case the library needs to add according layers and extensions make the model compatible
        /// </summary>
        ///
        Boolean currentModelHasMultipleBehaviors { get; set; }
        /// <summary>
        /// during parsing the reader needs to analyze if a model is just a simple PASS model and not an ALPS mode
        /// this only matters if it is a standard model where a base subject contains multiple behaviors
        /// In that case the library needs to add according layers and extensions make the model compatible
        /// </summary>
        ///
        Boolean currentModelHasLayers { get; set; }

        */

        /// <summary>
        /// The abstract pass ont and/or the standard pass ont must be given
        /// either inside the same owl file or as seperate files. These files will be used
        /// to create a parsing structure for models.
        /// This operation is likely to be an expensive one and should only be used once if loaded models share the same structure.
        /// </summary>
        /// <param name="filepathsToOWLFiles">The list of filepaths to valid formatted owl files containing the structure for models and components</param>
        void loadOWLParsingStructure(IList<string> filepathsToOWLFiles);

        /// <summary>
        /// Exports the given model as rdf/owl formatted file.
        /// The file will be saved under filename, this might be given as dynamic or static filepath.
        /// </summary>
        /// <param name="model">the model that should be exported</param>
        /// <param name="filepath">the path where the file will be saved</param>
        /// <param name="exportGraph">A graph representation of the exported owl file</param>
        /// <returns>The full filepath to the file that was exported</returns>
        string exportModel(IPASSProcessModel model, string filepath, out IGraph exportGraph);

        /// <summary>
        /// Exports the given model as rdf/owl formatted file.
        /// The file will be saved under filename, this might be given as dynamic or static filepath.
        /// </summary>
        /// <param name="model">the model that should be exported</param>
        /// <param name="filepath">the path where the file will be saved</param>
        /// <returns>The full filepath to the file that was exported</returns>
        string exportModel(IPASSProcessModel model, string filepath);

        /// <summary>
        /// Allows to set another factory to create the elements.
        /// The standard factory <see cref="BasicPASSProcessModelElementFactory"/> is used automatically and decides on mapped elements based on the name similarity.
        /// If the factory should choose other elements, a new factory can be inserted here.
        /// </summary>
        /// <param name="factory"></param>
        void setModelElementFactory(IPASSProcessModelElementFactory<IParseablePASSProcessModelElement> factory);

        /// <summary>
        /// Allows to add Assemblies to be searched for clesses which extend the PASSProcessModelElement class.
        /// When the parsing tree is built, these classes are considered as well, and a ModelElement might be parsed
        /// as instance of one of the found classes.
        /// </summary>
        /// <param name="assembly">The assembly that is searched for classes</param>
        void addAssemblyToCheckForTypes(Assembly assembly);
    }
}
