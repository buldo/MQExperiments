using System;
using System.Threading;

namespace Domain.Statistics
{
    public class WorkerStatisticsCounter : ICloneable
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

        public object Clone()
        {
            return new WorkerStatisticsCounter(WorkerId) {EventsCnt = EventsCnt};
        }
    }
}
