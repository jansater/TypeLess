using System;
using System.Diagnostics;

namespace TypeLess.Debugging
{
    public class PerfLogger
    {
        private long ticks;
        private long ticksBetween; 
        private string msg;

        private Action<string> _output;

        public PerfLogger Start(string msg, Action<string> output = null)
        {
            ticks = DateTime.UtcNow.Ticks;
            ticksBetween = ticks;
            this.msg = msg;

            if (output == null)
            {
#if DEBUG
                _output = x => Debug.WriteLine(x);
#else
                _output = x => {};
#endif
            }
            else {
                _output = output;
            }

            _output("START -> " + msg);
            return this;
        }

        public TimeSpan Total { get; set; }

        #if !DEBUG
        [Conditional("DEBUG")]
        #endif
        public void Log(string msg2 = null) {
            if (msg2 != null)
            {
                _output(msg + " MSG: " + msg2 + " Duration: " + TimeSpan.FromTicks(DateTime.UtcNow.Ticks - ticksBetween).ToString("mm\\:ss\\:fff") + " Time: " + TimeSpan.FromTicks(DateTime.UtcNow.Ticks - ticks).ToString("mm\\:ss\\:fff"));
                ticksBetween = DateTime.UtcNow.Ticks;
            }
            else {
                Total = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - ticks);
               _output("END -> " + msg + " TOTAL Duration: " + Total.ToString("mm\\:ss\\:fff"));
            }
            
        }
    }
}
