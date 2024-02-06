using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class DataMonButton : MonoBehaviour, IPointerClickHandler
{

    public DataDex.DataMonBtnDelegate AddToTeam;

    public DataDex.DataMonBtnDelegate RemoveFromTeam;

    public DataDex.DataMonBtnDelegate ClickAndDrag;
    //public UnityEvent RightClick;
    //public UnityEvent LeftClick;
    public DataMonHolder dataMonHolder;
    public bool inDataBank;
    public GameObject DataMonSummoned;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right && inDataBank)
        {
            AddToTeam(this);
            print("WHY NO WORk");
        }
        if (eventData.button == PointerEventData.InputButton.Left && inDataBank)
        {
            //ClickAndDrag(this);
        }
        if (eventData.button == PointerEventData.InputButton.Right && !inDataBank)
        {
            RemoveFromTeam(this);
        }
    }
    //public void RegisterToTeam(DataMonIndividualData _dataMon)
    //{

    //}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
