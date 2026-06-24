using UnityEngine;

public struct vec3
{
   
   public vec3(float x, float y, float z)
   {
      this.x = x;
       this.y = y;
       this.z = z;
   }
   public float x;
   public float y;
   public float z;
}
public class ObjectDTO 
{
   public ObjectDTO(Vector3 pos, string typeName)
   {
      position = new vec3(pos.x, pos.y, pos.z);
            
      type = typeName;
   }
   
   public string type { get; set; }
   public vec3 position { get; set; }
}
