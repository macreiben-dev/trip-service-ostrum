using System.Collections.Generic;
using System.Linq;
using TripServiceKata.Exception;
using TripServiceKata.User;

namespace TripServiceKata.Trip
{
    public class TripService
    {
        private readonly ITripDao tripDao;

        public TripService(ITripDao tripDao)
        {
            this.tripDao = tripDao;
        }

        public List<Trip> GetTripsByUser(TripServiceKata.User.User user)
        {
            User.User loggedUser = GetLoggedUser() ?? throw new UserNotLoggedInException();

            if (user.GetFriends().Any(c => c.Equals(loggedUser)))
            {
                return tripDao.FindTripsByUser(user);
            }

            return Empty();
        }

        private static List<Trip> Empty()
        {
            return new List<Trip>();
        }

        protected virtual TripServiceKata.User.User GetLoggedUser()
        {
            return UserSession.GetInstance().GetLoggedUser();
        }
    }

    public interface ITripDao
    {
        List<Trip> FindTripsByUser(User.User user);
    }

    public class TripDaoAdapter : ITripDao
    {
        public List<Trip> FindTripsByUser(User.User user)
        {
            return TripDAO.FindTripsByUser(user);
        }
    }
}
