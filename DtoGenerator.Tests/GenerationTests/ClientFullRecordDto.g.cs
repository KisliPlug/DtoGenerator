using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace DtoGenerator.Tests.Resources
{
    public record ClientFullRecordDto
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
        public static explicit operator ClientFullRecord(ClientFullRecordDto b)
        {
            return new ClientFullRecord
            {
                Id=b.Id,
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Price=b.Price,
                RegistrationTime=b.RegistrationTime
            };
        }
        public static explicit operator ClientFullRecordDto(ClientFullRecord b)
        {
            return new ClientFullRecordDto
            {
                Id=b.Id,
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Price=b.Price,
                RegistrationTime=b.RegistrationTime
            };
        }
        public ClientFullRecord SetProps(ClientFullRecord dto)
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
        public ClientFullRecordDto GetProps(ClientFullRecord dto)
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
    public static class ClientFullRecordDtoExtensions
    {
        public static ClientFullRecordDto AsDto(this ClientFullRecord entity)
        {
            return (ClientFullRecordDto)entity;
        }
        public static ClientFullRecord AsData(this ClientFullRecordDto dto)
        {
            return (ClientFullRecord)dto;
        }
        public static IEnumerable<ClientFullRecordDto> AsDto(this IEnumerable<ClientFullRecord> entity)
        {
            return  entity.Select(x=>x.AsDto());
        }
        public static IEnumerable<ClientFullRecord> AsData(this IEnumerable<ClientFullRecordDto> dtos)
        {
            return dtos.Select(x=>x.AsData());
        }
    }
}

