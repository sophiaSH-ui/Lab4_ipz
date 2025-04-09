using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

// Програмно промоделювати машинний алгоритм додавання чисел з плаваючою крапкою. Перший операнд представити у форматі 16 розрядів, другий – у форматі 8 розрядів.

namespace task4
{
    internal class MyWork
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            bool check1, check2;
            string line1, line2;
            string power1, power2;

            do
            {
                Console.WriteLine("""
                 -- Введіть перше число --
                 у вигляді прямого коду (довжина розрядної сітки 16):
                 Приклад: 0|0001001010010010 
                 """);

                line1 = Console.ReadLine();
                check1 = CheckBin(line1);

                if (!check1)
                {
                    Console.WriteLine("Здається ви ввели щось не те. ");
                }
                Console.WriteLine();
                if (line1.Length != 18)
                {
                    Console.WriteLine("Введена довжина є меншою або більшою ніж треба, спробуйте ще раз.");
                } 

            } while (line1.Length != 18 || !check1);

            do
            {
                Console.WriteLine("Введіть бінарний степінь 2 до першого числа (3 значення): ");
                power1 = Console.ReadLine();
                check1 = CheckBin(power1);

                if (power1.Length != 3 || !check1)
                    Console.WriteLine("Шось не то...");
                Console.WriteLine();

            } while (power1.Length != 3 || !check1);

            do
            {
                Console.WriteLine("""
                 -- Введіть друге число --
                 у вигляді прямого коду (довжина розрядної сітки 8):
                 Приклад: 1|01011010 
                 """);

                line2 = Console.ReadLine();
                check2 = CheckBin(line2);

                if (!check2)
                {
                    Console.WriteLine("Задається ви ввели щось не те. ");
                }
                if (line2.Length != 10)
                {
                    Console.WriteLine("Введена довжина є меншою або більшою ніж треба, спробуйте ще раз.");
                }
                
                Console.WriteLine();

            } while (line2.Length != 10 || !check2);

            do
            {
                Console.WriteLine("Введіть бінарний степінь 2 до другого числа (3 значення): ");
                power2 = Console.ReadLine();
                check2 = CheckBin(power2);

                if (power2.Length != 3 || !check2)
                    Console.WriteLine("Шось не то...");
                Console.WriteLine();

            } while (power2.Length != 3 || !check2);

            string[] number1 = line1.Split('|');
            string[] number2 = line2.Split('|');
            int sign1 = int.Parse(number1[0]);
            int sign2 = int.Parse(number2[0]);
            string mantis1 = number1[1];
            string mantis2 = number2[1] + "00000000";

            Console.WriteLine($"""
                -- Ваші введені значення --
                Перше: {sign1}|{mantis1} * 2 ^ ({power1})
                Друге: {sign2}|{mantis2} * 2 ^ ({power2})

                Натисніть Enter, щоб продовжити
                """);
            Console.ReadKey();
            Console.Clear();

            // пошук більшого степеня та його різниці
            (string b_power, int difference) = GreaterDegreAndDifference(power1, power2);

            string num1_d, num2_d;
            num1_d = CheckSign(sign1, mantis1,"Перше");
            Console.WriteLine();
            num2_d = CheckSign(sign2, mantis2, "Друге");

            Console.WriteLine();
            if(b_power == power1)
            {
                num2_d = Shift(num2_d, difference, sign2);
                Console.WriteLine($"""
                    У першого числа степінь виявився більшим, тож ми зсунули друге 
                    значення на {difference} позицію\-ї\-й вправо:
                    {num2_d}
                    """);
            }
            if(b_power == power2)
            {
                num1_d = Shift(num1_d, difference, sign1);
                Console.WriteLine($"""
                    У другого числа степінь виявився більшим, тож ми зсунули перше 
                    значення на {difference} позицію\-ї\-й вправо:
                    {num1_d}
                    """);
            }

            string add_num = Add(num1_d, num2_d);
            Console.WriteLine();
            Console.WriteLine($"Сума мантис після додавання (до перетворення в прямий код): \n{add_num}");

            int final_sign;
            string final_mantissa;

            if (add_num.Length > 0 && add_num[0] == '1')
            {
                final_sign = 1;
                final_mantissa = ArrPlusOne(Inversion(add_num));
            }
            else
            {
                final_sign = 0;
                final_mantissa = add_num;
            }

            Console.WriteLine();
            Console.WriteLine($"""
                -- Отже результатом обчислень є --
                {final_sign}|{final_mantissa} * 2 ^ ({b_power})
                """);

        }

        private static bool CheckBin(string num)
        {
            foreach (var c in num)
            {
                if(c != '0' && c != '1' && c!='|')
                    return false;
            }
            return true;
        }

        private static (string, int) GreaterDegreAndDifference(string n, string m)
        {
            int num1 = Convert.ToInt32(n, 2);
            int num2 = Convert.ToInt32(m, 2);

            //Console.WriteLine(num1);
            //Console.WriteLine(num2);

            int diff = Math.Abs(num1 - num2);

            return ((num1 > num2) ? n : m, diff);
        }

        private static string CheckSign(int sign, string numb, string name)
        {
            if (sign == 0)
            {
                Console.WriteLine($"{name} число є додатним, отже воно не змінюється у додаткому коді.");
                Console.WriteLine($"{sign}|{numb}");
                return numb;
            }
            else if (sign == 1)
            {

                string allBin = ArrPlusOne(Inversion(numb));

                Console.WriteLine($"{name} число від'ємне, тож перетворюємо:");
                Console.Write($"{sign}|");
                for (int i = 0; i < allBin.Length; i++)
                {
                    Console.Write(allBin[i]);
                }
                Console.WriteLine();

                return allBin;

            }
            else
            {
                return "Error";
            }
        }

        private static long[] Inversion(string num)
        {
            long numBinary = long.Parse(num);

            long[] num_b = new long[num.Length];
            for (int i = num_b.Length - 1; i >= 0; i--)
            {
                num_b[i] = numBinary % 10;
                numBinary /= 10;
            }

            for (int i = 0; i < num_b.Length; i++)
            {
                if (num_b[i] == 0)
                {
                    num_b[i] = 1;
                }
                else if (num_b[i] == 1)
                {
                    num_b[i] = 0;
                }
            }

            return num_b;

        }

        private static string ArrPlusOne(long[] arrAll)
        {
            int shift = 1;
            long sum;

            for (int i = arrAll.Length - 1; i >= 0; i--)
            {
                sum = arrAll[i] + shift;

                if (sum == 1 || sum == 0)
                {
                    arrAll[i] = sum;
                    shift = 0;
                }
                if (sum == 2)
                {
                    arrAll[i] = 0;
                    shift = 1;
                }

            }

            string newNumb = string.Join("", arrAll);
            return newNumb;

        }

        private static string Shift(string num,int diff, int sign)
        {
            int n = num.Length;
            if (sign == 0)
            {
                for(int i = 1; i <= diff; i++ )
                {
                    num = 0.ToString() + num;
                }
                return num.Substring(0, n);
            }
            if (sign == 1)
            {
                for (int i = 1; i <= diff; i++)
                {
                    num = 1.ToString() + num;
                }
                return num.Substring(0, n);
            }
            else
            {
                return "Error";
            }
        }

        private static string Add(string a, string b)
        {
            string result = "";
            int carry = 0;

            for (int i = 15; i >= 0; i--)
            {
                int bitA = a[i] - '0';
                int bitB = b[i] - '0';
                int sum = bitA + bitB + carry;

                result = (sum % 2) + result;
                carry = sum / 2;
            }

            if (carry == 1)
            {
                result = "1" + result;
            }

            return result;
        }

    }

}
