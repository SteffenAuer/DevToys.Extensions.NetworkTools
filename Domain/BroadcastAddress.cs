namespace Utils;

public class BroadcastAddress(uint address) : IPv4Address<BroadcastAddress>(address)
{
    public BroadcastAddress() : this(0xff_ff_ff_ff)
    {
    }

    public BroadcastAddress(InterfaceAddress interfaceAddress, NetMask netMask) : this(interfaceAddress.Address |
        ~netMask.Address)
    {
    }

    public BroadcastAddress(InterfaceAddress interfaceAddress, NetAddress netAddress) : this(interfaceAddress,
        new NetMask(interfaceAddress, netAddress))
    {
    }
}