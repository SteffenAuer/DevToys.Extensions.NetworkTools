namespace Domain.IPv4;

public class NetworkAddress(uint address) : IPv4Address(address)
{
    public NetworkAddress() : this(0)
    {
    }

    public NetworkAddress(InterfaceAddress interfaceAddress, NetMask netMask) : this(interfaceAddress.Address &
        netMask.Address)
    {
    }

    public NetworkAddress(InterfaceAddress interfaceAddress, BroadcastAddress broadcastAddress) : this(interfaceAddress,
        new NetMask(interfaceAddress, broadcastAddress))
    {
    }
}