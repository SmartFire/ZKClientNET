﻿using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKClientNET.Client;
using ZKClientNETTest.Util;
using ZooKeeperNet;

namespace ZKClientNETTest.Test
{
   public  class ZKAuthTest
    {
        protected ZKClient _zkClient;
        protected static readonly ILog LOG = LogManager.GetLogger(typeof(ZKAuthTest));

        [OneTimeSetUp]
        public  void SetUp()
        {
            LOG.Info("------------ BEFORE -------------");
            _zkClient = new ZKClient(string.Format("{0}:{1}", TestUtil.ip, TestUtil.port), new TimeSpan(0, 0, 0, 0, 5000));
           
        }

        [OneTimeTearDown]
        public  void TearDown()
        {
            LOG.Info("------------ AFTER -------------");
            _zkClient.Close();
        }

        [Test]
        public void TestAuthorized()
        {
            _zkClient.AddAuthInfo("digest", Encoding.Default.GetBytes("pat:pass") );
            TestUtil.ReSetPathUnCreate(_zkClient, "/path1");
            _zkClient.Create("/path1", null, Ids.CREATOR_ALL_ACL, CreateMode.Persistent);
            _zkClient.ReadData<object>("/path1");
        }

        [Test]
        public void TestSetAndGetAcls()
        {
            _zkClient.AddAuthInfo("digest", Encoding.Default.GetBytes("pat:pass"));
            TestUtil.ReSetPathUnCreate(_zkClient, "/path1");
            _zkClient.Create("/path1", null, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            Assert.True(_zkClient.GetAcl("/path1").Key.Count == Ids.OPEN_ACL_UNSAFE.Count);

            for (int i = 0; i < 100; i++)
            {
                _zkClient.SetAcl("/path1", Ids.OPEN_ACL_UNSAFE);
                Assert.True(_zkClient.GetAcl("/path1").Key.Count == Ids.OPEN_ACL_UNSAFE.Count);
            }
        }


    }
}
