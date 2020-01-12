using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PictureConvert
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Input picture Path:");
            string path = Console.ReadLine();

            ConvertOut(path);
            await PictMaker();
        }

        static void ConvertOut(string path)
        {
            Color[,] pixelColor; //ピクセルカラー情報
            int width, height; // 画像の幅と高さ
            try
            {
                Bitmap img = new Bitmap(Image.FromFile(path));
                width = img.Width;
                height = img.Height;
                pixelColor = new Color[img.Width, img.Height];
                //色の走査
                for(int h = 0; h < img.Height; h++)
                {
                    for(int w = 0; w < img.Width; w++)
                    {
                        pixelColor[w, h] = img.GetPixel(w, h);
                    }
                }

                //色情報の保存
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                StreamWriter writer = new StreamWriter("color.txt", false, encoding);
                for(int h = 0; h < img.Height; h++)
                {
                    for(int w = 0; w < img.Width; w++)
                    {
                        writer.Write($"{pixelColor[w, h]}, ");
                    }
                    writer.WriteLine("");
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

        }

        static async Task PictMaker()
        {

        }
    }
}
