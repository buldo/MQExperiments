using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace NetMQRealization
{
    public class NetMQBrockersFabric: IBrockersFabric
    {
        public IBrocker CreateNew(int workersCnt)
        {
            return new NetMQBrocker(pushAddress:StaticPrefs.PushPullUri, pullAddress:StaticPrefs.PullUri, workersCnt:workersCnt, context: StaticPrefs.Context);
        }
    }
}
