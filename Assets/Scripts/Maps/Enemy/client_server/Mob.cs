using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Photon.MonoBehaviour
{

    public struct NetworkState
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public double Timestamp;

        public NetworkState(Vector3 pos, Quaternion rotatinon, double timestamp)
        {
            Position = pos;
            Rotation = rotatinon;
            Timestamp = timestamp;
        }
    }
    private NetworkState[] _stateBuffer = new NetworkState[20];
    private int _stateCount;
    public float InterpolationBackTime = 0.1f;

    private bool player_visible;
    public float speed;
    private Transform _target;
    public SettingsEnimy setEn;
    private int step = 0;
    private Vector3 dTarget;
    private Game game;
    private bool isFire = false;

    void Start()
    {
        if (Boot.PVP)
            game = GameObject.Find("Boot").GetComponent<Game>();
    }
    private void ReceiveState(NetworkState state)
    {
        for (int i = _stateBuffer.Length - 1; i > 0; i--)
        {
            _stateBuffer[i] = _stateBuffer[i - 1];
        }
        _stateBuffer[0] = state;
        _stateCount = Mathf.Min(_stateCount + 1, _stateBuffer.Length);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            var pos = (Vector3)stream.ReceiveNext();
            var rot = (Quaternion)stream.ReceiveNext();
            ReceiveState(new NetworkState(pos, rot, info.timestamp));
        }
    }

    void Update()
    {

        if (photonView == null /*|| photonView.isMine*/)
            return;

        if (PhotonNetwork.isMasterClient)
        {

            UpdateLogic();
        }

        if (_stateCount == 0)
            return;


        var currentTime = PhotonNetwork.time;
        var interpolationTime = currentTime - InterpolationBackTime;

        if (_stateBuffer[0].Timestamp > interpolationTime)
        {
            for (int i = 0; i < _stateCount; i++)
            {
                if (_stateBuffer[i].Timestamp <= interpolationTime || i == _stateCount - 1)
                {
                    //the state closest to network time
                    var lhs = _stateBuffer[i];

                    //the state one slot newer
                    var rhs = _stateBuffer[Mathf.Max(i - 1, 0)];

                    //use time between lhs and rhs to interpolate
                    var length = rhs.Timestamp - lhs.Timestamp;

                    var t = 0f;
                    if (length > 0.0001)
                    {
                        t = (float)((interpolationTime - lhs.Timestamp) / length);
                    }
                    if (Vector3.Distance(lhs.Position, rhs.Position) > 5f)
                        transform.position = rhs.Position;
                    else
                        transform.position = Vector3.Lerp(lhs.Position, rhs.Position, t);

                    transform.rotation = Quaternion.Lerp(lhs.Rotation, rhs.Rotation, t);
                    break;
                }
            }
        }
        else
        {
            //Logger.(no timestamp)
            var lhs = _stateBuffer[0];

            if (Vector3.Distance(lhs.Position, lhs.Position) > 2f)
                transform.position = lhs.Position;
            else
                transform.position = Vector3.Lerp(lhs.Position, lhs.Position, 0.1f);

            transform.rotation = Quaternion.Lerp(lhs.Rotation, lhs.Rotation, 0.1f);
        }
    }

    private void UpdateLogic()
    {
        
        if (Game.Instance._step == 1)
        {
            if (step > 0)
            {
                step = 0;
            }
            if (setEn.od < setEn.maxOD)
                setEn.od = setEn.maxOD;

            return;
        }
        if (isFire && !setEn.view.GetComponent<Animator>().GetBool("fire") && step == 2)
        {
            isFire = false;
            step = 0;
        }
        if (setEn.od <= 0)
            return;


        FindTarget();
        if (step == 1)
            UpdateWalk();
        if (step == 2)
            UpdateAttack();
    }

    private void UpdateAttack()
    {
        if (_target.tag == "Player" || _target.tag == "Player_Friend")
        {
            if (_target.GetComponent<LifeComponent>().life > 0 && !isFire)
            {
                isFire = true;
                setEn.od -= setEn.damageOd;
                setEn.view.GetComponent<Animator>().SetBool("fire", true);
                // game.playerAttack(objectHit.GetComponent<Setting>().photonID, _ws.losMin, _ws.losMax, pr);
                //    game.playerAttack(objectHit.GetComponent<Setting>().photonID, _ws.losMin, _ws.losMax, pr);
                int pID = _target.GetComponent<Setting>().photonID;
                game.botPlayerAttack(pID, setEn.damage);
                //photonView.RPC("MobAttackPlaerID", PhotonTargets.MasterClient, pID, setEn.damage);
            }
        }
        if (_target.tag == "Tower")
        {
            Debug.Log("Fire Tower");
            if (_target.GetComponent<LifeComponent>().life > 0 && !isFire)
            {
                isFire = true;
                setEn.od -= setEn.damageOd;
                _target.GetComponent<Tower>().onDmg(setEn.damage);

            }
        }
    }

    private void FindTarget()
    {
        if (Game.Instance._step == 1)
        {
            if (step > 0)
            {
                step = 0;
            }
            return;
        }
        if (step > 0)
            return;
        if (setEn.od <= 0)
            return;
        var tower = Game.Instance.Tower.transform;
        var players = FindObjectsOfType<NetworkPlayer>();
        var minDist = float.MaxValue;

        Vector3 pos = Vector3.zero;
        foreach (var player in players)
        {
            var dist = Vector3.Distance(transform.position, player.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                _target = player.transform;
                pos = new Vector3(player.transform.position.x, 0f, player.transform.position.z);

            }
        }
        Vector3 vT = new Vector3(tower.position.x, 0f, tower.position.z);
        var dist2 = Vector3.Distance(transform.position, vT);
        if (dist2 < minDist)
        {
            minDist = dist2;
            _target = tower;
            pos = vT;

        }

        if (minDist > 1.5f)
        {
            List<Node> arr = GameObject.Find("Grid").GetComponent<Pathfinding>().FindPath(transform.position, pos);
            if (arr.Count > 0)
            {
                setEn.od--;
                dTarget = arr[0].worldPosition;
                step = 1;
            }
        }
        else
        {
            transform.LookAt(_target.position);
            if (setEn.od >= setEn.damageOd)
            {
                step = 2;
                Debug.Log("step = 2");
            }
            else
            {

                Debug.Log("end step");
            }

        }
    }

    public void onDmg(int dmg)
    {
        int id = GetComponent<PhotonView>().viewID;
        game.onDmgBot(id, dmg);
        /*LifeComponent life = GetComponent<LifeComponent>();
        if (life.life <= 0)
            return;
        if (life.life > 0)
        {
            life.life -= dmg;
        }
        if (life.life < 0)
        {
            dellBot();
        }*/
    }
    /*public void onDmgAll(int life)
    {
        LifeComponent life = GetComponent<LifeComponent>();
        life.life -= life;
    }*/
    private void UpdateWalk()
    {

        Vector3 p1 = new Vector3(dTarget.x, 0, dTarget.z);
        Vector3 p2 = new Vector3(transform.position.x, 0, transform.position.z);
        float dist = (float)Vector3.Distance(p1, p2);
     //   Debug.Log("UpdateWalk " + dist);
        if ((p1 == p2 || dist < 0.3f))
        {
            setEn.view.GetComponent<Animator>().SetBool("walk", false);
            step = 0;
        }
        else
        {
            if (!setEn.view.GetComponent<Animator>().GetBool("walk"))
                setEn.view.GetComponent<Animator>().SetBool("walk", true);

            transform.LookAt(new Vector3(dTarget.x, 0, dTarget.z));
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(dTarget.x, 0, dTarget.z), Time.deltaTime * 2);
            Debug.Log("UpdateWalk walk true");
        }


    }

    public void dellBot()
    {
        setEn.view.GetComponent<Animator>().SetBool("dead", true);
        setEn.view.tag = "Box";
        Mob mob = setEn.view.GetComponent<Mob>();
        mob.enabled = false;

        Mob _mob = setEn.view.GetComponent<Mob>();
        _mob.enabled = false;

        LifeComponent _lifec = setEn.view.GetComponent<LifeComponent>();
        _lifec.enabled = false;

        ZombiComponent _zc = setEn.view.GetComponent<ZombiComponent>();
        _zc.enabled = false;

        game.killBot();
    }
}
