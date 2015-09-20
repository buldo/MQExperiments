using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Domain;
using NetMQ;
using NetMQ.Sockets;
using NLog;
using NetMQContext = NetMQ.NetMQContext;

namespace NetMQRepReq
{
    internal class AsyncBrocker : IBrocker
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly DealerSocket _socket;
        private Poller _poller = new Poller();

        private readonly BinaryFormatter _formatter = new BinaryFormatter();

        public AsyncBrocker(NetMQContext context, string address)
        {
            _socket = context.CreateDealerSocket();
            _socket.Options.SendHighWatermark = 1;
            _socket.Options.ReceiveHighWatermark = 1;
            _socket.Bind(address);
            _socket.ReceiveReady += SocketReceiveReady;
            _poller.AddSocket(_socket);
            _poller.PollTillCancelledNonBlocking();
        }
        public void Process(IEnumerable<Frame> frames)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    _formatter.Serialize(ms, frames.ToList());
                    ms.Position = 0;
                    var mqMessage = new NetMQMessage();
                    mqMessage.AppendEmptyFrame();
                    mqMessage.Append(ms.ToArray());
                    _socket.SendMultipartMessage(mqMessage);
                    _logger.Trace("Sended {0}q", frames.FirstOrDefault()?.QueueId ?? 0);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        public event EventHandler<ProcessedEventArgs> FramesProcessed;
        
        protected virtual void OnFramesProcessed(ProcessedEventArgs e)
        {
            FramesProcessed?.Invoke(this, e);
        }

        private void SocketReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            var ba = e.Socket.ReceiveMultipartMessage();
            if (ba != null)
            {
                using (var ms = new MemoryStream())
                {
                    ms.Write(ba[2].ToByteArray(), 0, ba[2].MessageSize);
                    ms.Position = 0;
                    var data = (ProcessedEventArgs)_formatter.Deserialize(ms);
                    _logger.Trace("Brocker received result queue {0}", data.QueueId);
                    OnFramesProcessed(data);
                }
            }
            else
            {
                _logger.Trace("Brocker not received");
            }
        }

        public void ConnectToWorkers(int workersCnt)
        {
            
        }
    }
}
