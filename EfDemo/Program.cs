using System;
using System.Collections.Generic;
using System.Data;
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
            EagerLoad();
            //ImplicitLoad();
            //ExecuteProcedure();
            Console.ReadKey();
        }

        private static void ExecuteProcedure()
        {
            var product = new Product { Name = "IOMonitor" };
            using (var context = new PortalContext())
            {
                if (!context.Products.Any(x => x.Name == product.Name))
                {
                    var productNameParam = new SqlParameter("@name", SqlDbType.NVarChar, 250);
                    productNameParam.Value = product.Name;
                    var result = context.Database.ExecuteSqlCommand("insert_product @name", productNameParam);
                }

                var insert = context.Products.Where(x => x.Name == product.Name).FirstOrDefault();
                if (insert != null)
                {
                    var productIdParam = new SqlParameter("@product_id", SqlDbType.Int, 4);
                    productIdParam.Value = insert.ID;
                    var user1 = new User { Name = "KCE", ProductID = insert.ID };
                    var user2 = new User { Name = "IIO", ProductID = insert.ID };
                    var userNameParam1 = new SqlParameter("@name", SqlDbType.NVarChar, 250);
                    userNameParam1.Value = user1.Name;
                    var userNameParam2 = new SqlParameter("@name", SqlDbType.NVarChar, 250);
                    userNameParam2.Value = user2.Name;
                    var user_sql = "insert_user @product_id, @name";
                    context.Database.ExecuteSqlCommand(user_sql, productIdParam, userNameParam1);
                    context.Database.ExecuteSqlCommand(user_sql, productIdParam, userNameParam2);
                }
            }
        }

        private static void ImplicitLoad()
        {
            using (var context = new PortalContext())
            {
                var products = context.Products.ToArray(); // To Use Implicit query, must close first connection.

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
                        var users = context.Entry(product).Collection(p => p.Users).Query().Select(x => x).ToArray();
                        foreach (var user in users)
                        {
                            builder.Append(user.Name).Append(" ");
                        }
                        Console.WriteLine($"Users: {builder.ToString()}");
                        Console.WriteLine();
                    }
                }
            }
        }

        private static void EagerLoad()
        {
            using (var context = new PortalContext())
            {
                var products = context.Products.Include("Users"); //Eager loading.

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
            }

        }

    }
}
