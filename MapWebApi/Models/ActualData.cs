using System;

namespace MapWebApi.Models
{
    public class ActualData
    {
        public int Id { get; set; }
        public DateTime Date{ get; set; }
        public int SubdivisionId { get; set; }
        public Subdivision Subdivision { get; set; }
        public int DocumentId { get; set; }
        public Document Document { get; set; }
        public int DocumentPage { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
