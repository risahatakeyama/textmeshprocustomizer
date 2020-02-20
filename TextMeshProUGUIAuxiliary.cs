
using TMPro;
using UnityEditor;
using UnityEngine;

namespace OneProjest {

    [AddComponentMenu("TextMeshProUGUIAuxiliary")]
    [RequireComponent(typeof(CustomTextMeshProUGUI))]
    [DisallowMultipleComponent]
    public class TextMeshProUGUIAuxiliary : MonoBehaviour {
        //一つのマテリアルで動的に複数のテキストのデザインをカスタマイズするための機能。
        //値をこのクラスに保存しておき、ゲーム実行時に値をテキストデザインに反映する。
        //２つ以上のテキストデザインを表示するためには別途materialpresetを増やす必要がある。

        public Color _shadowColor;
        public float _shadowDilate;
        public float _shadowOffsetX;
        public float _shadowOffsetY;
        public float _shadowSoftness;

        public Color _outlineColor;
        public float _outlineDistance;

        public float _textSoftness;

        public CustomTextMeshProUGUI _textMeshPro;

        public Material _material;
        private float characterSpacing;

        private void Reset() {
            _textMeshPro = GetComponent<CustomTextMeshProUGUI>();
            _material = _textMeshPro.materialForRendering;
            characterSpacing = _textMeshPro.characterSpacing;

            _textMeshPro.SetCallBack(() => {
                characterSpacing = _textMeshPro.characterSpacing;
            });
        }

        private void Awake() {
            SetShadow();
            SetOutline();
            SetFaceSoftness();
        }

        public void SetShadow(Color color, float dilate, float offsetX, float offsetY, float softness) {
            _shadowColor = color;
            _shadowDilate = dilate;
            _shadowOffsetY = offsetY;
            _shadowOffsetX = offsetX;
            _shadowSoftness = softness;

            SetShadow();
        }

        public void SetShadow() {

            _material.SetColor("_UnderlayColor", _shadowColor);
            _material.SetFloat("_UnderlayOffsetX", _shadowOffsetX);
            _material.SetFloat("_UnderlayOffsetY", _shadowOffsetY);
            _material.SetFloat("_UnderlayDilate", _shadowDilate);
            _material.SetFloat("_UnderlaySoftness", _shadowSoftness);
        }

        public void SetOutline(Color color, float distance = 0) {
            _outlineColor = color;
            _outlineDistance = distance;
            SetOutline();
        }

        public void SetOutline() {
            _material.SetFloat("_FaceDilate", _outlineDistance);
            _material.SetFloat("_OutlineWidth", _outlineDistance);
            _material.SetColor("_OutlineColor", _outlineColor);
            //線が太くなるほど違和感が大きいため、太さに合わせてカーニングを行う。
            _textMeshPro.characterSpacing = 0;
            _textMeshPro.characterSpacing = _outlineDistance * 10 + characterSpacing;
            characterSpacing = _textMeshPro.characterSpacing;
        }

        public void SetFaceSoftness(float softness) {
            _textSoftness = softness;
            SetFaceSoftness();
        }

        public void SetFaceSoftness() {

            _material.SetFloat("_OutlineSoftness", _textSoftness);
        }
    }

    [CustomEditor(typeof(TextMeshProUGUIAuxiliary))]
    [CanEditMultipleObjects]
    public class TextMeshProUGUIAuxiliaryEditor : Editor {

        public override void OnInspectorGUI() {
            var textMeshProAuxiliary = target as TextMeshProUGUIAuxiliary;
            if (textMeshProAuxiliary == null) {
                return;
            }
            EditorGUILayout.LabelField("【Shadow】");
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("OffsetX");
            textMeshProAuxiliary._shadowOffsetX = EditorGUILayout.Slider(textMeshProAuxiliary._shadowOffsetX, -1, 1);
            EditorGUILayout.LabelField("OffsetY");
            textMeshProAuxiliary._shadowOffsetY = EditorGUILayout.Slider(textMeshProAuxiliary._shadowOffsetY, -1, 1);
            EditorGUILayout.LabelField("Dilate");
            textMeshProAuxiliary._shadowDilate = EditorGUILayout.Slider(textMeshProAuxiliary._shadowDilate, 0, 1);
            EditorGUILayout.LabelField("Softness");
            textMeshProAuxiliary._shadowSoftness = EditorGUILayout.Slider(textMeshProAuxiliary._shadowSoftness, 0, 1);
            EditorGUILayout.LabelField("Color");
            textMeshProAuxiliary._shadowColor = EditorGUILayout.ColorField(textMeshProAuxiliary._shadowColor);
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("【Outline】");
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Dilate");
            textMeshProAuxiliary._outlineDistance = EditorGUILayout.Slider(textMeshProAuxiliary._outlineDistance, 0, 1);
            EditorGUILayout.LabelField("Color");
            textMeshProAuxiliary._outlineColor = EditorGUILayout.ColorField(textMeshProAuxiliary._outlineColor);
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("【TextSoftness】");
            textMeshProAuxiliary._textSoftness = EditorGUILayout.Slider(textMeshProAuxiliary._textSoftness, 0, 1);

            if (GUILayout.Button("プレビュー")) {
                textMeshProAuxiliary.SetShadow();
                textMeshProAuxiliary.SetOutline();
                textMeshProAuxiliary.SetFaceSoftness();
            }
        }
    }
}

