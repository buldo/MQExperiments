using System;

namespace Domain
{
    /// <summary>
    /// Событие завершения обработки части очереди
    /// </summary>
    [Serializable]
    public class ProcessedEventArgs : EventArgs
    {
        public int QueueId { get; }

        public ProcessedEventArgs(int id)
        {
            QueueId = id;
        }
    }
}
