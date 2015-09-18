using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Domain;
using Domain.Statistics;
using Microsoft.Practices.Unity;
using NanoMsgRealization;
using NetMQRealization;
using Prism.Commands;
using Prism.Mvvm;
using Xceed.Wpf.DataGrid.Converters;

namespace InProcTest
{
    class MainWindowViewModel : BindableBase
    {
        private TestModel _model;

        private IStatisticsCollector _statisticsCollector;

        private Timer _timer;
        
        private bool _isStarted;

        public int WorkersCnt { get; set; } = 4;

        public int QueuesCnt { get; set; } = 20;

        public ICommand StartCommand { get; }

        public ICommand StopCommand { get; }

        public ObservableCollection<StatViewModel> StatisticsCollection { get; } = new ObservableCollection<StatViewModel>();

        public MainWindowViewModel()
        {
            _timer = new Timer(UpdateStatistics);

            _statisticsCollector = new BaseStatisticsCollector();
            //_model = new TestModel(new NanoWorkersFabric(), new NanoBrockersFabric(), _statisticsCollector);
            _model = new TestModel(new NetMQWorkersFabric(), new NetMQBrockersFabric(), _statisticsCollector);

            StartCommand = new DelegateCommand(ExecuteMethod, () => !_isStarted);
            StopCommand = new DelegateCommand(() => _model.Stop(), () => _isStarted);
        }
        
        private async void ExecuteMethod()
        {
            StatisticsCollection.Clear();
            for (int i = 0; i < WorkersCnt; i++)
            {
                StatisticsCollection.Add(new StatViewModel() {WorkerId = i});
            }
            _isStarted = true;
            _timer.Change(0, 100);
            await _model.StartAsync(QueuesCnt, WorkersCnt);
            
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _isStarted = false;
        }

        private void UpdateStatistics(object state)
        {
            var currentState = _statisticsCollector.GetCurrentStatisticsCounters();
            foreach (var counter in currentState)
            {
                var forUpdate = StatisticsCollection.FirstOrDefault(o => o.WorkerId == counter.WorkerId);
                if (forUpdate != null)
                {
                    forUpdate.Counter = counter.EventsCnt;
                }
            }
        }
    }
}
