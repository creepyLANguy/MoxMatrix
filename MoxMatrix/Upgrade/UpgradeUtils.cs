using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MoxMatrix
{
  public static partial class UpgradeUtils
  {
    private const string DownloadUrl = "https://github.com/creepyLANguy/MoxMatrix/releases/latest/download/MoxMatrix.zip";
    private const string LatestReleaseUrl = "https://github.com/creepyLANguy/MoxMatrix/releases/latest/";
    private const string VersionPattern = @"\/releases\/tag\/v([0-9]+\.[0-9]+\.[0-9]+)";
    private const string AppName = "Mox Matrix(beta)";

    public static void Run()
    {
      var localVersion = GetSemanticVersionFromCurrentExecutable();

      var latestVersion = GetSemanticVersionFromUrl(LatestReleaseUrl); ;

      if (localVersion <= latestVersion)
      {
        var message = 
          "A newer version is available." +
          Environment.NewLine + Environment.NewLine +
          "Would you like to install it?";

        var selection = MessageBox.Show(message, AppName, MessageBoxButtons.YesNo);

        if (selection == DialogResult.No)
        {
          return;
        }
      }      

      try
      {
        Log("Upgrading from version v" + localVersion + " to v" + latestVersion);
        if (Upgrade() == false)
        {
          var message = 
            "Upgrade Failed, please try again later." +
            Environment.NewLine + Environment.NewLine +
            "The application will now restart.";

          var selection = MessageBox.Show(message, AppName, MessageBoxButtons.OK);

          Application.Restart();
        }
      }
      catch (Exception ex)
      {
        Log(ex.Message);
      }
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

    private static bool Upgrade()
    {   
      var steps = new List<Func<bool>>
      {
        //AL. //TODO
        //FetchLatestRelease,
        //UnzipLatestRelease,
        //CopyNewFiles,
        //Cleanup,
        //LaunchNewVersion,
      };

      for (var index = 0; index < steps.Count; index++)
      {
        var step = steps[index];
        if (step() == false)
        {
          return false;
        }
      }

      Log("Upgrade Successful.");
     
      return true;
    }
  } 
}