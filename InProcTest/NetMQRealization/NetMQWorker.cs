using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Statistics;
using NetMQ;
using NetMQ.Sockets;
using NLog;

namespace NetMQRealization
{
    internal class NetMQWorker : BaseWorker
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IFormatter _formatter = new BinaryFormatter();

        private readonly PushSocket _pushSocket;
        private readonly PullSocket _pullSocket;

        public NetMQWorker(int id, string pushAddress, string pullAddress, NetMQContext context) : base(id)
        {
            _pullSocket = context.CreatePullSocket();
            _pushSocket = context.CreatePushSocket();
            _pushSocket.Connect(pushAddress);
            _pullSocket.Bind(pullAddress+id);
        }

        public async override Task StartProcessingAsync(CancellationToken ct)
        {
            await Task.Run(() =>
            {
                int qId =0;
                while (!ct.IsCancellationRequested)
                {
                    using (var ms = new MemoryStream())
                    {
                        var dt = _pullSocket.ReceiveFrameBytes();
                        _logger.Trace("Worker {0} received", Id);
                        ms.Write(dt,0,dt.Length);
                        ms.Position = 0;
                        var data = (List<Frame>)_formatter.Deserialize(ms);
                        foreach (var frame in data)
                        {
                            ProcessFunction(frame.Data);
                                 
                            qId = frame.QueueId;
                        }
                        _logger.Trace("Worker {0} {1} fr processed", Id, qId);
                        StatisticsCollector.TaskProcessed(this);
                    }

                    if (Id == 3)
                    {
                        while (true)
                        {
                                 
                        }
                    }

                    using (var ms = new MemoryStream())
                    {
                        _formatter.Serialize(ms, new ProcessedEventArgs(qId));
                        _pushSocket.SendFrame(ms.ToArray());
                        _logger.Trace("Worker {0} sended", Id);
                    }
                         
                }
             }, ct);
        }

        public override void Dispose()
        {
            _pullSocket.Dispose();
            _pushSocket.Dispose();
        }
    }
}
