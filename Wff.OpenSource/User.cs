namespace Wff.OpenSource
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2025 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2025/4/13 23:17:39
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2025/4/13 23:17:39                     BigWang         首次编写         
    ///
    public class User
    {
        public User(int userId, string ssn)
        {
            Id = userId;
            SSN = ssn;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string SomethingUnique { get; set; }
        public Guid SomeGuid { get; set; }

        public string Avatar { get; set; }
        public Guid CartId { get; set; }
        public string SSN { get; set; }
        public Gender Gender { get; set; }

        public List<Order> Orders { get; set; }
    }
}