namespace Domain
{
    public abstract class BaseWorker : IWorker
    {
        protected readonly IStatisticsCollector StatisticsCollector;
        public int Id { get; }

        protected BaseWorker(int id, IStatisticsCollector statisticsCollector)
        {
            Id = id;
            StatisticsCollector = statisticsCollector;
        }

        public abstract void StartProcessing();

    }
}
