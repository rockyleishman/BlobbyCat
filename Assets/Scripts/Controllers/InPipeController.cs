using UnityEngine;

[RequireComponent(typeof(ActionTrigger))]
public class InPipeController : MonoBehaviour
{
    [SerializeField] public OutPipe OutPipe;

    private void Start()
    {
        GetComponent<ActionTrigger>().ShowUIPrompt();
    }

    public void GoIn()
    {
        if (DataManager.Instance.GameStatusObject.unlockedLiquidCat)
        {
            GameManager.Instance.LiquidCatPipeTeleport(this);
        }
    }
}