using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace InProcTest
{
    class StatViewModel : BindableBase
    {
        private long _counter;
        public int WorkerId { get; set; }

        public long Counter
        {
            get { return _counter; }
            set
            {
                SetProperty(ref _counter, value);
                _counter = value;
            }
        }
    }
}
