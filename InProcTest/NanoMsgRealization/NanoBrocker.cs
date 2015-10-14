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
using NLog;
using NNanomsg;
using NNanomsg.Protocols;

namespace NanoMsgRealization
{
    public class NanoBrocker : IBrocker, IDisposable
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly PullSocket _pullSocket = new PullSocket();
        private readonly NanomsgListener _listener = new NanomsgListener();

        private readonly PushSocket _pushSocket = new PushSocket();

        private readonly BinaryFormatter _formatter = new BinaryFormatter();

        private readonly string _pushAddress;

        private bool _disposedValue = false; // Для определения избыточных вызовов

        public NanoBrocker(string pushAddress, string pullAddress, int workersCnt)
        {
            _logger.Trace("Brocker created");
            _pushAddress = pushAddress;

            //_pushSocket.Bind(pushAddress);
            _pullSocket.Bind(pullAddress);
            _listener.AddSocket(_pullSocket);
            _listener.ReceivedMessage += ListenerOnReceivedMessage;
            
            Task.Run(() => {
                while (true)
                {
                    _listener.Listen(null);
                }
            }
            );

            //Task.Run(() =>
            //{
            //    var _formatter = new BinaryFormatter();
            //    while (true)
            //    {
            //        //var ba = _socket.Receive();
            //        var ba = _pushSocket.Receive();
            //        if (ba != null)
            //        {

            //            using (var ms = new MemoryStream())
            //            {
            //                ms.Write(ba, 0, ba.Length);
            //                ms.Position = 0;
            //                var data = (ProcessedEventArgs) _formatter.Deserialize(ms);
            //                _logger.Trace("Brocker received result queue {0}", data.QueueId);
            //                OnFramesProcessed(data);
            //            }
            //        }
            //        else
            //        {
            //            _logger.Trace("Brocker not received");
            //           Thread.Sleep(200);
            //        }
            //    }
            //});

        }

        public void ConnectToWorkers(int workersCnt)
        {
            for (int i = 0; i < workersCnt; i++)
            {
                _pushSocket.Connect(_pushAddress + i);
            }
        }

        public void Process([NotNull]IEnumerable<Frame> frames)
        {
            var _formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                _formatter.Serialize(ms, frames.ToList());
                ms.Position = 0;
                var array = ms.ToArray();
                _pushSocket.Send(array);
                _logger.Trace("Sended {0}q", frames.FirstOrDefault()?.QueueId??0);
            }
        }

        public event EventHandler<ProcessedEventArgs> FramesProcessed;

        protected virtual void OnFramesProcessed(ProcessedEventArgs e)
        {
            FramesProcessed?.Invoke(this, e);
        }

        private void ListenerOnReceivedMessage(int socketId)
        {
            _logger.Trace("Replay received");
            var ba = _pullSocket.Receive();
            
            using (var ms = new MemoryStream())
            {
                ms.Write(ba,0,ba.Length);
                ms.Position = 0;
                var data = (ProcessedEventArgs)_formatter.Deserialize(ms);
                OnFramesProcessed(data);
            }
        }

        #region IDisposable Support


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _pullSocket.Dispose();
                    _pushSocket.Dispose();
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
