using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace NanoMsgRealization
{
    public class NanoWorker : BaseWorker
    {
        public NanoWorker(int id, IStatisticsCollector statisticsCollector) : base(id, statisticsCollector)
        {
        }

        public override void StartProcessing()
        {

        }
    }
}
