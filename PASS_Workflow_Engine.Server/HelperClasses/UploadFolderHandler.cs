using TestUploadAPI.Records;

namespace PASS_Workflow_Engine.Server.HelperClasses
{
    public class UploadFolderHandler
    {

        public string GetJsonsOfModels()
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadFolder");
            string[] modelFiles = Directory.GetFiles(directoryPath);

            var owlFiles = new List<OwlFile>();

            for (int i = 0; i < modelFiles.Length; i++)
            {
                owlFiles.Add(new OwlFile(Path.GetFileName(modelFiles[i]), modelFiles[i]));
            }

            string json = System.Text.Json.JsonSerializer.Serialize(owlFiles);

            return json;
        }


    }
}
