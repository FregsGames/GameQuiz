using System.Collections.Generic;

public class CompaniesDB : Singleton<CompaniesDB>
{
    public Dictionary<int, Company> allCompanies = new Dictionary<int, Company>();
    public Dictionary<int, Involved_Company> involved_companies = new Dictionary<int, Involved_Company>();

    public void AddInvolvedCompany(Involved_Company involved_Company)
    {
        if (!involved_companies.ContainsKey(involved_Company.id))
        {
            involved_companies.Add(involved_Company.id, involved_Company);
        }
    }

    public void AddCompany(Company company)
    {
        if (!allCompanies.ContainsKey(company.id))
        {
            allCompanies.Add(company.id, company);
        }
    }




    /*public void AddGameToCompany(Game game)
    {
        if (game.involved_companies == null)
            return;

        foreach (var involvedCompany in game.involved_companies)
        {
            if (!involved_companies.ContainsKey(involvedCompany)){
                involved_companies.Add(involvedCompany, new Company() { id = involvedCompany });
            }

            involved_companies[involvedCompany].games.Add(game);
        }
    }*/
}
