using System.ComponentModel;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;

namespace SudokuSolver;

public class Program
{
    private static readonly int[,] Board = NewBoard();
    public static void Main(string[] args)
    {

        //show empty board
        PrintBoard(Board);

        //user inputs known numbers
        // InputNumbers(board);
        Console.WriteLine("Validating board...");
        bool isSolvable = ValidateBoard(Board);
        
        
        //Verifies if user did not inputted a VALID
        
        Seed(Board);
        PrintBoard(Board);
        Console.WriteLine("Solving...");
        int[,] solvedBoard = SolveBoard();
        isSolvable = ValidateBoard(solvedBoard);
        if (isSolvable)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("SOLVED!");
            Console.ResetColor();
            Console.WriteLine();
            PrintBoard(Board);
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("Board is not solvable!");
            Console.ResetColor();
            Console.WriteLine();
        }
        

    }

    private static int[,] NewBoard()
    {
        int[,] board = new int[9, 9];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                board[i, j] = 0;
            }
        }
        return board;
    }

    private static void PrintBoard(int[,] board)
    {

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (j == 3 || j == 6)
                {
                    Console.Write("| ");
                }
                if ((i == 3 && j == 0) || (i == 6 && j == 0) )
                {
                    Console.WriteLine("- - - + - - - + - - - ");
                }
                if (j == 8)
                {
                    if (board[i, j] == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write($"{board[i, j]}");
                        Console.ResetColor();
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine($"{board[i, j]}");
                    }
                }
                else
                {
                    if (board[i, j] == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write($"{board[i, j]}");
                        Console.ResetColor();
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write($"{board[i, j]} ");
                    }
                }
            }
        }
        Console.WriteLine();
    }

    private static void InputNumbers(int[,] board)
    {
        Console.WriteLine(
            "Please input 1-9. If the number unknown, input 0. Board reads from top to bottom, left to right");
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                bool isConverted;
                do
                {
                    Console.Write($"What is the number for [{i + 1},{j + 1}]: ");
                    var userInput = Console.ReadLine();
                    int num;
                    isConverted = int.TryParse(userInput, out num);
                    if (isConverted && num is < 10 and >= 0)
                    {
                        board[i, j] = num;
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input\n");
                        isConverted = false;

                    }

                    PrintBoard(board);
                } while (!isConverted);
            }
        }

        Console.WriteLine("Solving Board...");
    }
    /// <summary>
    /// Function <c>SolveBoard</c> takes the inputted board, transfers to the Root Node and solves through
    /// Binary Search Tree
    /// </summary>
    private static int[,] SolveBoard()
    {
        //Creates the root node from the input and then sets to start from beginning
        //(x: 0, y: 0) and then set pointers to child's index (x: 0, y: 1)
        Node root = new Node
        {
            boardState = Board
        };
        root.Index.X = 0;
        root.Index.Y = 0;
        return Iterate(root);
        
    }

    private static bool IsRowValid(int[,] board, int value, int row)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[row, i] == value)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsColValid(int[,] board, int value, int col)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[i, col] == value)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsSquareValid(int[,] board, int value, int square)
    {
        //Square's x and y
        int x = 0;
        int y = 0;
        switch (square)
        {  
            case 0: 
                x = 0;
                y = 0; break;
            case 1: 
                x = 0;
                y = 3; break;
            case 2:
                x = 0;
                y = 6; break;
            case 3:
                x = 3;
                y = 0; break;
            case 4:
                x = 3;
                y = 3; break;
            case 5:
                x = 3;
                y = 6; break;
            case 6: 
                x = 6;
                y = 0; break;
            case 7: 
                x = 6;
                y = 3; break;
            case 8:
                x = 6;
                y = 6; break;
            
        }

        for (int i = x; i < x + 3; i++)
        {
            for (int j = y; j < y + 3; j++)
            {
                if (board[i, j] == value)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool ValidateValue(int[,] board, int value, int row, int col)
    {
        return IsColValid(board, value, col) &&
               IsRowValid(board, value, row) &&
               IsSquareValid(board, value, NumberSquareQuadrant(row, col));
    }
    
    public static void Seed(int[,] board)
    {
        //Hard coded seed
        board[0, 2] = 4; board[0, 6] = 8; board[1, 0] = 7;
        board[1, 1] = 2; board[1, 3] = 3; board[1, 4] = 5; 
        board[2, 4] = 4; board[2, 5] = 7; board[2, 8] = 3;
        board[3, 0] = 4; board[3, 1] = 8; board[3, 5] = 1; 
        board[3, 6] = 3; board[4, 1] = 6; board[4, 2] = 7;
        board[4, 6] = 1; board[4, 8] = 2; board[5, 3] = 5;
        board[5, 8] = 7; board[6, 1] = 7; board[6, 2] = 3;
        board[6, 3] = 8; board[6, 4] = 1; board[6, 5] = 5;
        board[6, 7] = 4; board[6, 8] = 9; board[7, 0] = 8; 
        board[7, 1] = 9; board[7, 2] = 2; board[7, 4] = 7;
        board[7, 5] = 6; board[7, 7] = 3; board[7, 8] = 1;
        board[8, 3] = 2; board[8, 7] = 6; board[8, 8] = 8;

    }

    public static int NumberSquareQuadrant(int x, int y)
    {
        int i = x / 3;
        int j = y / 3;
        return j + (i*3);
    }

    public static bool IsBoardFull(int[,] board)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j] == 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool ValidateBoard(int[,] board)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (!IsColValid(board, board[i, j], j) ||
                    !IsRowValid(board, board[i, j], i) ||
                    !IsSquareValid(board, board[i, j], NumberSquareQuadrant(i, j)))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static int[,] Iterate(Node root)
    {
        Console.WriteLine($"Iterating at {root.Index.X}, {root.Index.Y}");
        PrintBoard(root.boardState);
        //Check to see if the root was solved(There is no 0s left and no repeating numbers in the col, row, and square)
        if (root.IsSolved)
        {
            return root.boardState;
        }
        //If the current root value is zero, increment
        if (root.GetNodeBoardStateValue(root) == 0)
        {
            root.IncrementNodeBoardStateValue(root);
            Iterate(root);
        }
        
        
        //Set bool to if the root is iterable
        root.IsIteratable = Board[root.Index.X, root.Index.Y] != 0;
        
        //Check if the root is collapsed and iterable
        // If it is both, then create a root node of the value incremented (value = value + 1) 
        if (root.IsCollapsed&&root.IsIteratable)
        {
            if (root.GetNodeBoardStateValue(root) == 9)
            {
                return root.boardState;
            }

            root.IncrementNodeBoardStateValue(root);
            Iterate(root);
        }
        
        //Check if the root is valid (no unique numbers, ignoring 0s)
        bool isRootValid =
            ValidateValue(root.boardState, root.GetNodeBoardStateValue(root), root.Index.X, root.Index.Y);
        //if the root value is not valid, then we must iterate if possible, or we must return to the previous root
        if (isRootValid && root.IsIteratable)
        {
            if (root.GetNodeBoardStateValue(root) < 9)
            {
                root.IncrementNodeBoardStateValue(root);
                Iterate(root);
            }
        }
        

        if (!isRootValid && !root.IsIteratable)
        {
            return root.boardState;
        }
        

        if (!root.IsIteratable)
        {
            return root.boardState;
        }

        Console.WriteLine("Returning");
        if (ValidateBoard(root.boardState))
        {
            root.IsSolved = true;
            Iterate(root);
        }
        return root.boardState;
    }
}

public class Node
{
    public int[,] boardState;
    public (int X, int Y) Index;
    public (int X, int Y) ChildIndex;
    public bool IsIteratable = true;
    public bool IsSolved = false;
    public bool IsCollapsed = false;
    public List<Node>? ChildNodes;

    public int GetNodeBoardStateValue(Node node)
    {
        return node.boardState[node.Index.X, node.Index.Y];
    }

    public void IncrementNodeBoardStateValue(Node node)
    {
        node.boardState[node.Index.X, node.Index.Y]++;
    }
}
