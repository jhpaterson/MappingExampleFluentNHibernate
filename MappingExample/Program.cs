using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;


namespace MappingExample
{
    class Program
    {
        static void Main(string[] args)
        {
#region CONFIGURE
            var cfg = new Configuration();
            cfg.Configure();
            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                    .ConnectionString(c => c
                        .FromConnectionStringWithKey("CompanyConnection"))
                .ShowSql())
                .Mappings(m => m
                    .FluentMappings.AddFromAssemblyOf<Employee>()
                    .Conventions.AddFromAssemblyOf<Employee>())
                .BuildSessionFactory();
#endregion

#region ADD NEW
            // save, then read in a separate session
            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                HourlyPaidEmployee newEmp = new HourlyPaidEmployee
                {
                    Name = "Seb",
                    Username = "seb",
                    PhoneNumber = "9876",
                    Address = session.Load<Address>(2)     // Load creates a proxy, does not actually query database, so no need for FK fields (would get mapping error anyway - can't map column as property and as FK)
                };                                         // Get actually loads entity
                session.Save(newEmp);
                transaction.Commit();
            }
#endregion

            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
#region GET
                // Get an Employee
                Employee emp = session.Get<Employee>(2);
                Console.WriteLine(emp.Name + ", " + emp.Address.PropertyName);

                // Get an Address
                Address add = session.Get<Address>(2);
                Console.WriteLine(add.PropertyName);
                foreach (Employee em in add.Employees)
                {
                    Console.WriteLine(em.Name);
                }
 #endregion

#region HQL
                // HQL queries
                var emps1 = session.CreateQuery(
                    "from Employee as em where em.Address = ?")
                    .SetEntity(0, session.Load<Address>(2))
                    .List<Employee>();

                var emps2 = session.CreateQuery(
                    "from Employee as em where em.Address.AddressID = ?")
                    .SetParameter(0, 2)
                    .List<Employee>();
#endregion

#region CRITERIA + SQL
                Address addr = session.Get<Address>(2);

                // Criteria query
                var emps3 = session.CreateCriteria<Employee>()
                    .Add(Restrictions.Eq("Address", addr))
                    .SetFetchMode("Address", FetchMode.Join)
                    .List<Employee>();

                // SQL query
                var emps4 = session.CreateSQLQuery(
                    "SELECT * FROM Employees WHERE AddressID = :address")
                    .AddEntity(typeof(Employee))
                    .SetInt32("address", addr.AddressID)
                    .List<Employee>();
#endregion

#region LINQ
                // LINQ queries (supported in NHibernate 3)
                var emps5 = session.Query<Employee>()
                    .Where(e => e.Address == 
                        session.Get<Address>(2))
                    .Fetch(e => e.Address)
                    .ToList<Employee>();

                foreach (Employee em in emps5)
                {
                    Console.WriteLine(em.Name);
                    Console.WriteLine(em.Address.PropertyName);
                }

                var emps6 = (from e in session.Query<Employee>()
                             where e.Address == session.Get<Address>(2)
                             select e)
                    .ToList<Employee>();
#endregion

                // wait for key press before ending
                Console.ReadLine();
            }

        }
    }
}
