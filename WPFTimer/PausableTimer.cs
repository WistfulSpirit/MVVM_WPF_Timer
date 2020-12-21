using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace WPFTimer
{
    public class PausableTimer : DispatcherTimer
    {
        private Stopwatch stopwatch;
        private State state; 
        public PausableTimer(DispatcherPriority priority) : base(priority){
            stopwatch = new Stopwatch();
            state = State.None;
        }
        public State State {
            get => state; 
        }
        public TimeSpan ElapsedTime {
            get {
                return stopwatch.Elapsed;
            }
        }
        public new void Start() {
            base.Start();
            stopwatch.Start();
            state = State.Started;
        }
        public void Pause() {
            stopwatch.Stop();
            state = State.Paused;
        }
        public void Reset() {
            stopwatch.Reset();
            state = State.None;
        }
    }
}
