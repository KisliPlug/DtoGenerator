using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestNuget.Resources
{
    public record UpdateClientRecordDto1
    {
        public List<Guid> Orders { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }

        [Required]
        [Range(0, 99999)]
        public decimal Price { get; init; }

        public static explicit operator UpdateClientRecordDto1(ClientRecord b)
        {
            return new UpdateClientRecordDto1
                   {
                       Orders = b.Orders
                     , Name = b.Name
                     , Description = b.Description
                     , Price = b.Price
                   };
        }

        public static explicit operator ClientRecord(UpdateClientRecordDto1 b)
        {
            return new ClientRecord(Orders: b.Orders
                                  , Name: b.Name
                                  , Description: b.Description
                                  , Price: b.Price
                                  , Id : Guid.NewGuid()
                                  , RegistrationTime : DateTimeOffset.Now);
        }

        public ClientRecord SetProps(ClientRecord dto)
        {
            return dto with
                   {
                       Orders = this.Orders
                     , Name = this.Name
                     , Description = this.Description
                     , Price = this.Price
                   };
        }

        public UpdateClientRecordDto1 GetProps(ClientRecord dto)
        {
            return this with
                   {
                       Orders = dto.Orders
                     , Name = dto.Name
                     , Description = dto.Description
                     , Price = dto.Price
                   };
        }
    }
}
