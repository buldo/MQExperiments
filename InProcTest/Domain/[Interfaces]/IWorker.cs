using System;
using Domain.Statistics;

namespace Domain
{
    /// <summary>
    /// Интерфейс воркера
    /// </summary>
    public interface IWorker : IDisposable
    {
        /// <summary>
        /// Идентификатор воркера
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Начать обработку сообщений
        /// </summary>
        void StartProcessing();

        void SetStatisticsCollector(IStatisticsCollector statisticsCollector);
    }
}