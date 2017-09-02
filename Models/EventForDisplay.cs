using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventCalendar.Core.Models;
using EventCalendarBelle.Controller;
using Lucene.Net.Util;

namespace v3.climbingfish.be.Models
{
    public class EventViewModel
    {

        public static List<EventForDisplay> GetAllUpcomingEvents()
        {
            var eventApiController = new EventApiController();
            var eventList = eventApiController.GetAll();

            var locationList = GetAllEventLocations();

            var list = eventList.Where(p => p.Start != null && (DateTime)p.Start > DateTime.Now.AddDays(-1));
            return EventsToList(list, locationList);
        }

        private static List<EventForDisplay> EventsToList(IEnumerable<Event> list, List<EventLocation> locationList)
        {
            return list.Select(eEvent => new EventForDisplay
            {
                Date = eEvent.Start ?? DateTime.Now.AddDays(-1),
                Title = eEvent.Title,
                Id = eEvent.Id,
                Location = GetLocationById(eEvent.locationId, locationList).City
            }).OrderBy(p => p.Date).ToList();
        }

        public static List<EventForDisplay> GetFirstAmountUpcomingEvents(int amount)
        {
            var eventApiController = new EventApiController();
            var eventList = eventApiController.GetAll();

            var locationList = GetAllEventLocations();

            var list = eventList.Where(p => p.Start != null && (DateTime)p.Start > DateTime.Now.AddDays(-1)).Take(amount);

            return EventsToList(list, locationList);
        }


        internal static EventLocation GetLocationById(int locationId, List<EventLocation> locations)
        {
            return locations.FirstOrDefault(l => l.Id == locationId);
        }

        public static List<EventLocation> GetAllEventLocations()
        {
            var locationApiController = new LocationApiController();
            return locationApiController.GetAll().ToList();
        } 
    }

    public class EventForDisplay
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
    }

   
}