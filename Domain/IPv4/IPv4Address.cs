namespace Domain.IPv4;

public abstract class IPv4Address<T> : IIPAddress where T : IPv4Address<T>, new()
{
    protected IPv4Address() : this(0)
    {
    }

    protected IPv4Address(uint address)
    {
        Address = address;
    }

    public uint Address { get; protected set; }

    /// <summary>
    ///     Get the specified byte from the <c>IPv4Address</c>
    /// </summary>
    /// <param name="position">The position of the byte, counted from the right.</param>
    /// <returns>The specified byte</returns>
    /// <example>
    ///     Example:
    ///     <code>
    /// (new IPv4Address(0xff000000)).GetByte(3)
    /// </code>
    ///     returns 0xff
    /// </example>
    public byte GetByte(int position)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(position, 3);
        ArgumentOutOfRangeException.ThrowIfNegative(position);

        var shift = 8 * position;
        var byteMask = 255 << shift;

        return (byte)((Address & byteMask) >> shift);
    }

    public void SetByte(int position, byte value)
    {
        ClearByte(position);
        Address |= (uint)value << (8 * position);
    }

    /// <summary>
    ///     Expects a string in the format "x.x.x.x" where x is a number between 0 and 255.
    /// </summary>
    /// <param name="input">The correctly formatted IPv4 address as specified above</param>
    /// <returns>A new IPv4Address instance</returns>
    public static T Parse(string input)
    {
        var byteStrings = input.Split('.');
        ArgumentOutOfRangeException.ThrowIfNotEqual(byteStrings.Length, 4);
        var address = new T();
        for (var i = 0; i < 4; i++) address.SetByte(3 - i, byte.Parse(byteStrings[i]));
        return address;
    }

    /// <summary>
    ///     Creates a new <c>IPv4Address</c> with <paramref name="count" /> 1's filled in from the beginning.
    /// </summary>
    /// <param name="count">The number of 1's to be inserted from the left</param>
    /// <returns>The new IPv4 address</returns>
    public T fillWithOnes(int count)
    {
        Address = (uint)((1L << count) - 1) << (32 - count);
        return (T)this;
    }

    private void ClearByte(int position)
    {
        Address &= ~(255u << (8 * position));
    }

    public override string ToString()
    {
        return $"{GetByte(3)}.{GetByte(2)}.{GetByte(1)}.{GetByte(0)}";
    }

    public string ToBinaryString()
    {
        return
            $"{Convert.ToString(GetByte(3), 2).PadLeft(8, '0')}.{Convert.ToString(GetByte(2), 2).PadLeft(8, '0')}.{Convert.ToString(GetByte(1), 2).PadLeft(8, '0')}.{Convert.ToString(GetByte(0), 2).PadLeft(8, '0')}";
    }
}