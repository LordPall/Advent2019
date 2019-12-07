using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace AdventOfCode2019
{
    class Amplifiers
    {
        int numAmplifiers;

        List<IntComputer> generatedAmplifiers = new List<IntComputer>();

        public Amplifiers(int ampCount, string sourceProgram)
        {
            numAmplifiers = ampCount;
            for (int intI = 0; intI < ampCount; intI++)
            {
                IntComputer newComp = new IntComputer();
                newComp.curId = numAmplifiers - intI;
                newComp.InitializeMemory(sourceProgram);
                newComp.StartComputer(true); 
                generatedAmplifiers.Add(newComp);
                
            }
        }
        public void ResetAmplifiers()
        {
            for(int intI = 0; intI < generatedAmplifiers.Count; intI++)
            {
                generatedAmplifiers[intI].ResetMemory();
            }
        }
        public int RunAmplifiers(StreamWriter sw)
        {
            string outLine = "";
            bool amplifiersDone = false;
            int curIndex = 0;
            int outputVal =0;
            while (!amplifiersDone)
            {
                if(!generatedAmplifiers[curIndex].IsProgramCompleted())
                {
                    sw.WriteLine("Running amplifier " + curIndex+ " new input is " + outputVal);
                    generatedAmplifiers[curIndex].AddInputData(outputVal);
                    generatedAmplifiers[curIndex].ResumeProgram();
                    if(generatedAmplifiers[curIndex].HasOutputData())
                    {
                        outputVal = generatedAmplifiers[curIndex].ReadOutputData();
                        sw.WriteLine("output was " + outputVal);
                    }
                    else
                    {
                        sw.WriteLine("Amp at index " + curIndex + " finished");
                    }
                }
                else
                {
                    sw.WriteLine("Skipping amplifier " + curIndex + " because it is completed");
                }
                curIndex++;
                if(curIndex>generatedAmplifiers.Count-1)
                {
                    curIndex = 0;
                }
                amplifiersDone = CheckAmplifiersCompleted();

            }            
            return outputVal;
        }
        bool CheckAmplifiersCompleted()
        {
            
            for(int intI =0; intI < generatedAmplifiers.Count; intI++)
            {
                if(!generatedAmplifiers[intI].IsProgramCompleted())
                {
                    return false;
                }

            }
            return true;

        }

        public void SetPhaseSettings(int phaseVal)
        {
            for (int intI = numAmplifiers-1; intI >=0; intI--)
            {
                int curPhase = 0;
                if(phaseVal!=0)
                {
                    curPhase = phaseVal % 10;
                    phaseVal = (phaseVal /10);
                }
                generatedAmplifiers[intI].AddInputData(curPhase);
            }
        }
        
        public int GetPhase(int ampIndex)
        {
            return generatedAmplifiers[ampIndex].inputData[0]; 
        }

    }
}
