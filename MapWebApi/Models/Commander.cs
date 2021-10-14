namespace MapWebApi.Models
{
    public class Commander
    {
        public int Id { get; set; }
        public int RankId { get; set; }
        public Rank Rank { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }

    }
}
