using System.ComponentModel.DataAnnotations;
namespace Smairo.Template.Model.Entities
{
    public class MyEntity
    {
        [Key]
        public int Id { get; set; }
        public string Col { get; set; }
    }
}