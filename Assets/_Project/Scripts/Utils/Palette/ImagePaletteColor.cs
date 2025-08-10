using UnityEngine;
using UnityEngine.UI;


namespace DevNote.LevelUp
{
    [RequireComponent(typeof(Image))]
    public class ImagePaletteColor : MonoBehaviour
    {
        [field: SerializeField] public ImagePaletteColorConfig ColorConfig;


        public void ApplyColor()
        {
            if (ColorConfig == null) return;

            if (ColorConfig.UseGradientColor)
                ApplyGradientColor();

            else if (ColorConfig.UseSingleColor)
                ApplySingleColor();
        }

        public void RemoveUnusedComponents()
        {
            if (ColorConfig.UseSingleColor && TryGetComponent<UIGradient>(out var gradient))
                DestroyImmediate(gradient);

        }


        private void ApplySingleColor()
        {
            GetComponent<Image>().color = ColorConfig.Color;
        }

        private void ApplyGradientColor()
        {
            GetComponent<Image>().color = Color.white;

            if (!TryGetComponent<UIGradient>(out var gradient))
                gradient = gameObject.AddComponent<UIGradient>();

            gradient.m_color1 = ColorConfig.Color1;
            gradient.m_color2 = ColorConfig.Color2;
            gradient.m_angle = ColorConfig.Angle;

            gradient.UpdateMesh();
        }



#if UNITY_EDITOR
        private void OnValidate()
        {
            ApplyColor();
            UnityEditor.EditorApplication.delayCall += RemoveUnusedComponents;
        }
#endif


    }

}

