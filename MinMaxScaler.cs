
//MinMaxScalerByte scales numeric integer sequences down to their
//lower limit, and allows reversal to be accomplished again without
//any problems.

//There are certainly many applications for MinMaxScalerByte, and
//the one that comes to mind spontaneously could possibly be data
//compression. It would have to be adapted specifically for this.

//MinMaxScalerByte is freely changeable as well as available. 
//A small hint to my repository is enough.
//https://github.com/exc-jdbi/MinMaxScalerBytes

//Thanks and greetings exc-jdbi.


//MinMaxScalerByte skaliert numerische Ganzzahlenfolgen runter an
//ihren unteren Grenzwert, und lässt eine Umkehrung problemlos
//wieder bewerkstelligen.

//Für MinMaxScalerByte gibt es sicher viele Einsatzorte, und die die mir 
//spontan einfällt könnte eventuell die Datenkompression sein. Müsste 
//spezifisch dafür angepasst werden.

//MinMaxScalerByte ist frei veränderbar wie auch verfügbar. 
//Ein kleiner Hinweis zu meiner Repository reicht schon.
//https://github.com/exc-jdbi/MinMaxScalerBytes

//Danke und gruss exc-jdbi.


namespace exc.jdbi.Algorithms.MinMaxScalers;


public class MinMaxScalerByte
{
  public static byte ToLimited(
    ReadOnlySpan<byte> input, 
    int abort_after_round = 20)
  {
    var iter = 0;
    var maxval = int.MaxValue;
    var length = input.Length;
    var bytes = input.ToArray();
    while (true)
    {
      var min = (int)bytes.Min();
      var max = (int)bytes.Max();
      for (var i = 0; i < length; i++)
        bytes[i] = (byte)ModuloSpec(bytes[i] + min, max + 1);
      iter++;
      if (max < maxval) { maxval = max; iter = 0; }
      if (iter > abort_after_round) return (byte)maxval;
    }
  }


  public static bool MinMaxScaler(
    ReadOnlySpan<byte> input, byte limit,
    out (byte[] Source, byte[] MinMax) result,
    int abort_after_round = 20)
  {
    result = default;
    var iter = 0;
    var length = input.Length;
    var mm = new List<byte>();
    var minval = int.MaxValue;
    var bytes = input.ToArray();

    while (true)
    {
      var min = (int)bytes.Min();
      var max = (int)bytes.Max();
      if (max <= limit) break;
      for (var i = 0; i < length; i++)
        bytes[i] = (byte)ModuloSpec(bytes[i] + min, max + 1);
      iter++;
      if (max < minval) { minval = max; iter = 0; }
      if (iter > abort_after_round) return false;
      mm.AddRange([(byte)min, (byte)max]);
    }
    result = (bytes, mm.ToArray());
    return true;
  }

  public static (byte[] Source, byte[] MinMax) MinMaxScaler(
    ReadOnlySpan<byte> input,
    byte limit,
    int abort_after_round = 20)
  {
    var iter = 0;
    var length = input.Length;
    var mm = new List<byte>();
    var minval = int.MaxValue;
    var bytes = input.ToArray();

    while (true)
    {
      var min = (int)bytes.Min();
      var max = (int)bytes.Max();
      if (max <= limit) break;
      for (var i = 0; i < length; i++)
        bytes[i] = (byte)ModuloSpec(bytes[i] + min, max + 1);
      iter++;
      if (max < minval) { minval = max; iter = 0; }
      if (iter > abort_after_round) return default;
      mm.AddRange([(byte)min, (byte)max]);
    }
    return (bytes, mm.ToArray());
  }

  public static byte[] MinMaxDescaler((byte[] src, byte[] minmax) input) =>
    MinMaxDescaler(input.src, input.minmax);

  public static byte[] MinMaxDescaler(
    ReadOnlySpan<byte> input, ReadOnlySpan<byte> minmax)
  {
    var length = input.Length;
    var bytes = input.ToArray();
    for (var i = 1; i < minmax.Length; i += 2)
    {
      var max = (int)minmax[^i];
      var min = (int)minmax[^(i + 1)];
      for (var j = 0; j < length; j++)
        bytes[j] = (byte)ModuloSpec(bytes[j] - min, max + 1);
    }
    return bytes;
  }

  private static int ModuloSpec(int basic, int mod)
  {
    var result = basic % mod;
    result += mod;
    result %= mod;
    return result;
  }
}