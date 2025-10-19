using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    public Level currentLevel;

    public TextAsset testLevelFile;

    public bool acceptingInput;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currentLevel = new Level(testLevelFile);
        PresentationManager.main.LoadLevel(currentLevel);
        acceptingInput = true;
    }

    public async Task HandleInput(PlayerInput input)
    {
        if (acceptingInput && currentLevel != null)
        {
            if (input is ResetInput)
            {
                currentLevel = new Level(testLevelFile);
                PresentationManager.main.LoadLevel(currentLevel);
            }
            acceptingInput = false;
            await PresentationManager.main.PresentEvents(currentLevel.HandleInput(input));
            acceptingInput = true;
        } else
        {
            Debug.LogError("GameManager cannot handle this input!");
        }
    }
}
