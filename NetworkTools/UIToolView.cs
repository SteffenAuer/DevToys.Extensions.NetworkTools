using DevToys.Api;
using System.ComponentModel.Composition;
using static DevToys.Api.GUI;

namespace NetworkTools;

[Export(typeof(IGuiTool))]
[Name("NetworkTools")]
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons",
    IconGlyph = '\uE670',
    GroupName = PredefinedCommonToolGroupNames.Converters,
    ResourceManagerAssemblyIdentifier = nameof(MyNetworkToolsResourceAssemblyIdentifier),
    ResourceManagerBaseName = "NetworkTools.NetworkTools",
    ShortDisplayTitleResourceName = nameof(NetworkTools.ShortDisplayTitle),
    LongDisplayTitleResourceName = nameof(NetworkTools.LongDisplayTitle),
    DescriptionResourceName = nameof(NetworkTools.Description),
    AccessibleNameResourceName = nameof(NetworkTools.AccessibleName))]
internal sealed class NetworkToolsGui : IGuiTool
{
    public UIToolView View => new(Label().Style(UILabelStyle.BodyStrong).Text(NetworkTools.HelloWorldLabel));

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        throw new NotImplementedException();
    }
}