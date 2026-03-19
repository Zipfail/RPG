using UnityEngine;

public class Pic_Up_weapoint : MonoBehaviour
{
    [SerializeField] private Transform camer;
    [SerializeField] private float distance = 15f;
    private GameObject currentWeapon;
    [SerializeField] private bool canPickUp;
    [SerializeField]private Transform position;
    [SerializeField]private Vector3 povorot;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) PicUp();
        if (Input.GetKeyDown(KeyCode.Q)) Drop();
    }

    void PicUp()
    {
        RaycastHit hit;

        if (Physics.Raycast(camer.position, camer.forward, out hit, distance))
        {
            if (hit.transform.tag == "Weapon")
            {
                if (canPickUp) Drop();
                currentWeapon = hit.transform.gameObject;

                if(currentWeapon.GetComponent<gn_baz_mele>()) currentWeapon.GetComponent<gn_baz_mele>().razr(true);

                currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
                currentWeapon.transform.parent = camer;
                currentWeapon.transform.localPosition = position.localPosition;
                currentWeapon.transform.localEulerAngles = povorot;

                if (currentWeapon.GetComponent<gn_baz_mele>()) currentWeapon.transform.localPosition = currentWeapon.GetComponent<gn_baz_mele>().posUp;
                if (currentWeapon.GetComponent<gn_baz_mele>()) currentWeapon.transform.localEulerAngles = currentWeapon.GetComponent<gn_baz_mele>().rotation;

                canPickUp = true;
            }
        }

    }

    void Drop()
    {
        currentWeapon.transform.parent = null;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        canPickUp = false;
        if (currentWeapon.GetComponent<gn_baz_mele>()) currentWeapon.GetComponent<gn_baz_mele>().razr(false);
        currentWeapon = null;
    }
}
