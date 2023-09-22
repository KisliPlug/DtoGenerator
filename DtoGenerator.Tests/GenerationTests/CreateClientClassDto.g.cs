using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace DtoGenerator.Tests.Resources
{
    public class CreateClientClassDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0, 99999)]
        public decimal Price { get; set; }
        public static explicit operator ClientClass(CreateClientClassDto b)
        {
            return new ClientClass
            {
                Name=b.Name,
                Description=b.Description,
                Price=b.Price,
                Id=Guid.NewGuid(),
                Orders=new(),
                RegistrationTime=DateTimeOffset.Now
            };
        }
        public static explicit operator CreateClientClassDto(ClientClass b)
        {
            return new CreateClientClassDto
            {
                Name=b.Name,
                Description=b.Description,
                Price=b.Price
            };
        }
        public   void  SetProps(ClientClass b)
        {
            b.Name=Name;
            b.Description=Description;
            b.Price=Price;
        }
        public   void  GetProps(ClientClass b)
        {
            Name=b.Name;
            Description=b.Description;
            Price=b.Price;
        }
    }
    public static class CreateClientClassDtoExtensions
    {
        public static CreateClientClassDto AsCreateDto(this ClientClass entity)
        {
            return (CreateClientClassDto)entity;
        }
        public static ClientClass AsData(this CreateClientClassDto dto)
        {
            return (ClientClass)dto;
        }
        public static IEnumerable<CreateClientClassDto> AsCreateDto(this IEnumerable<ClientClass> entity)
        {
            return  entity.Select(x=>x.AsCreateDto());
        }
        public static IEnumerable<ClientClass> AsData(this IEnumerable<CreateClientClassDto> dtos)
        {
            return dtos.Select(x=>x.AsData());
        }
    }
}

