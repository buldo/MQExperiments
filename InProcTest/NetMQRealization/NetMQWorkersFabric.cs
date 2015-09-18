using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace NetMQRealization
{
    public class NetMQWorkersFabric : IWorkersFabric
    {
        public IWorker CreateNewWorker(int id)
        {
            return new NetMQWorker(id, StaticPrefs.PullUri, StaticPrefs.PushPullUri, StaticPrefs.Context);
        }
    }
}
