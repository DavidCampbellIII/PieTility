using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace PieTility
{
    public class DebugLabel : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Should the debug label be visible?")]
        private bool isVisible = true;

        [Header("==TEXT==")]
        [SerializeField]
        [TextArea]
        [Tooltip("What should the debug label say?")]
        private string text = "DEFAULT TEXT";
        [SerializeField]
        private Color textColor = Color.black;

        private GUIStyle style;
        private TrackedObjectField trackedObject;

        public class TrackedObjectField
        {
            private readonly object trackedObject;
            private readonly string trackedValue;

            private readonly bool isField; //is the tracked field a field, or a property?

            public TrackedObjectField(object trackedObject, string trackedValue)
            {
                this.trackedObject = trackedObject;
                this.trackedValue = trackedValue;
                isField = trackedObject.GetType().GetField(trackedValue) != null;
            }

            public object GetTrackedFieldValue()
            {
                if (isField)
                {
                    FieldInfo fieldInfo = trackedObject.GetType().GetField(trackedValue);
                    return fieldInfo.GetValue(trackedObject);
                }

                PropertyInfo propertyInfo = trackedObject.GetType().GetProperty(trackedValue);
                if (propertyInfo == null)
                {
                    Debug.LogError("==TRACKED VALUE '" + trackedValue + "' does not exist!==");
                }
                return propertyInfo.GetValue(trackedObject);
            }
        }

        public void DisplayLabel(string text, float duration = -1f)
        {
            DisplayLabel(text, Color.black, duration);
        }

        /// <summary>
        /// Displays label with text and color of text.  Duration of 0 or less is infinite display time, otherwise label is displayed
        /// for duration time in seconds.
        /// </summary>
        /// <param name="text">Text of the label we are setting</param>
        /// <param name="color">Color of the text of the label</param>
        /// <param name="duration">How long the label will be visible for.  Anything less than 0 is indefinite</param>
        public void DisplayLabel(string text, Color color, float duration = -1f)
        {
            this.text = text;
            this.textColor = color;
            isVisible = true;
            if (duration > 0f)
            {
                StopAllCoroutines();
                StartCoroutine(DisplayTimer(duration));
            }
        }

        /// <summary>
        /// Display a debug label while tracking a field or property of a specified object
        /// </summary>
        /// <param name="trackedObject">Object being tracked</param>
        /// <param name="trackedValue">Name of value wanting to be tracked within the tracked object</param>
        /// <param name="duration">How long the label will display for.  Defaults to -1 (which displays the lable indefinitely)</param>
        public void DisplayLabel(object trackedObject, string trackedValue, float duration = -1f)
        {
            DisplayLabel(trackedObject, trackedValue, Color.black, duration);
        }

        /// <summary>
        /// Display a debug label while tracking a field or property of a specified object
        /// </summary>
        /// <param name="color">Color of the label text</param>
        /// <param name="trackedObject">Object being tracked</param>
        /// <param name="trackedValue">Name of value wanting to be tracked within the tracked object</param>
        /// <param name="duration">How long the label will display for.  Defaults to -1 (which displays the lable indefinitely)</param>
        public void DisplayLabel(object trackedObject, string trackedValue, Color color, float duration = -1f)
        {
            this.trackedObject = new TrackedObjectField(trackedObject, trackedValue);
            this.textColor = color;
            isVisible = true;

            StartCoroutine(UpdateTrackedValue());
            if (duration > 0f)
            {
                StopAllCoroutines();
                StartCoroutine(DisplayTimer(duration));
            }
        }

        public void HideLabel()
        {
            StopAllCoroutines();
            isVisible = false;
        }

        private IEnumerator UpdateTrackedValue()
        {
            while (enabled)
            {
                text = trackedObject.GetTrackedFieldValue().ToString();
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator DisplayTimer(float duration)
        {
            yield return new WaitForSeconds(duration);
            isVisible = false;
        }

        private void SetUp()
        {
            style = new GUIStyle
            {
                padding = new RectOffset(10, 0, 10, 0)
            };
            style.normal.textColor = textColor;
            style.normal.background = Texture2D.whiteTexture;
        }

        private void OnDrawGizmos()
        {
            if (isVisible)
            {
                //we need to constantly do the setup so editor time labels work and update correctly
                SetUp();
                Handles.Label(this.transform.position, text, style);
            }
        }
    }
}
