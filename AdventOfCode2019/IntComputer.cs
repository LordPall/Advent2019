using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace AdventOfCode2019
{
    public class IntComputer
    {
        public int curId;
        int relativeBase = 0;

        long[] initialMemory;
        long[] memory;
        public List<long> outputData = new List<long>();
        public List<long> inputData = new List<long>();

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
            initialMemory = new long[splitVals.Length];
            for(int intI = 0; intI < initialMemory.Length; intI++)
            {
                initialMemory[intI] = long.Parse(splitVals[intI]);
            }
            memory = new long[initialMemory.Length];
            Array.Copy(initialMemory, memory, memory.Length);            
            memoryPointer = 0;
            relativeBase = 0;
            runState = runStateEnum.READY;
        }
        public void ResetMemory()
        {
            memoryPointer = 0;
            memory = new long[initialMemory.Length];
            Array.Copy(initialMemory, memory, initialMemory.Length);
            inputData = new List<long>();
            outputData = new List<long>();
            runState = runStateEnum.READY;
            relativeBase = 0;
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
        public long ReadNextMemoryAddress()
        {
            long retVal = memory[memoryPointer];
            memoryPointer++;
            return retVal;
        }        
        public void CheckForMemoryExpansion(int curAddress)
        {
            if(memory.Length-1<curAddress)
            {
                long[] newMemory = new long[memory.Length * 2];
                while(newMemory.Length<curAddress)
                {
                    newMemory = new long[newMemory.Length * 2];
                }
                Array.Copy(memory, 0, newMemory, 0, memory.Length);
                memory = newMemory;
            }
            // attempting to read or write to an address beyond the end of the array doubles the size 
        }
        
        public long ReadMemoryAtAddress(int address)
        {
            CheckForMemoryExpansion(address);
            return memory[address];
        }        
        public void ReplaceMemoryAtAddress(int address, long newVal)
        {
            CheckForMemoryExpansion(address);
            memory[address] = newVal;
        }
        public void ReplaceMemoryAtAddress(long address, long newVal)
        {
            ReplaceMemoryAtAddress((int)address, newVal);
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
        public long ReadNextInput()
        {
            long curVal = inputData[0];
            inputData.RemoveAt(0);
            return curVal;
        }
        public void AddInputData(long val)
        {
            inputData.Add(val);
        }
        public void WriteOutputData(long newVal)
        {
            outputData.Add(newVal);
        }
        public bool HasOutputData()
        {
            return outputData.Count>0;
        }

        public long ReadOutputData()
        {
            if(!HasOutputData())
            {
                return long.MinValue;
            }
            long curVal = outputData[0];
            outputData.RemoveAt(0);
            return curVal;
        }
        public void SetMemoryPointer(long newVal)
        {
            SetMemoryPointer((int)newVal);
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

        public void AdjustRelativeBase(long newVal)
        {
            AdjustRelativeBase((int)newVal);
        }
        public void AdjustRelativeBase(int newVal)
        {
            relativeBase = relativeBase + newVal;
        }
        public int GetRelativeBase()
        {
            return relativeBase;
        }      
    }
}
