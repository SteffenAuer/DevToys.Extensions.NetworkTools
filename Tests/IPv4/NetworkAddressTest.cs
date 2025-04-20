using Domain.IPv4;

namespace Tests.IPv4;

[TestFixture]
[TestOf(typeof(NetworkAddress))]
public class NetworkAddressTest
{
    [Test]
    public void TestNetAddress()
    {
        var interfaceAddr = IPv4Address.Parse<InterfaceAddress>("17.8.7.8");
        var netMask = new NetMask(0xff_ff_00_00);
        Assert.That(new NetworkAddress(interfaceAddr, netMask).ToString(), Is.EqualTo("17.8.0.0"));
    }
}