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
        
        bool isSeedValidation = ValidateBoard(ValidationSeed());
        Console.WriteLine(isSeedValidation);

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

                if ((i == 3 && j == 0) || (i == 6 && j == 0))
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

    private static bool IsRowValid(int[,] board, int value, int row, int col)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[row, i] == value && i != col)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsColValid(int[,] board, int value, int row, int col)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[i, col] == value && i != row)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsSquareValid(int[,] board, int value, int square, int row, int col)
    {
        //Square's x and y
        int x = 0;
        int y = 0;
        switch (square)
        {
            case 0:
                x = 0;
                y = 0;
                break;
            case 1:
                x = 0;
                y = 3;
                break;
            case 2:
                x = 0;
                y = 6;
                break;
            case 3:
                x = 3;
                y = 0;
                break;
            case 4:
                x = 3;
                y = 3;
                break;
            case 5:
                x = 3;
                y = 6;
                break;
            case 6:
                x = 6;
                y = 0;
                break;
            case 7:
                x = 6;
                y = 3;
                break;
            case 8:
                x = 6;
                y = 6;
                break;
        }

        for (int i = x; i < x + 3; i++)
        {
            for (int j = y; j < y + 3; j++)
            {
                if (board[i, j] == value && i != row && j != col)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool ValidateValue(int[,] board, int value, int row, int col)
    {
        return IsColValid(board, value, row, col) &&
               IsRowValid(board, value, row, col) &&
               IsSquareValid(board, value, NumberSquareQuadrant(row, col), row, col);
    }

    public static int[,] ValidationSeed()
    {
        int[,] seed = new int[,]
        {
            { 4, 6, 7, 9, 2, 1, 3, 5, 8, }, { 8, 9, 5, 4, 7, 3, 2, 6, 1 }, { 2, 3, 1, 8, 6, 5, 7, 4, 9 },
            { 5, 1, 3, 6, 9, 8, 4, 2, 7 }, { 9, 2, 8, 7, 5, 4, 6, 1, 3 }, { 7, 4, 6, 1, 3, 2, 9, 8, 5 },
            { 3, 5, 4, 2, 8, 7, 1, 9, 6 }, { 1, 8, 9, 3, 4, 6, 5, 7, 2 }, { 6, 7, 2, 5, 1, 9, 8, 3, 4 }
        };
        return seed;
    }

    public static void Seed(int[,] board)
    {
        //Hard coded seed
        board[0, 2] = 4;
        board[0, 6] = 8;
        board[1, 0] = 7;
        board[1, 1] = 2;
        board[1, 3] = 3;
        board[1, 4] = 5;
        board[2, 4] = 4;
        board[2, 5] = 7;
        board[2, 8] = 3;
        board[3, 0] = 4;
        board[3, 1] = 8;
        board[3, 5] = 1;
        board[3, 6] = 3;
        board[4, 1] = 6;
        board[4, 2] = 7;
        board[4, 6] = 1;
        board[4, 8] = 2;
        board[5, 3] = 5;
        board[5, 8] = 7;
        board[6, 1] = 7;
        board[6, 2] = 3;
        board[6, 3] = 8;
        board[6, 4] = 1;
        board[6, 5] = 5;
        board[6, 7] = 4;
        board[6, 8] = 9;
        board[7, 0] = 8;
        board[7, 1] = 9;
        board[7, 2] = 2;
        board[7, 4] = 7;
        board[7, 5] = 6;
        board[7, 7] = 3;
        board[7, 8] = 1;
        board[8, 3] = 2;
        board[8, 7] = 6;
        board[8, 8] = 8;
    }

    public static int NumberSquareQuadrant(int x, int y)
    {
        int i = x / 3;
        int j = y / 3;
        return j + (i * 3);
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
                if (!ValidateValue(board,board[i,j], i,j))
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
        if (root.IsCollapsed && root.IsIteratable)
        {
            if (root.GetNodeBoardStateValue(root) == 9)
            {
                return root.boardState;
            }

            Node incrementRoot = root;
            incrementRoot.ChildNodes = null;
            incrementRoot.IsCollapsed = false;
            incrementRoot.IncrementNodeBoardStateValue(incrementRoot);
            Iterate(incrementRoot);
        }

        //Check if the root is valid (no unique numbers, ignoring 0s)
        bool isRootValid =
            ValidateValue(root.boardState, root.GetNodeBoardStateValue(root), root.Index.X, root.Index.Y);
        //if the root value is not valid, then we must iterate if possible, or we must return to the previous root
        if (!isRootValid && root.IsIteratable)
        {
            if (root.GetNodeBoardStateValue(root) < 9)
            {
                root.IncrementNodeBoardStateValue(root);
                Iterate(root);
            }

            if (root.GetNodeBoardStateValue(root) == 9)
            {
                return root.boardState;
            }
        }

        
        if (!isRootValid && !root.IsIteratable)
        {
            return root.boardState;
        }
        //if root is valid but not solved, then we must setup to go to a lower depth
        Node child;
        // Check to see if there is any list of Child nodes
        if (root.ChildNodes == null)
        {
            child = new Node
            {
                boardState = root.boardState
            };
            //Set the new x and y pointers and verifies that is not out of range of [8,8]
            if (root.Index.Y == 8)
            {
                if (root.Index.X == 8)
                {
                    return new int[,] { { -1, -1 } }; // Shows that the pointers are at [8,8] and cannot go further
                }

                child.Index.X = root.Index.X + 1;
                child.Index.Y = 0;
            }
            else
            {
                child.Index.X = root.Index.X;
                child.Index.Y = root.Index.Y + 1;
            }

            root.ChildNodes = new List<Node> { child };
            Iterate(child);

        }
        else // If the childNode list exists, then the current child will be the last child indexed in the list
        {
            child = root.ChildNodes[^1];
        }
        //Check to see if child Node is valid, if not then we must increment if iterable
        bool isChildValid = ValidateValue(child.boardState, child.GetNodeBoardStateValue(child), child.Index.X,
            child.Index.Y);
        child.IsIteratable = Board[child.Index.X, child.Index.Y] != 0;
        //If the child node is invalid and iterable, then we can add an increment child node and reiterate root
        if (!isChildValid && child.IsIteratable)
        {
            Node incrementChild = child;
            incrementChild.ChildNodes = null;
            if (incrementChild.GetNodeBoardStateValue(incrementChild) < 9)
            {
                incrementChild.IncrementNodeBoardStateValue(incrementChild);
                Iterate(incrementChild);
            }
            //If the next child would be over 9, then the root is collapsed and cannot go further
            if (incrementChild.GetNodeBoardStateValue(incrementChild) == 9)
            {
                root.IsCollapsed = true;
                return root.boardState;
            }
            
        }
        //If the child is valid and not collapsed, then we lower the depth
        if (isChildValid && !child.IsCollapsed)
        {
            Iterate(child);
        }
        

        if (!root.IsIteratable)
        {
            return root.boardState;
        }

        if (root.IsIteratable)
        {
            Node incrementRoot = root;
            incrementRoot.ChildNodes = null;
            incrementRoot.IsCollapsed = false;
            incrementRoot.IncrementNodeBoardStateValue(incrementRoot);
            Iterate(incrementRoot);
        }

        Console.WriteLine("Returning");
        if (!ValidateBoard(root.boardState)) return root.boardState;
        root.IsSolved = true;
        Iterate(root);

        return root.boardState;
    }
}

public class Node
{
    public int[,] boardState;
    public (int X, int Y) Index;
    // public (int X, int Y) ChildIndex;
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