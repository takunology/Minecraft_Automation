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
                for (int x = 0; x < Value.GetLength(1); x++)
                {
                    for(int z = 0; z < Value.GetLength(2); z++)
                    {
                        string value = Value[y, x, z];
                        switch (value)
                        {
                            case "0":
                                Value[y, x, z] = "0"; //空気
                                break;

                            case "1":
                                Value[y, x, z] = "1"; //石
                                break;

                            case "2":
                                Value[y, x, z] = "20"; //ガラス
                                break;

                            case "3":
                                Value[y, x, z] = "5 0"; //木材(オーク)
                                break;

                            case "4":
                                Value[y, x, z] = "17"; //原木
                                break;

                            case "5":
                                Value[y, x, z] = "24"; //砂岩
                                break;

                            case "6":
                                Value[y, x, z] = "45"; //レンガ
                                break;
                        }
                    }
                }            
            }
        }

        public static void ShowSheet()
        {
            for (int y = 0; y < Value.GetLength(0); y++)
            {
                Console.WriteLine($"{y + 1}枚目のシート");
                for (int x = 0; x < Value.GetLength(1); x++)
                {
                    for (int z = 0; z < Value.GetLength(2); z++)
                    {
                        Console.Write(string.Format("{0, 3}", ($"{Value[y, x, z]}")));
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}