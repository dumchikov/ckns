using System.ComponentModel.DataAnnotations;

namespace Chicken.Domain.Models
{
    public class Entity
    {
        [Key]
        public int Id { get; protected set; }
    }
}
