using System.Collections.Generic;
using NUnit.Framework;
using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Tests
{
    public sealed class GameTagFilterTests
    {
        [TestCase(true, false, "first", false)]
        [TestCase(true, false, "unrelated", false)]
        [TestCase(false, false, "second", true)]
        [TestCase(false, false, "unrelated", false)]
        [TestCase(true, true, "first", true)]
        [TestCase(false, true, "second", false)]
        public void MultipleTagsCompareSelectedTagsWithTheOwner(bool all, bool inverse,
            string ownedTag, bool expected)
        {
            var filter = new GameTagFilter
            {
                isMultiple = true,
                gameTags = new[] { "first", "second" },
                isAll = all,
                inversed = inverse
            };

            Assert.That(filter.IsMatch(new TagOwner(ownedTag)), Is.EqualTo(expected));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void MultipleTagsMatchAnOwnerWithEverySelectedTag(bool all)
        {
            var filter = new GameTagFilter
            {
                isMultiple = true,
                gameTags = new[] { "first", "second" },
                isAll = all
            };

            Assert.That(filter.IsMatch(new TagOwner("first", "second")), Is.True);
            Assert.That(filter.IsMatch(new TagOwner()), Is.False);
        }

        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void EmptySelectionDoesNotFilter(bool multiple, bool inverse)
        {
            var filter = new GameTagFilter
            {
                isMultiple = multiple,
                gameTags = new string[0],
                inversed = inverse
            };

            Assert.That(filter.IsMatch(new TagOwner()), Is.True);
        }

        [TestCase(false, true, false)]
        [TestCase(true, false, true)]
        public void SingleTagPreservesInversion(bool inverse, bool matching, bool unrelated)
        {
            var filter = new GameTagFilter { gameTag = "first", inversed = inverse };

            Assert.That(filter.IsMatch(new TagOwner("first")), Is.EqualTo(matching));
            Assert.That(filter.IsMatch(new TagOwner("other")), Is.EqualTo(unrelated));
        }

        private sealed class TagOwner : IGameTagsOwner
        {
            public ICollection<string> GameTags { get; }

            public TagOwner(params string[] tags)
            {
                GameTags = new HashSet<string>(tags);
            }
        }
    }
}
