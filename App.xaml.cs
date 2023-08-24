using Calender.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Calender
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private List<Reminder> GetReminder()
        {
            List<Reminder> reminders;
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

            reminders = new List<Reminder>();

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
    }
}
