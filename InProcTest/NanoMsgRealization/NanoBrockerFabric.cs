using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace NanoMsgRealization
{
    public class NanoBrockerFabric: IBrockerFabric
    {
        public IBrocker CreateNew()
        {
            return new NanoBrocker(StaticPrefs.ReqRepUri);
        }
    }
}
