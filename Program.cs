using System;
using System.IO;

namespace Person1
{
    class Person
    {
        const int l_name = 30;
        string name;
        int birth_year;
        double pay;

        public Person() // конструктор без параметрів
        {
            name = "Anonimous";
            birth_year = 0;
            pay = 0;
        }

        public Person(string name, int birthYear, double salary) // конструктор з параметрами
        {
            this.name = name.Length > l_name ? name.Substring(0, l_name) : name;
            birth_year = birthYear;
            pay = salary;

            if (birth_year < 0) throw new FormatException("Неправильний рік народження");
            if (pay < 0) throw new FormatException("Неправильний оклад");
        }

        public override string ToString() // перевизначений метод
        {
            return string.Format("Name: {0,30} Birth: {1} Pay: {2:F2}", name, birth_year, pay);
        }

        public int Compare(string name) // порівняння прізвища
        {
            return string.Compare(this.name, 0, name + " ", 0, name.Length + 1, StringComparison.OrdinalIgnoreCase);
        }

        // Властивості класу
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Birth_year
        {
            get { return birth_year; }
            set
            {
                if (value > 0) birth_year = value;
                else throw new FormatException();
            }
        }

        public double Pay
        {
            get { return pay; }
            set
            {
                if (value >= 0) pay = value;
                else throw new FormatException();
            }
        }

        // Операції класу
        public static double operator +(Person pers, double a)
        {
            pers.pay += a;
            return pers.pay;
        }

        public static double operator -(Person pers, double a)
        {
            pers.pay -= a;
            if (pers.pay < 0) throw new FormatException();
            return pers.pay;
        }
    };

    class Program
    {
        static void Main(string[] args)
        {
            Person[] dbase = new Person[100];
            int n = 0;

            try
            {
                using (StreamReader f = new StreamReader("d:\\Persons.txt")) // Зчитування даних з файлу
                {
                    string s;
                    while ((s = f.ReadLine()) != null) // Читаємо рядки з файлу
                    {
                        string[] parts = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 3)
                        {
                            string name = parts[0];
                            int birthYear = int.Parse(parts[1]);
                            double salary = double.Parse(parts[2]);

                            dbase[n++] = new Person(name, birthYear, salary);
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Перевірте правильність імені і шляху до файлу!");
                return;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Неприпустима дата народження або оклад: " + e.Message);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("Помилка: " + e.Message);
                return;
            }

            double mean_pay = 0;
            int n_pers = 0;

            Console.WriteLine("Введіть прізвище співробітника");
            string nameInput;
            while ((nameInput = Console.ReadLine()) != "") // Цикл для введення прізвища
            {
                bool not_found = true;
                for (int k = 0; k < n; ++k)
                {
                    Person pers = dbase[k];
                    if (pers.Compare(nameInput) == 0)
                    {
                        Console.WriteLine(pers);
                        n_pers++;
                        mean_pay += pers.Pay;
                        not_found = false;
                    }
                }
                if (not_found) Console.WriteLine("Такого співробітника немає");
                Console.WriteLine("Введіть прізвище співробітника або Enter для завершення");
            }

            if (n_pers > 0)
                Console.WriteLine("Середній оклад: {0:F2}", mean_pay / n_pers);

            Console.ReadKey();
        }
    }
}
