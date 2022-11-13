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

    private static bool isRowValid(int[,] board, int value, int row)
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

    public static bool isColValid(int[,] board, int value, int col)
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

    public static bool isSquareValid(int[,] board, int value, int square)
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
        if (!isColValid(board, value, col)||
            !isRowValid(board, value, row)||
            !isSquareValid(board, value, NumberSquareQuadrant(row, col)))
        {
            return false;
        }
        return true;
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
                if (!isColValid(board, board[i, j], j) ||
                    !isRowValid(board, board[i, j], i) ||
                    !isSquareValid(board, board[i, j], NumberSquareQuadrant(i, j)))
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
        //Check to see if the root was solved
        if (root.IsSolved)
        {
            return root.boardState;
        }
        //Create a new list of child Nodes
        if (root.ChildNodes == null)
        {
            root.ChildNodes = new List<Node>();
        }
        //Create a child node to iterate at the child
        Node child = new Node
        {
            boardState = root.boardState
        };
        //Out of bounds checking for child node. If this occurs, then it returns -1,-1, meaning unsolvable
        if (root.Index.Y == 9)
        {
            root.Index.X++;
            if (root.Index.X == 9)
            {
                return new int[,]{{-1,-1}};
            }

            root.Index.Y = 0;
            child.Index.X = root.Index.X + 1;
            child.Index.Y = 0;
        }
        else
        {
            child.Index.Y = root.Index.Y + 1;
        }
        //Check to see if the value of the root was input/does not need to change(increment) and flags it is unchangeable
        if (Board[root.Index.X, root.Index.Y] != 0)
        {
            root.IsIteratable = false;
        }
        //If the root's value is 0, then iterate at the root, going to 1
        if (root.boardState[root.Index.X, root.Index.Y] == 0 &&
            Board[root.Index.X, root.Index.Y] == 0)
        {
            root.boardState[root.Index.X, root.Index.Y]++;
            return Iterate(root);
        }
        
        //Checks the legality of the root's value with Column, Row, and Square.
        if (!ValidateValue(root.boardState, root.boardState[root.Index.X,root.Index.Y], root.Index.X,root.Index.Y))
        {
            if (root.boardState[root.Index.X, root.Index.Y] == 9)
            {
                root.IsCollapsed = true;
                return root.boardState;
            }
            root.boardState[root.Index.X, root.Index.Y]++;
            return Iterate(child);
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
}
