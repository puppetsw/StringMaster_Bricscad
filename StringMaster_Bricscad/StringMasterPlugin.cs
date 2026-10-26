using StringMaster_Bricscad;
using StringMaster.Common.Services.Implementation;
using Teigha.Runtime;

[assembly: ExtensionApplication(typeof(StringMasterPlugin))]
namespace StringMaster_Bricscad;

public sealed class StringMasterPlugin : IExtensionApplication
{
    public void Initialize()
    {
        AcadApplicationService.Editor.WriteMessage("\n===========================");
        AcadApplicationService.Editor.WriteMessage("\nStringMaster Loaded (CIVIL)");
        AcadApplicationService.Editor.WriteMessage("\n===========================");
    }

    public void Terminate()
    {
    }
}
