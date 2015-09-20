using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using JetBrains.Annotations;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using NLog;

namespace NetMQFanInOut
{
    internal class FanBrocker : IBrocker, IDisposable
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly PullSocket _sinkSocket;
        private readonly PushSocket _ventSocket;

        private readonly string _sinkAddress;
        private readonly string _ventAddress;

        private readonly BinaryFormatter _formatter = new BinaryFormatter();

        private bool _disposedValue = false; // Для определения избыточных вызовов

        public FanBrocker(NetMQContext context, string ventAddress, string sinkAddress, int workersCnt)
        {
            _logger.Trace("Brocker created");
            _ventAddress = ventAddress;
            _sinkAddress = sinkAddress;

            _sinkSocket = context.CreatePullSocket();
            _sinkSocket.Options.ReceiveBuffer = 1;
            
            _sinkSocket.Bind(sinkAddress);

            _ventSocket = context.CreatePushSocket();
            _ventSocket.Options.SendBuffer = 1;
            _ventSocket.Bind(ventAddress);
            
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        var ba = _sinkSocket.ReceiveFrameString();
                        _logger.Trace("Brocker received data {0}", ba);
                        var data = JsonConvert.DeserializeObject<ProcessedEventArgs>(ba);
                        OnFramesProcessed(data);
                    }
                }
                catch (Exception)
                {

                    _logger.Error("EXCEPTION");
                }

            });

        }

        public void ConnectToWorkers(int workersCnt)
        {
        }

        public void Process(IEnumerable<Frame> frames)
        {
            var str = JsonConvert.SerializeObject(frames);
            if (str == "[]")
            {
                _logger.Error("Smth wrong");
            }

            _ventSocket.SendFrame(str);
            _logger.Trace("Brocker sended data {0}", str);
        }

        public event EventHandler<ProcessedEventArgs> FramesProcessed;

        
        protected virtual void OnFramesProcessed(ProcessedEventArgs e)
        {
            FramesProcessed?.Invoke(this, e);
        }
        #region IDisposable Support


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _sinkSocket.Dispose();
                    _ventSocket.Dispose();
                }

                // освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // задать большим полям значение NULL.

                _disposedValue = true;
            }
        }

        // переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~NanoBalancer() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
