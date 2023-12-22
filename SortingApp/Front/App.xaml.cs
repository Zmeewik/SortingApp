using System;
using System.Diagnostics;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SortingApp
{
    public partial class App : Application
    {

        private HandlerSpace.AppHandler a_handler;
        private InputSpace.InputHandler a_input;

        private Timer frameTimer;
        private const int DesiredFPS = 30;

        public App()
        {
            InitializeComponent();
            a_handler = HandlerSpace.AppHandler.getInstance(MainPage);
            //MainPage = new SignUpPage(a_handler.I_Handler);
            //MainPage = new MainPage(a_handler.I_Handler);
            //MainPage = new TablesPage(a_handler.I_Handler);

            MainPage = new TablesPage(a_handler.I_Handler);
        }

        //Обновление изображения
        private void FrameCallback(object state)
        {

        }

        //Стартовая функция
        protected override void OnStart()
        {
            base.OnStart();

            SetupTimer();
        }

        //На останорвку приложения
        protected override void OnSleep()
        {
            base.OnSleep();

            frameTimer.Dispose();
        }

        //На возвращение в приложение
        protected override void OnResume()
        {
            base.OnResume();

            SetupTimer();
        }

        private void SetupTimer()
        {
            //Инициализайция таймера
            int interval = 1000 / DesiredFPS;
            frameTimer = new Timer(FrameCallback, null, 0, interval);
        }
    }
}
