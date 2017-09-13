using System;
using Xunit;

namespace Gmb.Tests
{
    public class ResultShould
    {
        [Fact]
        public void BeOkOnNonError()
        {
            var r = Result.For<int, InvalidOperationException>( () => 42 );

            Assert.True(r.IsOk());
        }

        [Fact]
        public void ThrowWhenUnwrappingAnErr()
        {
            var r = Result.For<int, InvalidOperationException>( () => throw new InvalidOperationException() );
            Assert.Throws<InvalidOperationException>( () => r.Unwrap() );
        }
    }
}
