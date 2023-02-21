using Infrastructure.States;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.HUD
{
    public class HUD_Display : MonoBehaviour
    {
        [SerializeField] private Button _replayButton;
        [SerializeField] private Image _loseContainer;

        private IGameStateMachine _gameStateMachine;

        public void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            Debug.Log("Start");
            _replayButton.onClick.AddListener(() =>
                _gameStateMachine.Enter<LoadLevelState, string>(
                    SceneManager.GetActiveScene().name));
        }

        public void ShowLose() => 
            _loseContainer.gameObject.SetActive(true);

        public void HideLose() => 
            _loseContainer.gameObject.SetActive(false);
    }
}