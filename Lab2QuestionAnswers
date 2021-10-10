/*
/************************************************** Part 1: SingleThreaded and MultiThreaded MergeSort ********************************************************************/

For small arrays, both versions have similar runtimes, with the single threaded mergesort being more efficient. For very large arrays,
the MultiThreaded seems to be faster and more efficient.

  Q1) Duration of singlethread vs multithread sorting for different sized arrays (speed up factors) with 5 threads:

        Array Size:      SingleThread Duration:        MultiThread Duration:
        10                   00:00.03                        00:00.05
        100                  00:00.05                        00:00.07
        1000                 00:00.27                        00:00.57
        10,000               00:04.53                        00:04.65         
        100,000              00:06.56                        00:05.94       //MT becomes more efficient than ST here   
        1,000,000            00:47.73                        00:35.72
        10,000,000           06:06.53                        05:58.29
 
 
   Q2/Q3) Using Environment.ProcessorCount(); I discovered that my computer had 8 processor/cores. Comparing this to charts I found online,
   it is expected that I have a speedup factor of ~8 (number of processors). However, judging from the data I gathered above, the speedup factor I obtained was very low
   (slightly over 1). I believe that this may have been a result of the additional overhead that is required to create the threads, as well as the 
   fact that my computer's cores are used to run other tasks and programs concurrently with my parallel program. 
   
   Q4) After testing a different number of threads for a sized 10^6 array, I obtained the following data:
   
         # of Threads         Duration:
         2                    00:32.97
         5                    00:33.10
         8                    00:33.24
         12                   00:33.82
         20                   00:34.06
         
   From the data, it's clear that increasing the number of threads results in a increased duration for the program to complete. Once again, this is likely due to the 
   reasons mentioned above (overhead and such) which may actually end up slowing the system down
  
  
  /**************************************************  Part 2: SingleThreaded and Multithreaded Pi Estimation ************************************************************/
   
   With a 1000 sized array and a singlethread, my pi estimation is quite close, with estimations including 3.11, 3.14, 3.164 and 3.20 (notable variation).
   To obtain an estimation consistently close to 3 decimal points, I needed approximately a 10^6 - 10^8 sized sample. Similar to Part 1, my multithreadded pi estimate
   is faster than my singlethreaded at larger sized arrays (MT outperforms ST at a sample size of 10^5 and above). 

   Q1/Q2) I've learned that splitting up work between threads requires some degree of care as sharing resources between them may cause complications. 
   To prevent issues with parallel computing, critical sections and certain objects (such as Random) need to be locked using mutual exception to be threadsafe.
   These are all details that I will need to keep in mind when designing concurrent code. 
   
   Q3) Since I needed ~10^7 samples to get an accurate estimation of pi to 3 decimal places, I will likely need an extremely large sample size to get an accuracy of 7 
   (something like 10^15 samples maybe) that will be difficult to run with my code. Given how many decimals pi has, it will be very difficult for the random nature of Monte Carlo 
   simulations to provide a very accurate estimation.
 */
