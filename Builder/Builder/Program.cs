using NPOI.SS.UserModel;
using System;

namespace Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenExcelFile();
        }

        static void OpenExcelFile()
        {
            var Path = "../../../Excel/TestBook.xlsx";
            var Book = WorkbookFactory.Create(Path);
            var Sheet = Book.GetSheetAt(0); //1枚目？

            GetValue(Sheet, 0, 0); //シート1枚目の0行0列を取得
            
        }

        static void GetValue(ISheet Sheet, int Column, int Row)
        {
            var row = Sheet.GetRow(Row) ?? Sheet.CreateRow(Row); //例外対策
            var cell = row.GetCell(Column) ?? row.CreateCell(Column);
            string value;

            value = cell.StringCellValue;

            Console.WriteLine(value);
        }
    }
}
