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

        pillar1Text.text = pillarMath.Pillar1Score.ToString("F2");
        pillar2Text.text = pillarMath.Pillar2Score.ToString("F2");
        pillar3Text.text = pillarMath.Pillar3Score.ToString("F2");
        pillar4Text.text = pillarMath.Pillar4Score.ToString("F2");
    }
}
