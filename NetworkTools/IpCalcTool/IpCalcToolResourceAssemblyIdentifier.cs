using System.ComponentModel.Composition;
using DevToys.Api;

namespace NetworkTools.IpCalcTool;

[Export(typeof(IResourceAssemblyIdentifier))]
[Name(nameof(IpCalcToolResourceAssemblyIdentifier))]
internal sealed class IpCalcToolResourceAssemblyIdentifier : IResourceAssemblyIdentifier
{
    public ValueTask<FontDefinition[]> GetFontDefinitionsAsync()
    {
        throw new NotImplementedException();
    }
}