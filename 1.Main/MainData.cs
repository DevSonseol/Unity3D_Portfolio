using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public partial class Main : MonoBehaviour
{
    [Header("Data")]
    public string dataPath;

    public virtual void InitDatas()
    {
        dataPath = Application.dataPath + "/SaveData/";

        //SaveOptionToJson();

        LoadItemDB();
        LoadSkillDB();
    }


    public void GetFileList(string path, List<string> _listFName)
    {
       
    }

    public void SaveOptionToJson()
    {
        string filePath = dataPath + "savetest";
        //ES3.Save("myDictionary", myDictionary);
    }

    public void SavePlayerData()
    {
        if (mainPlayer == null) return;

        Debug.Log("Save");
        ES3.Save("PlayerDefaultData", mainPlayer.defaultData, dataPath + "PlayerDefaultData.json");
        ES3.Save("PlayerStat", mainPlayer.instanceStats , dataPath + "PlayerStat.json");

        SaveQuest();
    }

    public void LoadPlayerData()
    {
        if (mainPlayer == null) return;

        Debug.Log("Load");
        mainPlayer.defaultData = ES3.Load<PlayerDefaultData>("PlayerDefaultData", dataPath + "PlayerDefaultData.json");
        mainPlayer.instanceStats = ES3.Load<Dictionary<Stat, float>>("playerStat", dataPath + "playerStat.json");

        LoadQuest();
    }

    public void LoadInventory()
    {

    }

    void LoadItemDB()
    {
        var datas = Resources.LoadAll<ItemData>($"SO/Items");

        foreach(ItemData item in datas)
        {
            itemDatas.Add(item);
        }
    }

    void LoadSkillDB()
    {
        var datas = Resources.LoadAll<SkillData>($"SO/Skill/PlayerSkill");

        foreach (SkillData skill in datas)
        {
            skillDatas.Add(skill);
        }
    }
}
