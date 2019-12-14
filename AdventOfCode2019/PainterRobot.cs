using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
namespace AdventOfCode2019
{
    class PainterRobot
    {
        // computer runs, outputs instructions. Robot parses instructions, adds input, resumes computer.
        // we don't know how big the panel is.

        int xSize;
        int ySize;
        string outFile;
        IntComputer robotComputer;
        PaintPanelStruct[] panelsToPaint;
        Helpers.DirectionEnum curDirection = Helpers.DirectionEnum.UP;
        Vector2 curPos;
        struct PaintPanelStruct
        {
            public bool wasPainted;
            public PaintColorEnum curColor;            
            

        }

        enum PaintColorEnum
        {
            BLACK=0,
            WHITE=1                
        }
        public PainterRobot(string sourceInstructions, int width, int height, string debugFile)
        {
            outFile = debugFile;
            xSize = width;
            ySize = height;
            curPos = new Vector2();
            curPos.x = xSize / 2;
            curPos.y = ySize / 2;
            panelsToPaint = new PaintPanelStruct[xSize * ySize];
            int curIndex = Helpers.GetIndexFromCoordinate(curPos, xSize);
            panelsToPaint[curIndex].curColor = PaintColorEnum.WHITE;
            
                robotComputer = new IntComputer();
            robotComputer.InitializeMemory(sourceInstructions);
            robotComputer.StartComputer(true);
            RunRobot();            
        }
        

        PaintColorEnum GetPanelColorAtPosition()
        {
            int curIndex = Helpers.GetIndexFromCoordinate(curPos, xSize);            
            return panelsToPaint[curIndex].curColor;
        }
        int panelsPainted = 0;
        void PaintPanelAtPosition(PaintColorEnum colorToPaint)
        {
            int curIndex = Helpers.GetIndexFromCoordinate(curPos, xSize);
            sw.Write("Painting panel at " + curPos.x + "," + curPos.y+" curColor is "+panelsToPaint[curIndex].curColor);
            if (!panelsToPaint[curIndex].wasPainted)
            {
                panelsToPaint[curIndex].wasPainted = true;                
                panelsPainted++;                
            }
            sw.WriteLine(" New color is " + colorToPaint);
            panelsToPaint[curIndex].curColor = colorToPaint;
            if(colorToPaint==PaintColorEnum.BLACK)
            {
                bm.SetPixel(curPos.x, curPos.y, Color.Black);
            }
            else
            {
                bm.SetPixel(curPos.x, curPos.y, Color.White);
            }

        }
        Bitmap bm;
        public void RunRobot()
        {
            /*
            The Intcode program will serve as the brain of the robot. 
            The program uses input instructions to access the robot's camera: 
            provide 0 if the robot is over a black panel or 1 if the robot is over 
            a white panel. Then, the program will output two values:
            First, it will output a value indicating the color to paint the 
            panel the robot is over: 0 means to paint the panel black, 
            and 1 means to paint the panel white.
            Second, it will output a value indicating the 
            direction the robot should turn: 0 means it should turn 
            left 90 degrees, and 1 means it should turn right 90 degrees.
            After the robot turns, it should always move forward exactly one panel. The robot starts facing up.
            */
            bm = new Bitmap(xSize, ySize);
            for(int intX =0;intX< xSize; intX++)
            {
                for(int intY = 0; intY < ySize; intY++)
                {
                    bm.SetPixel(intX, intY, Color.Black);
                }
            }
            bm.SetPixel(curPos.x, curPos.y, Color.White);

            sw = new StreamWriter(outFile);
            PaintColorEnum colorToPaint;
            int directionToTurn;
            
            while(!robotComputer.IsProgramCompleted())
            {
                robotComputer.AddInputData((long)GetPanelColorAtPosition());
                sw.WriteLine("Adding input " + (long)GetPanelColorAtPosition());
                robotComputer.ResumeProgram();
                robotComputer.ResumeProgram();
                if(!robotComputer.IsProgramCompleted())
                {
                    long outputOne = robotComputer.ReadOutputData(); ;
                    long outputTwo = robotComputer.ReadOutputData();
                    sw.WriteLine("Program returned 2 values first=" + outputOne + " second=" + outputTwo);
                    colorToPaint = (PaintColorEnum)outputOne;
                    sw.Write("Painting current panel color " + colorToPaint);
                    PaintPanelAtPosition(colorToPaint);
                    directionToTurn = (int)outputTwo;
                    sw.Write(" Turning robot " + directionToTurn);
                    TurnRobot(directionToTurn);
                    sw.WriteLine("Facing " + curDirection + " startPos is " + curPos.x + "," + curPos.y);
                    curPos = Helpers.MovePos(curPos, curDirection);
                    sw.WriteLine(" New pos is " + curPos.x + "," + curPos.y);
                }
            }
            sw.WriteLine("PAINTED " + panelsPainted);
            OutputDebugInfo();
            sw.Close();            
            bm.Save(outFile+".png", System.Drawing.Imaging.ImageFormat.Png);

            
        }
        string dirInfo = "";


        void TurnRobot(int dirToTurn)
        {
            dirInfo = "Robot Facing " + curDirection + " got " + dirToTurn + " newDir =";
            // 0-left, 1=right
            if(dirToTurn==0)
            {
                curDirection = Helpers.TurnLeft(curDirection);
            }
            else
            {
                curDirection = Helpers.TurnRight(curDirection);
            }
            dirInfo = dirInfo + curDirection.ToString();
            sw.WriteLine(dirInfo);
            dirInfo = "";
        }
        StreamWriter sw;
        public void OutputDebugInfo()
        {
            int curIndex=0;
            string outLine = "";
            for(int intI =0; intI < panelsToPaint.Length; intI++)
            {
                if(panelsToPaint[intI].curColor==PaintColorEnum.WHITE)
                {
                    outLine += "#";
                }
                else
                {
                    outLine += ".";
                }
                curIndex++;
                if(curIndex>=xSize)
                {
                    sw.WriteLine(outLine);
                    outLine = "";
                    curIndex = 0;
                }
            }

        }



    }
}
