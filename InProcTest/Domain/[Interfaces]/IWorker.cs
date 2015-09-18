using System;
using System.Threading;
using System.Threading.Tasks;
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
        Task StartProcessingAsync(CancellationToken ct);

        void SetStatisticsCollector(IStatisticsCollector statisticsCollector);
    }
}