using UnityEngine;

namespace Navigation
{
   public interface INavigableCollection {
      INavigable NodeAt(Vector3 position);   
   }
}