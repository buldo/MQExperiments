using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace NanoMsgRealization
{
    public class NanoBalancer : IStrangeBalancer
    {
        public NanoBalancer()
        {
            
        }

        public void Process(IEnumerable<Frame> frames)
        {
            
        }

        public event EventHandler<ProcessedEventArgs> FramesProcessed;

        protected virtual void OnFramesProcessed(ProcessedEventArgs e)
        {
            FramesProcessed?.Invoke(this, e);
        }
    }
}
