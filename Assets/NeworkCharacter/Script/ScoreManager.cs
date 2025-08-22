using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager Instance;
    private NetworkVariable<int> global_score = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private TextMeshProUGUI score_text_UI;

    void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        global_score.OnValueChanged += OnScoreChanged;
    
    }

    private void OnScoreChanged(int prev_value, int new_value)
    {
        score_text_UI.text = new_value.ToString();
    }

    public void AddScore()
    {
        if (!IsServer)
            return;

        this.global_score.Value++;
    }
}
