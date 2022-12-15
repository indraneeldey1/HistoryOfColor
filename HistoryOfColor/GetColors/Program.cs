// See https://aka.ms/new-console-template for more information

using System.Drawing;
using Accord.MachineLearning;

class Program
{
    static void Main(string[] args)
    {
        DirectoryInfo dir = new("./squares");
        FileInfo[] files = dir.GetFiles();

        KMeans kmeans = new KMeans(4);

        // for (int i = 0; i < files.Length; i++)
        List<string> colorListing = new();
        for (int i = 0; i < 5; i++)
        {
            Bitmap map = new Bitmap(files[i].FullName);
            Color[] colors = new Color[map.Width * map.Height];
            for (int j = 0; j < map.Width; j++)
            {
                for (int k = 0; k < map.Height; k++)
                {
                    colors[j * map.Width + k] = map.GetPixel(j, k);
                }
            }
            
            double[][] data = colors.Select(color => new double[] { color.R, color.G, color.B }).ToArray();
            KMeansClusterCollection clusters = kmeans.Learn(data);
            double[][] centroids = clusters.Centroids;

            colorListing.AddRange(from centeriod in centroids
                let val1 = Convert.ToInt32(Math.Ceiling(centeriod[0]))
                let val2 = Convert.ToInt32(Math.Ceiling(centeriod[1]))
                let val3 = Convert.ToInt32(Math.Ceiling(centeriod[2]))
                select $"{val1:x}{val2:x}{val3:x}");
        }
        
        foreach (var colorFound in colorListing.Distinct())
        {
            Console.WriteLine(colorFound);
        }
    }
}
