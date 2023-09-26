#pragma warning disable
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region SETTINGS
    [Header("Ship Game Settings")]
    public Ship SpaceShip;
    [Range(0, 10)]
    public int lives = 3;
    [Range(0f, 10f)]
    public float respawnTime = 3.0f;
    [Range(0f, 10f)]
    public float invulnerability = 3.0f;
    [Header("Asteroids Scores Settings")]
    [Range(1f, 200f)]
    public int smallAsteroids = 50;
    [Range(1f, 300f)]
    public int mediumAsteroids = 100;
    [Range(1f, 500f)]
    public int bigAsteroids = 200;
    [Header("Game Settings")]
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public TextMeshProUGUI shipLifesText;
    public TextMeshProUGUI scoreValueText;
    public TextMeshProUGUI highscoreValueText;
    public Transform Composter;
    public ParticleSystem explosion;
    public AudioClip[] CrashSound;
    public AudioClip[] MainSounds;

    private static GameManager _instance;
    private bool isOnGameOverScreen = false;
    private bool isOnPauseScreen = false;
    private int score;
    private int highscore;
    AudioSource backgroundMusic;
    #endregion

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject("GameManager");
                _instance = singletonObject.AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        highscore = PlayerPrefs.GetInt("Highscore", 50000);
        highscoreValueText.text = highscore.ToString("###,###,###");
        Cursor.visible = false;
        backgroundMusic = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (isOnGameOverScreen && Input.anyKeyDown)
        {
            StartCoroutine(RestartGame());
        }
        if (!isOnGameOverScreen && Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            isOnPauseScreen = !isOnPauseScreen;
            if (!isOnPauseScreen)
            {
                Time.timeScale = 1; PauseScreen.SetActive(false);
            }
            else
            {
                Time.timeScale = 0; PauseScreen.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (backgroundMusic.isPlaying) { backgroundMusic.Stop(); }
            else { backgroundMusic.Play(); }
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("SpaceRocks");
        }
    }

    public void OnPlayerKilled()
    {
        this.explosion.transform.position = this.SpaceShip.transform.position;
        this.explosion.Play();
        this.lives--;
        shipLifesText.text = lives.ToString();
        if (this.lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), this.respawnTime);
        }
        AudioSource.PlayClipAtPoint(MainSounds[0], this.SpaceShip.transform.position);
    }
    public void OnAsteroidDestroy(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();
        if (asteroid.size < 0.75f)
        {
            score += smallAsteroids;
            AudioSource.PlayClipAtPoint(CrashSound[0], asteroid.transform.position);
        }
        else if (asteroid.size < 1.25f)
        {
            score += mediumAsteroids;
            AudioSource.PlayClipAtPoint(CrashSound[1], asteroid.transform.position);
        }
        else
        {
            score += bigAsteroids;
            AudioSource.PlayClipAtPoint(CrashSound[2], asteroid.transform.position);
        }
        scoreValueText.text = score.ToString("###,###,###");
        if(this.score > this.highscore)
        {
            this.highscore = this.score;
            highscoreValueText.text = highscore.ToString("###,###,###");
        }
    }
    private void Respawn()
    {
        this.SpaceShip.gameObject.layer = LayerMask.NameToLayer("Invensible");
        this.SpaceShip.transform.position = Vector3.zero;
        this.SpaceShip.gameObject.SetActive(true);
        Invoke(nameof(MakeDestructable), invulnerability);
    }
    private void MakeDestructable()
    {
        this.SpaceShip.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    private void GameOver()
    {
        backgroundMusic.Stop();
        if (this.score > this.highscore)
        {
            PlayerPrefs.SetInt("Highscore", this.highscore);
        }
        AudioSource.PlayClipAtPoint(MainSounds[1], this.SpaceShip.transform.position);
        GameOverScreen.SetActive(true);
        Spawner spgm = GetComponent<Spawner>();
        spgm.enabled = false;
        this.SpaceShip.enabled = false;
        isOnGameOverScreen = true;
        var gosc = GameOverScreen.transform.GetChild(0).gameObject;
        StartCoroutine(Blinker(gosc));
    }
    public IEnumerator Blinker(GameObject gameobject)
    {
        bool activated = true;
        while (isOnGameOverScreen)
        {
            activated = !activated;
            gameobject.SetActive(activated);
            yield return new WaitForSeconds(0.2f);
        }
    }
    public IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3f);
        isOnGameOverScreen = false;
        SceneManager.LoadScene("_SpaceRocks");
    }
}