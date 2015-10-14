
using System;
using System.Collections.Generic;

namespace Domain
{
    public interface IBrocker
    {
        void Process(IEnumerable<Frame> frames);

        event EventHandler<ProcessedEventArgs> FramesProcessed;

        void ConnectToWorkers(int workersCnt);
    }
}