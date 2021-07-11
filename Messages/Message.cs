using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_14_WPF.Messages
{
    class Message
    {
        public static void SendTo(string text)
        {
            File.AppendAllText("messages.log", text + "\n");
        }
    }
}
