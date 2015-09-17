
using System;
using System.Collections.Generic;

namespace Domain
{
    public interface IStrangeBalancer
    {
        void Process(IEnumerable<Frame> frames);

        event EventHandler<ProcessedEventArgs> FramesProcessed;
    }
}