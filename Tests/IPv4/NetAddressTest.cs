using Domain.IPv4;

namespace Tests.IPv4;

[TestFixture]
[TestOf(typeof(NetAddress))]
public class NetAddressTest
{
    [Test]
    public void TestNetAddress()
    {
        var interfaceAddr = InterfaceAddress.Parse("17.8.7.8");
        var netMask = new NetMask(0xff_ff_00_00);
        Assert.That(new NetAddress(interfaceAddr, netMask).ToString(), Is.EqualTo("17.8.0.0"));
    }
}