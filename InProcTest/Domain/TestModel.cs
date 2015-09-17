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

        private readonly IBrocker _brocker;

        private readonly DataGenerator _dataGenerator = new DataGenerator();

        private readonly CancellationTokenSource _generatorCancellationTokenSource = new CancellationTokenSource();

        private readonly CancellationTokenSource _processorCancellationTokenSource = new CancellationTokenSource();

        public IStatisticsCollector StatisticsCollector { get; }

        public TestModel(IWorkersFabric workersFabric,
                         IBrockersFabric brockersFabric,
                         IStatisticsCollector statisticsCollector)
        {
            StatisticsCollector = statisticsCollector;

            _workersRepository = new WorkersRepository(statisticsCollector);
            _workersFabric = workersFabric;

            _brocker = brockersFabric.CreateNew();
        }

        public async Task StartAsync(int queuesCnt, int workersCnt)
        {
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

            var generatorTask = _dataGenerator.StartGenerationAsync(_frameQueuesRepository, _generatorCancellationTokenSource.Token);
            
            var dataProcessor = new DataProcessor(_frameQueuesRepository.GetAll(), _brocker);
            var processorTask = dataProcessor.StartGeneratingAsync(_processorCancellationTokenSource.Token);

            await generatorTask;
            await processorTask;
        }

        public void Stop()
        {
            _generatorCancellationTokenSource.Cancel();
            _processorCancellationTokenSource.Cancel();
        }
    }
}
