using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdventOfCode2019
{
    public class SolverTests
    {

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
        //new object[] { new string[] { "2000-01-02", "2000-02-01" } }
        [Theory]
        [InlineData(12, new int[] { 0, 0, 0, 1, 2 })]
        public void TestAmplifierPhaseSet(int phaseVal, int[] expectedPhase)
        {
            string testProgram = "3,8,1001,8,10,8,105,1,0,0,21,30,51,76,101,118,199,280,361,442,99999,3,9,102,5,9,9,4,9,99,3,9,102,4,9,9,1001,9,3,9,102,2,9,9,101,2,9,9,4,9,99,3,9,1002,9,3,9,1001,9,4,9,102,5,9,9,101,3,9,9,1002,9,3,9,4,9,99,3,9,101,5,9,9,102,4,9,9,1001,9,3,9,1002,9,2,9,101,4,9,9,4,9,99,3,9,1002,9,2,9,1001,9,3,9,102,5,9,9,4,9,99,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,99,3,9,1001,9,1,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,102,2,9,9,4,9,99,3,9,1001,9,1,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,99,3,9,1001,9,1,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,1001,9,1,9,4,9,3,9,1002,9,2,9,4,9,99,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,2,9,4,9,99";
            Amplifiers curAmps = new Amplifiers(5, testProgram);
            curAmps.SetPhaseSettings(phaseVal);
            for (int intI = 0; intI < 5; intI++)
            {
                Assert.Equal(curAmps.GetPhase(intI), expectedPhase[intI]);
            }
        }

        [Theory]
        [InlineData(25, 25, 13, 12, 313)]
        public void TestIndexFromCoordinates(int xSize, int ySize, int xCoord, int yCoord, int expectedIndex)
        {

            LayeredImage li = new LayeredImage(xSize, ySize);
            Vector2 vec = new Vector2();
            vec.x = xCoord;
            vec.y = yCoord;
            int retVal = li.GetIndexFromCoordinate(vec);
            Assert.Equal(retVal, expectedIndex);


        }
        [Theory]
        [InlineData(25, 25, 313, 13, 12)]
        public void TestCoordinateFromIndex(int xSize, int ySize, int curIndex, int expectedX, int expectedY)
        {

            LayeredImage li = new LayeredImage(xSize, ySize);
            Vector2 retVal = li.GetCoordinatesFromIndex(curIndex);
            Assert.Equal(expectedX, retVal.x);
            Assert.Equal(expectedY, retVal.y);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void TestDayNineSampleData(int sampleIndex)
        {
            IntComputer curComp = new IntComputer();
            curComp.InitializeMemory(GetSampleData(sampleIndex));
            curComp.StartComputer(false);
            long outVal = curComp.ReadOutputData();
            long expectedRetVal = GetExpectedResults(sampleIndex);
            Assert.Equal(outVal, expectedRetVal);
        }
        [Theory]
        [InlineData(21102, new int[] {1, 1, 2, 2})]   
        public void TestOpCodeInstructionParse(int startVal, int[] expectedResults)
        {
            OpCodeInstruction curInst = new OpCodeInstruction(21102);
            // parammodes, then instruction
            Assert.Equal(expectedResults[0], (int)curInst.GetParamMode(0));
            Assert.Equal(expectedResults[1], (int)curInst.GetParamMode(1));
            Assert.Equal(expectedResults[2], (int)curInst.GetParamMode(2));
            Assert.Equal(expectedResults[3], (int)curInst.GetInstruction());



        }


        public static string GetSampleData(int sampleIndex)
        {
            string[] sampleData =
            {
                
                "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", // copy of itself as output? 
                "1102,34915192,34915192,7,4,7,99,0", // 1219070632396864 expect 16 digit numbver
                "104,1125899906842624,99", // expect 1125899906842624
                "00203,0"
            };

            return sampleData[sampleIndex];
        }
        public static long GetExpectedResults(int sampleIndex)
        {
            long[] expectedResults =
{
                109,
                1219070632396864,
                1125899906842624
            };
            return expectedResults[sampleIndex];
        }
    }
}