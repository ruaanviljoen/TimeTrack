using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTrack;
using System.IO;
using System.Collections.Generic;

namespace TestTimeTrack
{
    [TestClass]
    public class TestDatabase
    {
        [TestInitialize]
        public void Setup()
        {
            if (File.Exists(Database.DefaultDatabaseFileName))
            {
                File.Delete(Database.DefaultDatabaseFileName);
            }
        }

        [TestCleanup]
        public void Teardown()
        {
            if (File.Exists(Database.DefaultDatabaseFileName))
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                File.Delete(Database.DefaultDatabaseFileName);
            }
        }

        [TestMethod]
        public void TestCreateDatabaseFileOnFirstUse()
        {
            Assert.IsFalse(File.Exists(Database.DefaultDatabaseFileName));
            Database db = new Database();

            Assert.IsTrue(File.Exists(Database.DefaultDatabaseFileName));
        }

        [TestMethod]
        public void TestInsert()
        {
            Database db = new Database();
            TimeLog t = new TimeLog("abc", DateTime.Now);
            db.InsertTimeLog(t);

            Assert.AreEqual(t.Id, 1);
            
            //TODO This check is quite primitive, improve this maybe by a full check on all values inserted in the database
        }

        [TestMethod]
        public void TestUpdate()
        {
            Database db = new Database();
            DateTime now = DateTime.Now;
            TimeLog t = new TimeLog("abc", now);

            //insert, update, retrieve, check the updated value
            db.InsertTimeLog(t);          

            //update the endtime (close the timelog entry)
            DateTime endtime = now.AddHours(1);
            t.EndTime = endtime;
            db.UpdateTimeLog(t);

            List<TimeLog> logs = db.ReadTimeLogs(new List<long> { t.Id });
            TimeLog t_retrieved = logs[0];

            Assert.AreEqual(t_retrieved.Id, t.Id);
            //TODO use better way of comparing datetime values, Ticks actually differ even though formatted datetime is the same
            Assert.AreEqual(endtime.ToString(Database._sqLiteDateTimeFormatString), t_retrieved.EndTime?.ToString(Database._sqLiteDateTimeFormatString));
        }

        [TestMethod]
        public void TestRead_ByDate()
        {
            Database db = new Database();
            DateTime now = DateTime.Now;
            TimeLog t = new TimeLog("abc", now);
            db.InsertTimeLog(t);
            List<TimeLog> logs = db.ReadTimeLogs(DateTime.Today);
            TimeLog t_retrieved = logs[0];

            Assert.AreEqual(1, logs.Count);
            Assert.AreEqual(t, t_retrieved);
            Assert.AreEqual("abc", t_retrieved.TimeCode);
            //TODO use better way of doing this, Ticks actually differ even though formatted datetime is the same
            Assert.AreEqual(now.ToString(Database._sqLiteDateTimeFormatString), t_retrieved.StartTime.ToString(Database._sqLiteDateTimeFormatString));
            
        }

        [TestMethod]
        public void TestRead_ById()
        {
            Database db = new Database();
            DateTime now = DateTime.Now;
            TimeLog t = new TimeLog("abc", now);
            db.InsertTimeLog(t);

            List<TimeLog> logs = db.ReadTimeLogs(DateTime.Today);
            Assert.AreEqual(1, logs.Count);
            TimeLog t_retrieved = logs[0];

            Assert.AreEqual(t, t_retrieved);
            Assert.AreEqual("abc", t_retrieved.TimeCode);
            //TODO use better way of comparing datetime values, Ticks actually differ even though formatted datetime is the same
            Assert.AreEqual(now.ToString(Database._sqLiteDateTimeFormatString), t_retrieved.StartTime.ToString(Database._sqLiteDateTimeFormatString));

        }
    }
}
