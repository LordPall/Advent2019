using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace AdventOfCode2019
{
    public class IntComputer
    {
        
        int[] initialMemory;
        int[] memory;
        int memoryPointer;
        bool isRunning = false;
        public void InitializeMemoryFromFile(string inFile)
        {
            StreamReader sr = new StreamReader(inFile);
            string inLine;
            inLine = sr.ReadLine();
            sr.Close();
            InitializeMemory(inLine);
        }
        public void InitializeMemory(string inData)
        {       
            // should be one line 
            string[] splitVals = inData.Split(',');
            int[] intComp = new int[splitVals.Length];
            for(int intI = 0; intI < intComp.Length; intI++)
            {
                intComp[intI] = int.Parse(splitVals[intI]);
            }
            memory = intComp;
            initialMemory = new int[memory.Length];
            Array.Copy(memory, initialMemory, memory.Length);
            initialMemory = memory;
            memoryPointer = 0;            
        }
        public void ResetMemory()
        {
            memoryPointer = 0;
            memory = new int[initialMemory.Length];
            Array.Copy(initialMemory, memory, initialMemory.Length);
        }
        public void StopProgram()
        {
            isRunning = false;
        }
        public void RunProgram()
        {
            isRunning = true;
            while(isRunning)
            {
                OpCode opCode = OpCode.ReadNextOpCode(this);
                opCode.RunOpCode(this);
            }
        }
        public int ReadNextMemoryAddress()
        {
            int retVal = memory[memoryPointer];
            memoryPointer++;
            return retVal;
        }
        public int ReadMemoryAtAddress(int address)
        {
            return memory[address];
        }        
        public void ReplaceMemoryAtAddress(int address, int newVal)
        {
            memory[address] = newVal;
        }
        
        public void WriteMemoryToFile(string outFile)
        {
            string outLine = "";
            
            for (int intI = 0; intI < memory.Length; intI++)
            {
                if (intI > 0)
                {
                    outLine += ",";
                }
                outLine += memory[intI];
            }
            StreamWriter sw = new StreamWriter(outFile);
            sw.WriteLine(outLine);
            outLine = "Output Data: ";
            for(int intI = 0; intI < outputData.Count; intI++)
            {
                if (intI > 0)
                {
                    outLine += ",";
                }
                outLine += outputData[intI];
            }

            sw.WriteLine(outLine);
            sw.Close();
        }
        public List<int> outputData = new List<int>();
        public List<int> inputData = new List<int>();
        public int ReadNextInput()
        {
            int curVal = inputData[0];
            inputData.RemoveAt(0);
            return curVal;
        }
        public void AddInputData(int val)
        {
            inputData.Add(val);
        }
        public void WriteOutputData(int newVal)
        {
            outputData.Add(newVal);
        }
        public void SetMemoryPointer(int newVal)
        {
            // asplodes if you do this wrong. 
            memoryPointer = newVal;
        }
        


    }
}
