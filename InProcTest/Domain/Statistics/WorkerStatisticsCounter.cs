using System.Threading;

namespace Domain.Statistics
{
    public class WorkerStatisticsCounter
    {
        private long _eventsCnt;

        /// <summary>
        /// Получает идентификатор воркера
        /// </summary>
        public int WorkerId { get; }

        /// <summary>
        /// Получает количество произошедших событий
        /// </summary>
        public long EventsCnt
        {
            get { return _eventsCnt; }
            private set { _eventsCnt = value; }
        }

        public WorkerStatisticsCounter(int id)
        {
            WorkerId = id;
            EventsCnt = 0;
        }

        public void IncreaseCounter()
        {
            Interlocked.Increment(ref _eventsCnt);
        }
    }
}
