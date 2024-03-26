using System.Collections;
using Unity.MegacityMetro.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UIElements;

namespace Unity.MegacityMetro.UI {
	/// <summary>
	/// Tutorial Screen UI element
	/// </summary>
	[RequireComponent(typeof(UIDocument))]
	public class TutorialScreen : MonoBehaviour {
		public static TutorialScreen Instance { get; private set; }

		VisualElement m_TutorialScreen;

		/// <summary> [s]ingle [p]layer </summary>
		VisualElement sp;
		/// <summary> [m]ulti[p]layer </summary>
		VisualElement mp;
		/// <summary> [s]ingle [p]layer </summary>
		VisualElement spMobile;
		/// <summary> [m]ulti[p]layer </summary>
		VisualElement mpMobile;

		VisualElement m_ShootButton;
		VisualElement m_LeaderboardInstructions;
		VisualElement m_JoystickHandler;
		VisualElement m_SpeedSlider;
		bool active;



		void Awake () {
			if (Instance == null) {
				Instance = this;
			} else {
				Destroy(gameObject);
			}
		}

		void OnEnable () {
			var root = GetComponent<UIDocument>().rootVisualElement;
			m_TutorialScreen = root.Q<VisualElement>("tutorial-screen");
			sp = root.Q<VisualElement>("tutorial-single-player");
			mp = root.Q<VisualElement>("tutorial-multiplayer");
			spMobile = root.Q<VisualElement>("tutorial-mobile");
			m_ShootButton = root.Q<VisualElement>("shoot-button");
			m_LeaderboardInstructions = root.Q<VisualElement>("leaderboard-instructions");
			m_JoystickHandler = root.Q<VisualElement>("handle");
			m_SpeedSlider = root.Q<VisualElement>("speed-slider");
		}

		void Start () {
			if (PlayerInfoController.Instance == null)
				return;

			ShowTutorial();

#if UNITY_ANDROID || UNITY_IPHONE
            spMobile.style.display = DisplayStyle.Flex;
            sp      .style.display = DisplayStyle.None;
            mp      .style.display = DisplayStyle.None;

            if (PlayerInfoController.Instance.IsSinglePlayer)
            {
                m_ShootButton.style.display = DisplayStyle.None;
                m_LeaderboardInstructions.style.display = DisplayStyle.None;
            }
            else
            {
                m_ShootButton.style.display = DisplayStyle.Flex;
                m_LeaderboardInstructions.style.display = DisplayStyle.Flex;
            }
#else
			spMobile.style.display = DisplayStyle.None;

			if (PlayerInfoController.Instance.IsSinglePlayer) {
				sp.style.display = DisplayStyle.Flex;
				mp.style.display = DisplayStyle.None;
			} else {
				mp.style.display = DisplayStyle.Flex;
				sp.style.display = DisplayStyle.None;
			}
#endif
		}

		public void ShowTutorial () {
			if (active)
				return;

			m_TutorialScreen.style.display = DisplayStyle.Flex;
			active = true;

			StartCoroutine(WaitForAnyInput());
#if UNITY_ANDROID || UNITY_IPHONE
            StartCoroutine(AnimateMobileTutorial());
#endif
		}

		void HideTutorial () {
			m_TutorialScreen.style.display = DisplayStyle.None;
			active = false;
		}

		IEnumerator WaitForAnyInput () {
			while (active) {
				InputSystem.onAnyButtonPress.CallOnce(_ => { HideTutorial(); });
				yield return null;
			}
		}

		IEnumerator AnimateMobileTutorial () {
			while (active) {
				yield return new WaitForSeconds(1f);
				m_JoystickHandler.transform.position = Random.insideUnitCircle * 50f;
				m_SpeedSlider.transform.position = new Vector3(0, Random.Range(-100, 100), 0);
				yield return new WaitForSeconds(1f);
				m_JoystickHandler.transform.position = Vector3.zero;
			}
		}
	}
}