using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Class_Task2
    {
        public Class_Task2()
        {
            var sw = new Stopwatch();
            sw.Start();

            Test1();
            Test2();
            Test3();

            sw.Stop();
            var ts = sw.Elapsed;
            Console.WriteLine($"Class 总共耗时：{ts.TotalMilliseconds}ms");
        }
        public async Task Test1()
        {
            var sw = new Stopwatch();
            sw.Start();

            var userName = await GetUserNameAsync();
            var userAge = await GetUserAgeAsync();
            var userSex = await GetUserSexAsync();

            sw.Stop();
            var ts = sw.Elapsed;
            Console.WriteLine($"await 总共耗时：{ts.TotalMilliseconds}ms");
        }
        public async Task Test2()
        {
            var sw = new Stopwatch();
            sw.Start();

            var userName = string.Empty;
            var userAge = string.Empty;
            var userSex = string.Empty;
            Task.WaitAll(new Task[] {
                GetUserNameAsync(),
                GetUserAgeAsync(),
                GetUserSexAsync()
            });

            sw.Stop();
            var ts = sw.Elapsed;
            Console.WriteLine($"WaitAll 总共耗时：{ts.TotalMilliseconds}ms");
        }
        public async Task Test3()
        {
            var sw = new Stopwatch();
            sw.Start();

            var userName = string.Empty;
            var userAge = string.Empty;
            var userSex = string.Empty;

            var t = Task.WhenAll(
                GetUserNameAsync(),
                GetUserAgeAsync(),
                GetUserSexAsync()
            );
            t.Wait();
            string[] result = t.Result;

            sw.Stop();
            var ts = sw.Elapsed;

            Console.WriteLine($"WhenAll 总共耗时：{ts.TotalMilliseconds}ms");
        }
        private async Task<string> GetUserNameAsync()
        {
            await Task.Delay(500);
            return "小明";
        }

        private async Task<string> GetUserAgeAsync()
        {
            await Task.Delay(800);
            //return 11;
            return "11";
        }

        private async Task<string> GetUserSexAsync()
        {
            await Task.Delay(900);
            return "11";
        }

    }
}
