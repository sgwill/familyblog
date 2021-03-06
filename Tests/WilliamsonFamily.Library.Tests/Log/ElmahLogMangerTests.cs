﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamsonFamily.Models.Log;
using WilliamsonFamily.Library.Log;

namespace WilliamsonFamily.Library.Tests.Log
{
	[TestClass]
	public class ElmahLogMangerTests
	{
		ElmahLogManager cleaner = null;

		[TestMethod]
		public void RemoveOldLogs_EmptyConnectionString_ThrowsException()
		{
			cleaner.ConnectionString = "";

			try
			{
				cleaner.RemoveOldLogs(30);
			}
			catch (Exception ex)
			{
				Assert.AreEqual("ConnectionString required", ex.Message);
			}
		}

		[TestMethod]
		public void RemoveOldLogs_LessThan30DaysToKeep_ThrowsException()
		{
			cleaner.ConnectionString = "string";

			try
			{
				cleaner.RemoveOldLogs(10);
			}
			catch (Exception ex)
			{
				Assert.AreEqual("Must specify more than 30 days to delete", ex.Message);
			}			
		}

		[TestMethod]
		public void LogCount_EmptyConnectionString_ThrowsException()
		{
			cleaner.ConnectionString = "";

			try
			{
				cleaner.LogsCount();
			}
			catch (Exception ex)
			{
				Assert.AreEqual("ConnectionString required", ex.Message);
			}
		}

		[TestMethod]
		public void Compact_EmptyConnectionString_ThrowsException()
		{
			cleaner.ConnectionString = "";

			try
			{
				cleaner.Compact();
			}
			catch (Exception ex)
			{
				Assert.AreEqual("ConnectionString required", ex.Message);
			}
		}

		[TestInitialize]
		public void Init()
		{
			cleaner = new ElmahLogManager();
		}
	}
}