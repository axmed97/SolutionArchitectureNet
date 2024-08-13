namespace Business.Utilities.Storage.Abstract
{
    public interface IStorageService : IStorage
    {
        public string StorageName { get; }
    }
}
