using System;
using System.Reflection.Metadata;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace LqbParse
{
    internal class Program
    {
        private static Func<string, ContestType> ParseContestType = x =>
        {
            return x switch
            {
                "C/C++程序设计研究生组" => ContestType.CCppG,
                "C/C++程序设计大学A组" => ContestType.CCppA,
                "C/C++程序设计大学B组" => ContestType.CCppB,
                "C/C++程序设计大学C组" => ContestType.CCppC,
                "Java软件开发研究生组" => ContestType.JavaG,
                "Java软件开发大学A组" => ContestType.JavaA,
                "Java软件开发大学B组" => ContestType.JavaB,
                "Java软件开发大学C组" => ContestType.JavaC,
                "Python程序设计研究生组" => ContestType.PythonG,
                "Python程序设计大学A组" => ContestType.PythonA,
                "Python程序设计大学B组" => ContestType.PythonB,
                "Python程序设计大学C组" => ContestType.PythonC,
                "Web应用开发大学组" => ContestType.WebUG,
                "Web应用开发职业院校组" => ContestType.WebVC,
                "软件测试大学组" => ContestType.SwTest,
                _ => ContestType.Unknown
            };
        };

        private static Func<string, int> ParsePrizeLevel = x =>
        {
            return x switch
            {
                "一等奖" => 1,
                "二等奖" => 2,
                "三等奖" => 3,
                _ => 0
            };
        };

        private static Func<IGrouping<string, ReadEntity>, List<Player>> GetGroupResult = x =>
        {
            return x.Select(item => new Player
                {
                    Name = item.Name,
                    Id = item.Id,
                    ContestType = item.ContestType,
                    PrizeLevel = item.PrizeLevel,
                    FinalContest = item.FinalContest
                })
                .ToList();
        };

        static void Main(string[] args)
        {
            Console.WriteLine("file:");
            var filepath = Console.ReadLine().Trim();
            var all = File.ReadLines(filepath).ToList();
            var readEntities = all.Select(i => i.Split(','))
                .Select(t => new ReadEntity
                {
                    Id = int.Parse(t[1]),
                    School = t[2],
                    Name = t[3],
                    ContestType = ParseContestType(t[4]),
                    PrizeLevel = ParsePrizeLevel(t[5]),
                    FinalContest = t[6] == "是"
                })
                .ToList();
            var group = readEntities.GroupBy(x => x.School);
            List<ResultEntity> results = group
                .Select(item => new ResultEntity { School = item.Key, Player = GetGroupResult(item) }).ToList();

            File.WriteAllText("result.json", JsonSerializer.Serialize(results, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,
            }));
            var total = 0;
            foreach (var item in results)
            {
                total += item.Player.Count;
                Console.WriteLine($"{item.School} : {item.Player.Count}, now total {total}");
            }
        }
    }
}