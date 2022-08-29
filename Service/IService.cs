namespace UserCollectionBlaz.Service
{
    public interface IService<T, K>
    {
        Task Add(T item);
        Task Remove(T item);
        T? Get(K id);
        void Set(K key, T value);
        IEnumerable<T>? GetAll();
        IEnumerable<T>? GetAllByKey(string key);
    }
}
