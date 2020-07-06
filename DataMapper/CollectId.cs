using System;

namespace DataMapper
{
    class CollectId
    {
        public int? RentalId { get; set; }
        public int? ClientId { get; set; }
        public int? MovieId { get; set; }
        public int CopyId { get; set; }
        public DateTime? DateHired { get; set; }
        public DateTime? DateReturn { get; set; }
    }
}
