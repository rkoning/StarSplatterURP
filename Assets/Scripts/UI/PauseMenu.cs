using UnityEngine;

public class PauseMenu : UIMenu
{

    private GameObject menuPanel;
    bool paused;

    public void Initialize(GameObject menuPanel) {
        this.menuPanel = menuPanel;
        this.menuPanel.SetActive(false);
    }

    public void Pause() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        menuPanel.SetActive(true);
        paused = true;
        Time.timeScale = 0f;
    }

    public void Resume() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuPanel.SetActive(false);
        paused = false;
        Time.timeScale = 1f;
    }

    public override void Open() {
        Pause();
    }

    public override void Close() {
        Resume();
    }

    public void Quit() {
        Application.Quit();
    }

    public bool IsPaused()
    {
        return paused;
    }
}
