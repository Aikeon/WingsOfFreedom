using System;
using UnityEngine;

namespace sound
{
    public class CopyTransform : MonoBehaviour
    {
        [SerializeField] private Transform target = null;
        [SerializeField] private bool copyTransform = false;

        public void SetTarget(Transform target)
        {
            copyTransform = target != null;
            this.target = target;
        }

        public void ResetTarget()
        {
            copyTransform = false;
            this.target = null;
        }

        public bool HasTarget()
        {
            return copyTransform;
        }
        
        public bool TryGetTarget(out Transform target)
        {
            target = this.target;
            return copyTransform;
        }

        private void LateUpdate()
        {
            if (!copyTransform) return;
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }
}