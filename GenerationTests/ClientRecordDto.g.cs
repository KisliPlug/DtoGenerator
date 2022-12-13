using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace DtoGenerator.Tests.Resources
{
    public record ClientRecordDto
    {
        [Required]public Guid Id {get;init;}
        public List<Guid> Orders {get;init;}
        public string Name {get;init;}
        public string Description {get;init;}
        [Required] [Range(0,99999)]public decimal Price {get;init;}
        public DateTimeOffset RegistrationTime {get;init;}
        public static explicit operator ClientRecordDto(ClientRecord b)
        {
            return new ClientRecordDto
            (
            Id:b.Id,
            Orders:b.Orders,
            Name:b.Name,
            Description:b.Description,
            Price:b.Price,
            RegistrationTime:b.RegistrationTime
            );
        }
        public static explicit operator ClientRecord(ClientRecordDto b)
        {
            return new ClientRecord
            (
            Id:b.Id,
            Orders:b.Orders,
            Name:b.Name,
            Description:b.Description,
            Price:b.Price,
            RegistrationTime:b.RegistrationTime
            );
        }
        public ClientRecord SetProps(ClientRecord dto)
        {
            return dto with
            {
                Id=this.Id,
                Orders=this.Orders,
                Name=this.Name,
                Description=this.Description,
                Price=this.Price,
                RegistrationTime=this.RegistrationTime
            };
        }
        public ClientRecordDto GetProps(ClientRecord dto)
        {
            return this with
            {
                Id=dto.Id,
                Orders=dto.Orders,
                Name=dto.Name,
                Description=dto.Description,
                Price=dto.Price,
                RegistrationTime=dto.RegistrationTime
            };
        }
    }
    public static class ClientRecordDtoExtensions
    {
        public static ClientRecordDto AsClientRecordDto(this ClientRecord entity)
        {
            return (ClientRecordDto)entity;
        }
        public static ClientRecord AsClientRecord(this ClientRecordDto dto)
        {
            return (ClientRecord)dto;
        }
        public static IEnumerable<ClientRecordDto> AsClientRecordDto(this IEnumerable<ClientRecord> entity)
        {
            return  entity.Select(x=>x.AsClientRecordDto());
        }
        public static IEnumerable<ClientRecord> AsClientRecord(this IEnumerable<ClientRecordDto> dtos)
        {
            return dtos.Select(x=>x.AsClientRecord());
        }
    }
}

