using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that is never instiated but is the base class
public abstract class Agent : MonoBehaviour
{
	public Perception perception;
	public Movement movement;
}
