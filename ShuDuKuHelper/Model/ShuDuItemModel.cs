using System.Text;

namespace SuDoKuHelper.Model;

public class ShuDuItemModel : ICloneable
{
    public int Row { get; set; }

    public int Col { get; set; }

    public int Block { get; set; }

    public int? Val { get; set; }

    public string? Value => Val == default ? "?" : Val?.ToString();

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

    public object Clone()
    {
        var possible = new HashSet<int>();

        foreach (var i in this.PossibleValue)
        {
            possible.Add(i);
        }

        return new ShuDuItemModel()
        {
            Block = this.Block,
            Col = this.Col,
            Row = this.Row,
            Val = this.Val,
            PossibleValue = possible
        };

    }

}