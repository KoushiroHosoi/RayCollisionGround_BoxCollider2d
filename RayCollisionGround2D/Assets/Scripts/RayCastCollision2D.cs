using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //Rayを用いた接地判定用のクラス
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class RayCastCollision2D : MonoBehaviour
    {
        //接地判定用のRayとColliderの距離(少し離さないと判定されないので注意(0.2くらいがちょうどいいです))
        [SerializeField] private float distance;

        //接地判定したいLayer
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask wllLayer;

        //接地しているかどうか
        [SerializeField] private bool isGround;
        [SerializeField] private bool isWall;

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private BoxCollider2D boxCollider;

        //BoxColliderの4つの頂点座標
        Vector2[] vertices;

        void Start()
        {
            isGround = false;
            isWall = false;
        }

        void Update()
        {
            //BoxColliderの右下・飛騨下の座標を格納
            Vector2 lowerRightVertex = GetBoxCollide2DVertices(3);
            Vector2 lowerLeftVertex = GetBoxCollide2DVertices(2);


            Debug.DrawLine(lowerRightVertex - new Vector2(-1f * distance * Mathf.Sin(this.transform.localEulerAngles.z * Mathf.Deg2Rad), distance * Mathf.Cos(this.transform.localEulerAngles.z * Mathf.Deg2Rad)),
                           lowerLeftVertex - new Vector2(-1f * distance * Mathf.Sin(this.transform.localEulerAngles.z * Mathf.Deg2Rad), distance * Mathf.Cos(this.transform.localEulerAngles.z * Mathf.Deg2Rad))
                           , Color.red);

            //GroundとWallとの接触判定を行う
            if (OnCollisionStayLayer(lowerRightVertex, lowerLeftVertex, groundLayer))
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }

            if (OnCollisionStayLayer(lowerRightVertex, lowerLeftVertex, wllLayer))
            {
                isWall = true;
            }
            else
            {
                isWall = false;
            }
        }

        //Rayを発射して指定されたLayerとの当たり判定をおこなう。
        private bool OnCollisionStayLayer(Vector2 startPos, Vector2 endPos, LayerMask layer)
        {
            RaycastHit2D raycastHit2D = Physics2D.Linecast(startPos - new Vector2(-1f * distance * Mathf.Sin(this.transform.localEulerAngles.z * Mathf.Deg2Rad), distance * Mathf.Cos(this.transform.localEulerAngles.z * Mathf.Deg2Rad)),
                                                           endPos - new Vector2(-1f * distance * Mathf.Sin(this.transform.localEulerAngles.z * Mathf.Deg2Rad), distance * Mathf.Cos(this.transform.localEulerAngles.z * Mathf.Deg2Rad)),
                                                           layer);

            return raycastHit2D.collider != null;
        }

        //BoxColliderの頂点座標を返す
        //左上から順に0,1…となってます
        private Vector2 GetBoxCollide2DVertices(int num)
        {
            Vector2[] boxColliderPosition = CalculateBoxCollide2DVertices(boxCollider);
            Vector2 lowerRightPosition = boxColliderPosition[num];
            return lowerRightPosition;
        }

        //BoxColliderの各頂点を算出する
        private Vector2[] CalculateBoxCollide2DVertices(BoxCollider2D collider)
        {
            //オブジェクトの位置とスケールを取得
            Transform colliderTransform = collider.transform;
            Vector2 colliderLossyScale = colliderTransform.lossyScale;

            //BoxColliderのSizeをWorld基準に直す (ex Scale：2 colliderSize：3 なら6に直す)
            float boxColliderXSize = collider.size.x * colliderLossyScale.x;
            float boxColliderYSize = collider.size.y * colliderLossyScale.y;

            colliderLossyScale = new Vector2(boxColliderXSize, boxColliderYSize);

            colliderLossyScale *= 0.5f;

            //Colliderの中心座標(World基準)を格納
            //Vector2 cp = colliderTransform.TransformPoint(collider.center);
            Vector2 colliderWorldTransform = colliderTransform.TransformPoint(collider.offset);

            Vector2 vx = colliderTransform.right * colliderLossyScale.x;
            Vector2 vy = colliderTransform.up * colliderLossyScale.y;

            Vector2 p1 = -vx + vy;
            Vector2 p2 = vx + vy;
            Vector2 p3 = vx + -vy;
            Vector2 p4 = -vx + -vy;

            vertices = new Vector2[4];

            //左上、右上、右下、左下の順に座標を格納
            //それぞれの点の座標をWorldの基準に合わせる
            vertices[0] = colliderWorldTransform + p1;
            vertices[1] = colliderWorldTransform + p2;
            vertices[2] = colliderWorldTransform + p3;
            vertices[3] = colliderWorldTransform + p4;

            return vertices;
        }
    }
}
