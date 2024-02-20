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
    public IndividualDataMon.DataMon dataMon;
    public UnityEngine.UI.Image image;
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
    float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (dataMonHolder == null)
            return;
        if (timer>= (inDataBank ? 1 : 3))
        {
            timer = 0;
            dataMonHolder.dataMonCurrentAttributes.CurrentHealth += GameManager.instance.DataMonInDataDexHPRegen;
            dataMonHolder.dataMonCurrentAttributes.CurrentHealth = Mathf.Clamp(dataMonHolder.dataMonCurrentAttributes.CurrentHealth, -1, dataMonHolder.dataMonBaseAttributes.BaseHealth);
        }
        if (!inDataBank && dataMonHolder.dataMonData != null && DataMonSummoned == null && image.color != Color.blue && image.color != Color.green)
        {
            image.color = Color.blue;
            dataMonHolder.dataMonCurrentAttributes.CurrentHealth = 0;
        }
        if (!inDataBank && dataMonHolder.dataMonData != null &&
            DataDex.instance.datamonEvolution.DataMonToEvolve != null && image.color == Color.green && DataMonSummoned != null)
        {
            Destroy(DataMonSummoned);
        }
        if (!inDataBank && dataMonHolder.dataMonData != null &&
            DataDex.instance.datamonEvolution.DataMonToEvolve == null && image.color == Color.green && DataMonSummoned == null)
        {
            DataMonSummoned = DataDex.instance.SpawnCompanionDataMon(this);
            image.color = DataDex.instance.Testcolor;
        }

    }
    private void OnDestroy()
    {
        DataInspector.DataMonHovering = null;
        if (inDataBank)
        {
            DataDex.instance.DataMonObtained.Remove(gameObject);
            //DataDex.instance.DataMonObtained = DataDex.instance.DataMonObtained.RemoveNullReferencesInList();

        }
    }
}
