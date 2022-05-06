namespace ApiRestMaps.Data.VO
{
    public class MapsResult
    {
        public List<AddressObj> AddressArray { get; set; }
        public double ?Distance { get; set; }
        public bool ?Closeset { get; set; }
        public bool ?Further { get; set; }
    }
}
