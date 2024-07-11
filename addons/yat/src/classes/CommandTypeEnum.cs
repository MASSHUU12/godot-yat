using System.Collections.Generic;
using System.Text;
using YAT.Enums;

namespace YAT.Classes;

public class CommandTypeEnum : CommandType
{
    public readonly Dictionary<string, int> Values;

    public CommandTypeEnum(string name, bool isArray, Dictionary<string, int> values)
    : base(name, ECommandInputType.Enum, isArray)
    {
        Values = values;
        TypeDefinition = GenerateTypeDefinition();
    }

    protected override string GenerateTypeDefinition()
    {
        StringBuilder ss = new();

        var enumerator = Values.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var item = enumerator.Current;

            _ = ss.Append(
                (ss.Length == 0 ? string.Empty : ", ") +
                $"{item.Key}({item.Value})"
            );
        }

        return ss.ToString();
    }
}
