using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Statistics;
using JetBrains.Annotations;

namespace Domain
{
    public class WorkersRepository
    {
        private readonly List<IWorker> _workers = new List<IWorker>();

        private readonly IStatisticsCollector _statisticsCollector;

        public WorkersRepository([NotNull]IStatisticsCollector statisticsCollector)
        {
            _statisticsCollector = statisticsCollector;
        }

        public void Add(IWorker worker)
        {
            if (_workers.Exists(o => o.Id == worker.Id))
            {
                throw new ArgumentException("Id already exists");
            }

            _statisticsCollector.RegisterWorker(worker);
            _workers.Add(worker);
        }

        public void Clear()
        {
            foreach (var worker in _workers)
            {
                _statisticsCollector.UnregisterWorker(worker);
                worker.Dispose();
            }

            _workers.Clear();
        }
    }
}
