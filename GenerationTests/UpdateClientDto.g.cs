using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CodeGen.Tests.Resources
{
    public class UpdateClientDto
    {
        [Required]
        [Range(0,99999)]
        public decimal Price { get; set; }
        public List<Guid> Orders { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public static explicit operator UpdateClientDto(Client b)
        {
            return new UpdateClientDto
            {
                Price=b.Price,
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description
            };
        }
        public static explicit operator Client(UpdateClientDto b)
        {
            return new Client
            {
                Price=b.Price,
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Id=Guid.NewGuid(),
                RegistrationTime=DateTimeOffset.Now
            };
        }
        public   void  SetProps(Client b)
        {
            b.Price=Price;
            b.Orders=Orders;
            b.Name=Name;
            b.Description=Description;
        }
        public   void  GetProps(Client b)
        {
            Price=b.Price;
            Orders=b.Orders;
            Name=b.Name;
            Description=b.Description;
        }
    }
    public static class UpdateClientDtoExtensions
    {
        public static UpdateClientDto AsUpdateClientDto(this Client entity)
        {
            return (UpdateClientDto)entity;
        }
        public static Client AsClient(this UpdateClientDto dto)
        {
            return (Client)dto;
        }
        public static IEnumerable<UpdateClientDto> AsUpdateClientDto(this IEnumerable<Client> entity)
        {
            return  entity.Select(x=>x.AsUpdateClientDto());
        }
        public static IEnumerable<Client> AsClient(this IEnumerable<UpdateClientDto> dtos)
        {
            return dtos.Select(x=>x.AsClient());
        }
    }
}

