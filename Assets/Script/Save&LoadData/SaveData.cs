using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveData : MonoBehaviour
{
    public GameObject[] Wolf, Enemy;
    private PossessedSystem PossessedSystem;
    private CameraScript CameraScript;
    public string filename;

    public class Data
    {
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
    }

    void Start()
    {
        PossessedSystem = GameObject.Find("Pine").GetComponent<PossessedSystem>();
        CameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
    }


    public void SaveDataValue(string SaveDataName)//按鈕給字串
    {
        Data Save = new Data();
        //定义存档路径
        string dirpath = Application.persistentDataPath + @"\Save\";
        //创建存档文件夹
        IOHelper.CreateDirectory(dirpath);
        //定义存档文件路径
        filename = dirpath+ SaveDataName + ".sav";
        //儲存檔案
        Save.PlayerState = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().NowType;
        Debug.Log("主角狀態為" + Save.PlayerState);
        if (GameObject.FindGameObjectsWithTag("WolfMaster")[0] != null)//抓現在所有狼
            Wolf = GameObject.FindGameObjectsWithTag("WolfMaster");
        if (GameObject.FindGameObjectsWithTag("Enemy1")[0] != null) //抓現在所有敵人
            Enemy = GameObject.FindGameObjectsWithTag("Enemy1");
        Save.WolfState = new List<int> { };                       //創新的List用來存數值
        Save.WolfVector3 = new List<Vector3> { };                 //創新的List用來存數值
        Save.WolfQuaternion = new List<Quaternion> { };           //創新的List用來存數值
        Save.EnemyState = new List<int> { };                      //創新的List用來存數值
        Save.EnemyVector3 = new List<Vector3> { };                //創新的List用來存數值
        Save.EnemyQuaternion = new List<Quaternion> { };          //創新的List用來存數值
        for (int A = 0; A < Wolf.Length; A++)                     //抓動物
        {
            Save.WolfState.Add(1);                                //抓動物狀態 1為正常
            Save.WolfVector3.Add(Wolf[A].transform.position);     //抓動物座標
            Save.WolfQuaternion.Add(Wolf[A].transform.rotation);  //抓動物旋轉角度
            Debug.Log("保存了第" + (A + 1) + "隻狼," + "狀態為" + Save.WolfState[A] + "座標為" + Save.WolfVector3[A]);
        }
        for (int E = 0; E < Enemy.Length; E++)                    //抓敵人
        {
            Save.EnemyState.Add(1);                               //抓敵人狀態
            Save.EnemyVector3.Add(Enemy[E].transform.position);   //抓敵人座標
            Save.EnemyQuaternion.Add(Enemy[E].transform.rotation);//抓敵人旋轉角度
            Debug.Log("保存了第" + (E + 1) + "個敵人," + "狀態為" + Save.EnemyState[E] + "座標為" + Save.EnemyVector3[E]);
        }
        if (GameObject.Find("Pine"))
        {
            Save.PlayerVector3 = GameObject.Find("Pine").transform.position;
            Save.PlayerQuaternion = GameObject.Find("Pine").transform.rotation;
        }
        else//如果主角在附身狀態 則抓取附身的動物數值
        {
            Save.PlayerVector3 = GameObject.FindGameObjectWithTag("Player").transform.position;
            Save.PlayerQuaternion = GameObject.FindGameObjectWithTag("Player").transform.rotation;
            switch (Save.PlayerState)
            {
                case "Wolf":
                    Save.WolfState.Add(2);                              //抓動物狀態 2為被附身
                    Save.WolfVector3.Add(GameObject.FindGameObjectWithTag("Player").transform.position);   //抓動物座標
                    Save.WolfQuaternion.Add(GameObject.FindGameObjectWithTag("Player").transform.rotation);//抓動物旋轉角度
                    Debug.Log("保存了最後一隻狼," + "狀態為被附身,座標為" + Save.WolfVector3[Wolf.Length]);
                    break;
            }

        }
        Debug.Log("保存了派恩的位置,座標為" + Save.PlayerVector3 + "狀態為" + Save.PlayerState);
        //保存数据
        Save.SaveRotx = CameraScript.rotX;
        Save.SaveRoty = CameraScript.rotY;
        IOHelper.SetData(filename, Save);
        Debug.Log("保存了" + SaveDataName);
        Debug.Log("存檔路徑為" + filename);
        }

}
