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
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            AddToTeam(this);
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ClickAndDrag(this);
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
