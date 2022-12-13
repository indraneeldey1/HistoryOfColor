// See https://aka.ms/new-console-template for more information

using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

class Program
{
    static void Main(string[] args)
    {
        
        
        Mat pic = CvInvoke.Imread("image/young_woman_with_a_butterfly_1980.62.40.jpg");


        Size ksize = new(3, 3);

        Mat gaussian = new();
       
        Gray avg = pic.ToImage<Gray, Byte>().GetAverage();

        double lowerThresh = Math.Max(0, 0.66 * avg.Intensity);
        double upperThresh = Math.Max(255, 1.33 * avg.Intensity);
        CvInvoke.GaussianBlur(pic, gaussian, ksize, lowerThresh, upperThresh, BorderType.Constant);

        
        //
        // Mat sobelX = new(), sobelY = new(), sobelAdd = new();
        //
        // CvInvoke.Sobel(gaussian, sobelX, DepthType.Default, 1, 0);
        // CvInvoke.Sobel(gaussian, sobelY, DepthType.Default, 0, 1);
        //
        // CvInvoke.Add(sobelX, sobelY, sobelAdd, null, DepthType.Default);
        //
        // ksize = new(7, 7);
        // CvInvoke.GaussianBlur(sobelAdd, gaussian, ksize, lowerThresh, upperThresh, BorderType.Constant);
        //
        // Mat lap = new();
        // CvInvoke.Laplacian(sobelAdd, lap, DepthType.Default, 1);
        //
        // Mat sobelXy = new();
        // CvInvoke.Sobel(lap, sobelXy, DepthType.Default, 1, 1);
        
        Mat canny = new();
        CvInvoke.Canny(pic, canny, lowerThresh, upperThresh, 5);

        // CvInvoke.Imshow("gaussian",gaussian);
        // CvInvoke.Imshow("SobelAdd", sobelAdd);
        // CvInvoke.Imshow("contours", lap);
        Mat threshhold = new();
        Image<Gray, byte>? gray = gaussian.ToImage<Gray, Byte>();
        CvInvoke.Threshold(gray, threshhold, 210, 255, ThresholdType.Binary);
        
        //CvInvoke.Imshow("canny", threshhold);
        threshhold.Save("./image/threshold.jpg");
        canny.Save("./image/canny.jpg");
        gray.Save("./image/gray.jpg");
        CvInvoke.WaitKey();
    }
}