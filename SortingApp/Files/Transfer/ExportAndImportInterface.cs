using BaseSpace;
using System.Collections.Generic;

namespace TransferSpace
{
    public interface IExcelHandler
    {
        void Initialize();
        void CreateTable(string name, out string newString);
        void DeleteTable(string name);
        void WriteAllPathes(List<string> data);
        void ReadAllInformation(int table, List<Material> materials, List<Demand> demands);
        void WriteAllInformation(List<Material> materials, List<Demand> demands);
        bool isThereTables();
        List<ExcelTable> GetAllTables();
        List<string> GetAllPathesList();
        int FindTableByName(string path);
        void ChangeFileName(string oldFileName, ref string newFileName);
    }
}