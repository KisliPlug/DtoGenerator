using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace DtoGenerator.Tests.Resources
{
    public record ClientRecordContract
    {
        [Required]public Guid Id {get;init;}
        public List<Guid> Orders {get;init;}
        public string Name {get;init;}
        public string Description {get;init;}
        [Required] [Range(0, 99999)]public decimal Price {get;init;}
        public DateTimeOffset RegistrationTime {get;init;}
        public static explicit operator ClientRecord(ClientRecordContract b)
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
        public static explicit operator ClientRecordContract(ClientRecord b)
        {
            return new ClientRecordContract
            {
                Id=b.Id,
                Orders=b.Orders,
                Name=b.Name,
                Description=b.Description,
                Price=b.Price,
                RegistrationTime=b.RegistrationTime
            };
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
        public ClientRecordContract GetProps(ClientRecord dto)
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
    public static class ClientRecordContractExtensions
    {
        public static ClientRecordContract AsContract(this ClientRecord entity)
        {
            return (ClientRecordContract)entity;
        }
        public static ClientRecord AsData(this ClientRecordContract dto)
        {
            return (ClientRecord)dto;
        }
        public static IEnumerable<ClientRecordContract> AsContract(this IEnumerable<ClientRecord> entity)
        {
            return  entity.Select(x=>x.AsContract());
        }
        public static IEnumerable<ClientRecord> AsData(this IEnumerable<ClientRecordContract> dtos)
        {
            return dtos.Select(x=>x.AsData());
        }
    }
}

