using alps.net.api.parsing;
using alps.net.api.StandardPASS;

namespace PASS_Workflow_Engine.Server.HelperClasses
{
    /// <summary>
    /// Singleton class to parse ALPS models using the ALPS API.
    /// </summary>
    /// <remarks>
    /// The code implements a singleton pattern so no multiple instances of the parser are created within one session, because loading the ontology files through <c>PASSReaderWriter.loadOWLParsingStructure</c>, provided by the alps.net.api is resource intensive.
    /// </remarks>
    public class AlpsParser
    {
        /// <summary>
        /// Singleton instance of the AlpsParser class.
        /// </summary>
        private static AlpsParser instance;
        /// <summary>
        /// Instance of the IPASSReaderWriter interface for parsing PASS models.
        /// </summary>
        private IPASSReaderWriter parser;
        /// <summary>
        /// List of file paths to the ontologies used for parsing.
        /// </summary>
        private IList<string> ontologyPaths;

        /// <summary>
        /// Private constructor to initialize the AlpsParser instance according to the singleton pattern.
        /// </summary>
        private AlpsParser()
        {
            parser = PASSReaderWriter.getInstance();
            ontologyPaths = new List<string>
                {
                    "C:\\Users\\oster\\source\\repos\\ConsoleApp1\\ConsoleApp1\\Ontologies\\ALPS_ont_v_0.8.0.owl",
                    "C:\\Users\\oster\\source\\repos\\ConsoleApp1\\ConsoleApp1\\Ontologies\\standard_PASS_ont_dev.owl",
                };
            parser.loadOWLParsingStructure(ontologyPaths);
        }

        /// <summary>
        /// Gets the singleton instance of the AlpsParser class.
        /// </summary>
        /// <returns>
        /// The instance of the <c>AlpsParser</c> class.
        /// </returns>
        public static AlpsParser GetAlpsParser()
        {
            if (instance == null)
            {
                instance = new AlpsParser();
            }
            return instance;
        }

        /// <summary>
        /// Method to parse one selected model from the given OWL file path.
        /// </summary>
        /// <param name="OwlPath">String that contains full path to selected OWL file inside "/PASS_Workflow_Engine.Server/UploadFolder"</param>
        public void ParseModels(string OwlPath)
        {
            IList<IPASSProcessModel> models = parser.loadModels(new List<string> { OwlPath });
            // TODO: Implement a method to save the models into corresponding classes of PASS_Workflow_Engine.Server (e.g. SBD, SID, Subject, etc.)
        }
    }
}
