using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Domain;
using NNanomsg;
using NNanomsg.Protocols;

namespace NanoMsgRealization
{
    public class NanoBrocker : IBrocker, IDisposable
    {
        private readonly RequestSocket _socket = new RequestSocket();
        private readonly NanomsgListener _listener = new NanomsgListener(); 
        private readonly IFormatter _formatter = new BinaryFormatter();

        private bool _disposedValue = false; // Для определения избыточных вызовов

        public NanoBrocker(string address)
        {
            _socket.Connect(address);
            _listener.AddSocket(_socket);
            _listener.ReceivedMessage += ListenerOnReceivedMessage;
//            Task.Run(() => _listener.Listen(null));

        }

        public void Process(IEnumerable<Frame> frames)
        {
            using (var ms = new MemoryStream())
            {
                _formatter.Serialize(ms, frames.ToList());
                _socket.Send(ms.ToArray());
            }
        }

        public event EventHandler<ProcessedEventArgs> FramesProcessed;

        protected virtual void OnFramesProcessed(ProcessedEventArgs e)
        {
            FramesProcessed?.Invoke(this, e);
        }

        private void ListenerOnReceivedMessage(int socketId)
        {
            var ba = _socket.Receive();

            using (var ms = new MemoryStream())
            {
                ms.Write(ba,0,ba.Length);
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
                    _socket.Dispose();
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
