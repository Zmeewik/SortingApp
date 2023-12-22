using System.Drawing;

namespace BaseSpace
{
    public class Material
    {
        public int number; // Номер
        public string code; // Код

        public Color color; // Цвет
        public string name; // Название

        public double amount; // Общее количество
        public int busy; // Количество занятых

        public Material(string code, int num, double quantity, Color color, string name)
        {
            this.code = code;
            this.number = num;
            this.amount = quantity;
            this.color = color;
            this.name = name;
        }
    }
}