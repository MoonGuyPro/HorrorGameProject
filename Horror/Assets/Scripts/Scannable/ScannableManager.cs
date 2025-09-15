using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannableManager : MonoBehaviour
{
    private static ScannableManager instance;

    public Material overlayInstance;
    private ScannableData[] allScannableData;
    private Scannable[] currentScannables;
    public static Dictionary<ScannableData, bool> allScannableDic;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (allScannableDic == null)
        {
            allScannableData = Resources.LoadAll<ScannableData>("");
            allScannableDic = new Dictionary<ScannableData, bool>();

            foreach (var scannable in allScannableData)
            {
                allScannableDic[scannable] = false;
            }
        }

        currentScannables = FindObjectsOfType<Scannable>();

        foreach (var scannable in currentScannables)
        {
            if (!allScannableDic[scannable.Data])
            {
                scannable.overlayMaterial = Instantiate(overlayInstance);
                scannable.OnScanned.AddListener(() => UpdateScannable(scannable));
            }
        }
    }

    public void UpdateScannable(Scannable scannable)
    {
        if(scannable.WasScanned) return;
        if (!allScannableDic.ContainsKey(scannable.Data))
            allScannableDic.Add(scannable.Data, true);
        else
            allScannableDic[scannable.Data] = true;

        foreach (var currentSannable in currentScannables)
        {
            if(currentSannable.Data == scannable.Data)
                currentSannable.OnScannedFunc();
        }
    }

}
