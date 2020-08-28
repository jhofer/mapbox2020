using Endgame.Backend.Domain;
using Endgame.Backend.DTO;
using Endgame.DTOs;
using NUnit.Framework;

namespace EndgameTest
{
    [TestFixture]
    public class MapperTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var dto = new UserDto();
            dto.email = "asdf";
            var user = Mapper.Map<User, UserDto>(dto);
            Assert.AreEqual(dto.email, user.Email);
        }

        [Test]
        public void TestPostionMapping()
        {
            var dto = new UserDto();
            dto.location = new LocationDto(1, 2);
            var user = Mapper.Map<User, UserDto>(dto);
            Assert.AreEqual(dto.location.longitude, user.Location.Position.Longitude);
        }
    }
}