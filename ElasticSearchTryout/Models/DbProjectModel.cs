using System.ComponentModel.DataAnnotations;

namespace ElasticSearchTryout.Models;

public class DbProjectModel
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<int> Members { get; set; }
    
    public bool IsPublic { get; set; }
}