using StringMaster_Bricscad;
using StringMaster.Common;
using Teigha.Runtime;

[assembly: CommandClass(typeof(Commands))]
namespace StringMaster_Bricscad;

public static class Commands
{
    private static StringMasterPalette s_palette;

    [CommandMethod("WMS", "_StringMaster", CommandFlags.Modal)]
    public static void ShowStringMaster()
    {
        if (s_palette == null)
            s_palette = new StringMasterPalette(new StringCivilPointService(), true);

        s_palette.ShowPalette();
    }
}
