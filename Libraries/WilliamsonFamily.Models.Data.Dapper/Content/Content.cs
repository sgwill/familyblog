using System;
using System.ComponentModel.DataAnnotations;
using WilliamsonFamily.Models.Content;

namespace WilliamsonFamily.Models.Data.Dapper.Content
{
	public class Content : IContent
	{
		[Key]
		public int ContentID { get; set; }
		public string Token { get; set; }
		public string Value { get; set; }
	}
}