using System.ComponentModel.Composition;
using DevToys.Api;
using Domain.IPv4;
using Microsoft.Extensions.Logging;
using OneOf;

namespace NetworkTools.IpCalcTool.CLI;

[Export(typeof(ICommandLineTool))]
[Name("IpCalcTool")]
[CommandName(
    Name = "ipcalc",
    ResourceManagerBaseName = "NetworkTools.IpCalcTool.IpCalcToolStrings",
    DescriptionResourceName = nameof(IpCalcToolStrings.Description)
)]
internal sealed class IpCalcToolCli : ICommandLineTool
{
    [CommandLineOption(
        Name = "interface",
        Alias = "ip",
        IsRequired = true,
        DescriptionResourceName = nameof(IpCalcToolStrings.InterfaceAddress)
    )]
    private OneOf<string, uint> _interfaceAddressInput { get; set; }

    [CommandLineOption(
        Name = "prefix",
        Alias = "nm",
        IsRequired = true,
        DescriptionResourceName = nameof(IpCalcToolStrings.PrefixDescription)
    )]
    private byte _netMaskInput { get; set; }

    public async ValueTask<int> InvokeAsync(ILogger logger, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentOutOfRangeException.ThrowIfGreaterThan(_netMaskInput, 16);
        ArgumentOutOfRangeException.ThrowIfNegative(_netMaskInput);

        try
        {
            var interfaceAddress = _interfaceAddressInput.Match(
                IPv4Address.Parse<InterfaceAddress>,
                ip => new InterfaceAddress(ip)
            );
            var netMask = NetMask.FromPrefixLength(_netMaskInput);
            var networkAddress = new NetworkAddress(interfaceAddress, netMask);
            var broadcastAddress = new BroadcastAddress(interfaceAddress, netMask);

            Console.WriteLine($"Network Address: {networkAddress}");
            Console.WriteLine($"Broadcast Address: {broadcastAddress}");
            Console.WriteLine($"Number of Addresses: {netMask.AddressCount.ToString("N0")}");
        }
        catch
        {
            return 1;
        }

        return 0;
    }
}