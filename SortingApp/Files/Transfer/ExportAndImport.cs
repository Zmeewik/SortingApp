using BaseSpace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace TransferSpace
{
    class ExcelHandler : IExcelHandler
    {

        int CurrentTable { get; set; } = 0;
        List<ExcelTable> AllTables { get; set; } = new List<ExcelTable>();
        List<string> paths = new List<string>();

        public ExcelHandler()
        {

        }

        public void WriteAllPathes(List<string> data)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.txt");

            try
            {
                // Write each table name on a new line
                File.WriteAllLines(filePath, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }

        private List<string> GetAllPathes()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.txt");

            try
            {
                // Read all lines from the file
                return new List<string>(File.ReadAllLines(filePath));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from file: {ex.Message}");
                return new List<string>();
            }
        }

        private void LoadTables(List<string> paths)
        {
            AllTables = new List<ExcelTable>();
            foreach (var p in paths)
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), p + ".xlsx");
                AllTables.Add(new ExcelTable(filePath, 1, p));
            }
        }

        public void Initialize()
        {
            var paths = GetAllPathes();
            if (paths != null)
            {
                LoadTables(paths);
                this.paths = paths;
            }

        }

        public void CreateTable(string name, out string newFileName)
        {
            if (paths.Contains(name))
            {
                int r = 1;
                while (paths.Contains(name + " " + r + ")"))
                {
                    r++;
                }
                paths.Add(name + " " + r + ")");
                newFileName = name + " " + r + ")";
                List<string> newStrings = new List<string>();
                
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), name + " " + r + ")" + ".xlsx");
                //File.WriteAllLines(filePath, newStrings);
                AllTables.Add(new ExcelTable(filePath, 0, newFileName));

                WriteAllPathes(paths);
                LoadTables(paths);
            }
            else
            {
                paths.Add(name);
                newFileName = name;
                List<string> newStrings = new List<string>();
                
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), name + ".xlsx");
                //File.WriteAllLines(filePath, newStrings);
                AllTables.Add(new ExcelTable(filePath, 0, name));

                WriteAllPathes(paths);
                LoadTables(paths);
            }
        }

        public void DeleteTable(string name)
        {
            if (paths.Contains(name))
            {
                paths.Remove(name);
            }
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), name + ".xlsx");
            File.Delete(filePath);

            WriteAllPathes(paths);
            LoadTables(paths);
        }

        public void ReadAllInformation(int table, List<Material> materials, List<Demand> demands)
        {
            CurrentTable = table;

            int rowCount = 0; int colCount = 0;

            rowCount = (int)AllTables[table].ReadDbl(1, 2);
            colCount = (int)AllTables[table].ReadDbl(2, 2);

            if (rowCount == 0 && colCount == 0)
                return;

            System.Diagnostics.Debug.WriteLine($"Debug message: {rowCount}!");
            System.Diagnostics.Debug.WriteLine($"Debug message: {colCount}!");

            for (int r = 0; r < rowCount; r++)
            {
                materials.Add(new Material("", 0, 0, System.Drawing.Color.White, ""));
            }
            for (int r = 0; r < colCount; r++)
            {
                demands.Add(new Demand(new Dictionary<int, double>(),"", 0, 0, System.Drawing.Color.White, ""));
            }

            // Loop through rows
            for (int x = 1; x < 4 + rowCount; x++)
            {
                // Loop through columns
                for (int y = 1; y < 4 + colCount; y++)
                {
                    if (y == 1 && x == 1)
                    {
                        AllTables[table].date = AllTables[table].ReadStr(x, y);
                    }

                    // Read materials characteristics
                    if (y == 1 && x >= 4)
                    {
                        materials[x - 4].number = (int)AllTables[table].ReadDbl(x, y);
                    }
                    if (y == 2 && x >= 4)
                    {
                        materials[x - 4].name = AllTables[table].ReadStr(x, y);
                    }
                    if (y == 3 && x >= 4)
                    {
                        materials[x - 4].amount = AllTables[table].ReadDbl(x, y);
                    }

                    // Read demands characteristics
                    if (y >= 4 && x == 1)
                    {
                        demands[y - 4].num = (int)AllTables[table].ReadDbl(x, y);
                    }
                    if (y >= 4 && x == 2)
                    {
                        demands[y - 4].name = AllTables[table].ReadStr(x, y);
                    }
                    if (y >= 4 && x == 3)
                    {
                        demands[y - 4].quantity = (int)AllTables[table].ReadDbl(x, y);
                    }

                    if (y >= 4 && x >= 4)
                    {
                        if (AllTables[table].ReadStr(x, y) != default)
                        {
                            demands[y - 4].occupiedMaterials[x - 3] = AllTables[table].ReadDbl(x, y);
                        }
                    }
                }
            }
        }

        public void WriteAllInformation(List<Material> materials, List<Demand> demands)
        {
            int table = CurrentTable;

            if (!AllTables.Any())
            {
                return;
            }

            AllTables[table].Write(1, 2, materials.Count.ToString());
            AllTables[table].Write(2, 2, demands.Count.ToString());

            System.Diagnostics.Debug.WriteLine($"Debug message: {materials.Count.ToString()}!");
            System.Diagnostics.Debug.WriteLine($"Debug message: {demands.Count.ToString()}!");

            System.Diagnostics.Debug.WriteLine($"Debug message1: {AllTables[table].ReadDbl(1, 2).ToString()}!");
            System.Diagnostics.Debug.WriteLine($"Debug message1: {AllTables[table].ReadDbl(2, 2).ToString()}!");

            for (int x = 1; x < 4 + materials.Count; x++)
            {
                for (int y = 1; y < 4 + demands.Count; y++)
                {
                    if (y == 1 && x == 1)
                    {
                        AllTables[table].Write(x, y, AllTables[table].date);
                    }

                    //Write materials caracteristics
                    if (y == 1 && x >= 4)
                    {
                        AllTables[table].Write(x, y, materials[x - 4].number.ToString());
                    }
                    if (y == 2 && x >= 4)
                    {
                        AllTables[table].Write(x, y, materials[x - 4].name.ToString());
                    }
                    if (y == 3 && x >= 4)
                    {
                        AllTables[table].Write(x, y, materials[x - 4].amount.ToString());
                    }

                    //Write demands caracteristics
                    if (y >= 4 && x == 1)
                    {
                        AllTables[table].Write(x, y, demands[y - 4].num.ToString());
                    }
                    if (y >= 4 && x == 2)
                    {
                        AllTables[table].Write(x, y, demands[y - 4].name.ToString());
                    }
                    if (y >= 4 && x == 3)
                    {
                        AllTables[table].Write(x, y, demands[y - 4].quantity.ToString());
                    }

                    if (y >= 4 && x >= 4)
                    {
                        if (demands[y - 4].occupiedMaterials.ContainsKey(x - 3))
                        {
                            AllTables[table].Write(x, y, demands[y - 4].occupiedMaterials[x - 3].ToString());
                        }
                    }
                }
            }
            
            AllTables[table].Save();
        }

        public void ChangeFileName(string oldFileName, ref string newFileName)
        {
            if (paths.Contains(oldFileName))
            {
                paths.Remove(oldFileName);
            }

            if (paths.Contains(newFileName))
            {
                int r = 1;
                while (paths.Contains(newFileName + " " + r + ")"))
                {
                    r++;
                }
                paths.Add(newFileName + " " + r + ")");
                newFileName = newFileName + " " + r + ")";
            }
            else
            {
                paths.Add(newFileName);
            }

            // Get the path to the application's data folder
            string appFolderPath = FileSystem.AppDataDirectory;

            // Construct the full path of the old file
            string oldFilePath = Path.Combine(appFolderPath, oldFileName);

            // Check if the old file exists
            if (File.Exists(oldFilePath))
            {
                // Construct the full path of the new file
                string newFilePath = Path.Combine(appFolderPath, newFileName);

                // Rename the file
                File.Move(oldFilePath, newFilePath);
            }

            WriteAllPathes(paths);
            LoadTables(paths);
        }

        public bool isThereTables()
        {
            return AllTables.Any();
        }

        public List<ExcelTable> GetAllTables()
        {
            return AllTables;
        }

        public List<string> GetAllPathesList()
        {
            return paths;
        }

        public int FindTableByName(string path)
        {
            int y = 0;
            foreach (var p in paths)
            {
                if (path == p)
                {
                    return y;
                }
                y++;
            }
            return -1;
        }
    }
}