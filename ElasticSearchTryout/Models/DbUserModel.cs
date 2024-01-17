using System.ComponentModel.DataAnnotations;
using Nest;

namespace ElasticSearchTryout.Models;

public class DbUserModel
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }

    public List<int> SkillIds { get; set; } = [];
    public List<string> Languages { get; set; } = [];
    
    public GeoLocation Location { get; set; }
}