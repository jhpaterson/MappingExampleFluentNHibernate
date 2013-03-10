using FluentNHibernate.Mapping;

namespace MappingExample.Mappings
{
    public class AddressMap : ClassMap<Address>
    {
        public AddressMap()
        {
            Id(x => x.AddressID);
            Map(x => x.PropertyName)
                .Column("propertyName");
            Map(x => x.PropertyNumber)
                .Column("propertyNumber");
            Map(x => x.PostCode)
                .Column("postCode");
            HasMany(x => x.Employees)
                .KeyColumn("addressID")
                .Inverse()
                .Cascade.All()
                //.Not
                .LazyLoad();
               // .Fetch.Join();
        }
    }
}
