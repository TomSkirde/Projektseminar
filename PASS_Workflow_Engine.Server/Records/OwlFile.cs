namespace TestUploadAPI.Records
{
    public record OwlFile
    {
        public string Name { get; private set; }
        public string Path { get; private set; }


        public OwlFile(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }

}
