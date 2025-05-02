/*using alps.net.api.parsing;
using alps.net.api.StandardPASS;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TestUploadAPI.Records;

namespace PASS_Workflow_Engine.Server.HelperClasses
{
    public class AlpsApiHandler
    {
        string PassPath;
        string AlpsPath;
        IPASSReaderWriter io;
        IList<string> Paths;

        public AlpsApiHandler()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            PassPath = Path.Combine(currentDirectory, "PassFiles", "ALPS_ont_v_0.8.0.owl");
            AlpsPath = Path.Combine(currentDirectory, "PassFiles", "ALPS_ont_v_0.8.0.owl");
            io = PASSReaderWriter.getInstance();
            Paths = new List<string> { PassPath, AlpsPath};
        }

        public IDictionary<string,IPASSProcessModelElement> LoadOwl(OwlFile file)
        {
            string CurrentModelPath = file.Path;


            io.loadOWLParsingStructure(Paths);

            IList<IPASSProcessModel> models = io.loadModels(new List<string> { CurrentModelPath });
            if(models == null)
            {
                throw new Exception("Model was not loaded");
            }
            IDictionary<string, IPASSProcessModelElement> processModelElements = models[0].getAllElements();
            return processModelElements;
        }
    }
}
*/