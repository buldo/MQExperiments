using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain
{
    internal class DataProcessor
    {
        private readonly IBrocker _brocker;

        private readonly Dictionary<int, bool> _unlockedQueues;

        private List<FrameQueue> _queues;

        public DataProcessor(List<FrameQueue> queues, IBrocker brocker, WorkersRepository workersRepository)
        {
            _brocker = brocker;
            _unlockedQueues = queues.ToDictionary(queue => queue.Id, queue => true);
            _queues = queues;
            _brocker.FramesProcessed += BrockerOnFramesProcessed;
        }

        private void BrockerOnFramesProcessed(object sender, ProcessedEventArgs processedEventArgs)
        {
            _unlockedQueues[processedEventArgs.QueueId] = true;
        }

        public async Task StartProcessingAsync(CancellationToken ct)
        {
            await Task.Run(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    foreach (var queue in _queues)
                    {
                        if (_unlockedQueues[queue.Id])
                        {
                            _brocker.Process(queue.DequeueAll());
                        }
                    }
                }
            }, ct);
        }
    }
}
