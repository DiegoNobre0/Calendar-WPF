using Calender.Model;


using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Data.SQLite;
using System.Runtime.ConstrainedExecution;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualBasic;
using System.Data.Common;
using System.Windows.Media;
using System.Linq;
using System.Windows.Controls;
using System.Threading;

using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Calender.Repository;
using Calender.UserControls;


namespace Calender
{
    public partial class MainWindow : Window 
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            Reminders = _calendarRepository.GetReminder();

            ScheduleAlarm();

            FilterRemindersByDateToday();
        }

        ObservableCollection<Reminder> Reminders { get; set; }
        private CalendarRepository _calendarRepository = new CalendarRepository();
        private Timer alarmTimer;
        private bool isEditing = false;
        public event EventHandler<EditEventArgs> EditClicked;
        public event EventHandler<DeleteEventArgs> DeleteClicked;


        public void Calendar_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (myCalendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = myCalendar.SelectedDate.Value;
                selectedDayTextBlock.Text = selectedDate.Day.ToString();

                string monthName = selectedDate.ToString("MMMM");
                string dayOfWeek = selectedDate.ToString("dddd");

                monthTextBlock.Text = monthName;
                dayOfWeekTextBlock.Text = dayOfWeek;

                List<Reminder> filteredReminders = FilterRemindersByDate(selectedDate);

                reminderItemsControl.Items.Clear();
                foreach (var reminderData in filteredReminders)
                {
                    var userControl = new UserControls.Item();
                    userControl.Id = reminderData.Id;
                    userControl.Message = reminderData.Message;
                    userControl.Time = reminderData.Time;
                    userControl.Color = new SolidColorBrush(Colors.White);
                    userControl.Icon = FontAwesome.WPF.FontAwesomeIcon.CheckCircle;
                    userControl.IconBell = FontAwesome.WPF.FontAwesomeIcon.Bell;

                    reminderItemsControl.Items.Add(userControl);
                }

            }
        }

        public List<Reminder> FilterRemindersByDate(DateTime selectedDate)
        {
            return Reminders.Where(reminder => reminder.Date.Date == selectedDate.Date).ToList();
        }


        //Atualiza a Lista com a data do dia atual.
        public void FilterRemindersByDateToday()
        {
           
            DateTime selectedDate = DateTime.Today;
            selectedDayTextBlock.Text = selectedDate.Day.ToString();

            string monthName = selectedDate.ToString("MMMM");
            string dayOfWeek = selectedDate.ToString("dddd");

            monthTextBlock.Text = monthName;
            dayOfWeekTextBlock.Text = dayOfWeek;

            // Filtrar a lista de lembretes com base na data selecionada
            List<Reminder> filteredReminders = Reminders.Where(r => r.Date.Date == selectedDate.Date).ToList();

            // Limpar a lista de itens no ItemsControl
            reminderItemsControl.Items.Clear();

            // Adicionar os itens filtrados à lista
            foreach (var reminderData in filteredReminders)
            {
                var userControl = new UserControls.Item();
                userControl.Id = reminderData.Id;
                userControl.Message = reminderData.Message;
                userControl.Time = reminderData.Time;
                userControl.Color = new SolidColorBrush(Colors.White);
                userControl.Icon = FontAwesome.WPF.FontAwesomeIcon.CheckCircle;
                userControl.IconBell = FontAwesome.WPF.FontAwesomeIcon.Bell;

                reminderItemsControl.Items.Add(userControl);
            }
        }


        //Atualiza a Lista com a data do dia selecionado.
        public void FilterRemindersByDateSelect(DateTime selectedDate)
        {

            DateTime date = selectedDate;
            selectedDayTextBlock.Text = selectedDate.Day.ToString();;

            // Filtrar a lista de lembretes com base na data selecionada
            List<Reminder> filteredReminders = Reminders.Where(r => r.Date.Date == selectedDate.Date).ToList();

            // Limpar a lista de itens no ItemsControl
            reminderItemsControl.Items.Clear();

            // Adicionar os itens filtrados à lista
            foreach (var reminderData in filteredReminders)
            {
                var userControl = new UserControls.Item();
                userControl.Id = reminderData.Id;
                userControl.Message = reminderData.Message;
                userControl.Time = reminderData.Time;
                userControl.Color = new SolidColorBrush(Colors.White);
                userControl.Icon = FontAwesome.WPF.FontAwesomeIcon.CheckCircle;
                userControl.IconBell = FontAwesome.WPF.FontAwesomeIcon.Bell;

                reminderItemsControl.Items.Add(userControl);
            }
        }

        private void myCalendar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AddReminderButton_Click(object sender, RoutedEventArgs e)
        {
            if (myCalendar.SelectedDate.HasValue)
            {
                if (txtMessage.Text == "" || txtMessage.Text == "")
                {
                    MessageBox.Show($"Falta dados no cadastro do lembrete", "Lembrete", MessageBoxButton.OK, MessageBoxImage.Information);

                    return;
                }

                DateTime selectedDate = myCalendar.SelectedDate.Value;
                DateTime dateOnly = selectedDate.Date;
                string Message = txtMessage.Text;
                string time = txtTime.Text;

                Reminder reminder = new Reminder
                {
                    Message = Message,
                    Time = time,
                    Date = dateOnly
                };

                _calendarRepository.AddReminder(reminder);

                txtMessage.Text = "";
                txtTime.Text = "";

                ScheduleAlarm();

                Reminders.Add(reminder);

                FilterRemindersByDateSelect(myCalendar.SelectedDate.Value);
                
            }
        }

        

        //private void AddReminder(Reminder reminder)
        //{
        //    try
        //    {
        //        string connectionString = "Data Source=c:\\dados\\RemindersDataBase.sqlite; Version=3;";
        //        string commandQuery = "Insert into Reminders (Message, Time , Date) values (@Message, @Time, @Date)";

        //        SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);

        //        m_dbConnection.Open();

        //        SQLiteCommand command = new SQLiteCommand(commandQuery, m_dbConnection);

        //        command.CommandType = CommandType.Text;
        //        command.Parameters.AddWithValue("@Message", reminder.Message);
        //        command.Parameters.AddWithValue("@Time", reminder.Time);
        //        command.Parameters.AddWithValue("@Date", reminder.Date);

        //        command.ExecuteNonQuery();

        //        m_dbConnection.Close();

        //        ScheduleAlarm();

        //        Reminders.Add(reminder);

        //        FilterRemindersBySelectedDate();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private ObservableCollection<Reminder> GetReminder()
        //{
        //    ObservableCollection<Reminder> reminders;
        //    Reminder reminder;           

        //    SQLiteCommand command = new SQLiteCommand();

        //    string connectionString = "Data Source=c:\\dados\\RemindersDataBase.sqlite; Version=3;";
        //    string commandQuery = "SELECT  Id, Message, Time , Date from Reminders WHERE 1=1";

        //    command.CommandText = commandQuery;
        //    command.CommandType = CommandType.Text;

        //    SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);

        //    command = new SQLiteCommand(commandQuery, m_dbConnection);

        //    m_dbConnection.Open();

        //    SQLiteDataReader _SqliteDataReader = command.ExecuteReader();

        //    reminders = new ObservableCollection<Reminder>();

        //    while (_SqliteDataReader.Read())
        //    {
        //        reminder = new Reminder();
        //        reminder.Id = Convert.ToInt32(_SqliteDataReader["Id"]);
        //        reminder.Message = _SqliteDataReader["Message"].ToString();
        //        reminder.Time = _SqliteDataReader["Time"].ToString();
        //        reminder.Date = Convert.ToDateTime(_SqliteDataReader["Date"]);

        //        reminders.Add(reminder);
        //    }

        //    m_dbConnection.Close();
        //    m_dbConnection.Dispose();

        //    return reminders;
        //}

        //public static void CreateBankSQlite()
        //{
        //    try
        //    {
        //        SQLiteConnection.CreateFile(@"c:\dados\RemindersDataBase.sqlite");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        //public static void CreateTableSQlite()
        //{
        //    try
        //    {
        //        string connectionString = "Data Source=c:\\dados\\RemindersDataBase.sqlite; Version=3;";

        //        SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);

        //        m_dbConnection.Open();

        //        string commandQuery = "Create Table Reminders (Id INTEGER PRIMARY KEY AUTOINCREMENT, Message VARCHAR(20), Time VARCHAR(20), Date DATETIME)";

        //        SQLiteCommand command = new SQLiteCommand(commandQuery, m_dbConnection);

        //        command.ExecuteNonQuery();

        //        m_dbConnection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public void ScheduleAlarm()
        {
            ObservableCollection<Reminder> reminders = _calendarRepository.GetReminder();

            foreach (var reminder in reminders)
            {
                var timeReminder = reminder.Time;
                DateTime datetime;
                datetime = DateTime.ParseExact(timeReminder, "HH:mm", null);
                TimeSpan timeUntilAlarm;

                if (datetime > DateTime.Now)
                {
                    timeUntilAlarm = datetime - DateTime.Now;
                    alarmTimer = new Timer(state => HandleAlarm(reminder.Message), null, timeUntilAlarm, Timeout.InfiniteTimeSpan);
                }
            }
        }

        private void HandleAlarm(string message)
        {
            // Lógica para lidar com o alarme (por exemplo, exibir uma mensagem)
            MessageBox.Show($"Lembrete {message} disparado em: {DateTime.Now}", "Alarme", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void lblNote_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtMessage.Focus();
        }

        private void lblTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtTime.Focus();
        }

        private void txtNote_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMessage.Text) && txtMessage.Text.Length > 0)
                lblNote.Visibility = Visibility.Collapsed;
            else
                lblNote.Visibility = Visibility.Visible;
        }

        private void txtTime_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTime.Text) && txtTime.Text.Length > 0)
                lblTime.Visibility = Visibility.Collapsed;
            else
                lblTime.Visibility = Visibility.Visible;
        }

        private void Item_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

public class EditEventArgs
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string Time { get; set; }
}

public class DeleteEventArgs
{
    public int Id { get; set; }
}

