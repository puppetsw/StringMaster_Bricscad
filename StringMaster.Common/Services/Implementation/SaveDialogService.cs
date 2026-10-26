using StringMaster.UI.Services.Interfaces;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace StringMaster.Common.Services.Implementation;

public class SaveDialogService : ISaveDialogService
{
    public bool? ShowDialog()
    {
        var dialog = new SaveFileDialog
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
