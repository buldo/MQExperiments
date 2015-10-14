using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    /// <summary>
    /// Репозиторий фреймов
    /// </summary>
    public class FrameQueuesRepository
    {
        private readonly List<FrameQueue> _queues = new List<FrameQueue>();

        /// <summary>
        /// Добавление очереди в репозиторий
        /// </summary>
        /// <param name="queue">Очередь</param>
        public void Add(FrameQueue queue)
        {
            _queues.Add(queue);
        }

        /// <summary>
        /// Добавление очереди в репозиторий
        /// </summary>
        /// <param name="queues">Очереди</param>
        public void Add(IEnumerable<FrameQueue> queues)
        {
            _queues.AddRange(queues);
        }

        /// <summary>
        /// Получить все очереди
        /// </summary>
        /// <returns>
        /// Список очередей
        /// </returns>
        public List<FrameQueue> GetAll()
        {
            return _queues.ToList();
        }

        /// <summary>
        /// Получить конкретную очередь
        /// </summary>
        /// <param name="id">
        /// Идентификатор очереди
        /// </param>
        /// <returns>
        /// Очередь с соответствующим Id или null
        /// </returns>
        public FrameQueue GetById(int id)
        {
            return _queues.Find(o => o.Id == id);
        }


        /// <summary>
        /// Очистка репозитория
        /// </summary>
        public void Clear()
        {
            _queues.Clear();
        }
    }
}
