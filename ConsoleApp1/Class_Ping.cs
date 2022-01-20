using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace ConsoleApp1
{
    public class Class_Ping
    {
        public Class_Ping()
        {
            Main();
            Console.ReadLine();
        }
        void Main()
        {
            GetDey(new List<string> { "www.baidu.com", "www.pq-erp.com", "www.hao123.com", "www.cnblogs.com", "www.qq.com", "www.priest.ink" });
        }

        public static void GetDey(string ipStr)
        {
            //构造Ping实例
            Ping pingSender = new Ping();
            //Ping 选项设置
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            //测试数据
            string data = GetStr(65500);
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            //设置超时时间
            int timeout = 10000;
            //调用同步 send 方法发送数据,将返回结果保存至PingReply实例
            PingReply reply = pingSender.Send(ipStr, timeout, buffer, options);
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
        public static void GetDey(List<string> ipStrs)
        {
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
                Console.Write(ipStr+"：");
                PingReply reply = pingSender.Send(ipStr, timeout, buffer, options);
                PingReplyStatus(reply, buffer);
            });
        }
        public static void PingReplyStatus(PingReply reply,byte[] buffer)
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
    }
}

//来自 14.215.177.39 的回复: 字节 = 32 时间 = 6ms TTL = 56
//115.29.223.128