using System;
using System.Diagnostics;
using System.IO;
using Ionic.Zip;
using Microsoft.Build.Evaluation;

namespace BuildZipper
{
    public static class BuildZipper
    {
        static void Main(string[] args)
        {
            if (args.Length ==1)
            {
                addPostBuildEvent(args[0].Trim());
            }
            else if(args.Length ==3)
            {
                postBuild(args[0].Trim(), args[1].Trim(), args[2].Trim());
            }
            else
            {
                usage();
            }
        }

        private static void postBuild(string targetDir, string targetName, string solutionDir)
        {
            zip(targetDir, Path.Combine(solutionDir, targetName+ ".zip"),targetName);
        }

        private static void zip(string dir, string dest, string folderName)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(dir, folderName);
                zip.Save(dest);
            }
        }

        private static void addPostBuildEvent(string path)
        {
            Project project = new Project(path);
            project.SetProperty("PostBuildEvent",
                                string.Format(
                                    @"""{0}"" "" $(TargetDir) "" "" $(TargetName) "" "" $(SolutionDir) """,
                                    Process.GetCurrentProcess().MainModule.FileName));
            project.Save();
        }

        private static void usage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("BuildZipper ProjectPath");
            Console.WriteLine("Add BuildZipper to the post build event, it will archive the build result to the solution root folder.");
        }
    }
}
