using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereTrigger : MonoBehaviour {

    public int levelID;
    public AtmosphereManager atmosphereManager;

    public enum TriggerType { cube, entry, exit };
    public TriggerType triggerType;

    public string levelName;

    bool insideThisAlready;

    private void Start()
    {
        if (atmosphereManager == null)
        {
            atmosphereManager = PlayerManager.GetMainPlayer().ps.atmosphereManager;
            if (atmosphereManager == null)
                atmosphereManager = this.GetComponentInParent<AtmosphereManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!insideThisAlready && triggerType != TriggerType.exit && (levelName == "" || atmosphereManager.curLevel != levelName))
        {
            if (other.gameObject == PlayerManager.GetMainPlayer().gameObject)
            {
                atmosphereManager.EnterTrigger(levelID);
                if (triggerType == TriggerType.entry)
                    atmosphereManager.curLevel = levelName;
                else
                    insideThisAlready = true;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (triggerType == TriggerType.cube || (triggerType == TriggerType.exit && (levelName == "" || atmosphereManager.curLevel == levelName)))
        {
            if (other.gameObject == PlayerManager.GetMainPlayer().gameObject)
            {
                atmosphereManager.ExitTrigger();
                if (triggerType == TriggerType.exit)
                    atmosphereManager.curLevel = "";
                if (insideThisAlready)
                    insideThisAlready = false;
            }
        }
    }
}
