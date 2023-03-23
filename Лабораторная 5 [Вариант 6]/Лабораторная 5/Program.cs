using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
abstract class Polygon
{
    public string Type;
    public string Color;
    public int[] Cords;
    public Polygon()
    {
        Type = Color = null;
    }
    public Polygon(string Type, string Color, int[] Cords)
    {
        this.Type = Type;
        this.Color = Color;
        this.Cords = Cords;
    }
    abstract public double Square();
    abstract public double Perimeter();
    abstract public void PrintInfo();
}
class Rectangle : Polygon
{
    public Rectangle(string Color, int[] Cords) : base("Прямоугольник", Color, Cords){}
    override public double Square()
    {
        return Math.Sqrt((Math.Pow(Cords[2] - Cords[0], 2) + Math.Pow(Cords[3] - Cords[1], 2)) * (Math.Pow(Cords[4] - Cords[2], 2) + Math.Pow(Cords[5] - Cords[3], 2)));
    }
    override public double Perimeter()
    {
        return 2 * Math.Sqrt(Math.Pow(Cords[2] - Cords[0], 2) + Math.Pow(Cords[3] - Cords[1], 2)) * Math.Sqrt(Math.Pow(Cords[4] - Cords[2], 2) * Math.Pow(Cords[5] - Cords[3], 2));
    }
    public override void PrintInfo()
    {
        Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Color);
        Console.WriteLine("║{0,13}║{1,10:.##}║{2,10:.##}║{3,10}║", Type, Square(), Perimeter(), Color);
        Console.ForegroundColor = ConsoleColor.White;
    }
}
class Triangle : Polygon
{
    public Triangle(string Color, int[] Cords): base("Треугольник", Color, Cords){}
    override public double Square()
    {
        return 0.5 * Math.Abs((Cords[0] - Cords[4]) * (Cords[3] - Cords[5]) - (Cords[2] - Cords[4]) * (Cords[1] - Cords[5]));
    }
    public override double Perimeter()
    {
        return Math.Sqrt(Math.Pow(Cords[2] - Cords[0], 2) + Math.Pow(Cords[3] - Cords[1], 2)) + Math.Sqrt(Math.Pow(Cords[4] - Cords[0], 2) + Math.Pow(Cords[5] - Cords[1], 2)) + Math.Sqrt(Math.Pow(Cords[4] - Cords[2], 2) + Math.Pow(Cords[5] - Cords[3], 2));
    }
    public override void PrintInfo()
    {
        double A = Math.Sqrt(Math.Pow(Cords[2] - Cords[0], 2) + Math.Pow(Cords[3] - Cords[1], 2)),
        B = Math.Sqrt(Math.Pow(Cords[4] - Cords[0], 2) + Math.Pow(Cords[5] - Cords[1], 2)),
        C = Math.Sqrt(Math.Pow(Cords[4] - Cords[2], 2) + Math.Pow(Cords[5] - Cords[3], 2));
        Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Color);
        for (int i = 0; i < Cords.Length; i += 2)
            if (Cords[i] < 0 && Cords[i + 1] >= 0 && (Math.Pow(A, 2) + Math.Pow(B, 2) == Math.Pow(C, 2) || Math.Pow(B, 2) + Math.Pow(C, 2) == Math.Pow(A, 2) || Math.Pow(A, 2) + Math.Pow(C, 2) == Math.Pow(B, 2))) Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("║{0,13}║{1,10:.##}║{2,10:.##}║{3,10}║", Type, Square(), Perimeter(), Color);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}
class Sort : IComparer
{
    public int Compare(object Obj, object obj)
    {
        Polygon p = Obj as Polygon;
        Polygon p2 = obj as Polygon;
        if (p2.Square() > p.Square()) return 1;
        else
        {
            if (p2.Square() == p.Square()) return 0;
            else return -1;
        }
    }
}

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите имя файла");
            try
            {
                string s = Console.ReadLine();
                StreamReader f = new StreamReader(s, Encoding.Default);
                StreamReader file = new StreamReader(s, Encoding.Default);
                int count = 0;
                while (file.ReadLine() != null) count++;
                Polygon[] obj = new Polygon[count];
                string line;
                int i, j = 0;
                while ((line = f.ReadLine()) != null)
                {
                    i = 0;
                    string temp = "";
                    while (!(Char.IsDigit(line[i])))
                    {
                        if (line[i] == '‐') break;
                        temp = temp + line[i];
                        i++;
                    }
                    temp = temp.Trim().ToLower();
                    int[] tmparr = new int[10];
                    int k = 0;
                    string tmp = "";
                    while (!Char.IsLetter(line[i]))
                    {
                        tmp = "";
                        while ((line[i] == '‐') || (Char.IsDigit(line[i])))
                        {
                            tmp = tmp + line[i];
                            i++;
                        }
                        if (tmp != "")
                        {
                            try
                            {
                                if ((tmparr[k] += int.Parse(tmp)) != 0) k++;
                            }
                            catch (IndexOutOfRangeException)
                            {
                                break;
                            }
                        }
                        i++;
                    }
                    string color = "";
                    while (i < line.Length)
                    {
                        color = color + line[i];
                        i++;
                    }
                    if (String.Compare(temp, "треугольник") == 0)
                    {
                        obj[j] = new Triangle(color, tmparr);
                        j++;
                    }
                    else
                    if (String.Compare(temp, "прямоугольник") == 0)
                    {
                        obj[j] = new Rectangle(color, tmparr);
                        j++;
                    }
                }
                f.Close();
                Console.WriteLine("╔═════════════╦══════════╦══════════╦══════════╗");
                Console.WriteLine("║  Тип фигуры ║  Площадь ║ Периметр ║   Цвет   ║");
                Console.WriteLine("╠═════════════╬══════════╬══════════╬══════════╣");
                for (i = 0; i < j; i++)
                    obj[i].PrintInfo();
                Console.WriteLine("╚═════════════╩══════════╩══════════╩══════════╝");
                Array.Sort(obj, new Sort());
                Console.WriteLine("╔═════════════╦══════════╦══════════╦══════════╗");
                Console.WriteLine("║  Тип фигуры ║  Площадь ║ Периметр ║   Цвет   ║");
                Console.WriteLine("╠═════════════╬══════════╬══════════╬══════════╣");
                for (i = 0; i < j; i++)
                    obj[i].PrintInfo();
                Console.WriteLine("╚═════════════╩══════════╩══════════╩══════════╝");
            }
            catch (FileNotFoundException e)
            { Console.WriteLine(e.Message); }
            Console.ReadKey();
        }
    }
}