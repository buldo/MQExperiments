using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace NetMQFanInOut
{
    public class FanBrockersFabric: IBrockersFabric
    {
        public IBrocker CreateNew(int workersCnt)
        {
            return new FanBrocker(StaticPrefs.Context, StaticPrefs.Ventilator, StaticPrefs.Sink,workersCnt);
        }
    }
}
