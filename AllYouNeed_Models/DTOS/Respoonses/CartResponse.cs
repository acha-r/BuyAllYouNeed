namespace AllYouNeed_Models.DTOS.Respoonses
{
    public class CartResponse
    {
        public ICollection<string> Products { get; set; } = new List<string>();
        public decimal? Total { get; set; }
        public string Shopper { get; set; }
        public string Response { get; set; } = string.Empty;
    }
}
