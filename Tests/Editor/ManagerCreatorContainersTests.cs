using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VMFramework.Procedure;

namespace VMFramework.Tests
{
    public sealed class ManagerCreatorContainersTests
    {
        private Scene originalScene;
        private Scene testScene;
        private bool usesRunnerScene;
        private HashSet<GameObject> runnerSceneRoots;

        [SetUp]
        public void SetUp()
        {
            originalScene = SceneManager.GetActiveScene();

            if (originalScene.IsValid() && originalScene.isLoaded &&
                string.IsNullOrEmpty(originalScene.path))
            {
                var roots = originalScene.GetRootGameObjects();
                if (roots.Length > 0)
                {
                    Assert.Ignore(
                        "The active untitled scene contains unsaved user content and cannot be used for isolation.");
                }

                testScene = originalScene;
                usesRunnerScene = true;
                runnerSceneRoots = new HashSet<GameObject>(roots);
                return;
            }

            testScene = EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene,
                NewSceneMode.Additive);
            SceneManager.SetActiveScene(testScene);
        }

        [TearDown]
        public void TearDown()
        {
            if (usesRunnerScene && testScene.IsValid() && testScene.isLoaded)
            {
                foreach (var root in testScene.GetRootGameObjects())
                {
                    if (runnerSceneRoots.Contains(root) == false)
                    {
                        Object.DestroyImmediate(root);
                    }
                }

                return;
            }

            if (originalScene.IsValid() && originalScene.isLoaded)
            {
                SceneManager.SetActiveScene(originalScene);
            }

            if (testScene.IsValid() && testScene.isLoaded)
            {
                EditorSceneManager.CloseScene(testScene, true);
            }
        }

        [Test]
        public void InitIgnoresNestedObjectsWithReservedContainerNames()
        {
            var businessOwner = new GameObject("Business Owner");
            var nestedCore = new GameObject(ManagerCreatorContainers.CONTAINER_NAME);
            nestedCore.transform.SetParent(businessOwner.transform, false);
            var nestedAudio = new GameObject("Audio");
            nestedAudio.transform.SetParent(businessOwner.transform, false);

            ManagerCreatorContainers.Init();
            var audioContainer =
                ManagerCreatorContainers.GetOrCreateManagerTypeContainer("Audio");

            Assert.That(ManagerCreatorContainers.ManagerContainer.parent, Is.Null);
            Assert.That(
                ManagerCreatorContainers.ManagerContainer.gameObject.scene,
                Is.EqualTo(testScene));
            Assert.That(
                ManagerCreatorContainers.ManagerContainer,
                Is.Not.SameAs(nestedCore.transform));
            Assert.That(
                audioContainer.parent,
                Is.SameAs(ManagerCreatorContainers.ManagerContainer));
            Assert.That(audioContainer, Is.Not.SameAs(nestedAudio.transform));
            Assert.That(nestedCore.transform.parent, Is.SameAs(businessOwner.transform));
            Assert.That(nestedAudio.transform.parent, Is.SameAs(businessOwner.transform));
        }
    }
}
