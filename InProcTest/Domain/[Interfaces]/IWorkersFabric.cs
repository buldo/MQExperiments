namespace Domain
{
    interface IWorkersFabric
    {
        IWorker CreateNewWorker(int id);
    }
}
