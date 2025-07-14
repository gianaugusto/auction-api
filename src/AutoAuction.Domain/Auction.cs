namespace AutoAuction.Domain
{
    public class Auction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal StartingBid { get; set; }
        public decimal CurrentBid { get; set; }
        public bool IsActive { get; set; }
    }
}
