using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_14_WPF.Logs
{
    /// <summary>
    /// Класс описывающий запись в журнале
    /// </summary>
    class LogEntry
    {
        /// <summary>
        /// Дата и время
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public CMessage Message { get; set; }
    }
}
