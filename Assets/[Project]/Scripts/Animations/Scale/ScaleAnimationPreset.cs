using UnityEngine;

namespace DevNote.Gamebox
{
    [CreateAssetMenu(menuName = "Gamebox/Presets/Scale Animation", fileName = "ScaleAnimationPreset")]
    public class ScaleAnimationPreset : ScriptableObject
    {
        [field: SerializeField] public Vector2 FromToScale { get; private set; }
        [field: SerializeField] public float LoopDuration { get; private set; }
    }
}


