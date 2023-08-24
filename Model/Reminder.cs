using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Calender.Model
{
    class Reminder 
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public string Time { get; set; }

        public DateTime Date { get; set; }
    }
}
