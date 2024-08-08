using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) => 
        await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();

    public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) => 
        await FindByCondition(c => c.Id == companyId, trackChanges)
        .SingleOrDefaultAsync();

    public void CreateCompany(Company company) => Create(company);

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges) =>
         await FindByCondition(c => companyIds.Contains(c.Id), trackChanges)
          .ToListAsync(); 

    public void DeleteCompany(Company company) => Delete(company);
}
