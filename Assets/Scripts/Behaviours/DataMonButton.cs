using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class DataMonButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    //public UnityEvent RightClick;
    //public UnityEvent LeftClick;
    public DataMonHolder dataMonHolder;
    public bool inDataBank;
    public GameObject DataMonSummoned;
    UnityEngine.UI.Image image;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            DataDex.instance.RightClick(this);
        }
        if (eventData.button == PointerEventData.InputButton.Left && inDataBank)
        {
            //ClickAndDrag(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DataInspector.DataMonHovering = dataMonHolder;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DataInspector.DataMonHovering = null;

    }

    //public void RegisterToTeam(DataMonIndividualData DataMons)
    //{

    //}
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inDataBank)
        {
            dataMonHolder.dataMonCurrentAttributes.CurrentHealth += Time.deltaTime * GameManager.instance.DataMonInDataDexHPRegen;
            dataMonHolder.dataMonCurrentAttributes.CurrentHealth = Mathf.Clamp(dataMonHolder.dataMonCurrentAttributes.CurrentHealth, -1, dataMonHolder.dataMon.BaseAttributes.BaseHealth);
        }
        if (dataMonHolder == null)
            return;
        if(!inDataBank && dataMonHolder.dataMonData !=null && DataMonSummoned == null && image.color != Color.blue)
        {
            image.color = Color.blue;
            dataMonHolder.dataMonCurrentAttributes.CurrentHealth = 0;
        }
    }
    private void OnDestroy()
    {
        DataInspector.DataMonHovering = null;

    }
}
