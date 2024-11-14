using System.Collections;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
  [SerializeField] private MonoBehaviour[] _scriptsToEcecute;

  //------------------------------------

  private IBootstrap[] bootstraps;

  //====================================

  private void Awake()
  {
	bootstraps = new IBootstrap[_scriptsToEcecute.Length];

	for (int i = 0; i < _scriptsToEcecute.Length; i++)
	{
	  _scriptsToEcecute[i].enabled = false;
	  bootstraps[i] = (IBootstrap)_scriptsToEcecute[i];
    }

	foreach (var bootstrap in bootstraps)
      bootstrap.CustomAwake();

    for (int i = 0; i < _scriptsToEcecute.Length; i++)
      _scriptsToEcecute[i].enabled = true;
  }

  private IEnumerator Start()
  {
	foreach (var bootstrap in bootstraps)
	{
	  bootstrap.CustomStart();
	  yield return null;
	}
  }

  //====================================
}