using NaughtyAttributes;
using UnityEngine;


namespace DevNote.LevelUp
{

    [CreateAssetMenu(menuName = "Configs/LevelUp/Image Palette", fileName = "ImagePalette")]
    public class ImagePaletteColorConfig : ScriptableObject
    {
        private enum ColorType { Single, Gradient }


        [SerializeField] private ColorType _colorType;
        [field: SerializeField, Space, ShowIf(nameof(UseSingleColor))] public Color Color { get; private set; }
        [field: SerializeField, Space, ShowIf(nameof(UseGradientColor))] public Color Color1 { get; private set; }
        [field: SerializeField, ShowIf(nameof(UseGradientColor))] public Color Color2 { get; private set; }
        [field: SerializeField, Range(-180f, 180f), ShowIf(nameof(UseGradientColor))] public float Angle { get; private set; }


        public bool UseGradientColor => _colorType == ColorType.Gradient;
        public bool UseSingleColor => _colorType == ColorType.Single;




#if UNITY_EDITOR
        private void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                // Scenes
                foreach (var image in FindObjectsOfType<ImagePaletteColor>(true))
                {
                    if (image.ColorConfig == this)
                    {
                        image.ApplyColor();
                        UnityEditor.EditorApplication.delayCall += image.RemoveUnusedComponents;
                    }

                }

                // Prefabs
                string[] guids = UnityEditor.AssetDatabase.FindAssets("t:Prefab");
                foreach (string guid in guids)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (prefab == null) continue;

                    bool changed = false;

                    // Ищем все ImageListener в префабе
                    foreach (var image in prefab.GetComponentsInChildren<ImagePaletteColor>(true))
                    {
                        image.ApplyColor();
                        image.RemoveUnusedComponents();
                        UnityEditor.EditorUtility.SetDirty(image);
                        changed = true;
                    }

                    if (changed)
                        UnityEditor.AssetDatabase.SaveAssets();
                }
            };


            

        }
#endif


    }
}


