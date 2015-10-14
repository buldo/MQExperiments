using NetMQ;

namespace NetMQRepReq
{
    internal static class StaticPrefs
    {
        public static string ServerUri => "inproc://Server";

        public static NetMQContext Context { get; private set; } = NetMQContext.Create();
    }
}
