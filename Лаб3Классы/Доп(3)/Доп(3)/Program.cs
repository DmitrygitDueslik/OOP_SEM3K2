using System;

class Document
{
    private static int nextId = 1; // статический счетчик следующего ID

    public int id;
    public string title;

    public Document(string title)
    {
        this.title = title;
        this.id = nextId; // присваиваем текущий следующий ID
        nextId++;          // увеличиваем счетчик для следующего документа
    }

    public static void ResetIdCounter()
    {
        nextId = 1;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Документ #{id}: \"{title}\"");
    }
}

class Program
{
    static void Main()
    {
        Document doc1 = new Document("Отчёт");
        Document doc2 = new Document("Приказ");
        Document doc3 = new Document("Служебная записка");

        doc1.PrintInfo();
        doc2.PrintInfo();
        doc3.PrintInfo();

        Document.ResetIdCounter();
        Console.WriteLine("\nСчётчик сброшен\n");

        Document doc4 = new Document("Новый документ");
        doc4.PrintInfo();
    }
}
