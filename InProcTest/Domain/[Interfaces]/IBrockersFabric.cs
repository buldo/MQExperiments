namespace Domain
{
    public interface IBrockersFabric
    {
        IBrocker CreateNew(int workersCnt);
    }
}
