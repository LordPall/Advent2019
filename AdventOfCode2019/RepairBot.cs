using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2019
{
    public enum MovementCommandEnum
    {
        //north (1), south(2), west(3), and east(4).
        NORTH = 1,
        SOUTH = 2,
        WEST = 3,
        EAST = 4
    }

    public enum RobotResultCode
    {
        WALL,
        MOVE_SUCCESFUL,
        MOVE_SUCCESFUL_OXYGEN,
        OXYGENATED
        /*            0: The repair droid hit a wall. Its position has not changed.
        1: The repair droid has moved one step in the requested direction.
        2: The repair droid has moved one step in the requested direction; its new position is the location of the oxygen system.*/
    }

    public class RepairBot
    {
        Stack<MovementCommandEnum> fromStartCommands = new Stack<MovementCommandEnum>();

        

        int xSize;
        int ySize;
        
        public struct explorationStruct
        {
            public char curObject;
            
        }



        ExploreNode[] curMap;
        ExploreNode startNode;
        IntComputer curComputer;
        Vector2 curPos;
        MovementCommandEnum curDir=MovementCommandEnum.NORTH; // ish
        StreamWriter sw;
        public RepairBot(string inData, string outFile)
        {
            string curMapFile = "BotMap.txt";
            string rootPath = Path.GetDirectoryName(outFile);
            curMapFile = Path.Combine(rootPath, curMapFile);
            sw = new StreamWriter(outFile);
            xSize = 42;
            ySize = 42;
            startNode = new ExploreNode();
            startNode.nodeType = RobotResultCode.MOVE_SUCCESFUL;
            curNode = startNode;
            curMap = new ExploreNode[xSize * ySize];
            curPos.x = xSize / 2;
            curPos.y = ySize / 2;
            int startIndex= Helpers.GetIndexFromCoordinate(curPos, xSize);
            curMap[startIndex] = startNode;
            curComputer = new IntComputer();
            curComputer.InitializeMemory(inData);
            curComputer.StartComputer(true);
            RunRobot();
            
            //DrawMap();            
            int endIndex = Helpers.GetIndexFromCoordinate(curPos, xSize);
            int length = SearchTree(startIndex, endIndex);                
            sw.Close();
        }
        bool IsOxygenated()
        {
            int open = 0;
            int oxygenated = 0;
            int total = 0;
            for(int intI =0; intI < curMap.Length; intI++)
            {
                if(curMap[intI]!=null)
                {
                    if(curMap[intI].nodeType!=RobotResultCode.WALL)
                    {
                        // open
                        if (curMap[intI].nodeType == RobotResultCode.OXYGENATED)
                        {
                            oxygenated++;
                        }
                        else if (curMap[intI].nodeType == RobotResultCode.MOVE_SUCCESFUL_OXYGEN)
                        {
                            // okay

                        }
                        else if (curMap[intI].nodeType == RobotResultCode.MOVE_SUCCESFUL)
                        {
                            open++;
                        }
                        else
                        {
                            // der fark
                            int t = 0;
                        }
                    }
                }
            }
            sw.WriteLine("STATUS OPEN: " + open + " OXYGENATED " + oxygenated);
            if (open > 0)
            {
                return false;
            }
            return true;

        }

        
        int oxygenIndex = 0;
        public int SearchTree(int startIndex, int endIndex)
        {
            ExploreNode startNode = curMap[oxygenIndex];            
            
            
            HashSet<ExploreNode> oxygenatedNodes = new HashSet<ExploreNode>();

            
            Queue<ExploreNode> nextNodes = new Queue<ExploreNode>();
            Queue<ExploreNode> nodesToOxygenate = new Queue<ExploreNode>();

            bool isComplete = false;
            int minutes = 0;
            startNode.nodeType = RobotResultCode.OXYGENATED;
            nodesToOxygenate.Enqueue(startNode);

            while(!isComplete)
            {
                minutes++;
                while (nodesToOxygenate.Count > 0)
                {                    
                    ExploreNode curNode = nodesToOxygenate.Dequeue();                                                                                                   
                    List<ExploreNode> openNodes = curNode.GetOpenNodes();
                    foreach(ExploreNode openNode in openNodes)
                    {                        
                        if(!oxygenatedNodes.Contains(openNode))
                        {
                            nextNodes.Enqueue(openNode);
                            oxygenatedNodes.Add(openNode);
                            openNode.nodeType = RobotResultCode.OXYGENATED;
                        }
                    }
                }
                nodesToOxygenate = new Queue<ExploreNode>(nextNodes); // DURR
                if (nextNodes.Count == 0)
                {
                    int t = 90;
                    return -1 ;
                }             
                nextNodes.Clear();
                sw.WriteLine("-----------------MINUTE " + minutes + "-----------------------");
                IsOxygenated();
                DrawMap();
                
            }
            return -1;
        }

        MovementCommandEnum TurnRight(MovementCommandEnum startDir)
        {
            if(startDir==MovementCommandEnum.NORTH)
            {
                return MovementCommandEnum.EAST;
            }
            if (startDir == MovementCommandEnum.SOUTH)
            {
                return MovementCommandEnum.WEST;
            }
            if (startDir == MovementCommandEnum.EAST)
            {
                return MovementCommandEnum.SOUTH;
            }
            if (startDir == MovementCommandEnum.WEST)
            {
                return MovementCommandEnum.NORTH;
            }
            throw new Exception("DER FARK");
            return MovementCommandEnum.EAST;
        }
        
        bool isBackTracking = false;
        void RunRobot()
        {            

            RobotResultCode resultCode;
            // Dumb bot
            // go till wall, then turn right.
            for (int intI = 0; intI < 4000; intI++)
            {
                resultCode = RunRobotCycle(curDir);
                ProcessRobotResult(resultCode);
                if(resultCode==RobotResultCode.MOVE_SUCCESFUL_OXYGEN)
                {                    
                    oxygenIndex = Helpers.GetIndexFromCoordinate(curPos, xSize);                 
                }
                // forwards or backwards!
                if(curNode.IsExplored())
                {
                    // keep backtracking
                    // we already went ever direction
                    // back up one and check      
                    isBackTracking = true;
                    if(fromStartCommands.Count==0)
                    {
                        return;
                    }
                    MovementCommandEnum prevMove = fromStartCommands.Pop();
                    curDir = GetOppositeCommand(prevMove);                }
                else
                {
                    isBackTracking = false;
                    curDir = curNode.GetUnexploredDir();
                }
            }

        }
        
        public static MovementCommandEnum GetOppositeCommand(MovementCommandEnum curCommand)
        {
            switch (curCommand)
            {
                case MovementCommandEnum.NORTH:
                    return MovementCommandEnum.SOUTH;
                    
                case MovementCommandEnum.EAST:
                    return MovementCommandEnum.WEST;
                    
                case MovementCommandEnum.SOUTH:
                    return MovementCommandEnum.NORTH;
                    
                case MovementCommandEnum.WEST:
                    return MovementCommandEnum.EAST;
                    
                default:
                    throw new Exception("DER FARK");                    
            }
            return MovementCommandEnum.NORTH;
        }
       
        void GetNextStepForRobot()
        {

        }
        ExploreNode curNode;
        
        
        void ProcessRobotResult(RobotResultCode resultCode)
        {
            Vector2 posToUpdate = GetNewPosition(curPos, curDir);

            if ((resultCode==RobotResultCode.MOVE_SUCCESFUL)||(resultCode==RobotResultCode.MOVE_SUCCESFUL_OXYGEN))
            {

                // do nothing            
                // found a new node
                int curIndex = Helpers.GetIndexFromCoordinate(posToUpdate, xSize);
                if(curMap[curIndex]==null)
                {
                    // ALL NEW NEW NEW!
                    ExploreNode newNode = new ExploreNode();
                    
                    curMap[curIndex] = newNode;
                }
                curMap[curIndex].InitializeExploreNode(resultCode, curNode, curDir);
                // otherwise we've already been here. 
                curNode = curMap[curIndex];
                if(!isBackTracking)
                {
                    fromStartCommands.Push(curDir);
                }
                curPos = posToUpdate;
                return;
            }
            else if (resultCode == RobotResultCode.WALL)
            {
                if(isBackTracking)
                {
                    // errro
                    int t = 9;
                }
                // make a new node, but don't move. Go backwards one.
                int curIndex = Helpers.GetIndexFromCoordinate(posToUpdate, xSize);
                if (curMap[curIndex] == null)
                {
                    // ALL NEW NEW NEW!
                    ExploreNode newNode = new ExploreNode();                    
                    curMap[curIndex] = newNode;
                }
                curMap[curIndex].InitializeExploreNode(RobotResultCode.WALL, curNode, curDir);
                // otherwise we've already been here.
                // curpos does not change
                return;
            }
        }
        

        

        public RobotResultCode RunRobotCycle(MovementCommandEnum moveCommand)
        {
            // run until there's output.
            curComputer.AddInputData((int)moveCommand);
            curComputer.ResumeProgram();            
            return (RobotResultCode)curComputer.ReadOutputData();
        }
        /*UP=1,
            RIGHT=2,
            DOWN=3,
            LEFT=4,
            VOID=5
        */
        // mapping since hte helper functions are diff.
        public Vector2 GetNewPosition(Vector2 startPos,MovementCommandEnum moveCommand )
        {
            Helpers.DirectionEnum newDir;
            switch (moveCommand)
            {
                case MovementCommandEnum.NORTH:
                    newDir = Helpers.DirectionEnum.UP;
                    break;
                case MovementCommandEnum.SOUTH:
                    newDir = Helpers.DirectionEnum.DOWN;
                    break;
                case MovementCommandEnum.EAST:
                    newDir = Helpers.DirectionEnum.RIGHT;
                    break;
                case MovementCommandEnum.WEST:
                    newDir = Helpers.DirectionEnum.LEFT;
                    break;
                default:
                    throw new Exception("DER FARK?");

            }
            Vector2 retVal = Helpers.MovePos(startPos, newDir);
            return retVal;



        }
        string GetMapId(int num)
        {
            int padLength = 3;
            string retVal = num.ToString();
            if(retVal.Length<padLength)
            {
                for(int intI =0; intI<padLength-retVal.Length; intI++)
                {
                    retVal = "0" + retVal;
                }
            }
            return retVal;
        }
        public void DrawMap()
        {
            string outLine = "";
            for (int intY = 0; intY < ySize; intY++)
            {
                for (int intX = 0; intX < xSize; intX++)
                {
                    int curIndex = Helpers.GetIndexFromCoordinate(intX, intY, xSize);
                    if (curMap[curIndex] == null)
                    {
                        outLine += "-";
                    }
                    else
                    {
                        char toAdd = '.';
                        int nodeNum = 0;
                        nodeNum = curMap[curIndex].nodeNum;
                        if (curMap[curIndex].nodeType==RobotResultCode.WALL)
                        {
                            toAdd = '#';
                            
                        }
                        else if (curMap[curIndex].nodeType == RobotResultCode.MOVE_SUCCESFUL_OXYGEN)
                        {
                            toAdd = 'G';
                        }
                        else if (curMap[curIndex].nodeType == RobotResultCode.OXYGENATED)
                        {
                            toAdd = 'O';
                        }
                        if (curMap[curIndex]==startNode)
                        {
                            toAdd = 'S';
                        }
                        string baseString = ""; GetMapId(nodeNum);
                        outLine += baseString + toAdd + "";
                    }
                }
                sw.WriteLine(outLine);
                outLine = "";
            }           
            
        }
    }
    public class ExploreNode      
    {
        public char curChar;

        public RobotResultCode nodeType;
        // each can contain 4 connections        
        public ExploreNode northNode;
        public ExploreNode southNode;
        public ExploreNode eastNode;
        public ExploreNode westNode;
        static int curNum;
        public int nodeNum;
        public ExploreNode()
        {
            nodeNum = curNum;
            curNum++;
        }
        public void InitializeExploreNode(RobotResultCode curType, ExploreNode parentNode, MovementCommandEnum enterDir)
        {
            parentNode.AssignMovementNode(this, enterDir);
            MovementCommandEnum exitDir = RepairBot.GetOppositeCommand(enterDir);
            AssignMovementNode(parentNode, exitDir);
            nodeType = curType;
        }
        bool IsNodeOpen(ExploreNode curNode)
        {
            if (curNode != null)
            {
                if (curNode.nodeType != RobotResultCode.WALL)
                {
                    if (curNode.nodeType != RobotResultCode.OXYGENATED)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public List<ExploreNode> GetOpenNodes()
        {
            List<ExploreNode> retVal = new List<ExploreNode>();
            if(IsNodeOpen(northNode))
            {
                retVal.Add(northNode);
            }
            if (IsNodeOpen(southNode))
            {
                retVal.Add(southNode);
            }
            if (IsNodeOpen(eastNode))
            {
                retVal.Add(eastNode);
            }
            if (IsNodeOpen(westNode))
            {
                retVal.Add(westNode);
            }
            return retVal;
        }        

        public bool IsExplored()
        {
            if(northNode==null)
            {
                return false;
            }
            if(southNode==null)
            {
                return false;
            }
            if(eastNode==null)
            {
                return false;
            }
            if(westNode==null)
            {
                return false;
            }
            return true;
        }

        public MovementCommandEnum GetUnexploredDir()
        {
            // dont call this wihtout checking firs.t
            
            if (northNode == null)
            {
                return MovementCommandEnum.NORTH;
            }
            if (southNode == null)
            {
                return MovementCommandEnum.SOUTH;
            }
            if (eastNode == null)
            {
                return MovementCommandEnum.EAST;
            }
            if (westNode == null)
            {
                return MovementCommandEnum.WEST;    
            }
            throw new Exception("DER FARK 2");
            return MovementCommandEnum.NORTH;
        }
        
        ExploreNode GetNodeForDir(MovementCommandEnum curDir)
        {
            if(curDir==MovementCommandEnum.NORTH)
            {
                return northNode;
            }
            if (curDir == MovementCommandEnum.SOUTH)
            {
                return southNode;
            }

            if (curDir == MovementCommandEnum.EAST)
            {
                return eastNode;
            }

            if (curDir == MovementCommandEnum.WEST)
            {
                return westNode;
            }
            return null;
        }
        void AssignMovementNode(ExploreNode newNode, MovementCommandEnum curDir )
        {
            if (curDir == MovementCommandEnum.NORTH)
            {
                northNode = newNode;
            }
            if (curDir == MovementCommandEnum.SOUTH)
            {
                southNode = newNode;
            }

            if (curDir == MovementCommandEnum.EAST)
            {
                eastNode = newNode;
            }
            if (curDir == MovementCommandEnum.WEST)
            {
                westNode = newNode;
            }


        }




    }
}
