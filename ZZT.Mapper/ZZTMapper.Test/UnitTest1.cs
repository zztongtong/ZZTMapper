using System;
using System.Collections.Generic;
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

            ZZTMapper.CreateMap<Person, Customer>();
            Person per = new Person { FirstName = "firstName", LastName = "lastName",Notes="notes" };
            Customer cus = ZZTMapper.Map<Customer>(per);
          
            Customer cus2 = new Customer { FirstName = "firstName2", LastName = "lastName2",MyNotes="mynotes" };
            Person per2 = ZZTMapper.Map<Person>(cus2);
           
        }
    }

    [AutoMap(typeof(Customer),Reverse =true)]
    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [MapToOrFromProperty(typeof(Customer),nameof(Customer.MyNotes))]
        public string Notes { get; set; }

        public string DEF { get; set; }

        public List<Person> Ps { get; set; }
    }

    public class Customer
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MyNotes { get; set; }

        public string DEF2 { get; set; }
    }
}
