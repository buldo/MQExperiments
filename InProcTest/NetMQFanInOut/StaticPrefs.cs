using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;

namespace NetMQFanInOut
{
    internal static class StaticPrefs
    {
        public static string Ventilator => "inproc://Ventilator";

        public static string Sink => "inproc://Sink";

        //public static string Ventilator => "tcp://localhost:5559";

        //public static string Sink => "tcp://localhost:5558";

        public static NetMQContext Context { get; private set; } = NetMQContext.Create();
    }
}
