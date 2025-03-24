using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MoxMatrix
{
  public static partial class UpgradeUtils
  {
    private const string AppName = "Mox Matrix(beta)";
    private const string VersionPattern = @"\/releases\/tag\/v([0-9]+\.[0-9]+\.[0-9]+)";
    private const string LatestReleaseUrl = "https://github.com/creepyLANguy/MoxMatrix/releases/latest/";
    private const string DownloadUrl = "https://github.com/creepyLANguy/MoxMatrix/releases/latest/download/MoxMatrix.zip";
    private const string TempDownloadPath = "MoxMatrix_latest.zip";
    private const string TempExtractPath = "MoxMatrix_latest";

    public static void Run()
    {
      var localVersion = GetSemanticVersionFromCurrentExecutable();

      var latestVersion = GetSemanticVersionFromUrl(LatestReleaseUrl);

      if (localVersion >= latestVersion)
      {
        //AL.
        //return;
      }

      var message =
        "A newer version is available." +
        Environment.NewLine + Environment.NewLine +
        "Would you like to install it?";

      var selection = MessageBox.Show(message, AppName, MessageBoxButtons.YesNo);

      if (selection == DialogResult.No)
      {
        return;
      }

      Log("Trying upgrade from version v" + localVersion + " to v" + latestVersion);
      TryUpgrade();
    }

    private static void Log(string s)
     => Console.WriteLine(s + Environment.NewLine);

    private static SemanticVersion GetSemanticVersionFromCurrentExecutable()
    {
      var entryAssembly = Assembly.GetEntryAssembly();
      var version = entryAssembly?.GetName().Version;

      return version == null ? new SemanticVersion() : new SemanticVersion(version.ToString());
    }

    private static SemanticVersion GetSemanticVersionFromUrl(string url)
    {
      var html = GetHtmlFromUrl(url);
      return GetSemanticVersionFromHtml(html);
    }

    private static string GetHtmlFromUrl(string url)
    {
      var html = string.Empty;

      using WebClient webClient = new();
      webClient.Headers.Add("user-agent", AppName);
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      try
      {
        html = webClient.DownloadString(url);
      }
      catch (WebException ex)
      {
        Console.WriteLine(ex);
      }

      return html;
    }

    private static SemanticVersion GetSemanticVersionFromHtml(string html)
    {
      var match = Regex.Match(html, VersionPattern);

      if (match.Success == false)
      {
        return new SemanticVersion();
      }

      var latestVersion = match.Groups[1].Value;
      return new SemanticVersion(latestVersion);
    }

    private static void TryUpgrade()
    {
      if (PerformAllUpgradeSteps())
      {
        //AL. //TODO - Relaunch
        //Relaunch();
        return;
      }

      var message_upgradeFailed =
        "Upgrade Failed, please try again later." +
        Environment.NewLine + Environment.NewLine +
        "The application will now restart.";

      MessageBox.Show(message_upgradeFailed, AppName, MessageBoxButtons.OK);

      Application.Restart();
    }

    private static bool PerformAllUpgradeSteps()
    {
      var steps = new List<Func<bool>>
      {
        FetchLatestRelease,
        UnzipLatestRelease,
        //AL. //TODO
        //ReplaceOldExeWithNewExe
        //Cleanup
      };

      foreach (var step in steps)
      {
        try
        {
          if (!step())
          {
            return false;
          }
        }
        catch (Exception ex)
        {
          Log(ex.Message);
          return false;
        }
      }

      Log("Upgrade Successful.");
      return true;
    }

    private static bool FetchLatestRelease()
    {
      using WebClient webClient = new();
      webClient.Headers.Add("user-agent", AppName);
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      webClient.DownloadFile(DownloadUrl, TempDownloadPath);
      return true;
    }

    private static bool UnzipLatestRelease()
    {
      if (Directory.Exists(TempExtractPath))
      {
        Directory.Delete(TempExtractPath, true);
      }

      ZipFile.ExtractToDirectory(TempDownloadPath, TempExtractPath);
      return true;
    }

    private static bool Cleanup()
    {
      if (File.Exists(TempDownloadPath))
      {
        File.Delete(TempDownloadPath);
      }

      if (Directory.Exists(TempExtractPath))
      {
        Directory.Delete(TempExtractPath, true);
      }

      return true;
    }
  }
}


