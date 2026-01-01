namespace Wff.OpenSource
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2025 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2025/4/13 23:18:20
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2025/4/13 23:18:20                     BigWang         首次编写         
    ///
    public class Order
    {
        public int OrderId { get; set; }
        public string Item { get; set; }
        public int Quantity { get; set; }
        public int? LotNumber { get; set; }
    }
}