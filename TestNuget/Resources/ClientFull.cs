using System.ComponentModel.DataAnnotations;

namespace TestNuget.Resources
{
    // [Request("")]
    // [Request("Create", nameof(Id), nameof(RegistrationTime), nameof(Orders))]
    // [Request("Update", nameof(Id), nameof(RegistrationTime))]
    // public class Client
    // {
    //     [Required]
    //     public Guid Id { get; set; }
    //
    //     [Required]
    //     [Range(0,99999)]
    //     public decimal Price { get; set; }
    //     public List<Guid> Orders { get; set; }
    //     public string Name { get; set; }
    //     public string Description { get; set; }
    //     public DateTimeOffset RegistrationTime { get; set; }
    // }

    [Request("")]
    [Request("Create", nameof(Id), nameof(RegistrationTime), nameof(Orders))]
    [Request("Update", nameof(Id), nameof(RegistrationTime))]
    public record ClientFullRecord
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

    // [Request("")]
    // [Request("Create", nameof(Id), nameof(Age), nameof(FullName))]
    // [RequestAttribute("Update", nameof(Id), nameof(FullName))]
    // [RequestAttribute("Test", nameof(Id), nameof(FullName), nameof(DayOfBirth))]
    // public class ClientClass
    // {
    //     public string Name { get; set; }
    //     public string Sername { get; set; }
    //     public string FullName { get; set; }
    //     public Guid Id { get; set; }
    //     public int Age { get; set; }
    //     public DateTime DayOfBirth { get; set; }
    // }
}
