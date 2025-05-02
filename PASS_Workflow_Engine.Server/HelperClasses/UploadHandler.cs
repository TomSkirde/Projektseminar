namespace PASS_Workflow_Engine
{
    public class UploadHandler
    {
        public string Upload(IFormFile owlfile)
        {
            //extention
            List<string> validExtentions = new List<string>() { ".owl" };
            string extention = Path.GetExtension(owlfile.FileName);
            if (!validExtentions.Contains(extention))
            {
                return "Extention is not valid needs to be .owl";
            }

            //name changing
            string fileName = Path.GetFileName(owlfile.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "UploadFolder");
            using FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            owlfile.CopyTo(stream);


            return fileName;
        }
    }
}
