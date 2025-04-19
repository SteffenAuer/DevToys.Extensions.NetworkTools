using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using DevToys.Api;
using Domain.IPv4;

namespace NetworkTools;

[Export(typeof(IDataTypeDetector))]
[DataTypeName(IpAddressDataTypeName, PredefinedCommonDataTypeNames.Text)]
internal sealed class IPv4DataTypeDetector : IDataTypeDetector
{
    internal const string IpAddressDataTypeName = "IPv4 Address";
    private readonly Regex ipv4Regex = new(@"^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4}$");

    public ValueTask<DataDetectionResult> TryDetectDataAsync(object rawData,
        DataDetectionResult? resultFromBaseDetector,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(resultFromBaseDetector?.Data);
        if (resultFromBaseDetector is not null && resultFromBaseDetector.Data is string dataString)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // Assume CIDR notation
            if (dataString.Split("/") is not [var ipAddress, var prefixLength])
            {
                Console.WriteLine("Invalid CIDR notation");
                return ValueTask.FromResult(DataDetectionResult.Unsuccessful);
            }

            Console.WriteLine($"Address: {ipAddress}");

            try
            {
                var ipAddr = IPv4Address.Parse<InterfaceAddress>(ipAddress);
                var netMask = NetMask.FromPrefixLength(16);
                if (int.TryParse(prefixLength, out var prefix)) netMask = NetMask.FromPrefixLength(prefix);

                return ValueTask.FromResult(new DataDetectionResult(true, (netMask, ipAddr)));
            }
            catch
            {
                return ValueTask.FromResult(DataDetectionResult.Unsuccessful);
            }
        }

        // We did not detect an IPv4 address
        return ValueTask.FromResult(DataDetectionResult.Unsuccessful);
    }
}