using System.ComponentModel.DataAnnotations;

namespace ElasticSearchTryout.Models;

public class DbSkillModel
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
}