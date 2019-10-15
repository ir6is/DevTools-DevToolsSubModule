using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityDevTools.Console
{
	public class DragWhenClose : MonoBehaviour, IDragHandler
	{
		public void OnDrag(PointerEventData eventData)
		{
			//поправка на смещение
			transform.position = eventData.position;
		}
	}
}