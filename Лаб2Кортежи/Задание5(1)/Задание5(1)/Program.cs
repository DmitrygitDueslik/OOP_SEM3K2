using System;

class Program
{
    static void Main()
    {
        const int size = 3;
        int[,] matrix = new int[size, size];
        Random rand = new Random();
        

        for (int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                matrix[i,j] = rand.Next(1,10);
            }
        }

        Console.WriteLine("Матрица:\n");
        for(int i = 0; i < size; i++)
        {
            for( int j = 0; j < size; j++)
            {
                Console.Write(matrix[i,j]+" ");
            }
            Console.WriteLine();
        }

        //Главная диагональ
        int diagonalSum = 0;
        for(int i=0; i < size; i++)
        {
            diagonalSum += matrix[i,i];
        }
        Console.WriteLine($"\nСумма элементов главной диагонали: {diagonalSum}");
    }
}