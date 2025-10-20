using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Updatum;

namespace YetAnotherAnkiTool.Core.API
{
    public static class GithubAPI
    {
        internal static readonly UpdatumManager AppUpdater = new("MatarraNA", "YetAnotherAnkiTool")
        {
            AssetExtensionFilter = ".zip",
            AssetRegexPattern = @"^YetAnotherAnkiTool_win-x64_v(?<version>\d+\.\d+\.\d+\.\d+)\.zip"
        };
    }
}
