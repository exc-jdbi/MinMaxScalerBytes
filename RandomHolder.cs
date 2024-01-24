 

namespace exc.jdbi.Systems;


public class RandomHolder
{
  public static readonly Random Rand = new();

  public static byte[] RngBytes(int size)
  {
    var result = new byte[size];
    Rand.NextBytes(result);
    return result;
  }

  public static byte[] RngBytes(int size, int min, int max)
  {
    var result = new byte[size];
    for (int i = 0; i < size; i++)
      result[i] = (byte)Rand.Next(min, max);
    return result;
  }
}
