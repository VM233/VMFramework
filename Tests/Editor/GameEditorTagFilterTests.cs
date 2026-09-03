using NUnit.Framework;
using UnityEngine;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Tests
{
    public sealed class GameEditorTagFilterTests
    {
        [Test]
        public void KeepsMatchingNodesAndAncestorsWithoutTheirUnmatchedSiblings()
        {
            var root = new TreeNodePreBuildInfo { node = new object() };
            var branch = new TreeNodePreBuildInfo { node = new object(), parentInfo = root };
            var matching = new TreeNodePreBuildInfo { node = new Config("selected"), parentInfo = branch };
            var sibling = new TreeNodePreBuildInfo { node = new Config("other"), parentInfo = branch };
            var otherBranch = new TreeNodePreBuildInfo { node = new object(), parentInfo = root };
            var nodes = new[] { matching, sibling, otherBranch, branch, root };

            var visible = GameEditorTagFilter.SelectVisibleNodes(nodes, new[] { "selected" }, true);

            Assert.That(visible, Is.EquivalentTo(new[] { root, branch, matching }));
            Assert.That(GameEditorTagFilter.SelectVisibleNodes(nodes, new[] { "absent" }, true), Is.Empty);
            Assert.That(GameEditorTagFilter.SelectVisibleNodes(nodes, new string[0], true),
                Is.EquivalentTo(nodes));
        }

        [Test]
        public void ReadsTheSingleWrappersConfigInsteadOfItsDisplayObjectOrName()
        {
            var wrapper = ScriptableObject.CreateInstance<GamePrefabSingleWrapper>();
            try
            {
                wrapper.name = "Unrelated Display Name";
                wrapper.InitGamePrefabs(new[] { new Config("selected") });
                var node = new TreeNodePreBuildInfo { node = wrapper };

                Assert.That(GameEditorTagFilter.SelectVisibleNodes(new[] { node }, new[] { "selected" }, true),
                    Is.EquivalentTo(new[] { node }));
                Assert.That(GameEditorTagFilter.SelectVisibleNodes(new[] { node },
                    new[] { wrapper.name }, true), Is.Empty);
            }
            finally
            {
                Object.DestroyImmediate(wrapper);
            }
        }

        [Test]
        public void AllTagsMustBelongToOneConfigInAMultipleWrapper()
        {
            var wrapper = ScriptableObject.CreateInstance<GamePrefabMultipleWrapper>();
            try
            {
                wrapper.InitGamePrefabs(new[] { new Config("first"), new Config("second") });
                var node = new TreeNodePreBuildInfo { node = wrapper };
                var nodes = new[] { node };
                var tags = new[] { "first", "second" };

                Assert.That(GameEditorTagFilter.SelectVisibleNodes(nodes, tags, true), Is.Empty);
                Assert.That(GameEditorTagFilter.SelectVisibleNodes(nodes, tags, false), Is.EquivalentTo(nodes));

                wrapper.InitGamePrefabs(new[] { new Config("first", "second") });
                Assert.That(GameEditorTagFilter.SelectVisibleNodes(nodes, tags, true), Is.EquivalentTo(nodes));
            }
            finally
            {
                Object.DestroyImmediate(wrapper);
            }
        }

        [Test]
        public void EmptyWrapperHasNoTagMatch()
        {
            var wrapper = ScriptableObject.CreateInstance<GamePrefabSingleWrapper>();
            try
            {
                var node = new TreeNodePreBuildInfo { node = wrapper };
                Assert.That(GameEditorTagFilter.SelectVisibleNodes(new[] { node }, new[] { "selected" }, true),
                    Is.Empty);
            }
            finally
            {
                Object.DestroyImmediate(wrapper);
            }
        }

        private sealed class Config : GamePrefab
        {
            public Config(params string[] tags)
            {
                gameTags.UnionWith(tags);
            }
        }
    }
}
