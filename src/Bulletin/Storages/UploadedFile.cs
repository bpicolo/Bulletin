namespace Bulletin
{
    class UploadedFile
    {
        public string Location { get; }
        public string Extension { get; }

        public UploadedFile(string location,  string extension)
        {
            Location = location;
            Extension = extension;
        }
    }
}
