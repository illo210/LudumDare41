﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : BaseEntity {
    void FixedUpdate () {
        float move = Input.GetAxis("Horizontal");

        Move(move);
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
	}
}
