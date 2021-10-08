using System;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace MergeSort
{
    class Program
    {
        private const int numThreads = 5;
        static void Main(string[] args)
        {
            // Queue of 1D arrays to help merge array slices for the multithreaded mergesort variant
            Queue<int[]> arrayQueue = new Queue<int[]>();

            int ARRAY_SIZE = 1000;
            int[] arraySingleThread = new int[ARRAY_SIZE];

            for (int i = 0; i < ARRAY_SIZE; i++)
            {
                // Instantiate a new "Random" Class
                Random RNG = new Random();
                // Generate a random integer between 0-20 for the current array index
                arraySingleThread[i] = RNG.Next(0, 20);
            }

            // Generate a Multithread array with the same length as the Singlethread array
            int[] arrayMultiThread = new int[ARRAY_SIZE];
            // Invoke Array.Copy to copy exact contents from Singlethread to Multithread array
            Array.Copy(arraySingleThread, arrayMultiThread, arraySingleThread.Length);

            /*TODO : Use the  "Stopwatch" class to measure the duration of time that
               it takes to sort an array using one-thread merge sort and
               multi-thead merge sort
            */

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrintArray(arraySingleThread);
            // Invoke single-threaded MergeSort
            MergeSort(arraySingleThread);
            PrintArray(arraySingleThread);
            stopWatch.Stop();
            TimeSpan tsSingle = stopWatch.Elapsed;
            stopWatch.Reset();


            //TODO: Multi Threading Merge Sort
            stopWatch.Start();
            PrintArray(arrayMultiThread);

            // Generate a variable amount of threads depending on const int numThreads
            Thread[] mainThread = new Thread[numThreads];
            int start = 0;
            int piece_length = arrayMultiThread.Length / numThreads;

            int end = piece_length;

            // Generate a 2D array of proper size
            int[,] mainArray = new int[numThreads, piece_length];
            // Asign each thread an array
            for (int i = 0; i < numThreads; i++)
            {
                int j = 0;
                while (start < end)
                {
                    mainArray[i, j] = arrayMultiThread[start];
                    start++;
                    j++;
                }
                end += piece_length;
            }

            start = 0;
            for (int i = 0; i < numThreads; i++)
            {
                int[] sliced_array = new int[piece_length];
                for (int j = 0; j < piece_length; j++)
                {
                    // Assign each array slice a row in the 2D array
                    sliced_array[j] = mainArray[i, j];
                }
                // Invoke the MultiMergeSort function on each piece
                mainThread[i] = new Thread(() => MultiMergeSort(arrayMultiThread, sliced_array, start, piece_length));
                Console.WriteLine("Thread has been run");
                mainThread[i].Start();
                mainThread[i].Join();
                // Increment 'start' by the 'piece length' to traverse through "arrayMultiThread"
                start += piece_length;
            }

            // Now that the array has been sorted in pieces, I will use a queue to merge all the sorted piece

            int[] temp1;
            int[] temp2;

            // Using the Queue of array slices, I will peek the first pair of slices, merge them, then dequeue them. The array temp3[] will store the array result of the Merge function
            // and then add it to the back of the queue to be merged again later

            // When there is only 1 item left in the queue, that will be the fully sorted array; until then, I will keep merging array slices and dequeue/enqueuing
            while (arrayQueue.Count != 1)
            {
                temp1 = (arrayQueue.Peek() as int[]);
                arrayQueue.Dequeue();
                temp2 = (arrayQueue.Peek() as int[]);
                arrayQueue.Dequeue();
                // Merge result 'temp3' will have the size of both array slices combined
                int[] temp3 = new int[temp1.Length + temp2.Length];
                Merge(temp1, temp2, temp3);
                arrayQueue.Enqueue(temp3);
            }
            // The final sorted array will be the last item remaining in the queue
            arrayMultiThread = arrayQueue.Peek();

            PrintArray(arrayMultiThread);
            stopWatch.Stop();
            TimeSpan tsMulti = stopWatch.Elapsed;
            stopWatch.Reset();

            // Print out times elapsed for each merge algorithm
            string singlethreadTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", tsSingle.Hours, tsSingle.Minutes,
                tsSingle.Seconds, tsSingle.Milliseconds / 10);
            string multithreadTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", tsMulti.Hours, tsMulti.Minutes,
            tsMulti.Seconds, tsMulti.Milliseconds / 10);
            if(IsSorted(arraySingleThread))
                Console.WriteLine("Array has been sorted using a single thread!");
            if(IsSorted(arrayMultiThread))
                Console.WriteLine("Array has been sorted using multithreading!");
            Console.WriteLine("Singlethread Runtime " + singlethreadTime);
            Console.WriteLine("Multithread Runtime " + multithreadTime);


            /*********************** Methods **********************
             *****************************************************/
            /*
            implement Merge method. This method takes two sorted array and
            and constructs a sorted array in the size of combined arrays
            */

            /* Sorting algorithm/process occur in my merge function*/
            static void Merge(int[] LA, int[] RA, int[] A)
            {
                int leftLength = LA.Length;
                int rightLength = RA.Length;
                int i = 0; // Indexes for LA, RA and A
                int j = 0;
                int k = 0;

                while (i < leftLength && j < rightLength)
                {
                    // Compare values from left and right arrays to populate A
                    if (LA[i] <= RA[j])
                    {
                        A[k] = LA[i];
                        i++;
                    }
                    else
                    {
                        A[k] = RA[j];
                        j++;
                    }
                    k++;
                }
                // Fill in left-over entries of sorted array 
                while (i < leftLength)
                {
                    A[k] = LA[i];
                    i++;
                    k++;
                }
                while (j < rightLength)
                {
                    A[k] = RA[j];
                    j++;
                    k++;
                }
            }

            /*
            implement MergeSort method: takes an integer array by reference
            and makes some recursive calls to itself and then sorts the array
            */
            static void MergeSort(int[] A)
            {
                int midpoint = A.Length / 2;
                int[] leftArray = new int[midpoint];
                int[] rightArray;

                // New array to hold merged left and right halves
                int[] result = new int[A.Length];

                // Base case to prevent infinite recursion and errors when array is length 1 or less
                if (A.Length < 2)
                {
                    return;
                }

                // Check to see if length is odd; if it is, we will need to add 1 to the right array's size
                if (A.Length % 2 == 0)
                    rightArray = new int[midpoint];
                else

                    rightArray = new int[midpoint + 1];

                // Populate left array 
                for (int i = 0; i < midpoint; i++)
                    leftArray[i] = A[i];

                int k = 0;
                // Populate right array using 'k' counter to increment from index 0 
                for (int i = midpoint; i < A.Length; i++)
                {
                    rightArray[k] = A[i];
                    k++;
                }
                // Use mergeSort to recursively sort both halves of array and merge each time recursion completes
                MergeSort(leftArray);
                MergeSort(rightArray);
                Merge(leftArray, rightArray, A);
            }


            // Multithreaded MergeSort function that sorts each slice and adds them to the front of the queue for merging (which occurs in main)
            void MultiMergeSort(int[] array, int[] sliced_array, int increment, int piecelength)
            {
                MergeSort(sliced_array);
                Array.Copy(sliced_array, 0, array, increment, piecelength);
                arrayQueue.Enqueue(sliced_array);
            }

            // a helper function to print your array
            static void PrintArray(int[] myArray)
            {
                Console.Write("[");
                for (int i = 0; i < myArray.Length; i++)
                {
                    Console.Write("{0} ", myArray[i]);

                }
                Console.Write("]");
                Console.WriteLine();

            }

            // a helper function to confirm your array is sorted
            // returns boolean True if the array is sorted
            static bool IsSorted(int[] a)
            {
                int j = a.Length - 1;
                if (j < 1) return true;
                int ai = a[0], i = 1;
                while (i <= j && ai <= (ai = a[i])) i++;
                return i > j;
            }

        }


    }
}


