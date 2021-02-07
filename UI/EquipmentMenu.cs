using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentMenu : MonoBehaviour
{
    private const int COLS = 6;
    private const int ROWS = 4;

    [SerializeField] private GameObject[,] grid = new GameObject[ROWS, COLS];
    [SerializeField] private GameObject selector;
    [SerializeField] private GameObject[] rowEquipped;
    [SerializeField] private GameObject[] rowCollection1;
    [SerializeField] private GameObject[] rowCollection2;
    [SerializeField] private GameObject[] rowCollection3;

    [SerializeField] private TextMeshProUGUI previewText;
    [SerializeField] private Image previewImage;
    [SerializeField] private TextMeshProUGUI previewDescription;

    private Vector2 posIndex;
    private GameObject currentSlot;
    private bool isMoving = false;
    private int equippedIndex = 0;

    //==================== PUBLIC ====================//
    public void SetPreview()
    {
        if (currentSlot.GetComponent<EquipmentSlot>().equipment != null)
        {
            Debug.Log("NOT NULL");
            previewText.text = currentSlot.GetComponent<EquipmentSlot>().equipment.name;
            previewImage.gameObject.SetActive(true);
            previewImage.sprite = currentSlot.GetComponent<EquipmentSlot>().equipment.GetComponent<Image>().sprite;
            previewDescription.text = currentSlot.GetComponent<EquipmentSlot>().equipment.GetComponent<Equipment>().description;
        }
        else
        {
            Debug.Log("NULL");
            previewText.text = "";
            previewImage.gameObject.SetActive(false);
            previewDescription.text = "";
        }
    }

    //==================== PRIVATE ====================//
    private void Start()
    {
        AddRowToGrid(0, rowEquipped);
        AddRowToGrid(1, rowCollection1);
        AddRowToGrid(2, rowCollection2);
        AddRowToGrid(3, rowCollection3);

        posIndex = new Vector2(0, 1);
        currentSlot = grid[0, 1];
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (x > 0) MoveSelector("right");
        else if (x < 0) MoveSelector("left");
        else if (y > 0) MoveSelector("up");
        else if (y < 0) MoveSelector("down");

        if (Input.GetKeyDown(KeyCode.K)) SelectEquipment();
    }

    private void AddRowToGrid(int index, GameObject[] row)
    {
        for (int i = 0; i < row.Length; i++) grid[index, i] = row[i];
    }

    private void MoveSelector(string dir)
    {
        if (!isMoving)
        {
            isMoving = true;
            if (dir == "right" && posIndex.x < COLS - 1) posIndex.x++;
            else if (dir == "left" && posIndex.x > 0) posIndex.x--;
            else if (dir == "up" && posIndex.y > 0) posIndex.y--;
            else if (dir == "down" && posIndex.y < ROWS - 1) posIndex.y++;

            currentSlot = grid[(int)posIndex.y, (int)posIndex.x];
            selector.transform.position = currentSlot.transform.position;

            SetPreview();
            Invoke("ResetMoving", 0.2f);
        }
    }

    private void SelectEquipment()
    {
        if (posIndex.y == 0 && currentSlot.GetComponent<EquipmentSlot>().equipment != null) // Unequip.
        {
            MoveEquipment(currentSlot, currentSlot.GetComponent<EquipmentSlot>().equipment.GetComponent<Equipment>().orgPos);
            ArrangeRow(rowEquipped);
            equippedIndex--;
        }
        else if (posIndex.y > 0 && currentSlot.GetComponent<EquipmentSlot>().equipment != null) // Equip.
        {
            Equipment e = currentSlot.GetComponent<EquipmentSlot>().equipment.GetComponent<Equipment>();
            if (e != null && e.collected)
            {
                e.orgPos = currentSlot.transform;
                MoveEquipment(currentSlot, rowEquipped[equippedIndex].transform);
                equippedIndex++;
            }
        }
        SetPreview();
    }

    private void ArrangeRow(GameObject[] row)
    {
        for (int i = 0; i < row.Length - 1; i++)
        {
            if (row[i].GetComponent<EquipmentSlot>().equipment == null && 
                row[i + 1].GetComponent<EquipmentSlot>().equipment != null) MoveEquipment(row[i + 1], row[i].transform);
        }
    }

    private void MoveEquipment(GameObject selected, Transform dest)
    {
        selected.GetComponent<EquipmentSlot>().equipment.transform.position = dest.position;
        dest.gameObject.GetComponent<EquipmentSlot>().equipment = selected.GetComponent<EquipmentSlot>().equipment;
        selected.GetComponent<EquipmentSlot>().equipment = null;
    }

    private void ResetMoving()
    {
        isMoving = false;
    }
}
