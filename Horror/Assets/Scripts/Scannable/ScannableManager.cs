using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannableManager : MonoBehaviour
{
    private static ScannableManager instance;

    public Material overlayInstance;
    private ScannableData[] allScannableData;
    public static Dictionary<ScannableData, bool> allScannableDic;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("ScannableManager: ustawiono instancjê i nie niszczê obiektu");
        }
        else if (instance != this)
        {
            Debug.Log("ScannableManager: duplikat wykryty, niszczê obiekt");
            Destroy(gameObject);
            return;
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

        Scannable[] scannables = FindObjectsOfType<Scannable>();

        foreach (var scannable in scannables)
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
        if (!allScannableDic.ContainsKey(scannable.Data))
            allScannableDic.Add(scannable.Data, true);
        else
            allScannableDic[scannable.Data] = true;
    }

}
