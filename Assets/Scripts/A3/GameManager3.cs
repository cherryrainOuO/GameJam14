using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;

public class GameManager3 : Singleton<GameManager3>
{
    public bool isPlayerTurn = true;
    public bool isDrag = false;
    public int removeCount = 2;

    public bool isStop = false;

    public bool isPlayerMove = false;
    public int playerJumpCount = 1;

    private string path;

    [HideInInspector] public GameData data;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        path = Path.Combine(Application.dataPath, "save.json");
        ResetInfo();

        Load();
    }

    public void ResetScene()
    {
        //TODO SceneManager.LoadScene(data.cuurentScene);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResetInfo();
    }

    private void ResetInfo()
    {
        isPlayerTurn = true;
        isDrag = false;
        removeCount = 2;
        isStop = false;
        isPlayerMove = false;
        playerJumpCount = 1;
    }

    public void Save()
    {
        data = new GameData(0); //TODO 새로운 데이터 저장
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        // List 직렬화 -> Json 으로 만들어버리기
        // Formatting.Indented : Json preetyPrint 기능

        //File.WriteAllText(path, json); // json 저장 //? 디버깅 용

        File.WriteAllText(path, json);
    }

    private void Load()
    {
        if (!File.Exists(path)) //? 세이브파일 로드
        {
            Debug.Log("없어 ");

            data = new GameData(0);
        }
        else
        {
            string encryptData = File.ReadAllText(path);

            //dataLists = JsonConvert.DeserializeObject<List<GameData>>(encryptData); //? 디버깅 용

            data = JsonConvert.DeserializeObject<GameData>(encryptData);
        }
    }

    [System.Serializable]
    public struct GameData
    {
        public int currentScene;

        public GameData(int _currentScene)
        {
            currentScene = _currentScene;
        }
    }

}
