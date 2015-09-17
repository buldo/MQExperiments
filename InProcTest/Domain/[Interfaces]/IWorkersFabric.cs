namespace Domain
{
    public interface IWorkersFabric
    {
        IWorker CreateNewWorker(int id, IStatisticsCollector collector);
    }
}
