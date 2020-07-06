using System;

namespace DataMapper
{
    class Clients
    {
        public int ID { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public DateTime Birthday { get; private set; }
        public Clients(int id, string firstname, string lastname, DateTime birthday) 
        {
            ID = id;
            Firstname = firstname;
            Lastname = lastname;
            Birthday = birthday;
        }
    }
}
