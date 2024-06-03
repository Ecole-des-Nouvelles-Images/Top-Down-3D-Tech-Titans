using UnityEngine;

namespace Christopher.Scripts
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject PausePanel;

        public void TogglePauseMenuActivation() {
            if (PausePanel.activeSelf) {
                PausePanel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                PausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
        public void LeaveGame() {
            Application.Quit();
        }
    
    }
}
