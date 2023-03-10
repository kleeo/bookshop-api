using Bookshop.Data;
using Bookshop.Models;
using Bookshop.Services;
using Bookshop.UnitTests.Helpers;
using EntityFrameworkCore.Testing.NSubstitute;
using NUnit.Framework;

namespace Bookshop.UnitTests.ServicesTests
{
    public class BookingsServiceTests
    {
        private BookingsService _sut;
        private BookshopDbContext _bookshopDbContext;
        private int _userId;
        private int _productId;

        [SetUp]
        public void BeforeTest()
        {
            foreach (var entity in _bookshopDbContext.Bookings)
                _bookshopDbContext.Bookings.Remove(entity);
            _bookshopDbContext.SaveChanges();
        }

        public BookingsServiceTests()
        {
            _bookshopDbContext = Create.MockedDbContextFor<BookshopDbContext>();
            _sut = new BookingsService(_bookshopDbContext);
        }

        [Test]
        public async Task GetAllBookings_ReturnsAllBookings_WhenItsCalled()
        {
            var expectedBookings = new List<Booking>()
            {
                ModelHelper.GetRandomBooking(),
                ModelHelper.GetRandomBooking()
            };
            _bookshopDbContext.Set<Booking>().AddRange(expectedBookings);
            _bookshopDbContext.SaveChanges();

            var bookings = await _sut.GetBookingsAsync();

            Assert.That(bookings.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetBookingById_ReturnsSingleBooking_WhenValidRequestMade()
        {
            var expectedBooking = ModelHelper.GetRandomBooking();
            _bookshopDbContext.Set<Booking>().Add(expectedBooking);
            _bookshopDbContext.SaveChanges();

            var actualBooking = await _sut.GetBookingAsync(expectedBooking.Id);

            Assert.That(actualBooking.Id, Is.EqualTo(expectedBooking.Id));
        }

        [Test]
        public async Task UpdateBooking_UpdatesTheBooking_WhenCorrectModelIsPassed()
        {
            var expectedBooking = ModelHelper.GetRandomBooking();
            _bookshopDbContext.Set<Booking>().Add(expectedBooking);
            _bookshopDbContext.SaveChanges();

            expectedBooking.DeliveryAddress = "Updated address";
            await _sut.UpdateBookingAsync(expectedBooking);

            var actualBooking = await _sut.GetBookingAsync(expectedBooking.Id);
            Assert.That(actualBooking.DeliveryAddress, Is.EqualTo(expectedBooking.DeliveryAddress));
        }

        [Test]
        public async Task CreateBooking_CreatesTheBooking_WhenCorrectModelIsPassed()
        {
            var expectedBooking = ModelHelper.GetRandomBooking();

            var responseBooking = await _sut.CreateBookingAsync(expectedBooking);

            var dbBooking = await _sut.GetBookingAsync(expectedBooking.Id);
            Assert.That(responseBooking, Is.EqualTo(expectedBooking));
            Assert.That(dbBooking, Is.EqualTo(expectedBooking));
        }

        [Test]
        public async Task DeleteBooking_DeleteTheBooking_WhenExistingIdIsProvided()
        {
            var expectedBooking = ModelHelper.GetRandomBooking();
            _bookshopDbContext.Set<Booking>().Add(expectedBooking);
            _bookshopDbContext.SaveChanges();

            await _sut.DeleteBookingAsync(expectedBooking.Id);

            var actualBooking = await _sut.GetBookingAsync(expectedBooking.Id);
            Assert.That(actualBooking, Is.EqualTo(null));
        }

		[Test]
        public async Task FailingTest()
        {
            Assert.Fail("Bad luck, sorry 123123");
        }
	}
}
