using System;

namespace Domain
{
    /// <summary>
    /// Интрерфейс приёмщика сообщений
    /// </summary>
    interface IReceiver
    {
        event EventHandler<EventArgs> ResultReceived;
    }
}
