using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Statistics;

namespace Domain
{
    public class TestModel
    {
        
        private readonly FrameQueuesRepository _frameQueuesRepository = new FrameQueuesRepository();

        private readonly WorkersRepository _workersRepository;

        private readonly IWorkersFabric _workersFabric;

        private readonly IBrockersFabric _brockersFabric;

        private IBrocker _brocker;

        private readonly DataGenerator _dataGenerator = new DataGenerator();

        private readonly CancellationTokenSource _generatorCancellationTokenSource = new CancellationTokenSource();

        private readonly CancellationTokenSource _processorCancellationTokenSource = new CancellationTokenSource();

        public IStatisticsCollector StatisticsCollector { get; }

        public TestModel(IWorkersFabric workersFabric,
                         IBrockersFabric brockersFabric,
                         IStatisticsCollector statisticsCollector)
        {
            StatisticsCollector = statisticsCollector;

            _brockersFabric = brockersFabric;
            _workersRepository = new WorkersRepository(statisticsCollector);
            _workersFabric = workersFabric;
        }

        public async Task StartAsync(int queuesCnt, int workersCnt)
        {
            _brocker = _brockersFabric.CreateNew(workersCnt);
            _frameQueuesRepository.Clear();
            for (int i = 0; i < queuesCnt; i++)
            {
                _frameQueuesRepository.Add(new FrameQueue(i));
            }

            _workersRepository.Clear();
            for (int i = 0; i < workersCnt; i++)
            {
                _workersRepository.Add(_workersFabric.CreateNewWorker(i));
            }

            _brocker.ConnectToWorkers(workersCnt);

            List<Task> tasks = _workersRepository.GetAll().Select(worker => worker.StartProcessingAsync(_processorCancellationTokenSource.Token)).ToList();

            var generatorTask = _dataGenerator.StartGenerationAsync(_frameQueuesRepository, _generatorCancellationTokenSource.Token);
            
            var dataProcessor = new DataProcessor(_frameQueuesRepository.GetAll(), _brocker, _workersRepository);
            var processorTask = dataProcessor.StartProcessingAsync(_processorCancellationTokenSource.Token);

            await generatorTask;
            await processorTask;
            foreach (var task in tasks)
            {
                await task;
            }
        }

        public void Stop()
        {
            _generatorCancellationTokenSource.Cancel();
            _processorCancellationTokenSource.Cancel();
        }
    }
}
