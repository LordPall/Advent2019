using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xunit;
namespace AdventOfCode2019
{
    public class Solver
    {
        string baseDir = @"D:\Projects\Advent2019\inData";
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
            IntComputer newComp = new IntComputer();
            newComp.InitializeMemoryFromFile(inFile);

            // we need to find the values that give us 19690720 in address 0 after running. Reset each time.
            if(FindNounVerb(newComp))
            {
                newComp.WriteMemoryToFile(outFile);
                int val = 100 * newComp.ReadMemoryAtAddress(1) + newComp.ReadMemoryAtAddress(2);
            }
            else
            {
                Console.WriteLine("FAIL");
            }
            
            //19690720
        }
        public void SolveDayThree()
        {
            string resultsFile = "adventDayThreeSolution.txt";
            string dataFile = "adventDayThree.txt";
            string outFile = Path.Combine(baseDir, resultsFile);
            string inFile = Path.Combine(baseDir, dataFile);
            WirePanel wp = new WirePanel();
            wp.PlaceWiresFromFile(inFile, outFile);
        }
        public void SolveDayFour()
        {
            NumberGenerator numberGen = new NumberGenerator();
            numberGen.GenerateNewNumber();
        }
        public void SolveDayFive()
        {
            string resultsFile = "adventDayFiveSolution.txt";
            string dataFile = "adventDayFive.txt";
            string outFile = Path.Combine(baseDir, resultsFile);
            string inFile = Path.Combine(baseDir, dataFile);
            string[] sourceData = ReadAllLines(inFile);
            // should be one line 
            IntComputer newComp = new IntComputer();
            newComp.InitializeMemoryFromFile(inFile);
            newComp.AddInputData(5);
            newComp.RunProgram();
            newComp.WriteMemoryToFile(outFile);

        }
        public bool FindNounVerb(IntComputer newComp)
        {
            for (int intI = 0; intI < 100; intI++)
            {
                for (int intJ = 0; intJ < 100; intJ++)
                {
                    newComp.ResetMemory();
                    newComp.ReplaceMemoryAtAddress(1, intI);
                    newComp.ReplaceMemoryAtAddress(2, intJ);
                    newComp.RunProgram();
                    int result = newComp.ReadMemoryAtAddress(0);
                    if (result == 19690720)
                    {
                        // ALL DONE
                        return true;                        
                    }
                }
            }
            return false;
        }
        
    }
}
