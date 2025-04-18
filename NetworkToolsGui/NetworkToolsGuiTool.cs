using System.ComponentModel.Composition;
using DevToys.Api;
using Utils;
using static DevToys.Api.GUI;

namespace NetworkTools;

[Export(typeof(IGuiTool))]
[Name("NetworkTools")]
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons",
    IconGlyph = '\uE670',
    GroupName = PredefinedCommonToolGroupNames.Converters,
    ResourceManagerAssemblyIdentifier = nameof(MyNetworkToolsResourceAssemblyIdentifier),
    ResourceManagerBaseName = "NetworkTools.NetworkToolsGui",
    ShortDisplayTitleResourceName = nameof(NetworkTools.ShortDisplayTitle),
    LongDisplayTitleResourceName = nameof(NetworkTools.LongDisplayTitle),
    DescriptionResourceName = nameof(NetworkTools.Description),
    AccessibleNameResourceName = nameof(NetworkTools.AccessibleName))]
internal sealed class NetworkToolsGuiTool : IGuiTool
{
    private readonly int[] _addressCounts = new int[32];
    private readonly InterfaceAddress _interfaceAddress = new();

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

    public NetworkToolsGuiTool()
    {
        for (var i = 0; i < 32; i++)
        {
            var mask = new NetMask().fillWithOnes(i + 1);
            _subnetOptions[31 - i] = mask;
            _addressCounts[i] = mask.AddressCount;
        }

        _selectedNetMask = _subnetOptions[0];
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
                            .LargeSpacing()
                            .WithChildren(
                                Stack()
                                    .Vertical()
                                    .WithChildren(
                                        SelectDropDownList()
                                            .Title("Subnet")
                                            .WithItems(
                                                _subnetOptions.Select(mask =>
                                                        Item($"{mask} / {mask.PrefixLength}", mask))
                                                    .ToArray()
                                            ).OnItemSelected(item =>
                                            {
                                                if (item?.Value != null)
                                                {
                                                    _selectedNetMask = (NetMask)item.Value;
                                                    SettingsChanged();
                                                }
                                            })
                                    ),
                                NewIpInputs("Interface Address", _interfaceAddress)
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
        throw new NotImplementedException();
    }

    private IUIElement NewIpInputs(string label, IIPAddress ipAddress)
    {
        var inputs = new IUIElement[5];
        inputs[0] = Label().Text(label);
        for (var i = 1; i < 5; i++)
        {
            var byteIdx = 4 - i;
            inputs[i] = NumberInput().HideCommandBar().Step(1).Minimum(byte.MinValue)
                .Maximum(byte.MaxValue)
                .Value(ipAddress.GetByte(byteIdx))
                .OnValueChanged(val =>
                {
                    ipAddress.SetByte(byteIdx, (byte)val);
                    SettingsChanged();
                });
        }

        return Stack().Horizontal().SmallSpacing().WithChildren(inputs);
    }

    private void SettingsChanged()
    {
        var broadcast = new BroadcastAddress(_interfaceAddress, _selectedNetMask);
        var netAddr = new NetAddress(_interfaceAddress, _selectedNetMask);
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