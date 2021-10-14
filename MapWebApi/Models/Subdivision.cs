
namespace MapWebApi.Models
{
    public class Subdivision
    {
        public int Id { get; set; }
        public int? Strength { get; set; }
        public int TypeOfSubdivisionId { get; set; }
        public TypeOfSubdivision TypeOfSubdivision{ get; set; }
        public string Name { get; set; }

        public string Composition { get; set; }
        public int CommanderId { get; set; }
        public Commander Commander { get; set; }



    }
}
