using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RndArrayContinue
{
    class Program
    {
        static int[] arrayRnd;
        static void Main(string[] args)
        {
            Console.Write("Введите размер массива: ");
            int arraySize = Convert.ToInt32(Console.ReadLine());

            Task task1 = new Task(() => RndArray(arraySize));
            Action<Task> actionSum = new Action<Task>(SumArray);
            Task task2 = task1.ContinueWith(actionSum);
            Action<Task> actionMinMax = new Action<Task>(MinMaxArray);
            Task task3 = task2.ContinueWith(actionMinMax);

            task1.Start();
            //RndArray(arraySize);
            //SumArray(arrayRnd);
            //MinMaxArray(arrayRnd);

            Console.ReadKey();
        }

        static void RndArray(int arraySize) //Метод формирует массиив случайных чисел в диапазоне от -100 до 100
        {
            arrayRnd = new int[arraySize];
            Random rnd = new Random();
            Console.Write("Элементы массива: ");
            for (int i = 0; i < arraySize; i++)
            {
                arrayRnd[i] = rnd.Next(-100, 100);
                Console.Write($"{arrayRnd[i]} ");
            }
            Console.WriteLine();
        }

        static void SumArray(Task task) //Метод принимает массив и возвращает сумму элементов
        {

            int sum = 0;
            foreach (int k in arrayRnd)
                sum += k;
            Console.Write($"Сумма элементов: {sum}\n");
        }

        static void MinMaxArray(Task task)
        {
            Console.WriteLine($"Максимальный элемент массива: {arrayRnd.Max()}");
            Console.WriteLine($"Минимальный элемент массива: {arrayRnd.Min()}");
        }
    }
}
