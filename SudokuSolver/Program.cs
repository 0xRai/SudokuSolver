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
        Seed(Board);
        PrintBoard(Board);
        //perform the magic
        SolveBoard(Board);
        Console.WriteLine("SOLVED!");
        PrintBoard(Board);

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
                    Console.WriteLine($"{board[i, j]}");
                }
                else
                {
                    Console.Write($"{board[i, j]} ");
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

    private static void SolveBoard(int[,] board)
    {

        int iteration = 1;
        while (!IsBoardFull(board))
        {
            Console.WriteLine(iteration);
            int[,] newBoard = board;
        
            //Find the first zero number
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (newBoard[i,j] != 0) continue;
                    //Calculate number's square quadrant
                    int rowIndex = i;
                    int colIndex = j;
                    int squareIndex = NumberSquareQuadrant(rowIndex, colIndex);

                    int[,] numTable = CountMatrix();
                    CountRow(newBoard, numTable, rowIndex);
                    CountCol(newBoard, numTable, rowIndex);
                    CountSquare(newBoard, numTable, squareIndex);
                
                    //Find the first non-zero in the number table
                    for (int k = 1; k < 10; k++)
                    {
                        if (numTable[k, 0] == 0 && numTable[k, 1] == 0 && numTable[k, 2] == 0)
                        {
                            newBoard[i, j] = k;
                            numTable[k, 0]++;
                            numTable[k, 1]++;
                            numTable[k, 2]++;
                            break;
                        }
                    }
                }
            }
            PrintBoard(newBoard);
            iteration++;
        }
    }

    private static void CountRow(int[,] board, int[,]numCounts, int row)
    {
        for (int i = 0; i < 9; i++)
        {
            numCounts[board[row, i],0]++;
        }
    }

    public static void CountCol(int[,] board, int[,]numCounts, int col)
    {
        for (int i = 0; i < 9; i++)
        {
            numCounts[board[i, col],1]++;
        }
    }

    public static void CountSquare(int[,] board, int[,]numCounts, int square)
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
                numCounts[board[i, j],2]++;
            }
        }
    }

    public static int[,] CountMatrix()
    {
         //                      x y z
         //                     {0,0,0}
         int[,] numCount = { {0,0,0},{0,0,0},{0,0,0},
                            {0,0,0},{0,0,0},{0,0,0},
                            {0,0,0},{0,0,0},{0,0,0},
                            {0,0,0} };
        return numCount;
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
}
