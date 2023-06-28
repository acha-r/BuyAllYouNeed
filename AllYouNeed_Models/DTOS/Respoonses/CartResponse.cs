namespace AllYouNeed_Models.DTOS.Respoonses
{
    public class CartResponse
    {
        public Dictionary<string, int>? Products { get; set; } = new Dictionary<string, int>();
        public decimal? Total { get; set; }
        public string Response { get; set; } = string.Empty;
    }
}
