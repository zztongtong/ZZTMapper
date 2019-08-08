using System;
using Xunit;
using ZZT.Mapper;

namespace ZZTMapper.Test
{
    public class UnitTest1
    {
        [Fact]
        public void MapTest()
        {
            Person per = new Person { FirstName = "firstName", LastName = "lastName" };
            Customer cus = AutoMapper.Map<Customer, Person>(per);
          
            Customer cus2 = new Customer { FirstName = "firstName2", LastName = "lastName2" };
            Person per2 = AutoMapper.Map<Person, Customer>(cus2);
           
        }
    }

    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class Customer
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
