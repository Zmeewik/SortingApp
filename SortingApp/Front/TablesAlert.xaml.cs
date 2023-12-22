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

namespace SortingApp
{
    public partial class TablesAlert : PopupPage
    {
        public event EventHandler Closed;
        public TableItem tableItem;

        internal TablesAlert(TableItem table)
        {
            InitializeComponent();
            this.tableItem = table;
            nameEntry.Text = table.Name;
            dateEntry.Text = $"Последняя дата изменения {table.Date}";
        }

        protected override void OnAppearing()
        {
            
        }

        private void OnChange(object sender, System.EventArgs e)
        {
            // Modify Material properties as needed
            tableItem.Name = nameEntry.Text;
            tableItem.isSet = true;


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
            // Modify Material properties as needed
            if (tableItem.Name != nameEntry.Text)
                tableItem.isChanged = true;
            tableItem.Name = nameEntry.Text;
            

            // Close the popup
            OnClosed();
            PopupNavigation.Instance.PopAsync();
        }

        protected virtual void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
