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
using NLog;
using NNanomsg.Protocols;

namespace NanoMsgRealization
{
    public class NanoWorker : BaseWorker
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IFormatter _formatter = new BinaryFormatter();

        private readonly PushSocket _pushSocket = new PushSocket();
        private readonly PullSocket _pullSocket = new PullSocket();

        public NanoWorker(int id, string pushAddress, string pullAddress) : base(id)
        {
            
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
                        var dt = _pullSocket.Receive();
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
                        _pushSocket.Send(ms.ToArray());
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
