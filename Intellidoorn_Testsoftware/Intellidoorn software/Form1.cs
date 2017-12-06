//using IntellidoornClient.InterfaceComponents;
//using ServerData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intellidoorn_software
{
    
    public partial class Form1 : Form
    {
        ReaderConnection reader;
        bool connected;
        public static string COMPort = "COM4";
        public Form1()
        {
            AllocConsole();
            connected = false;
            InitializeComponent();
            textBox1.Text = "COM4";
        }

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool AllocConsole();

    private void button1_Click(object sender, EventArgs e)
        {
            reader = ReaderConnection.GetInstance();
            ReaderConnection.OpenConnection();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            COMPort = textBox1.Text;
        }
    }
}
