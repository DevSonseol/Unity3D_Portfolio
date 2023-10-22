using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Panel_Lobby : Panel
{
    public void OnClickBtsDefault(int type)
    {
        Main main = Main.Instance;

        if(type == (int)LobbyDefaultBts.New)
        {
            ChangeLobbyState(LobbyState.CharacterSelect);
        }
        else if(type == (int)LobbyDefaultBts.Continue)
        {
            ChangeLobbyState(LobbyState.Continue);
        }
        else if (type == (int)LobbyDefaultBts.Option)
        {
            ChangeLobbyState(LobbyState.Option);
        }
        else if (type == (int)LobbyDefaultBts.Quit)
        {
#if UNITY_EDITOR
            ChangeLobbyState(LobbyState.Default);
            main.ChangeGameState(GameState.Intro);
#endif
            Application.Quit();
        }
    }

    public void OnClickBtCancel()
    {
        ChangeLobbyState(LobbyState.Default);
    }

    public void OnClickBtContinue()
    {
        Main main = Main.Instance;

        if (lobbyState == LobbyState.CharacterSelect)
        {
            //신캐 만들기
            main.playerName = nameField.text;

        }
        else if (lobbyState == LobbyState.Continue)
        {
            //데이터 불러와서 ㄱㄱ
        }else if(lobbyState == LobbyState.Option)
        {
            //옵션 데이터 저장 질문하기
        }

        if(!canStart) return;

        main.ChangeGameState(GameState.Loading , IngameMap.Town);
    }

    public void OnClickBtCharacterSelectSlot(int index)
    {
        Main main = Main.Instance;

        canStart = true;

        if (characterSlots[0] == null)
        {
            Debug.Log("캐릭터 선택 슬롯비었음");
            return;
        }

        for (int i = 0; i < characterSlots.Length; ++i)
        {
            //if(i == index)
            //{
            //    characterSlots[i];
            //}
            //else
            //{
            //    characterSlots[i].SetActive(false);
            //}
        }
        OnFocusCamera(index);

        newStartClasstype = (CharacterClassType)index;

        Debug.Log("캐릭터 선택 " + newStartClasstype);

        //캐릭터 슬롯선택된 값에 따라 플레이어 불러오기

    }


    public  void OnClickBtLoadSlot(int index)
    {
        canStart = true;

        //캐릭터 보존데이터 불러오기


    }

    public void OnFocusCamera(int index)
    {
        Main main = Main.Instance;
        GameObject target = main.lobbyObjectsController.cameraTarget[index].gameObject;
        main.cineVirtualCam.ForceCameraPosition(main.lobbyObjectsController.cameraFocusPos.position, Quaternion.identity);
        main.cineVirtualCam.LookAt = target.transform;

        for (int i = 0; i < main.lobbyObjectsController.animators.Length; i++)
        {
            if (i == index)
                main.lobbyObjectsController.SetAnimator((LobbyCharacters)i, "StandUp");
            else
                main.lobbyObjectsController.SetAnimator((LobbyCharacters)i, "Sitting");
        }

    }

    public void OnNoneFocusCamera()
    {
        Main main = Main.Instance;
        main.cineVirtualCam.ForceCameraPosition(main.lobbyObjectsController.cameraNoneFocusPos.position, Quaternion.identity);
        main.cineVirtualCam.LookAt = main.lobbyObjectsController.fire;

        for (int i = 0; i < main.lobbyObjectsController.animators.Length; i++)
            main.lobbyObjectsController.SetAnimator((LobbyCharacters)i, "Sitting");
    }

    public void OnSlideOptionBGM()
    {
        Main main = Main.Instance;
        main.audioSources[(int)SoundType.BGM].volume = bgmSlider.value;
    }
}
