using System;
using System.Collections.Generic;

namespace Util
{
	public static class Assert
	{
		public static void NotEmpty<T>(ICollection<T> items, string paramName)
		{
			if (items == null)
				throw new ArgumentNullException(paramName);
			if (items.Count == 0)
				throw new ArgumentException(string.Concat("Collection should not be empty: ", paramName));
		}

		public static void Guid(Guid guid, string paramName)
		{
			if (guid == System.Guid.Empty)
				throw new ArgumentException(string.Concat(paramName, " shold not be an empty guid"));
		}

		public static void Greater(int value, int target, string paramName)
		{
			if (value <= target)
				throw new ArgumentOutOfRangeException(string.Concat(paramName, " should be greater then ", target));
		}

		public static void Greater(long value, long target, string paramName)
		{
			if (value <= target)
				throw new ArgumentOutOfRangeException(string.Concat(paramName, " should be greater then ", target));
		}

		public static void GreaterOrEqual(int value, int target, string paramName)
		{
			if (value < target)
				throw new ArgumentOutOfRangeException(string.Concat(paramName, " should greater or equal to ", target));
		}

		public static void Less(int value, int target, string paramName)
		{
			if (value >= target)
				throw new ArgumentOutOfRangeException(string.Concat(paramName, " should be less then ", target));
		}

		public static void LessOrEqual(int value, int target, string paramName)
		{
			if (value > target)
				throw new ArgumentOutOfRangeException(string.Concat(paramName, " should be less or equal to ", target));
		}

		public static void Range(int value, int min, int max, string paramName)
		{
			if (value > max)
				throw new ArgumentOutOfRangeException(string.Concat(paramName, " should be less or equal to ", max));
			if (value < min)
				throw new ArgumentOutOfRangeException(string.Concat(paramName, " should greater or equal to ", min));
		}

		public static void NotNull(object param, string paramName)
		{
			if (param == null)
				throw new ArgumentNullException(paramName);

			var str = param as string;
			if (str != null && (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)))
				throw new ArgumentException(paramName);

			var bytes = param as byte[];
			if (bytes != null && bytes.Length == 0)
				throw new ArgumentException(paramName);
		}
	}
}
