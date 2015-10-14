using System;

namespace Domain
{
    /// <summary>
    /// Класс, описывающий отдельный фрейм
    /// </summary>
    [Serializable]
    public class Frame
    {
        /// <summary>
        /// Получает данные фрейма
        /// </summary>
        public int Data { get; set; }

        /// <summary>
        /// Получает идентификатор очереди
        /// </summary>
        public int QueueId { get; set; }

        /// <summary>
        /// Создает новый экземпляр классса
        /// </summary>
        /// <param name="queueId">Идентификатор очереди к которой относится паке</param>
        /// <param name="data">Данные внутри фрейма</param>
        public Frame(int queueId, int data)
        {
            QueueId = queueId;
            Data = data;
        }

        public Frame()
        {
            
        }
    }
}
