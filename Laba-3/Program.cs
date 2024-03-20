using System;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography.X509Certificates;

class Project
{
    static void Main(string[] args)
    {
        Console.WriteLine("Через пробел X Y : ");
        List<string> a = new List<string>();
        Triangle t1 ;

        try
        {
            for (int i = 1; i <= 3; i++)
            {
                Console.Write($"Точка {i} = ");
                string[] p = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                a.AddRange(p);
            }

            t1 = new Triangle(new Point(double.Parse(a[0]), double.Parse(a[1])), new Point(double.Parse(a[2]), double.Parse(a[3])), new Point(double.Parse(a[4]), double.Parse(a[5])));

            Console.WriteLine("Площа трикутника: {0}", t1.Square()); // площа
            Console.WriteLine("Периметр трикутника = {0}", t1.Perimeter()); // периметр    
            t1.Heights();
            t1.Medians(t1.a, t1.b, "C", t1.c); // CM
            t1.Medians(t1.b, t1.c, "A", t1.a); // CM
            t1.Medians(t1.a, t1.c, "B", t1.b); // CM
            t1.IncircleRadius();
            t1.CircumcircleRadius();
            t1.TypeOfTriangle();

            t1.Rotate(45, t1.a);

            File.Ser(t1);

            File.DeSer();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        

    }
}

class Triangle
{
    private Point A;
    private Point B;
    private Point C;

    public Point a
    {
        get { return A; }
    }

    public Point b
    {
        get { return B; }
    }

    public Point c
    {
        get { return C; }
    }

    public Triangle(Point A,Point B,Point C) // конструктор
    {
        if((A.X == B.X && A.Y == B.Y) || (A.X == C.X && A.Y == C.Y) || C.X == B.X && C.Y == B.Y)
        {
            Console.WriteLine("Error , Точки не могут быть одинаковые");
            Process.GetCurrentProcess().Kill();
        }

        this.A = A;
        this.B = B;
        this.C = C;

    }

    public double Square()
    {
        return 0.5 * Math.Abs((A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y)));
    }

    public double Perimeter()
    {
         return
            (Math.Sqrt(Math.Pow(A.X - B.X,2) + Math.Pow(A.Y-B.Y,2))) +
            (Math.Sqrt(Math.Pow(C.X - A.X, 2) + Math.Pow(C.Y - A.Y, 2))) + 
            (Math.Sqrt(Math.Pow(C.X - B.X, 2) + Math.Pow(C.Y - B.Y, 2))) ;
    }

    public void Heights()
    {
        Console.WriteLine("Высота A = {0}",(2 * Square()) / B.DistanceTo(C));
        Console.WriteLine("Высота B = {0}", Math.Round(2 * Square() / A.DistanceTo(C),3));
        Console.WriteLine("Высота C = {0}", (2 * Square()) / A.DistanceTo(B));
    }

    public void Medians(Point x,Point y,string z,Point z1) // х и у это две точки где по центру точка M . z это буква куда идем медиана , z1 точка куда идет медиана
    {
        Point M = new Point((x.X + y.X)/2 ,(x.Y + y.Y )/2);

        Console.WriteLine($"Медиана M{z} = {M.DistanceTo(z1)}" );

    }

    public void IncircleRadius()
    {
        double S = Square();
        double p = Perimeter() / 2;

        // Обчислюємо радіус вписаного кола за формулою
        Console.WriteLine("Радiус вписаного кола: {0}", S / p);
    }

    public void CircumcircleRadius()
    {
        // Обчислюємо відстані між вершинами трикутника
        double AB = A.DistanceTo(B);
        double BC = B.DistanceTo(C);
        double CA = C.DistanceTo(A);

        double S = Square();

        Console.WriteLine("Радiус описаного кола: {0}",(AB * BC * CA) / (4 * S));
    }

    public void TypeOfTriangle()
    {

        if (A.DistanceTo(B) == A.DistanceTo(C) && A.DistanceTo(B) == B.DistanceTo(C)) // равностороний
        {
            Console.WriteLine("Ваш трикутник Piвносторонiй");
        }
        else if (A.DistanceTo(B) == A.DistanceTo(C) || A.DistanceTo(B) == B.DistanceTo(C) || A.DistanceTo(C) == B.DistanceTo(C)) // рiвнобедрений
        {
            Console.WriteLine("Ваш трикутник Piвнобедрений");
        }
        else if (Math.Abs((B.X - A.X) * (C.X - A.X) + (B.Y - A.Y) * (C.Y - A.Y)) == 0 || // через скалярынй добуток 
                 Math.Abs((A.X - B.X) * (C.X - B.X) + (A.Y - B.Y) * (C.Y - B.Y)) == 0 ||
                 Math.Abs((A.X - C.X) * (B.X - C.X) + (A.Y - C.Y) * (B.Y - C.Y)) == 0) // прямокутний c^2 = a^2 + b^2
        {
            Console.WriteLine("Ваш трикутник Прямокутний");
        }
        else if (Math.Pow(A.DistanceTo(B), 2) + Math.Pow(B.DistanceTo(C), 2) > Math.Pow(C.DistanceTo(A), 2) ||
            Math.Pow(B.DistanceTo(C), 2) + Math.Pow(C.DistanceTo(A), 2) > Math.Pow(A.DistanceTo(B), 2) ||
            Math.Pow(C.DistanceTo(A), 2) + Math.Pow(A.DistanceTo(B), 2) > Math.Pow(B.DistanceTo(C), 2))
        {
            Console.WriteLine("Ваш трикутник Гострий"); // гострий трикутник
        }
        else if (Math.Pow(A.DistanceTo(B), 2) + Math.Pow(B.DistanceTo(C), 2) < Math.Pow(C.DistanceTo(A), 2) ||
                 Math.Pow(B.DistanceTo(C), 2) + Math.Pow(C.DistanceTo(A), 2) < Math.Pow(A.DistanceTo(B), 2) ||
                 Math.Pow(C.DistanceTo(A), 2) + Math.Pow(A.DistanceTo(B), 2) < Math.Pow(B.DistanceTo(C), 2))

        {
            Console.WriteLine("Ваш трикутник Тупий");
        }
    }

    public void Rotate(double angleInDegrees, Point pivotPoint)
    {
        double angleInRadians = angleInDegrees * Math.PI / 180.0;

        // Перемещаем вершины треугольника в новые положения после поворота
        A = RotatePoint(A, pivotPoint, angleInRadians);
        B = RotatePoint(B, pivotPoint, angleInRadians);
        C = RotatePoint(C, pivotPoint, angleInRadians);

        // Выводим координаты вершин после поворота
        Console.WriteLine("Новые координаты вершин после поворота:");
        Console.WriteLine("A: ({0}, {1})", A.X, A.Y);
        Console.WriteLine("B: ({0}, {1})", B.X, B.Y);
        Console.WriteLine("C: ({0} ,{1})", C.X, C.Y);
    }
    private Point RotatePoint(Point point, Point pivot, double angle)
    {
        double cosTheta = Math.Cos(angle);
        double sinTheta = Math.Sin(angle);
        double x = Math.Round(cosTheta * (point.X - pivot.X) - sinTheta * (point.Y - pivot.Y) + pivot.X, 2); // Округляем до 10 знаков после запятой
        double y = Math.Round(sinTheta * (point.X - pivot.X) + cosTheta * (point.Y - pivot.Y) + pivot.Y, 2); // Округляем до 10 знаков после запятой
        return new Point(x, y);
    }

}

class Point // точки
{
    public double X { get; }
    public double Y { get; }

    public Point(double x,double y)
    {
        X = x;
        Y = y;
    }

    public double DistanceTo(Point other) // расстояние между точками
    {
        double dx = X - other.X;
        double dy = Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }


}

class File
{
    public static void Ser(Triangle g)
    {
        string json = JsonConvert.SerializeObject(g);

        StreamWriter streamWriter = new StreamWriter("laba1.json");
        streamWriter.Write(json);

        streamWriter.Close();

        Console.WriteLine("Файл в JSON");

    }

    public static Triangle DeSer()
    {    
            using (StreamReader streamReader = new StreamReader("laba1.json"))
            {
                string json = streamReader.ReadToEnd();
            Console.WriteLine("Файл дес из JSON");
            return JsonConvert.DeserializeObject<Triangle>(json);
            }
        
    }
}

