using System;
using NUnit.Framework;
using UnityEngine;
using VMFramework.UI;

namespace VMFramework.Editor.Tests
{
    public sealed class ContainerRefreshPublicationTests
    {
        [Test]
        public void ExplicitContainerRefreshConsumesItsAlreadyPublishedDirtyWork()
        {
            var source = new GameObject("Container Refresh Publication Witness");
            try
            {
                var probe = source.AddComponent<ContainerRefreshPublicationProbe>();
                probe.MarkDirty();
                probe.Refresh();
                Assert.That(probe.Publications, Is.EqualTo(1));
                probe.RunUpdate();
                Assert.That(probe.Publications, Is.EqualTo(1),
                    "The next production Update must not republish work already consumed explicitly.");
            }
            finally { UnityEngine.Object.DestroyImmediate(source); }
        }

        [Test]
        public void ContainerRefreshKeepsNewDirtyWorkRaisedByItsSubscriber()
        {
            var source = new GameObject("Reentrant Container Refresh Witness");
            try
            {
                var probe = source.AddComponent<ContainerRefreshPublicationProbe>();
                probe.DuringPublication = () => { probe.DuringPublication = null; probe.MarkDirty(); };
                probe.MarkDirty();
                probe.Refresh();
                probe.RunUpdate();
                Assert.That(probe.Publications, Is.EqualTo(2));
                probe.RunUpdate();
                Assert.That(probe.Publications, Is.EqualTo(2), "The new work must also be consumed once.");
            }
            finally { UnityEngine.Object.DestroyImmediate(source); }
        }

        public sealed class ContainerRefreshPublicationProbe : UIToolkitContainerModifierBase
        {
            public int Publications { get; private set; }
            public Action DuringPublication { get; set; }
            public void MarkDirty() => OnItemDirty(null, 0, null, false);
            public void RunUpdate() => base.Update();
            protected override void OnRefresh()
            {
                Publications++;
                DuringPublication?.Invoke();
                base.OnRefresh();
            }
        }

    }
}
