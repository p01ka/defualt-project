namespace IdxZero.Base.Pools
{
    public interface IPoolable
    {
        void GetFromPool();

        void SetToPool();
    }
}