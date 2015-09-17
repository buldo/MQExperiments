using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
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
    }
}
