namespace SpeedTest_DiscordBot
{
    public partial class Program
    {
        public Dictionary<string, string> GetInternetInfo()
        {
            Process process = new();
            Dictionary<string, string> result = new();
            try
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.FileName = "speedtest.exe";
                process.StartInfo.Arguments = "--accept-license";
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                List<string> list_output = output.Split('\n').ToList();

                result["Server"] = list_output[3].Split(':')[1].Trim();
                result["ISP"] = list_output[4].Split(':')[1].Trim();
                result["Latency"] = list_output[5].Split(':')[1].Trim();
                result["Download"] = list_output[6].Replace("Download:", "").Trim();
                result["Upload"] = list_output[8].Replace("Upload:", "").Trim();
                result["Packet Loss"] = list_output[10].Replace("Packet Loss:", "").Trim();
                result["Result URL"] = list_output[11].Replace("Result URL:", "").Trim();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (process != null)
                {
                    process.Dispose();
                }
            }
            return result;
        }
    }
}
