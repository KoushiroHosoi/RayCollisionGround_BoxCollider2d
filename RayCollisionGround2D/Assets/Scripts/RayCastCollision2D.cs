using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //Ray��p�����ڒn����p�̃N���X
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class RayCastCollision2D : MonoBehaviour
    {
        //�ڒn����p��Ray��Collider�̋���(���������Ȃ��Ɣ��肳��Ȃ��̂Œ���(0.2���炢�����傤�ǂ����ł�))
        [SerializeField] private float distance;

        //�ڒn���肵����Layer
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask wllLayer;

        //�ڒn���Ă��邩�ǂ���
        [SerializeField] private bool isGround;
        [SerializeField] private bool isWall;

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private BoxCollider2D boxCollider;

        //BoxCollider��4�̒��_���W
        Vector2[] vertices;

        void Start()
        {
            isGround = false;
            isWall = false;
        }

        void Update()
        {
            //BoxCollider�̉E���E��ˉ��̍��W���i�[
            Vector2 lowerRightVertex = GetBoxCollide2DVertices(3);
            Vector2 lowerLeftVertex = GetBoxCollide2DVertices(2);


            Debug.DrawLine(lowerRightVertex - new Vector2(-1f * distance * Mathf.Sin(this.transform.localEulerAngles.z * Mathf.Deg2Rad), distance * Mathf.Cos(this.transform.localEulerAngles.z * Mathf.Deg2Rad)),
                           lowerLeftVertex - new Vector2(-1f * distance * Mathf.Sin(this.transform.localEulerAngles.z * Mathf.Deg2Rad), distance * Mathf.Cos(this.transform.localEulerAngles.z * Mathf.Deg2Rad))
                           , Color.red);

            //Ground��Wall�Ƃ̐ڐG������s��
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

        //Ray�𔭎˂��Ďw�肳�ꂽLayer�Ƃ̓����蔻��������Ȃ��B
        private bool OnCollisionStayLayer(Vector2 startPos, Vector2 endPos, LayerMask layer)
        {
            RaycastHit2D raycastHit2D = Physics2D.Linecast(startPos - new Vector2(-1f * distance * Mathf.Sin(this.transform.localEulerAngles.z * Mathf.Deg2Rad), distance * Mathf.Cos(this.transform.localEulerAngles.z * Mathf.Deg2Rad)),
                                                           endPos - new Vector2(-1f * distance * Mathf.Sin(this.transform.localEulerAngles.z * Mathf.Deg2Rad), distance * Mathf.Cos(this.transform.localEulerAngles.z * Mathf.Deg2Rad)),
                                                           layer);

            return raycastHit2D.collider != null;
        }

        //BoxCollider�̒��_���W��Ԃ�
        //���ォ�珇��0,1�c�ƂȂ��Ă܂�
        private Vector2 GetBoxCollide2DVertices(int num)
        {
            Vector2[] boxColliderPosition = CalculateBoxCollide2DVertices(boxCollider);
            Vector2 lowerRightPosition = boxColliderPosition[num];
            return lowerRightPosition;
        }

        //BoxCollider�̊e���_���Z�o����
        private Vector2[] CalculateBoxCollide2DVertices(BoxCollider2D collider)
        {
            //�I�u�W�F�N�g�̈ʒu�ƃX�P�[�����擾
            Transform colliderTransform = collider.transform;
            Vector2 colliderLossyScale = colliderTransform.lossyScale;

            //BoxCollider��Size��World��ɒ��� (ex Scale�F2 colliderSize�F3 �Ȃ�6�ɒ���)
            float boxColliderXSize = collider.size.x * colliderLossyScale.x;
            float boxColliderYSize = collider.size.y * colliderLossyScale.y;

            colliderLossyScale = new Vector2(boxColliderXSize, boxColliderYSize);

            colliderLossyScale *= 0.5f;

            //Collider�̒��S���W(World�)���i�[
            //Vector2 cp = colliderTransform.TransformPoint(collider.center);
            Vector2 colliderWorldTransform = colliderTransform.TransformPoint(collider.offset);

            Vector2 vx = colliderTransform.right * colliderLossyScale.x;
            Vector2 vy = colliderTransform.up * colliderLossyScale.y;

            Vector2 p1 = -vx + vy;
            Vector2 p2 = vx + vy;
            Vector2 p3 = vx + -vy;
            Vector2 p4 = -vx + -vy;

            vertices = new Vector2[4];

            //����A�E��A�E���A�����̏��ɍ��W���i�[
            //���ꂼ��̓_�̍��W��World�̊�ɍ��킹��
            vertices[0] = colliderWorldTransform + p1;
            vertices[1] = colliderWorldTransform + p2;
            vertices[2] = colliderWorldTransform + p3;
            vertices[3] = colliderWorldTransform + p4;

            return vertices;
        }
    }
}
