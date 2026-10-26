using Microsoft.Win32;
using StringMaster.UI.Services.Interfaces;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace StringMaster.Common.Services.Implementation;

public class OpenDialogService : IOpenDialogService
{
    public bool? ShowDialog()
    {
        var dialog = new OpenFileDialog
        {
            DefaultExt = DefaultExt,
            Filter = Filter
        };

        var result = dialog.ShowDialog();

        if (result == true)
            FileName = dialog.FileName;

        return result;
    }

    public string FileName { get; set; }
    public string DefaultExt { get; set; }
    public string Filter { get; set; }
}
