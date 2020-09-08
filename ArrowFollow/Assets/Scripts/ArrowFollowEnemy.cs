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
        var pos = Camera.main.WorldToScreenPoint(target.transform.position);//Ŀ����������ת��Ļ
        Debug.Log("pos:" + pos);
        var distance = Vector3.Distance(prePos, pos);//����һ�μ��ľ���
        if (distance < 1) { return; }//�ƶ�����С��һ��Ϊû���ƶ�
        prePos = pos;
        if (0 < pos.x && pos.x < Screen.width && pos.y > 0 && pos.y < Screen.height)//Ŀ�괦����Ļ����ʾ��ͷ����Ļ�����ؼ�ͷ
        {
            arrows.gameObject.SetActive(false);
            return;
        }
        else 
        {
            arrows.gameObject.SetActive(true);
        }


        var startpos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);//��Ļ���ĵ�
        var dir = pos - startpos;//����ָ��Ŀ�������
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;//�����л�ȡĿ���������Ļ���ĵĽǶȣ�Mathf.Rad2Deg������ת������
        //Debug.Log("angle:" + angle);
        arrows.localEulerAngles = new Vector3(0, 0, angle + 90);//��ͷ��ת��Ӧ�ĽǶ�

        float screenAngle = (float)Screen.height / Screen.width;//��Ļ�ߺͿ�ı�ֵ
        var va = Mathf.Abs(dir.y / dir.x);//Ŀ��y��x�ı�ֵ
        if (va <= screenAngle)//�жϵ�������Ļ�����»������ұ�
        {   
            //��Ļ����
            var length = arrows.GetComponent<RectTransform>().sizeDelta.x;////ͼƬ����ê�㵽ͼƬ��Ե������xֵRectTransform.sizeDelta����ê��ľ��룬���ê����һ����sizeDelta��size��ͬ

            if (pos.x < 0)
                arrows.transform.position = GetNode(pos, startpos, length * 0.5f);//��Ļ�����
            else
                arrows.transform.position = GetNode(pos, startpos, Screen.width - length * 0.5f);//��Ļ���ұ�
        }
        else
        {
            //��Ļ����
            var length = arrows.GetComponent<RectTransform>().sizeDelta.y;//ͼƬ����ê�㵽ͼƬ��Ե������yֵ

            if (pos.y < 0)
                arrows.transform.position = GetNode2(pos, startpos, length * 0.5f);//��Ļ���·�
            else
                arrows.transform.position = GetNode2(pos, startpos, Screen.height - length * 0.5f);//��Ļ���Ϸ�
        }
        //Debug.Log(arrows.transform.position);
        Debug.Log(arrows.GetComponent<RectTransform>().sizeDelta);
    }

    /// ���ң�v��ȡֵ��Ϊ���ü�ͷ������ʾ
    
    private Vector3 GetNode2(Vector3 pos, Vector3 startpos, float v)//����������
    {
        pos = new Vector3(pos.x, pos.y, 0);
        Vector3 ab = pos - startpos;
        Vector3 am = ab * (Mathf.Abs(startpos.y - v) / Mathf.Abs(pos.y - startpos.y));
        Vector3 om = startpos + am;
        return om;
    }
    
    /// ����
   
    private Vector3 GetNode(Vector3 pos, Vector3 startpos, float v)
    {
        pos = new Vector3(pos.x, pos.y, 0);
        Vector3 ab = pos - startpos;
        Vector3 am = ab * (Mathf.Abs(startpos.x - v) / Mathf.Abs(pos.x - startpos.x));
        Vector3 om = startpos + am;
        return om;
    }
}

