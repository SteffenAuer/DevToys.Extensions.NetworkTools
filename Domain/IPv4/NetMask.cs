namespace Domain.IPv4;

public class NetMask(uint address) : IPv4Address(address)
{
    public NetMask() : this(0)
    {
    }

    public NetMask(IPv4Address address) : this(address.Address)
    {
    }

    public NetMask(InterfaceAddress interfaceAddress, NetworkAddress networkAddress) :
        this(0)
    {
        // Go bit by bit from left to right. The first bit where the interface address is different from the net address
        // marks the end of the netmask.
        for (var i = 31; i >= 0; i--)
        {
            var mask = 1u << i;
            var interfaceMasked = interfaceAddress.Address & mask;
            var netMasked = networkAddress.Address & mask;

            if (interfaceMasked != netMasked) break;
            Address |= mask;
        }
    }

    public NetMask(InterfaceAddress interfaceAddress, BroadcastAddress broadcastAddress) :
        this(0)
    {
        // Go bit by bit from left to right. The first bit where the interface address is different from the broadcast address
        // marks the end of the netmask.
        for (var i = 31; i >= 0; i--)
        {
            var mask = 1u << i;
            var interfaceMasked = interfaceAddress.Address & mask;
            var broadcastMasked = broadcastAddress.Address & mask;

            if (interfaceMasked != broadcastMasked) break;
            Address |= mask;
        }
    }

    public int PrefixLength => _calculatePrefixLength();

    public long AddressCount => 1L << (32 - PrefixLength); // equiv to 2^(32 - PrefixLength)

    public static NetMask FromPrefixLength(int prefix)
    {
        return new NetMask(fillWithOnes(prefix));
    }

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