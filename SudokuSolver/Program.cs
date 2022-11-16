using static SudokuSolver.Seed;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SudokuSolver;

public class Program
{
    private static int[,] _board = NewBoard();
    private static int[,] _solvedBoard = NewBoard();

    public static void Main(string[] args)
    {
        //user inputs known numbers
        if (args.Length == 0)
        {
            //show empty board
            PrintBoard(_board);
            InputNumbers(_board);
        }

        if (args.Length == 1)
        {
            char[] charArr = args[0].ToCharArray();
            if (charArr.Length == 81)
            {
                ConvertInputToBoard(charArr);
            }
            else
            {
                Console.WriteLine($"Invalid input: input is only {charArr.Length} characters long, input must be \n81 characters long");
                Environment.Exit(0);
            }
        }

        Console.WriteLine("Waiting 3 seconds");
        PrintBoard(_board);
        Thread.Sleep(3000);
        
        Console.WriteLine("Solving...");
        _solvedBoard = SolveBoard(_board);

        Console.BackgroundColor = ConsoleColor.Red;
        Console.Write("Board is not solvable!");
        Console.ResetColor();
        Console.WriteLine();
        Environment.Exit(0);
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

    public static void ConvertInputToBoard(char[] charArr)
    {
        int x = 0;
        bool isConverted;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int num;
                isConverted = int.TryParse(charArr[x].ToString(), out num);
                if (isConverted && num is < 10 and >= 0)
                {
                    _board[i, j] = num;
                    x++;
                }
                else
                {
                    Console.WriteLine($"Invalid input: character \"{charArr[x]}\" is not a number");
                    Environment.Exit(0);
                }
            }
        }
    }

    /// <summary>
    /// Function <c>SolveBoard</c> takes the inputted board, transfers to the Root Node and solves through
    /// Binary Search Tree
    /// </summary>
    private static int[,] SolveBoard(int[,] board)
    {
        //Creates the root node from the input and then sets to start from beginning
        //(x: 0, y: 0) and then set pointers to child's index (x: 0, y: 1)
        Node root = new Node
        {
            boardState = Node.CopyBoardState(board)
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

    public static bool ValidateValue(Tuple<int[,], int, int, int> tuple)
    {
        return IsColValid(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4) &&
               IsRowValid(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4) &&
               IsSquareValid(tuple.Item1, tuple.Item2, NumberSquareQuadrant(tuple.Item3, tuple.Item4), tuple.Item3,
                   tuple.Item4);
    }
    
    public static bool ValidateBoard(int[,] board)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (!ValidateValue(board, board[i, j], i, j))
                {
                    return false;
                }
            }
        }

        return true;
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

    public static bool ValidateValueWithBoard(int[,] originalBoard, int[,] newBoard)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (originalBoard[i, j] != newBoard[i, j] && originalBoard[i, j] != 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static int[,] Iterate(Node root, bool isSolved = false, int[,] solvedBoard = null)
    {
        while (!isSolved)
        {
            //Check to see if the root was solved(There is no 0s left and no repeating numbers in the col, row, and square)
            if (root.IsSolved)
            {
                return root.boardState;
            }

            if (ValidateBoard(root.boardState) && IsBoardFull(root.boardState) && ValidateValueWithBoard(_board, root.boardState))
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("SOLVED!");
                Console.WriteLine();
                Console.ResetColor();
                PrintBoard(root.boardState);
                Environment.Exit(1);
            }

            //If the current root value is zero, increment
            if (root.GetNodeBoardStateValue(root) == 0)
            {
                root.IncrementNodeBoardStateValue(root);
                Iterate(root);
            }


            //Set bool to if the root is iterable
            root.IsIteratable = _board[root.Index.X, root.Index.Y] == 0;

            //Check if the root is collapsed and iterable
            // If it is both, then create a root node of the value incremented (value = value + 1) 
            if (root.IsCollapsed && root.IsIteratable)
            {
                if (root.GetNodeBoardStateValue(root) >= 9)
                {
                    return root.boardState;
                }


                Node incrementRoot = root;
                incrementRoot.boardState = Node.CopyBoardState(root.boardState);
                incrementRoot.ChildNodes = null;
                incrementRoot.IsCollapsed = false;
                if (incrementRoot.GetNodeBoardStateValue(incrementRoot) >= 9)
                {
                    return root.boardState;
                }

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

                if (root.GetNodeBoardStateValue(root) >= 9)
                {
                    root.IsCollapsed = true;
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
                    boardState = Node.CopyBoardState(root.boardState)
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
            child.IsIteratable = _board[child.Index.X, child.Index.Y] != 0;
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
                if (incrementChild.GetNodeBoardStateValue(incrementChild) >= 9)
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
                incrementRoot.boardState = Node.CopyBoardState(root.boardState);
                incrementRoot.ChildNodes = null;
                incrementRoot.IsCollapsed = false;
                if (incrementRoot.GetNodeBoardStateValue(incrementRoot) >= 9)
                {
                    return root.boardState;
                }

                incrementRoot.IncrementNodeBoardStateValue(incrementRoot);
                Iterate(incrementRoot);
            }

            if (ValidateBoard(root.boardState))
            {
                Console.WriteLine("SOLVED!!!");
                System.Threading.Thread.Sleep(60000);
                root.IsSolved = true;
                Iterate(root);
            }

            return root.boardState;
        }

        Console.WriteLine("Returning...");
        PrintBoard(solvedBoard);
        return solvedBoard;
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

    public Tuple<int[,], int, int, int> PassNodeValue(Node node)
    {
        return Tuple.Create(node.boardState, node.GetNodeBoardStateValue(node), node.Index.X, node.Index.Y);
    }

    public static int[,] CopyBoardState(int[,] board)
    {
        int[,] newBoard = new int[9, 9];
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                newBoard[i, j] = board[i, j];
            }
        }

        return newBoard;
    }
}