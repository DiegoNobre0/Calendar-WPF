using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Text;
using Calender.Model;
using System.Collections.ObjectModel;
using Calender.UserControls;

namespace Calender.Repository
{
    internal class CalendarRepository
    {

        public ObservableCollection<Reminder> GetReminder()
        {
            ObservableCollection<Reminder> reminders;
            Reminder reminder;

            SQLiteCommand command = new SQLiteCommand();

            string connectionString = "Data Source=c:\\dados\\RemindersDataBase.sqlite; Version=3;";
            string commandQuery = "SELECT  Id, Message, Time , Date from Reminders WHERE 1=1";

            command.CommandText = commandQuery;
            command.CommandType = CommandType.Text;

            SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);

            command = new SQLiteCommand(commandQuery, m_dbConnection);

            m_dbConnection.Open();

            SQLiteDataReader _SqliteDataReader = command.ExecuteReader();

            reminders = new ObservableCollection<Reminder>();

            while (_SqliteDataReader.Read())
            {
                reminder = new Reminder();
                reminder.Id = Convert.ToInt32(_SqliteDataReader["Id"]);
                reminder.Message = _SqliteDataReader["Message"].ToString();
                reminder.Time = _SqliteDataReader["Time"].ToString();
                reminder.Date = Convert.ToDateTime(_SqliteDataReader["Date"]);

                reminders.Add(reminder);
            }

            m_dbConnection.Close();
            m_dbConnection.Dispose();

            return reminders;
        }


        public void AddReminder(Reminder reminder)
        {
            try
            {
                string connectionString = "Data Source=c:\\dados\\RemindersDataBase.sqlite; Version=3;";
                string commandQuery = "Insert into Reminders (Message, Time , Date) values (@Message, @Time, @Date)";

                SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);

                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand(commandQuery, m_dbConnection);

                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Message", reminder.Message);
                command.Parameters.AddWithValue("@Time", reminder.Time);
                command.Parameters.AddWithValue("@Date", reminder.Date);

                command.ExecuteNonQuery();

                m_dbConnection.Close();
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateReminder(EditEventArgs remider)
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

        public bool DeleteReminder(int? id)
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

        public static void CreateBankSQlite()
        {
            try
            {
                SQLiteConnection.CreateFile(@"c:\dados\RemindersDataBase.sqlite");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateTableSQlite()
        {
            try
            {
                string connectionString = "Data Source=c:\\dados\\RemindersDataBase.sqlite; Version=3;";

                SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);

                m_dbConnection.Open();

                string commandQuery = "Create Table Reminders (Id INTEGER PRIMARY KEY AUTOINCREMENT, Message VARCHAR(20), Time VARCHAR(20), Date DATETIME)";

                SQLiteCommand command = new SQLiteCommand(commandQuery, m_dbConnection);

                command.ExecuteNonQuery();

                m_dbConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
    }
}
