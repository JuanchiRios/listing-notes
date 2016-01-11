using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ejercicio.Utils
{
	public static class StringHelper
	{
		/// <summary>
		/// Convert a string from PascalCase to snake_case
		/// </summary>
		/// <param name="input">Value to convert.</param>
		/// <returns>A new string converted to snake_case.</returns>
		/// <remarks> http://stackoverflow.com/questions/18781027/regex-camel-case-to-underscore-ignore-first-occurrence </remarks>
		public static string ToSnakeCase(this string input)
		{
			return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
		}

	}
}