using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace DtoGenerator.Tests.Resources
{
    public class UpdateClientClassDto
    {
        public List<Guid> Orders { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0, 99999)]
        public decimal Price { get; set; }
        public static explicit operator ClientClass(UpdateClientClassDto b)
        {
            return new ClientClass
            {
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Price=b.Price,
                Id=Guid.NewGuid(),
                RegistrationTime=DateTimeOffset.Now
            };
        }
        public static explicit operator UpdateClientClassDto(ClientClass b)
        {
            return new UpdateClientClassDto
            {
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Price=b.Price
            };
        }
        public   void  SetProps(ClientClass b)
        {
            b.Orders=Orders;
            b.Name=Name;
            b.Description=Description;
            b.Price=Price;
        }
        public   void  GetProps(ClientClass b)
        {
            Orders=b.Orders;
            Name=b.Name;
            Description=b.Description;
            Price=b.Price;
        }
    }
    public static class UpdateClientClassDtoExtensions
    {
        public static UpdateClientClassDto AsUpdateDto(this ClientClass entity)
        {
            return (UpdateClientClassDto)entity;
        }
        public static ClientClass AsData(this UpdateClientClassDto dto)
        {
            return (ClientClass)dto;
        }
        public static IEnumerable<UpdateClientClassDto> AsUpdateDto(this IEnumerable<ClientClass> entity)
        {
            return  entity.Select(x=>x.AsUpdateDto());
        }
        public static IEnumerable<ClientClass> AsData(this IEnumerable<UpdateClientClassDto> dtos)
        {
            return dtos.Select(x=>x.AsData());
        }
    }
}

