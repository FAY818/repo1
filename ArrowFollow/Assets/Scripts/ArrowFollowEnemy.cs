using System.Collections;
using UnityEngine;

public class ArrowFollowEnemy : MonoBehaviour
{
    public Transform arrows;
    public Transform target;
    Vector3 prePos = Vector3.zero;

    public float checkTime;

    void Start()
    {
        arrows.gameObject.SetActive(false);
        StartCoroutine(UpdatePos());
    }

    private IEnumerator UpdatePos()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkTime);
            SetPos();
        }
    }

    private void SetPos()
    {
        var pos = Camera.main.WorldToScreenPoint(target.transform.position);//目标坐标世界转屏幕
        Debug.Log("pos:" + pos);
        var distance = Vector3.Distance(prePos, pos);//与上一次检测的距离
        if (distance < 1) { return; }//移动距离小于一视为没有移动
        prePos = pos;
        if (0 < pos.x && pos.x < Screen.width && pos.y > 0 && pos.y < Screen.height)//目标处于屏幕外显示箭头，屏幕内隐藏箭头
        {
            arrows.gameObject.SetActive(false);
            return;
        }
        else 
        {
            arrows.gameObject.SetActive(true);
        }


        var startpos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);//屏幕中心点
        var dir = pos - startpos;//中心指向目标的向量
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;//反正切获取目标相对于屏幕中心的角度（Mathf.Rad2Deg：弧度转度数）
        //Debug.Log("angle:" + angle);
        arrows.localEulerAngles = new Vector3(0, 0, angle + 90);//箭头旋转相应的角度

        float screenAngle = (float)Screen.height / Screen.width;//屏幕高和宽的比值
        var va = Mathf.Abs(dir.y / dir.x);//目标y和x的比值
        if (va <= screenAngle)//判断敌人在屏幕的上下还是左右边
        {   
            //屏幕左右
            var length = arrows.GetComponent<RectTransform>().sizeDelta.x;////图片中心锚点到图片边缘的向量x值RectTransform.sizeDelta：与锚点的距离，如果锚点在一起，则sizeDelta与size相同

            if (pos.x < 0)
                arrows.transform.position = GetNode(pos, startpos, length * 0.5f);//屏幕的左边
            else
                arrows.transform.position = GetNode(pos, startpos, Screen.width - length * 0.5f);//屏幕的右边
        }
        else
        {
            //屏幕上下
            var length = arrows.GetComponent<RectTransform>().sizeDelta.y;//图片中心锚点到图片边缘的向量y值

            if (pos.y < 0)
                arrows.transform.position = GetNode2(pos, startpos, length * 0.5f);//屏幕的下方
            else
                arrows.transform.position = GetNode2(pos, startpos, Screen.height - length * 0.5f);//屏幕的上方
        }
        //Debug.Log(arrows.transform.position);
        Debug.Log(arrows.GetComponent<RectTransform>().sizeDelta);
    }

    /// 左右，v的取值是为了让箭头完整显示
    
    private Vector3 GetNode2(Vector3 pos, Vector3 startpos, float v)//相似三角形
    {
        pos = new Vector3(pos.x, pos.y, 0);
        Vector3 ab = pos - startpos;
        Vector3 am = ab * (Mathf.Abs(startpos.y - v) / Mathf.Abs(pos.y - startpos.y));
        Vector3 om = startpos + am;
        return om;
    }
    
    /// 上下
   
    private Vector3 GetNode(Vector3 pos, Vector3 startpos, float v)
    {
        pos = new Vector3(pos.x, pos.y, 0);
        Vector3 ab = pos - startpos;
        Vector3 am = ab * (Mathf.Abs(startpos.x - v) / Mathf.Abs(pos.x - startpos.x));
        Vector3 om = startpos + am;
        return om;
    }
}

