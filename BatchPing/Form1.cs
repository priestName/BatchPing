using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatchPing
{
    public partial class Form1 : Form
    {
        static List<List<IpRequestRecord>> ResultList;
        public Form1()
        {
            InitializeComponent();
            ResultList = new List<List<IpRequestRecord>>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "www.qq.com\r\nwww.cnblogs.com\r\nwww.hao123.com\r\nmap.qq.com\r\nwww.baidu.com";
        }
        public void Write(string str)
        {
            //byte[] myByte = System.Text.Encoding.UTF8.GetBytes("\r\n" + str);
            //Directory.CreateDirectory(@"Log\");
            //using (FileStream fsWrite = new FileStream(@"Log\User_Ip_Name.txt", FileMode.Create, FileAccess.Write))
            //{
            //    if (fsWrite.Length == 0)
            //    {
            //        fsWrite.Write(Encoding.UTF8.GetBytes("{"), 0, Encoding.UTF8.GetBytes("{").Length);
            //    }
            //    fsWrite.Write(myByte, 0, myByte.Length);
            //};

            using (var tw = new StreamWriter("", true, Encoding.UTF8))
            {
                tw.WriteLine("---------------");
                tw.WriteLine();
                tw.WriteLine("");
            }
        }

        TabPage NewPage(string Name,int index)
        {
            TabPage tabPage = new System.Windows.Forms.TabPage();
            tabPage.Location = new System.Drawing.Point(4, 26);
            tabPage.Name = Name;
            tabPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage.Size = new System.Drawing.Size(564, 396);
            tabPage.TabIndex = index;
            tabPage.Text = Name;
            tabPage.UseVisualStyleBackColor = true;

            return tabPage;
        }
        DataGridView NewDataGridView(string Name,DataTable dt)
        {
            DataGridView dataGridView = new System.Windows.Forms.DataGridView();
            dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new System.Drawing.Point(0, 0);
            dataGridView.Name = Name;
            dataGridView.RowTemplate.Height = 25;
            dataGridView.Size = new System.Drawing.Size(564, 396);
            dataGridView.ReadOnly = false;
            dataGridView.TabIndex = 0;
            dataGridView.DataSource = dt;

            return dataGridView;
        }

        public static List<IpRequestRecord> GetDey(List<string> ipStrs)
        {
            List<IpRequestRecord> lines = new List<IpRequestRecord>();
            //构造Ping实例
            Ping pingSender = new Ping();
            //Ping 选项设置
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            //测试数据
            string data = GetStr(64);
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            //设置超时时间
            int timeout = 10000;
            //调用同步 send 方法发送数据,将返回结果保存至PingReply实例
            ipStrs.ForEach(ipStr => {
                Console.Write(ipStr + "：");
                PingReply reply = pingSender.Send(ipStr, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    lines.Add(new IpRequestRecord()
                    {
                        IP = ipStr,
                        dataSize = buffer.Length,
                        RoundtripTime = reply.RoundtripTime,
                        Ttl = reply.Options.Ttl,
                    });
                }
                PingReplyStatus(reply, buffer);
            });
            return lines;
        }
        public static void PingReplyStatus(PingReply reply, byte[] buffer)
        {
            switch (reply.Status)
            {
                case IPStatus.Unknown:
                    break;
                case IPStatus.Success:
                    Console.WriteLine($"来自 {reply.Address.ToString()} 的回复: 字节 = {buffer.Length} 时间 = {reply.RoundtripTime}ms TTL = {reply.Options.Ttl}");
                    break;
                case IPStatus.DestinationNetworkUnreachable:
                    break;
                case IPStatus.DestinationHostUnreachable:
                    break;
                case IPStatus.DestinationProhibited:
                    //case IPStatus.DestinationProtocolUnreachable:
                    break;
                case IPStatus.DestinationPortUnreachable:
                    break;
                case IPStatus.NoResources:
                    break;
                case IPStatus.BadOption:
                    break;
                case IPStatus.HardwareError:
                    break;
                case IPStatus.PacketTooBig:
                    break;
                case IPStatus.TimedOut:
                    Console.WriteLine($"请求超时！");
                    break;
                case IPStatus.BadRoute:
                    break;
                case IPStatus.TtlExpired:
                    break;
                case IPStatus.TtlReassemblyTimeExceeded:
                    break;
                case IPStatus.ParameterProblem:
                    break;
                case IPStatus.SourceQuench:
                    break;
                case IPStatus.BadDestination:
                    break;
                case IPStatus.DestinationUnreachable:
                    break;
                case IPStatus.TimeExceeded:
                    break;
                case IPStatus.BadHeader:
                    break;
                case IPStatus.UnrecognizedNextHeader:
                    break;
                case IPStatus.IcmpError:
                    break;
                case IPStatus.DestinationScopeMismatch:
                    break;
                default:
                    break;
            }
        }
        public static string GetStr(int length)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                str.Append(" ");
            }
            return str.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
            button1.Enabled = false;
            NewControls();
        }
        DataTable GetHistoryData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("序号", typeof(int));
            dt.Columns.Add("IP", typeof(string));
            dt.Columns.Add("字节", typeof(string));
            dt.Columns.Add("时间", typeof(string));
            dt.Columns.Add("TTL", typeof(string));
            return dt;
        }
        public void NewControls()
        {
            var ipStr = textBox1.Text.Replace("\r\n", ",").Replace(" ", ",").Replace("，", ",").Replace("|", ",");
            textBox1.Text = ipStr.Replace(",", "\r\n");
            var ipStrs = ipStr.Split(',').ToList();
            Task.Run(() => {
                try
                {
                    int Count = ResultList.Count();
                    var lines = GetDey(ipStrs);
                    DataTable dt = GetHistoryData();
                    lines.ForEach(line =>
                    {
                        var row = dt.NewRow();
                        row["序号"] = dt.Rows.Count;
                        row["IP"] = line.IP;
                        row["字节"] = line.dataSize;
                        row["时间"] = $"{line.RoundtripTime}ms";
                        row["TTL"] = line.Ttl;
                        dt.Rows.Add(row);
                    });

                    var dataGridView = NewDataGridView($"ResultData{Count + 1}", dt);
                    var tabPage = NewPage($"第{Count + 1}次请求结果", Count);
                    tabPage.Controls.Add(dataGridView);
                    Action<int> action = (data) =>
                    {
                        tabControl1.Controls.Add(tabPage);
                        tabControl1.SelectedIndex = Count;
                        textBox1.ReadOnly = false;
                        button1.Enabled = true;
                    };
                    Invoke(action,0);

                    ResultList.Add(lines);

                }
                catch
                {
                    Action<int> action = (data) =>
                    {
                        textBox1.ReadOnly = false;
                        button1.Enabled = true;
                    };
                    Invoke(action, 0);
                }
            });
        }
    }
    public class IpRequestRecord
    { 
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 数据大小（字节）
        /// </summary>
        public int dataSize { get; set; }
        /// <summary>
        /// 时间（ms）
        /// </summary>
        public long RoundtripTime { get; set; }
        /// <summary>
        /// Ttl
        /// </summary>
        public int Ttl { get; set; }
    }
}
