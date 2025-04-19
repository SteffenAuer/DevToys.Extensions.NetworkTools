using Domain.IPv4;

namespace Tests.IPv4;

[TestFixture]
[TestOf(typeof(IPv4Address))]
public class IPv4AddressTest
{
    [Test]
    public void TestByteResolution()
    {
        var addr = new InterfaceAddress(0x11223344u);

        Assert.Multiple(() =>
        {
            Assert.That(addr.GetByte(0), Is.EqualTo(0x44));
            Assert.That(addr.GetByte(1), Is.EqualTo(0x33));
            Assert.That(addr.GetByte(2), Is.EqualTo(0x22));
            Assert.That(addr.GetByte(3), Is.EqualTo(0x11));
        });
    }

    [Test]
    public void TestRangeThrows()
    {
        var addr = new InterfaceAddress(0x11223344u);
        Assert.Throws<ArgumentOutOfRangeException>(() => addr.GetByte(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => addr.GetByte(4));
    }

    [Test]
    public void TestFilledWithOnes()
    {
        var addr = IPv4Address.fillWithOnes(9);
        Assert.Multiple(() =>
        {
            Assert.That(addr.GetByte(3), Is.EqualTo(0xff));
            Assert.That(addr.GetByte(2), Is.EqualTo(0x80));
            Assert.That(addr.GetByte(1), Is.EqualTo(0));
            Assert.That(addr.GetByte(0), Is.EqualTo(0));
        });
    }

    [Test]
    public void TestFromString()
    {
        Assert.Multiple(() =>
        {
            Assert.That(IPv4Address.Parse<InterfaceAddress>("255.255.255.255").ToString(),
                Is.EqualTo("255.255.255.255"));
            Assert.That(IPv4Address.Parse<InterfaceAddress>("128.0.0.0").ToString(), Is.EqualTo("128.0.0.0"));
        });
    }

    [Test]
    public void TestFromString_Invalid()
    {
        Assert.Multiple(() =>
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => IPv4Address.Parse<InterfaceAddress>("255.255.255"));
            Assert.Throws<OverflowException>(() => IPv4Address.Parse<InterfaceAddress>("256.0.0.0"));
        });
    }
}