namespace Domain.IPv4;

public class InterfaceAddress(uint address) : IPv4Address(address)
{
    public InterfaceAddress() : this(0)
    {
    }

    public InterfaceAddress(IPv4Address address) : this(address.Address)
    {
    }
}