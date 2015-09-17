using Domain;
using Domain.Statistics;

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

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
