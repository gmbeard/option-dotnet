using Xunit;
using System.Linq;
using System.Collections;

namespace Gmb.Tests
{
	public class OptionShould
	{
		[Fact]
		public void NotThrowOnUnwrapWhenValueIsSet()
		{
			var o = Some.Value(42);
			Assert.Equal(42, o.Unwrap());
		}

		[Fact]
		public void ThrowOnUnwrapWhenEmpty()
		{
			var o = None.Value<int>();
			Assert.Throws<OptionIsEmptyException>(() => o.Unwrap());
		}

		[Fact]
		public void MapToAnotherType()
		{
			var o = Some.Value(42).Map( v => $"{v}");
			Assert.Equal("42", o.Unwrap());
		}

		[Fact]
		public void ReportIsNoneTrueWhenEmpty()
		{
			Assert.True(None.Value<int>().IsNone());
		}

		[Fact]
		public void ReportIsSomeTrueWhenEmpty()
		{
			Assert.True(Some.Value(42).IsSome());
		}

		[Fact]
		public void ProvideDefaultWithUnwrapWhenEmpty()
		{
			Assert.Equal(41, None.Value<int>().UnwrapOrDefault(41));
		}

		[Fact]
		public void ProvideDefaultLambdaWithUnwrapWhenEmpty()
		{
			Assert.Equal(41, None.Value<int>().UnwrapOr(() => 41));
		}

		[Fact]
		public void ConvertToEnumerable()
		{
			var values = Some.Value(42)
							 .ToEnumerable()
							 .Concat(Some.Value(43).ToEnumerable())
							 .ToArray();
			Assert.Equal(new int[] { 42, 43 }, values);

			values = None.Value<int>()
						 .ToEnumerable()
						 .Concat(None.Value<int>().ToEnumerable())
						 .ToArray();
			Assert.Equal(new int[] {}, values);
		}

		[Fact]
		public void ShouldProceedWithAndThenWhenNotEmpty()
		{
			Assert.Equal(1, Some.Value(0).AndThen(i => i + 1).Unwrap());
		}

		[Fact]
		public void ShouldNotProceedWithAndThenWhenEmpty()
		{
			Assert.Throws<OptionIsEmptyException>(() => None.Value<int>().AndThen(i => i + 1).Unwrap());
		}
	}
}
