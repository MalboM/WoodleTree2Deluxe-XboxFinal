using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PS4Manager
{
#if UNITY_PS4
    public static PS4StorageManager ps4StorageManager;
    public static PS4Trophy ps4TrophyManager;
    public static LoadingScreen loadingScreen;
#endif
}
