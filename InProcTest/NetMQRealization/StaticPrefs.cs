using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;

namespace NetMQRealization
{
    internal static class StaticPrefs
    {
        public static string PullUri => "inproc://ToWorkers";

        public static string PushPullUri => "inproc://FromWorkers";

        public static NetMQContext Context { get; private set; } = NetMQContext.Create();
    }
}
