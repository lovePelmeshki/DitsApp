using System;
using System.Linq;

namespace LINQ123
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] scores = { 1, 21, 54, 667, 76, 89, 98, 67, 123 };

            var highScoresQuery =
                (from score in scores
                 where score > 50
                 orderby score descending
                 select score)
                .Count();
           
                Console.WriteLine(highScoresQuery);
            
        }
    }
}
