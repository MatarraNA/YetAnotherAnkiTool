
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YetAnotherAnkiTool.Core.JSON
{
    public class FindNotesResponse
    {
        [JsonPropertyName("result")]
        public List<long>? Result { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
