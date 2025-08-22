using UnityEngine;
using Unity.Netcode;

public class NetworkBomb : NetworkBehaviour
{

    private float timer = 0f;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        base.OnNetworkSpawn();
    }

    void Update()
    {
        this.transform.Translate(Vector3.up * 10f * Time.deltaTime);

        this.timer += Time.deltaTime;
        if (timer >= 3f)
        {
            this.timer = 0f;
            ActiveBombServerRpc();
        }
    }

    [ServerRpc]
    private void ActiveBombServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
    }

}