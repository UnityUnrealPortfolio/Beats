using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Beats
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    [RequireComponent(typeof(ContentSizeFitter))]
    [RequireComponent(typeof(RectTransform))] 

    public class TrackView : MonoBehaviour
    {
        public enum Trigger { Missed,Right,Wrong}

        //[SerializeField]Track _track;
        [SerializeField] RectTransform _left;
        [SerializeField] RectTransform _right;
        [SerializeField] RectTransform _up;
        [SerializeField] RectTransform _down;
        [SerializeField] RectTransform _empty;
        
        RectTransform _rTransform;
        Vector2 _trackViewPos;
        public float TrackViewYPos
        {
            get => _trackViewPos.y;
            set
            {
                if(_trackViewPos.y != value)
                {
                    _trackViewPos.y = value;
                }
                _rTransform.anchoredPosition = _trackViewPos;
            }
        }

        private float _beatViewSize;
        private float _spacing;

        List<Image> _beatViews;

        public void Init(Track track)
        {
            _beatViews = new List<Image>();
            _rTransform = (RectTransform)transform;
            _trackViewPos = _rTransform.anchoredPosition;

            _beatViewSize = _empty.rect.height;
            _spacing = GetComponent<VerticalLayoutGroup>().spacing;

            foreach(var b in track.beats)
            {
                GameObject g;
                switch (b)
                {
                    case 0:
                        g = _left.gameObject;
                        break;
                    case 1:
                        g = _down.gameObject;
                        break;
                    case 2:
                        g = _up.gameObject;
                        break;
                    case 3:
                        g = _right.gameObject;
                        break;
                    default:
                        g = _empty.gameObject;
                        break;
                }
                var obj = Instantiate(g, transform).GetComponent<Image>();
                obj.transform.SetAsFirstSibling();
                _beatViews.Add(obj);
            }
        }

        private void Start()
        {
            Init(GameplayController.Instance.track);
        }

        private void Update()
        {
            TrackViewYPos -=(_beatViewSize + _spacing)* Time.deltaTime * GameplayController.Instance.SecondsPaBeat;
        }

        public void TriggerView(int index,Trigger trigger)
        {
            switch (trigger)
            {
                case Trigger.Missed:
                    _beatViews[index].color = Color.gray;
                    break;
                case Trigger.Right:
                    _beatViews[index].color = Color.yellow;
                    break;
                case Trigger.Wrong:
                    _beatViews[index].color = Color.cyan;
                    break;
            }

        }
    }
}
