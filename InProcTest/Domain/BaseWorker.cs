using Domain.Statistics;

namespace Domain
{
    public abstract class BaseWorker : IWorker
    {
        protected IStatisticsCollector StatisticsCollector;
        public int Id { get; }

        protected BaseWorker(int id)
        {
            Id = id;
        }

        public void SetStatisticsCollector(IStatisticsCollector statisticsCollector)
        {
            StatisticsCollector = statisticsCollector;
        }

        public abstract void StartProcessing();
        
        public abstract void Dispose();
    }
}
