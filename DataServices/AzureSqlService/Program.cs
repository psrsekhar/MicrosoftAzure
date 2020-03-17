using DataServices.Models;
using System;
using System.Data.SqlClient;
using System.Text;

namespace AzureSqlService
{
    public class Program
    {
        static void Main()
        {
            string[] first_name = { "Liam", "Emma", "Adler", "Anderson", "Beckett", "Brady", "Carson", "Carter" };
            string[] city = { "Campbell", "Orchard Park", "Fairport", "Buffalo", "Monroe", "Port Washington", "Monsey" };
            string[] state = { "NY", "CA", "TX" };

            Random random = new Random();
            Customer customer = new Customer();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "server address";
            builder.UserID = "user name";
            builder.Password = "Password";
            builder.InitialCatalog = "Database name";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO customers (first_name,last_name,email,phone,street,city,state,zip_code) VALUES (@first_name,@last_name,@email,@phone,@street,@city,@state,@zip_code);");
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

                    SqlConnection connection = new SqlConnection(builder.ConnectionString);
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sb.ToString(), connection))
                    {
                        command.Parameters.AddWithValue("@first_name", customer.first_name);
                        command.Parameters.AddWithValue("@last_name", customer.last_name);
                        command.Parameters.AddWithValue("@email", customer.email);
                        command.Parameters.AddWithValue("@phone", customer.phone);
                        command.Parameters.AddWithValue("@street", customer.street);
                        command.Parameters.AddWithValue("@city", customer.city);
                        command.Parameters.AddWithValue("@state", customer.state);
                        command.Parameters.AddWithValue("@zip_code", customer.zip_code);
                        if (command.ExecuteNonQuery() >= 0)
                        {
                            connection.Close();
                            System.Threading.Thread.Sleep(5000);
                        }
                    }
                }                
            }
            catch(Exception exception)
            {
                Console.WriteLine("Exception : " + exception.Message);
            }
        }
    }
}
