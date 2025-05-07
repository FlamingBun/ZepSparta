using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private Vector2 currentPosition;

    #region Player
    [Header("Player")]
    private GameObject player;
    public GameObject Player { get { return player; } }

    private PlayerController playerController;
    public PlayerController PlayerController { get { return playerController; } }

    [SerializeField] private GameObject playerGameObject;

    #endregion Player

    protected override void Initialize()
    {
        currentPosition = Vector2.zero;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainScene":
                UIManager.Instance.SetLeftUIActive(true);
                InitializePlayer();
                break;
            case "StackScene":
                UIManager.Instance.SetLeftUIActive(false);
                break;
        }
    }

    public void StackGameOver(int stackCount, int maxCombo)
    {
        SceneManager.LoadScene("MainScene");
        DataManager.Instance.UpdateStackScore(stackCount, maxCombo);
        UIManager.Instance.ChangeState(UIState.StackGameOverUI);
    }

    private void InitializePlayer()
    {
        player = Instantiate(playerGameObject);
        playerController = player.GetComponent<PlayerController>();
        playerController.Initialize();
        player.transform.position = currentPosition;
    }

    public void StackGameStart()
    {
        currentPosition = player.transform.position;
        SceneManager.LoadScene("StackScene");
    }

}
