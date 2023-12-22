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
    public partial class SureAlert : PopupPage
    {
        public event EventHandler Closed;
        public InfoTransfer isAgree;

        internal SureAlert(InfoTransfer agree)
        {
            InitializeComponent();
            isAgree = agree;
        }

        protected override void OnAppearing()
        {
            
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
            isAgree.isAgree = true;

            // Close the popup
            OnClosed();
            PopupNavigation.Instance.PopAsync();
        }

        protected virtual void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }

    public class InfoTransfer
    { 
        public bool isAgree;
    }
}
