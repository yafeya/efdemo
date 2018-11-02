using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new PortalContext();
            var products = context.Products.ToArray();
            var users = context.Users.ToArray();
            if (products == null || products.Count() == 0)
            {
                var iolibs_product = new Product { Name = "IOLIBS" };
                var user1 = new User { Name = "Keysight" };
                var user2 = new User { Name = "Cisco" };
                iolibs_product.Users = new[] { user1, user2 };
                context.Products.Add(iolibs_product);
                context.Users.AddRange(new[] { user1, user2 });
                context.SaveChanges();
            }
            else
            {
                foreach (var product in products)
                {
                    Console.WriteLine($"Product: {product.Name}");
                    var builder = new StringBuilder();
                    foreach (var user in product.Users)
                    {
                        builder.Append(user.Name).Append(" ");
                    }
                    Console.WriteLine($"Users: {builder.ToString()}");
                    Console.WriteLine();
                }
            }

            Console.ReadKey();
        }
    }
}
