namespace Domain;

public interface IIPAddress
{
    public byte GetByte(int position);
    public void SetByte(int position, byte value);
    public string ToBinaryString();
    public string ToString();
}