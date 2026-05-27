namespace WebThucPhamSach.Models
{
    /// <summary>
    /// Model đại diện cho một sản phẩm trong giỏ hàng.
    /// Không lưu trong database, chỉ dùng trong Session.
    /// </summary>
    public class CartItem
    {
        public int ProductId { get; set; }
        
        public Product Product { get; set; } = null!;
        
        public int Quantity { get; set; }
    }
}
