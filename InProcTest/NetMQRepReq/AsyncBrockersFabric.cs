using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace NetMQRepReq
{
    public class AsyncBrockersFabric : IBrockersFabric
    {
        public IBrocker CreateNew(int workersCnt)
        {
            return new AsyncBrocker(StaticPrefs.Context, StaticPrefs.ServerUri);
        }
    }
}
