using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject goodItem;
    public GameObject badItem;

    [Range(10, 50)]
    public int goodItemCount = 30;
    [Range(10, 50)]
    public int badItemCount = 20;

    public List<GameObject> goodItemList = new List<GameObject>();
    public List<GameObject> badItemList = new List<GameObject>();

    public void InitStage()
    {
        // 기존에 생성된 아이템 삭제
        foreach (var obj in goodItemList)
        {
            Destroy(obj);
        }
        goodItemList.Clear();

        foreach (var obj in badItemList)
        {
            Destroy(obj);
        }
        badItemList.Clear();

        // GoodItem 생성
        for (int i = 0; i < goodItemCount; i++)
        {
            // 불규칙한 위치 생성
            Vector3 pos = new Vector3(Random.Range(-23.0f, 23.0f)
                                    , 0.05f
                                    , Random.Range(-23.0f, 23.0f));

            // 불규칙한 회전값 생성
            Quaternion rot = Quaternion.Euler(Vector3.up * Random.Range(0, 360));

            goodItemList.Add(Instantiate(goodItem, transform.position + pos, rot, transform));
        }

        // BadItem 생성
        for (int i = 0; i < badItemCount; i++)
        {
            // 불규칙한 위치 생성
            Vector3 pos = new Vector3(Random.Range(-23.0f, 23.0f)
                                    , 0.05f
                                    , Random.Range(-23.0f, 23.0f));

            // 불규칙한 회전값 생성
            Quaternion rot = Quaternion.Euler(Vector3.up * Random.Range(0, 360));

            badItemList.Add(Instantiate(badItem, transform.position + pos, rot, transform));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitStage();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
