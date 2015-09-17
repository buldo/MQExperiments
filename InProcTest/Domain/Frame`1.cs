namespace Domain
{
    /// <summary>
    /// Класс, описывающий отдельный фрейм
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Frame<T>
    {
        /// <summary>
        /// Получает данные фрейма
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// Получает идентификатор очереди
        /// </summary>
        public int QueueId { get; }

        /// <summary>
        /// Создает новый экземпляр классса
        /// </summary>
        /// <param name="queueId">Идентификатор очереди к которой относится паке</param>
        /// <param name="data">Данные внутри фрейма</param>
        Frame(int queueId, T data)
        {
            QueueId = queueId;
            Data = data;
        }
    }
}
