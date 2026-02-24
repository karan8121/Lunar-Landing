using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    public static Lander Instance { get; private set; }
    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnLandedEventArg> OnLanded;

    public class OnLandedEventArg : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }
    public  enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
        
    }




    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount;
    private float fuelAmountMax = 10f;

    private void Awake()
    {

        Instance = this;
        fuelAmount = fuelAmountMax;
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        // Debug.Log(fuelAmount);
        if (fuelAmount <= 0f)
        {
            return;
        }
        if (Keyboard.current.upArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            ConsumeFuel();
        }


        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        if (Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);

            OnUpForce?.Invoke(this, EventArgs.Empty);
        }

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = +100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);

            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);

            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landing))
        {
            Debug.Log("CXrashed on The Terrain");
             OnLanded?.Invoke(this, new OnLandedEventArg
        {
            landingType = LandingType.WrongLandingArea,
            dotVector = 0f,
            landingSpeed = 0f,
            scoreMultiplier = 0,
            score = 0,
        });
            return;
        }
        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            Debug.Log("landed too hard");
             OnLanded?.Invoke(this, new OnLandedEventArg
        {
            landingType = LandingType.TooFastLanding,
            dotVector = 0f,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = 0,
            score = 0,
        });
            return;
        }
        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float miniDotVector = .90f;
        if (dotVector < miniDotVector)
        {
            Debug.Log("Landed on a too steep angle");
             OnLanded?.Invoke(this, new OnLandedEventArg
        {
            landingType = LandingType.TooSteepAngle,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = 0,
            score = 0,
        });
        return ;
        }
        Debug.Log("successful landing");
        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        Debug.Log("landingAngleScore:" + landingAngleScore);
        Debug.Log("landingspeedScore:" + landingSpeedScore);

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landing.GetScoreMultiplier());
        Debug.Log("score:" + score);
        OnLanded?.Invoke(this, new OnLandedEventArg
        {
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = landing.GetScoreMultiplier(),
            score = score,
        });

    }

    private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addfuelAmount = 10f;
            fuelAmount += addfuelAmount;
            if (fuelAmount > fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            fuelPickup.DestroySelf();
        }
        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }

    }
    public float Getfuel()
    {
        return fuelAmount;
    }
    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }
    public float GetSpeedX()
    {
        return landerRigidbody2D.velocity.x;
    }

    public float GetSpeedY()
    {
        return landerRigidbody2D.velocity.y;
    }

    internal class OnLandedEventArgs
    {
        internal LandingType landingType;
    }
}