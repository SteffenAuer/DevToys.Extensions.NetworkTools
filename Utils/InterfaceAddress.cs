namespace Utils;

public class InterfaceAddress(uint address) : IPv4Address<InterfaceAddress>(address)
{
    public InterfaceAddress() : this(0)
    {
    }
}