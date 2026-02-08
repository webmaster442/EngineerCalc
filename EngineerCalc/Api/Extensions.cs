//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Api;

internal static class Extensions
{
    extension(CommandRunnerApi api)
    {
        public IDictionary<string, IReadOnlyList<string>> GetAutocompleteData()
        {
            Dictionary<string, IReadOnlyList<string>> result = new();

            foreach (var cmd in api.KnownCommands)
            {
                List<string> args = new();
                if (cmd.Value.Parameters != null)
                {
                    foreach (var parameter in cmd.Value.Parameters)
                    {
                        args.Add($"-{parameter.Short}");
                        args.Add($"--{parameter.Long}");
                    }
                }


                result.Add(cmd.Key, args);
            }

            return result;
        }
    }
}
