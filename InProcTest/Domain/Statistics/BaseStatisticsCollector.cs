using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;

namespace Domain.Statistics
{
    public class BaseStatisticsCollector : IStatisticsCollector
    {
        private List<WorkerStatisticsCounter> _processedTasks { get; } = new List<WorkerStatisticsCounter>();

        public void RegisterWorker([NotNull]IWorker worker)
        {
            if (_processedTasks.Exists(o => o.WorkerId == worker.Id))
            {
                throw new ArgumentException("Element with this id is already created");
            }

            worker.SetStatisticsCollector(this);
            _processedTasks.Add(new WorkerStatisticsCounter(worker.Id));
        }

        public void TaskProcessed([NotNull]IWorker worker)
        {
            _processedTasks.Find(o => o.WorkerId == worker.Id)?.IncreaseCounter();
        }

        public void UnregisterWorker([NotNull]IWorker worker)
        {
            var counter = _processedTasks.Find(o => o.WorkerId == worker.Id);
            if (counter != null)
            {
                _processedTasks.Remove(counter);
            }
        }

        public List<WorkerStatisticsCounter> GetCurrentStatisticsCounters()
        {
            return _processedTasks.Select(o => (WorkerStatisticsCounter)o.Clone()).ToList();
        }
    }
}
