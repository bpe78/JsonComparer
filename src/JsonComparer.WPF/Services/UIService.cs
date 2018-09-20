using System;
using JsonComparer.WPF.Interfaces;
using Microsoft.Win32;

namespace JsonComparer.WPF.Services
{
    class UIService : IUIService
    {
        #region IUIService Members

        public string LoadFileDialog()
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                CheckPathExists = true,
                CheckFileExists = true,
                DefaultExt = "json",
                AddExtension = true,
                Multiselect = false
            };

            if (dlg.ShowDialog() == true)
                return dlg.FileName;

            return string.Empty;
        }

        #endregion
    }
}
