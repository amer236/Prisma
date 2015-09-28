﻿using UnityEngine;
using System.Collections;
using System;

// Enforces these modules to be loaded up with this module when placed on a prefab/game object
[RequireComponent(typeof(EntityMovement))]


public class Player : KillableEntityInterface {

    public EntityMovement entityMovement;
    public Rigidbody2D projectile;
    public float projectileSpeed = 10;
    public float xProjectileOffset = 0f;
    public float yProjectileOffset = 0f;
    public Boolean attacking = false;
    public float attackCooldown = 0.3f;
    public float lastAttack;
    public BoxCollider2D meleeCollider;

    public int strength = 1;    //Strength - Melee
    public int agility = 1;     //Agility- Speed
    public int dexterity = 1;   //Dexterity- Range
    public int intelligence = 1;//Intelligence - Special
    public int vitality = 1;    //Vitality - Health


    bool moveRight = false;
    bool moveLeft = false;
    bool isJumping = false;

    Vector3 movement;

    // Use this for initialization
    void Start () {
	    this.entityMovement = GetComponent<EntityMovement>();
        meleeCollider.enabled = false;
        attacking = false;
        lastAttack = Time.time;
    } 

    void Update () 
    {
        /*
        if (moveRight)
        {
            movement.Set(1, 0, 0);
            movement = movement.normalized * movementSpeed * Time.deltaTime;
            playerRigidBody.MovePosition(transform.position + movement);
        }
         * */
        //if pressing jump button, call jump method to toggle boolean
        if (Input.GetButtonDown("Jump"))
        {
            entityMovement.Jump();
        }

        if (isJumping)
        {
            entityMovement.Jump();
        }
        //float hVelocity = Input.GetAxis("Horizontal");
        float hVelocity = 0f;
        if (moveRight && !moveLeft)
        {
            hVelocity = 1.0f;
        }
        else if (moveLeft && !moveRight)
        {
            hVelocity = -1.0f;
        }
        if (!moveRight && !moveLeft)
        {
            hVelocity = 0.0f;
        }

        //hVelocity = Input.GetAxis("Horizontal");
        //call the base movement module method to handle movement
        entityMovement.Movement(hVelocity);

        //If the shift button is pressed
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Shoot();
        }

        //If the control button is pressed
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Melee();
        }

        if(attacking == true){
            meleeCollider.enabled = true;
            if((Time.time - lastAttack) > 0.1)
            {
                attacking = false;
                meleeCollider.enabled = false;
            }
        }
        else
        {
            meleeCollider.enabled = false;
        }

        UpdateStats();
    }

    public void UpdateStats()
    {
        this.maxHealth = vitality;
        entityMovement.maxSpeed = agility * 5.0f;
        //Strength and dexterity are called during damage calculations
    }

    public void Melee() {
        if (Time.time > (lastAttack + attackCooldown))
        {
            attacking = true;
            lastAttack = Time.time;
        }
    }

    public void Shoot () {
        Rigidbody2D clone;
        //Shoot to the right
        if (entityMovement.facingRight) {
            clone = (Rigidbody2D)Instantiate(projectile, new Vector3(transform.position.x + xProjectileOffset, transform.position.y + yProjectileOffset, transform.position.z), transform.rotation);
            //Set damage equal to dexterity stat
            clone.GetComponent<ProjectileScript>().damage = dexterity;
            //Set x speed 
            clone.velocity = new Vector2(projectileSpeed, 0);
        } else {
            //Shoot to the left
            clone = (Rigidbody2D)Instantiate(projectile, new Vector3(transform.position.x - xProjectileOffset, transform.position.y + yProjectileOffset, transform.position.z), transform.rotation);
            clone.GetComponent<ProjectileScript>().damage = dexterity;
            //Invert prefab
            Vector3 theScale = clone.transform.localScale;
            theScale.x *= -1;
            clone.transform.localScale = theScale;
            //Set x speed
            clone.velocity = new Vector2(-projectileSpeed, 0);
        }
    }

    public void rightButtonPressed()
    {
        moveRight = true;
    }

    public void rightButtonReleased()
    {
        moveRight = false;
    }

    public void leftButtonPressed()
    {
        moveLeft = true;
    }

    public void leftButtonReleased()
    {
        moveLeft = false;
    }

    public void jumpPressed()
    {
        isJumping = true;
    }

    public void jumpReleased()
    {
        isJumping = false;
    }


    public override void takeDamage(int damageReceived)
    {
        throw new NotImplementedException();
    }

    public override void die()
    {
        throw new NotImplementedException();
    }

}
