using System;
using NUnit.Framework;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Tests
{
    public class StateCloneContextTests
    {
        private static readonly StateCloneTag firstTag = StateCloneTag.Create();
        private static readonly StateCloneTag secondTag = StateCloneTag.Create();

        [Test]
        public void SpanConstructorIncludesEveryTag()
        {
            Span<StateCloneTag> tags = stackalloc[]
            {
                firstTag,
                secondTag
            };

            var context = new StateCloneContext(tags);

            Assert.That(context.HasTag(firstTag), Is.True);
            Assert.That(context.HasTag(secondTag), Is.True);
        }

        [Test]
        public void WithTagDoesNotModifyOriginalContext()
        {
            var context = StateCloneContext.Empty;
            var contextWithTag = context.WithTag(firstTag);

            Assert.That(context.HasTag(firstTag), Is.False);
            Assert.That(contextWithTag.HasTag(firstTag), Is.True);
        }

        [Test]
        public void DefaultTagIsRejected()
        {
            var context = StateCloneContext.Empty;

            Assert.Throws<ArgumentException>(() => context.HasTag(default));
            Assert.Throws<ArgumentException>(() => context.WithTag(default));
        }
    }
}
