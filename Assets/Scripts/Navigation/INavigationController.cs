using UnityEngine;
using System.Threading.Tasks;

namespace Navigation
{
   public interface INavigationController {
      Route GetRoute(Vector3 start, Vector3 end, float minSize);

      Route GetRoute(INavigable start, INavigable end, float minSize);

      Task<Route> GetRouteAsync(Vector3 start, Vector3 end, float minSize);

      Task<Route> GetRouteAsync(INavigable start, INavigable end, float minSize);
   }
}