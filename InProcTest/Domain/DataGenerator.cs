using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain
{
    internal class DataGenerator
    {
        public async Task StartGenerationAsync(FrameQueuesRepository repository, CancellationToken ct)
        {
            await Task.Run(() =>
            {
                var random = new Random();
                var list = repository.GetAll();

                while (!ct.IsCancellationRequested)
                {
                    Thread.Sleep(500);
                    foreach (var queue in list)
                    {
                        int cntToAdd = random.Next(1, 5);
                        for (int i = 0; i < cntToAdd; i++)
                        {
                            queue.Enqueue(new Frame(queue.Id, random.Next(1,50000)));
                            ct.ThrowIfCancellationRequested();
                        }
                    }
                }
            }, ct);
        }
    }
}
