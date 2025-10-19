using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public enum ScreenID
{
    Title,
    Level,
    LevelComplete,
    End
}

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    public Level currentLevel;

    public TextAsset[] levelList;
    public int currentLevelNumber;

    public bool acceptingInput;
    public ScreenID currentScreen;

    private UIDocument uiDoc;
    private VisualElement root;
    private Label fireCounter;
    private int fireCount;
    private VisualElement titleScreen;
    private VisualElement levelScreen;
    private VisualElement levelCompleteScreen;
    private VisualElement endScreen;
    private Button playButton;
    private Button nextLevelButton;

    private void Awake()
    {
        main = this;

        uiDoc = GetComponentInChildren<UIDocument>();
        root = uiDoc.rootVisualElement;
        fireCounter = root.Q<Label>("fire-counter");
        titleScreen = root.Q<VisualElement>("title-screen");
        levelScreen = root.Q<VisualElement>("level-screen");
        levelCompleteScreen = root.Q<VisualElement>("level-complete-screen");
        endScreen = root.Q<VisualElement>("end-screen");
        playButton = root.Q<Button>("play-button");
        nextLevelButton = root.Q<Button>("next-level-button");

        playButton.clicked += NextLevel;
        nextLevelButton.clicked += NextLevel;

    }

    private void Start()
    {
        currentLevelNumber = -1;
        SetScreen(ScreenID.Title);
    }

    private void NextLevel()
    {
        currentLevelNumber++;
        if (currentLevelNumber < levelList.Length)
        {
            currentLevel = new Level(levelList[currentLevelNumber]);
            PresentationManager.main.LoadLevel(currentLevel);
            SetFireCount(0, currentLevel.fireRequirement);
            acceptingInput = true;
            SetScreen(ScreenID.Level);
        } else
        {
            SetScreen(ScreenID.End);
        }
    }

    private void SetScreen(ScreenID id)
    {
        currentScreen = id;

        titleScreen.style.display = DisplayStyle.None;
        levelScreen.style.display = DisplayStyle.None;
        levelCompleteScreen.style.display = DisplayStyle.None;
        endScreen.style.display = DisplayStyle.None;

        if (id == ScreenID.Title)
        {
            titleScreen.style.display = DisplayStyle.Flex;
        } else if (id == ScreenID.Level)
        {
            levelScreen.style.display = DisplayStyle.Flex;
        }
        else if (id == ScreenID.LevelComplete)
        {
            levelCompleteScreen.style.display = DisplayStyle.Flex;
        }
        else if (id == ScreenID.End)
        {
            endScreen.style.display = DisplayStyle.Flex;
        }
    }

    public async Task HandleInput(PlayerInput input)
    {
        if (acceptingInput && currentLevel != null)
        {
            if (input is ResetInput)
            {
                currentLevel = new Level(levelList[currentLevelNumber]);
                PresentationManager.main.LoadLevel(currentLevel);
                SetFireCount(0, currentLevel.fireRequirement);
            }
            acceptingInput = false;
            await PresentationManager.main.PresentEvents(currentLevel.HandleInput(input));
            acceptingInput = true;
        } else
        {
            Debug.LogError("GameManager cannot handle this input!");
        }
    }

    public void SetFireCount(int current, int max)
    {
        fireCounter.text = $"{current}/{max} fires started";
        fireCount = current;
    }

    public void ScoreFire()
    {
        SetFireCount(fireCount + 1, currentLevel.fireRequirement);
        if (fireCount >= currentLevel.fireRequirement)
        {
            SetScreen(ScreenID.LevelComplete);
            PresentationManager.main.UnloadLevel();
            currentLevel = null;
        }
    }
}
