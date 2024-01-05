using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Extensions;
using NetworkUtility.DNS;
using NetworkUtility.Ping;
using System.Net.NetworkInformation;
using Xunit;

// Документация https://fluentassertions.com/

namespace NetworkUtility.Test.PingTest
{
    public class NetworkServiceTest
    {
        private readonly NetworkService _pingService;
        private readonly IDNS _dns;

        public NetworkServiceTest() 
        {
            //Depencies
            _dns = A.Fake<IDNS>();

           _pingService = new NetworkService(_dns);
        }

        [Fact]
        public void NetworkService_SendPing_ReturnString()
        {
            //Arrange
            //var pingService = new NetworkService();
            A.CallTo(() => _dns.SendDNS()).Returns(true);

            //Act
            var result = _pingService.SendPing();

            //Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be("Success: Ping Sent!");
            result.Should().Contain("Success", Exactly.Once());

        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(2, 2, 4)]
        public void NetworkService_PingTimeout_ReturnInt(int a, int b, int expected)
        { 
            // Arrange
            //var pingService = new NetworkService();

            //Act
            var result = _pingService.PingTimeout(a, b);

            //Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void NetworkService_LastPingDate_ReturnDate()
        {
            //Arrange
            //var pingService = new NetworkService();

            //Act
            var result = _pingService.LastPingDate();

            //Assert
            result.Should().BeBefore(1.January(2030));
            result.Should().BeAfter(1.January(2010));

        }

        [Fact]
        public void NetworkService_GetPingOptions_ReturnObject()
        {
            //Arrange
            //var pingService = new NetworkService();
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            //Act
            var result = _pingService.GetPingOptions();

            //Assert
            result.Should().BeOfType<PingOptions>();
            result.Should().BeEquivalentTo(expected);
            result.Ttl.Should().Be(1);
        }

        [Fact]
        public void NetworkService_MostRecentPings_ReturnObject()
        {
            //Arrange
            //var pingService = new NetworkService();
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            //Act
            var result = _pingService.MostRecentPings();

            //Assert
            result.Should().ContainEquivalentOf(expected);
            result.Should().Contain(x => x.DontFragment == true);
        }
    }
}
