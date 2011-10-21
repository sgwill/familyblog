using System;

namespace WilliamsonFamily.Models.Log
{
	public interface ILogCleaner
	{
		void RemoveOldLogs(int daysToKeep);
	}
}