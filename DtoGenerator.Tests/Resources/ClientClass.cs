using System.ComponentModel.DataAnnotations;

namespace DtoGenerator.Tests.Resources
{
    [Dto("")]
    [Dto("Create", nameof(Id), nameof(RegistrationTime), nameof(Orders))]
    [Dto("Update", nameof(Id), nameof(RegistrationTime))]
    [Dto("Update", nameof(Id), nameof(RegistrationTime))]
    public class ClientClass
    {
        [Required]
        public Guid Id { get; set; }

        public List<Guid> Orders { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(0, 99999)]
        public decimal Price { get; set; }

        public DateTimeOffset RegistrationTime { get; set; }
    }
}
