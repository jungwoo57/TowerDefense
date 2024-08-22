using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TowerDataViewer : MonoBehaviour
{
    public Image imageTower;
    public TextMeshProUGUI textDamage;
    public TextMeshProUGUI textRate;
    public TextMeshProUGUI textRange;
    public TextMeshProUGUI textLevel;
    public TowerAttackRange towerAttackRange;
    public Button upgradeButton;
    public SystemTextViewer systemTextViewer;
    private Bullet currentTower;
    private int level;

    private void Awake()
    {
        OffPanel();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon) {
        currentTower = towerWeapon.GetComponent<Bullet>();
        gameObject.SetActive(true);
        UpdateTowerData();
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }
    public void OffPanel() {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }
    private void UpdateTowerData()
    {
        if (currentTower.WeaponType == WeaponType.Cannon || currentTower.WeaponType == WeaponType.Laser)
        {
            imageTower.rectTransform.sizeDelta = new Vector2(88, 59);
            textDamage.text = "Damage : " + currentTower.Damage + "+" + "<color=red>" + currentTower.AddedDamage + "</color>";
        }
        else if(currentTower.WeaponType == WeaponType.Slow)
        {
            imageTower.rectTransform.sizeDelta = new Vector2(59, 59);
            textDamage.text = "Slow : " + currentTower.Slow * 100 + "%";
        }
        else
        {
            imageTower.rectTransform.sizeDelta = new Vector2(59, 59);
            textDamage.text = "Buff : " + currentTower.Buff * 100 + "%";
        }
        imageTower.sprite = currentTower.TowerSprite;
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + (currentTower.level+1); 

        upgradeButton.interactable = currentTower.level < currentTower.maxLevel ? true : false;
    }
    public void OnClickEnventTowerUpgrade()
    {
        bool isSuccess = currentTower.Upgrade();
        if (isSuccess == true)
        {
            UpdateTowerData();
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            systemTextViewer.PrintText(SystemType.Money);
        }
    }
    public void OnClickEnventTowerSell() {
        currentTower.Sell();
        OffPanel();
    }
}
