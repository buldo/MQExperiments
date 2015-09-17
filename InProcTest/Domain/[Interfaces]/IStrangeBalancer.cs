
using System;
using System.Collections.Generic;

namespace Domain
{
    public interface IStrangeBalancer<T>
    {
        void Process(IEnumerable<Frame<T>> frames);

        event EventHandler<ProcessedEventArgs> FramesProcessed;
    }
}