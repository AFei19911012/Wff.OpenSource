namespace Wff.OpenSource
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2025 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2025/3/14 16:55:56
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2025/3/14 16:55:56                     BigWang         首次编写         
    ///
    public class UserAccount
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public DateTime BoD { get; set; }
        public int Age { get; set; }
        public bool VIP { get; set; }
        public decimal Points { get; set; }
    }
}