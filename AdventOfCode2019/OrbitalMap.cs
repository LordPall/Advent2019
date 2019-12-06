using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    class OrbitalMap
    {
        Dictionary<string, OrbitalBody> bodyLookup = new Dictionary<string, OrbitalBody>();
        public void ReadSourceData(string[] inLines)
        {
            for(int intI =0;intI < inLines.Length; intI++)
            {
                string[] splitData = inLines[intI].Split(')');
                // first is orbitee, second is orbiter
                // first is who is being orbited
                // second is what's orbiteing
                // so first gets anew child, second gets new parent
                AddNewOrbitalRelationship(splitData[0], splitData[1]);
            }
            List<OrbitalBody> bottomNodes = new List<OrbitalBody>();

            foreach (KeyValuePair<string, OrbitalBody> kvp in bodyLookup)
            {

                bottomNodes.Add(kvp.Value);
                /*if (kvp.Value.IsLowestChild())
                {
                    bottomNodes.Add(kvp.Value);
                }*/
            }
            OrbitalBody you = bodyLookup["YOU"];
            OrbitalBody santa = bodyLookup["SAN"];
            OrbitalBody commonNode = GetCommonAncestor(you, santa);
            TraverseNodesUp(bottomNodes);
        }

        public int TraverseNodesUp(List<OrbitalBody> bottomNodes)
        {
            int bodyCount = 0;
            HashSet<string> bodyNames = new HashSet<string>(); // used names
            for(int intI =0; intI < bottomNodes.Count; intI++)
            {
                OrbitalBody curBody = bottomNodes[intI];

                while(curBody.GetParent()!=null)
                {                    
                    if(!bodyNames.Contains(curBody.id))
                    {
                        bodyCount++;
                        //bodyNames.Add(curBody.id);
                    }
                    curBody = curBody.GetParent();
                }
            }
            return bodyCount;
        }
        public OrbitalBody GetCommonAncestor(OrbitalBody bodyOne, OrbitalBody bodyTwo)
        {
            // build complete list first.

            Dictionary<string, int> firstPathDistances = new Dictionary<string, int>();
            OrbitalBody curBody = bodyOne.GetParent();
            int curDist = 0;
            while(curBody!=null)
            {
                firstPathDistances.Add(curBody.id, curDist);
                curDist++;
                curBody = curBody.GetParent();
            }
            int totalDist = 0;

            // now do the other one (find the common one first
            curDist = 0;
            curBody = bodyTwo.GetParent();
            while(curBody!=null)
            {
                if (firstPathDistances.ContainsKey(curBody.id))
                {
                    // ancestor in common. 
                    totalDist = firstPathDistances[curBody.id] + curDist;
                    return curBody;
                }

                curDist++;
                curBody = curBody.GetParent();
            }
            return null; 

        }
        void AddNewOrbitalRelationship(string orbiteeName, string orbiterName)
        {
            OrbitalBody orbiter;
            OrbitalBody orbitee;
            if(!bodyLookup.ContainsKey(orbiterName))
            {
                // make a new one
                orbiter = new OrbitalBody(orbiterName);
                bodyLookup[orbiterName] = orbiter;

            }
            if (!bodyLookup.ContainsKey(orbiteeName))
            {
                // make a new one
                orbitee = new OrbitalBody(orbiteeName);
                bodyLookup[orbiteeName] = orbitee;
            }
            bodyLookup[orbiteeName].SetChild(bodyLookup[orbiterName]);
            bodyLookup[orbiterName].SetParent(bodyLookup[orbiteeName]);            
        }
    }
    public class OrbitalBody
    {
        OrbitalBody parentBody = null;
        OrbitalBody childBody = null;
        public string id;               
        public OrbitalBody(string bodyName)
        {
            id = bodyName;
        }
        public void SetParent(OrbitalBody newParent)
        {
            parentBody = newParent;
        }
        public void SetChild(OrbitalBody newChild)
        {
            childBody = newChild;
        }
        public OrbitalBody GetParent()
        {
            return parentBody;
        }
        public OrbitalBody GetChild()
        {
            return childBody;
        }
        public bool IsLowestChild()
        {
            return childBody == null;
        }

    }
}

