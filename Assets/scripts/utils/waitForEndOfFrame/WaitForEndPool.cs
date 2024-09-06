using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForEndPool
{
    private static WaitForEndOfFrame _waitForEndOfFrame;
    private static WaitForFixedUpdate _waitForFixedUpdate;
    private static Dictionary<float, WaitForSeconds> _waitForSecondsCache = new Dictionary<float, WaitForSeconds>();

    /// <summary>
    /// Returns a cached instance of WaitForEndOfFrame.
    /// </summary>
    public static WaitForEndOfFrame WaitForEndOfFrame()
    {
        if (_waitForEndOfFrame == null)
        {
            _waitForEndOfFrame = new WaitForEndOfFrame();
        }
        return _waitForEndOfFrame;
    }

    /// <summary>
    /// Returns a cached instance of WaitForFixedUpdate.
    /// </summary>
    public static WaitForFixedUpdate WaitForFixedUpdate()
    {
        if (_waitForFixedUpdate == null)
        {
            _waitForFixedUpdate = new WaitForFixedUpdate();
        }
        return _waitForFixedUpdate;
    }

    /// <summary>
    /// Returns a cached instance of WaitForSeconds for the given duration.
    /// </summary>
    /// <param name="seconds">Time to wait in seconds</param>
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!_waitForSecondsCache.ContainsKey(seconds))
        {
            _waitForSecondsCache[seconds] = new WaitForSeconds(seconds);
        }
        return _waitForSecondsCache[seconds];
    }

    /// <summary>
    /// Clears the cached WaitForSeconds instances.
    /// </summary>
    public static void ClearWaitForSecondsCache()
    {
        _waitForSecondsCache.Clear();
    }
}
