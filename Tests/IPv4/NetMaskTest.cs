using Domain.IPv4;

namespace Tests.IPv4;

[TestFixture]
[TestOf(typeof(NetMask))]
public class NetMaskTest
{
    [Test]
    [TestCase("17.8.7.8", "17.8.0.0", "255.255.0.0")]
    [TestCase("17.7.177.4", "17.7.160.0", "255.255.224.0")]
    [TestCase("144.3.133.1", "144.3.128.0", "255.255.192.0")]
    [TestCase("255.255.255.255", "255.255.255.254", "255.255.255.254")]
    public void TestNetMask(string interfaceAddr, string netAddress, string expected)
    {
        var netMask = new NetMask(IPv4Address.Parse<InterfaceAddress>(interfaceAddr),
            IPv4Address.Parse<NetworkAddress>(netAddress));
        Assert.That(netMask.ToString(), Is.EqualTo(expected));
    }

    [Test]
    [TestCase("17.8.7.8", "17.8.255.255", "255.255.0.0")]
    [TestCase("11.7.177.4", "11.7.191.255", "255.255.224.0")]
    [TestCase("144.3.133.1", "144.3.191.255", "255.255.192.0")]
    [TestCase("31.4.2.166", "31.4.2.167 ", "255.255.255.248")]
    public void TestNetMaskFromIpAndBroadcast(string interfaceAddr, string broadcastAddr, string expected)
    {
        var iAddr = IPv4Address.Parse<InterfaceAddress>(interfaceAddr);
        var bAddr = IPv4Address.Parse<BroadcastAddress>(broadcastAddr);
        Console.WriteLine(iAddr.ToBinaryString());
        Console.WriteLine(bAddr.ToBinaryString());
        var netMask = new NetMask(iAddr, bAddr);
        Assert.That(netMask.ToString(), Is.EqualTo(expected));

        // var masks = NetMask.PossibleNetMasks(IPv4Address.Parse<InterfaceAddress>(interfaceAddr),
        //     IPv4Address.Parse<BroadcastAddress>(broadcastAddr));
        //
        // Assert.That(masks.Where(x => x.ToString() == expected), Is.True);
    }

    [Test]
    public void Test()
    {
        Assert.That(new NetMask(IPv4Address.fillWithOnes(32)).ToString(), Is.EqualTo("255.255.255.255"));
    }

    [Test]
    [TestCase(65_536, "255.255.0.0")]
    [TestCase(65_530, "255.255.0.0")]
    [TestCase(8_192, "255.255.224.0")]
    [TestCase(16_384, "255.255.192.0")]
    [TestCase(8, "255.255.255.248")]
    public void TestNetMaskFromAddrCount(int addrCount, string expected)
    {
        var netMask = NetMask.FromAddressCount(addrCount);
        Assert.That(netMask.ToString(), Is.EqualTo(expected));
    }

    [Test]
    [TestCase("255.255.0.0", 16)]
    public void TestPrefixLength(string netMask, int expectedPrefixLength)
    {
        var netMaskObj = IPv4Address.Parse<NetMask>(netMask);
        Assert.That(netMaskObj.PrefixLength, Is.EqualTo(expectedPrefixLength));
    }
}