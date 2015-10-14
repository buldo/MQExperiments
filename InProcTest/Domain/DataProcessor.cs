using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace Domain
{
    internal class DataProcessor
    {
        object lockobk = new object();
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IBrocker _brocker;

        private readonly Dictionary<int, bool> _unlockedQueues;

        private List<FrameQueue> _queues;

        public DataProcessor(List<FrameQueue> queues, IBrocker brocker, WorkersRepository workersRepository)
        {
            _brocker = brocker;
            _unlockedQueues = queues.ToDictionary(queue => queue.Id, queue => true);
            _queues = queues;
            foreach (var worker in workersRepository.GetAll())
            {
                worker.Ready += WorkerOnReady;
            }
        }

        private void WorkerOnReady(object sender, ProcessedEventArgs processedEventArgs)
        {
            lock (lockobk)
            {
                _logger.Trace("queue {0} unlocked", processedEventArgs.QueueId);
                _unlockedQueues[processedEventArgs.QueueId] = true;
            }
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
                            var toProcess = queue.DequeueAll();
                            if (toProcess.Count != 0)
                            {
                                lock (lockobk)
                                {
                                    _unlockedQueues[queue.Id] = false;
                                    _logger.Trace("queue {0} locked", queue.Id);
                                }

                                _brocker.Process(toProcess);
                            }
                        }
                    }
                }
            }, ct);
        }
    }
}
