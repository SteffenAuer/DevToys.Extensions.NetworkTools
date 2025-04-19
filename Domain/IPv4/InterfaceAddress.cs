namespace Domain.IPv4;

public class InterfaceAddress(uint address) : IPv4Address<InterfaceAddress>(address)
{
    public InterfaceAddress() : this(0)
    {
    }
}