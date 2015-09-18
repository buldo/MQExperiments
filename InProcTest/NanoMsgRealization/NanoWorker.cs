using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Statistics;
using NNanomsg.Protocols;

namespace NanoMsgRealization
{
    public class NanoWorker : BaseWorker
    {
        private readonly string _url;
        private readonly IFormatter _formatter = new BinaryFormatter();

        public NanoWorker(int id, string url) : base(id)
        {
            _url = url;
        }

        public async override Task StartProcessingAsync(CancellationToken ct)
        {
            await Task.Run(() =>
             {
                 using (var socket = new ReplySocket())
                 {
                     socket.Bind(_url);

                        while (!ct.IsCancellationRequested)
                        {
                            var dt = socket.Receive();
                            //var data = (List<Frame>)_formatter.Deserialize(socket.ReceiveStream());
                            //foreach (var frame in data)
                            //{
                            //    ProcessFunction(frame.Data);
                            //}
                            //StatisticsCollector.TaskProcessed(this);
                        }
                 }
             }, ct);
        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
