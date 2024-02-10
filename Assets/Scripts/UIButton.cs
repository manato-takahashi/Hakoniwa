using UnityEngine;
using UnityEngine.EventSystems; // EventSystemsを使用するために必要

public class ButtonClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // ここでボタンがクリックされたときの処理を行い、
        // イベントの伝搬を止めます。

        // eventDataを使用してさらに情報を取得したり、
        // 処理を行ったりできます。
        // 例えば、どのボタンがクリックされたかを判断することもできます。

        // このイベントを「使用済み」とマークすることで、
        // シーン内の他のオブジェクトには影響を与えないようにします。
        eventData.Use();
    }
}
