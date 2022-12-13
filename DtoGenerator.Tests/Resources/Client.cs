using System.ComponentModel.DataAnnotations;

namespace DtoGenerator.Tests.Resources
{
    [Request("")]
    [Request("Create", nameof(Id), nameof(RegistrationTime), nameof(Orders))]
    [Request("Update", nameof(Id), nameof(RegistrationTime))]
    public record ClientRecord([Required] Guid Id
                             , List<Guid> Orders
                             , string Name
                             , string Description
                             , [Required] [Range(0, 99999)] decimal Price
                             , DateTimeOffset
                                   RegistrationTime);
}
