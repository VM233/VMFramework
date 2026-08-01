using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VMFramework.Procedure;

namespace VMFramework.Tests
{
    public sealed class ManagerCreatorContainersTests
    {
        private const string TEST_SCENE_PATH =
            "Packages/com.vm233.vmframework/Tests/Editor/Fixtures/EmptyScene.unity";

        private Scene originalScene;
        private Scene testScene;

        [SetUp]
        public void SetUp()
        {
            originalScene = SceneManager.GetActiveScene();
            testScene = EditorSceneManager.OpenScene(
                TEST_SCENE_PATH,
                OpenSceneMode.Additive);

            Assert.That(SceneManager.SetActiveScene(testScene), Is.True);
        }

        [TearDown]
        public void TearDown()
        {
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
