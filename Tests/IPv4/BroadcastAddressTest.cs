using Domain.IPv4;

namespace Tests.IPv4;

[TestFixture]
[TestOf(typeof(BroadcastAddress))]
public class BroadcastAddressTest
{
    [Test]
    [TestCase("17.8.7.8", "255.255.0.0", "17.8.255.255")]
    [TestCase("11.7.177.4", "255.255.224.0", "11.7.191.255")]
    [TestCase("144.3.133.1", "255.255.192.0", "144.3.191.255")]
    [TestCase("31.4.2.166", "255.255.255.248", "31.4.2.167")]
    public void TestBroadcastAddressFromIpAndNetMask(string interfaceAddr, string netMask, string expected)
    {
        var broadcastAddr = new BroadcastAddress(IPv4Address.Parse<InterfaceAddress>(interfaceAddr),
            IPv4Address.Parse<NetMask>(netMask));
        Assert.That(broadcastAddr.ToString(), Is.EqualTo(expected));
    }
}