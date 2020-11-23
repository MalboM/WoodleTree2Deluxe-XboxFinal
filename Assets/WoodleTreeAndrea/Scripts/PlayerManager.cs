using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public static PlayerManager singleton;

    public TPC[] players;

    Player inputPlayer1;
    Player inputPlayer2;
    Player inputPlayer3;
    Player inputPlayer4;

    PauseScreen ps;

    public bool testingWithKeyboard;
    int connectedControllers;

    void Awake()
    {
        //
        if (singleton != null)
        {
            enabled = false;
            Destroy(this);
            return;
        }

        inputPlayer2 = ReInput.players.GetPlayer(1);
        inputPlayer3 = ReInput.players.GetPlayer(2);
        inputPlayer4 = ReInput.players.GetPlayer(3);

        singleton = this;
        singleton.players = this.players;
        
        ps = players[0].ps;

        players[1].gameObject.SetActive(false);
        players[2].gameObject.SetActive(false);
        players[3].gameObject.SetActive(false);
        //    player = new Dictionary<int, TPC>();
    }

    //
    public void CallSuspend(bool suspend)
    {
        if (suspend)
            PauseForSuspension();
        else
            UnpauseFromSuspension();
    }

    bool trpAct = false;
    private void LateUpdate()
    {
     //   if (!ps.inPause && !ps.sS.inStart)
        {
            connectedControllers = ReInput.controllers.GetControllerCount(ControllerType.Joystick);
            if (testingWithKeyboard)
                connectedControllers += 1;

            bool actTrp = false;
            if (!players[1].gameObject.activeInHierarchy && (inputPlayer2.GetButtonDown("Start") || inputPlayer2.GetButtonDown("Submit")) && (connectedControllers >= 2))
            {
                players[1].gameObject.SetActive(true);
                ps.foxAnim.SetBool("activated", true);
                actTrp = true;
            }
            if (!ps.foxAnim.GetBool("activated") && players[1].gameObject.activeInHierarchy && (connectedControllers >= 2))
                ps.foxAnim.SetBool("activated", true);
            if (players[1].gameObject.activeInHierarchy && (connectedControllers < 2))
            {
                players[1].gameObject.SetActive(false);
                ps.foxAnim.SetBool("activated", false);
                actTrp = true;
            }

            if (!players[2].gameObject.activeInHierarchy && (inputPlayer3.GetButtonDown("Start") || inputPlayer3.GetButtonDown("Submit")) && (connectedControllers >= 3))
            {
                players[2].gameObject.SetActive(true);
                ps.beaverAnim.SetBool("activated", true);
                actTrp = true;
            }
            if (!ps.beaverAnim.GetBool("activated") && players[2].gameObject.activeInHierarchy && (connectedControllers >= 3))
                ps.beaverAnim.SetBool("activated", true);
            if (players[2].gameObject.activeInHierarchy && (connectedControllers < 3))
            {
                players[2].gameObject.SetActive(false);
                ps.beaverAnim.SetBool("activated", false);
                actTrp = true;
            }

            if (!players[3].gameObject.activeInHierarchy && (inputPlayer4.GetButtonDown("Start") || inputPlayer4.GetButtonDown("Submit")) && (connectedControllers == 4))
            {
                players[3].gameObject.SetActive(true);
                ps.bushAnim.SetBool("activated", true);
                actTrp = true;
            }
            if (!ps.bushAnim.GetBool("activated") && players[3].gameObject.activeInHierarchy && (connectedControllers >= 4))
                ps.bushAnim.SetBool("activated", true);
            if (players[3].gameObject.activeInHierarchy && (connectedControllers < 4))
            {
                players[3].gameObject.SetActive(false);
                ps.bushAnim.SetBool("activated", false);
                actTrp = true;
            }

            if (actTrp && !trpAct)
            {
#if UNITY_PS4
                //
                // check friend trophy
                trpAct = true;
                PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.PLAY_WITH_A_FRIEND);
#endif
#if UNITY_XBOXONE
                //
                // check trophy : items >= 3 and items = all 
                //
                XONEAchievements.SubmitAchievement((int)XONEACHIEVS.WOODLE_FRIENDS);
#endif
            }
        }
    }

    void OnControllerDisconnected (ControllerStatusChangedEventArgs args)
    {

    }
    
    public static void AddPlayer(TPC player, int index)
    {
        Debug.Log(index + "  " + Time.timeSinceLevelLoad);
        if (singleton == null)
        {
            Debug.LogError("PlayerManager.AddPlayer("+ player.name + ", " + index.ToString() + ") was called but singleton was null! Aborting.");
            return;
        }
        
        singleton._AddPlayer(player, index);
    }

    public static TPC GetMainPlayer()
    {
        if (singleton == null)
        {
        //    Debug.LogError("PlayerManager.GetMainPlayer() was called but singleton was null! Aborting.");
            return null;
        }

        return singleton._GetMainPlayer();
    }

    public static TPC GetPlayer(int index)
    {
        if (singleton == null)
        {
            Debug.LogError("PlayerManager.GetPlayer(" + index.ToString() + ") was called but singleton was null! Aborting.");
            return null;
        }

        return singleton._GetPlayer(index);
    }

    TPC _GetMainPlayer()
    {
        if (players.Length == 0)
        {
            Debug.LogError("PlayerManager.GetMainPlayer() was called but no player was present! Aborting");
            return null;
        }

        return _GetPlayer(0);
    }

    void _AddPlayer(TPC newPlayer, int index)
    {/*
        if (player.ContainsKey(index))
        {
            Debug.LogWarning("PlayerManager.AddPlayer(" + newPlayer.name + ", " + index.ToString() + ") was called, but PlayerManager already contains player " + player[index].name + " at index " + index.ToString() + ". Overwriting previous player reference.");
            player[index] = newPlayer;
        }
        else
            player.Add(index, newPlayer);*/
    }

    TPC _GetPlayer(int index)
    {
        foreach(TPC tpc in players)
        {
            if (int.Parse(tpc.playerID) == index)
                return tpc;

        }
        
        Debug.LogWarning("PlayerManager.GetPlayer(" + index.ToString() + ") was called but no player exist at index = " + index.ToString());
        return null;
    }

    public static void PauseForSuspension()
    {
        if (singleton == null)
        {
            Debug.LogError("PlayerManager.PauseForSuspension() was called but singleton was null! Aborting.");
            return;
        }

        singleton._PauseForSuspension();
    }

    void _PauseForSuspension()
    {
        Debug.Log("PauseForSuspension() called.");
        PlayerManager.GetMainPlayer().ps.OpenPauseScreen();

        //PAUSE ANY OTHER FUNCTIONS TO STOP/PAUSE
    }
    public static void UnpauseFromSuspension()
    {
        if (singleton == null)
        {
            Debug.LogError("PlayerManager.PauseForSuspension() was called but singleton was null! Aborting.");
            return;
        }

        singleton._UnpauseFromSuspension();
    }

    void _UnpauseFromSuspension()
    {
        Debug.Log("UnpauseFromSuspension() called.");
        PlayerManager.GetMainPlayer().ps.ClosePauseScreen(false);

        //PAUSE ANY OTHER FUNCTIONS TO RESUME
    }
}
