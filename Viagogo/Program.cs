using Viagogo.Models;


var events = new List<Event>{
    new Event{ Name = "Phantom of the Opera", City = "New York"},
    new Event{ Name = "Metallica", City = "Los Angeles"},
    new Event{ Name = "Metallica", City = "New York"},
    new Event{ Name = "Metallica", City = "Boston"},
    new Event{ Name = "LadyGaGa", City = "New Yorks"},
    new Event{ Name = "LadyGaGa", City = "Boston"},
    new Event{ Name = "LadyGaGa", City = "Chicago"},
    new Event{ Name = "LadyGaGa", City = "San Francisco"},
    new Event{ Name = "LadyGaGa", City = "Washington"}
};

var customers = new List<Customer>
{
    new Customer { Name = "Mr. Fake", City = "New York" },
    new Customer { Name = "Jhon Smith", City = "Boston" }
};

// Solution 1 
var resultJoin = events.Join(customers, e => e.City, c => c.City, (e, c) => 
        new  
        {
            Event = e,
            Customer = c
        },
        new ContainsComparator());


// Solution 2
var resultQuery =
    from eachEvent in events
    from eachCustomer in customers 
    where eachEvent.City.Contains(eachCustomer.City,StringComparison.OrdinalIgnoreCase)
    select new
    {
        Customer = eachCustomer,
        Event = eachEvent
    };



resultJoin.ToList().ForEach(e => AddToEmail(e.Customer, e.Event));

Console.ReadKey();


void AddToEmail(Customer c, Event e) =>
    Console.WriteLine($"Customer= Name: {c.Name}, City: {c.City} Event= Name: {e.Name}, City: {e.City}");

public class ContainsComparator : IEqualityComparer<string>
{
    public bool Equals(string x, string y)
    {
        return x.Contains(y) || y.Contains(x);
    }

    public int GetHashCode(string obj)
    {
        return obj[0].GetHashCode();
    }
}