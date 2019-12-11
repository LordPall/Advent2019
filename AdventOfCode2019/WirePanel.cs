using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace AdventOfCode2019
{
    public struct Vector2
    {
        public int x;
        public int y;
    }

    class WirePanel
    {       

        public void PlaceWiresFromFile(string inFile, string outFile)
        {            
            // get the input
            StreamReader sr = new StreamReader(inFile);
            string inLine;
            inLine = sr.ReadLine();
            Wire wireOne = new Wire(inLine);
            
            inLine = sr.ReadLine();            
            Wire wireTwo= new Wire(inLine);
            // 2 lines
            sr.Close();
            wireOne.WriteCrosspoints(wireTwo, outFile);
        }        
        

    }
    public class Wire
    {
        Vector2 wireEnd = new Vector2();
        int wireLength = 0;
        Dictionary<Vector2, int> wirePoints = new Dictionary<Vector2, int>();
        public Wire(string wireDef)            
        {
            wireEnd = new Vector2();
            wireEnd.x = 0;
            wireEnd.y = 0;
            string[] splitCommands = wireDef.Split(',');
            for(int intI =0; intI < splitCommands.Length; intI++)
            {
                ProcessMoveCommand(splitCommands[intI]);
            }
        }
        void ProcessMoveCommand(string moveString)
        {
            string dirString = moveString.Substring(0, 1);
            Helpers.DirectionEnum curDir = DirStringToEnum(dirString);
            string distanceString = moveString.Substring(1, moveString.Length - 1);
            int curDist = int.Parse(distanceString);            
            for(int intI =0; intI < curDist; intI++)
            {
                ExtendWire(curDir);
            }               
        }
        void ExtendWire(Helpers.DirectionEnum curDir)
        {
            wireEnd = Helpers.MovePos(wireEnd, curDir);
            wireLength++;
            AddPoint(wireEnd);           
        }              
        public void AddPoint(Vector2 newPos)
        {            
            if (!wirePoints.ContainsKey(newPos))
            {
                wirePoints[newPos] = wireLength;
            }            
        }
        public bool HasPoint(Vector2 checkPos)
        {
            return wirePoints.ContainsKey(checkPos);
        }
        public int GetLengthAtPoint(Vector2 checkPos)
        {
            if(HasPoint(checkPos))
            {
                return wirePoints[checkPos];
            }
            return -1;
        }

        Helpers.DirectionEnum DirStringToEnum(string dir)
        {
            if (dir == "U")
            {
                return Helpers.DirectionEnum.UP;
            }
            else if (dir == "D")
            {
                return Helpers.DirectionEnum.DOWN;
            }
            else if (dir == "L")
            {
                return Helpers.DirectionEnum.LEFT;
            }
            else if (dir == "R")
            {
                return Helpers.DirectionEnum.RIGHT;
            }
            else
            {
                // der fark
                return Helpers.DirectionEnum.VOID;
            }

        }
        public void WriteCrosspoints(Wire wireToCheck, string outFile)
        {
            StreamWriter sw = new StreamWriter(outFile);
            int shortestVal = int.MaxValue;
            foreach(KeyValuePair<Vector2, int> kvp in wirePoints)
            {
                int checkLength = wireToCheck.GetLengthAtPoint(kvp.Key);
                if(checkLength>-1)
                {
                    // it's there
                    int totalLength = checkLength + kvp.Value;
                    sw.WriteLine("Crosspoint at " + kvp.Key.x + "," + kvp.Key.y + " l1 is " + kvp.Value + " l2 is " + checkLength + " total is " + totalLength);
                    if (totalLength<shortestVal)
                    {
                        shortestVal = totalLength;
                    }                
                }
            }
            sw.WriteLine("Shortest Length is "+shortestVal);
            sw.Close();
        }
    }
}
