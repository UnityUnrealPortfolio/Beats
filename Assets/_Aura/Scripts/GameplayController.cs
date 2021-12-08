using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beats
{
    public class GameplayController : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] KeyCode _left;
        [SerializeField] KeyCode _right;
        [SerializeField] KeyCode _up;
        [SerializeField] KeyCode _down;

        [Header("Tracks")]
        [SerializeField] Track _track;

        private TrackView _trackView;
        private WaitForSeconds waitForSeconds;
        private bool _completed;
        private bool _played; //has the player pressed a key?

        /// <summary>
        /// The current track
        /// </summary>
        public Track track
        {
            get => _track;
        }

        public float SecondsPaBeat { get; private set; }
        public float BeatsPaSecond { get; private set; }

        #region Singleton Implementation
        private static GameplayController instance;
        public static GameplayController Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<GameplayController>();
                }
                return instance;
            }
            set
            {
                instance = value;
            }
        }
        #endregion

        #region Monobehaviour Methods
        private void Awake()
        {
            _trackView = FindObjectOfType<TrackView>();
            Instance = this;
            SecondsPaBeat = track.bpm / 60f;
            BeatsPaSecond = 60f / track.bpm;
            waitForSeconds = new WaitForSeconds(BeatsPaSecond * 2);
        }
        private void Start()
        {
            InvokeRepeating("NextBeat", 0f, BeatsPaSecond);
        }
        private void Update()
        {
            if (_played || _completed) return;

            if (Input.GetKeyDown(_left))
            {
                PlayBeat(0);
            }
            if (Input.GetKeyDown(_down))
            {
                PlayBeat(1);
            }
            if (Input.GetKeyDown(_up))
            {
                PlayBeat(2);
            }
            if (Input.GetKeyDown(_right))
            {
                PlayBeat(3);
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
        #endregion

        #region Gameplay
        private int _current;
        public int Current
        {
            get => _current;
            set
            {
                if (value != _current)
                {
                    _current = value;
                    if (value == track.beats.Count)
                    {
                        CancelInvoke("NextBeat");
                        _completed = true;
                        StartCoroutine(WaitAndStop());
                    }
                }
            }
        }

        private void PlayBeat(int input)
        {
            _played = true;
            if(_track.beats[Current]== -1)
            {
                //played a beat we should have just watched
                Debug.Log("Untimely play");
 
            }
            else if (_track.beats[Current] == input)
            {
                //played correctly
                _trackView.TriggerView(Current, TrackView.Trigger.Right);
            }
            else
            {
                //we missed
                _trackView.TriggerView(Current, TrackView.Trigger.Wrong);
            }
        }

        private void NextBeat()
        {
            Debug.Log("Tick");
            if (_played && _track.beats[Current] != -1)
            {
                //Debug.Log(String.Format("{0} Missed", _track.beats[Current]));
                _trackView.TriggerView(Current, TrackView.Trigger.Missed);
            }
            _played = false;
            Current++;
        }

        IEnumerator WaitAndStop()
        {
            yield return waitForSeconds;
            enabled = false;
        }

        #endregion
    }
}
