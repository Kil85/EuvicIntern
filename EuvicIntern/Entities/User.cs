using System.Reflection.Metadata.Ecma335;

namespace EuvicIntern.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string HashedPassword { get; set; }

        public int? Age { get; set; }
        public Decimal? AveragePowerConsumption { get; set; }

    }
}
