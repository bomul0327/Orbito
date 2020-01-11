using UnityEngine;
using UnityEngine.UI;

namespace Experimental
{
    public class TestEaseManager : MonoBehaviour
    {
        [Header("Movement Settings")]
        public EaseType easeFunction;
        public float duration = 1;
        public float waitTime = 1;

        [Header("Object References")]
        public Transform actorTransform;
        public Transform startTransform;
        public Transform endTransform;

        [Header("UI References")]
        public Slider slider;
        public Text sliderText;
        public Dropdown dropdown;
        public Text text;
        public LineRenderer lineRenderer;

        private float time;

        private static readonly int LinePrecision = 30;

        // Start is called before the first frame update
        void Start()
        {
            time = 0;

            //Initialize dropdown with enum values.
            UGUIUtils.DropdownWithEnum<EaseType>(dropdown);
            dropdown.onValueChanged.AddListener(OnDropdownValueChange);

            //Initialize slider 
            slider.minValue = 0.1f;
            slider.maxValue = 10f;
            slider.value = duration;
            slider.onValueChanged.AddListener(OnSliderValueChange);
            slider.onValueChanged.Invoke(duration);

            DrawEaseFunction();

        }

        void Update()
        {
            time += Time.deltaTime;

            if (time > duration + waitTime)
            {
                time = 0;
                return;
            }

            float t = time / duration;

            UpdateActor(t);
            UpdateUI(t);
        }



        private void UpdateActor(float t)
        {
            actorTransform.position = MathExtension.Interp(easeFunction, startTransform.position, endTransform.position, t);
        }

        private void UpdateUI(float t)
        {
            text.text = $"t: {Mathf.Clamp01(MathExtension.Interp(easeFunction, 0, 1, t)):F2}";
        }

        private void DrawEaseFunction()
        {
            var startPos = startTransform.position;
            startPos.y -= 3;

            var endPos = endTransform.position;
            endPos.y += 3;

            lineRenderer.positionCount = LinePrecision + 1;

            var points = new Vector3[LinePrecision + 1];
            for (int i = 0; i <= LinePrecision; i++)
            {
                float t = i / (float)LinePrecision;
                points[i] = new Vector3(
                MathExtension.Interp(EaseType.Linear, startPos.x, endPos.x, t),
                MathExtension.Interp(easeFunction, startPos.y, endPos.y, t),
                0);

            }
            lineRenderer.SetPositions(points);

        }

        public void OnSliderValueChange(float value)
        {
            time = 0;
            duration = value;
            sliderText.text = value.ToString();
        }

        public void OnDropdownValueChange(int value)
        {
            easeFunction = (EaseType)value;
            time = 0;
            DrawEaseFunction();
        }

    }
}