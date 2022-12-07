using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CodeGen.Tests.Resources
{
    public record UpdateClientRecordDto
    {
        public List<Guid> Orders {get;init;}
        public string Name {get;init;}
        public string Description {get;init;}
        [Required] [Range(0,99999)]public decimal Price {get;init;}
        public static explicit operator UpdateClientRecordDto(ClientRecord b)
        {
            return new UpdateClientRecordDto
            {
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Price=b.Price
            };
        }
        public static explicit operator ClientRecord(UpdateClientRecordDto b)
        {
            return new ClientRecord
            {
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Price=b.Price,
                Id=Guid.NewGuid(),
                RegistrationTime=DateTimeOffset.Now
            };
        }
        public ClientRecord SetProps(ClientRecord dto)
        {
            return dto with
            {
                Orders=this.Orders,
                Name=this.Name,
                Description=this.Description,
                Price=this.Price
            };
        }
        public UpdateClientRecordDto GetProps(ClientRecord dto)
        {
            return this with
            {
                Orders=dto.Orders,
                Name=dto.Name,
                Description=dto.Description,
                Price=dto.Price
            };
        }
    }
    public static class UpdateClientRecordDtoExtensions
    {
        public static UpdateClientRecordDto AsUpdateClientRecordDto(this ClientRecord entity)
        {
            return (UpdateClientRecordDto)entity;
        }
        public static ClientRecord AsClientRecord(this UpdateClientRecordDto dto)
        {
            return (ClientRecord)dto;
        }
        public static IEnumerable<UpdateClientRecordDto> AsUpdateClientRecordDto(this IEnumerable<ClientRecord> entity)
        {
            return  entity.Select(x=>x.AsUpdateClientRecordDto());
        }
        public static IEnumerable<ClientRecord> AsClientRecord(this IEnumerable<UpdateClientRecordDto> dtos)
        {
            return dtos.Select(x=>x.AsClientRecord());
        }
    }
}

