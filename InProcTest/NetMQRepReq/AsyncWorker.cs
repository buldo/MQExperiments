using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using NetMQ;
using NetMQ.Sockets;
using NLog;

namespace NetMQRepReq
{
    internal class AsyncWorker : BaseWorker
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IFormatter _formatter = new BinaryFormatter();

        private readonly ResponseSocket _responseSocket;

        public AsyncWorker(int id, NetMQContext context, string address) : base(id)
        {
            _responseSocket = context.CreateResponseSocket();
            _responseSocket.Options.Identity = BitConverter.GetBytes(id);
            _responseSocket.Connect(address);
        }

        public override async Task StartProcessingAsync(CancellationToken ct)
        {
            await Task.Run(() =>
            {
                int qId = 0;
                while (!ct.IsCancellationRequested)
                {
                    using (var ms = new MemoryStream())
                    {
                        var dt = _responseSocket.ReceiveMultipartMessage();
                        _logger.Trace("Worker {0} received", Id);
                        ms.Write(dt[0].ToByteArray(), 0, dt[0].MessageSize);
                        ms.Position = 0;
                        var data = (List<Frame>) _formatter.Deserialize(ms);
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
                        var mqMessage = new NetMQMessage();
                        mqMessage.AppendEmptyFrame();
                        mqMessage.Append(ms.ToArray());
                        _responseSocket.SendMultipartMessage(mqMessage);
                        _logger.Trace("Worker {0} sended", Id);
                    }

                }
            }, ct);
        }

        public override void Dispose()
        {
            _responseSocket.Dispose();
        }
    }
}