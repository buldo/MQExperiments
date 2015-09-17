using System.Collections.Generic;

namespace Domain
{
    /// <summary>
    /// Описывает очередь фреймов
    /// </summary>
    public class FrameQueue
    {
        private readonly Queue<Frame> _internalQueue = new Queue<Frame>();

        public int Id { get; }
        
        public FrameQueue(int id)
        {
            Id = id;
        }

        public void Enqueue(Frame item)
        {
            _internalQueue.Enqueue(item);
        }

        public List<Frame> DequeueAll()
        {
            int len = _internalQueue.Count;
            var ret = new List<Frame>(len);

            for (int i = 0; i < len; i++)
            {
                ret.Add(_internalQueue.Dequeue());
            }

            return ret;
        }
    }
}
