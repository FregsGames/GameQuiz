using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CompaniesContainer", menuName = "ScriptableObjects/Companies")]
public class CompaniesContainer : ScriptableObject
{
    [SerializeField]
    public List<Company> allCompanies = new List<Company>();
    [SerializeField]
    public List<Involved_Company> involved_companies = new List<Involved_Company>();

    public void AddInvolvedCompany(Involved_Company involved_Company)
    {
        Involved_Company inv = involved_companies.FirstOrDefault(c => c.id == involved_Company.id);

        if (inv == null)
        {
            involved_companies.Add(involved_Company);
        }
        else
        {
            if(inv.company == 0 && involved_Company.company != 0)
            {
                inv.company = involved_Company.company;
                inv.developer = involved_Company.developer;
            }
        }
    }

    public string GetName(int id)
    {
        string name = "";
        Company company = allCompanies.FirstOrDefault(c => c.id == id);

        if(company != null)
        {
            name = company.name;
        }

        return name;
    }

    public void AddCompany(Company company)
    {
        Company cmp = allCompanies.FirstOrDefault(c => c.id == company.id);

        if (cmp == null)
        {
            allCompanies.Add(company);
        }
        else
        {
            ManageDeveloped(company, cmp);
            if(cmp.name == string.Empty && company.name != string.Empty)
            {
                cmp.name = company.name;
            }
        }
    }

    private static void ManageDeveloped(Company company, Company cmp)
    {
        if (cmp.developed == null || cmp.developed.Length == 0)
        {
            cmp.developed = company.developed;
        }
        else
        {
            AddNewDeveloped(company, cmp);
        }
    }

    private static void AddNewDeveloped(Company company, Company cmp)
    {
        List<int> originalDeveloped = cmp.developed.ToList();
        List<int> newDeveloped = company.developed.ToList();

        originalDeveloped.AddRange(newDeveloped);

        originalDeveloped = originalDeveloped.GroupBy(x => x).Select(y => y.First()).ToList();

        cmp.developed = originalDeveloped.ToArray();
    }

    public void AddGame(Game game)
    {
        int[] gameInvolved = game.involved_companies;
        foreach (var inv in gameInvolved)
        {
            if (InvolvedExists(inv))
            {
                var involved = involved_companies.FirstOrDefault(i => i.id == inv);
                if (CompanyExists(involved.company))
                {
                    Company cmp = allCompanies.FirstOrDefault(c => c.id == involved.company);
                    /*if(cmp.games.FirstOrDefault(g => g.id == game.id) == null)
                    {
                        cmp.games.Add(game);
                    }*/
                }
                else
                {
                    Company company = new Company() { id = involved.company, developed = new int[] {game.id } };
                    allCompanies.Add(company);
                }

            }
            else
            {
                Involved_Company involved_Company = new Involved_Company() {id = inv };
                involved_companies.Add(involved_Company);
            }
        }
    }

    private bool InvolvedExists(int id)
    {
        Involved_Company inv = involved_companies.FirstOrDefault(c => c.id == id);
        return inv != null;
    }

    private bool CompanyExists(int id)
    {
        Company cmp = allCompanies.FirstOrDefault(c => c.id == id);
        return cmp != null;
    }

    public List<CompanyTuple> GetCompanies(List<InvolvedTuple> involved)
    {
        List<CompanyTuple> companies = new List<CompanyTuple>();

        foreach (var item in involved)
        {
            var inv = involved_companies.FirstOrDefault(i => i.id == item.involved);
            if (inv != null)
            {
                var comp = allCompanies.FirstOrDefault(c => c.id == inv.company);
                if(comp != null)
                {
                    var result = companies.FirstOrDefault(r => r.company.id == comp.id);
                    if (result == null)
                    {
                        companies.Add(new CompanyTuple(new Company() { name = comp.name, id = comp.id }, 1));
                    }
                    else
                    {
                        var index = companies.IndexOf(result);
                        companies[index] = new CompanyTuple(companies[index].company, companies[index].counter + 1);
                    }
                }
            }
        }

        return companies;
    }
}
