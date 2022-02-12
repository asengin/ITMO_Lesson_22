using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RndArrayContinue
{
    class Program
    {
        /*
         * Реализованы задачи продолжения с передачей результатов от задачи к задаче.
         * В задаче taskRndArray осуществляется формирование массива случайных чисел заданной пользователем размерности
         * Далее продолжают работать задачи по нахождению суммы, максимального и минимального элемента массива.
         * Для мин и макс применил готовый метод Min Max для массивов и лямбда выражения.
         * Дожидаемся работы всех задач через Task.WaitAll.
         * Выводим результат через отдельную задачу.
         */
         static void Main(string[] args)
        {
            Console.Write("Введите размер массива: ");
            int arraySize = Convert.ToInt32(Console.ReadLine());

            #region Вариант с передачей параметров от задачи к задаче. Доработаны методы
            Func<object, int[]> funcRndArray = new Func<object, int[]>(RndArray);
            Task<int[]> taskRndArray = new Task<int[]>(funcRndArray, arraySize);

            Func<Task<int[]>, int> funcSumArray = new Func<Task<int[]>, int>(SumArray);
            Task<int> taskSumArray = taskRndArray.ContinueWith<int>(funcSumArray);

            Task<int> taskMaxInArray = taskRndArray.ContinueWith<int>((Task<int[]> task) => task.Result.Max());
            Task<int> taskMinInArray = taskRndArray.ContinueWith<int>((Task<int[]> task) => task.Result.Min());

            Task taskPrint = new Task(() => PrintArrayInfo(taskRndArray, taskSumArray, taskMaxInArray, taskMinInArray));

            taskRndArray.Start();
            Task.WaitAll(taskSumArray, taskMaxInArray, taskMinInArray);

            taskPrint.Start();
            #endregion
            Console.ReadKey();
        }

        static int[] RndArray(object size) //Метод формирует массиив случайных чисел в диапазоне от -100 до 100
        {
            int arraySize = (int)size;
            int[] arrayRnd = new int[arraySize];
            Random rnd = new Random();
            //Console.Write("Элементы массива: ");
            for (int i = 0; i < arraySize; i++)
            {
                arrayRnd[i] = rnd.Next(-100, 100);
                //Console.Write($"{arrayRnd[i]} ");
            }
            //Console.WriteLine();
            return arrayRnd;
        }

        static int SumArray(Task<int[]> task) //Метод принимает таск-массив, вычисляет и возвращает сумму элементов
        {
            int[] arrayRnd = task.Result;
            int sum = 0;
            foreach (int k in arrayRnd)
                sum += k;
            return sum;
        }
        static void PrintArrayInfo(Task<int[]> taskArray, Task<int> taskSum, Task<int> taskMax, Task<int> taskMin) //Метод принимает выполненные таски и выводит результаты
        {
            int[] array = taskArray.Result;
            int sum = taskSum.Result;
            int max = taskMax.Result;
            int min = taskMin.Result;

            Console.Write("Элементы массива: ");
            for (int i = 0; i < array.Count(); i++)
                Console.Write($"{array[i]} ");

            Console.WriteLine($"\nСумма элементов массива: {sum}\n" +
                $"Максимальный элемент массива: {max}\n" +
                $"Минимальный элемент массива: {min}");
        }
             
    }
}
