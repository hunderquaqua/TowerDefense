using UnityEngine;

public class SoundManager : Loader <SoundManager>
{
    [SerializeField]
    AudioClip arrow;
    [SerializeField]
    AudioClip rock;
    [SerializeField]
    AudioClip fireBall;
    [SerializeField]
    AudioClip newGame;
    [SerializeField]
    AudioClip level;
    [SerializeField]
    AudioClip towerBuilt;
    [SerializeField]
    AudioClip death;
    [SerializeField]
    AudioClip hit;
    [SerializeField]
    AudioClip gameOver;

    public AudioClip Arrow
    {
        get
        {
            return arrow;
        }
    }
    public AudioClip Rock
    {
        get
        {
            return rock;
        }
    }
    public AudioClip FireBall
    {
        get
        {
            return fireBall;
        }
    }
    public AudioClip NewGame
    {
        get
        {
            return newGame;
        }
    }
    public AudioClip Level
    {
        get
        {
            return level;
        }
    }
    public AudioClip TowerBuilt
    {
        get
        {
            return towerBuilt;
        }
    }
    public AudioClip Death
    {
        get
        {
            return death;
        }
    }
    public AudioClip Hit
    {
        get
        {
            return hit;
        }
    }
    public AudioClip GameOver
    {
        get
        {
            return gameOver;
        }
    }
}
