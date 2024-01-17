using Bogus;
using ElasticSearchTryout.Models;
using ElasticSearchTryout.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;

namespace ElasticSearchTryout.Controllers;

[Route("[controller]")]
public class PostController(ElasticService elasticService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Init()
    {
        var faker = new Faker();
        
        var availableSkillIds = new List<int> {1, 2, 3, 4, 5};
        var availableLanguageIds = new List<string> {"de", "en", "fr", "it", "es"};
        
        var user = new DbUserModel
        {
            Id = faker.Random.Int(0),
            Name = faker.Person.FullName,
            SkillIds = faker.Random.ListItems(availableSkillIds, 3).ToList(),
            Languages = faker.Random.ListItems(availableLanguageIds, 2).ToList(),
            Location = new GeoLocation(faker.Address.Latitude(), faker.Address.Longitude())
        };

        var result = await elasticService.InsertUser(user);
        
        return Ok(JsonConvert.SerializeObject(result));
    }
    
    [HttpPost("truncate")]
    public async Task<IActionResult> DeleteAllDocuments()
    {
        var response = await elasticService.DeleteAllDocuments(new DeleteByQueryRequest<DbUserModel>(Nest.Indices.Index<DbUserModel>())
        {
            Query = new QueryContainer(new MatchAllQuery())
        }
        );

        if (response.IsValid)
        {
            return Ok("Alle Dokumente wurden erfolgreich gelöscht.");
        }
        else
        {
            return BadRequest($"Fehler beim Löschen der Dokumente: {response.DebugInformation}");
        }
    }
    
}