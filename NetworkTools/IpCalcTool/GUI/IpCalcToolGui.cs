using System.ComponentModel.Composition;
using DevToys.Api;
using Domain;
using Domain.IPv4;
using static DevToys.Api.GUI;

namespace NetworkTools.IpCalcTool.GUI;

[Export(typeof(IGuiTool))]
[Name("IpCalcTool")]
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons",
    IconGlyph = '\uF562',
    GroupName = "Network Tools",
    ResourceManagerAssemblyIdentifier = nameof(IpCalcToolResourceAssemblyIdentifier),
    ResourceManagerBaseName = "NetworkTools.IpCalcTool.IpCalcToolStrings",
    ShortDisplayTitleResourceName = nameof(IpCalcToolStrings.ShortDisplayTitle),
    LongDisplayTitleResourceName = nameof(IpCalcToolStrings.LongDisplayTitle),
    DescriptionResourceName = nameof(IpCalcToolStrings.Description),
    AccessibleNameResourceName = nameof(IpCalcToolStrings.AccessibleName))]
[AcceptedDataTypeName(IPv4DataTypeDetector.IpAddressDataTypeName)]
internal sealed class IpCalcToolGui : IGuiTool
{
    private readonly InterfaceAddress _interfaceAddress = new();
    private readonly IUISingleLineTextInput[] _interfaceAddressBytes = new IUISingleLineTextInput[4];
    private readonly IUISelectDropDownList _netMaskDropdown;

    private readonly Dictionary<NetMask, IUIDropDownListItem> _netMaskDropdownItems = new();

    private readonly IUISingleLineTextInput _outputBroadcastAddress =
        SingleLineTextInput().ReadOnly().Title("Broadcast Address");

    private readonly IUISingleLineTextInput _outputNetAddress =
        SingleLineTextInput().ReadOnly().Title("Network Address");

    private readonly IUISingleLineTextInput _outputNumberOfAddresses =
        SingleLineTextInput().ReadOnly().Title("Possible Addresses");

    private readonly IUISingleLineTextInput _outputNumberOfHosts =
        SingleLineTextInput().ReadOnly().Title("Number of Hosts");

    private readonly NetMask[] _subnetOptions = new NetMask[32];

    private NetMask _selectedNetMask;

    public IpCalcToolGui()
    {
        for (var i = 0; i < 32; i++) _subnetOptions[31 - i] = new NetMask(IPv4Address.fillWithOnes(i + 1));

        _selectedNetMask = _subnetOptions[0];
        _interfaceAddressBytes = NewIpInputs(_interfaceAddress);
        foreach (var netMask in _subnetOptions)
            _netMaskDropdownItems.Add(netMask, Item($"{netMask} / {netMask.PrefixLength}", netMask));
        _netMaskDropdown = SelectDropDownList()
            .Title("Subnet")
            .WithItems(_netMaskDropdownItems.Values.ToArray()).OnItemSelected(item =>
            {
                if (item?.Value == null) return;
                _selectedNetMask = (NetMask)item.Value;
                SettingsChanged();
            });
    }

    public UIToolView View
        => new(
            true,
            Grid()
                .ColumnLargeSpacing()
                .RowLargeSpacing()
                .Rows(
                    (GridRow.Settings, Auto),
                    (GridRow.Results, new UIGridLength(1, UIGridUnitType.Fraction)))
                .Columns(
                    (GridColumn.Stretch, new UIGridLength(1, UIGridUnitType.Fraction)))
                .Cells(
                    Cell(
                        GridRow.Settings,
                        GridColumn.Stretch,
                        Stack()
                            .Vertical()
                            .MediumSpacing()
                            .WithChildren(_netMaskDropdown,
                                Stack().Horizontal().SmallSpacing().WithChildren([
                                    Label().Text("Interface Address"),
                                    .._interfaceAddressBytes
                                ])
                            )
                    ),
                    Cell(GridRow.Results,
                        GridColumn.Stretch,
                        Stack()
                            .Vertical()
                            .WithChildren(
                                _outputBroadcastAddress,
                                _outputNetAddress,
                                _outputNumberOfAddresses,
                                _outputNumberOfHosts
                            )
                    )
                ));

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        if (dataTypeName != IPv4DataTypeDetector.IpAddressDataTypeName)
            throw new ArgumentException("Unsupported dataType", nameof(dataTypeName));

        if (parsedData is not (NetMask netMask, InterfaceAddress ipAddress))
            throw new ArgumentOutOfRangeException(nameof(parsedData));

        if (_netMaskDropdownItems.ContainsKey(netMask))
            return;

        _selectedNetMask = netMask;
        _interfaceAddress.Address = ipAddress.Address;

        for (var i = 0; i < _interfaceAddressBytes.Length; i++)
            _interfaceAddressBytes[i].Text(ipAddress.GetByte(3 - i).ToString());
        if (_netMaskDropdownItems.TryGetValue(netMask, out var item))
            _netMaskDropdown.Select(item);

        SettingsChanged();
    }

    private IUISingleLineTextInput[] NewIpInputs(IIPAddress ipAddress)
    {
        var inputs = new IUISingleLineTextInput[4];
        for (var i = 0; i < 4; i++)
        {
            var byteIdx = 3 - i;
            inputs[i] = NumberInput().HideCommandBar().Step(1).Minimum(byte.MinValue)
                .Maximum(byte.MaxValue)
                .Value(ipAddress.GetByte(byteIdx))
                .OnValueChanged(val =>
                {
                    ipAddress.SetByte(byteIdx, (byte)val);
                    SettingsChanged();
                });
        }

        return inputs;
    }

    private void SettingsChanged()
    {
        var broadcast = new BroadcastAddress(_interfaceAddress, _selectedNetMask);
        var netAddr = new NetworkAddress(_interfaceAddress, _selectedNetMask);
        _outputBroadcastAddress.Text(broadcast.ToString());
        _outputNetAddress.Text(netAddr.ToString());
        _outputNumberOfAddresses.Text(_selectedNetMask.AddressCount.ToString());
        _outputNumberOfHosts.Text((_selectedNetMask.AddressCount - 2).ToString());
    }

    private enum GridColumn
    {
        Stretch
    }

    private enum GridRow
    {
        Settings,
        Results
    }
}