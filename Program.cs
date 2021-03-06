﻿/*  ORDER OF OPS PROGRAM
 *  BY:                    CONNOR MEADS
 *  FOCUS:                 THIS PROGRAM WAS CREATED AS A WAY TO PRACTICE INFIX TO POSTFIX, AND FROM POSTFIX TO RESOLUTION
 *  DESCRIPTION:           THE PROGRAM ACCEPTS A STRING AND TEST IT FOR BEING A VALID ALGEBRAIC EXPRESSION, THEN IT CONVERTS THE INFIX EXPRESSION TO A POSTFIX EXPRESSION.  FINALLY, IT RESOLVES THE POSTFIX EXPRESSION TO GIVE YOU A RESULT.
 *  SPECIAL THANKS:        KEN MEADS FOR CHALLENGING ME WITH THE IDEA AND TESTING MY PROGRAM
 *  QUESTIONS & COPMMENTS: PLEASE EMAIL ME AT 'CONNOR.MEADS@PROTONMAIL.COM'
 *
 * ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 *
 *  GUIDE:
 *  VOID MAIN(): TEST FOR VALID EXPRESSION, CONVERT FROM INFIX TO POSTFIX, RESOLVE POSTFIX EXPRESSION
 *  INT PRESCEDENCE(STRING L_CHAR): RETURNS AN INTEGER ON THE PRESCEDENCE OF AN OPERATOR, WITH '+' & '-' BEING HEAVIER, '*' & '/' BEING LIGHTER, AND '^' BEING THE LIGHTEST
 *  VOID ERRORMESSAGE(STRING L_ERRMSG): PRINTS OUT ANY ERRORS ENCOUNTERED WHILE TESTING THE INPUTTED STRING TO BE AN ALGEBRAIC EXPRESSION
 */

using System;
using System.Collections.Generic;

namespace OrderOfOps
{
    class Program
    {
        static void Main(string[] args)
        {
            //variables
            string algExpression;
            List<string> infixExpression = new List<string>();
            List<string> postfixExpression = new List<string>();
            string errMessage = "";
            bool allGucci = true; 
            bool repeat = false;
            char[] operators = {'*', '/', '+', '-', '%', '^'};
            int numUnclosedParenthesis = 0;
            int numUnclosedBrackets = 0;
            Stack<string> stack = new Stack<string>();
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();

            //Intro
            Console.WriteLine("\n\nWelcome to OrderOfOps!\nThis program takes an algebraic expression that you type into the console, tests for valid input and outputs the result.");
            
            do
            {
                //Grab input from user
                Console.Write("\nPlease input your algebraic expression: ");
                algExpression = Console.ReadLine();

                //Logic to parse through the expression and check for valid input
                //Get rid of whitespaces and replace 'x' with '*'
                algExpression = algExpression.Replace(" ", "").Replace("x", "*").Replace("[", "(").Replace("]", ")");

                //Check if the first & last character are operators (with the exception of a negative sign at the beginning of the statement)
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

                        //If the expression implies multiplication such as 3(4+2)
                        if(i+1 < algExpression.Length && Convert.ToInt32(algExpression[i]) >= 48  && Convert.ToInt32(algExpression[i]) <= 57 && algExpression[i+1] == '(') {
                            algExpression = algExpression.Insert(i+1, "*");
                        }

                        //if the expression implies '0.4', but instead puts '.4'
                        if(i+1 < algExpression.Length && Convert.ToInt32(algExpression[i]) == 46) {
                            foreach(char op in operators) 
                            {
                                if(algExpression[i-1] == op || algExpression[i-1] == '(') {
                                    algExpression = algExpression.Insert(i, "0");
                                    break;
                                }
                            }
                        }

                        //checks if there are equal number of open parenthesis as closing
                        if(algExpression[i] == '(') {
                            numUnclosedParenthesis++;
                        } else if(algExpression[i] == ')') {
                            numUnclosedParenthesis--;
                        }

                        //If, at any time, there are more closing parenthesis than opening, that's an error.
                        if(allGucci) //to prevent repeat errors
                        {
                            if(numUnclosedParenthesis < 0 || numUnclosedBrackets < 0) {
                                allGucci = false;
                                errMessage = errMessage + "\nYou cannot have more closing parenthesis than opening at any given point.";
                            }
                        }
                        

                        //checks if there are two operators next to each other
                        for(int j = 0; j < operators.Length; j++)
                        {
                            if(algExpression[i] == operators[j] || algExpression[i] == '(') {
                                for(int g = 0; g < operators.Length; g++)
                                {
                                    if(!allGucci)
                                    {
                                        break;
                                    }
                                    if(algExpression[i+1] == operators[g]  && algExpression[i] != '('){
                                        if(algExpression[i+1] == '-') {
                                            errMessage = errMessage + "\n*If you meant to indicate the number was negative, please put parenthesis around it*";
                                        }
                                        errMessage = errMessage + "\nYou had two operators next to each other: " + algExpression[i] + algExpression[i+1];
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

                string temp = "";
                //convert the expression from a string to a list (to encorperate numbers >9)
                for(int i = 0; i < algExpression.Length; i++)
                {
                    if(i+1 < algExpression.Length) { //to make sure we don't go out of bounds
                        //this if statement checks to see if the current character is a number OR a . AND if the next character is a number OR a .
                        if((Convert.ToInt32(algExpression[i]) >= 48 && Convert.ToInt32(algExpression[i]) <= 57 || Convert.ToInt32(algExpression[i]) == 46)
                         && (Convert.ToInt32(algExpression[i+1]) >= 48 && Convert.ToInt32(algExpression[i+1]) <= 57 || Convert.ToInt32(algExpression[i+1]) == 46)) {
                            temp += algExpression[i];
                        } else { //This adds an item to the list if it's a single digit, an operator, or parenthesis
                            temp += algExpression[i];
                            infixExpression.Add(temp);
                            temp = "";
                        }
                    } else {
                        temp += algExpression[i];
                        infixExpression.Add(temp);
                    }
                }

                //checks if a period is displayed twice in the same number
                foreach(string i in infixExpression)
                {
                    if(Convert.ToInt32(i[0]) >= 48 && Convert.ToInt32(i[0]) <= 57) {
                        int periodCount = 0;
                        foreach(char j in i)
                        {
                            if(j == '.') {
                                periodCount++;
                            }
                            if(periodCount > 1) {
                                allGucci = false;
                                errMessage += "You had more than one decimal point in the number " + i;
                            }
                        }
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
                    infixExpression.Clear();
                } else { //if everything is good to go, start solving the problem
                    repeat = false;
                }
            } while(repeat);

            //Solve the problem
            //Convert from Infix to Postfix
            foreach(string i in infixExpression)
            {
                if(Convert.ToInt32(i[0]) >= 48 && Convert.ToInt32(i[0]) <= 57) { //If the current character is 0-9, add it to the postfix expression
                    postfixExpression.Add(i);
                } else if(i == "(") { //push opening parenthesis
                    stack.Push(i);
                } else if(i == ")") { //some logic on closing parenthesis
                    while(stack.Peek() != "(") {
                        postfixExpression.Add(stack.Pop());
                    }
                    stack.Pop();
                } else { //logic for operators
                    while(stack.Count > 0 && prescedence(i) <= prescedence(stack.Peek()) && stack.Peek() != "(")
                    {
                        postfixExpression.Add(stack.Pop());
                    }
                    stack.Push(i); //pushes the current operator onto the convertStackafter popping the higher prescedented operators
                }
            }
            while(stack.Count > 0)
            {
                postfixExpression.Add(stack.Pop());
            }
            
            Console.Write("\nInfix: " + algExpression + "  ---> Postfix: ");
            foreach(string i in postfixExpression) 
            {
                Console.Write(i + " ");
            }

            //Solve the postfix expression
            foreach(string i in postfixExpression)
            {
                double operandOne;
                double operandTwo;
                string operatorStr = "";
                double result = 0;
                if(Convert.ToInt32(i[0]) >= 48 && Convert.ToInt32(i[0]) <= 57) { //if the current character is 0-9, add it to the stack containing operands
                    operandStack.Push(Convert.ToDouble(i));
                } else { //if the current character is an operator, pop operands from the stack.  Evaluate and push the result back to the stack
                    operatorStack.Push(i);
                    if(operandStack.Count > 1) {
                        operandTwo = operandStack.Pop();
                        operandOne = operandStack.Pop();
                        operatorStr = operatorStack.Pop();

                        switch(operatorStr) {
                            case "+": 
                                result = operandOne + operandTwo;
                                break;
                            case "-":
                                result = operandOne - operandTwo;
                                break;
                            case "*":
                                result = operandOne * operandTwo;
                                break;
                            case "/":
                                result = operandOne / operandTwo;
                                break;
                            case "%":
                                result = operandOne % operandTwo;
                                break;
                            case "^":
                                result = Math.Pow(operandOne, operandTwo);
                                break;
                        }
                        operandStack.Push(result);
                    }
                }
            }

            Console.WriteLine("\n\n***FINAL RESULT***");
            Console.WriteLine(algExpression + " = " + operandStack.Pop());
        }

        private static int prescedence(string l_char) //A function to test the prescedence (or weight) of an operator
        {
            switch(l_char)
            {
                case "+":
                case "-":
                    return 1; //heaviest
                case "/":
                case "*":
                case "%":
                    return 2;
                case "^":
                    return 3; //lightest
            }
            return -1; //if, for whatever reason, the character isn't an operator
        }

        private static void errorMessage(string l_errMsg)  //A function to output an error, usually an error associated with incorrect input
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
