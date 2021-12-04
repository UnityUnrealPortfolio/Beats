using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beats
{
    [CreateAssetMenu(menuName ="Beats/New Track",fileName ="New Beats Track.asset")]
    public class Track : ScriptableObject
    {
        [Header("Player Settings.")]

        [Tooltip("# of beats pa minute")]
        [Range(30, 360)]
        public int bpm;
        [HideInInspector]public List<int> beats;

        public static int inputs = 4;

        [Header("Random Settings.")]

        [Tooltip("# of preroll(empty) beats")]
        [Range(0,10)]
        [SerializeField] private int _preroll = 10;

        [Tooltip("Minimum # of beats pa Block")]
        [Range(1, 20)]
        [SerializeField] private int _minBlock = 2;

        [Tooltip("Maximum Number of beats pa block")]
        [Range(1, 20)]
        [SerializeField] private int _maxBlock = 5;

        [Tooltip("Minimum # of empty beats between blocks.")]
        [Range(1, 20)]
        [SerializeField] private int _minInterval = 1;

        [Tooltip("Maximum # of empty beats between blocks.")]
        [Range(1, 20)]
        [SerializeField] private int _maxInterval = 2;

        [Tooltip("Number of beats blocks")]
        [SerializeField] private int _blocks = 10;


        public void Randomize()
        {
            beats = new List<int>();

            //set up the preroll(empty) beats
            for(int b = 0; b < _preroll; b++)
            {
                beats.Add(-1);
            }

            //set up blocks of beats
            for(int blk = 0;blk < _blocks; blk++)
            {
                int blockLength = Random.Range(_minBlock, _maxBlock+1);
                for(int b = 0; b < blockLength; b++)
                {
                    int beat = Random.Range(0, inputs);
                    beats.Add(beat);
                }

                //break out if we're at the last beat so as not to end on an empty beat
                if(blk == _blocks - 1)
                {
                    break;
                }

                int intervalLength = Random.Range(_minInterval, _maxInterval + 1);
                for (int b = 0; b < intervalLength; b++)
                {
                    int beat = Random.Range(0, inputs);
                    beats.Add(-1);
                }
            }

            
        }


    } 
}
