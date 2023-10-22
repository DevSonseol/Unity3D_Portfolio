using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum AsyncOperationType
{
    None = 0,
    Lobby,
    InGame,
};

public partial class Main : MonoBehaviour
{
    [NonSerialized] public AsyncOperationType asyncOpType;
    [NonSerialized] public AsyncOperation asyncLoadScene;
    string asyncOpURL;

    [SerializeField] public float LoadingProcessValue = 0.0f;

    public void LoadAssetAsync(string assetURL, AsyncOperationType type)
    {
        asyncOpURL = assetURL;
        asyncOpType = type;
    }
  

    public IEnumerator _LoadSceneAsync(string sceneName, bool isAdditive)
    {
        asyncLoadScene = SceneManager.LoadSceneAsync(sceneName, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);

        asyncOpType = AsyncOperationType.InGame;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoadScene.isDone)
        {
            float progress = asyncLoadScene.progress;
            LoadingProcessValue = progress;
            yield return null;
        }

        if (sceneName == "InGame")
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            ChangeGameState(GameState.InGame);
            MakePlayer();
            mainPlayer.GetComponent<NavMeshAgent>().Warp(GameObject.FindWithTag("StartPoint").transform.position);

            directionalLight.transform.Rotate(new Vector3(45f, 0, 0));
        }
        else if (sceneName == "Dungeon")
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            ChangeGameState(GameState.InGame);
            mainPlayer.GetComponent<NavMeshAgent>().Warp(GameObject.FindWithTag("StartPoint").transform.position);
            mainPlayer.GetComponent<NavMeshAgent>().ResetPath();
        }
    }

    public T Load<T>(string path) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');

            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = GetOriginal(name);
            if (go != null)
                return go as T;
        }
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Fail to load prefab : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Pop(original, parent).gameObject;

        GameObject go = UnityEngine.Object.Instantiate(original, parent);
        go.name = original.name;

        return go;
    }

    public GameObject Instantiate(GameObject obj, Transform parent = null)
    {
        if (obj.GetComponent<Poolable>() != null)
            return Pop(obj, parent).gameObject;

        GameObject go = UnityEngine.Object.Instantiate(obj, parent);
        go.name = obj.name;

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Push(poolable);
            return;
        }

        UnityEngine.Object.Destroy(go);
    }

}