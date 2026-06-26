using UnityEngine;

/// <summary>
/// this is a struct representing a vector3 it is used to represent the values of a vector 3 in a DTO
/// </summary>
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
/// <summary>
/// object dto represents a simple object position and type (as name) which can be used for both floor tiles and building objects
/// </summary>
public class ObjectDTO 
{
   /// <summary>
   /// a simple constructor that allows for easily setting up all the necessary data in the dto
   /// </summary>
   /// <param name="pos">the position of the object most importantly the x and z but you can give a vector3 for ease</param>
   /// <param name="typeName">the name of the data monobehaviour that the object is holding</param>
   public ObjectDTO(Vector3 pos, string typeName)
   {
      position = new vec3(pos.x, pos.y, pos.z);
            
      type = typeName;
   }
   
   public string type { get; set; }
   public vec3 position { get; set; }
}
