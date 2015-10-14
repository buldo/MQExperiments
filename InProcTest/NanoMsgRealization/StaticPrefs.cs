namespace NanoMsgRealization
{
    internal static class StaticPrefs
    {
        public static string PullUri => "inproc://ToWorkers";

        public static string PushPullUri => "inproc://FromWorkers";
    }
}
