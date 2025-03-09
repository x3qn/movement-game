using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string weaponName;   // Name der Waffe
    public GameObject weaponObject;   // Das GameObject der Waffe (Waffe als Objekt)
    public bool isActive;      // Gibt an, ob die Waffe aktuell aktiv ist

    public void SetActive(bool active)
    {
        isActive = active;
        weaponObject.SetActive(active);  // Aktiviert oder deaktiviert die Waffe
    }
}
