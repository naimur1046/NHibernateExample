using System;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernateExample.Models;

namespace NHibernateExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connection string to the database
            string connStr = "Data Source=NAIMURRAHMAN;Initial Catalog=STUDENT;TrustServerCertificate=True; Trusted_Connection=True;";


            // Configure NHibernate
            var config = new Configuration();
            config.DataBaseIntegration(d =>
            {
                d.ConnectionString = connStr;
                d.Dialect<MsSql2012Dialect>();
                d.Driver<NHibernate.Driver.MicrosoftDataSqlClientDriver>();

            });

            // Add assembly mapping (Product.hbm.xml)
            config.AddAssembly(Assembly.GetExecutingAssembly());

            // Build session factory
            var sessionFactory = config.BuildSessionFactory();

            // Open session and begin transaction
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Create a new Product object
                        var product = new Product
                        {
                            Name = "Rumel",
                            Description = "Hi this is",
                            Price = 250
                        };

                        // Save the product to the database
                        session.Save(product);

                        // Commit the transaction
                        transaction.Commit();
                        Console.WriteLine("Product saved successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}
