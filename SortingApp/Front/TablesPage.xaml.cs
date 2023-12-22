using BaseSpace;
using HandlerSpace;
using Rg.Plugins.Popup.Services;
using SortingApp.Files.Visuals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Globalization;
using System.IO;

namespace SortingApp
{
    public partial class TablesPage : ContentPage
    {
        public InfoHandler infoHandler;

        //Material list//
        public ObservableCollection<TableItem> AllTables { get; set; } = new ObservableCollection<TableItem>();
        private bool isSaved = true;
        public bool IsSaved
        {
            get
            {
                return isSaved;
            }
            set
            {
                isSaved = value;
                if (isSaved) saveTextButton.Text = "Сохранить";
                else saveTextButton.Text = "Сохранить *";
            }
        }

        private int chosenTab = 0;

        internal TablesPage(InfoHandler infoHandler)
        {
            InitializeComponent();
            BindingContext = new TablesViewModel(this);
            this.infoHandler = infoHandler;



            if (infoHandler.excel.isThereTables())
            {
                AllTables = new ObservableCollection<TableItem>();
                foreach (var table in infoHandler.excel.GetAllTables())
                {
                    if (DateTime.TryParseExact(table.date, "MM.dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                    {
                        AllTables.Add(new TableItem(table.name, result));
                    }
                    else
                    {
                        AllTables.Add(new TableItem(table.name, DateTime.Now));
                    }
                }
            }



            materialList.ItemsSource = AllTables;
        }

        private void ChangeImage(object sender, EventArgs e)
        { 
            
        }


        void OnAddMaterialButtonClicked(object sender, EventArgs e)
        {
            AllTables.Add(new TableItem("1", DateTime.Now));
            //materialList.ItemsSource = allMaterials;
        }

        async void OnFrameEmail_Tapped(object sender, EventArgs e)
        {
            _menuVisible = true;
            swipeMenuGrid.IsVisible = true;
            //await swipeMenu.TranslateTo(100, 0, 250, Easing.SinOut);
            await Task.WhenAll(
                swipeMenu.TranslateTo(100, 0, 250, Easing.SinOut),
                swipeMenuGrid.FadeTo(1, 250, Easing.SinOut)
            );
        }

        async void OnMaterialAdd(object sender, EventArgs e)
        {
            InfoTableHandler info = new InfoTableHandler() { isSet = false };
            var alert = new TablesAddAlert(info);

            alert.Closed += (sender1, args) =>
            {
                // Update tables after the popup is closed
                AddTable(info);
            };
            await PopupNavigation.Instance.PushAsync(alert);
        }

        void AddTable(InfoTableHandler info)
        {
            AllTables.Add(new TableItem(info.name, DateTime.Now));
            infoHandler.excel.CreateTable(info.name, out string newName);
            AllTables.Last().Name = newName;
            infoHandler.excel.GetAllTables().Last().date = DateTime.Now.ToString("MM.dd");
            infoHandler.excel.WriteAllPathes(infoHandler.excel.GetAllPathesList());
        }

        void OnMaterialChange(object sender, EventArgs e)
        {

        }

        void OnMaterialChangeNum(object sender, EventArgs e)
        {

        }

        protected override void OnAppearing()
        {
            
        }

        //Menu//

        private double _previousX;
        private bool _menuVisible;

        private void SwipeMenuLeft()
        {

        }

        private void SwipeMenuRight()
        {

        }

        private void OnUserImageTap(object sender, EventArgs e)
        { 
            
        }

        private async void OnOpenMenuClicked(object sender, EventArgs e)
        {
            _menuVisible = true;
            swipeMenuGrid.IsVisible = true;
            await Task.WhenAll(
                swipeMenu.TranslateTo(100, 0, 250, Easing.SinOut),
                swipeMenuGrid.FadeTo(1, 250, Easing.SinOut)
            );
        }

        private async void OnSwipeMenuBackgroundTapped(object sender, EventArgs e)
        {
            if (_menuVisible)
            {
                _menuVisible = false;
                await Task.WhenAll(
                    swipeMenu.TranslateTo(400, 0, 250, Easing.SinOut),
                    swipeMenuGrid.FadeTo(0, 250, Easing.SinOut)
                );
                swipeMenuGrid.IsVisible = false;
            }
        }

        private double swipeMenuTranslationX = 0;

        /*        private void OnSwipeMenuBackgroundPanned(object sender, PanUpdatedEventArgs e)
                {
                    switch (e.StatusType)
                    {
                        case GestureStatus.Running:
                            // Adjust the translation based on the velocity to smooth out the movement
                            swipeMenuTranslationX += e.TotalX * 0.5; // You can adjust the multiplier as needed
                            swipeMenu.TranslationX = swipeMenuTranslationX;
                            break;

                        case GestureStatus.Completed:
                            // Determine whether to hide or show the swipe menu based on the translationX value
                            if (Math.Abs(swipeMenuTranslationX) > (swipeMenuGrid.Width / 2))
                            {
                                // Hide the swipe menu
                                swipeMenu.TranslateTo(swipeMenuGrid.Width, 0, 250, Easing.SinOut);
                                swipeMenuGrid.IsVisible = false;
                                _menuVisible = false;
                            }
                            else
                            {
                                // Show the swipe menu
                                swipeMenu.TranslateTo(0, 0, 250, Easing.SinOut);
                                swipeMenuGrid.IsVisible = true;
                                _menuVisible = true;
                            }

                            // Reset the translation value after completing the gesture
                            swipeMenuTranslationX = 0;
                            break;
                    }
                }*/

        private void OnSwipeMenuBackgroundPanned(object sender, PanUpdatedEventArgs e)
        {/*
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    // Adjust the translation based on the velocity to smooth out the movement
                    swipeMenuTranslationX += e.TotalX * 0.5; // You can adjust the multiplier as needed
                    swipeMenu.TranslateTo(swipeMenuTranslationX, 0);
                    break;

                case GestureStatus.Completed:
                    // Determine whether to hide or show the swipe menu based on the translationX value
                    if (Math.Abs(swipeMenuTranslationX) > (swipeMenuGrid.Width / 2))
                    {
                        // Hide the swipe menu
                        swipeMenuGrid.TranslateTo(swipeMenuGrid.Width, 0, 250, Easing.SinOut);
                        swipeMenuGrid.IsVisible = false;
                    }
                    else
                    {
                        // Show the swipe menu
                        swipeMenuGrid.TranslateTo(0, 0, 250, Easing.SinOut);
                        swipeMenuGrid.IsVisible = true;
                    }

                    // Reset the translation value after completing the gesture
                    swipeMenuTranslationX = 0;
                    break;
            }*/
        }

        protected override bool OnBackButtonPressed()
        {
            if (_menuVisible)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    _menuVisible = false;
                    await Task.WhenAll(
                        swipeMenu.TranslateTo(400, 0, 250, Easing.SinOut),
                        swipeMenuGrid.FadeTo(0, 250, Easing.SinOut)
                    );
                    swipeMenuGrid.IsVisible = false;
                });

                return true; // Indicate that the back button press is handled
            }

            return base.OnBackButtonPressed();
        }


        private void DoNothing(object sender, EventArgs e)
        { 
            
        }

        private void GoToProfile(object sender, EventArgs e)
        { 
            
        }

        private void GoToUpload(object sender, EventArgs e)
        {

        }

        private void SaveInfo(object sender, EventArgs e)
        {
            IsSaved = true;
            SaveInfo();
        }

        public void SaveInfo()
        {
            var original = infoHandler.excel.GetAllTables();
            for (int r = 0; r < AllTables.Count; r++)
            {
                if (AllTables[r].Name != original[r].name)
                {
                    string newName = AllTables[r].Name;
                    infoHandler.excel.ChangeFileName(original[r].name, ref newName);
                    original[r].name = newName;
                    AllTables[r].Name = newName;
                }
            }
            //infoHandler.excel.WriteAllPathes(infoHandler.excel.GetAllPathesList());
        }

        private void GoToTablese(object sender, EventArgs e)
        {
            if (!IsSaved)
            {
                chosenTab = 0;
                NeedSave();
            }
            else
                Application.Current.MainPage = new TablesPage(infoHandler);
        }

        private void GoToProducts(object sender, EventArgs e)
        {
            if (!IsSaved)
            {
                chosenTab = 1;
                NeedSave();
            }
            else
                Application.Current.MainPage = new DemandPage(infoHandler);
        }

        private void GoTomaterials(object sender, EventArgs e)
        {
            if (!IsSaved)
            {
                chosenTab = 2;
                NeedSave();
            }
            else
                Application.Current.MainPage = new MaterialPage(infoHandler);
        }

        public void GoTomaterials()
        {
            if (!IsSaved)
            {
                chosenTab = 2;
                NeedSave();
            }
            else
                Application.Current.MainPage = new MaterialPage(infoHandler);
        }


        private async void CloseApp(object sender, EventArgs e)
        {
            if (!IsSaved)
            {
                chosenTab = 3;
                NeedSave();
            }
            else
                await Navigation.PopToRootAsync();
        }

        private async void NeedSave()
        {
            InfoTransfer isAgreed = new InfoTransfer() { isAgree = false };
            var alert = new SaveAlert(isAgreed);

            alert.Closed += (sender, args) =>
            {
                // Update all materials after the popup is closed
                OpenNext(isAgreed.isAgree);
            };
            await PopupNavigation.Instance.PushAsync(alert);
        }

        private async void OpenNext(bool isSaving)
        {
            if (isSaving)
            {
                SaveInfo();
            }
            switch (chosenTab)
            {
                case 0:
                    Application.Current.MainPage = new TablesPage(infoHandler);
                    break;
                case 1:
                    Application.Current.MainPage = new DemandPage(infoHandler);
                    break;
                case 2:
                    Application.Current.MainPage = new MaterialPage(infoHandler);
                    break;
                case 3:
                    await Navigation.PopToRootAsync();
                    break;
                case 4:
                    break;
            }
        }

        public void ChangeTable(int index)
        {
            infoHandler.material.ClearMaterial();
            infoHandler.demand.ClearDemand();
            infoHandler.excel.ReadAllInformation(index, infoHandler.material.GetAllMaterials(), infoHandler.demand.GetAllDemands());
        }
    }

    public class TablesViewModel : INotifyPropertyChanged
    {
        private string _displayText;

        public event PropertyChangedEventHandler PropertyChanged;
        TablesPage mPage;

        public Command<TableItem> ChangeCommand { get; set; }
        public Command<TableItem> DeleteCommand { get; set; }
        public Command<TableItem> SaveCommand { get; set; }

        public TablesViewModel(TablesPage page)
        {
            mPage = page;
            ChangeCommand = new Command<TableItem>(ChangeButtonCommandClick);
            DeleteCommand = new Command<TableItem>(DeleteButtonCommandClick);
            SaveCommand = new Command<TableItem>(SaveButtonCommandClick);
        }

        async void DeleteButtonCommandClick(TableItem material)
        {
            InfoTransfer isAgreed = new InfoTransfer() { isAgree = false };
            var alert = new SureAlert(isAgreed);

            alert.Closed += (sender, args) =>
            {
                // Update all materials after the popup is closed
                DeleteAlertButtonCommandClick(material, isAgreed.isAgree);
            };
            await PopupNavigation.Instance.PushAsync(alert);


        }

        void DeleteAlertButtonCommandClick(TableItem material, bool agree)
        {
            if (agree)
            {
                var matItem = material as TableItem;
                var index = mPage.AllTables.IndexOf(matItem);
                mPage.AllTables.RemoveAt(index);
                mPage.infoHandler.excel.DeleteTable(matItem.Name);
                mPage.infoHandler.excel.WriteAllPathes(mPage.infoHandler.excel.GetAllPathesList());
            }
        }

        void SaveButtonCommandClick(TableItem table)
        { 
            
        }

        private void DisplayInfo(bool isAgreed)
        {
            mPage.DisplayAlert(isAgreed.ToString(), "i", "M");
        }

        void UpdateMaterial(TableItem matItem)
        {
            if (matItem.isChanged)
            {
                mPage.IsSaved = false;
                matItem.isChanged = false;
            }


            var index = mPage.AllTables.IndexOf(matItem);
            mPage.AllTables[index] = matItem;


            if (matItem.isSet)
            {
                matItem.isSet = true;

                mPage.IsSaved = true;
                mPage.SaveInfo();

                mPage.ChangeTable(mPage.infoHandler.excel.FindTableByName(matItem.Name));
                mPage.GoTomaterials();
            }
        }

        async void ChangeButtonCommandClick(TableItem material)
        {
            var matItem = material as TableItem;

            var alert = new TablesAlert(matItem);

            alert.Closed += (sender, args) =>
            {
                // Update all materials after the popup is closed
                UpdateMaterial(matItem);
            };

            await PopupNavigation.Instance.PushAsync(alert);
        }
        
        // INotifyPropertyChanged implementation goes here
    }

}
