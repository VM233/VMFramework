using System;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using VMFramework.Editor.GameEditor;
using Object = UnityEngine.Object;

namespace VMFramework.Tests
{
    public sealed class GameEditorTagNavigationTests
    {
        private GameEditor window;

        [SetUp]
        public void SetUp()
        {
            window = ScriptableObject.CreateInstance<GameEditor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(window);
        }

        [Test]
        public void BuildsTheMenuTreeBeforeTheNewWindowsFirstRepaint()
        {
            Assert.That(window.MenuTree, Is.Null);

            window.FilterByGameTag("selected");

            Assert.That(window.MenuTree, Is.Not.Null);
            AssertSingleTag("selected");
            Assert.That(window.MenuTree.Config.SearchTerm, Is.Empty);
        }

        [Test]
        public void ReplacesExistingTagsAndTextSearchWithTheClickedTag()
        {
            using (var state = new SerializedObject(window))
            {
                var tags = state.FindProperty("selectedGameTags");
                tags.arraySize = 2;
                tags.GetArrayElementAtIndex(0).stringValue = "old-first";
                tags.GetArrayElementAtIndex(1).stringValue = "old-second";
                state.FindProperty("matchAllGameTags").boolValue = false;
                state.ApplyModifiedPropertiesWithoutUndo();
            }

            window.ForceMenuTreeRebuild();
            window.MenuTree.Config.SearchTerm = "an unrelated text search";
            window.FilterByGameTag("selected");

            AssertSingleTag("selected");
            Assert.That(window.MenuTree.Config.SearchTerm, Is.Empty);

            window.MenuTree.Config.SearchTerm = "another unrelated search";
            window.FilterByGameTag("next");

            AssertSingleTag("next");
            Assert.That(window.MenuTree.Config.SearchTerm, Is.Empty);

            window.ForceMenuTreeRebuild();
            AssertSingleTag("next");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void RejectsAnEmptyTagWithoutChangingTheCurrentFilter(string gameTag)
        {
            window.FilterByGameTag("selected");

            Assert.Throws<ArgumentException>(() => window.FilterByGameTag(gameTag));
            AssertSingleTag("selected");
        }

        private void AssertSingleTag(string expected)
        {
            using var state = new SerializedObject(window);
            var tags = state.FindProperty("selectedGameTags");
            Assert.That(tags.arraySize, Is.EqualTo(1));
            Assert.That(tags.GetArrayElementAtIndex(0).stringValue, Is.EqualTo(expected));
            Assert.That(state.FindProperty("matchAllGameTags").boolValue, Is.True);
        }
    }
}
