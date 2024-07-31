﻿using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;

namespace VMFramework.ResourcesManagement
{
    [ManagerCreationProvider(ManagerType.ResourcesCore)]
    public class AudioSpawner : SerializedMonoBehaviour
    {
        private static readonly DefaultPool<AudioSource> pool = new(new DefaultComponentPoolPolicy<AudioSource>(() =>
        {
            var gameObject = new GameObject();

            return gameObject.AddComponent<AudioSource>();
        }));

        public static void Return(AudioSource audioSource)
        {
            audioSource.Stop();

            if (audioSource.gameObject.activeSelf)
            {
                audioSource.transform.SetParent(ResourcesManagementSetting.AudioGeneralSetting.container);
                pool.Return(audioSource);
            }
        }

        [Button]
        public static AudioSource Spawn([GamePrefabID(typeof(AudioPreset))] string id, Vector3 pos = default,
            Transform parent = null)
        {
            var preset = GamePrefabManager.GetGamePrefabStrictly<AudioPreset>(id);

            var audioSource = pool.Get();

            audioSource.clip = preset.audioClip;
            audioSource.name = preset.id;
            audioSource.volume = preset.volume;
            audioSource.loop = false;
            audioSource.time = 0;

            var container = parent == null ? ResourcesManagementSetting.AudioGeneralSetting.container : parent;

            audioSource.transform.SetParent(container);
            audioSource.transform.position = pos;

            if (preset.autoCheckStop)
            {
                _ = CheckStop(audioSource);
            }

            if (preset.enablePlayFromTime)
            {
                audioSource.time = preset.timeToPlay;
            }

            audioSource.Play();

            return audioSource;
        }

        private static async UniTaskVoid CheckStop(AudioSource audioSource)
        {
            await UniTask.WaitUntil(() => audioSource.isPlaying == false);

            Return(audioSource);
        }
    }
}
