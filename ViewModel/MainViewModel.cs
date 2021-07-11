using HomeWork_14_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using System.Windows.Input;
using HomeWork_14_WPF.View;
using System.Windows;

namespace HomeWork_14_WPF.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Названия департаментов банка
        /// </summary>
        public ObservableCollection<string> departments { get; set; } = new ObservableCollection<string>()
            {Const.personalName, Const.businessName, Const.VIPName};
        /// <summary>
        /// Список клиентов банка
        /// </summary>
        public static ObservableCollection<Client> clients { get; set; } = new ObservableCollection<Client>();
        Random rnd = new Random();
        /// <summary>
        /// Выбранный клинт в списке
        /// </summary>
        public static Client SelectedClient { get; set; }
        /// <summary>
        /// выбранный департамент в списке
        /// </summary>
        public static string SelectedDep { get; set; }
        /// <summary>
        /// CollectionViewSource для департаментов
        /// </summary>
        public static System.ComponentModel.ICollectionView Source;
        static bool isLoad = false;

        /// <summary>
        /// Имя выбранного клиента
        /// </summary>
        public string SelectClientName { get; set; }
        /// <summary>
        /// Сумма на счёте выбранного клиента
        /// </summary>
        public uint SelectClientMoney { get; set; }
        /// <summary>
        /// Тип клиента
        /// </summary>
        public string SelectClientType { get; set; }
        /// <summary>
        /// Вклад клиента
        /// </summary>
        public string SelectClientDeposit { get; set; }
        /// <summary>
        /// Ставка по вкладу
        /// </summary>
        public float SelectClientInterestRate { get; set; }
        /// <summary>
        /// Дата открытия
        /// </summary>
        public string SelectClientDataBegin { get; set; }
        /// <summary>
        /// На срок в днях
        /// </summary>
        public uint SelectClientDays { get; set; }


        public MainViewModel()
        {
            if (!isLoad)
            {
                //AddClientsToBank(rnd.Next(10, 30), rnd.Next(10, 30), rnd.Next(10, 30));
                AddClientsToBank(3, 3, 3);
                isLoad = true;
            }
        }

        /// <summary>
        /// Создаёт клиентов при запуске автоматически
        /// </summary>
        /// <param name="personal">Кол-во физ.лиц</param>
        /// <param name="business">Кол-во юр.лиц</param>
        /// <param name="vip">Кол-во VIP клиентов</param>
        void AddClientsToBank(int personal, int business, int vip)
        {
            // 
            for (int i = 0; i < personal; i++)
            {
                CreateClientsCollection<PersonalClient>();
            }

            // 
            for (int i = 0; i < business; i++)
            {
                CreateClientsCollection<BusinessClient>();
            }

            // 
            for (int i = 0; i < vip; i++)
            {
                CreateClientsCollection<VIPClient>();
            }
        }

        /// <summary>
        /// Добавляет клиентов в список при запуске автоматически
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void CreateClientsCollection<T>() where T : Client, new()
        {
            clients.Add(new T());
        }

        /// <summary>
        /// Команда "Выбор департамента в ListBox"
        /// </summary>
        public ICommand SelectedItemChangedCommand
        {
            get
            {
                var a = new DelegateCommand((obj) =>
                {
                    if (Source != null)
                        Source.Filter = new Predicate<object>(MyFilter);
                });
                return a;
            }
        }

        /// <summary>
        /// Команда "Выбор клинта в ListView"
        /// </summary>
        public ICommand LVSelectedItemChangedCommand
        {
            get
            {
                var a = new DelegateCommand((obj) =>
                {
                    Client client = obj as Client;
                    if (client != null)
                    {
                        SelectClientName = client.Name;
                        SelectClientMoney = client.Money;
                        SelectClientType = client.Status;
                        SelectClientDeposit = client.DepositClientStr;
                        if (client.DepositClient != null)
                        {
                            SelectClientInterestRate = client.DepositClient.InterestRate;
                            SelectClientDataBegin = client.DepositClient.DateBegin.ToLongDateString();
                            SelectClientDays = client.DepositClient.Days;
                        }
                        else
                        {
                            SelectClientInterestRate = 0;
                            SelectClientDataBegin = "";
                            SelectClientDays = 0;
                        }
                    }
                });
                return a;
            }
        }

        /// <summary>
        /// Фильтр для показа клинтов по департаментам
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool MyFilter(object obj)
        {
            var c = obj as Client;
            if (SelectedDep != null)
            {
                switch (SelectedDep)
                {
                    case Const.personalName:
                        if (c.BankDepartmentProp == BankDepartment.PersonalDepartment)
                            return true;
                        else
                            return false;
                    case Const.businessName:
                        if (c.BankDepartmentProp == BankDepartment.BusinessDepartment)
                            return true;
                        else
                            return false;
                    case Const.VIPName:
                        if (c.BankDepartmentProp == BankDepartment.VIPDepartment)
                            return true;
                        else
                            return false;
                }
            }
            return true;
        }


        #region Открыть счёт
        /// <summary>
        /// Открыть счёт
        /// </summary>
        public ICommand AddAccount_Click
        {
            get
            {
                var a = new DelegateCommand((obj) =>
                {
                    var displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    var addAccountViewModel = new AddAccountViewModel();
                    displayRootRegistry.ShowModalPresentation(addAccountViewModel);
                    //(Application.Current as App)._messageBus.SendTo<LogPageViewModel>(new TextMessage(LogText));
                });
                return a;
            }
        }
        /// <summary>
        /// Возвращяет Client из диалогового окна AddClient
        /// </summary>
        /// <param name="employee"></param>
        public static void ReturnAddClient(Client client)
        {
            clients.Add(client);
        }
        #endregion


        #region Закрыть счёт
        /// <summary>
        /// Закрыть счёт
        /// </summary>
        public ICommand CloseAccount_Click
        {
            get
            {
                var a = new DelegateCommand((obj) =>
                {
                    if (SelectedClient == null)
                    {
                        MessageBox.Show("Не выбран счёт для закрытия", "Закрыть счёт");
                    }
                    else
                    {
                        if (MessageBox.Show($"Закрыть счёт для   '{SelectedClient.Name}'", "Закрыть счёт", MessageBoxButton.YesNo) == MessageBoxResult.No)
                            return;
                        clients.Remove(SelectedClient);
                    }
                });
                return a;
            }
        }
        #endregion


        #region Перевести на другой счёт
        /// <summary>
        /// Перевести на другой счёт
        /// </summary>
        public ICommand MoveMoney_Click
        {
            get
            {
                var a = new DelegateCommand((obj) =>
                {
                    if (SelectedClient != null)
                    {
                        var displayRootRegistry = (Application.Current as App).displayRootRegistry;

                        var moveMoneyViewModel = new MoveMoneyViewModel();
                        displayRootRegistry.ShowModalPresentation(moveMoneyViewModel);
                    }
                    else
                        MessageBox.Show("Не выбран счёт для перевода", "Перевести на другой счёт");
                });
                return a;
            }
        }
        /// <summary>
        /// Возвращяет Client из диалогового окна MoveMoney
        /// </summary>
        /// <param name="employee"></param>
        public static void ReturnMoveMoney(Dictionary<Client, uint> client)
        {
            uint moveMoney;
            Client moveClient;
            foreach (KeyValuePair<Client, uint> kvp in client)
            {
                moveClient = kvp.Key;
                moveMoney = kvp.Value;
                if (SelectedClient.Money >= moveMoney)
                {
                    SelectedClient.Money -= moveMoney;
                    moveClient.Money += moveMoney;
                    Source.Filter = new Predicate<object>(MyFilter);
                }
                else
                {
                    MessageBox.Show($"На счёту клиента {SelectedClient} недостаточно средств", "Перевести на другой счёт");
                }
            }
            //SelectedClient.Money = 
        }
        #endregion


        #region Открыть вклад без капитализации %
        /// <summary>
        /// Открыть вклад без капитализации %
        /// </summary>
        public ICommand AddDepositNoCapitalize_Click
        {
            get
            {
                var a = new DelegateCommand((obj) =>
                {
                    if (SelectedClient != null)
                    {
                        var displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        var addDepositNoCapitalizeViewModel = new AddDepositNoCapitalizeViewModel();
                        Dictionary<BankDepartment, uint> bd = new Dictionary<BankDepartment, uint>();
                        switch (SelectedClient.BankDepartmentProp)
                        {
                            case BankDepartment.BusinessDepartment:
                                bd.Add(BankDepartment.BusinessDepartment, 0);
                                Messenger.Default.Send(bd);
                                displayRootRegistry.ShowModalPresentation(addDepositNoCapitalizeViewModel);
                                break;
                            case BankDepartment.PersonalDepartment:
                                bd.Add(BankDepartment.PersonalDepartment, 0);
                                Messenger.Default.Send(bd);
                                displayRootRegistry.ShowModalPresentation(addDepositNoCapitalizeViewModel);
                                break;
                            case BankDepartment.VIPDepartment:
                                bd.Add(BankDepartment.VIPDepartment, 0);
                                Messenger.Default.Send(bd);
                                displayRootRegistry.ShowModalPresentation(addDepositNoCapitalizeViewModel);
                                break;
                        }
                        if (SelectedClient.DepositClient != null)
                        {
                            SelectClientDeposit = SelectedClient.DepositClientStr;
                            SelectClientInterestRate = SelectedClient.DepositClient.InterestRate;
                            SelectClientDataBegin = SelectedClient.DepositClient.DateBegin.ToLongDateString();
                            SelectClientDays = SelectedClient.DepositClient.Days;
                        }
                    }
                    else
                        MessageBox.Show("Не выбран клиент", "Открыть вклад без капитализации %");
                });
                return a;
            }
        }

        /// <summary>
        /// Возвращяет Deposit из окна AddDepositNoCapitalizeWindow
        /// </summary>
        /// <param name="deposit"></param>
        public static void ReturnAddDepositNoCapitalize(Deposit deposit)
        {
            SelectedClient.DepositClient = deposit;
            SelectedClient.DepositClientStr = "вклад без капитализации %";
        }
        #endregion


        #region Открыть вклад с капитализацией %
        /// <summary>
        /// Открыть вклад с капитализацией %
        /// </summary>
        public ICommand AddDepositCapitalize_Click
        {
            get
            {
                var a = new DelegateCommand((obj) =>
                {
                    if (SelectedClient != null)
                    {
                        var displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        var addDepositCapitalizeViewModel = new AddDepositCapitalizeViewModel();
                        switch (SelectedClient.BankDepartmentProp)
                        {
                            case BankDepartment.BusinessDepartment:
                                Messenger.Default.Send(BankDepartment.BusinessDepartment);
                                displayRootRegistry.ShowModalPresentation(addDepositCapitalizeViewModel);
                                break;
                            case BankDepartment.PersonalDepartment:
                                Messenger.Default.Send(BankDepartment.PersonalDepartment);
                                displayRootRegistry.ShowModalPresentation(addDepositCapitalizeViewModel);
                                break;
                            case BankDepartment.VIPDepartment:
                                Messenger.Default.Send(BankDepartment.VIPDepartment);
                                displayRootRegistry.ShowModalPresentation(addDepositCapitalizeViewModel);
                                break;
                        }
                        if (SelectedClient.DepositClient != null)
                        {
                            SelectClientDeposit = SelectedClient.DepositClientStr;
                            SelectClientInterestRate = SelectedClient.DepositClient.InterestRate;
                            SelectClientDataBegin = SelectedClient.DepositClient.DateBegin.ToLongDateString();
                            SelectClientDays = SelectedClient.DepositClient.Days;
                        }
                    }
                    else
                        MessageBox.Show("Не выбран клиент", "Открыть вклад с капитализацией %");
                });
                return a;
            }
        }

        /// <summary>
        /// Возвращяет Deposit из окна AddDepositNoCapitalizeWindow
        /// </summary>
        /// <param name="deposit"></param>
        public static void ReturnAddDepositCapitalize(Deposit deposit)
        {
            SelectedClient.DepositClient = deposit;
            SelectedClient.DepositClientStr = "вклад с капитализацией %";
        }
        #endregion


        #region Показать окно с расчётом %
        /// <summary>
        /// Показать окно с расчётом %
        /// </summary>
        public ICommand RateView_Click
        {
            get
            {
                var a = new DelegateCommand((obj) =>
                {

                    if (SelectedClient != null && SelectedClient.DepositClient != null)
                    {
                        var displayRootRegistry = (Application.Current as App).displayRootRegistry;

                        var rateViewModel = new RateViewModel();
                        Dictionary<Client, short> client = new Dictionary<Client, short>();
                        client.Add(SelectedClient, 0);
                        Messenger.Default.Send(client);
                        displayRootRegistry.ShowModalPresentation(rateViewModel);
                    }
                    else
                        MessageBox.Show("Не выбран клиент", "Расчёт %");
                });
                return a;
            }
        }
        #endregion
    }
}
