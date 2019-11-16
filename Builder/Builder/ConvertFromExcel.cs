using System;
using System.Collections.Generic;
using System.Text;

namespace Builder
{
    public class ConvertFromExcel:ReadExcelFile
    {
        public static void Convert()
        {
            for(int y = 0; y < Value.GetLength(0); y++)
            {
                Console.WriteLine($"{y + 1}枚目のシート");
                for (int x = 0; x < Value.GetLength(1); x++)
                {
                    for(int z = 0; z < Value.GetLength(2); z++)
                    {
                        Console.Write(string.Format("{0, 3}", ($"{Value[y, x, z]}")));                      
                    }
                    Console.WriteLine();
                }            
            }


        }


    }
}
