namespace AllYouNeed_Models.DTOS.Respoonses
{
    public class CartResponse
    {
        public Dictionary<string, int>? Products { get; set; }
        public string OrderComplete { get; set; } = string.Empty;
    }
}
