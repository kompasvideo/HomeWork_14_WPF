using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_14_WPF.Logs
{
    class LogProcessor
    {
        public event Action<LogEntry> PostMessage;

        #region пока не используется
        Func<List<LogEntry>> _logs;
        public LogProcessor(Func<List<LogEntry>> logs)
        {
            _logs = logs;
        }
        public void ProcessLogs()
        {
            foreach(var logEntry in _logs.Invoke())
            {
                SaveLogEntry(logEntry);
            }
        }
        #endregion

        /// <summary>
        /// Сохраняет сообщение в лог
        /// </summary>
        /// <param name="logEntry"></param>
        public void SaveLogEntry(LogEntry logEntry)
        {
            
        }


        /// <summary>
        /// Временный пустой конструктор
        /// </summary>
        public LogProcessor()
        {
            PostMessage += SaveLogEntry;
        }

    }
}
