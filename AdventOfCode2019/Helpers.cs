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

        public enum DirectionEnum
        {
            UP=1,
            RIGHT=2,
            DOWN=3,
            LEFT=4,
            VOID=5
        }
        public static Vector2 MovePos(Vector2 curPos, DirectionEnum curDir)
        {
            switch (curDir)
            {
                case DirectionEnum.DOWN:
                    curPos.y++; // this breaks wire
                    return curPos;
                case DirectionEnum.UP:
                    curPos.y--;
                    return curPos;

                case DirectionEnum.LEFT:
                    curPos.x--;
                    return curPos;

                case DirectionEnum.RIGHT:
                    curPos.x++;
                    return curPos;
                default: return curPos;
            }
        }
        public static DirectionEnum TurnLeft(DirectionEnum startDir)
        {
            switch (startDir)
            {
                case DirectionEnum.DOWN:
                    return DirectionEnum.RIGHT;
                case DirectionEnum.UP:
                    return DirectionEnum.LEFT;
                case DirectionEnum.LEFT:
                    return DirectionEnum.DOWN;                    
                case DirectionEnum.RIGHT:
                    return DirectionEnum.UP; 
                default: return DirectionEnum.VOID;
            }
        }
        public static DirectionEnum TurnRight(DirectionEnum startDir)
        {
            switch (startDir)
            {
                case DirectionEnum.DOWN:
                    return DirectionEnum.LEFT;
                case DirectionEnum.UP:
                    return DirectionEnum.RIGHT;
                case DirectionEnum.LEFT:
                    return DirectionEnum.UP;
                case DirectionEnum.RIGHT:
                    return DirectionEnum.DOWN; 
                default:
                    return DirectionEnum.VOID;
            }
        }

    }
}
