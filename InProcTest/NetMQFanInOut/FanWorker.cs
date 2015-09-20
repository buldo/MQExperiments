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
using Newtonsoft.Json;
using NLog;

namespace NetMQFanInOut
{
    internal class FanWorker : BaseWorker
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IFormatter _formatter = new BinaryFormatter();

        private readonly string _ventAddress;
        private readonly string _sinkAddress;

        private readonly NetMQContext _context;

        public FanWorker(int id, NetMQContext context, string ventAddress, string sinkAddress) : base(id)
        {
            _context = context;
            _sinkAddress = sinkAddress;
            _ventAddress = ventAddress;
        }

        public async override Task StartProcessingAsync(CancellationToken ct)
        {
            await Task.Run(() =>
            {
                int qId =0;
                using (var receiver = _context.CreatePullSocket())
                {
                    receiver.Options.ReceiveBuffer = 1;
                    receiver.Connect(_ventAddress);
                    using (var sender = _context.CreatePushSocket())
                    {
                        sender.Connect(_sinkAddress);
                        sender.Options.SendBuffer = 1;
                        while (!ct.IsCancellationRequested)
                        {
                            var dt = receiver.ReceiveFrameString();
                            if (string.IsNullOrWhiteSpace(dt)) 
                                continue;
                            _logger.Trace("Worker {0} received data {1}", Id, dt);
                             
                            var data = JsonConvert.DeserializeObject<List<Frame>>(dt);

                            foreach (var frame in data)
                            {
                                ProcessFunction(frame.Data);
                                qId = frame.QueueId;
                            }
                            StatisticsCollector.TaskProcessed(this);

                            if (Id == 3)
                            {
                                while (true)
                                {

                                }
                            }

                            var evArgs = new ProcessedEventArgs(qId);
                            var toSend = JsonConvert.SerializeObject(evArgs);
                            sender.SendFrame(toSend);
                            _logger.Trace("Worker {0} sended data {1}", Id, toSend);
                        }
                    }
                }
                
             }, ct);
        }

        public override void Dispose()
        {
        }
    }
}
