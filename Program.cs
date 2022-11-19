using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Viagogo
{
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
    }
    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
    }
    public class Solution
    {
        static Dictionary<Customer, List<Event>> dicCustomerEvents = new Dictionary<Customer, List<Event>>();

        static void Main(string[] args)
        {
            var events = new List<Event>{
                                new Event{ Name = "Phantom of the Opera", City = "New York"},
                                new Event{ Name = "Metallica", City = "Los Angeles"},
                                new Event{ Name = "Metallica", City = "New York"},
                                new Event{ Name = "Metallica", City = "Boston"},
                                new Event{ Name = "LadyGaGa", City = "New York"},
                                new Event{ Name = "LadyGaGa", City = "Boston"},
                                new Event{ Name = "LadyGaGa", City = "Chicago"},
                                new Event{ Name = "LadyGaGa", City = "San Francisco"},
                                new Event{ Name = "LadyGaGa", City = "Washington"}
                                };

            //1. find out all events that arein cities of customer
            // then add to email.
            var customer = new Customer { Name = "John Smith", City = "New York" };

            #region mycode Biruk Abel

            //Question 1.1 What should be your approach to getting the list of events
            //Answer: using LINQ method expression (Lambda expression) to determine if event is located in Customer's city or not as follows

            var eventsforCustomer = events.Where(x => x.City == customer.City);

            //Question 1.2 How would you call the AddToEmail method in order to send the events in an email?
            //Answer: I have to use foreach loop to call AddToEmail() since the variable  'eventsforCustomer' may contain more
            //than one element

            foreach (var ev in eventsforCustomer)
            {
                AddToEmail(customer, ev);
            }

            Console.WriteLine("Improved AddToEmail is called");
            foreach (var ev in eventsforCustomer)
            {
                AddToEmailImproved(customer, ev);
            }

            //Question 1.3	What is the expected output if we only have the client John Smith?
            //Answer: I would write the below lambda expression to filter list of events @ New York which is the city of John Smith 

            var eventsforJhon = events.Where(x => x.City == "New York");
            /* Or the below code is also possible with LINQ to SQL */
            eventsforJhon = from j_events in events
                            where j_events.City.ToLower() == "New York".ToLower()
                            select j_events;
            //The output will be as follows
            //The code to display the output is as follows
            Console.WriteLine("List of Events for Jhon");
            foreach (var ev in eventsforJhon)
            {
                Console.WriteLine("Event Name {0} Event City {1}", ev.Name, ev.City);
            }

            /*
              Event Name = Phantom of the Opera Event City = New York
              Event Name = Metallica Event City  = New York
              Event Name = LadyGaGa Event City = New York                               
             */

            //How to send five closest distance to client to an email?
            //Answer: the below code does that
            List<Event> lstEvents = GetFiveClosestEvents(customer, events);
            Console.WriteLine("Five Closest events for customer {0} ", customer.Name);
            foreach (Event e in lstEvents)
            {
                AddToEmail(customer, e);
                Console.WriteLine("Event Name= {0} Event City = {1} ", e.Name, e.City);
            }

            //Question 2.3: What is the expected output if we only have the client John Smith?
            //Answer: use the below code
            List<Event> lstJhonEvents = GetFiveClosestEvents(customer, events);
            foreach (Event e in lstJhonEvents)
            {
                Console.WriteLine(e);
            }

            //Question 2.4.	Do you believe there is a way to improve the code you first wrote?
            //Answer: may be  checking variables if they are not null to avoid exceptions the improved code is written in Question 3

            #endregion

            var query = from result in customer
                        where result.Contains("New York")
                        select result;
            // 1. TASK
            foreach (var item in events)
            {
                AddToEmail(query, item);
            }
            /*
            *	We want you to send an email to this customer with all events in their city
            *	Just call AddToEmail(customer, event) for each event you think they should get
            */
        }

        #region myCode Biruk Abel

        //Question 1: Write AddToEmail method 
        public static void AddToEmail(Customer customer, Event live_evenet)
        {
            if (dicCustomerEvents.ContainsKey(customer))//Customer already contained in dictionary
            {
                if (!dicCustomerEvents[customer].Contains(live_evenet) && customer.City == live_evenet.City)
                {
                    dicCustomerEvents[customer].Add(live_evenet);
                }
            }
            else
            {
                if (customer.City == live_evenet.City)
                {
                    dicCustomerEvents.Add(customer, new List<Event>() { live_evenet });
                }
            }

            foreach (Customer c in dicCustomerEvents.Keys)
            {
                Console.WriteLine("Customer Name {0} City {1}", c.Name, c.City);
                foreach (List<Event> lsEvents in dicCustomerEvents.Values)
                {
                    if (dicCustomerEvents[c] == lsEvents)
                    {
                        foreach (Event ev in lsEvents)
                        {
                            Console.WriteLine("Customer={0} will attend Event Name ={1} Event City = {2}", c.Name, ev.Name, ev.City);
                        }
                    }
                }
            }
        }


        //Question 1.4: Do you believe there is a way to improve the code you first wrote?
        //Answer: Yeah there is, I would use an inner for loop to get all List of events to the specific key (customer)
        //and avoid rechecking the List belongs to the key (customer) plus the code becomes shorter.
        //code is written below

        public static void AddToEmailImproved(Customer customer, Event live_evenet)
        {
            if (dicCustomerEvents.ContainsKey(customer))//Customer already contained in dictionary
            {
                if (!dicCustomerEvents[customer].Contains(live_evenet) && customer.City == live_evenet.City)
                {
                    dicCustomerEvents[customer].Add(live_evenet);
                }
            }
            else
            {
                if (customer.City == live_evenet.City)
                {
                    dicCustomerEvents.Add(customer, new List<Event>() { live_evenet });
                }
            }

            foreach (Customer c in dicCustomerEvents.Keys)
            {
                Console.WriteLine("Customer Name {0} City {1}", c.Name, c.City);
                for (int i = 0; i < dicCustomerEvents[c].Count; i++)
                {
                    Event ev = dicCustomerEvents[c][i];
                    Console.WriteLine("Customer={0} will attend Event Name ={1} Event City = {2}", c.Name, ev.Name, ev.City);
                }
            }
        }

        //Question 2: Write method GetDistance(fromCity,toCity)
        //Answer: since we don't have the actual distance between the two cities we have to calculate string distance between
        //the two cities as follows


        public int GetDistance(string fromCity, string toCity)
        {
            int distance = 0;
            int maxLength = Math.Max(fromCity.Length, toCity.Length);
            int minLength = Math.Min(fromCity.Length, toCity.Length);
            int i = 0;

            for (; i < minLength; i++)//for to add up difference between characters
            {
                distance += Math.Abs(fromCity[i] - toCity[i]);
            }

            for (int j = i; j < maxLength; j++)//for the remaining characters get all ASCII values of characters in string array
            {
                distance += fromCity.Length > toCity.Length ? Convert.ToInt32(fromCity[j]) : Convert.ToInt32(toCity[j]);
            }
            return distance;
        }

        //Question 2.1: What should be your approach to getting the distance between the customer’s city and the other cities on the list?
        //Answer: use character distance in each string and keep on adding this distance to get the final distance

        //Question 2.2: How would you get the 5 closest events and how would you send them to the client in an email?
        //Answer: first store the events in a dictionary<Event,int> event being key and distance will be value and then sort the
        //dictionary via values in ascending and store the top five events in a list and return the new list.

        public static List<Event> GetFiveClosestEvents(Customer customer, List<Event> lstEvent)
        {
            Dictionary<Event, int> dicFiveClosestEvents = new();
            List<Event> fiveNearestevents = new List<Event>();
            foreach (Event ev in lstEvent)
            {
                if (!dicFiveClosestEvents.ContainsKey(ev))
                {
                    int dis = GetDistance(customer.City, ev.City);
                    dicFiveClosestEvents.Add(ev, dis);
                }
            }
            var fiveEvents = from x in dicFiveClosestEvents
                             orderby x.Value
                             select x;
            Console.WriteLine("Events sorted in ascending order");
            foreach (var eve in fiveEvents)
            {
                Console.WriteLine("Nearest Event Name = {0} and Event distance = {1}", eve.Key, eve.Value);
            }
            int index = 0;
            foreach (var y in fiveEvents)
            {
                if (index < 5)
                {
                    fiveNearestevents.Add(y.Key);
                    index++;
                }
                if (index == 5) break;
            }
            Console.WriteLine("Five nearest events sorted in ascending order");
            foreach (var r in fiveNearestevents)
            {
                Console.WriteLine("Nearest Event Name = {0} and Event City = {1}", r.Name, r.City);
            }
            return fiveNearestevents;
        }

        //Question 3 If the GetDistance method is an API call which could fail or is too expensive, how will u
        //improve the code written in 2? Write the code.
        //Check if inputs and intermediate results are not null

        public static List<Event> GetFiveClosestEventsImproved(Customer customer, List<Event> lstEvent)
        {
            Dictionary<Event, int> dicFiveClosestEvents = new();
            List<Event> fiveNearestevents = new List<Event>();
            foreach (Event ev in lstEvent)
            {
                if (!dicFiveClosestEvents.ContainsKey(ev))
                {
                    int dis = GetDistance(customer.City, ev.City);
                    dicFiveClosestEvents.Add(ev, dis);
                }
            }
            if (dicFiveClosestEvents.Count > 0)//Newly added code to check Dictionary has values
            {
                var fiveEvents = from x in dicFiveClosestEvents
                                 orderby x.Value
                                 select x;
                Console.WriteLine("Events sorted in ascending order");
                foreach (var eve in fiveEvents)
                {
                    Console.WriteLine("Nearest Event Name = {0} and Event distance = {1}", eve.Key, eve.Value);
                }
                int index = 0;
                foreach (var y in fiveEvents)
                {
                    if (index < 5)
                    {
                        fiveNearestevents.Add(y.Key);
                        index++;
                    }
                    if (index == 5) break;
                }
                Console.WriteLine("Five nearest events sorted in ascending order");
                foreach (var r in fiveNearestevents)
                {
                    Console.WriteLine("Nearest Event Name = {0} and Event City = {1}", r.Name, r.City);
                }
            }
            return fiveNearestevents;
        }

        //Question 4: If the GetDistance method can fail, we don't want the process to fail. What can be done? Code it
        //Answer: add try catch block to avoid method failure

        public static List<Event> GetFiveClosestEventsFailureCheck(Customer customer, List<Event> lstEvent)
        {
            Dictionary<Event, int> dicFiveClosestEvents = new();
            List<Event> fiveNearestevents = new List<Event>();

            try
            {
                foreach (Event ev in lstEvent)
                {
                    if (!dicFiveClosestEvents.ContainsKey(ev))
                    {
                        int dis = GetDistance(customer.City, ev.City);
                        dicFiveClosestEvents.Add(ev, dis);
                    }
                }
                if (dicFiveClosestEvents.Count > 0)//Newly added code to check Dictionary has values
                {
                    var fiveEvents = from x in dicFiveClosestEvents
                                     orderby x.Value
                                     select x;
                    Console.WriteLine("Events sorted in ascending order");
                    foreach (var eve in fiveEvents)
                    {
                        Console.WriteLine("Nearest Event Name = {0} and Event distance = {1}", eve.Key, eve.Value);
                    }
                    int index = 0;
                    foreach (var y in fiveEvents)
                    {
                        if (index < 5)
                        {
                            fiveNearestevents.Add(y.Key);
                            index++;
                        }
                        if (index == 5) break;
                    }
                    Console.WriteLine("Five nearest events sorted in ascending order");
                    foreach (var r in fiveNearestevents)
                    {
                        Console.WriteLine("Nearest Event Name = {0} and Event City = {1}", r.Name, r.City);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return fiveNearestevents;
        }

        //Question 5: If we also want to sort the resulting events by other fields like price, etc.
        //to determine which ones to send to the customer, how would you implement it? Code it
        //Answer: and one more property Price to each Event and pass it in dictionary. code is implemented below
        public static List<Event> GetFiveClosestEventsWithPrice(Customer customer, Dictionary<Event, decimal> dicEventprice)
        {
            Dictionary<Event, int> dicFiveClosestEvents = new();
            List<Event> fiveNearestevents = new List<Event>();
            foreach (Event ev in dicEventprice.Keys)
            {
                if (!dicFiveClosestEvents.ContainsKey(ev))
                {
                    int dis = GetDistance(customer.City, ev.City);
                    dicFiveClosestEvents.Add(ev, dis);
                }
            }
            var fiveEvents = from x in dicFiveClosestEvents
                             orderby x.Value
                             select x;

            foreach (var eve in fiveEvents)
            {
                Console.WriteLine("Nearest Event Name = {0} Event distance = {1}", eve.Key, eve.Value);
            }
            int index = 0;


            Dictionary<Event, decimal> dicEP = new();
            foreach (var ep in fiveEvents)
            {
                if (dicEventprice.ContainsKey(ep.Key))
                {
                    if (!dicEP.ContainsKey(ep.Key))
                    {
                        dicEP.Add(ep.Key, dicEventprice[ep.Key]);
                    }
                }
            }

            var fiveChepestEvents = from x in dicEP
                                    orderby x.Value
                                    select x;

            foreach (var y in fiveChepestEvents)
            {
                if (index < 5)
                {
                    fiveNearestevents.Add(y.Key);
                    index++;
                }
                if (index == 5) break;
            }
            foreach (var r in fiveNearestevents)
            {
                Console.WriteLine("Nearest and Cheapest Event Name = {0} Event City = {1}", r.Name, r.City);
            }
            return fiveNearestevents;
        }

        #endregion

        // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
        static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }
        static int GetDistance(string fromCity, string toCity)
        {
            return AlphebiticalDistance(fromCity, toCity);
        }
        private static int AlphebiticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i < Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
}
/*
var customers = new List<Customer>{
new Customer{ Name = "Nathan", City = "New York"},
new Customer{ Name = "Bob", City = "Boston"},
new Customer{ Name = "Cindy", City = "Chicago"},
new Customer{ Name = "Lisa", City = "Los Angeles"}
};
*/
