using System;
using System.Threading;
using System.Diagnostics;

namespace pi
{
    class Program
    {

        static Object lockObj = new Object();
        static void Main(string[] args)
        {
            const int numThreads = 10;
            Thread[] mainThread = new Thread[numThreads];

            // Set up stopwatch
            Stopwatch stopWatch = new Stopwatch();

            long numberOfSamples = 100000;
            long hits = 0;
            /**************** Single-threaded Pi Estimate ****************/
            stopWatch.Start();
            double piSingleThread = EstimatePI(numberOfSamples, ref hits);
            Console.WriteLine("{0}", piSingleThread);
            stopWatch.Stop();
            TimeSpan tsSingle = stopWatch.Elapsed;
            stopWatch.Reset();

            // Reset value of hits back to 0 before multithreading sort
            hits = 0;

            /**************** Multi-threaded Pi Estimate ****************/
            // PC has 8 cores; meaning it is ideal to have 2 * 8 = 16 threads in total
            Console.WriteLine("Number of Cores: {0}", Environment.ProcessorCount);
            long piecelength = (numberOfSamples / (long) numThreads);
            stopWatch.Start();
            for (int i = 0; i < numThreads; i++)
            { 
                // Generate random, independent samples and keep track of hits 
                mainThread[i] = new Thread(() => EstimatePI(piecelength, ref hits));
                mainThread[i].Start();
            }

            for (int i = 0; i < numThreads; i++)
            {
                mainThread[i].Join();
            }
            // Manually calculate value of pi estimate using hits which have been passed by reference from threads
            double piMultiThread = 4 * ((double)hits / (double)numberOfSamples);
            Console.WriteLine("{0}", piMultiThread);
            stopWatch.Stop();
            TimeSpan tsMulti = stopWatch.Elapsed;
            stopWatch.Reset();

            // Print out times elapsed for singlethreaded and multithreaded estimates
            string singlethreadTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", tsSingle.Hours, tsSingle.Minutes,
            tsSingle.Seconds, tsSingle.Milliseconds / 10);
            string multithreadTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", tsMulti.Hours, tsMulti.Minutes,
            tsMulti.Seconds, tsMulti.Milliseconds / 10);
            Console.WriteLine("Singlethread Runtime " + singlethreadTime);
            Console.WriteLine("Multithread Runtime " + multithreadTime);

            /**************** Functions/Methods ****************/
            static double EstimatePI(long numberOfSamples, ref long hits)
            {
                double x;
                double y;
                double[,] dataArray = GenerateSamples(numberOfSamples);
                for (int i = 0; i < numberOfSamples; i++)
                {
                    x = dataArray[i, 0];
                    y = dataArray[i, 1];

                    // Check if (x,y) are within the unit circle; if yes, score a hit
                    if ((x * x + y * y) <= 1)
                       Interlocked.Increment(ref hits);
                }
                // Since probability(hit) = hits / shots = pir^2/4r^2 = pi/4 , 4 * probability(hit) = pi 
                // I casted both numerator and denominator to double to prevent the division from resulting in 0
                double piEstimate = 4 * ((double)hits / (double)numberOfSamples);
                return piEstimate;
            }


            static double[,] GenerateSamples(long numberOfSamples)
            {
                // Create 'numberOfSamples' rows of (x,y) coordinates
                double[,] dataArray = new double[numberOfSamples, 2];
                for (int i = 0; i < numberOfSamples; i++)
                {
                    for (int j = 0; j < 2; j++)
                        // Populate each x and y coordinate with a random value
                        dataArray[i,j] = RandomNumberGen();
                }
                return dataArray;
            }

            static double RandomNumberGen()
            {
                double lowerBound = -1;
                double upperBound = 1;
                Random RNG = new Random();
                lock (RNG) // Locking object protects the random seed used by each thread
                {
                    // To generate a random number in [-1,1], multiply by the difference between the bounds and 
                    // add the difference between 0 and the lower bound
                    double randomVal = (RNG.NextDouble() * (upperBound - lowerBound) + (0 + lowerBound));
                    return randomVal;
                }
            }
            

        }
    }
}
