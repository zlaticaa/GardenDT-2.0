using UnityEngine;
using TMPro;


public class PillarUI : MonoBehaviour
{
    public PillarMath pillarMath;

    public TextMeshProUGUI pillar1Text;
    public TextMeshProUGUI pillar2Text;
    public TextMeshProUGUI pillar3Text;
    public TextMeshProUGUI pillar4Text;

    private void Start()
    {
        // Wait until PillarMath has calculated values
        UpdateUI();
    }



    public void UpdateUI()
    {
        if (pillarMath == null) return;

        float p1 = Mathf.Min(pillarMath.Pillar1Score, 10f);
        float p2 = Mathf.Min(pillarMath.Pillar2Score, 10f);
        float p3 = Mathf.Min(pillarMath.Pillar3Score, 10f);
        float p4 = Mathf.Min(pillarMath.Pillar4Score, 10f);

        pillar1Text.text = p1.ToString("F2");
        pillar2Text.text = p2.ToString("F2");
        pillar3Text.text = p3.ToString("F2");
        pillar4Text.text = p4.ToString("F2");
    }
}
