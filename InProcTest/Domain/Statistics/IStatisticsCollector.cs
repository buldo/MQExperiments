namespace Domain.Statistics
{
    public interface IStatisticsCollector
    {
        /// <summary>
        /// Регистрация в системе статистики
        /// </summary>
        /// <param name="worker">
        /// Регистрируемый воркер
        /// </param>
        void RegisterWorker(IWorker worker);

        /// <summary>
        /// Отчёт об обработке задания
        /// </summary>
        /// <param name="worker">
        /// Отчитывающийся воркер
        /// </param>
        void TaskProcessed(IWorker worker);

        /// <summary>
        /// Разрегистрация воркера
        /// </summary>
        /// <param name="worker">
        /// Воркер для деригистрации
        /// </param>
        void UnregisterWorker(IWorker worker);
    }
}
