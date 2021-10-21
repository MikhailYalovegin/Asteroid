using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Space(10)]
    [SerializeField] private Canvas canvas;
    [Space(10)]
    [SerializeField] private Text TextNameControler;
    [SerializeField] private Text TextRocketLife;
    [SerializeField] private Text TextPointsEarnedCounter;
    [SerializeField] private GameObject ButtonContinue;

    private bool buttonContinueActive;
    private bool newCheck;
    private bool pauseGame = true;
    private KeyInput keyInput;

    private void Start()
    {
        keyInput = GameLogic.Instance.RocketPlayer.GetComponent<KeyInput>();
        buttonContinueActive = false;
        newCheck = false;
    }

    private void Update()
    {
        MenuPause();
        ButtonContinue.SetActive(buttonContinueActive);
        TextRocketLife.text = "Жизней осталось: " + GameLogic.Instance.RocketLife.ToString();
        TextPointsEarnedCounter.text = "Счёт: " + GameLogic.Instance.PointsEarnedCounter.ToString();
    }

    private void MenuPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame = true;
        }

        Time.timeScale = pauseGame ? 0f : 1f;

        canvas.gameObject.SetActive(pauseGame);

        TextNameControler.text = keyInput.MouseFlag ? "Управление: Мышь" : "Управление: WAD Space";
    }

    public void ButtonPauseGame()
    {
        pauseGame = false;
    }

    public void ButtonNewGame()
    {
        if (newCheck)
        {
            Restart.RestartLevel();
        }
        else
        {
            ButtonPauseGame();
            buttonContinueActive = true;
            newCheck = true;
        }
    }

    public void ButtonGameControler()
    {
        keyInput.ControlSwitch();
    }

    public void ButtonExitGame()
    {
        Application.Quit();
        Debug.Log("Конец ");
    }

    public void RocketFail()
    {
        buttonContinueActive = false;
        newCheck = true;
        pauseGame = true;
    }
}
