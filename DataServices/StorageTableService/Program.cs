using DataServices.Models;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace StorageTableService
{
    public class Program
    {
        static string connectionString = "connection string from Access Keys";
        static async Task Main()
        {
            Random random = new Random();
            CustomerEntity customer = new CustomerEntity();
            string[] first_name = { "Liam", "Emma", "Adler", "Anderson", "Beckett", "Brady", "Carson", "Carter" };
            string[] city = { "Campbell", "Orchard Park", "Fairport", "Buffalo", "Monroe", "Port Washington", "Monsey" };
            string[] state = { "NY", "CA", "TX" };
            try
            {

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
                tableClient.TableClientConfiguration.UseRestExecutorForCosmosEndpoint = true;
                CloudTable table = tableClient.GetTableReference("customers");
                await table.CreateIfNotExistsAsync();
                for (int i = 1; i > 0; i++)
                {
                    customer.PartitionKey = ""+random.Next(10000, 999999)+ random.Next(10000, 999999);
                    customer.RowKey = "abcdefghijklmnopqrstuvwxyz"[random.Next(0, 25)].ToString() + "abcdefghijklmnopqrstuvwxyz"[random.Next(0, 25)].ToString() + "abcdefghijklmnopqrstuvwxyz"[random.Next(0, 25)].ToString() + "@gmail.com";
                    customer.first_name = first_name[random.Next(0, 8)];
                    customer.last_name = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[random.Next(0, 25)].ToString();
                    customer.phone = "9" + random.Next(000000000, 999999999);                    
                    customer.street = "#Site " + random.Next(2, 100) + "," + "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[random.Next(0, 30)].ToString() + " Block," + random.Next(1, 20) + " Sector";
                    customer.city = city[random.Next(0, 6)];
                    customer.state = state[random.Next(0, 2)];
                    customer.zip_code = "" + random.Next(10000, 99999);

                    TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(customer);
                    TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                    if(result.HttpStatusCode == 204)
                    {
                        System.Threading.Thread.Sleep(2000);
                    }                    
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception : " + exception.Message);
            }

        }
    }
}
