using Microsoft.Azure.Cosmos.Table;

namespace DataServices.Models
{
    public class Customer
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
    }

    public class CustomerEntity : TableEntity
    {
        public CustomerEntity()
        {
        }

        public CustomerEntity(string customerId, string email)
        {
            PartitionKey = customerId;
            RowKey = email;
        }

        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }        
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
    }
}
