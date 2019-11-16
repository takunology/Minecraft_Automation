using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Builder
{
    public class ReadExcelFile
    {
        static int SHEET = 2; //高さ
        static int ROW = 5; //幅
        static int COL = 5; //奥行

        public static int[,,] Value = new int[SHEET, ROW, COL]; //エクセルデータの保持

        public static void OpenExcelFile()
        {
            try
            {
                string Path = "../../../Excel/TestBook.xlsx";
                var Book = WorkbookFactory.Create(Path); //参照するブックのパス

                //マイクラの座標に合わせるために x, y, z を使う
                for (int y = 0; y < SHEET; y++)
                {
                    var Sheet = Book.GetSheetAt(y); //N枚目のシートを参照
                    for (int x = 0; x < ROW; x++)
                    {
                        for (int z = 0; z < COL; z++)
                        {
                            Value[y, x, z] = GetValue(Sheet, x, z);
                        }
                    }
                }
            }
            catch(Exception e) { Console.WriteLine(e); }
        }

        public static int GetValue(ISheet Sheet, int Row, int Column)
        {
            var row = Sheet.GetRow(Row) ?? Sheet.CreateRow(Row); //例外対策
            var cell = row.GetCell(Column) ?? row.CreateCell(Column);
            int value;

            value = int.Parse(cell.NumericCellValue.ToString());
            return value;
        }
    }
}