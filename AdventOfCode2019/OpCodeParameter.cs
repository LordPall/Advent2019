using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    public class OpCodeParameter
    {
        public enum PARAMETER_MODES
        {
            NULL=-1,
            POSITION = 0, // value represents the memory address we want to read. x, val, val, we use val as a memory pointer, and read the val from tehre
            IMMEDIATE =1, // Value is the actual parameter passed into the opcode. So x,val,val,val. We use val as is. 
            RELATIVE = 2 //// Value is the memory address we need to read, but we modify it by relativebase. 

        }
        PARAMETER_MODES paramMode; // immediate or position
        long paramData;
        IntComputer curComputer;
        public OpCodeParameter(PARAMETER_MODES newParamMode, long curVal, IntComputer computer)
        {
            paramMode = newParamMode;
            paramData = curVal;
            curComputer = computer;
            
        }
        long GetParamData()
        {
            return paramData;
        }
        long ReadParamMemory()
        {
            return curComputer.ReadMemoryAtAddress((int)paramData);
        }
        long ReadMemoryAtRelativeAddress()
        {
            // paramData is the start point, then use relativebase.
            int address = ((int)GetParamData()+ curComputer.GetRelativeBase());
            return curComputer.ReadMemoryAtAddress(address);
        }
        public int GetMemoryWriteAddressForInputParameter()
        {
            if (paramMode == PARAMETER_MODES.IMMEDIATE)
            {
                return (int)paramData; // actual value
            }
            else if (paramMode == PARAMETER_MODES.RELATIVE)
            {
                int memoryAddress = (int)paramData;
                memoryAddress = memoryAddress + curComputer.GetRelativeBase();
                return memoryAddress;
            }            
            else
            {
                // position
                return (int)paramData;
            }
            

        }
        public int GetMemoryWriteAddressFromParameter()
        {
            if (paramMode == PARAMETER_MODES.IMMEDIATE)
            {
                throw new Exception("No writes in immediate mode");
            }
            else if (paramMode == PARAMETER_MODES.RELATIVE)
            {
                int memoryAddress = (int)paramData;
                memoryAddress = memoryAddress + curComputer.GetRelativeBase();
                return memoryAddress;
            }
            return (int)paramData; // position

        }
        
        public long ReadParamFromMemory()
        {
            // do not use this for writes. 
            if (paramMode == PARAMETER_MODES.IMMEDIATE)
            {
                return GetParamData();
            }
            else if (paramMode==PARAMETER_MODES.RELATIVE)
            {
                return ReadMemoryAtRelativeAddress();   
            }
            else
            {
                return ReadParamMemory();
            }
        }
        
    }
}
