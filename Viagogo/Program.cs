using Viagogo.Common;
using Viagogo.DTOs;
using Viagogo.Models;


List<Event> events = new List<Event>{
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

List<Customer> customers = new List<Customer>
{
    new Customer { Name = "Mr. Fake", City = "New York" },
    new Customer { Name = "Jhon Smith", City = "Boston" }
};


Console.WriteLine("Send Email to a customer for an event at his/her location/city");
/// Solution for Question # 1
/// Send Email to a customer for an event at his/her location/city
new Emailcampaign(customers, events)
    .GetJoinedResult()
    .SendEventsAtCustomerLocation();


Console.WriteLine("End Solution 1");
Console.WriteLine("-----------");

Console.WriteLine("Send Email to a customer for an event at his/her location/city and nearby events Total of 5 events per customer ");
/// Solution for Question # 2
/// Send Email to a customer for an event at his/her location/city plus nearest Total of 5 events per customer
new Emailcampaign(customers, events)
    .SendEventsAtAndNearByToCustomer(noOfEvents: 5);
Console.WriteLine("-----------");



Console.WriteLine("Send Email to a customer for an event at his/her location/city and additional 5 nearby events");
/// Solution for Question # 2
/// Send Email to a customer for an event at his/her location/city and additional 5 nearby events
new Emailcampaign(customers, events)
    .GetJoinedResult()
    .SendEventsAtCustomerLocation()
    .SendEventsNearToCustomer(noOfEvents : 5);


Console.WriteLine("End Solution 2");
Console.ReadKey();
Console.WriteLine("-----------");



internal class Emailcampaign
{
    private  List<Event> _events;
    private  List<Customer> _customers;
    private  IEnumerable<ResultDto> _joinedResult;
    public Emailcampaign(List<Customer> customers,List<Event> events)
    {
        _events = events;
        _customers= customers;
    }
    public Emailcampaign GetJoinedResult() 
    {

        _joinedResult = _events.Join(_customers, e => e.City, c => c.City, (e, c) =>
                new ResultDto()
                {
                    Item = new CustomerEvent()
                    {
                        Event = e,
                        Customer = c
                    },
                    Distance = GetDistance(c.City, e.City)
                },
            new ContainsComparator());
        return this;
    }

    public Emailcampaign SendEventsAtCustomerLocation(IEnumerable<ResultDto>? joinedResult=null)
    {
        (joinedResult ?? _joinedResult)
            .OrderBy(r => r.Item.Customer.Name)
            .ToList()
            .ForEach(e => AddToEmail(e.Item.Customer, e.Item.Event, e.Distance));
        return this;
    }
    public void SendEventsAtAndNearByToCustomer(int noOfEvents = 5) =>
        _customers
            .ForEach(x => GetCustomerAtAndNearByEvents(x, noOfEvents));
     
    
    public void SendEventsNearToCustomer(IEnumerable<ResultDto>? joinedResult = null, int noOfEvents = 5) =>
        (joinedResult ?? _joinedResult)
            .DistinctBy(e => e.Item.Customer)
            .ToList()
            .ForEach(x => GetCustomerNearByEvents(x.Item.Customer, noOfEvents));


    private void GetCustomerAtAndNearByEvents(Customer customer, int noOfEvents = 5) =>
        _events
            .Select(e => new ResultDto() { Item = new CustomerEvent() { Customer = customer, Event = e }, Distance = GetDistance(e.City, customer.City) })
            .OrderBy(e => e.Distance)
            .OrderBy(e => e.Item.Customer)
            .DistinctBy(e => e.Item)
            .Take(noOfEvents)
            .ToList()
            .ForEach(e => AddToEmail(e.Item.Customer, e.Item.Event, e.Distance));
    
    private void GetCustomerNearByEvents(Customer customer, int noOfEvents = 5)
    {
        var listEvents = _joinedResult.Where(x => x.Item.Customer == customer).Select(x => x.Item.Event);
        _events
            .Where(e => !listEvents.Contains(e))
            .Select(e => new ResultDto() { Item = new CustomerEvent() { Customer = customer, Event = e }, Distance = GetDistance(e.City, customer.City) })
            .OrderBy(e => e.Distance)
            .OrderBy(e => e.Item.Customer)
            .DistinctBy(e => e.Item)
            .Take(noOfEvents)
            .ToList()
            .ForEach(e => AddToEmail(e.Item.Customer, e.Item.Event, e.Distance));
    }
    

    private static void AddToEmail(Customer c, Event e, double d) =>
        Console.WriteLine($"Customer= Name: {c.Name}, City: {c.City} Event= Name: {e.Name}, City: {e.City},   Distance : {d} KM");

    private static double GetDistance(string city1, string city2)
    {
        if (city1.Contains(city2, StringComparison.OrdinalIgnoreCase) || city2.Contains(city1, StringComparison.OrdinalIgnoreCase))
            return 0;

        var result = 0;
        var i = 0;
        for (i = 0; i < Math.Min(city1.Length, city2.Length); i++)
        {
            result += Math.Abs(city1[i] - city2[i]);
        }
        for (; i < Math.Max(city1.Length, city2.Length); i++)
        {
            result += city1.Length > city2.Length ? city1[i] : city2[i];
        }
        return result;

    }
}



