using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CodeGen.Tests.Resources
{
    public record CreateClientRecordDto
    {
        public string Name {get;init;}
        public string Description {get;init;}
        [Required] [Range(0,99999)]public decimal Price {get;init;}
        public static explicit operator CreateClientRecordDto(ClientRecord b)
        {
            return new CreateClientRecordDto
            {
                Name=b.Name,
                Description=b.Description,
                Price=b.Price
            };
        }
        public static explicit operator ClientRecord(CreateClientRecordDto b)
        {
            return new ClientRecord
            {
                Name=b.Name,
                Description=b.Description,
                Price=b.Price,
                Id=Guid.NewGuid(),
                Orders=new(),
                RegistrationTime=DateTimeOffset.Now
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
        public static CreateClientRecordDto AsCreateClientRecordDto(this ClientRecord entity)
        {
            return (CreateClientRecordDto)entity;
        }
        public static ClientRecord AsClientRecord(this CreateClientRecordDto dto)
        {
            return (ClientRecord)dto;
        }
        public static IEnumerable<CreateClientRecordDto> AsCreateClientRecordDto(this IEnumerable<ClientRecord> entity)
        {
            return  entity.Select(x=>x.AsCreateClientRecordDto());
        }
        public static IEnumerable<ClientRecord> AsClientRecord(this IEnumerable<CreateClientRecordDto> dtos)
        {
            return dtos.Select(x=>x.AsClientRecord());
        }
    }
}

