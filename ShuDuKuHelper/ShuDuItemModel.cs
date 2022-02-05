using System.Text;

namespace ShuDuKuHelper;

public class ShuDuItemModel
{
    public int Row { get; set; }

    public int Col { get; set; }

    public int Block { get; set; }

    public int? Val{ get; set; }

    public string Value => Val == null ? "?" : Val.ToString();

    public HashSet<int> PossibleValue { get; set; }


    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (var i in PossibleValue)
        {
            sb.Append($"{i},");
        }

        return $"Row:{Row},Col:{Col}@Block:{Block} => "
            + (Val == null
                ? $"[{sb.ToString().TrimEnd(',')}]"
                : $"Value:{Val}");
    }
}