using System.Collections;
using UnityEngine;

namespace yoyohan
{
    public class CoroutineBase
    {

        public IEnumerable coroutineIEnumerable = null;
        public IEnumerator coroutineIEnumerator; 

        /* 
         * Creates a coroutine whose name is random.
         */
        public CoroutineBase(IEnumerable coroutineIEnumerable, IEnumerator aCoroutine)
        {
            this.coroutineIEnumerable = coroutineIEnumerable;
            this.coroutineIEnumerator = aCoroutine;
            this.name = aCoroutine.GetType().ToString(); //"Coroutine-"+System.Guid.NewGuid().ToString();
        }
        /* 
         * Creates a coroutine 
         */
        public CoroutineBase(IEnumerable coroutineIEnumerable, IEnumerator aCoroutine, string aName)
        {
            this.coroutineIEnumerable = coroutineIEnumerable;
            this.coroutineIEnumerator = aCoroutine;
            this.name = aName;
        }

        public int index = -1;

        //Some sort of "Event"
        public CoroutineMgr.CoroutineRunCondition coroutineActionCondition = null;

        public bool loop = false;
        public string name;
        public bool isPaused = false;
        public bool isFinished = false;
        public bool isWaiting = false;

        public int pauseFrame = -1;
        public float pauseTime = -1.0f;

        public void Destroy()
        {
            CoroutineMgr.DestroyCoroutine(this);
        }

        public void Pause()
        {
            CoroutineMgr.PauseCoroutine(this);
        }

        public void Resume()
        {
            CoroutineMgr.ResumeCoroutine(this);
        }

        public void Reset()
        {
            CoroutineMgr.ResetCoroutine(this);
        }

        public bool isRunning()
        {
            return CoroutineMgr.isCoroutineRunning(this);
        }

    }



    public class CoroutineMgr : MonoBehaviour
    {
        public const int MAX_ARRAY_SIZE = 512;
        //Can handle up to 512 coroutines. Change this value for a higher number of running coroutines.
        public static CoroutineBase[] coroutines = new CoroutineBase[MAX_ARRAY_SIZE];
        public delegate bool CoroutineRunCondition();

        public delegate IEnumerator CoroutineYield();

        private static int coroutine_maxindex = 0;

        static int frame;
        static float time;


        /*Please provide an IEnumerable instead of a IEnumerator as a coroutine. This way you will be able to Reset a coroutine and Loop over it (Like InvokeRepeating).
        * Examples: IEnumerable myCoroutine(){ int i = 0; while(true) { print(i); yield return 0; } }
        * 		IEnumerable myCoroutine2(){ { print("test for loopable coroutine"); yield return 0; } }	
        */

        public static CoroutineBase StartCoroutine(IEnumerable coroutineEnumerable)
        {
            return StartCoroutine(coroutineEnumerable, null, null, false);
        }

        public static CoroutineBase StartCoroutine(IEnumerable coroutineEnumerable, string coroutineName)
        {
            return StartCoroutine(coroutineEnumerable, coroutineName, null, false);
        }

        public static CoroutineBase StartCoroutine(IEnumerable coroutineEnumerable, CoroutineRunCondition method)
        {
            return StartCoroutine(coroutineEnumerable, null, method, false);
        }

        public static CoroutineBase StartCoroutine(IEnumerable coroutineEnumerable, string coroutineName, CoroutineRunCondition method)
        {
            return StartCoroutine(coroutineEnumerable, coroutineName, method, false);
        }

        public static CoroutineBase StartCoroutine(IEnumerable coroutineEnumerable, string coroutineName, CoroutineRunCondition method, bool loop)
        {
            StartCoroutineMgr();

            if (coroutineEnumerable == null)
            {
                throw new System.ArgumentException("This coroutine is null");
            }
            CoroutineBase coroutine;

            if (coroutineName == null || coroutineName == "")
            {
                coroutine = new CoroutineBase(coroutineEnumerable, coroutineEnumerable.GetEnumerator());
            }
            else
            {
                coroutine = new CoroutineBase(coroutineEnumerable, coroutineEnumerable.GetEnumerator(), coroutineName);
            }
            coroutine.coroutineActionCondition = method;
            coroutine.loop = loop;

            PushCoroutineIntoManager(coroutine);
            return coroutine;
        }



        public static new CoroutineBase StartCoroutine(IEnumerator coroutineYield)
        {
            return StartCoroutine(coroutineYield, null, null);
        }

        public static CoroutineBase StartCoroutine(IEnumerator coroutineYield, string coroutineName)
        {
            return StartCoroutine(coroutineYield, coroutineName, null);
        }

        public static CoroutineBase StartCoroutine(IEnumerator coroutineYield, CoroutineRunCondition method)
        {
            return StartCoroutine(coroutineYield, null, method);
        }

        public static CoroutineBase StartCoroutine(IEnumerator coroutineYield, string coroutineName, CoroutineRunCondition method)
        {
            StartCoroutineMgr();

            if (coroutineYield == null)
            {
                throw new System.ArgumentException("This coroutine is null");
            }
            CoroutineBase coroutine;
            if (coroutineName == null || coroutineName == "")
            {
                coroutine = new CoroutineBase(null, coroutineYield);
            }
            else
            {
                coroutine = new CoroutineBase(null, coroutineYield, coroutineName);
            }
            coroutine.coroutineActionCondition = method;

            PushCoroutineIntoManager(coroutine);
            return coroutine;
        }






        //Will stop all coroutines that have this name
        public static void DestroyCoroutine(string name)
        {
            for (int i = 0; i <= coroutine_maxindex; i++)
            {
                CoroutineBase coroutine = coroutines[i];
                if (coroutine.name == name)
                {
                    RemoveCoroutineFromManager(coroutine);
                }
            }
        }
        //Will stop this coroutine
        public static void DestroyCoroutine(CoroutineBase coroutine)
        {
            if (coroutine == null)
            {
                throw new System.ArgumentException("This coroutine is null");
            }
            RemoveCoroutineFromManager(coroutine);

        }

        //Will pause the coroutine
        public static void PauseCoroutine(CoroutineBase coroutine)
        {
            SetPause(coroutine, true);
        }

        //Will pause all coroutines that have this name
        public static void PauseCoroutine(string name)
        {
            SetPause(name, true);
        }
        //Will pause all coroutines
        public static void PauseAllCoroutines()
        {
            for (int i = 0; i <= coroutine_maxindex; i++)
            {
                CoroutineBase coroutine = coroutines[i];
                if (coroutine != null)
                {
                    SetPause(coroutine, true);
                }
            }
        }

        //Will resume all coroutines that have this name
        public static void ResumeCoroutine(CoroutineBase coroutine)
        {
            SetPause(coroutine, false);
        }

        //Will resume all coroutines that have this name
        public static void ResumeCoroutine(string name)
        {
            SetPause(name, false);
        }
        //Will resume all coroutines that were paused
        public static void ResumeAllCoroutines()
        {
            for (int i = 0; i <= coroutine_maxindex; i++)
            {
                CoroutineBase coroutine = coroutines[i];
                if (coroutine != null)
                {
                    SetPause(coroutine, false);
                }
            }
        }

        private static void ResetCoroutineInManager(CoroutineBase coroutine)
        {
            if (coroutine.coroutineIEnumerable != null)
            {
                coroutine.coroutineIEnumerator = coroutine.coroutineIEnumerable.GetEnumerator();
                //Reset some members.
                coroutine.isPaused = false;
                coroutine.isWaiting = false;
                coroutine.pauseFrame = -1;
                coroutine.pauseTime = -1.0f;
            }
            else
            {
                Debug.LogWarning("This coroutine:" + coroutine.name + " cannot be reset. In your code, make the coroutine return an 'IEnumerable' instead of a 'IEnumerator'");
            }
        }

        //Will reset the coroutine
        public static void ResetCoroutine(CoroutineBase coroutine)
        {
            if (coroutine == null)
            {
                throw new System.ArgumentException("This coroutine is null");
            }
            int i = coroutine.index;
            if (i >= 0)
            {
                ResetCoroutineInManager(coroutine);
            }
        }
        //Will reset all coroutines that have this name
        public static void ResetCoroutine(string name)
        {
            for (int i = 0; i <= coroutine_maxindex; i++)
            {
                CoroutineBase coroutine = coroutines[i];
                if (coroutine.name == name)
                {
                    ResetCoroutineInManager(coroutine);
                }
            }
        }

        //Will reset all coroutines 
        public static void ResetAllCoroutines()
        {
            for (int i = 0; i <= coroutine_maxindex; i++)
            {
                CoroutineBase coroutine = coroutines[i];
                if (coroutine != null)
                {
                    ResetCoroutineInManager(coroutine);
                }
            }
        }



        //Will return true if the coroutine is paused.
        public static bool isCoroutinePaused(CoroutineBase coroutine)
        {
            if (coroutine == null)
            {
                throw new System.ArgumentException("This coroutine is null");
            }
            int i = coroutine.index;
            if (i >= 0)
            {
                return !coroutines[i].isPaused;
            }
            else
            {
                return false; //Not even in the CoroutineMgr
            }
        }

        /*
        *Will return true if the first found coroutine that has this name is paused.
        */
        public static bool isCoroutinePaused(string name)
        {
            for (int i = 0; i <= coroutine_maxindex; i++)
            {
                CoroutineBase coroutine = coroutines[i];
                if (coroutine.name == name)
                {
                    if (coroutine.isPaused)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Will return true if the coroutine has finished.
        //You can use the isFinished public member instead
        public static bool isCoroutineFinished(CoroutineBase coroutine)
        {
            if (coroutine == null)
            {
                throw new System.ArgumentException("This coroutine is null");
            }
            int i = coroutine.index;
            if (i >= 0)
            {
                return coroutines[i].isPaused;
            }
            else
            {
                return true; //Not even in the CoroutineMgr
            }
        }

        //Will return true if the first found coroutine that has this name is finished.
        //Be careful with this. It is better to use: 
        //if (myCoroutine.isFinished){ /.../}
        public static bool isCoroutineFinished(string name)
        {
            for (int i = 0; i <= coroutine_maxindex; i++)
            {
                CoroutineBase coroutine = coroutines[i];
                if (coroutine.name == name)
                {
                    if (coroutine.isFinished)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //Will return true if the coroutine is running.
        public static bool isCoroutineRunning(CoroutineBase coroutine)
        {
            if (coroutine == null)
            {
                throw new System.ArgumentException("This coroutine is null");
            }
            int i = coroutine.index;
            if (i >= 0)
            {
                return !coroutines[i].isFinished;
            }
            else
            {
                return false; //Not even in the CoroutineMgr
            }
        }

        /*
        *Will return true if the first found coroutine that has this name is running.
        */
        public static bool isCoroutineRunning(string name)
        {
            for (int i = 0; i <= coroutine_maxindex; i++)
            {
                CoroutineBase coroutine = coroutines[i];
                if (coroutine.name == name)
                {
                    if (coroutine.isFinished)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void DestroyAllCoroutines()
        {
            for (int i = 0; i <= coroutine_maxindex; i++)
            {
                CoroutineBase coroutine = coroutines[i];
                if (coroutine != null)
                {
                    coroutine.isFinished = true;
                    coroutine.index = -1;
                    coroutines[i] = null;
                }
            }
        }

        //Main Processing
        private static void ProcessCoroutine(CoroutineBase coroutine)
        {

            IEnumerator coroutineSteps = coroutine.coroutineIEnumerator;

            if (coroutine.isWaiting == true)
            {
                if (coroutine.pauseTime > 0 && time >= coroutine.pauseTime)
                {
                    coroutine.pauseTime = -1.0f;
                    coroutine.isWaiting = false;
                }
                else
                if (coroutine.pauseFrame > 0 && frame >= coroutine.pauseFrame)
                {
                    coroutine.pauseFrame = -1;
                    coroutine.isWaiting = false;

                }
                else
                {
                    coroutine.isWaiting = true;
                }

            }

            if (coroutine.isPaused == true)
            {
                coroutine.pauseTime += Time.deltaTime;
                coroutine.pauseFrame += 1;
            }
            if (coroutine.isWaiting == false
                && coroutine.isPaused == false
                && (coroutine.coroutineActionCondition == null
                    || (coroutine.coroutineActionCondition != null && coroutine.coroutineActionCondition() == true)))
            {

                if (coroutine.coroutineIEnumerator.MoveNext())
                {
                    System.Object step = coroutineSteps.Current;
                    if (step == null) { step = (System.Object)0; }
                    if (step is UnityEngine.WaitForSeconds)
                    {
                        throw new System.ArgumentException("You should use CoroutineMgr.WaitForSeconds instead of WaitForSeconds. ");
                    }
                    else if (step is UnityEngine.WaitForFixedUpdate)
                    {
                        throw new System.ArgumentException("Sorry WaitForFixedUpdate is not (yet?) supported in CoroutineMgr ");
                    }
                    else if (step is UnityEngine.WaitForEndOfFrame)
                    {
                        throw new System.ArgumentException("Sorry WaitForEndOfFrame is not (yet?) supported in CoroutineMgr ");
                    }
                    else if (step is CoroutineMgr.WaitForSeconds)
                    {
                        coroutine.pauseTime = ((CoroutineMgr.WaitForSeconds)step).seconds;
                        coroutine.pauseTime += time;
                        coroutine.isWaiting = true;
                    }
                    else if (step.GetType() == typeof(int))
                    {
                        coroutine.pauseFrame = (int)step;
                        coroutine.pauseFrame += frame;
                        coroutine.isWaiting = true;
                    }
                    else
                    {
                        throw new System.ArgumentException("this yield expression is not supported by CoroutineMgr");
                    }
                }
                else
                {
                    if (coroutine.coroutineIEnumerable != null && coroutine.loop == true)
                    {
                        print("here");
                        coroutine.coroutineIEnumerator = coroutine.coroutineIEnumerable.GetEnumerator();
                    }
                    else
                    {
                        // coroutine finished
                        coroutine.isFinished = true;
                        RemoveCoroutineFromManager(coroutine);
                    }
                }
            }
        }

        private static void PushCoroutineIntoManager(CoroutineBase coroutine)
        {
            //Built-in Array implementation for mobiles.
            for (int i = 0; i <= MAX_ARRAY_SIZE; i++)
            {
                if (i == MAX_ARRAY_SIZE)
                {
                    throw new System.Exception("Coroutine Manager is full of coroutines. Should you increase its allowed size of " + MAX_ARRAY_SIZE);
                }
                if (coroutines[i] == null)
                {
                    coroutine.index = i;
                    coroutines[i] = coroutine;
                    if (i > coroutine_maxindex)
                    {
                        coroutine_maxindex = i;
                    }
                    break;
                }
            }
        }

        private static void RemoveCoroutineFromManager(CoroutineBase coroutine)
        {
            //Built-in Array implementation for mobiles
            int i = coroutine.index;
            if (i > -1)
            {
                coroutines[i] = null;
                coroutine.index = -1;
                if (i >= coroutine_maxindex)
                {
                    coroutine_maxindex = i - 1;
                }
            }
            else
            {
                throw new System.Exception("This coroutine has not been set into the Coroutine Manager");
            }
        }

        private static void SetPause(CoroutineBase coroutine, bool pauseValue)
        {
            if (coroutine == null)
            {
                throw new System.ArgumentException("This coroutine does not exist or no longer exists.");
            }
            int i = coroutine.index;
            if (i >= 0)
            {
                coroutines[i].isPaused = pauseValue;
            }

        }

        private static void SetPause(string name, bool pauseValue)
        {
            for (int i = 0; i <= coroutine_maxindex; i++)
            {
                CoroutineBase coroutine = coroutines[i];
                if (coroutine.name == name)
                {
                    coroutine.isPaused = pauseValue;
                }
            }

        }


        private static CoroutineMgr instance = null;

        private static void StartCoroutineMgr()
        {
            if (instance == null)
            {
                GameObject go = new GameObject("CoroutineMgr");
                instance = go.AddComponent<CoroutineMgr>();
            }
        }


        void Awake()
        {
            //TODO: Add initialization code there when needed.
            DontDestroyOnLoad(this);
        }


        void Update()
        {
            time = Time.time;
            frame = Time.frameCount;
            for (int i = 0; i <= coroutine_maxindex; i++)
            {
                CoroutineBase aCoroutine = coroutines[i];
                if (aCoroutine != null)
                {
                    ProcessCoroutine(aCoroutine);
                }
            }
        }

        public class WaitForSeconds
        {
            //Class used for future use.
            public float seconds;
            public WaitForSeconds(float seconds)
            {
                this.seconds = seconds;
            }
        }
        //TODO: Add WaitForEndOfFrame?
    }
}