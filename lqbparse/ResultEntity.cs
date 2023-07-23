using System.Text.Json.Serialization;

namespace LqbParse;

public class ResultEntity
{
    public string School { get; set; }

    //public List<Player> Player { get; set; }
    public List<Player> Player { get; set; }
}

public class Player
{
    public string Name { get; set; }
    public int Id { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ContestType ContestType { get; set; }

    public int PrizeLevel { get; set; }
    public bool FinalContest { get; set; }
}