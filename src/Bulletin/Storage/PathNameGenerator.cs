using System;
using System.IO;

namespace Bulletin.Storage
{
    public static class PathNameGenerator
    {
        public static string GenerateUniquePathName(string extension)
        {
            var random = Guid.NewGuid().ToString();

            return Path.Combine(
                random.Substring(0, 2), random.Substring(2, 2),
                $"{random}{extension}");
        }
    }
}