
using System.Collections.Generic;

public class Epic950Code39
{
    public string EncodeThis { get; set; }
    public byte[] Build()
    {
        var payload = new List<byte>();

        var bytes = System.Text.Encoding.ASCII.GetBytes(EncodeThis);
        payload.AddRange(bytes);

        // Force null terminated string
        if (EncodeThis[EncodeThis.Length - 1] != '\0')
        {
            payload.Add(0);
        }

        return payload.ToArray();
    }
}
