namespace Domain
{
    /// <summary>
    /// Интерфейс воркера
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        /// Идентификатор воркера
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Начать обработку сообщений
        /// </summary>
        void StartProcessing();
    }
}