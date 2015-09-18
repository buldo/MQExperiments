using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace NanoMsgRealization
{
    public class NanoBrockersFabric: IBrockersFabric
    {
        public IBrocker CreateNew(int workersCnt)
        {
            return new NanoBrocker(StaticPrefs.ReqRepUri, StaticPrefs.PushPullUri, workersCnt);
        }
    }
}
