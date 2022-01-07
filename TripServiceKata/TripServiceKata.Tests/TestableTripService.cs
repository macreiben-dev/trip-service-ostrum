using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TripServiceKata.Trip;

namespace TripServiceKata.Tests
{
    public class TestableTripService : TripService
    {
        public TestableTripService(ITripDao tripDao) : base(tripDao)
        {
        }

        protected override User.User GetLoggedUser()
        {
            return null;
        }
    }

    public class TestableTripServiceWithNotNullLoggedUser : TripService
    {
        private User.User _loggedUser;
        private List<Trip.Trip> _tripList;

        public TestableTripServiceWithNotNullLoggedUser(ITripDao tripDao) : base(tripDao)
        {
            _loggedUser = new User.User();

            _tripList = new List<Trip.Trip>()
            {
                new Trip.Trip()
            };
        }

        public User.User CurrentLoggedUser => _loggedUser;

        public List<Trip.Trip> UserTripList => _tripList;

        protected override User.User GetLoggedUser()
        {
            return _loggedUser;
        }
    }
}
