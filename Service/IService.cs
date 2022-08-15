namespace UserCollectionBlaz.Service
{
    public interface IService<T, K>
    {
        void Add(T item);
        void Remove(T item);
        T? Get(K id);
        void Set(K key, T value);
        IEnumerable<T>? GetAll();
        IEnumerable<T>? GetAllByKey(string key);
    }
}
