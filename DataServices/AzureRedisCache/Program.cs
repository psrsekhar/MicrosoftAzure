using StackExchange.Redis;
using System;
using DataServices.Models;
using Newtonsoft.Json;

namespace AzureRedisCache
{
    class Program
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string connectionString = "enetr your connection string here";
            return ConnectionMultiplexer.Connect(connectionString);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
        static void Main(string[] args)
        {
            string[] first_name = { "Liam", "Emma", "Adler", "Anderson", "Beckett", "Brady", "Carson", "Carter" };
            string[] city = { "Campbell", "Orchard Park", "Fairport", "Buffalo", "Monroe", "Port Washington", "Monsey" };
            string[] state = { "NY", "CA", "TX" };
            
            Random random = new Random();
            Customer customer = new Customer();
            try
            {
                IDatabase cache = Connection.GetDatabase();
                for (int i = 1; i <= 10; i++)
                {
                    customer.first_name = first_name[random.Next(0, 8)];
                    customer.last_name = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[random.Next(0, 25)].ToString();
                    customer.phone = "9" + random.Next(000000000, 999999999);
                    customer.email = "abcdefghijklmnopqrstuvwxyz"[random.Next(0, 25)].ToString() + "abcdefghijklmnopqrstuvwxyz"[random.Next(0, 25)].ToString() + "abcdefghijklmnopqrstuvwxyz"[random.Next(0, 25)].ToString() + "@gmail.com";
                    customer.street = "#Site " + random.Next(2, 100) + "," + "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[random.Next(0, 30)].ToString() + " Block," + random.Next(1, 20) + " Sector";
                    customer.city = city[random.Next(0, 6)];
                    customer.state = state[random.Next(0, 2)];
                    customer.zip_code = "" + random.Next(10000, 99999);

                    //to insert data into cache
                    cache.StringSet(customer.phone, JsonConvert.SerializeObject(customer));
                }

                customer = JsonConvert.DeserializeObject<Customer>(cache.StringGet(customer.phone));
                Console.WriteLine("Name : " + customer.last_name + " " + customer.first_name);
                Connection.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
