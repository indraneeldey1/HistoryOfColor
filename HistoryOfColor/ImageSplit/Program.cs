// See https://aka.ms/new-console-template for more information

using System.Drawing;
using Emgu.CV;

class Program
{
    static void Main(string[] args)
    {
        string imagePath = "image/young_woman_with_a_butterfly_1980.62.40.jpg";

        Mat imageMat = CvInvoke.Imread(imagePath);
        int squareWidth = imageMat.Width / 100;
        int squareHeight = imageMat.Height / 100;
        
        Mat square = new();
        
        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                int xPos = x * squareWidth;
                int yPos = y * squareHeight;
                square = new(imageMat, new Rectangle(xPos, yPos, squareWidth, squareHeight));
                square.Save($"squares/{x}_{y}.jpg");
            }
        }
        
    }
}