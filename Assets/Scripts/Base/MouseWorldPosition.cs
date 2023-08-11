using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseWorldPosition
{
    /***** 이하의 내용은 마우스 월드 위치 반환 함수 **건드리지 않는편이 좋음 *****/
    ///////////////////////////////////////////////////
    // 전역 클래스
    public static Vector3 GetMouseWorldPostion()
    {
        Vector3 vec = GetMouseWorldPostionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPostionWithZ()
    {
        return GetMouseWorldPostionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPostionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPostionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPostionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    ///////////////////////////////////////////////////
}