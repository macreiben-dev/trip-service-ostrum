using NFluent;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TripServiceKata.Exception;
using TripServiceKata.Trip;
using UserInParam = TripServiceKata.User.User;

namespace TripServiceKata.Tests
{
    [TestFixture]
    public class TripServiceTest
    {
        private ITripDao _tripDao;
        private List<Trip.Trip> _tripList;

        [SetUp]
        public void SetUp()
        {
            _tripDao = Substitute.For<ITripDao>();

            _tripList = new List<Trip.Trip>()
            {
                new Trip.Trip()
            };
        }


        [Test]
        public void Should_fail_when_loggedUser_isNull()
        {
            TripService target = new TestableTripService(_tripDao);

            Check.ThatCode(() => target.GetTripsByUser(new UserInParam()))
                .Throws<UserNotLoggedInException>();
        }

        [Test]
        public void Should_return_emptyTripList_when_loggedUser_notFriend_with_givenUser()
        {
            TripService target = new TestableTripServiceWithNotNullLoggedUser(_tripDao);

            var actual = target.GetTripsByUser(new UserInParam());

            Check.That(actual).IsEmpty();
        }

        [Test]
        public void Should_nullRefException_when_user_is_null()
        {
            TripService target = new TestableTripServiceWithNotNullLoggedUser(_tripDao);

            Check.ThatCode(() => target.GetTripsByUser(null))
                .Throws<NullReferenceException>();
        }

        [Test]
        public void Should_return_emptyTripList_when_givenUser_hasFriend_but_not_loggedUser()
        {
            // ARRANGE
            TripService target = new TestableTripServiceWithNotNullLoggedUser(_tripDao);

            UserInParam user = new UserInParam();

            user.AddFriend(new UserInParam());

            // ACT
            var actual = target.GetTripsByUser(user);

            // ASSERT
            Check.That(actual).IsEmpty();
        }

        [Test]
        public void Should_return_givenUserTripList_when_givenUser_isFriend_with_loggedUser()
        {
            // ARRANGE
            TestableTripServiceWithNotNullLoggedUser target = new TestableTripServiceWithNotNullLoggedUser(_tripDao);

            UserInParam user = new UserInParam();

            user.AddFriend(target.CurrentLoggedUser);

            _tripDao.FindTripsByUser(user)
                .Returns(_tripList);

            // ACT
            var actual = target.GetTripsByUser(user);

            // ASSERT
            Check.That(actual).IsEqualTo(_tripList);
        }
    }
}
