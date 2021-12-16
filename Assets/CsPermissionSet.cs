using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class CsPermissionSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }

        if (Permission.HasUserAuthorizedPermission(Permission.Camera) == false)
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
    }
}
