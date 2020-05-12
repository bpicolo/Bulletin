namespace Bulletin.Storage
{
    public interface IUrlGenerator
    {
        public string AbsoluteUrlFor(string location);
    }
}