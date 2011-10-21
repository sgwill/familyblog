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
	public class ElmahLogCleanerTests
	{
		ElmahLogCleaner cleaner = null;

		[TestMethod]
		public void EmptyConnectionString_ThrowsException()
		{
			// Arrange
			cleaner.ConnectionString = "";

			// Assert
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
		public void LessThan30DaysToKeep_ThrowsException()
		{
			// Arrange
			cleaner.ConnectionString = "string";

			// Assert
			try
			{
				cleaner.RemoveOldLogs(10);
			}
			catch (Exception ex)
			{
				Assert.AreEqual("Must specify more than 30 days to delete", ex.Message);
			}			
		}

		[TestInitialize]
		public void Init()
		{
			cleaner = new ElmahLogCleaner();
		}
	}
}