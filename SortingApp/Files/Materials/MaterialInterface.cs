using System;
using System.Collections.Generic;
using System.Drawing;

namespace BaseSpace
{
    public interface IMaterial
    {
        void CreateExample();
        //Создать материал
        void CreateMaterial(string code, int num, int quantity, Color color, string name);
        //Создать материал
        void DeleteMaterial(int index);
        //Создать материал
        void ClearMaterial();

        //Замена номера
        void ReplaceNumber(int index, int new_number);
        //замена цвета
        void ReplaceColor(int index, Color new_color);

        //замена названия
        void ReplaceName(int index, string new_name);

        //замена общего количества
        void ReplaceAmount(int index, int new_amount);

        //замена количества занятых
        void ReplaceBusy(int index, int new_busy);

        //сортировка по номеру (1 - 999999)
        //сортировка по номеру реверс (999999 - 1)
        void SortByNumber(bool reverse);
        
        //сортировка по названию (а - я / a - z)
        //сортировка по названию реверс (я - а / z - a)
        void SortByName(bool reverse);

        //сортировка по общему количеству (0 - 999999)
        //сортировка по общему количеству реверс (999999 - 0)
        void SortByAmount(bool reverse);

        //сортировка по количеству занятых (0 - 999999)
        //сортировка по количеству занятых реверс (999999 - 0)
        void SortByBusy(bool reverse);

        //поиск по номеру
        int FindByNumber(int number);

        //поиск по названию
        int FindByName(string name);

        //получение характеристики
        type GetCharacteristic<type>(int index, Func<Material, type> characteristicSelector);
        //Получить весь список
        List<Material> GetAllMaterials();
        //Сохранить весь список
        void SaveAllMaterials(List<Material> demands);
    }
}