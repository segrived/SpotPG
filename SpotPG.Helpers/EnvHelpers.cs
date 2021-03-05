using System;

namespace SpotPG.Helpers
{
    public static class EnvHelpers
    {
        public static string GetEnvOrDefault(string envKey, string value)
            => Environment.GetEnvironmentVariable(envKey) ?? value;

        public static bool HasEnv(string envKey)
            => Environment.GetEnvironmentVariable(envKey) != null;
    }
}