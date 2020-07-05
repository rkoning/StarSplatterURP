using System.IO;
using UnityEngine;

namespace Navigation {
   public class NavTreeReader {
      private BinaryReader reader;

      public NavTreeReader(BinaryReader reader) {
         this.reader = reader;
      }
      
      public float ReadFloat() {
         return reader.ReadSingle();
      }

      public int ReadInt() {
         return reader.ReadInt32();
      }

      public bool ReadBool() {
         return reader.ReadBoolean();
      }

      public Quaternion ReadQuaternion() {
         Quaternion value;
         value.x = reader.ReadSingle();
         value.y = reader.ReadSingle();
         value.z = reader.ReadSingle();
         value.w = reader.ReadSingle();
         return value;
      }

      public Vector3 ReadVector3() {
         Vector3 value;
         value.x = reader.ReadSingle();
         value.y = reader.ReadSingle();
         value.z = reader.ReadSingle();
         return value;
      }
   }
}