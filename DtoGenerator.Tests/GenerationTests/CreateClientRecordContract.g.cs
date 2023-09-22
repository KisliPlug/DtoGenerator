using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace DtoGenerator.Tests.Resources
{
    public record CreateClientRecordContract
    {
        public string Name {get;init;}
        public string Description {get;init;}
        [Required] [Range(0, 99999)]public decimal Price {get;init;}
        public static explicit operator ClientRecord(CreateClientRecordContract b)
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
        public static explicit operator CreateClientRecordContract(ClientRecord b)
        {
            return new CreateClientRecordContract
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
        public CreateClientRecordContract GetProps(ClientRecord dto)
        {
            return this with
            {
                Name=dto.Name,
                Description=dto.Description,
                Price=dto.Price
            };
        }
    }
    public static class CreateClientRecordContractExtensions
    {
        public static CreateClientRecordContract AsCreateContract(this ClientRecord entity)
        {
            return (CreateClientRecordContract)entity;
        }
        public static ClientRecord AsData(this CreateClientRecordContract dto)
        {
            return (ClientRecord)dto;
        }
        public static IEnumerable<CreateClientRecordContract> AsCreateContract(this IEnumerable<ClientRecord> entity)
        {
            return  entity.Select(x=>x.AsCreateContract());
        }
        public static IEnumerable<ClientRecord> AsData(this IEnumerable<CreateClientRecordContract> dtos)
        {
            return dtos.Select(x=>x.AsData());
        }
    }
}

