using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace AdventOfCode2019
{
    public class IntComputer
    {
        public int curId;
        int[] initialMemory;
        int[] memory;
        int memoryPointer;        
        runStateEnum runState;
        bool startPaused = false;
        public enum runStateEnum
        {
            READY,
            RUNNING,
            PAUSED,
            EXITED
        }
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
            runState = runStateEnum.READY;
        }
        public void ResetMemory()
        {
            memoryPointer = 0;
            memory = new int[initialMemory.Length];
            Array.Copy(initialMemory, memory, initialMemory.Length);
            inputData = new List<int>();
            outputData = new List<int>();
            runState = runStateEnum.READY;        
            if(startPaused)
            {
                StartComputer(startPaused);
            }
        }
        public void StartComputer(bool startPausedFlag)
        {
            startPaused = startPausedFlag;
            if(runState==runStateEnum.READY)
            {
                if(startPaused)
                {
                    runState = runStateEnum.PAUSED;
                }
                else
                {
                    runState = runStateEnum.RUNNING;
                }                
            }
            ContinueProgram();
        }
        public void PauseProgram()
        {
            runState = runStateEnum.PAUSED;
        }
        public void ResumeProgram()
        {
            if(runState==runStateEnum.PAUSED)
            {
                runState = runStateEnum.RUNNING;
                ContinueProgram();
            }
        }
        public void StopProgram()
        {
            runState = runStateEnum.EXITED;
        }
        public bool IsProgramCompleted()
        {
            return runState == runStateEnum.EXITED;
        }
        void ContinueProgram()
        {            
            while(runState==runStateEnum.RUNNING)
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
        public bool HasOutputData()
        {
            return outputData.Count>0;
        }

        public int ReadOutputData()
        {
            if(!HasOutputData())
            {
                return int.MinValue;
            }
            int curVal = outputData[0];
            outputData.RemoveAt(0);
            return curVal;
        }
        public void SetMemoryPointer(int newVal)
        {
            // asplodes if you do this wrong. 
            memoryPointer = newVal;
        }
        public string GetDebugOutputString()
        {
            string outLine = "OutputData = ";
            for (int intI = 0; intI < outputData.Count; intI++)
            {
                if (intI > 0)
                {
                    outLine += ",";
                }
                outLine += "[" + intI + "]=" + outputData[intI];
            }
            return outLine;
        }
        public string GetDebugInputString()
        {
            string outLine = "InputData = ";
            for(int intI = 0; intI < inputData.Count; intI++)
            {
                if(intI>0)
                {
                    outLine += ",";
                }
                outLine+="["+intI+"]=" + inputData[intI];
            }
            return outLine;
        }

    }
}
