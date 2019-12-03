using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    public class OpCode
    {
        public enum OPCODE_TYPES
        {            
            ADD=1,
            MUL = 2,
            EXIT = 99
        };


        // todo move this to computer. 
        public static OpCode ReadNextOpCode(IntComputer computer)
        {
            OPCODE_TYPES nextOpCode = (OPCODE_TYPES)computer.ReadNextMemoryAddress();
            switch (nextOpCode)
            {
                case OPCODE_TYPES.ADD:
                    return new AddOpCode();
                case OPCODE_TYPES.MUL:
                    return new MulOpcode();
                case OPCODE_TYPES.EXIT:
                    return new ExitOpCode();
                default:
                    Console.WriteLine("DER FARK?");
                    return null;
            }
        }
        public OPCODE_TYPES instruction;   
        public void RunOpCode(IntComputer computer)
        {
            ReadOpCodeData(computer);
            ProcessOpCode(computer);
        }
        protected virtual void ReadOpCodeData(IntComputer computer)
        {
            // nada

        }
        protected virtual void ProcessOpCode(IntComputer computer)
        {

        }
        
    }
    public class MulOpcode : OpCode
    {
        int firstNumberAddress;
        int secondNumberAddress;
        int destinationAddress;
        int result;

        protected override void ReadOpCodeData(IntComputer computer)
        {
            instruction = OPCODE_TYPES.MUL;
            firstNumberAddress = computer.ReadNextMemoryAddress();
            secondNumberAddress = computer.ReadNextMemoryAddress();
            destinationAddress = computer.ReadNextMemoryAddress();
        }


        protected override void ProcessOpCode(IntComputer computer)
        {
            result = computer.ReadMemoryAtAddress(firstNumberAddress) * computer.ReadMemoryAtAddress(secondNumberAddress);
            computer.ReplaceMemoryAtAddress(destinationAddress, result);
        }
    }
    public class AddOpCode : OpCode
    {
        int firstNumberAddress;
        int secondNumberAddress;
        int destinationAddress;
        int result;
        protected override void ReadOpCodeData(IntComputer computer)
        {
            instruction = OPCODE_TYPES.ADD;
            firstNumberAddress = computer.ReadNextMemoryAddress();
            secondNumberAddress = computer.ReadNextMemoryAddress();
            destinationAddress = computer.ReadNextMemoryAddress();
        }
        protected override void ProcessOpCode(IntComputer computer)
        {
            result = computer.ReadMemoryAtAddress(firstNumberAddress) + computer.ReadMemoryAtAddress(secondNumberAddress);
            computer.ReplaceMemoryAtAddress(destinationAddress, result);
        }
    }
    public class ExitOpCode : OpCode
    {
        protected override void ReadOpCodeData(IntComputer computer)
        {
            instruction = OPCODE_TYPES.EXIT;
        }
        protected override void ProcessOpCode(IntComputer computer)
        {
            computer.StopProgram();
        }
    }
}
