using Intellidoorn_software.ReaderStates;
//using ServerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Intellidoorn_software
{
    class ReaderConnection
    {
        public static ReaderConnection _instance { get; private set; }
        public static bool disconnected { get; private set; }

        static Thread scanThread;
        static Thread controlThread;
        static NetworkStream stream;
        static StreamWriter output;
        static StreamReader input;
        static TcpClient socket;

        static string host = "192.168.10.99";
        static int port = 23;

        public static AReaderState state;
        //public static List<Stand> stands;
        public static List<TagInfo> tags;
        public static TagInfo currentCarpet;
        public static string currentCarpetNumber;
        public static string targetStand;

        public ReaderConnection()
        {
            scanThread = new Thread(Run);
            controlThread = new Thread(PrintTags);
            tags = new List<TagInfo>();
            //state = new ClearState();

            disconnected = false;
        }

        public static void ReaderConnectionRetarded()
        {
            scanThread = new Thread(Run);
            controlThread = new Thread(PrintTags);
            tags = new List<TagInfo>();
            //state = new ClearState();

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

            controlThread.Start();
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

                foreach (TagInfo t in tags)
                    Console.WriteLine(t.ToString());
                Console.WriteLine("-----------------------------------------");

                Thread.Sleep(100);
            }

            Console.WriteLine("Exited print");
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

                    if (tag == null)
                        tags.Add(new TagInfo(itemCode, strength));
                    else
                        tag.signalStrength = strength;
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
