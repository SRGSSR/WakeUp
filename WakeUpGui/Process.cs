using System;

namespace WakeUpGui
{
    class LocalProcess
    {
        private string name;
        private string fullName;
        private string reason;
        private DateTime date;

        //constructor
        public LocalProcess(string name, string fullname, string reason, DateTime date)
        {
            this.name = name;
            this.fullName = fullname;
            this.reason = reason;
            this.date = date;
        }

        //returns name
        public string getName()
        {
            return name;
        }

        //sets name
        public void setName(string name)
        {
            this.name = name;
        }

        //returns fullname
        public string getFullName()
        {
            return fullName;
        }

        //sets fullname
        public void setFullName(string fullName)
        {
            this.fullName = fullName;
        }

        //get reason
        public string getReason()
        {
            return reason;
        }

        //set reason
        public void setReason(string reason)
        {
            this.reason = reason;
        }

        //get date
        public DateTime getDate()
        {
            return date;
        }

        //set date
        public void setDate(DateTime date)
        {
            this.date = date;
        }
    }
}
