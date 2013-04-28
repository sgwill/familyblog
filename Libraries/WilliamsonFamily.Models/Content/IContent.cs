using System;
using System.ComponentModel.DataAnnotations;

namespace WilliamsonFamily.Models.Content
{
	public interface IContent
	{
		[Key]
		int ContentID { get; set; }
		string Token { get; set; }
		string Value { get; set; }

	}
}