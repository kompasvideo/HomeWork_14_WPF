using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_14_WPF.Model
{
    abstract class Client
    {
        /// <summary>
        /// Имя клиента
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Сумма на счёте клиента
        /// </summary>
        public uint Money { get; set; }
        /// <summary>
        /// Хранит в тексовом виде к какому департаменту относится клиент
        /// </summary>
        public abstract string Status { get; }
        /// <summary>
        /// Вклад 
        /// </summary>
        public Deposit DepositClient { get; set; }
        /// <summary>
        /// Название вклада
        /// </summary>
        public string DepositClientStr { get; set; }
        /// <summary>
        /// Число дней в месяц
        /// </summary>
        static Random rnd = new Random();

        
        public Client(string name)
        {
            Name = name;
            Money = (uint)rnd.Next(0, 10000);
            DepositClientStr = "Нет";
        }
        public Client(string name, uint Money)
        {
            Name = name;
            this.Money = Money;
            DepositClientStr = "Нет";
        }

        /// <summary>
        /// Возвращяет enum к какому департаменту относится клиент
        /// </summary>
        /// <returns></returns>
        public abstract BankDepartment BankDepartmentProp { get; }
    }
}
