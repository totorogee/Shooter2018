using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFormation<T>  where T : Units
{
    void ChangeFormation(T Target, FormationTypes formationTypes);
    void OnKill();
     
}
