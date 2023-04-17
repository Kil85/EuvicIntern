namespace EuvicIntern.Models
{
    public class ReturnUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? Age { get; set; }
        public Decimal? AveragePowerConsumption { get; set; }
        public string Role { get; set; }
    }
}
