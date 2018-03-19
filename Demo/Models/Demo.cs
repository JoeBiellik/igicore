using System;
using System.ComponentModel.DataAnnotations;
using IgiCore.SDK.Models;

namespace IgiCore.Plugins.Demo.Models
{ 
	public class Demo : IDatabaseModel
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string Something { get; set; }
	}
}
