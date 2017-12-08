//using ServerData;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Intellidoorn_software
{
    class ReaderConnection
    {
        public static ReaderConnection _instance { get; private set; }
        public static bool disconnected { get; private set; }
        public static bool first = true;
        private System.IO.Ports.SerialPort serialPort1;

        static Thread scanThread;
        static Thread controlThread;
        static Thread standThread;
        static Thread laserThread;
        static NetworkStream stream;
        static StreamWriter output;
        static StreamReader input;
        static TcpClient socket;


        static string host = "192.168.10.99";
        static int port = 23;

        public static LocationAlgorithm state;
        public static Serial serial1;
        public static List<TagInfo> tags;
        public static List<Stand> stands;
        public static TagInfo closestTag;
        public static TagInfo currentCarpet;
        public static string currentCarpetNumber;
        public static string targetStand;

        public static String standCode = "100000";
        public static TagInfo strongestTag = null;
        public static double laserHeight = -200.0;
        

        public ReaderConnection()
        {
            scanThread = new Thread(Run);
            controlThread = new Thread(PrintTags);
            standThread = new Thread(PrintLocation);
            laserThread = new Thread(laserDistance);
            tags = new List<TagInfo>();
            stands = new List<Stand>();
            state = new LocationAlgorithm();
            serial1 = new Serial();


            Stand a1 = new Stand(1, "100000000000000000000103", "100000000000000000000112", "100000000000000000000126", "100000000000000000000127", "A", 1.30, 0.00, 1);
            Stand a2 = new Stand(2, "100000000000000000000112", "100000000000000000000103", "100000000000000000000118", "100000000000000000000123", "A", 1.00, 1.30, 2);
            Stand a3 = new Stand(3, "100000000000000000000123", "100000000000000000000118", "100000000000000000000113", "100000000000000000000102", "A", 1.00, 2.35, 3);
            Stand a4 = new Stand(4, "100000000000000000000102", "100000000000000000000113", "100000000000000000000130", "100000000000000000000130", "A", 1.00, 3.35, 4);

            Stand b1 = new Stand(1, "100000000000000000000106", "100000000000000000000115", "100000000000000000000128", "100000000000000000000129", "B", 1.30, 0.00, 1);
            Stand b2 = new Stand(2, "100000000000000000000106", "100000000000000000000115", "100000000000000000000116", "100000000000000000000121", "B", 1.00, 1.30, 2);
            Stand b3 = new Stand(3, "100000000000000000000109", "100000000000000000000116", "100000000000000000000121", "100000000000000000000114", "B", 1.00, 2.35, 3);
            Stand b4 = new Stand(4, "100000000000000000000109", "100000000000000000000114", "100000000000000000000130", "100000000000000000000130", "B", 1.00, 3.35, 4);

            Stand c1 = new Stand(1, "100000000000000000000110", "100000000000000000000130", "100000000000000000000130", "100000000000000000000130", "C", 1.30, 0.00, 1);
            Stand c2 = new Stand(2, "100000000000000000000110", "100000000000000000000117", "100000000000000000000130", "100000000000000000000130", "C", 1.00, 1.30, 2);
            Stand c3 = new Stand(3, "100000000000000000000117", "100000000000000000000107", "100000000000000000000130", "100000000000000000000130", "C", 1.00, 2.35, 3);
            Stand c4 = new Stand(4, "100000000000000000000107", "100000000000000000000130", "100000000000000000000130", "100000000000000000000130", "C", 1.00, 3.35, 4);

            stands.Add(a1);
            stands.Add(a2);
            stands.Add(a3);
            stands.Add(a4);
            stands.Add(b1);
            stands.Add(b2);
            stands.Add(b3);
            stands.Add(b4);
            stands.Add(c1);
            stands.Add(c2);
            stands.Add(c3);
            stands.Add(c4);

            disconnected = false;
        }

        public static ReaderConnection GetInstance()
        {
            if (_instance == null)
                _instance = new ReaderConnection();

            return _instance;
        }

        public static void OpenConnection()
        {
            try
            {
                socket = new TcpClient(host, port);
                stream = socket.GetStream();
                output = new StreamWriter(stream);
                input = new StreamReader(stream);
                

                scanThread.Start();
            }
            catch (SocketException)
            {
                Console.WriteLine($"Couldn't connect to reader on host { host }.");
            }
            
            //controlThread.Start();
            Thread.Sleep(600);
            standThread.Start();
            laserThread.Start();
        }

        public static void CloseConnection()
        {
            disconnected = true;

            if (socket != null)
                socket.Close();
        }

        public static void PrintTags()
        {
            while (!disconnected)
            {
                //Console.Clear();
                foreach (TagInfo t in tags) {
                    Console.WriteLine(t.ToString());
                }
                Console.WriteLine("-----------------------------------------");

                Thread.Sleep(100);
            }

            Console.WriteLine("Exited print");
        }

        public static void PrintLocation()
        {
            while (!disconnected)
            {
                String s = state.getLocation();
                Console.WriteLine(s);
                Thread.Sleep(100);
            }
            Console.WriteLine("Exited Stand Print");
        }

        public static TagInfo getStrongestSignalTag()
        {
            bool first = true;
            foreach(TagInfo tag in tags)
            {
                if (first)
                {
                    strongestTag = tag;
                }
                if (tag.signalStrength > strongestTag.signalStrength)
                {
                    strongestTag = tag;
                }
            }
            first = false;
            return strongestTag;
        }

        public static void laserDistance()
        {
            while(!disconnected)
            {
                laserHeight = serial1.ReadData();
                //Console.WriteLine(laserHeight);
                Thread.Sleep(100);
            }
            Console.WriteLine("Exited Laser Connection");
        }

        public static void Run()
        {
            while (!disconnected)
            {
                string line = input.ReadLine();

                if (line == null)
                    continue;

                if (line.IndexOf("[\"hit\"") > -1)
                {
                    int codeIndex = line.IndexOf("\"item_code\"");
                    int codeEndIndex = line.IndexOf("item_codetype") - 16;
                    string itemCode = line.Substring(codeIndex + 13, codeEndIndex - codeIndex);

                    int strengthIndex = line.IndexOf("\"signal_strength\"");
                    int strength = Convert.ToInt32(line.Substring(strengthIndex + 18, 4));

                    TagInfo tag = tags.FirstOrDefault(t => t.itemCode == itemCode);

                    if (itemCode.Contains(standCode))
                    {
                        if (tag == null)
                            tags.Add(new TagInfo(itemCode, strength));
                        else
                            tag.signalStrength = strength;
                    }
                    
                }
                else if (line.IndexOf("\"presence\",\"delete\"") > -1)
                {
                    int codeIndex = line.IndexOf("\"item_code\"");
                    int codeEndIndex = line.IndexOf("item_codetype") - 16;
                    string itemCode = line.Substring(codeIndex + 13, codeEndIndex - codeIndex);

                    TagInfo tag = tags.FirstOrDefault(t => t.itemCode == itemCode);

                    if (tag != null)
                        tags.Remove(tag);
                }
            }

            Console.WriteLine("Exited run");
        }
    }
}
