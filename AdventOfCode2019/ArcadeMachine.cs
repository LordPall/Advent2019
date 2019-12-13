using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    public class ArcadeMachine
    {
        enum BlockTypeEnum
            /*
             * 
             * 0 is an empty tile. No game object appears in this tile.
1 is a wall tile. Walls are indestructible barriers.
2 is a block tile. Blocks can be broken by the ball.
3 is a horizontal paddle tile. The paddle is indestructible.
4 is a ball tile. The ball moves diagonally and bounces off objects*/

        { 
            EMPTY=0,
            WALL=1,
            BLOCK=2,
            HORIZONTAL_PADDLE=3,
            BALL=4
        };

        List<List<BlockTypeEnum>> screen = new List<List<BlockTypeEnum>>(); 
        int requiredOutputPerCycle = 3;
        IntComputer curComputer;
        StreamWriter sw;
        bool startDrawing = false;
        long curScore = 0;
        public ArcadeMachine(string sourceData, string outFile)
        {
            curComputer = new IntComputer();
            curComputer.InitializeMemory(sourceData);
            curComputer.ReplaceMemoryAtAddress(0, 2); // infinite play            
            curComputer.StartComputer(true);
            sw = new StreamWriter(outFile);
        }
        public void StartMachine()
        {
            RunMachine();

        }
        void ReadComputerDataAndUpdateScreen()
        {
            curComputer.AddInputData(0);
            for (int intI = 0; intI < requiredOutputPerCycle; intI++)
            {
                curComputer.ResumeProgram(); // 1
            }
            if(curComputer.HasOutputData())
            {

                int xLoc = (int)curComputer.ReadOutputData();
                int yLoc = (int)curComputer.ReadOutputData();
                if (xLoc<0)
                {
                    // score, not the other thing                    
                    curScore = curComputer.ReadOutputData();
                    Console.WriteLine(curScore);
                    startDrawing = true;
                    curComputer.AddInputData(0);
                }
                else
                {
                    
                    BlockTypeEnum bt = (BlockTypeEnum)curComputer.ReadOutputData();
                    screen = Helpers.ExpandListOfLists(screen, xLoc, yLoc);
                    screen[yLoc][xLoc] = bt;
                    if(startDrawing)
                    {
                        //DrawScreen();
                    }


                }
            }
        }
        void RunMachine()
        {
            while(!curComputer.IsProgramCompleted())
            {
                ReadComputerDataAndUpdateScreen();
            }
            ReadComputerDataAndUpdateScreen(); // for the last run            
            //DrawScreen();            
            sw.Close();
        }
        public void DrawScreen()
        {
            Console.Clear();
            Console.BufferWidth = 120;
            
            Console.SetWindowSize(Console.BufferWidth, 50);

            string outLine = "";
            Dictionary<BlockTypeEnum, int> blockCount = new Dictionary<BlockTypeEnum, int>();
            foreach(List<BlockTypeEnum> row in screen)
            {
                foreach(BlockTypeEnum bt in row)
                {
                    outLine += GetCharForBlockType(bt);
                    if(blockCount.ContainsKey(bt))
                    {
                        blockCount[bt]++;
                    }
                    else
                    {
                        blockCount.Add(bt, 1);
                    }
                }
                Console.WriteLine(outLine);
                //sw.WriteLine(outLine);
                outLine = "";
            }
            foreach (KeyValuePair<BlockTypeEnum, int> kvp in blockCount)
            {
                Console.WriteLine("BLOCK TYPE " + kvp.Key.ToString() + "=" + kvp.Value);
                //sw.WriteLine("BLOCK TYPE " + kvp.Key.ToString() + "=" + kvp.Value);
            }
            //System.Threading.Thread.Sleep(10);
        }

        char GetCharForBlockType(BlockTypeEnum bt)
        {
            switch (bt)
            {
                case BlockTypeEnum.EMPTY:
                    return '.';
                case BlockTypeEnum.BLOCK:
                    return '#';
                case BlockTypeEnum.WALL:
                    return 'W';
                case BlockTypeEnum.HORIZONTAL_PADDLE:
                    return '_';
                case BlockTypeEnum.BALL:
                    return 'O';
                default:
                    return 'F';
            }
        }        
    }    
}
