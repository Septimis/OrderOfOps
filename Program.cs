using System;

namespace OrderOfOps
{
    class Program
    {
        static void Main(string[] args)
        {
            //variables
            string algExpression;
            string errMessage = "";
            bool allGucci = true; 
            bool repeat = false;
            char[] operands = {'*', '/', '+', '-', '%'};
            int numUnclosedParenthesis = 0;
            int numUnclosedBrackets = 0;

            //Intro
            Console.WriteLine("\n\nWelcome to OrderOfOps!\nThis program takes an algebraic expression that you type into the console, tests for valid input and outputs the result.");
            
            do
            {
                //Grab input from user
                Console.Write("\nPlease input your algebraic expression: ");
                algExpression = Console.ReadLine();

                //Logic to parse through the expression and check for valid input
                //Get rid of whitespaces and replace 'x' with '*'
                algExpression = algExpression.Replace(" ", "").Replace("x", "*");

                //Check if the first & last character are operands (with the exception of a negative sign at the beginning of the statement)
                if(algExpression[0] == '+' || algExpression[algExpression.Length - 1] == '+'
                || algExpression[0] == '*' || algExpression[algExpression.Length -1 ] == '*'
                || algExpression[0] == '/' || algExpression[algExpression.Length - 1] == '/'
                || algExpression[0] == '%' || algExpression[algExpression.Length - 1] == '%'
                || algExpression[0] == '^' || algExpression[algExpression.Length - 1] == '^'
                || algExpression[algExpression.Length - 1] == '-')
                {
                    errMessage = errMessage + "\nYou cannot have an operand at the beginning or end of an expression";
                    allGucci = false;
                }

                //for loop to go through the expression and check for valid characters
                if(allGucci) { //makes sure everything so far is good before it wastes time figuring other logic out
                    for(int i = 0; i < algExpression.Length; i++)
                    {
                        //checks for invalid characters
                        if(!(Convert.ToInt32(algExpression[i]) == 37  //if it equals % (modulus)
                        || (Convert.ToInt32(algExpression[i]) >= 40 && Convert.ToInt32(algExpression[i]) <= 57) //if it equals: (  )  *  +  -  /  .  0-9
                        || Convert.ToInt32(algExpression[i]) == 91 //if it equals [
                        || Convert.ToInt32(algExpression[i]) == 93 //if it equals ]
                        || Convert.ToInt32(algExpression[i]) == 94)) //if it equals ^
                        {
                            allGucci = false;
                            errMessage = errMessage + "\nCharacter " + algExpression[i] + " at index " + i + " was invalid...";
                        }

                        //checks if there are equal number of open parenthesis as closing
                        if(algExpression[i] == '(') {
                            numUnclosedParenthesis++;
                        } else if(algExpression[i] == '[') {
                            numUnclosedBrackets++;
                        } else if(algExpression[i] == ')') {
                            numUnclosedParenthesis--;
                        } else if(algExpression[i] == ']') {
                            numUnclosedBrackets--;
                        }

                        //If, at any time, there are more closing parenthesis than opening, that's an error.
                        if(allGucci) //to prevent repeat errors
                        {
                            if(numUnclosedParenthesis < 0 || numUnclosedBrackets < 0) {
                                allGucci = false;
                                errMessage = errMessage + "\nYou cannot have more closing parenthesis than opening at any given point.";
                            }
                        }
                        

                        //checks if there are two operands next to each other
                        for(int j = 0; j < operands.Length; j++)
                        {
                            if(algExpression[i] == operands[j] || algExpression[i] == '(' || algExpression[i] == '^') {
                                for(int g = 0; g < operands.Length; g++)
                                {
                                    if(!allGucci)
                                    {
                                        break;
                                    }
                                    if(algExpression[i+1] == operands[g]  && algExpression[i] != '('){
                                        if(algExpression[i+1] == '-') {
                                            errMessage = errMessage + "\n*If you meant to indicate the number was negative, please put parenthesis around it*";
                                        }
                                        errMessage = errMessage + "\nYou had two operands next to each other: " + algExpression[i] + algExpression[i+1];
                                        allGucci = false;
                                    }                                
                                }
                            }
                        }
                    }

                    if(numUnclosedBrackets != 0 || numUnclosedParenthesis != 0) {
                        allGucci = false;
                        errMessage = errMessage + "\nYou had an unequal amount of opening and closing parenthesis";
                    }
                }

                //Checks if there are any errors that occurred in the code and spits out the error
                if(!allGucci) {
                    errorMessage(errMessage); //invoke the errorMessage function

                    //reset variables
                    errMessage = "";
                    numUnclosedParenthesis = 0;
                    numUnclosedBrackets = 0;
                    repeat = true;
                    allGucci = true;
                } else { //if everything is good to go, start solving the problem
                    repeat = false;
                }
            } while(repeat);
        }

        private static void errorMessage(string l_errMsg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n******************************************");
            Console.WriteLine("                   ERROR                    ");
            Console.WriteLine(l_errMsg);
            Console.WriteLine("\n\n*******************************************\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
