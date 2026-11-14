using UnityEngine;

public enum GameState
{
    Start,
    Playing,
    Paused,
    Victory,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("State Settings")]
    [SerializeField] private GameState currentState;
    private int lastHealth = -1;
    private int lastScore = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);//UI y escenas
    }
    
    void Start()
    {
        ChangeState(GameState.Start);
    }

    
    void Update()
    {
        if (currentState == GameState.Start)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                ChangeState(GameState.Playing);
            }
        }

        //pausa inputs
        if (Input.GetKeyDown(KeyCode.P))
        {
            Paused();
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.Start:
                Time.timeScale = 0f;//0f se detiene - 1f se mueve normal
                Debug.Log("<color=orange>Inicio el juego. Presiona ENTER</color>");
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                Debug.Log("<color=green>ESTADO: jugando</color>");
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                Debug.Log("<color=yellow>ESTADO: pausa</color>");
                break;
            case GameState.Victory:
                Time.timeScale = 0f;
                Debug.Log("<color=cyan>ESTADO: Victoria</color>");
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                Debug.Log("<color=red>ESTADO: Game Over</color>");
                break;
        }
        LogPlayerStatus();
    }

    //pausa
    public void Paused()
    {
        if (currentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
        else if (currentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }

    //notificaciones
    public void NotifyPlayerDead()
    {
        if (currentState != GameState.Playing) return;

        ChangeState(GameState.GameOver);
    }
    public void NotifyGoalZone()
    {
        if (currentState != GameState.Playing) return;

        ChangeState(GameState.Victory);
    }
    public void NotifyPlayerStatusChanged()
    {
        LogPlayerStatus();
    }
    public void LogPlayerStatus()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("No se encontró al jugador.");
            return;
        }

        HealthSystem health = player.GetComponent<HealthSystem>();
        ScoreManager score = player.GetComponent<ScoreManager>();
        if (health == null || score == null) return;

        int currentHealth = health.CurrentHealth;
        int currentScore = score.Score;

        if (currentHealth != lastHealth || currentScore != lastScore)
        {
            lastHealth = currentHealth;
            lastScore = currentScore;

            Debug.Log(
                "<color=red>Salud actual: " + lastHealth + "</color>\n" +
                "<color=yellow>Puntuación: " + lastScore + "</color>\n");
        }
    }
}
