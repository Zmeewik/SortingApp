using BaseSpace;
using InputSpace;
using VisualSpace;
using TransferSpace;
using InputSpace;
using SortingApp;
using System;

namespace HandlerSpace
{
    public class InfoHandler
    {

        //Синглтон
        public static InfoHandler instance;
        public static InfoHandler getInstance(Xamarin.Forms.Page inputHandler)
        {
            if (instance == null)
            {
                instance = new InfoHandler(inputHandler);
            }
            return instance;
        }

        //Конструктор
        private InfoHandler(Xamarin.Forms.Page inputHandler)
        {
            this.inputHandler = inputHandler;
            CreateBases();
        }

        private Xamarin.Forms.Page inputHandler;

        //Создать базы данных
        private void CreateBases()
        {
            demand = new DemandBase();
            material = new MaterialBase();
            visual = new VisualHandler();
            excel = new ExcelHandler();

            Initialization();
        }

        private void Initialization()
        {
            excel.Initialize();
        }

        private void SubscribeEvents() 
        {
            
        }

        private void Visualize()
        { 
            
        }
        
        public void ChangeTable()
        {
            System.Diagnostics.Debug.WriteLine("TableChaanged!");
        }

        public void AddMaterial()
        {
            System.Diagnostics.Debug.WriteLine("Adding");
        }

        public IDemand demand;
        public IMaterial material;
        public IVisual visual;
        public IExcelHandler excel;

    }
}