using HandlerSpace;
using System.Drawing;
using System.Collections.Generic;
using System;

namespace BaseSpace
{
    public class MaterialBase : IMaterial
    {
        private List<Material> array = new List<Material>(); // Массив материалов

        ///////////////////////////////////////////////////////////Взаимодействие
        
        public void CreateExample()
        {
            array = new List<Material>
            {
                new Material("", 1, 16, Color.Black, "Wood"),
                new Material("", 2, 80, Color.Black, "Whool"),
                new Material("", 3, 19, Color.Black, "Metal")
            };
        }

        //Создать материал
        public void CreateMaterial(string code, int num, int quantity, Color color, string name)
        {
            array.Add(new Material(code, num, quantity, color, name));
        }

        //Удалить материал
        public void DeleteMaterial(int index)
        {
            array.RemoveAt(index);

            //Пересчитать индексы
        }

        //Очистить базу материалов
        public void ClearMaterial()
        {
            array.Clear();
        }

        ///////////////////////////////////////////////////////////Замена
        
        //Получение списка элементов
        public List<Material> GetMaterials() 
        {
            return array;
        }

        //замена номера
        public void ReplaceNumber(int index, int new_number)
        {
            if (index >= 0 && index < array.Count)
            {
                array[index].number = new_number;
            }
        }

        //замена цвета
        public void ReplaceColor(int index, Color new_color)
        {
            if (index >= 0 && index < array.Count)
            {
                array[index].color = new_color;
            }
        }

        //замена названия
        public void ReplaceName(int index, string new_name)
        {
            if (index >= 0 && index < array.Count)
            {
                array[index].name = new_name;
            }
        }

        //замена общего количества
        public void ReplaceAmount(int index, int new_amount)
        {
            if (index >= 0 && index < array.Count)
            {
                array[index].amount = new_amount;
            }
        }

        //замена количества занятых
        public void ReplaceBusy(int index, int new_busy)
        {
            if (index >= 0 && index < array.Count)
            {
                array[index].busy = new_busy;
            }
        }

        ///////////////////////////////////////////////////////////Сортировки
        
        public void SortByNumber(bool reverse = false)
        {
            if(!reverse)
                //сортировка по номеру (1 - 999999)
                array.Sort((material1, material2) => material1.number.CompareTo(material2.number));
            else
                //сортировка по номеру реверс (999999 - 1)
                array.Sort((material1, material2) => material2.number.CompareTo(material1.number));
        }

        
        public void SortByName(bool reverse = false)
        {
            if (!reverse)
                //сортировка по названию (а - я / a - z)
                array.Sort((material1, material2) => material1.name.CompareTo(material2.name));
            else
                //сортировка по названию реверс (я - а / z - a)
                array.Sort((material1, material2) => material2.name.CompareTo(material1.name));
        }

        

        
        public void SortByAmount(bool reverse = false)
        {
            if (!reverse)
                //сортировка по общему количеству (0 - 999999)
                array.Sort((material1, material2) => material1.amount.CompareTo(material2.amount));
            else
                //сортировка по общему количеству реверс (999999 - 0)
                array.Sort((material1, material2) => material2.amount.CompareTo(material1.amount));
        }

        
        public void SortByBusy(bool reverse = false)
        {
            if (!reverse)
                //сортировка по количеству занятых (0 - 999999)
                array.Sort((material1, material2) => material1.busy.CompareTo(material2.busy));
            else
                //сортировка по количеству занятых реверс (999999 - 0)
                array.Sort((material1, material2) => material2.busy.CompareTo(material1.busy));
        }

        ///////////////////////////////////////////////////////////Поиск
        //поиск по номеру
        public int FindByNumber(int number)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].number == number)
                {
                    return i;
                }
            }
            return -1;
        }

        //поиск по названию
        public int FindByName(string name)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        //получение характеристики
        public type GetCharacteristic<type>(int index, Func<Material, type> characteristicSelector)
        {
            if (index >= 0 && index < array.Count)
            {
                Material material = array[index];
                return characteristicSelector(material);
            }
            return default;
        }

        //Получить весь список
        public List<Material> GetAllMaterials()
        {
            return array;
        }

        //Сохранить весь список
        public void SaveAllMaterials(List<Material> demands)
        {
            this.array = demands;
        }
    }
}