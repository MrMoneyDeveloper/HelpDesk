using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using C8.eServices.Mvc.Keys;
using C8.eServices.Mvc.Models;

namespace C8.eServices.Mvc.Helpers
{
    public class EventLogHelper
    {
        public static void Log( string logEntry )
        {
            if ( !EventLog.SourceExists( EventLogKeys.Source ) )
                EventLog.CreateEventSource( EventLogKeys.Source, EventLogKeys.Log );

            EventLog.WriteEntry( EventLogKeys.Source, logEntry );
        }
    }
}