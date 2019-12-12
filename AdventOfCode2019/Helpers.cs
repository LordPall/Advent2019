using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    public struct Vector3
    {
        public int x;
        public int y;
        public int z;
        public Vector3(int startX, int startY, int startZ)
        {
            x = startX;
            y = startY;
            z = startZ;
        }
        public Vector3(string sourcedata)
        {
            //<x=17, y=5, z=1>            
            sourcedata = sourcedata.Replace("<", "");
            sourcedata = sourcedata.Replace(">", "");
            string[] splitVals = sourcedata.Split(',');
            // second time
            string[] tempSplit = splitVals[0].Split('=');
            // tempSplit 1 = x
            x = int.Parse(tempSplit[1]);
            
            tempSplit = splitVals[1].Split('=');            
            y = int.Parse(tempSplit[1]);

            tempSplit = splitVals[2].Split('=');
            z = int.Parse(tempSplit[1]);
           


        }
        public bool Add(Vector3 toAdd)
        {        
            
            x = x + toAdd.x;
            y = y + toAdd.y;
            z = z + toAdd.z;
            return (toAdd.x == 0) && (toAdd.y == 0) && (toAdd.z == 0);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public void Invert()
        {
            x = x * -1;
            y = y * -1;
            z = z * -1;
        }
        public override string ToString()
        {
            return String.Format("{0},{1},{2}", x, y, z);
        }
    }
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
        public static long LCM(long[] numbers)
        {
            return numbers.Aggregate(lcm);
        }
        public static long lcm(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
        public static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }


    }
}
