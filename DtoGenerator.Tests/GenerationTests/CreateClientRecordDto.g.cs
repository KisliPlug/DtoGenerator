using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace DtoGenerator.Tests.Resources
{
    public record CreateClientRecordDto
    {
        public string Name {get;init;}
        public string Description {get;init;}
        [Required] [Range(0, 99999)]public decimal Price {get;init;}
        public static explicit operator ClientRecord(CreateClientRecordDto b)
        {
            return new ClientRecord
            (
            Name:b.Name,
            Description:b.Description,
            Price:b.Price,
            Id:Guid.NewGuid(),
            Orders:new(),
            RegistrationTime:DateTimeOffset.Now
            );
        }
        public static explicit operator CreateClientRecordDto(ClientRecord b)
        {
            return new CreateClientRecordDto
            {
                Name=b.Name,
                Description=b.Description,
                Price=b.Price
            };
        }
        public ClientRecord SetProps(ClientRecord dto)
        {
            return dto with
            {
                Name=this.Name,
                Description=this.Description,
                Price=this.Price
            };
        }
        public CreateClientRecordDto GetProps(ClientRecord dto)
        {
            return this with
            {
                Name=dto.Name,
                Description=dto.Description,
                Price=dto.Price
            };
        }
    }
    public static class CreateClientRecordDtoExtensions
    {
        public static CreateClientRecordDto AsCreateDto(this ClientRecord entity)
        {
            return (CreateClientRecordDto)entity;
        }
        public static ClientRecord AsData(this CreateClientRecordDto dto)
        {
            return (ClientRecord)dto;
        }
        public static IEnumerable<CreateClientRecordDto> AsCreateDto(this IEnumerable<ClientRecord> entity)
        {
            return  entity.Select(x=>x.AsCreateDto());
        }
        public static IEnumerable<ClientRecord> AsData(this IEnumerable<CreateClientRecordDto> dtos)
        {
            return dtos.Select(x=>x.AsData());
        }
    }
}

