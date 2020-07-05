namespace Navigation
{
   public interface IPersitable {
      void Save(NavTreeWriter writer);

      void Load(NavTreeReader reader);
   }
}