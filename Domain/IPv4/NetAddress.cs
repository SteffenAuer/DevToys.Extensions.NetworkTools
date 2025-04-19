namespace Domain.IPv4;

public class NetAddress(uint address) : IPv4Address(address)
{
    public NetAddress() : this(0)
    {
    }

    public NetAddress(InterfaceAddress interfaceAddress, NetMask netMask) : this(interfaceAddress.Address &
        netMask.Address)
    {
    }

    public NetAddress(InterfaceAddress interfaceAddress, BroadcastAddress broadcastAddress) : this(interfaceAddress,
        new NetMask(interfaceAddress, broadcastAddress))
    {
    }
}