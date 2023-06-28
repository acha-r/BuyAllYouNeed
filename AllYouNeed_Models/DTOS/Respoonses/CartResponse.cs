namespace AllYouNeed_Models.DTOS.Respoonses
{
    public class CartResponse
    {
        public Dictionary<string, int>? Products { get; set; }
        public string Response { get; set; } = string.Empty;
        public decimal ? Total { get; set; } 
    }
}
