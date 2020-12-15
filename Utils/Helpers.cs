using System;

namespace SpotPG.Utils
{
    public static class Helpers
    {
        public static string GetEnvOrDefault(string envKey, string value)
            => Environment.GetEnvironmentVariable(envKey) ?? value;

        public static bool HasEnv(string envKey)
            => Environment.GetEnvironmentVariable(envKey) != null;
    }
}