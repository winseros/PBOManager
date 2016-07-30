using System;
using NUnit.Framework;
using AssertHelper = Util.Assert;

namespace Test.Util
{
	[TestFixture]
	public class AssertTest
	{
		[Test]
		public void Test_NotEmpty_Throws_If_Collection_Is_Null()
		{
			TestDelegate caller = () => AssertHelper.NotEmpty<string>(null, "parameter name1");
			var ex = Assert.Throws<ArgumentNullException>(caller);
			StringAssert.Contains("parameter name1", ex.Message);
		}

		[Test]
		public void Test_NotEmpty_Throws_If_Collection_Is_Empty()
		{
			TestDelegate caller = () => AssertHelper.NotEmpty(new string[0], "parameter name1");
			var ex = Assert.Throws<ArgumentException>(caller);
			StringAssert.Contains("parameter name1", ex.Message);
		}

		[Test]
		public void Test_NotEmpty_Passes_If_Collection_Is_Not_Empty()
		{
			TestDelegate caller = () => AssertHelper.NotEmpty(new string[1], "parameter name1");
			Assert.DoesNotThrow(caller);
		}


		[Test]
		public void Test_Guid_Throws_If_Input_Is_Empty_Guid()
		{
			TestDelegate caller = () => AssertHelper.Guid(Guid.Empty, "parameter name1");
			var ex = Assert.Throws<ArgumentException>(caller);
			StringAssert.Contains("parameter name1", ex.Message);

			caller = () => AssertHelper.Guid(new Guid(), "parameter name1");
			ex = Assert.Throws<ArgumentException>(caller);
			StringAssert.Contains("parameter name1", ex.Message);
		}

		[Test]
		public void Test_Guid_Passes_If_Input_Not_Is_Empty_Guid()
		{
			TestDelegate caller = () => AssertHelper.Guid(Guid.NewGuid(), "parameter name1");
			Assert.DoesNotThrow(caller);
		}


		[Test]
		public void Test_Greater_Throws_If_Value_Is_LessOrEqual_To_Target()
		{
			TestDelegate caller = () => AssertHelper.Greater(10, 15, "param name 1");
			var ex = Assert.Throws<ArgumentOutOfRangeException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
			StringAssert.Contains("15", ex.Message);

			caller = () => AssertHelper.Greater(10, 10, "param name 1");
			ex = Assert.Throws<ArgumentOutOfRangeException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
			StringAssert.Contains("10", ex.Message);
		}


		[Test]
		public void Test_GreaterLong_Passes_If_Value_Is_Greater_Than_Target()
		{
			TestDelegate caller = () => AssertHelper.Greater(20L, 15L, "param name 1");
			Assert.DoesNotThrow(caller);
		}

		[Test]
		public void Test_GreaterLong_Throws_If_Value_Is_LessOrEqual_To_Target()
		{
			TestDelegate caller = () => AssertHelper.Greater(10L, 15L, "param name 1");
			var ex = Assert.Throws<ArgumentOutOfRangeException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
			StringAssert.Contains("15", ex.Message);

			caller = () => AssertHelper.Greater(10L, 10L, "param name 1");
			ex = Assert.Throws<ArgumentOutOfRangeException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
			StringAssert.Contains("10", ex.Message);
		}

		[Test]
		public void Test_Greater_Passes_If_Value_Is_Greater_Than_Target()
		{
			TestDelegate caller = () => AssertHelper.Greater(20, 15, "param name 1");
			Assert.DoesNotThrow(caller);
		}


		[Test]
		public void Test_GreaterOrEqual_Throws_If_Value_Is_Less_Than_Target()
		{
			TestDelegate caller = () => AssertHelper.GreaterOrEqual(10, 15, "param name 1");
			var ex = Assert.Throws<ArgumentOutOfRangeException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
			StringAssert.Contains("15", ex.Message);
		}

		[Test]
		public void Test_GreaterOrEqual_Passes_If_Value_Is_GreaterOrEqual_To_Target()
		{
			TestDelegate caller = () => AssertHelper.GreaterOrEqual(20, 15, "param name 1");
			Assert.DoesNotThrow(caller);

			caller = () => AssertHelper.GreaterOrEqual(15, 15, "param name 1");
			Assert.DoesNotThrow(caller);
		}


		[Test]
		public void Test_Less_Throws_If_Value_Is_GreaterOrEqual_To_Target()
		{
			TestDelegate caller = () => AssertHelper.Less(15, 10, "param name 1");
			var ex = Assert.Throws<ArgumentOutOfRangeException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
			StringAssert.Contains("10", ex.Message);

			caller = () => AssertHelper.Greater(10, 10, "param name 1");
			ex = Assert.Throws<ArgumentOutOfRangeException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
			StringAssert.Contains("10", ex.Message);
		}

		[Test]
		public void Test_Less_Passes_If_Value_Is_Less_Than_Target()
		{
			TestDelegate caller = () => AssertHelper.Less(15, 20, "param name 1");
			Assert.DoesNotThrow(caller);
		}


		[Test]
		public void TestLessOrEqualThrowsIfValueIsGreaterThenTarget()
		{
			TestDelegate caller = () => AssertHelper.LessOrEqual(15, 10, "param name 1");
			var ex = Assert.Throws<ArgumentOutOfRangeException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
			StringAssert.Contains("10", ex.Message);
		}

		[Test]
		public void Test_LessOrEqual_Passes_If_Value_Is_LessOrEqual_To_Target()
		{
			TestDelegate caller = () => AssertHelper.LessOrEqual(15, 20, "param name 1");
			Assert.DoesNotThrow(caller);

			caller = () => AssertHelper.LessOrEqual(15, 15, "param name 1");
			Assert.DoesNotThrow(caller);
		}


		[Test]
		public void Test_Range_Throws_If_Value_Is_Out_Of_Range()
		{
			TestDelegate caller = () => AssertHelper.Range(15, 5, 10, "param name 1");
			var ex = Assert.Throws<ArgumentOutOfRangeException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
			StringAssert.Contains("less or equal to", ex.Message);
			StringAssert.Contains("10", ex.Message);

			caller = () => AssertHelper.Range(0, 5, 10, "param name 1");
			ex = Assert.Throws<ArgumentOutOfRangeException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
			StringAssert.Contains("greater or equal to", ex.Message);
			StringAssert.Contains("5", ex.Message);
		}

		[Test]
		public void Test_Range_Throws_If_Value_Is_In_Range()
		{
			TestDelegate caller = () => AssertHelper.Range(5, 5, 10, "param name 1");
			Assert.DoesNotThrow(caller);

			caller = () => AssertHelper.Range(10, 5, 10, "param name 1");
			Assert.DoesNotThrow(caller);

			caller = () => AssertHelper.Range(8, 5, 10, "param name 1");
			Assert.DoesNotThrow(caller);
		}


		[Test]
		public void Test_NotNullOrEmpty_Throws_If_Value_Null()
		{
			TestDelegate caller = () => AssertHelper.NotNull(null, "param name 1");
			var ex = Assert.Throws<ArgumentNullException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
		}

		[Test]
		public void Test_NotNullOrEmpty_Throws_If_Value_Is_EmptyString()
		{
			TestDelegate caller = () => AssertHelper.NotNull(string.Empty, "param name 1");
			var ex = Assert.Throws<ArgumentException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
		}

		[Test]
		public void Test_NotNullOrEmpty_Throws_If_Value_Is_EmptyByteArray()
		{
			TestDelegate caller = () => AssertHelper.NotNull(new byte[0], "param name 1");
			var ex = Assert.Throws<ArgumentException>(caller);
			StringAssert.Contains("param name 1", ex.Message);
		}

		[Test]
		public void Test_NotNullOrEmpty_Passes_If_Value_Is_Not_Null()
		{
			TestDelegate caller = () => AssertHelper.NotNull(new object(), "param name 1");
			Assert.DoesNotThrow(caller);
		}
	}
}