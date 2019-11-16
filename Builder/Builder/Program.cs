using NPOI.SS.UserModel;
using System;

namespace Builder
{
    class Program : ConvertFromExcel
    {
        //static ConvertFromExcel convertFromExcel = new ConvertFromExcel();
        static void Main(string[] args)
        {
            //Convert();
            OpenExcelFile(); //Excelファイルを開く
            Convert(); //得られたデータ
        }


    }
}
