using DataServices.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Newtonsoft.Json;
using System;

namespace StorageQueueService
{
    public class Program
    {
        static string connectionString = "connection string from Access Keys";
        static void Main()
        {
            Random random = new Random();
            Customer customer = new Customer();            
            string[] first_name = { "Liam", "Emma", "Adler", "Anderson", "Beckett", "Brady", "Carson", "Carter" };
            string[] city = { "Campbell", "Orchard Park", "Fairport", "Buffalo", "Monroe", "Port Washington", "Monsey" };
            string[] state = { "NY", "CA", "TX" };
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue queue = queueClient.GetQueueReference("customers");
                queue.CreateIfNotExists();
                for (int i = 1; i > 0; i++)
                {
                    customer.first_name = first_name[random.Next(0, 8)];
                    customer.last_name = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[random.Next(0, 25)].ToString();
                    customer.phone = "9" + random.Next(000000000, 999999999);
                    customer.email = "abcdefghijklmnopqrstuvwxyz"[random.Next(0, 25)].ToString() + "abcdefghijklmnopqrstuvwxyz"[random.Next(0, 25)].ToString() + "abcdefghijklmnopqrstuvwxyz"[random.Next(0, 25)].ToString() + "@gmail.com";
                    customer.street = "#Site " + random.Next(2, 100) + "," + "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[random.Next(0, 30)].ToString() + " Block," + random.Next(1, 20) + " Sector";
                    customer.city = city[random.Next(0, 6)];
                    customer.state = state[random.Next(0, 2)];
                    customer.zip_code = "" + random.Next(10000, 99999);

                    CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(customer));                    
                    queue.AddMessage(message);
                    System.Threading.Thread.Sleep(3000);
                }                                    
            }
            catch(Exception exception)
            {
                Console.WriteLine("Exception : " + exception.Message);
            }
        }
    }
}
