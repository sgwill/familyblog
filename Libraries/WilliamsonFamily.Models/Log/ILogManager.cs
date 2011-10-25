using System;

namespace WilliamsonFamily.Models.Log
{
	public interface ILogManager
	{
		int LogsCount();
		void RemoveOldLogs(int daysToKeep);
		void Compact();
	}
}