using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the DoState class
    /// </summary>

    public interface IDoState : IStandardPASSState, IHasDuration, IHasSiSiCostPerExecution, ICanBeEndState, IHasSiSiEndStayChance, IHasSiSiTimeCategory
    {
        /// <summary>
        /// Overrides the functions that define how incoming data will be mapped to local data
        /// </summary>
        /// <param name="dataMappingIncomingToLocal">the set of functions that define how incoming data will be mapped to local data</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setDataMappingFunctionsIncomingToLocal(ISet<IDataMappingIncomingToLocal> dataMappingIncomingToLocal, int removeCascadeDepth = 0);

        /// <summary>
        /// Adds a new function that defines how incoming data will be mapped to local data
        /// </summary>
        /// <param name="dataMappingIncomingToLocal">the new mapping function</param>
        void addDataMappingFunctionIncomingToLocal(IDataMappingIncomingToLocal dataMappingIncomingToLocal);

        /// <summary>
        /// Gets the set of functions that define how incoming data will be mapped to local data
        /// </summary>
        /// <returns>the set of functions that define how incoming data will be mapped to local data </returns>
        IDictionary<string, IDataMappingIncomingToLocal> getDataMappingFunctionsIncomingToLocal();

        /// <summary>
        /// Removes a data mapping function (outgoing to local)
        /// </summary>
        /// <param name="id">the id of the function</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeDataMappingFunctionIncomingToLocal(string id, int removeCascadeDepth = 0);


        /// <summary>
        /// Overrides the functions that define how local data will be mapped to outgoing data
        /// </summary>
        /// <param name="dataMappingIncomingToLocal">the set of functions that define how local data will be mapped to outgoing data</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setDataMappingFunctionsLocalToOutgoing(ISet<IDataMappingLocalToOutgoing> dataMappingIncomingToLocal, int removeCascadeDepth = 0);

        /// <summary>
        /// Adds a new function that defines how local data will be mapped to outgoing data
        /// </summary>
        /// <param name="dataMappingIncomingToLocal">the new function</param>
        void addDataMappingFunctionLocalToOutgoing(IDataMappingLocalToOutgoing dataMappingIncomingToLocal);

        //// <summary>
        /// Adds a new function that defines how local data will be mapped in general
        /// </summary>
        /// <param name="dataMappingFunction">the new function</param>
        void addDataMappingFunction(PassProcessModelElements.DataDescribingComponents.IDataMappingFunction dataMappingFunction);

         IDictionary<string, PassProcessModelElements.DataDescribingComponents.IDataMappingFunction> getDataMappingFunctions();

        //// <summary>
        /// Removes a data mapping function
        /// </summary>
        /// <param name="id">the id of the function</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeDataMappingFunction(string id, int removeCascadeDepth = 0);

        /// <summary>
        /// Gets the set of functions that define how local data will be mapped to outgoing data
        /// </summary>
        /// <returns>the set of functions that define how incoming data will be mapped to local data</returns>
        IDictionary<string, IDataMappingLocalToOutgoing> getDataMappingFunctionsLocalToOutgoing();

        /// <summary>
        /// Removes a data mapping function (local to outgoing)
        /// </summary>
        /// <param name="id">the id of the function</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeDataMappingFunctionLocalToOutgoing(string id, int removeCascadeDepth = 0);

        /// <summary>
        /// Sets the function specification 
        /// </summary>
        /// <param name="specification">the function specification</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        new void setFunctionSpecification(IFunctionSpecification specification, int removeCascadeDepth = 0);
        /// <summary>
        /// Gets the function specification 
        /// </summary>
        /// <returns>>the function specification</returns>
        new IDoFunction getFunctionSpecification();

        

    }

}
