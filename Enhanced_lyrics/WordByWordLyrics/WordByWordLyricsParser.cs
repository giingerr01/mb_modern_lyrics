using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace MusicBeePlugin
{
    public static class WordLrcParser
    {
        private static readonly Regex WordTimingRegex = new
        Regex(@"<(?<min>\d{2}):(?<sec>\d{2})\.(?<ms>\d{2,3})>\s*(?<word>[^<]+?)(?=\s*<|\s*$)",
        RegexOptions.Compiled);

    }
}