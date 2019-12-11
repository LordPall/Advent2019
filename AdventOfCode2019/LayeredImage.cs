using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace AdventOfCode2019
{
    public class LayeredImage
    {
        List<int[]> imageLayers = new List<int[]>();

        public int xSize, ySize;
        public LayeredImage(int width, int height)
        {
            xSize = width;
            ySize = height;            
        }
        public void ImportImageData(string curLine)
        {
            int[] curLayer = new int[xSize * ySize];
            
            int numLayers = curLine.Length / (xSize * ySize);

            for(int intI = 0; intI < numLayers; intI++)
            {
                for (int intX = 0; intX < xSize; intX++)
                {
                    for (int intY = 0; intY < ySize; intY++)
                    {
                        int globalIndex = (xSize * ySize) * intI;
                        int localIndex = GetIndexFromCoordinate(intX, intY);
                        
                        char curChar = curLine[globalIndex+localIndex];
                        int curVal = int.Parse((string)""+curChar);
                        curLayer[localIndex] = curVal;
                    }
                }
                imageLayers.Add(curLayer);
                curLayer = new int[xSize * ySize];
            }
        }
        public int GetIndexFromCoordinate(int xCoord, int yCoord)
        {
            return Helpers.GetIndexFromCoordinate(xCoord, yCoord, xSize);
        }

        public int GetIndexFromCoordinate(Vector2 coordinate)
        {
            return Helpers.GetIndexFromCoordinate(coordinate, xSize);
            // y*xwidth +x
        }
        public Vector2 GetCoordinatesFromIndex(int curIndex )
        {
            return Helpers.GetCoordinatesFromIndex(curIndex, xSize);
        }
        int CountValuesInImage(int[] curLayer, int checkVal)
        {
            int retVal = 0;
            for (int intI = 0; intI < curLayer.Length; intI++)
            {
                if (curLayer[intI] == checkVal)
                {
                    retVal++;
                }
            }
            return retVal;
        }

        public int ValidateImageLayers()
        {
            int minZeroes = int.MaxValue;
            int curCount = 0;
            int minLayer = -1;
            for (int intI = 0; intI < imageLayers.Count; intI++)
            {
                int[] curLayer = imageLayers[intI];
                curCount = CountValuesInImage(curLayer, 0);
                if (curCount < minZeroes)
                {
                    minLayer = intI;
                    minZeroes = curCount;
                }
            }
            int[] targetLayer = imageLayers[minLayer];
            int numOnes = CountValuesInImage(targetLayer, 1);
            int numTwos = CountValuesInImage(targetLayer, 2);
            return numOnes * numTwos;
        }
        enum ColorEnum
        {
            BLACK = 0,
            WHITE=1,            
            TRANSPARENT=2
        }
        ColorEnum GetPixelToDraw(ColorEnum topColor, ColorEnum bottomColor)
        {
            
            if(topColor!=ColorEnum.TRANSPARENT)
            {
                return topColor;
            }
            return bottomColor;

        }

        ColorEnum[] GetPixelColors()
        {
            ColorEnum[] pixelsToDraw = new ColorEnum[xSize * ySize];            
            for(int intI = 0; intI < xSize*ySize; intI++)
            {
                ColorEnum visiblePixel = ColorEnum.TRANSPARENT; // lower pixel technically.
                for (int intJ = imageLayers.Count-1; intJ>=0; intJ--)
                {
                    ColorEnum curPixel = (ColorEnum)imageLayers[intJ][intI];
                    visiblePixel = GetPixelToDraw(curPixel, visiblePixel);
                }
                pixelsToDraw[intI] = visiblePixel;
            }
            return pixelsToDraw;
        }
        public void DrawImage(string fileName)
        {

            Bitmap bm = new Bitmap(xSize, ySize);

            //bitmap.Save(@"C:\Users\johndoe\test.png", ImageFormat.Png);
            // Draw myBitmap to the screen.
            // Set each pixel in myBitmap to black.
            ColorEnum[] pixelsToDraw = GetPixelColors();
            for(int intX = 0; intX< xSize; intX++)
            {
                for(int intY = 0; intY < ySize; intY++)
                {
                    int curIndex = GetIndexFromCoordinate(intX, intY);
                    if(pixelsToDraw[curIndex]==ColorEnum.BLACK)
                    {
                        bm.SetPixel(intX, intY, Color.Black);
                        //myBitmap.SetPixel(Xcount, Ycount, Color.Black);
                    }
                    else if(pixelsToDraw[curIndex]==ColorEnum.WHITE)
                    {
                        bm.SetPixel(intX, intY, Color.White);
                    }
                    else
                    {
                        // the fark?
                    }
                }
            }
            bm.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }

    }
}
