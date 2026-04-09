using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SelfConscious
{
    [CustomEditor(typeof(ScriptableObject), true)]
    public class EventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ScriptableObject scriptable = (ScriptableObject)target;

            Type type = scriptable.GetType();
            if (type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(Event<>))
            {
                if (GUILayout.Button("Invoke"))
                {
                    Type eventType = type.BaseType.GetGenericArguments()[0];
                    FieldInfo valuefield = type.BaseType.GetField("testingValue", BindingFlags.Public | BindingFlags.Instance);
                    object value = valuefield.GetValue(scriptable);

                    MethodInfo method = type.GetMethod("Invoke");
                    method?.Invoke(scriptable, new object[] { value });
                }
            }
        }
    }
}

