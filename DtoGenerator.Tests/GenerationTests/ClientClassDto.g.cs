using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace DtoGenerator.Tests.Resources
{
    public class ClientClassDto
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
        public static explicit operator ClientClass(ClientClassDto b)
        {
            return new ClientClass
            {
                Id=b.Id,
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Price=b.Price,
                RegistrationTime=b.RegistrationTime
            };
        }
        public static explicit operator ClientClassDto(ClientClass b)
        {
            return new ClientClassDto
            {
                Id=b.Id,
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Price=b.Price,
                RegistrationTime=b.RegistrationTime
            };
        }
        public   void  SetProps(ClientClass b)
        {
            b.Id=Id;
            b.Orders=Orders;
            b.Name=Name;
            b.Description=Description;
            b.Price=Price;
            b.RegistrationTime=RegistrationTime;
        }
        public   void  GetProps(ClientClass b)
        {
            Id=b.Id;
            Orders=b.Orders;
            Name=b.Name;
            Description=b.Description;
            Price=b.Price;
            RegistrationTime=b.RegistrationTime;
        }
    }
    public static class ClientClassDtoExtensions
    {
        public static ClientClassDto AsDto(this ClientClass entity)
        {
            return (ClientClassDto)entity;
        }
        public static ClientClass AsData(this ClientClassDto dto)
        {
            return (ClientClass)dto;
        }
        public static IEnumerable<ClientClassDto> AsDto(this IEnumerable<ClientClass> entity)
        {
            return  entity.Select(x=>x.AsDto());
        }
        public static IEnumerable<ClientClass> AsData(this IEnumerable<ClientClassDto> dtos)
        {
            return dtos.Select(x=>x.AsData());
        }
    }
}

