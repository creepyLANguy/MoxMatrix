/*
SemanticVersion
https://github.com/creepyLANguy/SemanticVersion

| Test Case                | Input String   | Major | Minor | Patch | Expected Behavior
|--------------------------|----------------|-------|-------|-------|----------------------------------------
| **Valid Cases**          |                |       |       |       |
| Standard version         | "1.2.3"        |   1   |   2   |   3   | ✅ Parses correctly
| No patch                 | "1.2"          |   1   |   2   |   0   | ✅ Defaults patch to 0
| No minor & patch         | "1"            |   1   |   0   |   0   | ✅ Defaults minor & patch to 0
| Empty string             | "" (empty)     |   0   |   0   |   0   | ✅ Defaults to 0.0.0
| **Negative Values**      |                |       |       |       |
| Negative major           | "-1.2.3"       |   0   |   2   |   3   | ✅ Converts negative major to 0
| Negative minor           | "1.-2.3"       |   1   |   0   |   3   | ✅ Converts negative minor to 0
| Negative patch           | "1.2.-3"       |   1   |   2   |   0   | ✅ Converts negative patch to 0
| **Invalid Formats**      |                |       |       |       |
| Extra version part       | "1.2.3.4"      |   -   |   -   |   -   | ❌ Should throw FormatException
| Non-numeric input        | "abc"          |   -   |   -   |   -   | ❌ Should throw FormatException
| Mixed valid & invalid    | "1..2"         |   -   |   -   |   -   | ❌ Should throw FormatException
| Trailing delimiter       | "1.2."         |   1   |   2   |   0   | ✅ Treats trailing dot as missing patch
| Leading delimiter        | ".1.2.3"       |   -   |   -   |   -   | ❌ Should throw FormatException
| **Edge Cases**           |                |       |       |       |
| Single delimiter         | "."            |   -   |   -   |   -   | ❌ Should throw FormatException
| Hyphenated version       | "1-2-3"        |   1   |   2   |   3   | ✅ Parses correctly (hyphens as delimiters)
| Spaces in version        | " 1.2.3 "      |   1   |   2   |   3   | ✅ Trims spaces and parses correctly
*/

namespace MoxMatrix.Upgrade;

public readonly struct SemanticVersion : IEquatable<SemanticVersion>
{
  public int Major { get; }
  public int Minor { get; }
  public int Patch { get; }

  public SemanticVersion(string versionString)
  {
    if (string.IsNullOrWhiteSpace(versionString))
    {
      throw new ArgumentException(@"Version string cannot be null or empty.", nameof(versionString));
    }

    versionString = versionString.Trim().ToLower().Replace("v", string.Empty);

    var delimiters = new[] { '.', '-' };
    var parts = versionString.Split(delimiters, StringSplitOptions.None);

    if (parts.Length == 0 || parts.Any(p => !int.TryParse(p, out _)))
    {
      throw new FormatException($"Invalid version format: {versionString}");
    }        

    Major = int.TryParse(parts.ElementAtOrDefault(0), out var major) && major >= 0 ? major : 0;
    Minor = int.TryParse(parts.ElementAtOrDefault(1), out var minor) && minor >= 0 ? minor : 0;
    Patch = int.TryParse(parts.ElementAtOrDefault(2), out var patch) && patch >= 0 ? patch : 0;
  }

  public SemanticVersion(int major = 0, int minor = 0, int patch = 0)
  {
    Major = major >= 0 ? major : throw new ArgumentOutOfRangeException(nameof(major),@"Major version cannot be negative.");
    Minor = minor >= 0 ? minor : throw new ArgumentOutOfRangeException(nameof(minor),@"Minor version cannot be negative.");
    Patch = patch >= 0 ? patch : throw new ArgumentOutOfRangeException(nameof(patch),@"Patch version cannot be negative.");
  }

  public bool Equals(SemanticVersion other)
    => Major == other.Major && Minor == other.Minor && Patch == other.Patch;

  public override bool Equals(object? obj)
    => obj is SemanticVersion other && Equals(other);

  public override int GetHashCode()
    => HashCode.Combine(Major, Minor, Patch);

  public static bool operator ==(SemanticVersion a, SemanticVersion b)
    => a.Equals(b);

  public static bool operator !=(SemanticVersion a, SemanticVersion b)
    => !(a == b);

  public static bool operator <(SemanticVersion a, SemanticVersion b)
    => (a.Major, a.Minor, a.Patch).CompareTo((b.Major, b.Minor, b.Patch)) < 0;

  public static bool operator >(SemanticVersion a, SemanticVersion b)
    => b < a;

  public static bool operator <=(SemanticVersion a, SemanticVersion b)
    => !(b < a);

  public static bool operator >=(SemanticVersion a, SemanticVersion b)
    => !(a < b);

  public override string ToString()
    => $"{Major}.{Minor}.{Patch}";
}