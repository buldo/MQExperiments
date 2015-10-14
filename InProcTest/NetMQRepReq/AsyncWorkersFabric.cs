using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace NetMQRepReq
{
    public class AsyncWorkersFabric : IWorkersFabric
    {
        public IWorker CreateNewWorker(int id)
        {
            return new AsyncWorker(id, StaticPrefs.Context, StaticPrefs.ServerUri);
        }
    }
}
