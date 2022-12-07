using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CodeGen.Tests.Resources
{
    public class CreateClientDto
    {
        [Required]
        [Range(0,99999)]
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public static explicit operator CreateClientDto(Client b)
        {
            return new CreateClientDto
            {
                Price=b.Price,
                Name=b.Name,
                Description=b.Description
            };
        }
        public static explicit operator Client(CreateClientDto b)
        {
            return new Client
            {
                Price=b.Price,
                Name=b.Name,
                Description=b.Description,
                Id=Guid.NewGuid(),
                Orders=new(),
                RegistrationTime=DateTimeOffset.Now
            };
        }
        public   void  SetProps(Client b)
        {
            b.Price=Price;
            b.Name=Name;
            b.Description=Description;
        }
        public   void  GetProps(Client b)
        {
            Price=b.Price;
            Name=b.Name;
            Description=b.Description;
        }
    }
    public static class CreateClientDtoExtensions
    {
        public static CreateClientDto AsCreateClientDto(this Client entity)
        {
            return (CreateClientDto)entity;
        }
        public static Client AsClient(this CreateClientDto dto)
        {
            return (Client)dto;
        }
        public static IEnumerable<CreateClientDto> AsCreateClientDto(this IEnumerable<Client> entity)
        {
            return  entity.Select(x=>x.AsCreateClientDto());
        }
        public static IEnumerable<Client> AsClient(this IEnumerable<CreateClientDto> dtos)
        {
            return dtos.Select(x=>x.AsClient());
        }
    }
}

