using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    private GameManager gameManager;
    public static CharacterSpawner Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void SpawnByCardData(CardData cardData, Vector3 spawnBase)
    {
        foreach (var spawnUnit in cardData.spawnUnits)
        {
            var prefab = Resources.Load<GameObject>(spawnUnit.unitPrefabPath);
            if (prefab == null)
            {
                continue;
            }
            // 소환할 유닛의 유닛 데이터 가져오기
            UnitData unitData = gameManager.GameDataManager.GetUnitById(cardData.unitId);

            // 소환 효과음 경로 찾기
            string sfxPath = unitData != null ? unitData.spawnSfxPath : null;


            // 세로 4마리씩, 5번째는 옆줄로 넘어감
            int unitsPerCol = 4;
            float spacingZ = 1.2f; // 앞뒤 간격 
            float spacingX = 1.2f; // 좌우 간격 

            for (int i = 0; i < spawnUnit.count; i++)
            {
                int col = i / unitsPerCol;    // 0: 첫열, 1: 둘째열, ...
                int row = i % unitsPerCol;    // 0~3

                // 중앙정렬
                float xOffset = col * spacingX;
                float zOffset = (row - (unitsPerCol - 1) / 2.0f) * spacingZ;

                Vector3 offset = new Vector3(xOffset, 0, zOffset);

                Vector3 spawnPos = spawnBase + offset;
                spawnPos.y += 0.1f;

                RaycastHit hit;
                if (Physics.Raycast(spawnPos + Vector3.up * 5f, Vector3.down, out hit, 20f, LayerMask.GetMask("Ground")))
                {
                    spawnPos = hit.point;
                }

                var go = Instantiate(prefab, spawnPos, Quaternion.Euler(0, -90, 0));

                var player = go.GetComponent<Player>();
                if (player != null)
                {
                    player.isRealSpawned = true;
                    player.Init();
                }
            }
            // 효과음 실제 재생
            if (!string.IsNullOrEmpty(sfxPath))
            {
                SoundManager.Instance.SetSFXVolume(0.03f);
                SoundManager.Instance.PlaySFX(sfxPath);
            }
        }
    }
}
