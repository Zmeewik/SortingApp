using HandlerSpace;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SortingApp
{
    public partial class OptionsPage : ContentPage
    {
        InfoHandler infoHandler;
        internal OptionsPage(InfoHandler infoHandler)
        {
            InitializeComponent();
            this.infoHandler = infoHandler;

        }


        protected override void OnAppearing()
        {
            
        }

    }


}
