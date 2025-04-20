using System.ComponentModel.Composition;
using DevToys.Api;
using Domain.IPv4;

namespace NetworkTools.IpCalcTool.GUI;

[Export(typeof(IDataTypeDetector))]
[DataTypeName(IpAddressDataTypeName, PredefinedCommonDataTypeNames.Text)]
internal sealed class IPv4DataTypeDetector : IDataTypeDetector
{
    internal const string IpAddressDataTypeName = "IPv4 Address";

    public ValueTask<DataDetectionResult> TryDetectDataAsync(object rawData,
        DataDetectionResult? resultFromBaseDetector,
        CancellationToken cancellationToken)
    {
        if (resultFromBaseDetector?.Data is not string dataString)
            return ValueTask.FromResult(DataDetectionResult.Unsuccessful);

        cancellationToken.ThrowIfCancellationRequested();
        // Assume CIDR notation
        if (dataString.Split("/") is not [var ipAddress, var prefixLength])
            return ValueTask.FromResult(DataDetectionResult.Unsuccessful);

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
}