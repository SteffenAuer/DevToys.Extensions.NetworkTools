namespace Domain.IPv4;

public class NetMask(uint address) : IPv4Address(address)
{
    public NetMask() : this(0)
    {
    }

    public NetMask(IPv4Address address) : this(address.Address)
    {
    }

    public NetMask(InterfaceAddress interfaceAddress, NetAddress netAddress) :
        this(0)
    {
        // Go bit by bit from left to right. The first bit where the interface address is different from the net address
        // marks the end of the netmask.
        for (var i = 31; i >= 0; i--)
        {
            var mask = 1u << i;
            var interfaceMasked = interfaceAddress.Address & mask;
            var netMasked = netAddress.Address & mask;

            if (interfaceMasked != netMasked) break;
            Address |= mask;
        }
    }

    public NetMask(InterfaceAddress interfaceAddress, BroadcastAddress broadcastAddress) :
        this(0)
    {
        // Go bit by bit from right to left. The first bit that is different between the interface address
        // and the broadcast address is the end of the inverted netmask.
        for (var i = 0; i < 32; i++)
        {
            var mask = 1u << i;
            var interfaceMasked = interfaceAddress.Address & mask;
            var netMasked = broadcastAddress.Address & mask;

            if (interfaceMasked == 0 && netMasked == 0) break;
            Address |= mask;
        }

        Address = ~Address;
    }

    public int PrefixLength => _calculatePrefixLength();

    public long AddressCount => 1L << (32 - PrefixLength);

    private int _calculatePrefixLength()
    {
        var count = 0;
        for (var i = 31; i >= 0; i--)
        {
            var mask = 1u << i;
            var masked = Address & mask;
            if (masked == 0) break;
            count++;
        }

        return count;
    }

    public static NetMask FromAddressCount(int addressCount)
    {
        var numberOfOnes = (int)Math.Ceiling(Math.Log2(addressCount));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(numberOfOnes, 32);
        ArgumentOutOfRangeException.ThrowIfNegative(numberOfOnes);

        return new NetMask(fillWithOnes(32 - numberOfOnes));
    }
}