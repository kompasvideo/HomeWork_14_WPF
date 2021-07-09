using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_14_WPF.Logs
{
    /// <summary>
    /// Класс описывающий сообщение
    /// </summary>
    class CMessage
    {
        /// <summary>
        /// Текст
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Тип сообщения
        /// </summary>
        public CMessageType MessageType { get; set; }
    }
}
