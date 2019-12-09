using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    public class OpCode
    {
        
        protected OpCodeInstruction curInstruction;
        public static OpCode ReadNextOpCode(IntComputer computer)
        {
            // 1002
            //ABCDE
            //01002 = DE = opcode, c = mode for op 1, b mode for op 2 a, mode for op 3

            long opCodeData = computer.ReadNextMemoryAddress();
            OpCodeInstruction newInstruction = new OpCodeInstruction(opCodeData);
            switch (newInstruction.GetInstruction())
            {
                case OpCodeInstruction.OPCODE_TYPES.ADD:
                    return new AddOpCode(newInstruction);
                case OpCodeInstruction.OPCODE_TYPES.MUL:
                    return new MulOpcode(newInstruction);
                case OpCodeInstruction.OPCODE_TYPES.EXIT:
                    return new ExitOpCode(newInstruction);
                case OpCodeInstruction.OPCODE_TYPES.INPUT:
                    return new InputOpCode(newInstruction);
                case OpCodeInstruction.OPCODE_TYPES.OUTPUT:
                    return new OutputOpCode(newInstruction);

                case OpCodeInstruction.OPCODE_TYPES.JUMP_IF_TRUE:
                    return new JumpIfTrueOpCode(newInstruction);

                case OpCodeInstruction.OPCODE_TYPES.JUMP_IF_FALSE:
                    return new JumpIfFalseOpCode(newInstruction);

                case OpCodeInstruction.OPCODE_TYPES.LESS_THAN:
                    return new LessThanOpCode(newInstruction);

                case OpCodeInstruction.OPCODE_TYPES.EQUALS:
                    return new EqualsOpCode(newInstruction);
                case OpCodeInstruction.OPCODE_TYPES.ADJUST_RELATIVE_BASE:
                    return new AdjustRelativeBaseOpCode(newInstruction);

                default:
                    Console.WriteLine("DER FARK?");
                    return null;
            }
        }
        public OpCode(OpCodeInstruction newInstruction)
        {
            curInstruction = newInstruction;
        }

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
        OpCodeParameter firstVal;
        OpCodeParameter secondVal;
        OpCodeParameter thirdVal;
        long result;
        public MulOpcode(OpCodeInstruction newInstruction) : base(newInstruction)
        {           

        }

        protected override void ReadOpCodeData(IntComputer computer)
        {
            long nextMemoryData = computer.ReadNextMemoryAddress();
            firstVal = new OpCodeParameter(curInstruction.GetParamMode(0), nextMemoryData, computer);

            nextMemoryData = computer.ReadNextMemoryAddress();
            secondVal = new OpCodeParameter(curInstruction.GetParamMode(1), nextMemoryData, computer);

            nextMemoryData = computer.ReadNextMemoryAddress();
            thirdVal = new OpCodeParameter(curInstruction.GetParamMode(2), nextMemoryData, computer);
        }


        protected override void ProcessOpCode(IntComputer computer)
        {
            result = firstVal.ReadParamFromMemory() * secondVal.ReadParamFromMemory();
            computer.ReplaceMemoryAtAddress(thirdVal.GetMemoryWriteAddressFromParameter(), result);            
        }
    }
    public class AddOpCode : OpCode
    {
        OpCodeParameter firstVal;
        OpCodeParameter secondVal;
        OpCodeParameter thirdVal;
        long result;
        public AddOpCode(OpCodeInstruction newInstruction) : base(newInstruction)
        {

        }

        protected override void ReadOpCodeData(IntComputer computer)
        {
            long nextMemoryData = computer.ReadNextMemoryAddress();
            firstVal = new OpCodeParameter(curInstruction.GetParamMode(0), nextMemoryData, computer);

            nextMemoryData = computer.ReadNextMemoryAddress();
            secondVal = new OpCodeParameter(curInstruction.GetParamMode(1), nextMemoryData, computer);

            nextMemoryData = computer.ReadNextMemoryAddress();
            thirdVal = new OpCodeParameter(curInstruction.GetParamMode(2), nextMemoryData, computer);
        }
        protected override void ProcessOpCode(IntComputer computer)
        {

            result = firstVal.ReadParamFromMemory() + secondVal.ReadParamFromMemory();
            computer.ReplaceMemoryAtAddress(thirdVal.GetMemoryWriteAddressFromParameter(), result);

        }
    }
    public class ExitOpCode : OpCode
    {
        public ExitOpCode (OpCodeInstruction newInstruction) : base(newInstruction)
        {

        }

        protected override void ReadOpCodeData(IntComputer computer)
        {
        }
        protected override void ProcessOpCode(IntComputer computer)
        {
            computer.StopProgram();
        }
    }
    public class InputOpCode : OpCode
    {
        OpCodeParameter destinationParam;
        long inputData;
        public InputOpCode(OpCodeInstruction newInstruction) : base(newInstruction)
        {

        }

        protected override void ReadOpCodeData(IntComputer computer)
        {
            long nextMemoryData = computer.ReadNextMemoryAddress();
            destinationParam = new OpCodeParameter(curInstruction.GetParamMode(0), nextMemoryData, computer);
            inputData = computer.ReadNextInput();
        }
        protected override void ProcessOpCode(IntComputer computer)
        {
            // input is forked up
            // you can have a relative address. It's technically a write, but it can be in immediate, position and relative mode.
            int targetAddress = destinationParam.GetMemoryWriteAddressForInputParameter();
            computer.ReplaceMemoryAtAddress(targetAddress, inputData);
        }
    }
    // debate whether to parameterize this. 
    public class OutputOpCode : OpCode
    {
        OpCodeParameter targetData;

        public OutputOpCode(OpCodeInstruction newInstruction) : base(newInstruction)
        { 
        }
        protected override void ReadOpCodeData(IntComputer computer)
        {
            long nextMemoryData = computer.ReadNextMemoryAddress();
            targetData = new OpCodeParameter(curInstruction.GetParamMode(0), nextMemoryData, computer) ;
        }
        protected override void ProcessOpCode(IntComputer computer)
        {
            computer.WriteOutputData(targetData.ReadParamFromMemory());
            computer.PauseProgram();
        }
    }

    public class JumpIfTrueOpCode : OpCode
    {
        /*Opcode 5 is jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter. 
         * Otherwise, it does nothing.*/

        OpCodeParameter firstVal;
        OpCodeParameter secondVal;

        public JumpIfTrueOpCode(OpCodeInstruction newInstruction) : base(newInstruction)
        {
        }
        protected override void ReadOpCodeData(IntComputer computer)
        {
            long nextMemoryData = computer.ReadNextMemoryAddress();
            firstVal= new OpCodeParameter(curInstruction.GetParamMode(0), nextMemoryData, computer);
            
            nextMemoryData = computer.ReadNextMemoryAddress();
            secondVal = new OpCodeParameter(curInstruction.GetParamMode(1), nextMemoryData, computer);

        }
        protected override void ProcessOpCode(IntComputer computer)
        {
            if(firstVal.ReadParamFromMemory()!=0)
            {// just setting memoryPointer so we don't need to use the specialty writy addresses
                computer.SetMemoryPointer(secondVal.ReadParamFromMemory());
            }

        }
    }

    public class JumpIfFalseOpCode : OpCode
    {
        OpCodeParameter firstVal;
        OpCodeParameter secondVal;

        public JumpIfFalseOpCode(OpCodeInstruction newInstruction) : base(newInstruction)
        {
        }
        protected override void ReadOpCodeData(IntComputer computer)
        {
            long nextMemoryData = computer.ReadNextMemoryAddress();
            firstVal = new OpCodeParameter(curInstruction.GetParamMode(0), nextMemoryData, computer);
            
            nextMemoryData = computer.ReadNextMemoryAddress();
            secondVal = new OpCodeParameter(curInstruction.GetParamMode(1), nextMemoryData, computer);

        }
        protected override void ProcessOpCode(IntComputer computer)
        {
            if (firstVal.ReadParamFromMemory() == 0)
            {
                // just setting memoryPointer so we don't need to use the specialty writy addresses
                computer.SetMemoryPointer(secondVal.ReadParamFromMemory());
            }

        }
    }

    public class LessThanOpCode : OpCode
    {
        /*
         * 
         * Opcode 7 is less than: if the first parameter 
         * is less than the second parameter, it stores 1 in the position given by 
         * the third parameter. Otherwise, it stores 0.
         * */
        OpCodeParameter firstVal;
        OpCodeParameter secondVal;
        OpCodeParameter thirdVal;

        public LessThanOpCode(OpCodeInstruction newInstruction) : base(newInstruction)
        {
        }
        protected override void ReadOpCodeData(IntComputer computer)
        {
            long nextMemoryData = computer.ReadNextMemoryAddress();
            firstVal = new OpCodeParameter(curInstruction.GetParamMode(0), nextMemoryData, computer);

            nextMemoryData = computer.ReadNextMemoryAddress();
            secondVal = new OpCodeParameter(curInstruction.GetParamMode(1), nextMemoryData, computer);

            nextMemoryData = computer.ReadNextMemoryAddress();
            thirdVal = new OpCodeParameter(curInstruction.GetParamMode(2), nextMemoryData, computer);
        }
        protected override void ProcessOpCode(IntComputer computer)
        {
            if(firstVal.ReadParamFromMemory()<secondVal.ReadParamFromMemory())
            {
                computer.ReplaceMemoryAtAddress(thirdVal.GetMemoryWriteAddressFromParameter(), 1);
            }
            else
            {
                computer.ReplaceMemoryAtAddress(thirdVal.GetMemoryWriteAddressFromParameter(), 0);
            }
            
        }
    }

    public class EqualsOpCode : OpCode
    {
        OpCodeParameter firstVal;
        OpCodeParameter secondVal;
        OpCodeParameter thirdVal;
        public EqualsOpCode(OpCodeInstruction newInstruction) : base(newInstruction)
        {
        }
        protected override void ReadOpCodeData(IntComputer computer)
        {
            long nextMemoryData = computer.ReadNextMemoryAddress();
            firstVal = new OpCodeParameter(curInstruction.GetParamMode(0), nextMemoryData, computer);

            nextMemoryData = computer.ReadNextMemoryAddress();
            secondVal = new OpCodeParameter(curInstruction.GetParamMode(1), nextMemoryData, computer);

            nextMemoryData = computer.ReadNextMemoryAddress();
            thirdVal = new OpCodeParameter(curInstruction.GetParamMode(2), nextMemoryData, computer);
        }

        protected override void ProcessOpCode(IntComputer computer)
        {
            if (firstVal.ReadParamFromMemory() == secondVal.ReadParamFromMemory())
            {
                computer.ReplaceMemoryAtAddress(thirdVal.GetMemoryWriteAddressFromParameter(), 1);
            }
            else
            {
                computer.ReplaceMemoryAtAddress(thirdVal.GetMemoryWriteAddressFromParameter(), 0);
            }

        }
    }
    public class AdjustRelativeBaseOpCode : OpCode
    {
        OpCodeParameter sourceData;
        
        public AdjustRelativeBaseOpCode(OpCodeInstruction newInstruction) : base(newInstruction)
        {

        }

        protected override void ReadOpCodeData(IntComputer computer)
        {
            long nextMemoryData = computer.ReadNextMemoryAddress();
            sourceData = new OpCodeParameter(curInstruction.GetParamMode(0), nextMemoryData, computer);
        }


        protected override void ProcessOpCode(IntComputer computer)
        {
            // this isn't writing to a memory address so we just read from memory
            computer.AdjustRelativeBase(sourceData.ReadParamFromMemory());
        }
    }
}

