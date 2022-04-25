using Let_sTalk.Models;
using LetsTalkBackend.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Let_sTalk.Data.IRepos
{
    public interface IMatchRepository
    {
        Match create(Match match);
        Match getMatch(int idUser1, int idUser2);
        void updateIfMatch(int idUser1, int idUser2);
        void deleteIfUnmatch(int idUser1, int idUser2);
        HashSet<string> getMatchesByUserId(int idUser);
    }
}
