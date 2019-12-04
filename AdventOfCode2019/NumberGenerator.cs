using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    class NumberGenerator
    {
        /*
         * It is a six-digit number.
The value is within the range given in your puzzle input.
Two adjacent digits are the same (like 22 in 122345).
Going from left to right, the digits never decrease; they only ever increase or stay the same (like 111123 or 135679).
111111 meets these criteria (double 11, never decreases).
223450 does not meet these criteria (decreasing pair of digits 50).
123789 does not meet these criteria (no double).

 */


        //235741-706948.

        //8, 9, 0, 1, 2, 3, 4, 5, 6, 7
        int minVal = 235741;
        int maxVal = 706948;
        //int lengthVal = 100000;

        //int minVal = 2357;
        //int maxVal = 7069;
        int lengthVal = 1000;

        HashSet<int> generatedNumbers = new HashSet<int>();
        HashSet<int> generatedPrefixes = new HashSet<int>();
        public void GetNextNumberInSequence(int curNum, int largestDigit)
        {
            // you pass in a number. it adds a number. 
            // if it is a finished number, it returns the number
            //otherwise it adds another number.

            for (int intI = largestDigit; intI <10; intI++ )
            {
                int startNum = curNum;                
                curNum = curNum * 10;
                curNum += intI;
                //if(generatedPrefixes.Contains(curNum))
                //{
                    //curNum = startNum;
                    //continue;
                //}
                if ((curNum / lengthVal ) > 0)
                {
                    if((curNum>=minVal)||(curNum<=maxVal))
                    {
                        generatedNumbers.Add(curNum);
                    }                    
                    curNum = startNum;
                    continue;
                }
                else
                {
                    GetNextNumberInSequence(curNum, intI);
                }
            }
            // add to the prefix
            generatedPrefixes.Add(curNum);
        }

        bool ValidateNumber(int curNum)
        {
            bool doubles = false;            
            int maxDigit = 0;
            string curCheck = curNum.ToString();
            char prevChar= ' ';
            //44 3 4444

            int repeatedCharCount = 0;

            
            for(int intI = 0; intI < curCheck.Length; intI++)
            {

                char curChar = curCheck[intI];
                if (prevChar == curChar)
                {
                    // double
                    repeatedCharCount++;
                }
                else
                {
                    if(repeatedCharCount==1)
                    {
                        doubles = true;
                    }
                    repeatedCharCount = 0;
                }
                prevChar = curChar;

                int curDigit = int.Parse((string)""+curChar);
                if(curDigit>=maxDigit)
                {
                    maxDigit = curDigit;
                }
                else
                {
                    //curDigit is  lower than previous. BAD!
                    return false;
                }
            }

            if (repeatedCharCount == 1)
            {
                doubles = true;
            }
            if (doubles)
            {
                return true;
            }
            return false;

        }
        // prefix 
        public void GenerateNewNumber()
        {

            int validCount = 0;

            // cheese mechanism
            for(int intI =minVal; intI <= maxVal; intI++)
            {
                
                if(ValidateNumber(intI))
                {
                    generatedNumbers.Add(intI);
                }
            }
            ValidateNumber(111122);
            ValidateNumber(111122);

            int t = 90;
            // 2 = 3 4 5 6 7 8 9
            // 23 4 5 6 7 8 9 
            //24 5 6 7 8 9
            
        }

    }
}
