using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace MappingExample.Mappings
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Id(x => x.EmployeeID);
            Map(x => x.Name)
                .Column("name");
            Map(x => x.Username)
                .Column("username");
            Map(x => x.PhoneNumber)
                .Column("phoneNumber");
            References(x => x.Address)
                .Column("addressID")
                .LazyLoad(Laziness.Proxy);
                //.Fetch.Join();
            DiscriminateSubClassesOnColumn("discriminator");

        }
    }

    public class SalariedEmployeeMap : SubclassMap<SalariedEmployee>
    {
        public SalariedEmployeeMap()
        {
            Map(x => x.PayGrade)
                .Column("payGrade");
            DiscriminatorValue("S         ");
        }
    }

    public class HourlyPaidEmployeeMap : SubclassMap<HourlyPaidEmployee>
    {
        public HourlyPaidEmployeeMap()
        {
            DiscriminatorValue("H         ");
        }
    }
}
