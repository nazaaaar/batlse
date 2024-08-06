using UnityEngine;

namespace nazaaaar.platformBattle.mini.model
{
    [CreateAssetMenu]
    public class CameraConfig: ScriptableObject
    {
        public Vector3 Offset;
        public Quaternion BlueRotation;
        public Quaternion RedRotation;
        public float SmoothSpeed;
    }
}
