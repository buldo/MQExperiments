using System;
using Domain.Statistics;
using System.Data.HashFunction.CRCStandards;
using System.Threading;
using System.Threading.Tasks;

namespace Domain
{
    public abstract class BaseWorker : IWorker
    {
        private readonly CRC32 _crc32 = new CRC32();

        protected IStatisticsCollector StatisticsCollector;
        public int Id { get; }

        protected BaseWorker(int id)
        {
            Id = id;
        }

        public abstract Task StartProcessingAsync(CancellationToken ct);

        public void SetStatisticsCollector(IStatisticsCollector statisticsCollector)
        {
            StatisticsCollector = statisticsCollector;
        }
        
        public abstract void Dispose();

        protected void ProcessFunction(int val)
        {
            double a = val;
            for (int i = 0; i < 10; i++)
            {
                a += i;
                //var hash = _crc32.ComputeHash(BitConverter.GetBytes(val));
                //if ((0x2 & hash[0]) == 0x2)
                //{
                //    break;
                //}
            }
        }
    }
}
