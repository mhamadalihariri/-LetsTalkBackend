using Let_sTalk.Data.Context;
using Let_sTalk.Data.IRepos;
using Let_sTalk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Let_sTalk.Data.Repos
{
    public class MatchRepository : IMatchRepository
    {
        private readonly MyDbContext _dbContext;
        
        public MatchRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Match create(Match match)
        {
            _dbContext.matches.Add(match);
            _dbContext.SaveChanges();
            return match;
        }

        public Match getMatch(int idUser1,int idUser2)
        {
            return _dbContext.matches.FirstOrDefault(m => m.User1 == idUser2 && m.User2 == idUser1);
        }

        public void updateIfMatch(int idUser1, int idUser2)
        {
            Match m = getMatch(idUser1, idUser2);
            m.IsMatched = 1;
            _dbContext.matches.Update(m);
            _dbContext.SaveChanges();
        }

        public void deleteIfUnmatch(int idUser1, int idUser2)
        {
            Match m1 = getMatch(idUser1, idUser2);
            Match m2 = getMatch(idUser2, idUser1);
            _dbContext.matches.Remove(m1);
            _dbContext.matches.Remove(m2);
            _dbContext.SaveChanges();
        }

    }
}
