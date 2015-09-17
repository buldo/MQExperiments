using System.Collections.Generic;

namespace Domain
{
    /// <summary>
    /// Описывает очередь фреймов
    /// </summary>
    /// <typeparam name="T">Тип фрейма</typeparam>
    public class FrameQueue<T>
    {
        private readonly Queue<Frame<T>> _internalQueue = new Queue<Frame<T>>();

        public int Id { get; }
        
        public FrameQueue(int id)
        {
            Id = id;
        }

        public void Enqueue(Frame<T> item)
        {
            _internalQueue.Enqueue(item);
        }

        public List<Frame<T>> DequeueAll()
        {
            int len = _internalQueue.Count;
            var ret = new List<Frame<T>>(len);

            for (int i = 0; i < len; i++)
            {
                ret.Add(_internalQueue.Dequeue());
            }

            return ret;
        }
    }
}
