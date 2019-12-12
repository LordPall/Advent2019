using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    class PlanetarySystem
    {
        PlanetaryBody[] curPlanets;
        StreamWriter sw;
        public PlanetarySystem(string[] sourceData, string outFile)
        {
            curPlanets = new PlanetaryBody[sourceData.Length];
            for (int intI = 0; intI < sourceData.Length; intI++)
            {

                PlanetaryBody pb = new PlanetaryBody();
                pb.position = new Vector3(sourceData[intI]);
                pb.name = "" + intI;
                curPlanets[intI] = pb;
            }
            sw = new StreamWriter(outFile);


        }



        public void Abstraction()
        {
            // we don't care about actual simulation
            // we care about x1, x2, vx1, vx2
            //and they modify each other. 



            // 0 1, 0 2, 0 3
            // 1 2, 1 3
            // 2 3
            // find cycles to x velocoity reaching 0
            // cycles to y velocity reaching 0
            // z cycles to reach 0
            // then find common multiple of those 43



            // structure 
            int[] trackedInts = new int[8];

        }

        HashSet<string> systemSnapshots = new HashSet<string>();
        public void SimulateSystem()
        {
            ulong numCycles = 0;
            bool foundDuplicate = false;
            bool planetsMoving = false;
            ulong xMult = 0;
            ulong yMult = 0;
            ulong zMult = 0;
            while (!foundDuplicate)
            {
                for (int intJ = 0; intJ < curPlanets.Length; intJ++)
                {
                    // curplanet is j                    
                    planetsMoving = false;
                    for (int intK = intJ; intK < curPlanets.Length; intK++)
                    {
                        if (intK == intJ)
                        {
                            continue;// thatsa us
                        }
                        if (xMult == 0)
                        {
                            UpdateXVel(curPlanets[intJ], curPlanets[intK]);
                        }
                        else if (yMult == 0)
                        {
                            UpdateYVel(curPlanets[intJ], curPlanets[intK]);
                        }
                        else if (zMult == 0)
                        {
                            UpdateZVel(curPlanets[intJ], curPlanets[intK]);
                        }
                        else
                        {
                            // done
                        }
                    }
                }
                planetsMoving = false;
                for (int intJ = 0; intJ < curPlanets.Length; intJ++)
                {
                    if (xMult == 0)
                    {
                        curPlanets[intJ].position.x += curPlanets[intJ].velocity.x;
                        planetsMoving = planetsMoving || curPlanets[intJ].velocity.x != 0;
                        numCycles++;
                    }
                    else if (yMult == 0)
                    {
                        curPlanets[intJ].position.y += curPlanets[intJ].velocity.y;
                        planetsMoving = planetsMoving || curPlanets[intJ].velocity.y != 0;
                        numCycles++;

                    }
                    else if (zMult == 0)
                    {
                        curPlanets[intJ].position.z += curPlanets[intJ].velocity.z;
                        planetsMoving = planetsMoving || curPlanets[intJ].velocity.z != 0;
                        numCycles++;
                    }
                    else
                    {
                        // done
                    }

                }
                numCycles++;
                if (!planetsMoving)
                {
                    if(xMult==0)
                    {
                        // xmult found
                        xMult = numCycles;
                        numCycles = 0;
                    }
                    else if(yMult==0)
                    {
                        yMult = numCycles;
                        numCycles = 0;

                    }
                    else if(zMult==0)
                    {
                        zMult = numCycles;
                        numCycles = 0;

                    }
                    else
                    {
                        // done
                        sw.WriteLine("Found mults = x" + xMult + " y " + yMult + " z " + zMult);
                        foundDuplicate = true;
                        break;
                    }
                }
            }
            long[] testVals = new long[3];
            testVals[0] = (long)xMult*2;
            testVals[1] = (long)yMult*2;
            testVals[2] = (long)zMult*2;

            long retVal = Helpers.LCM(testVals);
            sw.WriteLine("Finished run, energy info "+retVal*2);

            int SystemTotal = 0;
            for (int intI = 0; intI < curPlanets.Length; intI++)
            {
                int curPotential = curPlanets[intI].GetPotentialEnergy();
                int curKinetic = curPlanets[intI].GetKineticEnergy();
                int curTotal = curPotential * curKinetic;
                SystemTotal += curTotal;
                sw.WriteLine(" energy for " + intI + " Kinetic = " + curKinetic + " potential is " + curPotential + " total is " + curTotal);
            }
            sw.WriteLine("System total is " + SystemTotal);
            sw.Close();
        }
        string GetPlanetarySystemState()
        {
            string retVal = "";
            for (int intJ = 0; intJ < curPlanets.Length; intJ++)
            {
                retVal += string.Format("[{0}]{1}{2} ", intJ, curPlanets[intJ].position, curPlanets[intJ].velocity);
            }
            return retVal;
        }

        void WritePlanetStatus()
        {
            for (int intI = 0; intI < curPlanets.Length; intI++)
            {
                sw.WriteLine(string.Format("Planet{0} pos={1} vel={2}", curPlanets[intI].name, curPlanets[intI].position.ToString(), curPlanets[intI].velocity.ToString()));
            }
        }

        void UpdateXVel(PlanetaryBody p1, PlanetaryBody p2)
        {
            if (p1.position.x < p2.position.x)
            {
                p1.velocity.x += 1;
                p2.velocity.x -= 1;
            }
            if (p1.position.x > p2.position.x)
            {
                p1.velocity.x -= 1;
                p2.velocity.x += 1;
            }
        }
        void UpdateYVel(PlanetaryBody p1, PlanetaryBody p2)
        {

            if (p1.position.y < p2.position.y)
            {
                p1.velocity.y += 1;
                p2.velocity.y -= 1;
            }
            if (p1.position.y > p2.position.y)
            {
                p1.velocity.y -= 1;
                p2.velocity.y += 1;
            }
        }
        void UpdateZVel(PlanetaryBody p1, PlanetaryBody p2)
        {
            if (p1.position.z < p2.position.z)
            {
                p1.velocity.z += 1;
                p2.velocity.z -= 1;
            }
            if (p1.position.z > p2.position.z)
            {
                p1.velocity.z -= 1;
                p2.velocity.z += 1;
            }
        }
    }
            
    public class PlanetaryBody
    {
        public Vector3 position;
        public Vector3 velocity;
        public string name;

        public int GetKineticEnergy()
        {
            int retVal = 0;
            retVal += Math.Abs(velocity.x);
            retVal += Math.Abs(velocity.y);
            retVal += Math.Abs(velocity.z);
            return retVal;
        }
        public int GetPotentialEnergy()
        {
            int retVal = 0;
            retVal += Math.Abs(position.x);
            retVal += Math.Abs(position.y);
            retVal += Math.Abs(position.z);
            return retVal;
        }
    }
}
