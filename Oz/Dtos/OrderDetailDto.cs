namespace Oz.Dtos
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public double TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
