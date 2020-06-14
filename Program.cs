using System;

namespace OrderOfOps
{
    class Program
    {
        static void Main(string[] args)
        {
            //variables
            string algExpression;
            bool allGucci = true; 

            //Intro
            Console.WriteLine("\n\nWelcome to OrderOfOps!\n\nThis program takes an algebraic expression that you type into the console, tests for valid input and outputs the result.");
            
            //Grab input from user
            Console.Write("Please input your algebraic expression: ");
            algExpression = Console.ReadLine();

            //Logic to parse through the expression and check for valid input
            //for loop to go through the expression and check for valid characters
            for(int i = 0; i < algExpression.Length; i++)
            {
                if(Convert.ToInt32(algExpression[i]) == 37  //if it equals % (modulus)
                || (Convert.ToInt32(algExpression[i]) >= 40 && Convert.ToInt32(algExpression[i]) <= 57) //if it equals: (  )  *  +  -  /  .  0-9
                || Convert.ToInt32(algExpression[i]) == 91 //if it equals [
                || Convert.ToInt32(algExpression[i]) == 93) //if it equals ]
                {
                    Console.Write("  Correct Character: " + i);
                } else {
                    allGucci = false;
                    Console.WriteLine("\n\nCharacter: " + i + " was invalid...");
                    break;
                }
            }
            
        }
    }
}
