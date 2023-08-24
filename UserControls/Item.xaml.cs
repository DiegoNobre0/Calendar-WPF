using System.Data.SQLite;
using System.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Calender.Model;
using Microsoft.Graph.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Globalization;
using System.Windows.Media.Imaging;


namespace Calender.UserControls
{
    public partial class Item : UserControl
    {
        public Item()
        {
            InitializeComponent();
        }

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(Item));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(Item));


        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(string), typeof(Item));


        public SolidColorBrush Color
        {
            get { return (SolidColorBrush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(SolidColorBrush), typeof(Item));


        public FontAwesome.WPF.FontAwesomeIcon Icon
        {
            get { return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(Item));


        public FontAwesome.WPF.FontAwesomeIcon IconBell
        {
            get { return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconBellProperty); }
            set { SetValue(IconBellProperty, value); }
        }

        public static readonly DependencyProperty IconBellProperty = DependencyProperty.Register("IconBell", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(Item));


        public event EventHandler<EditEventArgs> EditClicked;
        public event EventHandler<DeleteEventArgs> DeleteClicked;
        private bool isEditing = false;


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

                EditEventArgs remider = new EditEventArgs();
                remider.Id = Id;
                remider.Message = Message;
                remider.Time = Time;
                //remider.Date = Calendar.SelectedDate.Value;

                UpdateReminder(remider);
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
            DeleteEventArgs _id = new DeleteEventArgs();
            _id.Id = Id;

            DeleteReminder(_id.Id);
        }

        private void UpdateReminder(EditEventArgs remider)
        {
            SQLiteCommand command;
            SQLiteConnection m_dbConnection;
            string connectionString = "Data Source=c:\\dados\\RemindersDataBase.sqlite; Version=3;";
            string commandQuery = "Update Reminders set Message = @Message, Time = @Time where Id = @Id";

            try
            {
                command = new SQLiteCommand();

                command.CommandText = commandQuery;
                command.CommandType = CommandType.Text;

                m_dbConnection = new SQLiteConnection(connectionString);
                m_dbConnection.Open();

                command = new SQLiteCommand(commandQuery, m_dbConnection);
                command.Parameters.AddWithValue("@Id", remider.Id);
                command.Parameters.AddWithValue("@Message", remider.Message);
                command.Parameters.AddWithValue("@Time", remider.Time);             

                var Update = command.ExecuteScalar();

                m_dbConnection.Close();
                m_dbConnection.Dispose();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception);
            }
        }

        private bool DeleteReminder(int? id)
        {
            SQLiteCommand command;
            SQLiteConnection m_dbConnection;
            string connectionString = "Data Source=c:\\dados\\RemindersDataBase.sqlite; Version=3;";
            string commandQuery = "Delete from Reminders where Id = @Id";
            int rowsCommand;
            bool isDeleted = true;
          
            try
            {
                command = new SQLiteCommand();

                command.CommandText = commandQuery;
                command.CommandType = CommandType.Text;

                m_dbConnection = new SQLiteConnection(connectionString);

                m_dbConnection.Open();

                command = new SQLiteCommand(commandQuery, m_dbConnection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Id", id);

                rowsCommand = command.ExecuteNonQuery();

                m_dbConnection.Close();
                m_dbConnection.Dispose();
 

                if (rowsCommand == 0)
                {
                    throw new Exception("Erro - nenhum registro foi deletado para o id informado: " + id);
                }
                else
                {
                    isDeleted = true;                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            

            return isDeleted;
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
}