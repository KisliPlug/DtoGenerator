using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace DtoGenerator.Tests.Resources
{
    public record UpdateClientRecordContract
    {
        public List<Guid> Orders {get;init;}
        public string Name {get;init;}
        public string Description {get;init;}
        [Required] [Range(0, 99999)]public decimal Price {get;init;}
        public static explicit operator ClientRecord(UpdateClientRecordContract b)
        {
            return new ClientRecord
            (
            Orders:b.Orders,
            Name:b.Name,
            Description:b.Description,
            Price:b.Price,
            Id:Guid.NewGuid(),
            RegistrationTime:DateTimeOffset.Now
            );
        }
        public static explicit operator UpdateClientRecordContract(ClientRecord b)
        {
            return new UpdateClientRecordContract
            {
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Price=b.Price
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
        public UpdateClientRecordContract GetProps(ClientRecord dto)
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
    public static class UpdateClientRecordContractExtensions
    {
        public static UpdateClientRecordContract AsUpdateContract(this ClientRecord entity)
        {
            return (UpdateClientRecordContract)entity;
        }
        public static ClientRecord AsData(this UpdateClientRecordContract dto)
        {
            return (ClientRecord)dto;
        }
        public static IEnumerable<UpdateClientRecordContract> AsUpdateContract(this IEnumerable<ClientRecord> entity)
        {
            return  entity.Select(x=>x.AsUpdateContract());
        }
        public static IEnumerable<ClientRecord> AsData(this IEnumerable<UpdateClientRecordContract> dtos)
        {
            return dtos.Select(x=>x.AsData());
        }
    }
}

