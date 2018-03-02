using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadData : MonoBehaviour {
    public GameObject LoadingCanvas;
    public GameObject PlayerPrefab, WolfPrefab, EnemyPrefab;
    public GameObject Wolf;
    public List<int> WolfState;
    public List<Vector3> WolfVector3;
    public List<Quaternion> WolfQuaternion;
    public List<int> EnemyState;
    public List<Vector3> EnemyVector3;
    public List<Quaternion> EnemyQuaternion;
    public string PlayerState;
    public Vector3 PlayerVector3;
    public Quaternion PlayerQuaternion;
    public float SaveRotx, SaveRoty;
    public Slider LoadingSlider;
    private AsyncOperation _async;
    public string LoadSelectedData;
    private CameraScript CameraScript;

    // Use this for initialization
    void Awake () {
        SpawnAllObject();
    }
    private void Start()
    {
        CameraScript = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        CameraScript.rotX = SaveRotx;
        CameraScript.rotY = SaveRoty;
    }

    // Update is called once per frame
    void Update () {

    }

    public void LoadSence()
    {
        Instantiate(LoadingCanvas, Vector2.zero, Quaternion.identity).name = "LoadingCanvas";
        StartCoroutine(LoadLevelWithBar("Game"));
    }

    IEnumerator LoadLevelWithBar(string level)
    {
        _async = Application.LoadLevelAsync(level);
        while (!_async.isDone)
        {
            LoadingSlider.value = _async.progress;
            yield return null;
        }            

    }
    public  void SpawnAllObject()
    {
        if (File.Exists(Application.persistentDataPath + @"\Save\NewGame.sav"))
            LoadSelectedData = GameObject.Find("ChooseSaveData").GetComponent<ChooseSaveData>().SelectedData;
        else
        {
            LoadSelectedData = "NewGame";//測試用
            StreamWriter streamWriter = File.CreateText(Application.persistentDataPath + @"\Save\NewGame.sav");
            streamWriter.Write("spyWmyJSqjGq+Otk8NxMux4BVoibDvNZmOVkIUOQD6OwwAeDpjZnv/yfyHmO843kuhRN9Nzox/CPnSwQSMW8kvfrUMyi/YVtVR4ckWtqJlkgDpik5YL1hox+GEu6kBAUs0/ji4gKUtGugNTRZumLqAJ1vRwN9QnSzmSW1yh0mcjJBQUEXJvSlsiBNLNJADvdA+B6kb+jwl37WgmlgvVd2puNg+z1tbubcZ8N7MmgkHDvfFNdk0uWmY4IsfoSRlbve4INqRyOssGiqvKrzc3OYrgB5rzhqgcn+H0kfcRNDFWuHbrcKLzRTnBxjYIDkpn7ZqezFTMU4UhPYZGyFIQEaPAHMszQK/lHyEoybGKT4J5S/Ly0ptysECaZEJAE8ObB2MJvbR3gXH6FWezwAVtQdv69tvGeShYMS7Jnbn1NPvZ3RTo/hwTkhEVoHg7E/XS/cmdJzGNkSClk4hhmGVE9T/5xOarCpAxSjemloY583CMQjTJ6jKEX6N3hp10k6EDAMmJSQ8K4RDm0JxoOnv+aLUdDhKngZEdr5TNv2yD8yjWhngyvlDbWsCMmja6owjGxYGN7syrCNtsrnPn4RZfNRxuHe9qDEcVoHENwQ++SqR05Ngm5CUXGOJ0GBOL26YezZGZYInWi+X2b4u9bE0M2qWdBQA2PkEG8qbR6FP06FZ2noF4jHC7dR4Ocg87/MpnwLiUJn46J4g3LUXWK6WIjAfAHMszQK/lHyEoybGKT4J5S/Ly0ptysECaZEJAE8ObB2MJvbR3gXH6FWezwAVtQdv69tvGeShYMS7Jnbn1NPvZ3RTo/hwTkhEVoHg7E/XS/cmdJzGNkSClk4hhmGVE9T/5xOarCpAxSjemloY583CMQjTJ6jKEX6N3hp10k6EDAMmJSQ8K4RDm0JxoOnv+aLRf7Al1XOErx7nXmwCkrGnklhtntoaZU/H7CIf2ULEMpJLBDEOL9uBijddSt3hkPJ1z6Xd8Dd6qc+8+YgSTdqxruc87GWKwhDNm4asrI5PixJuMWlLuPWUT7YYy5RbeVLJOWSwmIccmNnfzM00reYHxeDbpBoxXmaTfmV5yswgEZ1kxi6Rg00ptNlNqRWAq5uwNZOEMQK8baIkofRFAvxUfX3unIu1+cSa7NzZD2oBWdkO/7Gw9jc89mc/yHKfkcI8+M6DSkd1bhzH3PHXHDIbM=");
            streamWriter.Close();
        }
        SaveData.Data Load = (SaveData.Data)IOHelper.GetData(Application.persistentDataPath+ @"\Save\"+ LoadSelectedData + ".sav", typeof(SaveData.Data));
        Debug.Log("讀取了" + LoadSelectedData);
        PlayerState = Load.PlayerState;
        PlayerVector3 = Load.PlayerVector3;
        PlayerQuaternion = Load.PlayerQuaternion;
        SaveRotx = Load.SaveRotx;
        SaveRoty = Load.SaveRoty;

        Instantiate(PlayerPrefab, PlayerVector3, PlayerQuaternion).name = "Pine";
        Debug.Log("讀取了派恩的位置,座標為" + PlayerVector3);
        for (int A =0 ; A< Load.WolfState.Count; A++)     //讀取動物數據
        {
            WolfState.Add(Load.WolfState[A]);           //讀取動物狀態
            WolfVector3.Add(Load.WolfVector3[A]);       //讀取動物座標
            WolfQuaternion.Add(Load.WolfQuaternion[A]); //讀取動物旋轉角度
            if (WolfState[A]==1)                          //如果動物活著(WolfState=1)才生成
            {
                Instantiate(WolfPrefab, WolfVector3[A], WolfQuaternion[A]).name="Wolf"+A;
                Debug.Log("讀取了第" + (A + 1) + "隻狼," + "狀態為" + WolfState[A] + ",座標為" + WolfVector3[A]);
            }
            if (WolfState[A] == 2)                        //如果動物被附身(WolfState=2)生成後把主角掛在狼身上
            {
                Wolf = Instantiate(WolfPrefab, GameObject.Find("Pine").transform.position, Quaternion.identity);
                GameObject.Find("Pine").transform.parent = Wolf.transform;
                GameObject.Find("Pine").SetActive(false);
                Debug.Log("讀取了第" + (A + 1) + "隻狼," + "狀態為" + WolfState[A] + ",座標為" + WolfVector3[A]);
            }
            //Debug.Log(D1.WolfVector3[A]);
            //Debug.Log("讀" + WolfVector3[A]);
        }

        for (int E = 0; E < Load.EnemyState.Count; E++)   //讀取敵人數據
        {
            EnemyState.Add(Load.EnemyState[E]);           //讀取敵人狀態
            EnemyVector3.Add(Load.EnemyVector3[E]);       //讀取敵人座標
            EnemyQuaternion.Add(Load.EnemyQuaternion[E]); //讀取敵人旋轉角度
            if (EnemyState[E] == 1)                       //如果敵人活著(EnemyState=1)才生成
            {
                Instantiate(EnemyPrefab, EnemyVector3[E], EnemyQuaternion[E]).name = "Enemy" + E;
                Debug.Log("讀取了第" + (E + 1) + "個敵人," + "狀態為" + EnemyState[E] + ",座標為" + EnemyVector3[E]);
            }
        }



    }

}
