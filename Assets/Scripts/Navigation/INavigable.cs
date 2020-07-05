using UnityEngine;
using System.Collections.Generic;

namespace Navigation
{
   public interface INavigable {

      Vector3 Position();
      
      bool Obstructed(float size);

      bool InBounds(Vector3 position);
            
      List<INavigable> Neighbors();

   }
}