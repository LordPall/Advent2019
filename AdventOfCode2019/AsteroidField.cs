using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace AdventOfCode2019
{
    class AsteroidField
    {
        public enum AsteroidContentEnum
        { 
            ASTEROID,
            NOTHING            
        }

        public struct angleCheckStruct
        {
            public Vector2 asteroidLocation;
            public double angle;
            public double distance;
        }
        
        
        public int visibleIndex = -1;
        public int visibleCount = int.MinValue;
        public struct CellContentsStruct
        {
            public char curChar;            
            public Dictionary<Vector2, angleCheckStruct> curAngles;            
            public Dictionary<double, List<angleCheckStruct>> angleDistance;
        }
        int xSize = 0;
        int ySize = 0;
        public CellContentsStruct[] asteroidField;

        public AsteroidField(string[] sourceData)
        {
            ySize = sourceData.Length;
            xSize = sourceData[0].Length;
            asteroidField = new CellContentsStruct[xSize * ySize];
            
            for(int intJ = 0; intJ < sourceData.Length; intJ++)
            {
                for(int intI =0;intI < sourceData[intJ].Length; intI++)
                {
                    CellContentsStruct curStruct = new CellContentsStruct();
                    curStruct.curChar = sourceData[intJ][intI];
                    asteroidField[(intJ * xSize) + intI] = curStruct;
                }
            }
            int curMax = int.MinValue; ;
            

            for (int intI = 0; intI < asteroidField.Length; intI++)
            {
                if (asteroidField[intI].curChar == '.')
                {
                    continue;
                }
                Vector2 startVec = new Vector2();
                startVec = Helpers.GetCoordinatesFromIndex(intI, xSize);
                asteroidField[intI].curAngles = new Dictionary<Vector2, angleCheckStruct>();
                asteroidField[intI].angleDistance = new Dictionary<double, List<angleCheckStruct>>();                
                for(int intJ = 0; intJ < asteroidField.Length; intJ++)
                {
                    if(intI!=intJ) // skip if it's us
                    {
                        if(asteroidField[intJ].curChar!='.')
                        {
                            Vector2 endVec = Helpers.GetCoordinatesFromIndex(intJ, xSize);
                            angleCheckStruct curAngleData = GetAngleCheckStruct(startVec, endVec);
                            asteroidField[intI].curAngles[endVec] = curAngleData;
                            if(asteroidField[intI].angleDistance.ContainsKey(curAngleData.angle))
                            {                                
                                asteroidField[intI].angleDistance[curAngleData.angle].Add(curAngleData);
                            }
                            else
                            {
                                List<angleCheckStruct> newAngleList = new List<angleCheckStruct>();
                                newAngleList.Add(curAngleData);
                                asteroidField[intI].angleDistance.Add(curAngleData.angle, newAngleList);
                            }                            
                        }
                    }                        
                }

                /*int visibleAsteroids = asteroidField[intI].angleDistance.Count;
                if(visibleAsteroids> visibleCount)
                {
                    visibleCount= visibleAsteroids;
                    visibleIndex= intI;
                }*/
            }
            for(int intI =0;intI < asteroidField.Length; intI++)
            {
                if(asteroidField[intI].curChar=='#')
                {
                    if(asteroidField[intI].angleDistance.Count>visibleCount)
                    {
                        visibleCount = asteroidField[intI].angleDistance.Count;
                        visibleIndex = intI;
                    }
                }
            }
        }
        public void StartVaporizing(string outFile, int numToVaporize)
        {

            
            CellContentsStruct asteroidBase = asteroidField[visibleIndex];
            StreamWriter sw = new StreamWriter(outFile);
            Vector2 bestSpot = Helpers.GetCoordinatesFromIndex(visibleIndex, xSize);
            string outLine = "";
            outLine = "VISIBLE INDEX at " + bestSpot.x + "," + bestSpot.y + " can see " + visibleCount + " asteroids";
            sw.WriteLine(outLine);
            outLine = "";
            List<double> destroyedAsteroids = new List<double>();
            int numVaporized=0;
            bool doneDestroying = false;
            while (!doneDestroying)
            {
                destroyedAsteroids = new List<double>();
                foreach (KeyValuePair<double, List<angleCheckStruct>> kvp in asteroidBase.angleDistance.OrderBy(i => i.Key))
                {
                    numVaporized++;
                    int removedIndex = GetLowestDistanceIndex(kvp.Value);
                    outLine = "#" + numVaporized + " from " + bestSpot.x + "," + bestSpot.y + " Angle is " + kvp.Value[removedIndex].angle;
                    outLine = outLine + "Location is " + kvp.Value[removedIndex].asteroidLocation.x + "," + kvp.Value[removedIndex].asteroidLocation.y + " remaing on line " + (kvp.Value.Count - 1);
                    sw.WriteLine(outLine);
                    outLine = "";                    
                    int removeIndex = Helpers.GetIndexFromCoordinate(kvp.Value[removedIndex].asteroidLocation, xSize);
                    int curNum = numVaporized % 10;
                    char repChar = curNum.ToString()[0];
                    //asteroidField[removeIndex].curChar = repChar;
                    asteroidField[removeIndex].curChar = '.';
                    destroyedAsteroids.Add(kvp.Key);
                    if (numVaporized >= numToVaporize)
                    {
                        doneDestroying = true;
                        break;
                    }
                }
                WriteAsteroidField(sw);
                foreach(double angle in destroyedAsteroids)
                {
                    // cleanup dictionary;
                    if(asteroidBase.angleDistance[angle].Count>1)
                    {
                        int indexToRemove = GetLowestDistanceIndex(asteroidBase.angleDistance[angle]);
                        asteroidBase.angleDistance[angle].RemoveAt(indexToRemove);
                    }
                    else
                    {
                        asteroidBase.angleDistance.Remove(angle);
                    }
                }
            }
            sw.Close();

        }
        public void WriteAsteroidField(StreamWriter sw)
        {
            sw.WriteLine("----------------ASTEROID STAGE-------------------------");
            string outLine = "";
            int curIndex = 0; ;
            for (int intI = 0; intI < asteroidField.Length; intI++)
            {
                outLine += asteroidField[intI].curChar;
                curIndex++;
                if (curIndex >= xSize)
                {
                    sw.WriteLine(outLine);
                    outLine = "";
                    curIndex = 0;
                }
            }
            sw.WriteLine("END----------------ASTEROID STAGE-------------------------");
        }

        int GetLowestDistanceIndex(List<angleCheckStruct> curList)
        {
            int retVal = -1;
            double lowestDist = double.MaxValue;
            for(int intI =0; intI< curList.Count; intI++)
            {
                if(curList[intI].distance<lowestDist)
                {
                    retVal = intI;
                    lowestDist = curList[intI].distance;
                }
            }
            return retVal;
        }
        public double GetAngleForVectors(Vector2 startVec, Vector2 endVec)
        {
            double yDiff = endVec.y - startVec.y;
            double xDiff = endVec.x - startVec.x;
            double angle = Math.Atan2(yDiff, xDiff);
            if (angle < 0) 
            { 
                angle += 2 * Math.PI; 
            }
            angle = angle * 180 / Math.PI;
            angle = RotateAngle(angle, 90);
            return angle;
        }
        public double RotateAngle(double angle, double amount)
        {
            angle += amount;
            if(angle<0)
            {
                angle = 360 - angle;
            }
            if(angle>=360)
            {
                angle = angle - 360;
            }
            return angle;
        }
        public double GetDistance(Vector2 startVec, Vector2 endVec)
        {
            double yDiff = endVec.y - startVec.y;
            double xDiff = endVec.x - startVec.x;
            double retVal = Math.Sqrt((yDiff * yDiff) + (xDiff * xDiff));
            return retVal;
        }
        public angleCheckStruct GetAngleCheckStruct(Vector2 startVec, Vector2 endVec)
        {
            angleCheckStruct curStruct = new angleCheckStruct();
            curStruct.angle = GetAngleForVectors(startVec, endVec);
            curStruct.distance = GetDistance(startVec, endVec);
            curStruct.asteroidLocation = endVec; 
            return curStruct;
        }
        public void WriteDebugData(Vector2 targetPoint, string outFile)
        {
            StreamWriter sw = new StreamWriter(outFile);
            Vector2 bestSpot = Helpers.GetCoordinatesFromIndex(visibleIndex, xSize);
            string outLine = "";
            outLine = "VISIBLE INDEX at " + bestSpot.x + "," + bestSpot.y + " can see " + visibleCount + " asteroids";
            sw.WriteLine(outLine);
            int curIndex = Helpers.GetIndexFromCoordinate(targetPoint, xSize);            
            
            outLine = "";
            /*foreach (KeyValuePair<Vector2, angleCheckStruct> kvp in asteroidField[curIndex].curAngles)
            {
                int dataIndex = Helpers.GetIndexFromCoordinate(kvp.Key, xSize);
                outLine +="["+kvp.Value.angle+":"+ kvp.Value.distance + "]";
                
                curLength++;
                if(curLength>=xSize)
                {
                    sw.WriteLine(outLine);
                    outLine = "";
                    curLength = 0;
                }
            }*/
            sw.Close();
            
        }
    }
}
