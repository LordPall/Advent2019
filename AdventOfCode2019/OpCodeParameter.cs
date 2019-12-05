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
            POSITION = 0,
            IMMEDIATE =1 // read from memory address
             // value is actually passed in. 
        }
        PARAMETER_MODES paramMode; // immediate or position
        int paramData;
        IntComputer curComputer;
        public OpCodeParameter(PARAMETER_MODES newParamMode, int curVal, IntComputer computer)
        {
            paramMode = newParamMode;
            paramData = curVal;
            curComputer = computer;
            
        }
        public int GetParamData()
        {
            return paramData;
        }
        public int ReadParamMemory()
        {
            return curComputer.ReadMemoryAtAddress(paramData);
        }
        public int GetParamDataUsingMode()
        {
            if (paramMode == PARAMETER_MODES.IMMEDIATE)
            {
                return GetParamData();
            }
            else
            {
                return ReadParamMemory();
            }
        }
        
    }
}
