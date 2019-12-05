using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    public class OpCodeInstruction
    {
        public enum OPCODE_TYPES
        {
            ADD = 1,
            MUL = 2,
            INPUT=3,
            OUTPUT=4,
            JUMP_IF_TRUE=5,
            JUMP_IF_FALSE=6,
            LESS_THAN=7,
            EQUALS=8,
            EXIT = 99
        };
        int rawInstructionData;
        List<OpCodeParameter.PARAMETER_MODES> paramModes = new List<OpCodeParameter.PARAMETER_MODES>();
        OPCODE_TYPES instruction;
        
        public OpCodeInstruction(int rawData)
        {
            rawInstructionData = rawData;
            int instructionData = rawData % 100; // last 2?
            instruction = (OPCODE_TYPES)instructionData;

            rawData = rawData - instructionData;
            rawData = rawData / 100;
            // now we have the last 3 digits. 
            for(int intI =0; intI < 3; intI++)
            {
                //yes I hardcoded to 3 instructions all the time. I think that's okay. 
                if(rawData==0)
                {
                    paramModes.Add(OpCodeParameter.PARAMETER_MODES.POSITION);
                }
                else
                {
                    // get the digit
                    int curMode = rawData % (int)Math.Pow(10, intI+1);
                    rawData = rawData / (int)Math.Pow(10, intI+1);
                    paramModes.Add((OpCodeParameter.PARAMETER_MODES)curMode);
                }
            }            
        }
        public OpCodeParameter.PARAMETER_MODES GetParamMode(int paramPosition)
        {
            
            if(paramPosition >paramModes.Count-1)
            {
                return OpCodeParameter.PARAMETER_MODES.NULL;
            }
            return paramModes[paramPosition];
        }
        public OPCODE_TYPES GetInstruction()
        {
            return instruction;
        }
    }
}
