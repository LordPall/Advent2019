using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

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
            newComp.StartComputer(false);
            newComp.WriteMemoryToFile(outFile);

        }
        public void SolveDaySix()
        {
            string resultsFile = "adventDaySixSolution.txt";
            string dataFile = "adventDaySix.txt";
            string outFile = Path.Combine(baseDir, resultsFile);
            string inFile = Path.Combine(baseDir, dataFile);
            string[] sourceData = ReadAllLines(inFile);

            OrbitalMap om = new OrbitalMap();
            om.ReadSourceData(sourceData);
        }
        public void SolveDayEight()
        {
            string resultsFile = "adventDayEightSolution.png";
            string dataFile = "adventDayEight.txt";
            string outFile = Path.Combine(baseDir, resultsFile);
            string inFile = Path.Combine(baseDir, dataFile);
            string[] sourceData = ReadAllLines(inFile);
            int width = 25;
            int height = 6;
            LayeredImage li = new LayeredImage(width, height);
            li.ImportImageData(sourceData[0]);
            //int ret = li.ValidateImageLayers();
            li.DrawImage(outFile);
            
        }
        public void SolveDaySeven()
        {
            string resultsFile = "adventDaySevenSolution.txt";
            string dataFile = "adventDaySeven.txt";
            string outFile = Path.Combine(baseDir, resultsFile);
            string inFile = Path.Combine(baseDir, dataFile);
            string[] sourceData = ReadAllLines(inFile);
            int endVal = 44444;
            int curMax = int.MinValue;
            Amplifiers curAmps = new Amplifiers(5, sourceData[0]);
            
            
            // base 5 number  ?
            int nextVal = 0;
            int nextPhase = 0;
            List<int> phasesRun = new List<int>();
            List<int> results = new List<int>();
            List<string> generatedNumbers = new List<string>();
            char[] digits = new char[] { '5', '6', '7', '8', '9' };
            int numDigits = 5;
            int maxPhase = 0;
            Solver.GeneratePermutations(digits, numDigits, "", numDigits, ref generatedNumbers);
            string outLine = "";
            StreamWriter sw = new StreamWriter(outFile);
            
            
            foreach(string curNumString in generatedNumbers)
            {
                if(HasDuplicateChars(curNumString))
                {
                    continue; 
                }
                int curPhase = int.Parse(curNumString);
                curAmps.ResetAmplifiers();                
                curAmps.SetPhaseSettings(curPhase);                
                phasesRun.Add(curPhase);
                sw.WriteLine("BEGINNING AMP RUN FOR PHASE " + curPhase);
                nextVal = curAmps.RunAmplifiers(sw);
                results.Add(nextVal);                
                if (nextVal>curMax)
                {
                    curMax = nextVal;
                    maxPhase = curPhase;
                }
                sw.WriteLine("--------------END RUN END RUN------------");
            }
            sw.WriteLine("FOR " + maxPhase + " value is " + curMax);           
            sw.Close();
            int T = 9;

      
            
        }
        // horky but i'm irritated
        bool HasDuplicateChars(string inString)
        {
            HashSet<char> curChars = new HashSet<char>();
            foreach(char curChar in inString)
            {
                if(curChars.Contains(curChar))
                {
                    return true;
                }
                curChars.Add(curChar);
            }
            return false;
            
        }
        public static void GenerateVariants(string startString, string curString, int curIndex, ref List<string> generatedStrings)
        {

            for(int intI =curIndex; intI < startString.Length; intI++)
            {
                curString += startString[intI];
                
            }
        }
        public static void GeneratePermutations(char[] sourceChars, int stringLength, string generatedString , int curGeneratedIndex, ref List<string> generatedPermutions)
        {
            if(curGeneratedIndex==0)
            {
                // we're done generating, add and return;
                generatedPermutions.Add(generatedString);
                return;
            }
            for(int intI =0; intI < sourceChars.Length; intI++)
            {
                string newGeneratedString = generatedString + sourceChars[intI];
                GeneratePermutations(sourceChars, stringLength, newGeneratedString, curGeneratedIndex - 1, ref generatedPermutions);
            }
        }


/*public static void GeneratePermutations(char[] digits,String curVal, int curCount, int curDigit, ref  List<string> generatedNumbers)
{                        
    if (curDigit == 0)
    {
        //Console.WriteLine(prefix);
        // full number                
        generatedNumbers.Add(curVal);
        return;
    }                        
    for(int intI =0; intI < curCount; intI++)
    {

        // Next character of input added 
        string newVal = curVal + digits[intI];
        // k is decreased, because  
        // we have added a new character 
        Solver.GeneratePermutations(digits, newVal, intI, curDigit - 1, ref generatedNumbers);
    }
}*/


        public static int GetNextNumberInSystem(int curNum, int maxDigit)        
        {
            if (maxDigit> 9)
            {
                throw new Exception("Fucked, not supported more than 9");
            }
            
            // MAKE IT INTO A list SINCE IT'S EASIER;
            string curNumString = curNum.ToString();
            List<int> numAsList = new List<int>(curNumString.Length);
            for(int intI =0; intI <curNumString.Length; intI++)
            {
                numAsList.Add(0);
            }
            for (int intI = 0; intI < curNumString.Length; intI++)
            {
                numAsList[intI] = int.Parse("" + curNumString[intI]);
            }

            int remainder = 0;
            numAsList[0]++;
            if(numAsList[0]>maxDigit)
            {
                remainder = 1;
                numAsList[0] = maxDigit;
            }
            int curIndex = 1;
            while(remainder>0)
            {
                if (numAsList.Count - 1 < curIndex)
                {
                    // add one 
                    numAsList.Add(0);
                }
                remainder = 0;
                numAsList[curIndex]++;
              
                if(numAsList[curIndex]>maxDigit)
                {
                    numAsList[curIndex] = maxDigit;
                    remainder = 1;
                }
                curIndex++;
            }
            string retValString = "";
            foreach(int curInt in numAsList)
            {
                retValString = retValString+curInt.ToString();
            }
            
            int retVal = int.Parse(retValString);
            return retVal;
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
                    newComp.StartComputer(false);
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
