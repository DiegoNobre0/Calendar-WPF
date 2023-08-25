using System.Data.SQLite;
using System.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Calender.Model;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Globalization;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Calender.Repository;
using System.Collections.Generic;


namespace Calender.UserControls
{
    public partial class Item : UserControl
    {
        public Item()
        {
            InitializeComponent();
        }

        private CalendarRepository _calendarRepository = new CalendarRepository();
        public event EventHandler<EditEventArgs> EditClicked;
        public event EventHandler<DeleteEventArgs> DeleteClicked;
        private bool isEditing = false;
        

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(Item));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value);}
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(Item));


        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value);}
        }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(string), typeof(Item));


        public SolidColorBrush Color
        {
            get { return (SolidColorBrush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value);}
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(SolidColorBrush), typeof(Item));


        public FontAwesome.WPF.FontAwesomeIcon Icon
        {
            get { return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value);}
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(Item));


        public FontAwesome.WPF.FontAwesomeIcon IconBell
        {
            get { return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconBellProperty); }
            set { SetValue(IconBellProperty, value); }
        }

        public static readonly DependencyProperty IconBellProperty = DependencyProperty.Register("IconBell", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(Item));

     


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (isEditing)
            {
                // Salvar as alterações e alternar de volta para o modo de exibição
                Message = editMessageTextBox.Text;
                Time = editTimeTextBox.Text;
                textBlockMessage.Visibility = Visibility.Visible;
                editMessageTextBox.Visibility = Visibility.Collapsed;
                textBlockTime.Visibility = Visibility.Visible;
                editTimeTextBox.Visibility = Visibility.Collapsed;

                Reminder remider = new Reminder();
                remider.Id = Id;
                remider.Message = Message;
                remider.Time = Time;
                //remider.Date = date;

                //_calendarRepository.UpdateReminder(remider);

                //_mainWindow.FilterRemindersBySelectedDate();

                editButton.Content = "Editar";
            }
            else
            {
                // Alternar para o modo de edição
                textBlockMessage.Visibility = Visibility.Collapsed;
                editMessageTextBox.Visibility = Visibility.Visible;
                editMessageTextBox.Text = textBlockMessage.Text;
                editMessageTextBox.Focus();
                textBlockTime.Visibility = Visibility.Collapsed;
                editTimeTextBox.Visibility = Visibility.Visible;
                editTimeTextBox.Text = textBlockTime.Text;
                editTimeTextBox.Focus();
                editButton.Content = "Salvar";
            }

            isEditing = !isEditing; 
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Reminder remider = new Reminder();
            remider.Id = Id;

            //_calendarRepository.DeleteReminder(remider.Id);
        }

        //public void FilterRemindersByDateSelect(DateTime selectedDate)
        //{

        //    DateTime date = selectedDate;
        //    selectedDayTextBlock.Text = selectedDate.Day.ToString(); ;

        //    // Filtrar a lista de lembretes com base na data selecionada
        //    List<Reminder> filteredReminders = Reminders.Where(r => r.Date.Date == selectedDate.Date).ToList();

        //    // Limpar a lista de itens no ItemsControl
        //    reminderItemsControl.Items.Clear();

        //    // Adicionar os itens filtrados à lista
        //    foreach (var reminderData in filteredReminders)
        //    {
        //        var userControl = new UserControls.Item();
        //        userControl.Id = reminderData.Id;
        //        userControl.Message = reminderData.Message;
        //        userControl.Time = reminderData.Time;
        //        userControl.Color = new SolidColorBrush(Colors.White);
        //        userControl.Icon = FontAwesome.WPF.FontAwesomeIcon.CheckCircle;
        //        userControl.IconBell = FontAwesome.WPF.FontAwesomeIcon.Bell;

        //        reminderItemsControl.Items.Add(userControl);
        //    }
        //}

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
}