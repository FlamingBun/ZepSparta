using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackGameController : MonoBehaviour
{
    private const float BoundSize = 3.5f;
    private const float MovingBoundsSize = 3f;
    private const float StackMovingSpeed = 5.0f;
    private const float BlockMovingSpeed = 2.5f;
    private const float ErrorMargin = 0.1f;
    private Vector3 prevBlockPosition;
    private Vector3 desiredPosition;
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize); // 다음 블록을 생성할 사이즈
    Transform lastBlock = null;
    float blockTransition = 0f;
    float secondaryPosition = 0f;

    int stackCount = -1;
    public int Score { get { return stackCount; } }
    int comboCount = 0;
    public int Combo { get { return comboCount; } }

    private int maxCombo = 0;
    public int MaxCombo { get => maxCombo; }

    public Color prevColor;
    public Color nextColor;


    private bool isGameOver = false;


    public GameObject originBlock = null;


    private void Start()
    {
        isGameOver = false;
        if (originBlock == null)
        {
            Debug.Log("OriginBlock is NULL");
            return;
        }

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        prevBlockPosition = Vector3.down;

        Spawn_Block();
        Spawn_Block();
    }

    void Update()
    {
        if (isGameOver) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceBlock())
            {
                Spawn_Block();
            }
            else
            {
                // 게임 오버
                Debug.Log("Game Over");

                isGameOver = true;
                GameOverEffect();
                StartCoroutine(GameOver());
            }
        }

        MoveBlock();
        // 매 프레임마다 desiredPosition으로 이동하는 구문
        transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime);
    }

    bool Spawn_Block()
    {
        if (lastBlock != null)
        {
            prevBlockPosition = lastBlock.localPosition;
        }

        GameObject newBlock = null;
        Transform newTrans = null;

        newBlock = Instantiate(originBlock);

        if (newBlock == null)
        {
            Debug.Log("NewBlock Instantiate Failed");
            return false;
        }

        ColorChange(newBlock);

        newTrans = newBlock.transform;
        newTrans.parent = this.transform; // 부모 객체 지정
        newTrans.localPosition = prevBlockPosition + Vector3.up; // 한 칸 위로 올림
        newTrans.localRotation = Quaternion.identity; // 쿼터니언의 초기값
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

        stackCount++;

        desiredPosition = Vector3.down * stackCount; // 전체 움직임을 아래로 내리기 위함
        blockTransition = 0f; // 이동에 대한 처리를 위한 기준값 초기화

        lastBlock = newTrans; // lastBlock에 현재 Transform 저장

        UIManager.Instance.stackGameUI.SetScore(stackCount, comboCount);

        return true;
    }

    private Color GetRandomColor()
    {
        // 100보다 낮으면 어둡기 때문에 100부터 시작
        float r = Random.Range(120f, 250f) / 255f;
        float g = Random.Range(120f, 250f) / 255f;
        float b = Random.Range(120f, 250f) / 255f;

        return new Color(r, g, b);
    }

    public void ColorChange(GameObject go)
    {
        //  (stackCount % 11) : 0부터 10까지의 값들을 순환
        //  /10f : 0~1까지로 변경
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);

        SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.Log("Renderer is Null");
            return;
        }

        // 색상을 변경할 때는 material.color로 접근
        spriteRenderer.color = applyColor;
        // 가장 처음에 "MainCamera" tag를 가진 카메라의 정보를 가지고 있음
        Camera.main.backgroundColor = applyColor - new Color(0.2f, 0.2f, 0.2f);

        // 색상이 같아지면(stackCount가 10이 될 때마다) 다음 색상 변경 !
        if (applyColor.Equals(nextColor) == true)
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }
    }

    void MoveBlock()
    {
        // blockTransition: 이동하는 수치의 %를 가져옴
        blockTransition += Time.deltaTime * BlockMovingSpeed;

        // Mathf.PingPong(t, length): 0부터 지정한 값까지 순환 (sin과 차이는 sin은 완만한 곡선으로 순환하고 양수에서 음수까지 순환)
        // BoundSize / 2를 빼주는 이유 중심을 기준으로 블록의 사이즈만큼 왔다 갔다 하기 위해서
        float movePosition = Mathf.PingPong(blockTransition, BoundSize) - BoundSize / 2;

        // 마지막 블록을 이동
        // secondaryPosition: 각 축이 이동을 했을 때 필요로하는 다른 좌표
        lastBlock.localPosition = new Vector3(movePosition * MovingBoundsSize, stackCount, secondaryPosition);

    }

    private bool PlaceBlock()
    {
        Vector3 lastPosition = lastBlock.localPosition;

        // 이전 블록과 현재 블록의 중심 좌표의 차이
        float deltaX = prevBlockPosition.x - lastPosition.x;


        bool isNegativeNum = (deltaX < 0) ? true : false; // 어느 방향에서 Rubble이 떨어질지 정하기 위한 변수

        deltaX = Mathf.Abs(deltaX);

        // ErrorMargin보다 크면 잘못 배치된 것
        if (deltaX > ErrorMargin)
        {
            // x축 사이즈를 줄이고
            stackBounds.x -= deltaX;

            // stackBounds.x가 0보다 작으면 -> 게임 오버
            if (stackBounds.x <= 0)
            {
                return false;
            }

            // 새로운 배치를 위해 중심값을 구함
            float middle = (prevBlockPosition.x + lastPosition.x) / 2f;

            // 블록을 자르는 구문
            lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

            // 새로운 중심값을 적용하는 구문
            // 이전 블록 위에서 이탈한 만큼을 제외한 새로운 블록의 위치를 만들어주는 것 
            Vector3 tempPosition = lastBlock.localPosition;
            tempPosition.x = middle;

            // lastPosition을 tempPosition으로 변경 -> lastBlock.localPosition을 lastPosition으로 변경
            lastBlock.localPosition = lastPosition = tempPosition;

            float rubbleHalfScale = deltaX / 2f;
            // isNegativeNum ? lastPosition.x + stackBounds.x / 2 + rubbleHalfScale 
            // : lastPosition.x - stackBounds.x / 2 - rubbleHalfScale; 는 생성될 rubble의 중심점을 구하는 구문
            CreateRubble
            (
                new Vector3(
                    isNegativeNum
                    ? lastPosition.x + stackBounds.x / 2 + rubbleHalfScale
                    : lastPosition.x - stackBounds.x / 2 - rubbleHalfScale
                    , lastPosition.y
                    , lastPosition.z
                ),
                new Vector3(deltaX, 1, stackBounds.y)
            );

            comboCount = 0;
        }
        else
        {
            ComboCheck();
            // 이전 블록에서 한 칸 위로
            lastBlock.localPosition = prevBlockPosition + Vector3.up;
        }


        // 이동 방향이 어떤 방향이냐에 따라서 이동 방향의 localPosition의 x 또는 z값을 저장
        secondaryPosition = lastBlock.localPosition.x;

        return true;
    }

    // 파편을 만드는 함수
    void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject rubble = Instantiate(lastBlock.gameObject);
        rubble.transform.parent = this.transform;

        rubble.transform.localPosition = pos;
        rubble.transform.localScale = scale;
        rubble.transform.localRotation = Quaternion.identity;

        rubble.AddComponent<Rigidbody2D>();
        // 게임 오브젝트의 이름 변경
        rubble.name = "Rubble";

        StartCoroutine(DestroyRubble(rubble));
    }

    private IEnumerator DestroyRubble(GameObject rubble)
    {
        yield return new WaitForSeconds(2f);
        Destroy(rubble);
    }

    void ComboCheck()
    {
        comboCount++;

        if (comboCount > maxCombo)
            maxCombo = comboCount;

        // 5회 성공하면 크기를 키우고
        if ((comboCount % 5) == 0)
        {
            Debug.Log("5 Combo Success!");
            stackBounds += new Vector3(0.5f, 0.5f);
            // 초기값보다 커지지 않게 제한
            stackBounds.x = (stackBounds.x > BoundSize) ? BoundSize : stackBounds.x;
            stackBounds.y = (stackBounds.y > BoundSize) ? BoundSize : stackBounds.y;
        }
    }


    private void GameOverEffect()
    {
        // 자식 개수
        int childCount = this.transform.childCount;

        // 19개의 자식들을 돌면서 Rubble이 아니면 Rigidbody를 추가하고 Addforce를 통해 블록을 날려준다
        for (int i = 1; i < 20; i++)
        {
            if (childCount < i) break;

            GameObject go = transform.GetChild(childCount - i).gameObject;

            if (go.name.Equals("Rubble")) continue;

            Rigidbody2D rigid = go.AddComponent<Rigidbody2D>();

            rigid.AddForce(
                (Vector3.up * Random.Range(0, 10f) + Vector3.right * (Random.Range(0, 10f) - 5f)) * 100f
            );
        }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.StackGameOver(stackCount, maxCombo);
    }
}


