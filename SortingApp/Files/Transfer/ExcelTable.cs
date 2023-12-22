using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using OfficeOpenXml;

namespace TransferSpace
{
    public class ExcelTable
    {

        public string name = "";
        public string path = "";
        public string date = "";
        public int Rows = 0;
        public int Columns = 0;

        ExcelPackage package;
        ExcelWorksheet ws;

        public ExcelTable(string path, int Sheet, string name)
        {
            this.path = path;
            this.name = name;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (File.Exists(path))
            {
                package = new ExcelPackage(new FileInfo(path));
                if (package.Workbook.Worksheets.Count > 0)
                {
                    ws = package.Workbook.Worksheets[0];
                }
                else
                    ws = package.Workbook.Worksheets.Add("Sheet1");

            }
            else
            {
                package = new ExcelPackage();
                ws = package.Workbook.Worksheets.Add("Sheet1");
                package.SaveAs(new FileInfo(path));
            }

        }

        public void CreateFile()
        {
            
        }

        // прочитать ячейку (слово)
        public string ReadStr(int i, int j)
        {
            i++; j++;
            if (ws.Cells[i, j].Value != null)
                return ws.Cells[i, j].Value.ToString();
            else
                return default;
        }

        // прочитать ячейку (число)
        public double ReadDbl(int i, int j)
        {
            i++; j++;
            
            if (ws.Cells[i, j].Value != null && double.TryParse(ws.Cells[i, j].Value.ToString(), out double result))
                return result;
            else
                return default;
        }

        // записать в ячейку
        public void Write(int i, int j, string s)
        {
            i++; j++;
            ws.Cells[i, j].Value = s;
        }

        // сохранить изменения
        public void Save()
        {
            var fileInfo = new FileInfo(path);
            package.SaveAs(fileInfo);
        }

        // закрыть таблицу
        public void Close()
        {

        }
    }
}