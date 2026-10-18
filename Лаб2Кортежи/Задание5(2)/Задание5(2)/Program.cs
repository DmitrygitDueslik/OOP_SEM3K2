using System;

class Program
{
    static(int count,double sum, double average) AnalyzeRange(double[] array,double low,double high)
    {
        int count = 0;
        double sum = 0;
        foreach (double value in array)
        {
            if(value>=low && value <= high)
            {
                count++;
                sum+= value;
            }
        }
        double average = count > 0 ? sum / count : 0;
        return (count, sum, average);
    }
    static void Main()
    {
        Random random = new Random();
        double[] numbers = new double[10];
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = Math.Round(random.NextDouble()*20-10,2);
        }

        Console.WriteLine("Массив: [" + string.Join(", ", numbers) + "]\n");
        var result = AnalyzeRange(numbers, -5, 5);

        Console.WriteLine($"Количество элементов в [-5, 5]: {result.count}");
        Console.WriteLine($"Сумма этих элементов: {result.sum:F2}");
        Console.WriteLine($"Среднее арифметическое: {result.average:F2}");


    }
}