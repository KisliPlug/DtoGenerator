using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CodeGen.Tests.Resources
{
    public class ClientDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [Range(0,99999)]
        public decimal Price { get; set; }
        public List<Guid> Orders { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset RegistrationTime { get; set; }
        public static explicit operator ClientDto(Client b)
        {
            return new ClientDto
            {
                Id=b.Id,
                Price=b.Price,
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                RegistrationTime=b.RegistrationTime
            };
        }
        public static explicit operator Client(ClientDto b)
        {
            return new Client
            {
                Id=b.Id,
                Price=b.Price,
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                RegistrationTime=b.RegistrationTime
            };
        }
        public   void  SetProps(Client b)
        {
            b.Id=Id;
            b.Price=Price;
            b.Orders=Orders;
            b.Name=Name;
            b.Description=Description;
            b.RegistrationTime=RegistrationTime;
        }
        public   void  GetProps(Client b)
        {
            Id=b.Id;
            Price=b.Price;
            Orders=b.Orders;
            Name=b.Name;
            Description=b.Description;
            RegistrationTime=b.RegistrationTime;
        }
    }
    public static class ClientDtoExtensions
    {
        public static ClientDto AsClientDto(this Client entity)
        {
            return (ClientDto)entity;
        }
        public static Client AsClient(this ClientDto dto)
        {
            return (Client)dto;
        }
        public static IEnumerable<ClientDto> AsClientDto(this IEnumerable<Client> entity)
        {
            return  entity.Select(x=>x.AsClientDto());
        }
        public static IEnumerable<Client> AsClient(this IEnumerable<ClientDto> dtos)
        {
            return dtos.Select(x=>x.AsClient());
        }
    }
}

