﻿using FishNet.Object;
using UnityEngine;

namespace FishNet.Demo.NetworkLod
{
    public class MoveRandomly : NetworkBehaviour
    {
        //Colors green for client.
        [SerializeField] private Renderer _renderer;
        [SerializeField] private bool _updateRotation;

        //Time to move to new position.
        private const float _moveRate = 3f;
        //Maximum range for new position.
        private const float _range = 10f;
        //Position to move towards.
        private Vector3 _goalPosition;
        //Rotation to move towards.
        private Quaternion _goalRotation;
        //Position at spawn.
        private Vector3 _startPosition;


        public float RotationRate = 15f;

        private Quaternion _lastRot;

        private void Update()
        {
            //Client should not move these.
            if (base.IsClientOnlyStarted)
                return;
            //Server shouldn't move client one.
            if (base.Owner.IsValid)
                return;

            transform.position = Vector3.MoveTowards(transform.position, _goalPosition, _moveRate * Time.deltaTime);
            if (_updateRotation)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _goalRotation, RotationRate * Time.deltaTime);
            if (transform.position == _goalPosition)
                RandomizeGoal();

            // if (InstanceFinder.TimeManager.FrameTicked)
            // {
            //     if (_lastRot != transform.rotation)
            //     {
            //         float x = transform.rotation.x - _lastRot.x;
            //         float y = transform.rotation.y - _lastRot.y;
            //         float z = transform.rotation.z - _lastRot.z;
            //         float w = transform.rotation.w - _lastRot.w;
            //
            //         Debug.Log($"Differences: X {x}, Y {y}, Z {z}, W {w}.");
            //         _lastRot = transform.rotation;
            //     }
            // }
        }

        public override void OnStartNetwork()
        {
            _startPosition = transform.position;
            RandomizeGoal();

            if (_renderer != null && base.Owner.IsActive)
                _renderer.material.color = Color.green;

            if (!base.Owner.IsValid)
                gameObject.name = "LOD " + base.ObjectId;
            else
                gameObject.name = "Owned " + base.ObjectId;
        }

        private void RandomizeGoal()
        {
            _goalPosition = _startPosition + (Random.insideUnitSphere * _range);

            if (_updateRotation)
            {
                bool rotate = (Random.Range(0f, 1f) <= 0.33f);
                if (rotate)
                {
                    Vector3 euler = Random.insideUnitSphere * 180f;
                    _goalRotation = Quaternion.Euler(euler);
                }
                else
                {
                    _goalRotation = transform.rotation;
                }
            }
        }
    }
}