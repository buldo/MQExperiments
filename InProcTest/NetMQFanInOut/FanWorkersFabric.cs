using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace NetMQFanInOut
{
    public class FanWorkersFabric : IWorkersFabric
    {
        public IWorker CreateNewWorker(int id)
        {
            return new FanWorker(id, StaticPrefs.Context, StaticPrefs.Ventilator, StaticPrefs.Sink);
        }
    }
}
