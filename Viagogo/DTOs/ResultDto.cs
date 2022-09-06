using Viagogo.Models;

namespace Viagogo.DTOs
{
    public class ResultDto
    {
        public CustomerEvent Item { get; set; }
        public double Distance { get; set; } = 0;
    }

    public class CustomerEvent
    {
        public Customer Customer { get; set; }
        public Event Event { get; set; }
    }
}
