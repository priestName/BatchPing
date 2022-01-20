using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ConsoleApp1
{
    public class Class_Redis
    {
        private static readonly string ConnectionWriteString = "127.0.0.1:6379";
        private static readonly IConnectionMultiplexer ConnMultiplexer = ConnectionMultiplexer.Connect(ConnectionWriteString);
        private static readonly IDatabase _db = ConnMultiplexer.GetDatabase(0);
        public Class_Redis()
        {
            Main();
        }
        void Main()
        {
            //SubOperation();
            //SubOperation2();
            //HashOperation();
            ListOperation();
            //IncrementOrDecrement();
            //StringOperation();
        }
        void SubOperation()
        {
            SubOperation2();
            ISubscriber sub = ConnMultiplexer.GetSubscriber();

            for (int i = 0; i < 10; i++)
            {
                if (i < 5)
                {
                    sub.Publish("Channel1", "msg" + i);//向频道 Channel1 发送信息
                }
                else
                {
                    sub.Publish("Channel2", "msg" + i);//向频道 Channel1 发送信息
                }
            }
            System.Threading.Thread.Sleep(5*6000);
        }
        void SubOperation2()
        {
            ISubscriber sub = ConnMultiplexer.GetSubscriber();
            sub.Subscribe("Channel1", new Action<RedisChannel, RedisValue>((channel, message) => {
                Console.WriteLine($"SUB1：Channel1 订阅收到消息：{message},{DateTime.Now.ToString()}");
            }));
            sub.Subscribe("Channel2", new Action<RedisChannel, RedisValue>((channel, message) => {
                Console.WriteLine($"SUB1：Channel2 订阅收到消息：{message},{DateTime.Now.ToString()}");
            }));

            ISubscriber sub2 = ConnMultiplexer.GetSubscriber();
            sub2.Subscribe("Channel1", new Action<RedisChannel, RedisValue>((channel, message) => {
                Console.WriteLine($"SUB2：Channel1 订阅收到消息：{message},{DateTime.Now.ToString()}");
            }));

            ISubscriber sub3 = ConnMultiplexer.GetSubscriber();
            sub3.Subscribe("Channel2", new Action<RedisChannel, RedisValue>((channel, message) => {
                Console.WriteLine($"SUB3：Channel2 订阅收到消息：{message},{DateTime.Now.ToString()}");
            }));
        }
        void HashOperation()
        {
            List<HashEntry> hashFields = new List<HashEntry>()
            {
                new HashEntry("Test_Key1","Test_Value1"),
                new HashEntry("Test_Key2","Test_Value2"),
                new HashEntry("Test_Key3","Test_Value3"),
                new HashEntry("Test_Key4","Test_Value4"),
                new HashEntry("Test_Key5","Test_Value5"),
            };
            _db.HashSet("Hash_Test", hashFields.ToArray());//批量添加Hash列表
            var add1 = _db.HashSet("Hash_Test", "Test_Key7", "Test_Value7");//单个添加或修改
            var Test_Key1 = _db.HashGet("Hash_Test", "Test_Key1");//获取Hash列表中对应值
            var edit1 = _db.HashSet("Hash_Test", "Test_Key1", "Test_Value_Edit");//修改
            Test_Key1 = _db.HashGet("Hash_Test", "Test_Key1");//获取
            var Hash_Tests = _db.HashGetAll("Hash_Test");//获取Hash列表所有键值
            var Hash_Test_Values = _db.HashValues("Hash_Test");//获取Hash列表中所有的值

            var Ex1 =  _db.HashExists("Hash_Test", "Test_Key5");//判断是否存在
            var Del = _db.HashDelete("Hash_Test", "Test_Key5");//删除
            var Ex2 = _db.HashExists("Hash_Test", "Test_Key5");
        }
        void ListOperation()
        {
            for (int i = 0; i < 5; i++)
            {
                _db.ListLeftPush("Test_List", i);//顶部开始插入
            }
            for (int i = 0; i < 5; i++)
            {
                _db.ListRightPush("Test_List", i);//底部开始插入
            }
            var len = _db.ListLength("Test_List");
            var LeftVal = _db.ListLeftPop("Test_List");//顶部取出
            var RightVal = _db.ListRightPop("Test_List");//底部取出（取出后即删除）
            var list = _db.ListRange("Test_List");//整个列表

            var val1 = _db.ListInsertAfter("Test_List", "0", "TestAfter_0");//在第一个匹配行后添加
            var val2 = _db.ListInsertBefore("Test_List", "0", "TestBefore_0");//在第一个匹配行前添加
            var val3 = _db.ListGetByIndex("Test_List", 8);//获取指定索引位置数据，0开始
            var val4 = _db.ListRemove("Test_List", "TestBefore_0");//删除所有匹配项
            _db.ListSetByIndex("Test_List", 7, "9");//修改指定索引处值
            _db.ListTrim("Test_List", 1, 8);//只保留索引区间内的值，索引从0开始或者反向序列-1开始
            _db.ListTrim("Test_List", -7, -2);

        }
        void IncrementOrDecrement()
        {
            for (int i = 0; i < 3; i++)
            {
                //增量更新
                double increment = _db.StringIncrement("StringIncrement", 2);
                Console.WriteLine($"{DateTime.Now.ToString()}---{increment}");
            }
            for (int i = 0; i < 3; i++)
            {
                //减量更新
                double decrement = _db.StringDecrement("StringDecrement", 2);
                Console.WriteLine($"{DateTime.Now.ToString()}---{decrement}");
            }
        }
        void StringOperation()
        {
            //可以设置过期时间，不设置或为null则为长期有效
            var IsOk = _db.StringSet("Test_Key", "Test_Value");//, TimeSpan.FromSeconds(60)
            _db.StringAppend("Test_Key", "Test_Value_Append");
            _db.StringSet("Test_Key", "Test_Value_Edit");
            _db.KeyDelete("Test_Key");
            var Test_Key = _db.StringGet("Test_Key");
            Console.WriteLine($"{DateTime.Now.ToString()}---{Test_Key}");
            System.Threading.Thread.Sleep(6000);
            var Test_Key1 = _db.StringGet("Test_Key");
            Console.WriteLine($"{DateTime.Now.ToString()}---{Test_Key1}");
        }
    }
}
