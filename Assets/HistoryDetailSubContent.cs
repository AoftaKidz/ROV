using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryDetailSubContent : MonoBehaviour
{
    public HistoryModelCombo combo;
    [SerializeField] GameObject content;
    public GameObject prefabCell;
    List<GameObject> items = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateData();
    }
    void CreateData()
    {
        if(combo == null || combo.lines == null || combo.lines.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < combo.lines.Count;i++)
        {
            HistoryDetailSubCombo cb = new HistoryDetailSubCombo();
            cb.lineID = combo.lines[i];
            cb.reward = combo.lineRewards[i];
            cb.data = combo.data;
            cb.match = combo.matches[i];
            cb.subReward = 0;

            GameObject it = Instantiate(prefabCell,Vector3.zero,Quaternion.identity);
            HistoryDetailSubContentCell cell = it.GetComponent<HistoryDetailSubContentCell>();
            cell.combo = cb;
            it.transform.SetParent(content.transform, false);

            //items.Add(it);
        }
    }
    
}
