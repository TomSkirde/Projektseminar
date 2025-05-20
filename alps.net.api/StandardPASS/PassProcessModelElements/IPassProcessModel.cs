using alps.net.api.ALPS;
using alps.net.api.parsing;
using System.Collections.Generic;
using VDS.Common.References;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface of the pass process model class
    /// </summary>
    public interface IPASSProcessModel : IPASSProcessModelElement, IImplementingElement<IPASSProcessModel>
    {

        /// <summary>
        /// Sets the base uri for the model.
        /// This uri is important for the exporter to function properly.
        /// Every element inside the model will be parsed with this specified uri.
        /// </summary>
        /// <param name="baseURI">the base uri</param>
        void setBaseURI(string baseURI);

        /// <summary>
        /// Returns the base graph behind the model.
        /// This graph collects all information held by the model in the form of nodes and triples.
        /// This information is redundand for anyone who has access to the model, but is used for exporting the model to owl.
        /// </summary>
        /// <returns>The underlying graph</returns>
        IPASSGraph getBaseGraph();

        /// <summary>
        /// Sets whether the model is layered or not
        /// </summary>
        void setIsMultiLayered(bool layered);

        /// <summary>
        /// Sets whether the model is layered or not
        /// </summary>
        /// <returns>true if layered, false if not</returns>
        bool isLayered();


        // ######################## StartSubject methods ########################

        /// <summary>
        /// Method that sets the start subject attribute of the instance
        /// </summary>
        /// <param name="startSubject">the new start subject</param>
        void addStartSubject(ISubject startSubject);

        /// <summary>
        /// Method that overrides the current set of start subjects
        /// </summary>
        /// <param name="startSubjects"></param>
        void setStartSubjects(ISet<ISubject> startSubjects, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that removes a specified subject as start subject.
        /// Does NOT remove the element from the model completely
        /// </summary>
        /// <param name="id"></param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeStartSubject(string id, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the dictionary of all start subjects
        /// </summary>
        /// <returns>The known start subjects</returns>
        IDictionary<string, ISubject> getStartSubjects();

        // ######################## All contained elements methods ########################

        /// <summary>
        /// Adds a <see cref="IPASSProcessModelElement"/> to the model
        /// </summary>
        /// <param name="pASSProcessModelElement">the new model element</param>
        /// <param name="layerID">the layer it should be added to.
        /// If null, the element will be added to the base (default) layer</param>
        void addElement(IPASSProcessModelElement pASSProcessModelElement, string layerID = null);

        /// <summary>
        /// Overrides the model elements currently contained by the model.
        /// </summary>
        /// <param name="elements">The new model elements</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setAllElements(ISet<IPASSProcessModelElement> elements, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns a dictionary containing all known PASSProcessModelElements (in the current context) mapped with their model component id.
        /// </summary>
        /// <returns>The dict of all PASSProcessModelElements</returns>
        IDictionary<string, IPASSProcessModelElement> getAllElements();

        /// <summary>
        /// Removes a <see cref="IPASSProcessModelElement"/> specified by its id
        /// </summary>
        /// <param name="modelComponentID">the model component id of the element that should be removed</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeElement(string modelComponentID, int removeCascadeDepth = 0);

        // ######################## Contained layer methods ########################

        /// <summary>
        /// Adds a new model Layer to the model
        /// </summary>
        /// <param name="modelLayer">The new model layer</param>
        void addLayer(IModelLayer modelLayer);

        /// <summary>
        /// Overrides the layers currently contained by the model.
        /// </summary>
        /// <param name="modelLayers">The new model layers</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setLayers(ISet<IModelLayer> modelLayers, int removeCascadeDepth = 0);

        /// <summary>
        /// Removes a model layer specified by its model component id
        /// </summary>
        /// <param name="id">the model component id of the layer</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeLayer(string id, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns a dictionary containing all known Model layers (in the current context) mapped with their model component id.
        /// </summary>
        /// <returns>The dict of all model layers</returns>
        IDictionary<string, IModelLayer> getModelLayers();

        /// <summary>
        /// Returns the current base layer (the standard layer of the model)
        /// </summary>
        /// <returns>The current base layer</returns>
        IModelLayer getBaseLayer();

        /// <summary>
        /// Sets a layer as the base layer for this model.
        /// The base layer is the standard layer, and should not extend any other layers.
        /// </summary>
        /// <param name="layer">The model layer</param>
        void setBaseLayer(IModelLayer layer);

        /// <summary>
        /// Exports the current model to the specified path using the underlying OWLGraph and TripleStore.
        /// The result is a owl/rdf file at the specified location.
        /// </summary>
        /// <param name="filepath">The specified location for saving the file</param>
        /// <returns>The absolute path the file was written to.</returns>
        string export(string filepath);

        void setModelGraph(IGraphFactory graphFactory);
    }
}
