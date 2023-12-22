using HandlerSpace;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SortingApp
{
    public partial class SignUpPage : ContentPage
    {
        InfoHandler infoHandler;
        internal SignUpPage(InfoHandler infoHandler)
        {
            InitializeComponent();
            //BindingContext = this;
            this.infoHandler = infoHandler;

        }


        protected override void OnAppearing()
        {
            
        }

    }


}
