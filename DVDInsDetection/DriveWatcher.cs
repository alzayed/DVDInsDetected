using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace DVDInsDetection {
    class DriveWatcher {
        public delegate void DiskInsertEventHandler(Object sender, DiskInsertEventArgs e);
        public event DiskInsertEventHandler DiskInserted;
        Timer timer;
        Dictionary<string, bool> drives;
        bool haveDisk;
        public void Start() {
            drives = new Dictionary<string, bool>();
            foreach(DriveInfo drive in DriveInfo.GetDrives().Where(x => x.DriveType.Equals(DriveType.CDRom))) {
                drives.Add(drive.Name, drive.IsReady);
            }
            timer = new Timer() { Interval = 1000 };
            timer.Elapsed += DriveTimerOnElapsed;
            timer.Start();
        }
        private void DriveTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs) {
            if(!haveDisk) {
                try {
                    haveDisk = true;
                    foreach(DriveInfo drive in from drive in DriveInfo.GetDrives()
                                               where drives.ContainsKey(drive.Name)
                                               where !drives[drive.Name].Equals(drive.IsReady)
                                               select drive) {
                        drives[drive.Name] = drive.IsReady;
                        OnDiskInserted(new DiskInsertEventArgs { Drive = drive });
                    }
                } catch(Exception ex) {
                    Console.WriteLine(ex.Message);
                } finally {
                    haveDisk = false;
                }
            }
        }
        private void OnDiskInserted(DiskInsertEventArgs e)
            => DiskInserted?.Invoke(this, e);

    }
}


