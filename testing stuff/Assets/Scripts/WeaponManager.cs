using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Waffen-Konfiguration")]
    public Weapon[] weapons; // Array, das alle Waffen speichert

    [Header("Hotkeys")]
    public KeyCode[] weaponHotkeys; // Array der Hotkeys, um zwischen Waffen zu wechseln

    private int currentWeaponIndex = -1; // Aktuell aktive Waffe

    void Start()
    {
        // Deaktiviere alle Waffen zu Beginn
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }

        // Wähle die erste Waffe zu Beginn
        if (weapons.Length > 0)
        {
            SwitchWeapon(0);
        }
    }

    void Update()
    {
        // Gehe durch alle Hotkeys und wechsle die Waffe
        for (int i = 0; i < weaponHotkeys.Length; i++)
        {
            if (Input.GetKeyDown(weaponHotkeys[i]))
            {
                SwitchWeapon(i);
            }
        }
    }

    void SwitchWeapon(int weaponIndex)
    {
        // Überprüfe, ob der Index innerhalb der Array-Grenzen liegt
        if (weaponIndex < 0 || weaponIndex >= weapons.Length) return;

        // Deaktiviere die derzeit aktive Waffe
        if (currentWeaponIndex != -1)
        {
            weapons[currentWeaponIndex].SetActive(false);
        }

        // Aktiviere die neue Waffe
        weapons[weaponIndex].SetActive(true);

        // Setze den aktuellen Waffenschalter
        currentWeaponIndex = weaponIndex;
    }
}
