using DataServices.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StorageBlobService
{
    public class Program
    {
        static string connectionString = "connection string from Access Keys";
        static async Task Main()
        {
            Random random = new Random();
            Customer customer = new Customer();
            List<Customer> customers = new List<Customer>();
            string[] first_name = { "Liam", "Emma", "Adler", "Anderson", "Beckett", "Brady", "Carson", "Carter" };            
            string[] city = { "Campbell", "Orchard Park", "Fairport", "Buffalo", "Monroe", "Port Washington", "Monsey" };
            string[] state = { "NY", "CA", "TX" };
            string[] pincode = { "95381", "66197", "83458", "44639", "73945" };

            for (int i = 1; i > 0; i++)
            {
                string customerString = GetBlob("uploads", "customers.json");
                if (customerString != null)
                {
                    try
                    {
                        if(!string.IsNullOrEmpty(customerString))
                        {
                            customers = JsonConvert.DeserializeObject<List<Customer>>(customerString);
                        }                        
                        customer.first_name = first_name[random.Next(0, 8)];
                        customer.last_name = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[random.Next(0, 25)].ToString();
                        customer.phone = "9" + random.Next(000000000, 999999999);
                        customer.email = "abcdefghijklmnopqrstuvwxyz"[random.Next(0, 25)].ToString() + "abcdefghijklmnopqrstuvwxyz"[random.Next(0, 25)].ToString() + "abcdefghijklmnopqrstuvwxyz"[random.Next(0, 25)].ToString() + "@gmail.com";
                        customer.street = "#Site " + random.Next(2, 100) + "," + "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[random.Next(0, 30)].ToString() + " Block," + random.Next(1, 20) + " Sector";
                        customer.city = city[random.Next(0, 6)];
                        customer.state = state[random.Next(0, 2)];
                        customer.zip_code = pincode[random.Next(0, 4)];
                        customers.Add(customer);
                        bool status = await UploadBlob("uploads", "customers.json", customers);
                        if (status)
                        {
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

        public static string GetBlob(string containerName, string fileName)
        {
            try
            {
                // Setup the connection to the storage account
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

                // Connect to the blob storage
                CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();

                // Connect to the blob container
                CloudBlobContainer container = serviceClient.GetContainerReference($"{containerName}");

                // Connect to the blob file
                CloudBlockBlob blob = container.GetBlockBlobReference($"{fileName}");

                // Get the blob file as text                
                string contents = blob.DownloadTextAsync().Result;

                return contents;
            }
            catch(Exception exception)
            {
                Console.WriteLine("Exception : " + exception.Message);
                return null;
            }
        }

        public static async Task<Boolean> UploadBlob(string containerName, string fileName, List<Customer> customers)
        {
            try
            {
                // Setup the connection to the storage account
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

                // Connect to the blob storage
                CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();

                // Connect to the blob container
                CloudBlobContainer container = serviceClient.GetContainerReference($"{containerName}");

                // Connect to the blob file
                CloudBlockBlob blob = container.GetBlockBlobReference($"{fileName}");                
                
                // Get the blob file as text                
                await blob.UploadTextAsync(JsonConvert.SerializeObject(customers));

                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception : " + exception.Message);
                return false;
            }
        }
    }
}