using System;
using Xunit;
using ZZT.Mapper;
using ZZT.Mapper.Attr;

namespace ZZT.Mapper.Test
{
    public class UnitTest1
    {
        [Fact]
        public void MapTest()
        {
            Person per = new Person { FirstName = "firstName", LastName = "lastName",Notes="notes" };
            Customer cus = ZZTMapper.Map<Customer, Person>(per);
          
            Customer cus2 = new Customer { FirstName = "firstName2", LastName = "lastName2",MyNotes="mynotes" };
            Person per2 = ZZTMapper.Map<Person, Customer>(cus2);
           
        }
    }

    [AutoMap(typeof(Customer))]
    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [MapToOrFromProperty(typeof(Customer),nameof(Customer.MyNotes))]
        public string Notes { get; set; }
    }

    public class Customer
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MyNotes { get; set; }
    }
}
