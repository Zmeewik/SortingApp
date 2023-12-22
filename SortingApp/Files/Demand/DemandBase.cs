using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BaseSpace
{
    internal class DemandBase : IDemand
    { 
        //Список заказов
        List<Demand> demands = new List<Demand> ();

        ///////////////////////////////////////////////////////////Взаимодействие

        public void CreateExample()
        {
            demands = new List<Demand>
            {
                new Demand(new Dictionary<int, double>{ { 1, 1 }, { 2, 4 } }, "", 1, 1, Color.Black, "Bed"),
                new Demand(new Dictionary<int, double>{ {1, 3 }, { 2, 4 } }, "", 2, 3, Color.Black, "Sofa"),
                new Demand(new Dictionary<int, double>{ { 1, 4 }, { 3, 2 } }, "", 3, 2, Color.Black, "Shelf")
            };
        }

        //Создать заказ
        public void CreateDemand(List<int> mats, List<double> nums, string code, int num, int quantity, Color color, string name)
        {
            demands.Add(new Demand(mats, nums, code, num, quantity, color, name));
        }

        public void CreateDemand(Dictionary<int, double> mats, string code, int num, int quantity, Color color, string name)
        {
            demands.Add(new Demand(mats, code, num, quantity, color, name));
        }

        //Удалить заказ
        public void DeleteDemand(Demand demand)
        {
            demands.Remove(demand);
        }

        //Отчистить базу заказов
        public void ClearDemand()
        {
            demands.Clear();
        }

        //Создать код
        private void CreateCode()
        { 
            
        }

        ///////////////////////////////////////////////////////////Замена
        //Сменить номер заказа
        public void ChangeDemandNum(int num, int newNum)
        {
            if (num >= 0 && num < demands.Count)
            {
                demands[num].num = newNum;
            }
        }
        //Изменить имя заказа
        public void ChangeDemandName(int num, string newNum)
        {
            if (num >= 0 && num < demands.Count)
            {
                demands[num].name = newNum;
            }
        }
        //Изменить цвет заказа
        public void ChangeDemandColor(int num, Color newNum)
        {
            if (num >= 0 && num < demands.Count)
            {
                demands[num].color = newNum;
            }
        }
        //Изменить количество заказов одного типа
        public void ChangeDemandQuantity(int num, int newNum)
        {
            if (num >= 0 && num < demands.Count)
            {
                demands[num].quantity = newNum;
            }
        }

        //Изменить количечство занятых материалов заказа
        public void ChangeDemandOccupied(int num, int mat, double mNum)
        {
            if (num >= 0 && num < demands.Count)
            {
                if (demands[num].occupiedMaterials.ContainsKey(mat))
                {
                    demands[num].occupiedMaterials[mat] = mNum;
                }
            }
        }

        ///////////////////////////////////////////////////////////Сортировка
        //Отсортировать по имени
        public void SortByName(bool ascending = true)
        {
            if (ascending)
            { List<Demand> sortedDemands = demands.OrderBy(d => d.name).ToList(); }
            else
            { List<Demand> sortedDemands = demands.OrderByDescending(d => d.name).ToList(); }
        }

        //Отсортировать по ноиеру
        public void SortByNum(bool ascending = true)
        {
            if (ascending)
            { List<Demand> sortedDemands = demands.OrderBy(d => d.num).ToList(); }
            else
            { List<Demand> sortedDemands = demands.OrderByDescending(d => d.num).ToList(); }
        }

        //Отсортировать по количеству заказов
        public void SortByQuantity(bool ascending = true)
        {
            if (ascending)
            { List<Demand> sortedDemands = demands.OrderBy(d => d.quantity).ToList(); }
            else
            { List<Demand> sortedDemands = demands.OrderByDescending(d => d.quantity).ToList(); }
        }

        //Отсортировать по материалу и их количеству
        public void SortByMaterial(int mat, bool ascending = true)
        {
            if (ascending)
            {   List<Demand> sortedDemands = demands
                .OrderByDescending(d => d.occupiedMaterials.ContainsKey(mat))
                .ThenByDescending(d => d.occupiedMaterials[mat])
                .ToList(); }
            else
            {
                List<Demand> sortedDemands = demands
                .OrderBy(d => d.occupiedMaterials.ContainsKey(mat))
                .ThenBy(d => d.occupiedMaterials[mat])
                .ToList();
            }
        }


        ///////////////////////////////////////////////////////////Поиск

        public double GetUsedMaterials(int index)
        {
            double sum = 0;

            foreach (var demand in demands)
            {
                foreach (var ocMat in demand.occupiedMaterials)
                {
                    if (ocMat.Key == index)
                    {
                        sum += ocMat.Value * demand.quantity;
                    }
                }
            }

            return sum;
        }

        //Получить элемент по номеру
        public int FindByNumber(int num)
        {
            for (int i = 0; i < demands.Count; i++)
            {
                if (demands[i].num == num)
                {
                    return i;
                }
            }
            return -1;
        }

        //Получить элемент по названию
        public int FindByName(string name)
        {
            for (int i = 0; i < demands.Count; i++)
            {
                if (demands[i].name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        //Получение характеристики
        public type GetCharacteristic<type>(int index, Func<Demand, type> characteristicSelector)
        {
            if (index >= 0 && index < demands.Count)
            {
                Demand demand = demands[index];
                return characteristicSelector(demand);
            }
            return default;
        }

        public List<Demand> GetAllDemands()
        { 
            return demands;
        }

        public void SaveAllDemands(List<Demand> demands)
        {
            this.demands = demands;
        }

        public void DeleteDependencies(int index)
        {
            foreach (var demand in demands)
            {
                if (demand.occupiedMaterials.ContainsKey(index))
                {
                    demand.occupiedMaterials.Remove(index);
                }
            }
        }
    }
}