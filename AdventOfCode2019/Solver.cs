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
    }
}
