using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace NanoMsgRealization
{
    public class NanoWorkersFabric : IWorkersFabric
    {
        public IWorker CreateNewWorker(int id)
        {
            return new NanoWorker(id, StaticPrefs.PullUri, StaticPrefs.PushPullUri);
        }
    }
}
