using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace DtoGenerator.Tests.Resources
{
    public record CreateClientFullRecordDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0, 99999)]
        public decimal Price { get; set; }
        public static explicit operator CreateClientFullRecordDto(ClientFullRecord b)
        {
            return new CreateClientFullRecordDto
            (
            Name:b.Name,
            Description:b.Description,
            Price:b.Price
            );
        }
        public static explicit operator ClientFullRecord(CreateClientFullRecordDto b)
        {
            return new ClientFullRecord
            (
            Name:b.Name,
            Description:b.Description,
            Price:b.Price,
            Id:Guid.NewGuid(),
            Orders:new(),
            RegistrationTime:DateTimeOffset.Now
            );
        }
        public ClientFullRecord SetProps(ClientFullRecord dto)
        {
            return dto with
            {
                Name=this.Name,
                Description=this.Description,
                Price=this.Price
            };
        }
        public CreateClientFullRecordDto GetProps(ClientFullRecord dto)
        {
            return this with
            {
                Name=dto.Name,
                Description=dto.Description,
                Price=dto.Price
            };
        }
    }
    public static class CreateClientFullRecordDtoExtensions
    {
        public static CreateClientFullRecordDto AsCreateClientFullRecordDto(this ClientFullRecord entity)
        {
            return (CreateClientFullRecordDto)entity;
        }
        public static ClientFullRecord AsClientFullRecord(this CreateClientFullRecordDto dto)
        {
            return (ClientFullRecord)dto;
        }
        public static IEnumerable<CreateClientFullRecordDto> AsCreateClientFullRecordDto(this IEnumerable<ClientFullRecord> entity)
        {
            return  entity.Select(x=>x.AsCreateClientFullRecordDto());
        }
        public static IEnumerable<ClientFullRecord> AsClientFullRecord(this IEnumerable<CreateClientFullRecordDto> dtos)
        {
            return dtos.Select(x=>x.AsClientFullRecord());
        }
    }
}

