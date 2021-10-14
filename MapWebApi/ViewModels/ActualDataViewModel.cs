using System;
namespace MapWebApi.ViewModels
{
    public class ActualDataViewModel
    {
        public int SubdivisionId { get; set; }

        public int CommanderId { get; set; }
        public int LocationId { get; set; }

        public int DocumentId { get; set; }
        public DateTime Date { get; set; }
        public int DocumentPage { get; set; }
        public int Strength { get; set; }
    }
}
