using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    public class Helpers
    {
        public static int GetIndexFromCoordinate(Vector2 coordinate, int xSize)
        {
            // y*xwidth +x
            int retVal = coordinate.y * xSize;
            retVal += coordinate.x;
            return retVal;
        }
        public static Vector2 GetCoordinatesFromIndex(int curIndex, int xSize)
        {
            Vector2 retVal = new Vector2();
            retVal.y = curIndex / xSize; ;
            retVal.x = curIndex - (retVal.y * xSize);
            return retVal;
        }
        public static int GetIndexFromCoordinate(int xCoord, int yCoord, int xSize)
        {
            Vector2 scratchVal = new Vector2();
            scratchVal.x = xCoord;
            scratchVal.y = yCoord;
            return GetIndexFromCoordinate(scratchVal, xSize);
        }
    }
}
