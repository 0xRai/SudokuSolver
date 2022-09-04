namespace SudokuSolver;

public class Program
{
    public static void Main(string[] args)
    {
        //create board

        int[,] board = NewBoard();

        //show empty board
        PrintBoard(board);

        //user inputs known numbers
        InputNumbers(board);

        //perform the magic
        SolveBoard(board);

    }

    public static int[,] NewBoard()
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

    public static void PrintBoard(int[,] board)
    {

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
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
    }

    public static void InputNumbers(int[,] board)
    {
        Console.WriteLine(
            "Please input 1-9. If the number unknown, input 0. Board reads from top to bottom, left to right");
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                bool isConverted = false;
                string userInput = "";
                do
                {
                    Console.Write($"What is the number for [{i + 1},{j + 1}]: ");
                    userInput = Console.ReadLine();
                    int num;
                    isConverted = int.TryParse(userInput, out num);
                    if (isConverted && num < 10 && num >= 0)
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

    public static void SolveBoard(int[,] board)
    {
        int[,] newBoard = board;
        SolveRow(newBoard);
        // SolveCol(newBoard);
        // SolveSquare(newBoard);
    }

    public static bool SolveRow(int[,] board)
    {
        int[] numCounts = CountMatrix();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                numCounts[board[i, j]]++;
            }
        }

        return false;
    }

    public static bool SolveCol(int[,] board)
    {
        return false;
    }

    public static bool SolveSquare(int[,] board)
    {
        return false;
    }

    public static int[] CountMatrix()
    {
        /*
         Creates array. Instead of using 2d array for {key, value},
         using single array where the index doubles as the key
         */
        int[] numCount = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        return numCount;
    }
}
