using System;
using System.IO;

namespace DVDInsDetection {
    class Program {
        static void Main(string[] args) {
            var driveWatcher = new DriveWatcher();
            driveWatcher.DiskInserted += DriveWatcherOnDiskInserted;
            driveWatcher.Start();

            static void DriveWatcherOnDiskInserted(object sender, DiskInsertEventArgs e) {
                string drive = e.Drive.Name;
                Console.WriteLine(drive);
                foreach(string file in Directory.EnumerateFiles(drive, "*.*")) {
                    Console.WriteLine(file);
                }
            }
            Console.WriteLine("I am listening now ...!");
            Console.ReadLine();
        }
    }
}
