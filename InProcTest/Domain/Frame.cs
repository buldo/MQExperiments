namespace Domain
{
    /// <summary>
    /// Класс, описывающий отдельный фрейм
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// Получает данные фрейма
        /// </summary>
        public int Data { get; }

        /// <summary>
        /// Получает идентификатор очереди
        /// </summary>
        public int QueueId { get; }

        /// <summary>
        /// Создает новый экземпляр классса
        /// </summary>
        /// <param name="queueId">Идентификатор очереди к которой относится паке</param>
        /// <param name="data">Данные внутри фрейма</param>
        internal Frame(int queueId, int data)
        {
            QueueId = queueId;
            Data = data;
        }
    }
}
