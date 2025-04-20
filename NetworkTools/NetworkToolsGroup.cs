using System.ComponentModel.Composition;
using DevToys.Api;

namespace NetworkTools;

[Export(typeof(GuiToolGroup))]
[Name("Network Tools")]
[Order(After = PredefinedCommonToolGroupNames.Converters)]
internal class NetworkToolsGroup : GuiToolGroup
{
    [ImportingConstructor]
    internal NetworkToolsGroup()
    {
        IconFontName = "FluentSystemIcons";
        IconGlyph = '\ueeec';
        DisplayTitle = NetworkToolsGroupStrings.GroupDisplayTitle;
        AccessibleName = NetworkToolsGroupStrings.GroupAccessiblename;
    }
}