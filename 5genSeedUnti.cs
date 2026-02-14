using System;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Windows.Forms;
using RepoHariTool;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static RepoHariTool.Main;

public class SeedSearcher
{
    public struct InSeedData
    {
        public ulong Seed;
        public byte Year;
        public byte Month;
        public byte Day;
        public byte Hour;
        public byte Minute;
        public byte Second;
        public uint VCount;
        public uint InTimer0;
    }

    private PIDPrm param;
    private List<int> Report;

    public SeedSearcher(PIDPrm param, List<int> Report)
    {
        this.param = param;
        this.Report = Report;
    }

    public void GenerateSeeds(CancellationToken token, DataGridView DataGridView1)
    {
        DataGridView1.Invoke(new Action(() =>
        {
            DataGridView1.Rows.Clear();
            DataGridView1.SuspendLayout();
        }));

        for (DateTime Dt = param.InDt; Dt <= param.EnDt; Dt += param.Increment)
        {

            for (uint Timer0 = param.Values[4]; Timer0 <= param.Values[5]; Timer0++)
            {
                InSeedData SeedData = GenSeed(param, Dt, Timer0);
                ulong Seed = SeedData.Seed;
                ulong CurrentSeed = 0x0;
                Console.WriteLine("----------------------");
                Console.WriteLine($"Seed:{SeedData.Seed:X16}");

                //param.values[14]でPT消費、InSeedの決定
                //とりあえず思い出リンクなし（Rand*3）でつくる
                //SHA1→Rand×1→PT×5
                //SHA1→Rand×1→PT×1→Rand×3→PT×4→Extra

                Seed = NextSeed(Seed);
                Seed = ProbabilityTable(Seed);

                if (param.Values[14] == 2 || param.Values[14] == 3)
                {
                    if (param.Values[16] == 0) Seed = NextSeed(Seed);
                    Seed = NextSeed(Seed);
                    Seed = NextSeed(Seed);
                }

                Seed = ProbabilityTable(Seed);
                Seed = ProbabilityTable(Seed);
                Seed = ProbabilityTable(Seed);
                Seed = ProbabilityTable(Seed);

                if (param.Values[14] == 2 || param.Values[14] == 3)
                {
                    int ExVal1, ExVal2, ExVal3;

                    do
                    {
                        Seed = NextSeed(Seed);
                        ExVal1 = (int)(((Seed >> 32) * 15) >> 32);

                        Seed = NextSeed(Seed);
                        ExVal2 = (int)(((Seed >> 32) * 15) >> 32);

                        Seed = NextSeed(Seed);
                        ExVal3 = (int)(((Seed >> 32) * 15) >> 32);
                    } while (ExVal1 == ExVal2 || ExVal1 == ExVal3 || ExVal2 == ExVal3);
                }

                ulong InSeed = Seed;

                for (int n = 0; n < param.Values[15]; n++)  //消費数
                {
                    Seed = InSeed;
                    bool match = true;
                    Console.WriteLine($"Count:{n}");

                    for (int m = 0; m < Report.Count; m++)
                    {
                        Console.WriteLine($"ReportCount:{m}");
                        Console.WriteLine($"Seed:{Seed:X16}");
                        Console.WriteLine($"Val:{(((Seed >> 32) * 7) >> 32):X16}");
                        if (Report[m] == 8)
                        {
                            CurrentSeed = Seed;
                            Seed = NextSeed(Seed);
                            continue;
                        }
                        else
                        {
                            int val = (int)(((Seed >> 32) * 8) >> 32);
                            if (val == Report[m])
                            {
                                CurrentSeed = Seed;
                                Seed = NextSeed(Seed);
                                continue;
                            }
                            else
                            {
                                match = false;
                                break;
                            }
                        }
                    }

                    

                    if (match == true)
                    {
                        DataGridView1.Invoke(new Action(() =>
                        {
                            int rowIndex = DataGridView1.Rows.Add();
                            var row = DataGridView1.Rows[rowIndex];

                            row.Cells[0].Value = SeedData.Seed.ToString("X16");
                            row.Cells[1].Value = SeedData.Minute;
                            row.Cells[2].Value = SeedData.Second;
                            row.Cells[3].Value = SeedData.VCount.ToString("X2");
                            row.Cells[4].Value = SeedData.InTimer0.ToString("X4");
                            row.Cells[5].Value = n;
                            row.Cells[6].Value = CurrentSeed.ToString("X16");
                        }));
                    }

                    InSeed = NextSeed(InSeed);
                }
            }
        }
        return;
    }
    private ulong NextSeed(ulong Seed)
    {
        return Seed * 0x5D588B656C078965UL + 0x269EC3UL;
    }

    private ulong ProbabilityTable(ulong Seed)
    {
        //--------------------------------------------
        Seed = NextSeed(Seed);
        //--------------------------------------------
        Seed = NextSeed(Seed);
        int PTval = (int)(((Seed >> 32) * 101) >> 32);

        if (PTval > 50)
        {
            Console.WriteLine($"PTval1:{PTval}");
            Seed = NextSeed(Seed);
        }
        //--------------------------------------------
        Seed = NextSeed(Seed);
        PTval = (int)(((Seed >> 32) * 101) >> 32);

        if (PTval > 30)
        {
            Console.WriteLine($"PTval2:{PTval}");
            Seed = NextSeed(Seed);
        }
        //--------------------------------------------
        Seed = NextSeed(Seed);
        PTval = (int)(((Seed >> 32) * 101) >> 32);

        if (PTval > 25)
        {
            Console.WriteLine($"PTval3:{PTval}");
            Seed = NextSeed(Seed);
            PTval = (int)(((Seed >> 32) * 101) >> 32);

            if (PTval > 30)
            {
                Console.WriteLine($"PTval4:{PTval}");
                Seed = NextSeed(Seed);
            }
        }
        //--------------------------------------------
        Seed = NextSeed(Seed);
        PTval = (int)(((Seed >> 32) * 101) >> 32);

        if (PTval > 20)
        {
            Console.WriteLine($"PTval5:{PTval}");
            Seed = NextSeed(Seed);
            PTval = (int)(((Seed >> 32) * 101) >> 32);

            if (PTval > 25)
            {
                Console.WriteLine($"PTval6:{PTval}");
                Seed = NextSeed(Seed);
                PTval = (int)(((Seed >> 32) * 101) >> 32);

                if (PTval > 33)
                {
                    Console.WriteLine($"PTval7:{PTval}");
                    Seed = NextSeed(Seed);
                }
            }
        }
        //--------------------------------------------
        return Seed;
    }

    //------------------------------------------------------------------------
    public static InSeedData GenSeed(PIDPrm param, DateTime Dt, uint Timer0)
    {
        uint[] Data = new uint[80];

        byte[] YMDD = new byte[4];

        YMDD[3] = toHex(Dt.Year % 100);
        YMDD[2] = toHex(Dt.Month);
        YMDD[1] = toHex(Dt.Day);
        YMDD[0] = (byte)(Dt.DayOfWeek);

        uint Date = BitConverter.ToUInt32(YMDD, 0);

        byte[] HMSZ = new byte[4];

        HMSZ[3] = toHex(Dt.Hour);
        if (Dt.Hour > 11) HMSZ[3] += 0x40;
        HMSZ[2] = toHex(Dt.Minute);
        HMSZ[1] = toHex(Dt.Second);
        HMSZ[0] = 0;

        uint Time = BitConverter.ToUInt32(HMSZ, 0);

        uint NazoAdd = 0;
        if (param.Values[14] == 0  || param.Values[14] == 1) NazoAdd = 0x4C;
        else if (param.Values[14] == 2 || param.Values[14] == 3) NazoAdd = 0x54;


        Data[0] = toLittleEndian(param.Values[0]);
        Data[1] = toLittleEndian(param.Values[1]);
        Data[2] = toLittleEndian(param.Values[2]);
        Data[3] = toLittleEndian(param.Values[2] + NazoAdd);
        Data[4] = toLittleEndian(param.Values[2] + NazoAdd);
        Data[5] = toLittleEndian((param.Values[3] << 16) + Timer0);
        Data[6] = (param.Values[6]);
        Data[7] = toLittleEndian(((param.Values[8] ^ param.Values[9])) ^ toLittleEndian(param.Values[7]));
        Data[8] = (Date);
        Data[9] = (Time);
        Data[10] = 0;
        Data[11] = 0;
        Data[12] = toLittleEndian(param.Values[10]);
        Data[13] = (param.Values[11]);
        Data[14] = (param.Values[12]);
        Data[15] = (param.Values[13]);



        //------------------------------------------------------
        for (int t = 16; t < 80; t++)
        {
            var w = Data[t - 3] ^ Data[t - 8] ^ Data[t - 14] ^ Data[t - 16];
            Data[t] = (w << 1) | (w >> 31);
        }

        const uint H0 = 0x67452301;
        const uint H1 = 0xEFCDAB89;
        const uint H2 = 0x98BADCFE;
        const uint H3 = 0x10325476;
        const uint H4 = 0xC3D2E1F0;

        uint A, B, C, D, E;
        A = H0; B = H1; C = H2; D = H3; E = H4;

        for (int t = 0; t < 20; t++)
        {
            var temp = ((A << 5) | (A >> 27)) + ((B & C) | ((~B) & D)) + E + Data[t] + 0x5A827999;
            E = D;
            D = C;
            C = (B << 30) | (B >> 2);
            B = A;
            A = temp;
        }
        for (int t = 20; t < 40; t++)
        {
            var temp = ((A << 5) | (A >> 27)) + (B ^ C ^ D) + E + Data[t] + 0x6ED9EBA1;
            E = D;
            D = C;
            C = (B << 30) | (B >> 2);
            B = A;
            A = temp;
        }
        for (int t = 40; t < 60; t++)
        {
            var temp = ((A << 5) | (A >> 27)) + ((B & C) | (B & D) | (C & D)) + E + Data[t] + 0x8F1BBCDC;
            E = D;
            D = C;
            C = (B << 30) | (B >> 2);
            B = A;
            A = temp;
        }
        for (int t = 60; t < 80; t++)
        {
            var temp = ((A << 5) | (A >> 27)) + (B ^ C ^ D) + E + Data[t] + 0xCA62C1D6;
            E = D;
            D = C;
            C = (B << 30) | (B >> 2);
            B = A;
            A = temp;
        }

        ulong Seed = toLittleEndian(H1 + B);
        Seed <<= 32;
        Seed |= toLittleEndian(H0 + A);

        //------------------------------------------------------



        InSeedData SeedData = new InSeedData
        {
            Seed = Seed,
            Year = (byte)(Dt.Year % 100),
            Month = (byte)Dt.Month,
            Day = (byte)Dt.Day,
            Hour = (byte)Dt.Hour,
            Minute = (byte)Dt.Minute,
            Second = (byte)Dt.Second,
            VCount = param.Values[3],
            InTimer0 = Timer0
        };
        return SeedData;
    }

    static byte toHex(int value)
    {
        return (byte)((value / 10) * 6 + value);
    }

    static uint toLittleEndian(uint values)
    {
        return ((values & 0x000000FF) << 24) |
                ((values & 0x0000FF00) << 8) |
                ((values & 0x00FF0000) >> 8) |
                ((values & 0xFF000000) >> 24);
    }
}
