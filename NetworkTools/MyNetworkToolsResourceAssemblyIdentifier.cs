using DevToys.Api;
using System.ComponentModel.Composition;

namespace NetworkTools;

[Export(typeof(IResourceAssemblyIdentifier))]
[Name(nameof(MyNetworkToolsResourceAssemblyIdentifier))]
internal sealed class MyNetworkToolsResourceAssemblyIdentifier : IResourceAssemblyIdentifier
{
    public ValueTask<FontDefinition[]> GetFontDefinitionsAsync()
    {
        throw new NotImplementedException();
    }
}