namespace Domain.IPv4;

public class BroadcastAddress(uint address) : IPv4Address(address)
{
    public BroadcastAddress() : this(0xff_ff_ff_ff)
    {
    }

    public BroadcastAddress(InterfaceAddress interfaceAddress, NetMask netMask) : this(interfaceAddress.Address |
        ~netMask.Address)
    {
    }

    public BroadcastAddress(InterfaceAddress interfaceAddress, NetworkAddress networkAddress) : this(interfaceAddress,
        new NetMask(interfaceAddress, networkAddress))
    {
    }
}