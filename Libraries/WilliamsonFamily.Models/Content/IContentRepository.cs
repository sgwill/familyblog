using System;

namespace WilliamsonFamily.Models.Content
{
	public interface IContentRepository
	{
		IContent New();
		IContent Lookup(string token);
		void Set(IContent content);
	}
}