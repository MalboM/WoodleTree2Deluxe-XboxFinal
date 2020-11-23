using UnityEngine;
using System.Collections;
using Users;
using Storage;
//using UnityPluginLog;

public class StorageKeyValue
{
#if UNITY_XBOXONE

    #region Static factory "PlayerPrefs" like
    static public ConnectedStorage storageInstance;

    static public StorageKeyValue SetString(string key, string value)
    {
        if (storageInstance == null) { Debug.LogError(" Create a ConnectedStorage instance first"); return null; }
        StorageKeyValue kv = new StorageKeyValue(key, value, storageInstance);
        return kv.Save();
    }
    static public StorageKeyValue GetString(string key, string defaultValue = "")
    {
        if (storageInstance == null) { Debug.LogError(" Create a ConnectedStorage instance first"); return null; }
        StorageKeyValue kv = new StorageKeyValue(key, defaultValue, storageInstance);
        return kv.Load();
    }

    static public void CheckQuota()
    {
        storageInstance.QueryRemainingQuotaAsync(delegate (ConnectedStorage storage, QueryRemainingQuotaAsyncOp op, StorageQuota quota)
        {
            Debug.LogError("REMAINING QUOTA:" + quota.TotalSize + "  C:" + quota.SizeConsumed + " SL:" + quota.SizeLeft + " TS:" + quota);
        });
        Debug.LogError("GetMaxQuotaForStorage:" + storageInstance.GetMaxQuotaForStorage);

    }


    static public StorageKeyValue DeleteKey(string key)
    {
        if (storageInstance == null) { Debug.LogError(" Create a ConnectedStorage instance first"); return null; }
        StorageKeyValue kv = new StorageKeyValue(key, "", storageInstance);
        return kv.Delete();
    }
    #endregion

    #region Events
    public delegate void StorageKeyValueEvent(StorageKeyValue keyValue);
    public event StorageKeyValueEvent OnSaveDone;
    public event StorageKeyValueEvent OnLoadDone;
    public event StorageKeyValueEvent OnSaveError;
    public event StorageKeyValueEvent OnDeleteDone;
    #endregion

    #region Properties
    private string _key;
    private string _value;    

    private ConnectedStorage _storage;

    public string key { get { return _key; } }
    public string value { get { return _value; } }

    //flags
    public bool keyNotFoundInStorage = false;
    #endregion


    #region Methods
    public StorageKeyValue(string key, string value, ConnectedStorage storage)
    {
        _key = key;
        _value = value;
        _storage = storage;
    }

    public StorageKeyValue Save()
    {
        DataMap keyValue = DataMap.Create();
        keyValue.AddOrReplaceBuffer(_key, GetBytes(_value));
        _storage.SubmitUpdatesAsync(keyValue, null, hSaveDone);

        return this;
    }

    public StorageKeyValue Load()
    {
        //Debug.LogError("STORAGE GET ASYNC "+_key); 
        _storage.GetAsync(new string[] { _key }, hLoadDone);

        return this;
    }

    public StorageKeyValue Delete()
    {
        _storage.DeleteAsync(new string[] { _key }, hDeleteDone);

        return this;
    }
    #endregion

    #region Callbacks
    public void hSaveDone(ContainerContext storage, SubmitDataMapUpdatesAsyncOp op)
    {
        bool ok = op.Success && op.Status == ConnectedStorageStatus.SUCCESS;
        Debug.LogError(key + "  [" + (ok ? "PASS" : "FAIL") + "] SAVE: Completed RESULT: [" + op.Result + "] STATUS: [" + op.Status.ToString() + "]\n");
        if (op.Success)
        {
            //Debug.LogError("["+_key+"]. SAVED: "+_value);
            if (ok) if (OnSaveDone != null) OnSaveDone(this);
        }
        else {
            CheckQuota();
            if (OnSaveError != null) OnSaveError(this);
        }
    }

    void hLoadDone(ContainerContext storage, GetDataMapViewAsyncOp op, DataMapView view)
    {
        //Debug.LogError("LOAD_DONE "); 
        bool ok = op.Success;
        //	Debug.LogError("  [" + (ok ? "PASS" : "FAIL") + "] GETASYNC: \"NewBuffer\": [" + (op.Success ? "Exists" : "NonExistent") + "] [0x" + op.Result.ToString("X") + "]\n");
        if (op.Success)
        {
            byte[] buffer = view.GetBuffer(_key);
            //Debug.LogError("["+_key+"]. LOADED: "+GetString(buffer));
            _value = GetString(buffer);
            if (OnLoadDone != null) OnLoadDone(this);
        }
        else {
            keyNotFoundInStorage = true;
            if (OnLoadDone != null) OnLoadDone(this);
        }

    }

    void hDeleteDone(ContainerContext storage, SubmitDataMapUpdatesAsyncOp op)
    {
        bool ok = op.Success && op.Status == ConnectedStorageStatus.SUCCESS;
        Debug.LogError(key + "  [" + (ok ? "PASS" : "FAIL") + "] DELETE: Completed RESULT: [" + op.Result + "] STATUS: [" + op.Status.ToString() + "]\n");
        if (OnDeleteDone != null) OnDeleteDone(this);
    }
    #endregion

    #region To Move in utils lib
    static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    static string GetString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }
    #endregion

#endif
}
