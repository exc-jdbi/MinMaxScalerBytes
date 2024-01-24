
using System.Text;


namespace exc.jdbi.Algorithms.MinMaxScalers;

using static exc.jdbi.Systems.RandomHolder;
using static exc.jdbi.Algorithms.MinMaxScalers.MinMaxScalerByte;

public class Program
{
  public static void Main()
  {
    KopieSourceFiles();

    var round = 2;
    for (var i = 0; i < round; i++)
    {
      TestMinMaxScalerText("2.txt"); // 4.txt
      TestMinMaxScalerRandom();
    }

    Console.WriteLine("Finish");
    Console.ReadLine();
  }

  private static void TestMinMaxScalerText(string filepath)
  {
    var bytes = Encoding.UTF8.GetBytes(FileText(filepath));

    var limit = ToLimited(bytes);

    var (src, minmax) = MinMaxScaler(bytes, limit);

    var result = MinMaxDescaler(src, minmax);

    if (!bytes.SequenceEqual(result))
      throw new Exception();
  }

  private static void TestMinMaxScalerRandom()
  {
    //var size = Rand.Next(5, 15);
    var size = Rand.Next(10, 1024);

    //var bytes = RngBytes(size);
    var bytes = RngBytes(size, 10, 190);

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
}