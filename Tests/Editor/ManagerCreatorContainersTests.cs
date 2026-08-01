using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VMFramework.Procedure;

namespace VMFramework.Tests
{
    public sealed class ManagerCreatorContainersTests
    {
        private Scene testScene;
        private Transform originalManagerContainer;
        private Dictionary<string, Transform> originalManagerTypeContainers;

        [SetUp]
        public void SetUp()
        {
            originalManagerContainer = ManagerCreatorContainers.ManagerContainer;
            originalManagerTypeContainers =
                new Dictionary<string, Transform>(ManagerCreatorContainers.ManagerTypeContainers);
            testScene = EditorSceneManager.NewPreviewScene();
        }

        [TearDown]
        public void TearDown()
        {
            RestoreManagerContainerState();

            if (testScene.IsValid() && testScene.isLoaded)
            {
                EditorSceneManager.ClosePreviewScene(testScene);
            }
        }

        [Test]
        public void InitIgnoresNestedObjectsWithReservedContainerNames()
        {
            var businessOwner = new GameObject("Business Owner");
            SceneManager.MoveGameObjectToScene(businessOwner, testScene);

            var nestedCore = new GameObject(ManagerCreatorContainers.CONTAINER_NAME);
            SceneManager.MoveGameObjectToScene(nestedCore, testScene);
            nestedCore.transform.SetParent(businessOwner.transform, false);

            var nestedAudio = new GameObject("Audio");
            SceneManager.MoveGameObjectToScene(nestedAudio, testScene);
            nestedAudio.transform.SetParent(businessOwner.transform, false);

            ManagerCreatorContainers.Init(testScene);
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

        private void RestoreManagerContainerState()
        {
            var managerContainerProperty = typeof(ManagerCreatorContainers).GetProperty(
                nameof(ManagerCreatorContainers.ManagerContainer),
                BindingFlags.Public | BindingFlags.Static);
            managerContainerProperty.SetValue(null, originalManagerContainer);

            var managerTypeContainersField = typeof(ManagerCreatorContainers).GetField(
                "managerTypeContainers",
                BindingFlags.NonPublic | BindingFlags.Static);
            var managerTypeContainers =
                (Dictionary<string, Transform>)managerTypeContainersField.GetValue(null);
            managerTypeContainers.Clear();

            foreach (var (name, container) in originalManagerTypeContainers)
            {
                managerTypeContainers.Add(name, container);
            }
        }
    }
}
