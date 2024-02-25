
using System.Text;
using System.Collections;


//Here you will find that the attempt to abbreviate MinMax
//produces incorrect results.

//Surprisingly, it works correctly more often with EnglishText
//than with RandomBytes.


namespace MinMaxScalerBytes.AnotherAttempt;


using static exc.jdbi.Systems.RandomHolder;
using static exc.jdbi.Algorithms.MinMaxScalers.MinMaxScalerByte;


public class Program2
{
    public static void MainStart()
    //public static void Main()
    {

        //var bla = Encoding.UTF8.GetBytes("üÜ");
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

        //var mmin = minmax.Select((x, i) => (i & 1) == 0 ? x : 0).Sum(x => x);

        var result = MinMaxDescaler(src, minmax);

        if (!bytes.SequenceEqual(result))
            throw new Exception();


        ////The attempt to take a shortcut.
        ////Unfortunately, this does not work. What is noticeable,
        ////however, is that it works better with EnglishText than
        ////with RandomBytes.

        ////Der Versuch über eine Abkürzung.
        ////Leider funktioniert das nicht. Was aber auffällt, ist
        ////das es bei EnglishText eher funktioniert als bei RandomBytes.
        //var result1 = MinMaxDescaler(src, [ (byte)mmin, minmax.Last() ]);

        //if (!result.SequenceEqual(result1))
        //  throw new Exception();
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


        ////The attempt to take a shortcut.
        ////Unfortunately, this does not work. What is noticeable,
        ////however, is that it works better with EnglishText than
        ////with RandomBytes.

        ////Der Versuch über eine Abkürzung.
        ////Leider funktioniert das nicht. Was aber auffällt, ist
        ////das es bei EnglishText eher funktioniert als bei RandomBytes.
        //if (MinMaxScaler(bytes, limit, out var src_minmax_))
        //{
        //var (src, minmax) = src_minmax_;
        //var mmin = (byte)minmax.Select((x, i) => (i & 1) == 0 ? x : 0).Sum(x => x);
        //if (!bytes.SequenceEqual(MinMaxDescaler((src,[mmin,minmax.Last()]))))
        //  throw new Exception();
        //}
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

