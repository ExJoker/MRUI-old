using UnityEngine;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity.InputModule;

namespace HoloUi
{
    /// <summary>
    /// Hologram interaction that uses one gestureRecognizer for everything, 
    /// suggested here: https://forums.hololens.com/discussion/7254/best-approach-to-implement-multiple-gesturerecognizers
    /// 
    /// It also simulates interactions using touch and mouse events.
    /// </summary>
    public class HoloUiInteractionManager : MonoBehaviour
    {
        protected GestureRecognizer gestureRecognizer;

        public delegate void OnTappedDelegate();

        public event OnTappedDelegate OnTapped;

        // hololens actions
        private void Start()
        {
            gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap |
                                                      GestureSettings.ManipulationTranslate |
                                                      GestureSettings.Hold);
            gestureRecognizer.TappedEvent += OnAirTapped;

            gestureRecognizer.StartCapturingGestures();
        }

        private void OnAirTapped(InteractionSourceKind source, int tapCount, Ray headRay)
        {
            OnTap();
        }

        // mouse / touch actions
        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                OnTap();
            }
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    OnTap();
                }
            }
        }

        // logic
        void OnTap()
        {
            // user wants to press a button
            GameObject go = GazeManager.Instance.HitObject;
            if (go != null)
            {
                HoloUiButton holoBtn = go.GetComponent<HoloUiButton>();
                if (holoBtn != null && holoBtn.OnPressed != null)
                {
                    holoBtn.OnPressed.Invoke();
                    return;
                }
            }
            // user tapped somewhere (not on a button)
            if (OnTapped != null)
            {
                OnTapped();
            }
        }
    }
}