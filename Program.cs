
using System.Text;
using System.Collections; 

namespace exc.jdbi.Algorithms.MinMaxScalers;

using static exc.jdbi.Systems.RandomHolder;
using static exc.jdbi.Algorithms.MinMaxScalers.MinMaxScalerByte;


//What is noticeable is, that data compression is taking place.

//This works much better with shorter English texts than with
//random bytes.

//With data compression there is always the problem that
//compression is not possible with short texts. 

//This could be the solution to this problem.


public class Program
{
  public static void Main()
  {
    KopieSourceFiles();

    var round = 10;
    for (var i = 0; i < round; i++)
    {
      TestMinMaxScalerText("0b.txt"); // 4.txt
      TestMinMaxScalerRandom();
    }

    Console.WriteLine("Finish");
    Console.ReadLine();
  }


  private static void TestMinMaxScalerText(string filepath)
  {
    //var txt = FileText(filepath);
    var bytes = Encoding.UTF8.GetBytes(FileText(filepath));

    var balen1 = new BitArray(bytes).Length;
    var sblen1 = ShortBitLength(bytes);

    var limit = ToLimited(bytes);
    var (src, minmax) = MinMaxScaler(bytes, limit);

    var balen2 = new BitArray(src).Length;
    var sblen2 = ShortBitLength(src);
    var bsblen = sblen1 > sblen2;
    
    var result = MinMaxDescaler(src, minmax);

    if (!bytes.SequenceEqual(result))
      throw new Exception();

  }

  private static void TestMinMaxScalerRandom()
  {
    //var size = Rand.Next(5, 15);
    var size = Rand.Next(10, 1024);

    var bytes = RngBytes(size);
    //var bytes = RngBytes(size, 10, 190);

    var limit = ToLimited(bytes);

    if (MinMaxScaler(bytes, limit, out var src_minmax))
      if (!bytes.SequenceEqual(MinMaxDescaler(src_minmax)))
        throw new Exception();
 
  }

  private static void KopieSourceFiles()
  {
    var filefolder = @"..\..\..\..\TextSource\";
    for (var i = 0; i < 5; i++)
    {
      var fp = filefolder + i.ToString() + ".txt";
      if (File.Exists(fp))
        File.Copy(fp, i.ToString() + ".txt", true);
    }
  }

  private static string FileText(string filepath) =>
   File.ReadAllText(filepath);

  private static int ShortBitLength(ReadOnlySpan<byte> input)
  {
    var result = 0;
    foreach (var number in input)
      result += Convert.ToString(number, 2).Length;
    return result;
  }
}

 