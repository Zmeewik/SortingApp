using System;
using System.Collections.Generic;
using System.Drawing;

namespace BaseSpace
{
    public interface IDemand
    {
        //Создать заказ
        void CreateExample();
        void CreateDemand(List<int> mats, List<double> nums, string code, int num, int quantity, Color color, string name);
        void CreateDemand(Dictionary<int, double> mats, string code, int num, int quantity, Color color, string name);
        //Удалить заказ
        void DeleteDemand(Demand demand);
        //Отчистить базу заказов
        void ClearDemand();
        
        //Изменить номер заказа
        void ChangeDemandNum(int num, int newNum);
        //Изменить имя заказа
        void ChangeDemandName(int num, string newNum);
        //Изменить цвет заказа
        void ChangeDemandColor(int num, Color newNum);
        //Изменить количество заказов одного типа
        void ChangeDemandQuantity(int num, int newNum);

        //Изменить количечство занятых материалов заказа
        void ChangeDemandOccupied(int num, int mat, double mNum);

        //Отсортировать по имени
        void SortByName(bool ascending);
        //Отсортировать по ноиеру
        void SortByNum(bool ascending);
        //Отсортировать по количеству заказов
        void SortByQuantity(bool ascending);
        //Отсортировать по материалу и их количеству
        void SortByMaterial(int mat, bool ascending);
        
        //Получить всю сумму материалов
        double GetUsedMaterials(int index);
        //Получить заказ по yjvthe
        int FindByNumber(int num);
        //Получить заказ по имени
        int FindByName(string name);
        //Получение характеристики
        type GetCharacteristic<type>(int index, Func<Demand, type> characteristicSelector);
        //Получить весь список
        List<Demand> GetAllDemands();
        //Сохранить весь список
        void SaveAllDemands(List<Demand> demands);
        //Удалить зависимости
        void DeleteDependencies(int index);
    }
}