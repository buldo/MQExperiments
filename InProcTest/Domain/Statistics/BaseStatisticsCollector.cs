using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;

namespace Domain.Statistics
{
    internal class BaseStatisticsCollector : IStatisticsCollector
    {
        public List<WorkerStatisticsCounter> ProcessedTasks { get; } = new List<WorkerStatisticsCounter>();

        public void RegisterWorker([NotNull]IWorker worker)
        {
            if (ProcessedTasks.Exists(o => o.WorkerId == worker.Id))
            {
                throw new ArgumentException("Element with this id is already created");
            }

            ProcessedTasks.Add(new WorkerStatisticsCounter(worker.Id));
        }

        public void TaskProcessed([NotNull]IWorker worker)
        {
            ProcessedTasks.Find(o => o.WorkerId == worker.Id)?.IncreaseCounter();
        }

        public void UnregisterWorker([NotNull]IWorker worker)
        {
            var counter = ProcessedTasks.Find(o => o.WorkerId == worker.Id);
            if (counter != null)
            {
                ProcessedTasks.Remove(counter);
            }
        }
    }
}
