using System;

namespace DataMapper
{
    class Rentals
    {
        public int Clientld { get; private set; }
        public Movie Movie { get; private set; }
        public int CopyId { get; private set; }
        public DateTime DateOfRental { get; private set; }
        public DateTime? DateOfReturn { get; private set; }
        public Rentals(int clientid, Movie movie, int copyId, DateTime dateOfRental, DateTime? dateOfReturn)
        {
            Clientld = clientid;
            CopyId = copyId;
            Movie = movie;
            DateOfRental = dateOfRental;
            DateOfReturn = dateOfReturn;
        }
    }
}

