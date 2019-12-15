using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    public struct recipeStruct
    {
        public Recipe recipe;
        public int quantity;
    }
    public struct productionStruct
    {
        public int created;
        public int consumed;
    }

    public class FuelProcessor
    {
     
        StreamWriter sw;

        
     
        Dictionary<string, int> requiredChemicals = new Dictionary<string, int>();
        Dictionary<String, int> producedChemicals = new Dictionary<string, int>();

        
        int oreUsed = 0;
        /*bool CanConsumeOre(int amount)
        {
            oreUsed += amount;
            oreAvailable -= amount;
            if(oreAvailable>0)
            {
                return true;
            }
            return false;
        }*/
        int GetAvailableChemical(string chemical)
        {
            if (producedChemicals.ContainsKey(chemical))
            {
                return producedChemicals[chemical];
            }
            return 0;
        }
        //1000000000000
        long oreCount = 1000000000000;
        int ConsumeChemical(string chemical, int amount)
        {
            if(chemical=="ORE")
            {
                oreCount = oreCount - (long) amount;                                
                //sw.WriteLine("Consumed ore " + amount);
                return 0;
            }
            int amountAvailable = 0;
            if (producedChemicals.ContainsKey(chemical))
            {
                amountAvailable = producedChemicals[chemical];
            }
            else
            {
                producedChemicals[chemical] = 0;
            }
            if(amountAvailable>=amount)
            {
                producedChemicals[chemical] = amountAvailable - amount; // all good
                return 0;
            }
            else
            {
                producedChemicals[chemical] = 0;
                return amount - amountAvailable;
            }
            
            

        }
        void AddToProducedChemicals(string produced, int amount)
        {
            int curAmount = 0;
            if(producedChemicals.ContainsKey(produced))
            {
                curAmount = producedChemicals[produced];
                producedChemicals.Remove(produced);
            }

            curAmount += amount;
            //sw.WriteLine("Produced  " + produced + " amount is " + amount + " New total is " + curAmount);
            producedChemicals.Add(produced, curAmount);
        }
        void DumpContents()
        {
            foreach (KeyValuePair<string, int> kvp in producedChemicals)
            {
                sw.WriteLine("Chemical " + kvp.Key + "=" + kvp.Value);
            }
        }
        void ProduceChemical(string recipeToCreate, int amount)
        {
            Recipe curRecipe = recipeLookup[recipeToCreate];
            // how many iterations`
            
            int numIterations = amount/ curRecipe.resultAmount;
            if((numIterations*curRecipe.resultAmount)-amount <0)
            {
                numIterations++;
            }                                    
            if(!HasOreRemaining())
            {
                return;
            }
            foreach (recipeStruct rs in curRecipe.ingredients)
            {
                int amountToMake = rs.quantity * numIterations;
                // how many of the ingredient we require

                amountToMake = ConsumeChemical(rs.recipe.name, amountToMake);
                if (amountToMake > 0)
                {
                    // actually need to make some
                    ProduceChemical(rs.recipe.name, amountToMake);
                    if (!HasOreRemaining())
                    {
                        return;
                    }
                    amountToMake = ConsumeChemical(rs.recipe.name, amountToMake);
                    if(amountToMake>0)
                    {
                        //FORKED
                        Console.WriteLine("FOREKED");
                    }
                    
                }                                
            }
            if(curRecipe.name=="ORE")
            {
                int t = 0;
            }
                
            AddToProducedChemicals(curRecipe.name, curRecipe.resultAmount * numIterations);
            if (!HasOreRemaining())
            {
                return;
            }
            //DumpContents();
        }
        public void AddRequirement(Recipe recipe, int amount)
        {
            int curAmount = 0;
            if(requiredChemicals.ContainsKey(recipe.name))
            {
                curAmount = requiredChemicals[recipe.name];
                requiredChemicals.Remove(recipe.name);                
            }
            curAmount += amount;
            requiredChemicals.Add(recipe.name, curAmount);
        }
        public Dictionary<string, Recipe> recipeLookup = new Dictionary<string, Recipe>();
        public FuelProcessor(string[] inData, string outFile)
        {
            sw = new StreamWriter(outFile);
            foreach (string inLine in inData)
            {
                //1 HKCVW, 2 DFCT => 5 ZJZRN
                SetupRecipe(inLine);                
            }
            
            recipeLookup["FUEL"].resultAmount = 1;
            recipeLookup["ORE"].resultAmount = 1;
            
            
            while(HasOreRemaining())
            {
                ProduceChemical("FUEL", 1);
            }          


            foreach (KeyValuePair<string, int> kvp in producedChemicals)
            {
                sw.WriteLine("Chemical " + kvp.Key + "=" + kvp.Value);
            }
            
            sw.Write("oreu " + oreUsed);
            sw.Close();
        }
        bool HasOreRemaining()
        {
            return oreCount > 0;
        }
        Dictionary<string, productionStruct> createdChemicals = new Dictionary<string, productionStruct>();
        public Recipe GetOrAddNewRecipe(string recipeName)
        {
            if(!recipeLookup.ContainsKey(recipeName))
            {
                Recipe newRecipe = new Recipe();
                newRecipe.name = recipeName;
                recipeLookup.Add(recipeName, newRecipe);                
            }
            return recipeLookup[recipeName];
        }
        public void SetupRecipe(string inLine)
        {
            //1 HKCVW, 2 DFCT => 5 ZJZRN
            string[] splitScratch = inLine.Split('>');
            string ingredString = splitScratch[0];
            string resultString = splitScratch[1];

            List<string> names = new List<string>();
            List<int> amounts = new List<int>();

            GetIngredientData(resultString, out names, out amounts);
            string resultName = names[0];
            int resultAmount = amounts[0];

            Recipe recipeToUpdate = GetOrAddNewRecipe(resultName);
            recipeToUpdate.resultAmount = resultAmount;
            names = new List<string>();
            amounts = new List<int>();

            GetIngredientData(ingredString, out names, out amounts);
            for (int intI = 0; intI < names.Count; intI++)
            {
                string ingredientName = names[intI];
                int ingredientAmount = amounts[intI];
                recipeStruct newIngredient = new recipeStruct();
                newIngredient.recipe = GetOrAddNewRecipe(ingredientName);
                newIngredient.quantity = ingredientAmount;
                recipeToUpdate.ingredients.Add(newIngredient);
            }
        }
        void GetIngredientData(string inData, out List<String> names, out List<int> amounts)
        {
            inData = inData.Replace("=", "");
            inData = inData.Trim();
            amounts = new List<int>();
            names = new List<string>();
            string[] splitData = inData.Split(',');
            for (int intI = 0; intI < splitData.Length; intI++)
            {
                string[] scratch = splitData[intI].Trim().Split(' ');
                amounts.Add(int.Parse(scratch[0]));
                names.Add(scratch[1]);
            }
        }
            public void AddNewRecipeData()
        {

        }
    }
    public class Recipe
    {
        
        public string name;
 
        public List<recipeStruct> ingredients = new List<recipeStruct>();
        // result is us
        public int resultAmount;

        public Recipe()
        {

        }
        public void UpdateRegisteredRecipe(string sourceData, Dictionary<string, Recipe> registrationDictionary)
        {
            // read in a string
            // if we have a recipe, great
            // otherwise, make new recipes for all referecned recipes.

        }        

    }
}
