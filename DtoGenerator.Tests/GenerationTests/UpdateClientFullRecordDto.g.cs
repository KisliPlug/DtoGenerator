using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace DtoGenerator.Tests.Resources
{
    public record UpdateClientFullRecordDto
    {
        public List<Guid> Orders { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0, 99999)]
        public decimal Price { get; set; }
        public static explicit operator ClientFullRecord(UpdateClientFullRecordDto b)
        {
            return new ClientFullRecord
            {
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Price=b.Price,
                Id=Guid.NewGuid(),
                RegistrationTime=DateTimeOffset.Now
            };
        }
        public static explicit operator UpdateClientFullRecordDto(ClientFullRecord b)
        {
            return new UpdateClientFullRecordDto
            {
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Price=b.Price
            };
        }
        public ClientFullRecord SetProps(ClientFullRecord dto)
        {
            return dto with
            {
                Orders=this.Orders,
                Name=this.Name,
                Description=this.Description,
                Price=this.Price
            };
        }
        public UpdateClientFullRecordDto GetProps(ClientFullRecord dto)
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
    public static class UpdateClientFullRecordDtoExtensions
    {
        public static UpdateClientFullRecordDto AsUpdateDto(this ClientFullRecord entity)
        {
            return (UpdateClientFullRecordDto)entity;
        }
        public static ClientFullRecord AsData(this UpdateClientFullRecordDto dto)
        {
            return (ClientFullRecord)dto;
        }
        public static IEnumerable<UpdateClientFullRecordDto> AsUpdateDto(this IEnumerable<ClientFullRecord> entity)
        {
            return  entity.Select(x=>x.AsUpdateDto());
        }
        public static IEnumerable<ClientFullRecord> AsData(this IEnumerable<UpdateClientFullRecordDto> dtos)
        {
            return dtos.Select(x=>x.AsData());
        }
    }
}

