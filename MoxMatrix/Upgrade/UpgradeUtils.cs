using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Text.Json;

namespace MoxMatrix.Upgrade;

public static class UpgradeUtils
{
  private const string AppName = "Mox Matrix(beta)";
  private const string LatestVersionEndpoint = "https://api.github.com/repos/creepyLANguy/MoxMatrix/releases/latest";
  private const string DownloadUrl = "https://github.com/creepyLANguy/MoxMatrix/releases/latest/download/MoxMatrix.zip";
  private const string UserAgent = "MoxMatrixUpdater/1.0";
  private const string TempDownloadPath = "MoxMatrix_latest.zip";
  private const string TempExtractPath = "MoxMatrix_latest";
  private const string OutdatedMarker = "_outdated";
  private const string DateTimeFormatPattern = "dd-MM-yyyy_HH-mm-ss";
  private const int CleanupMaxFailures = 5;
  private const int CleanupSleepMs = 30000;
  private const string ExecutableExtension = ".exe";
  private const int KillProcessRetrySleepMs = 3000;

  private static void Log(string s = "")
    => Debug.WriteLine(">>\t" + (s.Length == 0 ? new StackTrace().GetFrame(1)?.GetMethod()?.Name + "();" : s));

  public static void Run()
  {
    Task.Run(CleanupOutdatedFiles);

    var localVersion = GetVersionFromCurrentExecutable();
    
    var latestVersion = GetLatestVersionInfo();

    if (localVersion >= latestVersion)
    {
      return;
    }

    var message =
      "A newer version is available." + Environment.NewLine + Environment.NewLine +
      "Current version :\t" + localVersion + Environment.NewLine +
      "Updated version :\t" + latestVersion + Environment.NewLine +
      Environment.NewLine +
      "Would you like to install it?";

    var selection = MessageBox.Show(message, AppName, MessageBoxButtons.YesNo);

    if (selection == DialogResult.No)
    {
      return;
    }

    Log("Trying upgrade from version v" + localVersion + " to v" + latestVersion);

    TryUpgrade();
  }

  private static void CleanupOutdatedFiles()
  {
    var failures = 0;

    while (failures < CleanupMaxFailures)
    {
        var allFiles =
          Directory
            .GetFiles(Directory.GetCurrentDirectory(), "*" + OutdatedMarker + "*", SearchOption.AllDirectories)
            .Select(Path.GetFileName)
            .ToList();

      if (allFiles.Count == 0)
      {
        break;
      }

      try
      {
        foreach (var file in allFiles.OfType<string>())
        {
          File.Delete(file);
        }
      }
      catch (Exception e)
      {
        ++failures;
        Console.WriteLine(e);
        Thread.Sleep(CleanupSleepMs);
      }
    }

    Log(
      "Async Notice: Cleanup of outdated files completed after " + failures + 
      " failures where max failures threshold is " + CleanupMaxFailures
    );
  }

  public static SemanticVersion GetVersionFromCurrentExecutable()
  {
    var version = Assembly.GetEntryAssembly()?.GetName().Version;
    return version == null ? new SemanticVersion() : new SemanticVersion(version.ToString());
  }

  private static SemanticVersion GetLatestVersionInfo()
  {
    using var client = new HttpClient();
    client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);

    try
    {
      var response = client.GetAsync(LatestVersionEndpoint).GetAwaiter().GetResult();
      response.EnsureSuccessStatusCode();

      using var stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
      using var json = JsonDocument.Parse(stream);


      if (json.RootElement.TryGetProperty("tag_name", out var tag) && tag.GetString() is { } tagString)
      {
        return new SemanticVersion(tagString);
      }
    }
    catch (Exception ex)
    {
      Log("Error fetching latest version info: " + ex.Message);
    }

    return new SemanticVersion();
  }

  private static void TryUpgrade()
  {
    if (PerformAllUpgradeSteps())
    {
      KillCurrentProcess();        
    }

    var messageUpgradeFailed =
      "Upgrade Failed, please try again later." +
      Environment.NewLine + Environment.NewLine +
      "The application will now restart.";

    MessageBox.Show(messageUpgradeFailed, AppName, MessageBoxButtons.OK);

    Application.Restart();
  }

  private static bool PerformAllUpgradeSteps()
  {
    Log();

    var steps = new List<Func<bool>>
    {
      FetchLatestRelease,
      UnzipLatestRelease,
      MarkCurrentExeForDeletion,
      CopyNewFiles,
      Cleanup,
      LaunchNewVersion,
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
        Log(ex.ToString());
        return false;
      }
    }

    Log("Upgrade Successful.");

    return true;
  }

  private static bool FetchLatestRelease()
  {
    Log();

    var client = new HttpClient();

    try
    {
      var response = client
        .GetAsync(DownloadUrl, HttpCompletionOption.ResponseHeadersRead)
        .GetAwaiter()
        .GetResult();
      response.EnsureSuccessStatusCode();

      using var stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
      using var fileStream = new FileStream(TempDownloadPath, FileMode.Create, FileAccess.Write, FileShare.None);
      stream.CopyTo(fileStream);

      Log("Downloaded latest release to " + TempDownloadPath);

      return true;
    }
    catch (Exception ex)
    {
      Log("Error downloading release: " + ex.Message);
      return false;
    }
  }

  private static bool UnzipLatestRelease()
  {
    Log();

    if (Directory.Exists(TempExtractPath))
    {
      Directory.Delete(TempExtractPath, true);
    }

    ZipFile.ExtractToDirectory(TempDownloadPath, TempExtractPath);

    Log("Unzipped to " + TempExtractPath);

    return true;
  }

  private static bool MarkCurrentExeForDeletion()
  {
    Log();

    var newName = DateTime.Now.ToString(DateTimeFormatPattern) + OutdatedMarker;

    File.Move(Application.ExecutablePath, newName);

    Log("Marked current executable for deletion: " + newName);

    return true;
  }

  private static bool CopyNewFiles()
  {
    Log();

    var files = Directory.GetFiles(TempExtractPath, "*", SearchOption.AllDirectories);
    foreach (var file in files)
    {
      var newFileName = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileName(file));
      File.Copy(file, newFileName, true);
    }

    Log("Copied new files to current directory.");

    return true;
  }

  private static bool Cleanup()
  {
    Log();

    if (File.Exists(TempDownloadPath))
    {
      File.Delete(TempDownloadPath);
    }

    if (Directory.Exists(TempExtractPath))
    {
      Directory.Delete(TempExtractPath, true);
    }

    Log("Cleaned up temporary files.");

    return true;
  }

  private static bool LaunchNewVersion()
  {
    Log();

    var exe = Application.ProductName + ExecutableExtension;

    var process = new Process();
    process.StartInfo.FileName = exe;
    process.StartInfo.RedirectStandardOutput = true;
    process.StartInfo.RedirectStandardError = true;
    process.StartInfo.UseShellExecute = false;
    process.StartInfo.CreateNoWindow = true;
    process.StartInfo.Arguments = "";
    process.Start();

    Log("Launched new version: " + exe);

    return true;
  }

  private static void KillCurrentProcess()
  {
    Log();

    var currentProcess = Process.GetCurrentProcess();

    if (currentProcess.Id == 0)
    {
      return;
    }

    try
    {
      currentProcess.Kill();
    }
    catch (ArgumentException)
    {
      Thread.Sleep(KillProcessRetrySleepMs);
      KillCurrentProcess();
    }

    Log("Current process killed: " + currentProcess.ProcessName + " (ID: " + currentProcess.Id + ")");
  }
}