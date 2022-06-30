#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;
using System.Linq;

namespace SyluxDev.ElementsEngine
{
    public class SyluxMonoBehaviour : MonoBehaviour
    {
        public virtual void OnValidate()
        {
            #if UNITY_EDITOR
            if(Application.isEditor && !Application.isPlaying) InjectGetComponent();
            #endif
        }

        public virtual void Reset()
        {
            #if UNITY_EDITOR
            if(Application.isEditor && !Application.isPlaying) LoadDefaults();
            #endif
        }

        private void InjectGetComponent()
        {
            var fieldInfos = GetType().GetFields(EEReflection.FULL_BINDING);

            for (int i = 0; i < fieldInfos.Length; i++)
            {
                var field = fieldInfos[i];
                var getComponentAttribute = field.GetCustomAttributes(typeof(GetComponentAttribute), false).Cast<GetComponentAttribute>().FirstOrDefault();
                if (getComponentAttribute == null) continue;
                
                var type = field.FieldType;
                if (TryGetComponent(type, out var component))
                    field.SetValue(this, component);
                else
                    Debug.LogWarning("GetComponent typeof(" + type.Name + ") in game object '" + gameObject.name + "' is null");
            }
        }

        protected void LoadDefaults()
        {
            var fieldInfos = GetType().GetFields(EEReflection.FULL_BINDING);

            for (int i = 0; i < fieldInfos.Length; i++)
            {
                var field = fieldInfos[i];
                var defaultAssetAttribute = field.GetCustomAttributes(typeof(DefaultAssetAttribute), false).Cast<DefaultAssetAttribute>().FirstOrDefault();
                if (defaultAssetAttribute == null) continue;

                var assetAtPath = Resources.Load(defaultAssetAttribute.AssetName);
                if (assetAtPath != null)
                {
                    var fieldType = field.FieldType;
                    if (fieldType == assetAtPath.GetType() || fieldType.IsSubclassOf(typeof(Object)))
                    {
                        field.SetValue(this, assetAtPath);
                    }
                }
            }

            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

    }
}