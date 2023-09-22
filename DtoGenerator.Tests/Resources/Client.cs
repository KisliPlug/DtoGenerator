using System.ComponentModel.DataAnnotations;

namespace DtoGenerator.Tests.Resources
{
    [Dto("")]
    [Dto("Create", nameof(Id), nameof(RegistrationTime), nameof(Orders))]
    [Dto("Update", nameof(Id), nameof(RegistrationTime))]
    [Contract("")]
    [Contract("Create", nameof(Id), nameof(RegistrationTime), nameof(Orders))]
    [Contract("Update", nameof(Id), nameof(RegistrationTime))]
    public record ClientRecord([Required] Guid Id, List<Guid> Orders, string Name, string Description
  , [Required] [Range(0, 99999)] decimal Price, DateTimeOffset RegistrationTime);
}
