namespace HandlerSpace
{
    internal class AppHandler
    {

        //Синглтон
        public static AppHandler instance;
        public static AppHandler getInstance(Xamarin.Forms.Page inputHandler)
        {
            if (instance == null)
            {
                instance = new AppHandler(inputHandler);
            }
            return instance;
        }
        


        //Конструктор
        private HandlerSpace.InfoHandler i_handler;
        public HandlerSpace.InfoHandler I_Handler { get { return i_handler; } }
        private AppHandler(Xamarin.Forms.Page inputHandler)
        {
            i_handler = HandlerSpace.InfoHandler.getInstance(inputHandler);
        }


    }
}