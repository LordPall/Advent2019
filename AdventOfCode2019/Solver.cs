using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xunit;
namespace Advent
{
    public class Solver
    {
        string baseDir = @"c:\Projects\Advent\inData";
        // helper stuffs
        string[] ReadAllLines(string inFile)
        {
            StreamReader sr = new StreamReader(inFile);
            string inLine;
            List<string> readStrings = new List<string>();
            while ((inLine = sr.ReadLine()) != null)
            {
                readStrings.Add(inLine);

            }
            sr.Close();
            return readStrings.ToArray();

        }
        public void SolveDayOne()
        {
            /*    Fuel required to launch a given module is based on its mass.
                    Specifically, to find the fuel required for a module, take its mass,
                    divide by three, round down, and subtract 2.*/

            string resultsFile = "adventDayOneSolution.txt";
            string dataFile = "adventDayOne.txt";
            string outFile = Path.Combine(baseDir, resultsFile);
            string inFile = Path.Combine(baseDir, dataFile);
            string[] sourceData = ReadAllLines(inFile);
            int fuelRequired = 0;
            foreach(string sourceNumber in sourceData)
            {
                // asplode if this is forked. 
                int massVal = int.Parse(sourceNumber);
                fuelRequired += GetRequiredFuelForMass(massVal);
            }
            StreamWriter sw = new StreamWriter(outFile);
            sw.WriteLine("FUEL REQUIRED : " + fuelRequired);
            sw.Close();
            
        }
        public int GetRequiredFuelForMass(int inMass)
        {
            bool finishedMass = false;
            int fuelRequired = 0;
            while(!finishedMass)
            {
                inMass = inMass / 3;
                inMass = inMass - 2;
                if (inMass <= 0)
                {
                    // we done!
                    finishedMass = true;
                }
                else
                {
                    fuelRequired += inMass;
                }
            }
            return fuelRequired;
        }
        [Theory]
        [InlineData(12, 2)]
        [InlineData(14, 2)]
        [InlineData(1969, 966)]
        [InlineData(100756, 50346)]
        public void TestGetRequiredFuelForMass(int testMass, int expectedFuel)
        {
            Solver coreSolver = new Solver();
            int result = coreSolver.GetRequiredFuelForMass(testMass);
            Assert.Equal(expectedFuel, result);
        }
        public void SolveDayTwo()
        {
            string resultsFile = "adventDayTwoSolution.txt";
            string dataFile = "adventDayTwo.txt";
            string outFile = Path.Combine(baseDir, resultsFile);
            string inFile = Path.Combine(baseDir, dataFile);
            string[] sourceData = ReadAllLines(inFile);
            // should be one line 
            string[] splitVals = sourceData[0].Split(',');
            int[] intComp = new int[splitVals.Length];
            for(int intI = 0; intI < intComp.Length; intI++)
            {
                intComp[intI] = int.Parse(splitVals[intI]);
            }
            // replace the parameters to get back to original run
            intComp[1] = 12;
            intComp[2] = 2;
            ProcessOpCode(ref intComp);
            string outLine = "";
            for (int intI = 0; intI < intComp.Length; intI++)
            {
                if(intI>0)
                {
                    outLine += ",";
                }
                outLine += intComp[intI];

            }
            StreamWriter sw = new StreamWriter(outFile);
            sw.WriteLine(outLine);
            sw.Close();

        }
        public void ProcessOpCode(ref int[] intComp)
        {
            int curPos = 0;
            bool finishedRunning = false;
            while (!finishedRunning)
            {
                int curCode = intComp[curPos];
                if(curCode==99)
                {
                    return;
                    // done
                }
                else if(curCode==1)
                {
                    curPos = ProcessAddOpCode(ref intComp, curPos);
                }
                else if(curCode==2)
                {
                    curPos = ProcessMultiplyOpCode(ref intComp, curPos);
                }
                else
                {
                    // DER FARK?
                    break;
                }
            }

        }
        // moar complicated
        int ProcessAddOpCode(ref int[] intComp, int curPos)
        {

            int newVal = 0;
            curPos++;
            newVal  = intComp[intComp[curPos]];
            curPos++;
            newVal += intComp[intComp[curPos]];
            curPos++;
            intComp[intComp[curPos]] = newVal;
            curPos++;
            return curPos;
            // setup is as follows
            // insturctyion, number1, number 2, destination

        }
        int ProcessMultiplyOpCode(ref int[] intComp, int curPos)
        {
            int newVal = 0;
            curPos++;
            newVal = intComp[intComp[curPos]];
            curPos++;
            newVal  = newVal * intComp[intComp[curPos]];
            curPos++;
            intComp[intComp[curPos]] = newVal;
            curPos++;
            return curPos;
        }
    }
}
