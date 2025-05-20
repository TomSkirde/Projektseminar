using alps.net.api.parsing;
using alps.net.api.StandardPASS;


namespace PASS_Workflow_Engine.Server.HelperClasses
{
    public class AlpsApi
    {
        IPASSReaderWriter io;
        IList<string> paths;
        IList<IPASSProcessModel> models;

        public AlpsApi()
        {
            io = PASSReaderWriter.getInstance();
            paths = new List<string>
            {
                "C:\\Users\\oster\\source\\repos\\ConsoleApp1\\ConsoleApp1\\Ontologies\\ALPS_ont_v_0.8.0.owl",
                "C:\\Users\\oster\\source\\repos\\ConsoleApp1\\ConsoleApp1\\Ontologies\\standard_PASS_ont_dev.owl",
            };
            io.loadOWLParsingStructure(paths);
            this.models = io.loadModels(new List<string> { "C:\\Users\\oster\\source\\repos\\ConsoleApp1\\ConsoleApp1\\OWL Models\\Urlaubsantrag_Discord_Uebung.owl" });

            Console.WriteLine(this.models[0].getAllElements());
        }

        public void parseModels()
        {
        }

    }
}
