using ElasticSearchTryout.Models;
using Nest;
using Newtonsoft.Json;

namespace ElasticSearchTryout.Services;

public class ElasticService(IElasticClient elasticClient)
{
    public async Task<IndexResponse> InsertUser(DbUserModel user)
    {
        var result = await elasticClient.IndexDocumentAsync(user);
        return result;
    }

    public async Task<IEnumerable<DbUserModel>> GetActiveUsers(SearchDescriptor<DbUserModel> s)
    {
        var searchResponse = await elasticClient.SearchAsync<DbUserModel>(s);
        return searchResponse.Documents;
    }

    public async Task<DeleteByQueryResponse> DeleteAllDocuments(DeleteByQueryRequest request)
    {
        return await elasticClient.DeleteByQueryAsync(request);
    }
}