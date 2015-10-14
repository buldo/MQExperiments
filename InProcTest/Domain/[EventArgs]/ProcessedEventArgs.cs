using System;

namespace Domain
{
    /// <summary>
    /// Событие завершения обработки части очереди
    /// </summary>
    [Serializable]
    public class ProcessedEventArgs : EventArgs
    {
        public int QueueId { get; set; }

        public ProcessedEventArgs(int id)
        {
            QueueId = id;
        }

        public ProcessedEventArgs()
        {
            
        }
    }
}
