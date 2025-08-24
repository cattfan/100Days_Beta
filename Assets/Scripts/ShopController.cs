using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ShopController : MonoBehaviour
{
    public List<ShopItem> danhSachVatPham;
    public Transform npcTransform;
    public TextMeshProUGUI thongBaoText;

    public int soTienCuaNguoiChoi = 1000;
    private Transform lastSpawnedItemTransform;

    // Thêm biến Text mới để hiển thị tiền
    public TextMeshProUGUI moneyText;

    void Start()
    {
        // Cập nhật số tiền ban đầu khi game bắt đầu
        CapNhatGiaoDienTien();
    }

    public void MuaVatPham(int idVatPham)
    {
        if (idVatPham >= 0 && idVatPham < danhSachVatPham.Count)
        {
            ShopItem vatPham = danhSachVatPham[idVatPham];

            if (soTienCuaNguoiChoi >= vatPham.giaTien)
            {
                soTienCuaNguoiChoi -= vatPham.giaTien;

                Vector3 viTriDatXe;
                float khoangCachX = 0f;

                if (lastSpawnedItemTransform != null)
                {
                    Bounds lastBounds = GetBounds(lastSpawnedItemTransform);
                    khoangCachX = lastBounds.size.x;
                }

                viTriDatXe = (lastSpawnedItemTransform == null)
                    ? new Vector3(npcTransform.position.x + 3, npcTransform.position.y, npcTransform.position.z)
                    : lastSpawnedItemTransform.position + new Vector3(khoangCachX + 0.5f, 0, 0);

                GameObject newVehicle = Instantiate(vatPham.prefabVatPham, viTriDatXe, Quaternion.identity);
                lastSpawnedItemTransform = newVehicle.transform;

                if (vatPham.uiPanel != null)
                {
                    vatPham.uiPanel.SetActive(false);
                }

                // Gọi hàm cập nhật tiền sau khi mua thành công
                CapNhatGiaoDienTien();

                HienThiThongBao("Bạn đã nhận được " + vatPham.tenVatPham + "!");
                Debug.Log("Đã mua " + vatPham.tenVatPham + " thành công! Tiền còn lại: " + soTienCuaNguoiChoi);
            }
            else
            {
                HienThiThongBao("Không đủ tiền để mua " + vatPham.tenVatPham + "!");
                Debug.Log("Không đủ tiền.");
            }
        }
    }

    // Hàm mới để cập nhật hiển thị số tiền
    private void CapNhatGiaoDienTien()
    {
        if (moneyText != null)
        {
            moneyText.text = "Tiền: " + soTienCuaNguoiChoi;
        }
    }

    private void HienThiThongBao(string noiDung)
    {
        if (thongBaoText != null)
        {
            thongBaoText.text = noiDung;
        }
    }

    private Bounds GetBounds(Transform objTransform)
    {
        Bounds combinedBounds = new Bounds(objTransform.position, Vector3.zero);
        Renderer[] renderers = objTransform.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            combinedBounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                combinedBounds.Encapsulate(renderers[i].bounds);
            }
        }
        else
        {
            Collider[] colliders = objTransform.GetComponentsInChildren<Collider>();
            if (colliders.Length > 0)
            {
                combinedBounds = colliders[0].bounds;
                for (int i = 1; i < colliders.Length; i++)
                {
                    combinedBounds.Encapsulate(colliders[i].bounds);
                }
            }
            else
            {
                combinedBounds.Encapsulate(objTransform.position);
            }
        }
        return combinedBounds;
    }
}