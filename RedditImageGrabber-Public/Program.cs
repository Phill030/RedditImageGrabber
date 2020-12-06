using Octokit;
using RedditSharp;
using RedditSharp.Things;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace RedditImageGrabber_Public
{
    class Program // Fuck you, dont steal my code >:C - Phill
    {
        static void Main(string[] args)
        {
            Console.Title = "RedditImageGrabber v1.0.0 - Made by Phill";
            sP1();
            cfU();
            rig();
        }
        private static async void sP1()
        {
            Console.WriteLine(" ██▀███   ██▓  ▄████ ", Console.ForegroundColor = ConsoleColor.Red);
            Console.WriteLine("▓██ ▒ ██▒▓██▒ ██▒ ▀█▒");
            Console.WriteLine("▓██ ░▄█ ▒▒██▒▒██░▄▄▄░");
            Console.WriteLine("▒██▀▀█▄  ░██░░▓█  ██▓");
            Console.WriteLine("░██▓ ▒██▒░██░░▒▓███▀▒");
            Console.WriteLine("░ ▒▓ ░▒▓░░▓   ░▒   ▒ ");
            Console.WriteLine("  ░▒ ░ ▒░ ▒ ░  ░   ░ ");
            Console.WriteLine("  ░░   ░  ▒ ░░ ░   ░ ");
            Console.WriteLine(" ");
            Console.WriteLine("   ░      ░        ░ ");
            Console.WriteLine("                     ");
        }

        private static async void cfU()
        {
            Console.WriteLine("Connecting to GitHub to search for the latest version!", Console.ForegroundColor = ConsoleColor.Yellow);
            GitHubClient client = new GitHubClient(new ProductHeaderValue("RedditImageGrabber"));
            var releases = await client.Repository.Release.GetAll("Phill030", "RedditImageGrabber");
            Version latestGitHubVersion = new Version(releases[0].TagName);
            Version localVersion = new Version("1.0.0");
            int versionComparison = localVersion.CompareTo(latestGitHubVersion);
            if (versionComparison < 0)
            {
                Console.WriteLine("There is a newer version on GitHub!", Console.ForegroundColor = ConsoleColor.Red);
                Console.ForegroundColor = ConsoleColor.White;
                System.Diagnostics.Process.Start("https://github.com/Phill030/RedditImageGrabber");
            }
            else
            {
                Console.WriteLine("Version is up to date!", Console.ForegroundColor = ConsoleColor.Green);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static void rig()
        {
            string sub = "/r/";
            Console.WriteLine("Subreddit: ", Console.ForegroundColor = ConsoleColor.White);
            sub += Console.ReadLine();
            string saveDir = @Environment.SpecialFolder.Desktop + @"\RIG\" + sub.Replace("/r/", "") + "\\";
            if (!Directory.Exists(@Environment.SpecialFolder.Desktop + @"\RIG\" + sub.Replace("/r/", "") + "\\"))
            {
                Directory.CreateDirectory(@Environment.SpecialFolder.Desktop + @"\RIG\" + sub.Replace("/r/", ""));
            }

            Console.WriteLine("Amount:");
            int amount = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Time Period (all,hot): ");
            string timePer = Console.ReadLine();

            Reddit reddit = new Reddit();
            var subreddit = reddit.GetSubreddit(sub);

            if (timePer.ToLower() == "all")
            {
                for (int i = 0; i < amount; i++)
                {
                    foreach (var post in subreddit.GetTop(FromTime.All).Take(amount))
                    {
                        if (post.IsStickied || post.IsSelfPost || Convert.ToString(post.Url).Contains("reddituploads")) continue;
                        string postURL = Convert.ToString(post.Url);
                        DownloadImages(postURL, saveDir);
                    }
                }
            }
            else
            {
                if (timePer.ToLower() == "hot")
                {
                    for (int i = 0; i < amount; i++)
                    {
                        foreach (var post in subreddit.Hot.Take(amount))
                        {
                            if (post.IsStickied || post.IsSelfPost || Convert.ToString(post.Url).Contains("reddituploads")) continue;
                            string postURL = Convert.ToString(post.Url);
                            DownloadImages(postURL, saveDir);
                        }
                    }
                }
            }
        }


        public static void DownloadImages(string imageURL, string userDir)
        {

            if (imageURL.Contains("gfycat.com"))
            {
                imageURL = imageURL.Replace("gfycat.com", "zippy.gfycat.com") + ".mp4";
            }

            if (imageURL.Contains(".gifv"))
            {
                imageURL = imageURL.Replace(".gifv", ".mp4");
            }

            Console.WriteLine("Downloading {0}", imageURL, Console.ForegroundColor = ConsoleColor.White);
            string[] splitURL = imageURL.Split('/');
            int index = splitURL.Length - 1;
            string fileName = splitURL[index];
            WebClient client = new WebClient();
            try
            {
                client.DownloadFile(imageURL, userDir + fileName);
                Console.WriteLine("[✓] File saved!", Console.ForegroundColor = ConsoleColor.Green);
            }
            catch (Exception)
            {
                Console.WriteLine("[✘] Error Downloading File", Console.ForegroundColor = ConsoleColor.Red);
            }
        }


    }
}
