using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Net.Helpers
{
    public class SimpleLog
    {

        public static void WriteToEventLog(string msg, string source) {
            

            if (!EventLog.SourceExists(source)) { 
                EventSourceCreationData sourceData = new EventSourceCreationData(source, source);

                EventLog.CreateEventSource(sourceData);
            }

            EventLog log = new EventLog();
            log.Source = source;
            log.WriteEntry(msg);
        }

    }
}
