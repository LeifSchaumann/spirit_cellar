using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    public Level currentLevel;

    public TextAsset testLevelFile;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        
    }
}
