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



