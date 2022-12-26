using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIController : MonoBehaviour
{
    [SerializeField] private List<UIHeartSlot> heartSlots;

    [SerializeField] private GameObject heartSlotPrefab;

    private void OnEnable()
    {
        EventManager.onPlayerHealthChanged += OnPlayerHealthChanged;
    }

    private void OnDisable()
    {
        EventManager.onPlayerHealthChanged -= OnPlayerHealthChanged;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        int fullHearts = newHealth / 2;
        int maxHearts = maxHealth / 2;

        //for (int i = 0; i < heartSlots.Count; i++)
        //{
        //    Destroy(heartSlots[i].gameObject);
        //}

        for (int i = 0; i < maxHearts; i++)
        {
            if(heartSlots.Count <= i)
            {
                var heartSlot = Instantiate(heartSlotPrefab, transform).GetComponent<UIHeartSlot>();
                heartSlots.Add(heartSlot);
            }
        }

        for (int i = 0; i < heartSlots.Count; i++)
        {
            if (fullHearts > i)
            {
                heartSlots[i].SetHeartPortion(2);
            }
            else if (fullHearts == i && fullHearts != maxHearts)
            {
                heartSlots[i].SetHeartPortion(newHealth % 2);
            }
            else if (maxHearts > i)
            {
                heartSlots[i].SetHeartPortion(0);
            }
            else
            {
                heartSlots[i].SetVisibilty(false);
            }

        }
    }
    public void HealthChanged()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
