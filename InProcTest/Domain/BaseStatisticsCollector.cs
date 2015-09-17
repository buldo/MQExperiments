using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    internal class BaseStatisticsCollector : IStatisticsCollector
    {
        public void RegisterWorker(IWorker worker)
        {
            throw new NotImplementedException();
        }

        public void TaskProcessed(IWorker worker)
        {
            throw new NotImplementedException();
        }
    }
}
