using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    /// <summary>
    /// Репозиторий фреймов
    /// </summary>
    /// <typeparam name="T">Тип данных внутри фрейма</typeparam>
    public class FrameQueueRepository<T>
    {
        private readonly List<FrameQueue<T>> _queues = new List<FrameQueue<T>>();

        /// <summary>
        /// Добавление очереди в репозиторий
        /// </summary>
        /// <param name="queue">Очередь</param>
        public void Add(FrameQueue<T> queue)
        {
            _queues.Add(queue);
        }

        /// <summary>
        /// Добавление очереди в репозиторий
        /// </summary>
        /// <param name="queues">Очереди</param>
        public void Add(IEnumerable<FrameQueue<T>> queues)
        {
            _queues.AddRange(queues);
        }

        /// <summary>
        /// Получить все очереди
        /// </summary>
        /// <returns>
        /// Список очередей
        /// </returns>
        public List<FrameQueue<T>> GetAll()
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
        public FrameQueue<T> GetById(int id)
        {
            return _queues.Find(o => o.Id == id);
        }
    }
}
