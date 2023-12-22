using HandlerSpace;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SortingApp.Files.Visuals;
using System.Runtime.CompilerServices;
using BaseSpace;
using System;
using Xamarin.Essentials;
using System.Linq;
using System.IO;

namespace SortingApp
{
    public partial class TablesAddAlert : PopupPage
    {
        public event EventHandler Closed;
        public InfoTableHandler infoTable;

        internal TablesAddAlert(InfoTableHandler table)
        {
            InitializeComponent();
            this.infoTable = table;
        }

        protected override void OnAppearing()
        {
            
        }

        private async void PickFileButton_Clicked(object sender, EventArgs e)
        {
            try
            {

                var filePicker = await FilePicker.PickAsync();
                if (filePicker != null)
                {
                    if (IsExcelFile(filePicker.FileName))
                    {
                        // Access file properties
                        var fileName = filePicker.FileName;
                        var filePath = filePicker.FullPath;
                        

                        FileButton.Text = "Выбрать другой файл...";
                        nameEntry.Text = Path.GetFileNameWithoutExtension(fileName);

                        infoTable.name = Path.GetFileNameWithoutExtension(fileName);
                        infoTable.path = filePath;
                    }
                    else
                    {
                        DisplayAlert("Предупреждение!", "Добавляемый файл должен иметь расширение .xlsx или .xls\n(Расширение excel таблицы)", "Ок");
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private bool IsExcelFile(string fileName)
        {
            var excelExtensions = new[] { ".xlsx", ".xls" }; // Add more extensions as needed

            return excelExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }

        private void OnChange(object sender, System.EventArgs e)
        {
            OnClosed();
            PopupNavigation.Instance.PopAsync();
            //Change table on other
        }

        private void OnCancel(object sender, System.EventArgs e)
        {
            // Close the popup
            OnClosed();
            PopupNavigation.Instance.PopAsync();
        }

        private void OnSave(object sender, EventArgs e)
        {
            // Set if path to excel is set or not
            if(string.IsNullOrEmpty(infoTable.path)) infoTable.isSet = false;
            else infoTable.isSet = true;

            if (infoTable.isSet)
            {
                if (infoTable.name != nameEntry.Text) infoTable.name = nameEntry.Text;
            }
            else
            {
                infoTable.name = nameEntry.Text;
            }

            // Close the popup
            OnClosed();
            PopupNavigation.Instance.PopAsync();
        }

        protected virtual void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }

    public class InfoTableHandler
    {
        public bool isSet = false;
        public string path = "";
        public string name = "";
    }
}
